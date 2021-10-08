using UnityEngine;
using System.Collections;
using System;
using UnityEngine.Events;
using System.Linq;
using System.Collections.Generic;
using Scripts.Home.AdsRev;

namespace home
{
	public class HomeMediator : SceneInitializer
	{
		public PageMediator[] pages;
		public Home home;
		public PopupLoader popupLoader;
		public AssetBundleResourcesLoader assetBundleResourcesLoader;
		public HeaderMediator headerMediator;
		public FooterMediator footerMediator;
		public NoticeManager noticeManager;
		public ChangeBadgeManager badgeManager;
		public GamePageMediator gamePageMediator;

		private void Awake ()
		{
			Time.timeScale = 1;
			ComponentConstant.SCREEN_MANAGER.ShowOrHideBorder (true);
			ComponentConstant.SOUND_MANAGER.Play (SoundEnum.bgm02_menu);
			ComponentConstant.SCENE_COMBINE_MANAGER.unityAction = (LongLoadingMediator longLoadingMediator) => {
				ComponentConstant.SCENE_COMBINE_MANAGER.unityAction = null;
				assetBundleResourcesLoader.unityAction = () => {
					assetBundleResourcesLoader.unityAction = null;
					headerMediator.gameObject.SetActive (true);
					footerMediator.gameObject.SetActive (true);
					headerMediator.SetCurrentHeaderImage ();
					gamePageMediator.unityAction = () => {
						InitScene ();
						gamePageMediator.ShowWindow ();
						longLoadingMediator.Hide ();
					};
					gamePageMediator.CheckRequiredResources ();
				};
				StartCoroutine (assetBundleResourcesLoader.LoadForHome ());
			};
		}

		private void InitScene ()
		{
			AddEventListeners ();
			CardRate.GetTotalForAll ();
			noticeManager.UpdateNoticeManager ();
			badgeManager.UpdateBadgeManager ();
			badgeManager.SetChangeBadge (ChangeBadgeManager.isChangeTime);

			home.ad.SetActive (PlayerPrefs.GetInt ("AdUnlock", 0) == 0);
			if (GameConstant.isPlayingGame) {
				AdsReviewProcess.Start (AdsPlayCallback, popupLoader);
			} else {
				if (!ShowLoginBonus ()) {
					ShowNotice ();
				}
			}

			GameConstant.isPlayingGame = false;
		}

		private void AdsPlayCallback ()
		{
			if (!ShowLoginBonus ()) {
				ShowNotice ();
			}
		}

		private void ShowNotice ()
		{
			if (noticeManager.GetStatus (NoticeManager.INFO)) {
				popupLoader.Popup (PopupEnum.Notice);
				SendMessageUpwards (GameConstant.ClearNoticeManager, NoticeManager.INFO, SendMessageOptions.DontRequireReceiver);
			}
		}

		private bool ShowLoginBonus ()
		{
			LoginBonus loginBonus = LoginBonus.GetInstance;
			if (loginBonus.m_login_bonus_id != 0) {
				int dayIndex = loginBonus.login_num;
				IEnumerable<LoginBonusDetailCSVStructure> loginBonusDetailCSVStructureEnumerable = MasterCSV.loginBonusDetailCSV.Where (result => loginBonus.m_login_bonus_id == result.m_login_bonus_id);
				popupLoader.no = () => {
					popupLoader.no = null;
					ShowNotice ();
					AssetBundlePool.Dispose (AssetBundleName.login_bonus.ToString (), true);
				};
				popupLoader.Popup (PopupEnum.LoginBonus, null, new List<object> () {
					loginBonusDetailCSVStructureEnumerable,
					dayIndex
				});
				loginBonus.m_login_bonus_id = 0;
				return true;
			}
			return false;
		}

		private void AddEventListeners ()
		{
			int length = pages.Length;
			for (int i = 0; i < length; i++) {
				pages [i].setDockActive = (bool isActive) => {
					SetDockActive (isActive);
				};
			}
		}

		private void RemoveEventListeners ()
		{
			int length = pages.Length;
			for (int i = 0; i < length; i++) {
				pages [i].setDockActive = null;
			}
		}

		private void SetDockActive (bool isActive)
		{
			home.header.gameObject.SetActive (isActive);
			home.footer.gameObject.SetActive (isActive);
		}

		private void OnDestroy ()
		{
			RemoveEventListeners ();
		}
	}
}