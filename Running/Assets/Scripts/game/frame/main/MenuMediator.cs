using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using System.Linq;
using System;
using System.Text;
using System.Collections.Generic;

public class MenuMediator : ActivityMediator
{
	private Menu menu;
	public SigninLogic signinLogic;
	public SignupLogic signupLogic;
	public SystemInfoLogic systemInfoLogic;
	public MigrationLogic migrationLogic;
	public CSVParser csvParser;
	public MainPage mainPage;
	public static bool isTutorial;

	private void OnEnable ()
	{
		menu = viewWithDefaultAction as Menu;
		string p_code = PlayerPrefs.GetString (LanguageJP.P_CODE, string.Empty);
		if (string.IsNullOrEmpty (p_code)) {
			menu.id.text = string.Format ("{0}{1}", LanguageJP.ID, LanguageJP.XXXXXXXXXX);
		} else {
			menu.id.text = string.Format ("{0}{1}", LanguageJP.ID, p_code);
		}
		menu.ver.text = string.Format ("{0}{1}", LanguageJP.VER, SystemConstant.CLIENT_VERSION);
	}

	protected override void InitData ()
	{
		menu = viewWithDefaultAction as Menu;
		GameConstant.hasLogin = false;
		if (isTutorial) {
			CloseTutorial ();
		} else {
			systemInfoLogic.complete = () => {
				InitNewResources ();
			};
			systemInfoLogic.error = (string status) => {
				SetEnableGame (true);
			};
			systemInfoLogic.SendAPI ();
			ComponentConstant.SOUND_MANAGER.Play (SoundEnum.bgm01_title, null, false);
		}
	}

	private void InitNewResources ()
	{
		csvParser.Parse ();
		GameConstant.hasDownloaded = true;
		StartCoroutine (ComponentConstant.ASSETBUNDLE_RESOURCES_GETTER.GetAssetBundle (AssetBundleName.sound.ToString (), GetAudioResourcesComplete, false, false));
	}

	private void GetAudioResourcesComplete (AssetBundle assetBundle)
	{
		ComponentConstant.SHOP_INITIALIZER.SetProduct ();
		SetEnableGame (true);
	}

	private void SetEnableGame (bool isEnable)
	{
		int length = menu.buttonArray.Length;	
		for (int i = 0; i < length; i++) {
			menu.buttonArray [i].interactable = isEnable;
		}
	}

	private void Login ()
	{
		ComponentConstant.DEBUG_MANAGER.Init ();
		SendAPI ();
	}

	private void SendAPI ()
	{
		if (!string.IsNullOrEmpty (SystemConstant.DeviceID)) {
			if (PlayerPrefs.GetInt (GameConstant.HasCompletedTutorial, 0) == 1) {
				Signin ();
			} else {
				ShowTutorial ();
			}
		} else {
			PlayerPrefs.SetInt (GameConstant.HasCompletedTutorial, 0);
			ShowConvention ();
		}
	}

	private void Signup ()
	{
		signupLogic.complete = () => {
			ShowTutorial ();
		};
		signupLogic.error = (string status) => {
			ClearEvent ();
			SetEnableGame (true);
		};
		signupLogic.SendAPI ();
	}

	public void CloseTutorial ()
	{
		isTutorial = false;
		ClearEvent ();
		Signin ();
	}

	private void ShowTutorial ()
	{
		isTutorial = true;
		ComponentConstant.SCENE_COMBINE_MANAGER.LoadScene (SceneEnum.Tutorial);
	}

	private void Signin ()
	{
		signinLogic.complete = () => {
			ClearEvent ();
			ComponentConstant.DEBUG_MANAGER.Init ();
			PlayerPrefs.SetString (LanguageJP.P_CODE, Player.GetInstance.p_code);
			ComponentConstant.SCENE_COMBINE_MANAGER.LoadScene (SceneEnum.Home);
			GameConstant.hasLogin = true;
		};
		signinLogic.error = (string status) => {
			ClearEvent ();
			SetEnableGame (true);
		};
		signinLogic.SendAPI ();
	}

	private void ShowConvention ()
	{
		mainPage.popupLoader.no = () => {
			ComponentConstant.SCENE_COMBINE_MANAGER.LoadScene (SceneEnum.Splash);
		};
		mainPage.popupLoader.yes = () => {
			PopupEnterUsername ();
		};
		ConventionCSVStructure conventionCSVStructure = MasterCSV.conventionCSV.FirstOrDefault ();
		menu.page.popupLoader.Popup (PopupEnum.Convention, null, new System.Collections.Generic.List<object> () {
			conventionCSVStructure.title,
			conventionCSVStructure.description
		});
	}

	private void PopupEnterUsername ()
	{
		PopupContentMediator popupContentMediator = mainPage.popupLoader.Popup (PopupEnum.EnterUserName);
		(popupContentMediator as EnterUserNameMediator).unityAction = (bool isSuccessful, string name) => {
			if (isSuccessful) {
				Signup ();
			} else {
				ComponentConstant.POPUP_LOADER.ok = () => {
					ComponentConstant.POPUP_LOADER.ok = null;
					PopupEnterUsername ();
				};
				ComponentConstant.POPUP_LOADER.Popup (PopupEnum.MessagePopup, null, new List<object> () {
					string.Empty,
					LanguageJP.NAME_ERROR
				});
			}
		};
	}

	private void ClearEvent ()
	{
		signupLogic.complete = null;
		signupLogic.error = null;
		signinLogic.complete = null;
		signinLogic.error = null;
		systemInfoLogic.complete = null;
		systemInfoLogic.error = null;
		mainPage.popupLoader.no = null;
		mainPage.popupLoader.yes = null;
		mainPage.popupLoader.ok = null;
	}

	protected override void OnDestroy ()
	{
		base.OnDestroy ();
		ClearEvent ();
	}

	protected override void CreateActions ()
	{
		unityActionArray = new UnityAction[] {
			() => {
				ComponentConstant.SOUND_MANAGER.Play (SoundEnum.se01_ok);
				SetEnableGame (false);
				Login ();
				UnityEngine.Analytics.Analytics.CustomEvent ("title_start", new Dictionary<string, object> () { {
						"title_start",
						1
					}
				});
			},
			() => {
				ComponentConstant.SOUND_MANAGER.Play (SoundEnum.se01_ok);
				popup (PopupEnum.ClearCache);
			},
			() => {
				ComponentConstant.SOUND_MANAGER.Play (SoundEnum.se01_ok);
				PopupContentMediator popupContentMediator = mainPage.popupLoader.Popup (PopupEnum.Migration);
				(popupContentMediator as MigrationMediator).unityAction = (string mail, string pw) => {
					if (!string.IsNullOrEmpty (mail) && !string.IsNullOrEmpty (pw)) {
						migrationLogic.mail = mail;
						migrationLogic.pw = Convert.ToBase64String (Encoding.Default.GetBytes (pw));
						migrationLogic.complete = () => {
							if (APIInformation.GetInstance.result_code == 0) {
								ComponentConstant.POPUP_LOADER.Popup (PopupEnum.MessagePopup, null, new List<object> () {
									LanguageJP.MIGRATION_FAIL_TITLE,
									LanguageJP.MIGRATION_FAIL_CONTENT
								});
							} else {
								ComponentConstant.POPUP_LOADER.Popup (PopupEnum.MessagePopup, null, new List<object> () {
									LanguageJP.MIGRATION_SUCCESSFUL_TITLE,
									LanguageJP.MIGRATION_SUCCESSFUL_CONTENT
								});
								ComponentConstant.POPUP_LOADER.ok = () => {
									ComponentConstant.POPUP_LOADER.ok = null;
									ComponentConstant.SCENE_COMBINE_MANAGER.LoadScene (SceneEnum.Splash);
								};
								PlayerPrefs.SetInt (GameConstant.HasCompletedTutorial, 1);
								SystemConstant.DeviceID = APIInformation.GetInstance.device_id;
								PlayerPrefs.SetString (LanguageJP.P_CODE, APIInformation.GetInstance.p_code);
							}
						};
						migrationLogic.SendAPI ();
					} else {
						ComponentConstant.POPUP_LOADER.Popup (PopupEnum.MessagePopup, null, new List<object> () {
							LanguageJP.MIGRATION_FAIL_TITLE,
							LanguageJP.MIGRATION_FAIL_CONTENT
						});
					}
				};
			},
			() => {
				ComponentConstant.SOUND_MANAGER.Play (SoundEnum.se01_ok);
				popup (PopupEnum.ResetGame);
			}
		};
	}
}
