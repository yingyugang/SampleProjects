using UnityEngine;
using System.Collections;
//using Prime31;
using System.IO;

public class SocialNetwork : MonoBehaviour 
{
	public ShareLogic shareLogic;

	public const string TWITTER_CONSUMER_KEY = "U7NlVUkU0YxbFocSjgzZpQ8s3";
	public const string TWITTER_CONSUMER_SECRET = "5Rqhk2EpFa4EGWH2FjDE09vsJwnk0rJhg4gSRsA3kwlHdAo2CD";

	public static string screenshotFilename = "Screenshot.png";

	private bool m_ClickShareTwitter = false;

	string imagePath
	{
		get
		{

			#if UNITY_EDITOR_OSX
			return Application.dataPath + "/../image.png";
			#endif

			#if UNITY_STANDALONE_OSX
			return Application.dataPath +"/Data/image.png";
			#endif

			return Application.persistentDataPath + "/image.png";
		}
	}

	private static SocialNetwork m_Instance;
	public static SocialNetwork Instance
	{
		get
		{
			return m_Instance;
		}
	}

	void Awake()
	{
		m_Instance = this;
	}

	void Start()
	{
		//TwitterInit ();
		//TwitterManager.requestDidFinishEvent += RequestDidFinish;
		//TwitterManager.requestDidFailEvent += RequestDidFail;
	}

	void OnEnable()
	{
		
	}

	void OnDisable()
	{
		//TwitterManager.requestDidFinishEvent -= RequestDidFinish;
		//TwitterManager.requestDidFailEvent -= RequestDidFail;
	}

//	void RequestDidFinish(object json)
//	{
//		Debug.Log("-------RequestDidFinish: " + json.ToString());
//		if (m_ClickShareTwitter)
//		{
//			shareLogic.SendAPI();
//		}
//		m_ClickShareTwitter = false;
//	}
//
//	void RequestDidFail(string json)
//	{
//		Debug.Log("-------RequestDidFail: " + json);
//		m_ClickShareTwitter = false;
//	}

//	public void FbInit()
//	{
//		FacebookCombo.init();
//	}
//
//	public void FBLogin()
//	{
//		FbInit();
//		
//		// Note: requesting publish permissions here will result in a crash. Only read permissions are permitted.
//		var permissions = new string[] { "email", "user_friends" };
//		FacebookCombo.loginWithReadPermissions( permissions );
//	}
//
//	public static bool FbIsSessionValid()
//	{
//		return FacebookCombo.isSessionValid();
//	}
//
//	public void FBShareImage()
//	{
//		if (!FbIsSessionValid())
//		{
//			FBLogin();
//			return;
//		}
//
//		// grab a screenshot for later use
//		Application.CaptureScreenshot( screenshotFilename );
//
//		var pathToImage = Application.persistentDataPath + "/" + SocialNetwork.screenshotFilename;
//		if( !System.IO.File.Exists( pathToImage ) )
//		{
//			Debug.LogError( "there is no screenshot avaialable at path: " + pathToImage );
//			return;
//		}
//
//		var bytes = System.IO.File.ReadAllBytes( pathToImage );
//		Facebook.instance.postImage( bytes, "im an image posted from game", CompletionHandler );
//	}
//
//	// common event handler used for all graph requests that logs the data to the console
//	void CompletionHandler( string error, object result )
//	{
//		if( error != null )
//			Debug.LogError( error );
//		else
//			Prime31.Utils.logObject( result );
//	}
//
//
//	public void TwitterInit()
//	{
//		TwitterCombo.init(TWITTER_CONSUMER_KEY, TWITTER_CONSUMER_SECRET);
//		m_ClickShareTwitter = false;
//	}
//
//	public void TwitterLogin()
//	{
//		TwitterInit();
//		TwitterCombo.showLoginDialog();
//	}
//	void SaveTextureToFile(  Texture2D texture ,string filename)
//	{
//		byte[] bytes = texture.EncodeToPNG();
//		File.WriteAllBytes(filename, bytes);
//	}
//	IEnumerator TwitterShare()
//	{
//		if (!TwitterCombo.isLoggedIn())
//		{
//			TwitterLogin();
//			yield break;
//		}
//
//		m_ClickShareTwitter = true;
//		// grab a screenshot for later use
//
//		int w = Mathf.RoundToInt(1080f *  (Screen.width / 1080f));
//		int h = Mathf.RoundToInt(1490f *  (Screen.height / 1920f));
//		Debug.Log (w + " x " + h);
//		Texture2D m_Texture = new Texture2D(w, h, TextureFormat.RGB24, false);
//		yield return new WaitForEndOfFrame ();
//		m_Texture.ReadPixels(new Rect(0f, 211f * (Screen.height / 1920f), w, h), 0, 0, false);
//		m_Texture.Apply();
//		var pathToImage = Application.persistentDataPath + "/" + screenshotFilename;
//		SaveTextureToFile (m_Texture, pathToImage);
//		yield return new WaitForSeconds(0.5f);
//
//		TwitterCombo.postStatusUpdate( "-----\nTVアニメ「おそ松さん」の迷シーンがいろんなゲームになった！\nミニゲーム集スマホアプリ『おそ松さん はちゃめちゃパーティー！』\nhttp://osomatsusan-app.d-techno.jp/party.html\n#おそパー\n-----", pathToImage );
//	}
//
//
//	// Share using social connector
//	public void ShareImageFB()
//	{
//		ComponentConstant.SOUND_MANAGER.Play(SoundEnum.se01_ok);
//		StartCoroutine(SaveScreenshotFB());
//		//Application.CaptureScreenshot("image.png");
//
//		//SocialConnector.PostMessage(SocialConnector.ServiceType.Facebook, "Social Connector", "https://github.com/anchan828/social-connector", imagePath);
//	}
//
//	public void ShareTwitterFB()
//	{
//		Debug.Log("ShareTwitterFB");
//		ComponentConstant.SOUND_MANAGER.Play(SoundEnum.se01_ok);
//		//StartCoroutine(SaveScreenshotTwitter());
//		#if UNITY_EDITOR
//		shareLogic.SendAPI();
//		#else
//		StartCoroutine(TwitterShare());
//		#endif
//
//		//SocialConnector.PostMessage(SocialConnector.ServiceType.Twitter, "Social Connector", "https://github.com/anchan828/social-connector", imagePath);
//	}
//
//	IEnumerator SaveScreenshotFB()
//	{
//		yield return new WaitForEndOfFrame();
//		Application.CaptureScreenshot("image.png");
//		yield return new WaitForSeconds(1f);
//		string app = Application.productName;
//		SocialConnector.PostMessage(SocialConnector.ServiceType.Facebook, app, "http://osomatsusan-app.d-techno.jp/", imagePath);
//	}
//
//	IEnumerator SaveScreenshotTwitter()
//	{
//		yield return new WaitForEndOfFrame();
//		Application.CaptureScreenshot("image.png");
//		yield return new WaitForSeconds(1f);
//		string app = Application.productName;
//		SocialConnector.PostMessage(SocialConnector.ServiceType.Twitter, app, "http://osomatsusan-app.d-techno.jp/", imagePath);
//	}
}
