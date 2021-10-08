using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;

public class APILogic : MonoBehaviour
{
	protected byte[] byteArray;
	protected Dictionary<string,string> headers;
	protected string apiPath;
	public UnityAction complete;
	public UnityAction<string> error;

	private void SetHeader ()
	{
		headers = new Dictionary<string, string> ();
		headers.Add ("Content-Type", "application/json; charset=UTF-8");
		string deviceID = SystemConstant.DeviceID;
		Debug.Log ("DeviceID: " + deviceID);
		if (!string.IsNullOrEmpty (deviceID)) {
			headers.Add ("Device-Id", deviceID);
		}
		headers.Add ("Device-Platform", 
			#if UNITY_IOS
			"1"
			#elif UNITY_ANDROID
			"2"
			#endif
		);
		headers.Add ("Client-Version", SystemConstant.CLIENT_VERSION);
		string hashCodeOfVersionFile = SystemConstant.HashCodeOfVersionFile;
		if (!string.IsNullOrEmpty (hashCodeOfVersionFile)) {
			headers.Add ("Assets-Version", hashCodeOfVersionFile);
		}
	
		if (SystemInformation.GetInstance != null && !string.IsNullOrEmpty (SystemInformation.GetInstance.api_token)) {
			headers.Add ("Api-Token", SystemInformation.GetInstance.api_token);
		}
		headers.Add ("User-Agent", SystemConstant.UserAgent);
		headers.Add ("Content-Encoding", "gzip");
	}

	virtual protected void SetAPI ()
	{
		
	}

	virtual public void SendAPI ()
	{
		ComponentConstant.API_MANAGER.complete = (string content) => {
			APICallback (content);
			UpdateFromData ();
			ComponentConstant.API_MANAGER.complete = null;
			if (complete != null) {
				complete ();
			}
		};
		ComponentConstant.API_MANAGER.error = (string status) => {
			ComponentConstant.API_MANAGER.error = null;
			ErrorHandler (status);
			if (error != null) {
				error (status);
			}
		};
		SetHeader ();
		SetAPI ();
		ComponentConstant.API_MANAGER.SendAPI (APIConstant.PREFIX + apiPath, byteArray, headers);
	}

	virtual protected void ErrorHandler (string status)
	{
		
	}

	virtual public void APICallback (string content)
	{
		
	}

	virtual public void UpdateFromData ()
	{
		
	}

	private void OnDestroy ()
	{
		ComponentConstant.API_MANAGER.complete = null;
	}
}
