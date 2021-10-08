using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
//using Prime31;

public class Email : MonoBehaviour 
{
	public void SendEmail ()
	{
		Debug.Log("SendEmail");
		ComponentConstant.SOUND_MANAGER.Play(SoundEnum.se01_ok);
		string email = "";
		string subject = MyEscapeURL("おそ松さん はちゃめちゃパーティー!(ミニゲーム集)");
		string body = MyEscapeURL("TVアニメ「おそ松さん」の迷シーンがいろんなゲームになった! ミニゲーム集スマホアプリ『おそ松さん はちゃめちゃパーティー!』 http://osomatsusan-app.d-techno.jp/party.html");
		Application.OpenURL("mailto:" + email + "?subject=" + subject + "&body=" + body);

//		StartCoroutine( saveScreenshotToSDCard( path =>
//		{
//				showEmailComposer( "", "おそ松さん はちゃめちゃパーティー!(ミニゲーム集)", "TVアニメ「おそ松さん」の迷シーンがいろんなゲームになった! ミニゲーム集スマホアプリ『おそ松さん はちゃめちゃパーティー!』 http://osomatsusan-app.d-techno.jp/party.html", true, path );
//		} ) );
	}

	string MyEscapeURL (string url)
	{
		return WWW.EscapeURL(url).Replace("+","%20");
	}


//	// Saves a screenshot to the SD card and then calls completionHandler with the path to the image
//	private IEnumerator saveScreenshotToSDCard( System.Action<string> completionHandler )
//	{
//		yield return new WaitForEndOfFrame();
//		int w = Mathf.RoundToInt(1080f *  (Screen.width / 1080f));
//		int h = Mathf.RoundToInt(1490f *  (Screen.height / 1920f));
//		Debug.Log (w + " x " + h);
//		Texture2D m_Texture = new Texture2D(w, h, TextureFormat.RGB24, false);
//		yield return new WaitForEndOfFrame ();
//		m_Texture.ReadPixels(new Rect(0f, 211f * (Screen.height / 1920f), w, h), 0, 0, false);
//		m_Texture.Apply();
//		var bytes = m_Texture.EncodeToPNG();
//		var path = System.IO.Path.Combine( Application.persistentDataPath, "myImage.png" );
//		System.IO.File.WriteAllBytes( path, bytes );
//
//		completionHandler( path );
//	}
//
//	void showEmailComposer( string toAddress, string subject, string text, bool isHTML, string attachmentFilePath )
//	{   
//		#if UNITY_ANDROID
//		AndroidJavaClass intentClass = new AndroidJavaClass("android.content.Intent");
//		AndroidJavaObject intentObject = new AndroidJavaObject("android.content.Intent");
//
//		intentObject.Call<AndroidJavaObject>("setAction", intentClass.GetStatic<string>("ACTION_SEND"));
//		intentObject.Call<AndroidJavaObject>("setType", "text/html");
//		intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_SUBJECT"), subject);
//		intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_TITLE"), subject);
//		intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_TEXT"), text);
//
//		AndroidJavaClass uriClass = new AndroidJavaClass("android.net.Uri");
//
//		AndroidJavaObject fileObject = new AndroidJavaObject("java.io.File", attachmentFilePath);// Set Image Path Here
//
//		AndroidJavaObject uriObject = uriClass.CallStatic<AndroidJavaObject>("fromFile", fileObject);
//
//		intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_STREAM"), uriObject);
//
//		bool fileExist = fileObject.Call<bool>("exists");
//		Debug.Log("File exist : " + fileExist);
//		AndroidJavaClass unity = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
//		AndroidJavaObject currentActivity = unity.GetStatic<AndroidJavaObject>("currentActivity");
//		//currentActivity.Call("startActivity", intentObject);
//
//		AndroidJavaObject jChooser = intentClass.CallStatic<AndroidJavaObject>("createChooser", intentObject, subject);
//		currentActivity.Call("startActivity", jChooser);
//
//		#elif UNITY_IOS
//
//		Debug.Log("showEmailComposer: " + attachmentFilePath); 
//		//CallSocialShareAdvanced(text, subject, "", attachmentFilePath);
//
//		EtceteraBinding.showMailComposerWithAttachment(attachmentFilePath, "image/png", "myImage.png", toAddress, subject, text, isHTML );
//		#else
//				Debug.Log("No sharing set up for this platform.");
//		#endif
//	}
//
//#if UNITY_IOS
//	public struct ConfigStruct
//	{
//		public string title;
//		public string message;
//	}
//
//	[DllImport ("__Internal")] private static extern void showAlertMessage(ref ConfigStruct conf);
//	
//	public struct SocialSharingStruct
//	{
//		public string text;
//		public string url;
//		public string image;
//		public string subject;
//	}
//	
//	[DllImport ("__Internal")] private static extern void showSocialSharing(ref SocialSharingStruct conf);
//	
//	public static void CallSocialShare(string title, string message)
//	{
//		ConfigStruct conf = new ConfigStruct();
//		conf.title  = title;
//		conf.message = message;
//		showAlertMessage(ref conf);
//	}
//
//	public static void CallSocialShareAdvanced(string defaultTxt, string subject, string url, string img)
//	{
//		SocialSharingStruct conf = new SocialSharingStruct();
//		conf.text = defaultTxt; 
//		conf.url = url;
//		conf.image = img;
//		conf.subject = subject;
//		
//		showSocialSharing(ref conf);
//	}
//#endif

}
