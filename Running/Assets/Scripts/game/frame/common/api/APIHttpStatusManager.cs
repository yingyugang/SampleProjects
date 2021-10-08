using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using System.Collections.Generic;

public class APIHttpStatusManager : MonoBehaviour
{
	public UnityAction downloadAssetsComplete;
	private PopupContentMediator popupContentMediator;

	public bool CheckStatus (int status)
	{
		switch (status) {
		case 2001:
			popupContentMediator = ComponentConstant.POPUP_LOADER.Popup (PopupEnum.MessagePopup, null, new List<object> () {
				LanguageJP.MAINTENANCE_TITLE,
				APIInformation.GetInstance.maintenance_text
			});
			popupContentMediator.ok = () => {
				popupContentMediator.ok = null;
				ComponentConstant.API_MANAGER.ReSendAPI ();
			};
			return true;
		case 2002:
			popupContentMediator = ComponentConstant.POPUP_LOADER.Popup (PopupEnum.MessagePopup, null, new List<object> () {
				LanguageJP.UPDATE_APP_VERSION_TITLE,
				LanguageJP.UPDATE_APP_VERSION_CONTENT
			});
			popupContentMediator.ok = () => {
				popupContentMediator.ok = null;
				if (Application.platform == RuntimePlatform.Android) {
					Application.OpenURL ("https://play.google.com/store/apps/details?id=com.dtechno.osoparty");
				} else {
					Application.OpenURL ("https://itunes.apple.com/jp/app/id1090811848");
				}
			};
			return true;
		case 2003:
			if (GameConstant.hasLogin) {
				popupContentMediator = ComponentConstant.POPUP_LOADER.Popup (PopupEnum.MessagePopup, null, new List<object> () { 
					LanguageJP.UPDATE_RESOURCES_VERSION_TITLE,
					LanguageJP.UPDATE_RESOURCES_VERSION_CONTENT,
				});
				popupContentMediator.ok = () => {
					popupContentMediator.ok = null;
					StartDownload ();
				};
			} else {
				StartDownload ();
			}
			return true;
		case 2004:
			PathConstant.isReview = true;
			PathConstant.REVIEW_SERVER_PATH = APIInformation.GetInstance.review_url;
			StartDownload ();
			return true;
		case 2005:
			popupContentMediator = ComponentConstant.POPUP_LOADER.Popup (PopupEnum.MessagePopup, null, new List<object> () { 
				LanguageJP.RESTART_APP_TITLE,
				LanguageJP.RESTART_APP_CONTENT,
			});
			popupContentMediator.ok = () => {
				popupContentMediator.ok = null;
				ComponentConstant.SCENE_COMBINE_MANAGER.LoadScene (SceneEnum.Startup);
			};
			return true;
		case 1001:
			popupContentMediator = ComponentConstant.POPUP_LOADER.Popup (PopupEnum.MessagePopup, null, new List<object> () { 
				LanguageJP.WRONG_TITLE,
				LanguageJP.CODE_1001_WRONG_CONTENT,
			});
			popupContentMediator.ok = () => {
				popupContentMediator.ok = null;
				ComponentConstant.SCENE_COMBINE_MANAGER.LoadScene (SceneEnum.Startup);
			};
			return true;
		case 1002:
			popupContentMediator = ComponentConstant.POPUP_LOADER.Popup (PopupEnum.MessagePopup, null, new List<object> () { 
				LanguageJP.WRONG_TITLE,
				LanguageJP.CODE_1002_WRONG_CONTENT,
			});
			popupContentMediator.ok = () => {
				popupContentMediator.ok = null;
				ComponentConstant.SCENE_COMBINE_MANAGER.LoadScene (SceneEnum.Startup);
			};
			return true;
		case 1004:
			popupContentMediator = ComponentConstant.POPUP_LOADER.Popup (PopupEnum.MessagePopup, null, new List<object> () { 
				LanguageJP.WRONG_TITLE,
				LanguageJP.CODE_1004_WRONG_CONTENT,
			});
			popupContentMediator.ok = () => {
				popupContentMediator.ok = null;
				ComponentConstant.SCENE_COMBINE_MANAGER.LoadScene (SceneEnum.Startup);
			};
			return true;
		case 1111:
			popupContentMediator = ComponentConstant.POPUP_LOADER.Popup (PopupEnum.MessagePopup, null, new List<object> () { 
				LanguageJP.WRONG_TITLE,
				LanguageJP.CODE_1111_WRONG_CONTENT,
			});
			popupContentMediator.ok = () => {
				popupContentMediator.ok = null;
				ComponentConstant.SCENE_COMBINE_MANAGER.LoadScene (SceneEnum.Startup);
			};
			return true;
		case 2006:
			popupContentMediator = ComponentConstant.POPUP_LOADER.Popup (PopupEnum.MessagePopup, null, new List<object> () {
				LanguageJP.WRONG_TITLE,
				LanguageJP.CODE_2006_WRONG_CONTENT
			});
			popupContentMediator.ok = () => {
				popupContentMediator.ok = null;
				ComponentConstant.SCENE_COMBINE_MANAGER.LoadScene (SceneEnum.Startup);
			};
			return true;
		case 3000:
			popupContentMediator = ComponentConstant.POPUP_LOADER.Popup (PopupEnum.MessagePopup, null, new List<object> () {
				LanguageJP.WRONG_TITLE,
				APIInformation.GetInstance.maintenance_text
			});
			popupContentMediator.ok = () => {
				popupContentMediator.ok = null;
				ComponentConstant.SCENE_COMBINE_MANAGER.LoadScene (SceneEnum.Startup);
			};
			return true;
		case 5000:
			popupContentMediator = ComponentConstant.POPUP_LOADER.Popup (PopupEnum.MessagePopup, null, new List<object> () {
				LanguageJP.WRONG_TITLE,
				APIInformation.GetInstance.maintenance_text
			});
			popupContentMediator.ok = () => {
				popupContentMediator.ok = null;
			};
			return true;
		default :
			return false;
		}
	}

	private void StartDownload ()
	{
		GameConstant.hasDownloaded = false;
		ComponentConstant.SOUND_MANAGER.Play (SoundEnum.bgm01_title, null, false);
		AssetBundlePool.DisposeAll (false);
		ComponentConstant.SCENE_COMBINE_MANAGER.LoadScene (SceneEnum.Downloading);
	}
}