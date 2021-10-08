using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using System.Collections.Generic;

public class APIManager : MonoBehaviour
{
	private string apiPath;
	private byte[] byteArray;
	private Dictionary<string,string> headers;
	public UnityAction<string> complete;
	public UnityAction<string> error;
	public APIHttpStatusManager apiHttpStatusManager;
	public ShortLoadingMediator shortLoadingMediator;
	private PopupContentMediator popupContentMediator;
	private WWW www;
	private float time = 0;
	private float timeOut = 10f;
	private bool isLoading;
	private static bool isSending;

	public void SendAPI (string apiPath, byte[] byteArray, Dictionary<string,string> headers)
	{
		this.apiPath = apiPath;
		this.byteArray = byteArray;
		this.headers = headers;
		if (Application.internetReachability == NetworkReachability.NotReachable) {
			popupContentMediator = ComponentConstant.POPUP_LOADER.Popup (PopupEnum.MessagePopup, null, new List<object> () {
				LanguageJP.NET_ERROR_TITLE,
				LanguageJP.NET_ERROR_CONTENT
			});
			popupContentMediator.ok = () => {
				popupContentMediator.ok = null;
				ComponentConstant.API_MANAGER.ReSendAPI ();
			};
		} else {
			if (!isSending) {
				StartCoroutine (Send ());
			}
		}
	}

	public void ReSendAPI ()
	{
		if (!isSending) {
			StartCoroutine (Send ());
		}
	}

	private void Update ()
	{
		time += Time.deltaTime;
		if (time >= timeOut && isLoading) {
			StopAllCoroutines ();
			www.Dispose ();
			isLoading = false;
			popupContentMediator = ComponentConstant.POPUP_LOADER.Popup (PopupEnum.MessagePopup, null, new List<object> () {
				LanguageJP.NET_ERROR_TITLE,
				LanguageJP.NET_ERROR_CONTENT
			});
			isSending = false;
			shortLoadingMediator.Hide ();
			popupContentMediator.ok = () => {
				popupContentMediator.ok = null;
				ComponentConstant.API_MANAGER.ReSendAPI ();
			};
		}
	}

	private IEnumerator Send ()
	{
		time = 0;
		isLoading = true;
		isSending = true;
		shortLoadingMediator.Show ();
		Debug.Log ("URL: " + PathConstant.Get_SERVER_PATH + apiPath);
		www = new WWW (PathConstant.Get_SERVER_PATH + apiPath, byteArray, headers);
		yield return www;
		isSending = false;
		shortLoadingMediator.Hide ();
		if (www.isDone && string.IsNullOrEmpty (www.error)) {
			Parse (www.text);
			int status = SystemInformation.GetInstance.system_code;
			if (!apiHttpStatusManager.CheckStatus (status)) {
				if (status != 0) {
					if (error != null) {
						error (status.ToString ());
					}
				} else {
					if (complete != null) {
						complete (www.text);
					}
				}
			} else {
				if (error != null) {
					error (status.ToString ());
				}
			}
		} else {
			Debug.Log (www.error);
			popupContentMediator = ComponentConstant.POPUP_LOADER.Popup (PopupEnum.MessagePopup, null, new List<object> () {
				LanguageJP.NET_ERROR_TITLE,
				LanguageJP.NET_ERROR_CONTENT
			});
			popupContentMediator.ok = () => {
				popupContentMediator.ok = null;
				ComponentConstant.API_MANAGER.ReSendAPI ();
			};
		}
		isLoading = false;
	}

	private void Parse (string content)
	{
		Debug.Log (content);
		if (Model.GetInstance != null) {
			JsonUtility.FromJsonOverwrite (content, Model.GetInstance);
		} else {
			JsonUtility.FromJson<Model> (content);
		}
	}

	private void OnDestroy ()
	{
		apiHttpStatusManager.downloadAssetsComplete = null;
	}
}
