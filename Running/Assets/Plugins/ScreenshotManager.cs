#pragma warning disable 0168 // variable declared but not used.
#pragma warning disable 0219 // variable assigned but not used.

using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Runtime.InteropServices;

public class ScreenshotManager : MonoBehaviour {
	
	public static event Action ScreenshotFinishedSaving;
	public static event Action ImageFinishedSaving;
	
	#if UNITY_IPHONE
	
	[DllImport("__Internal")]
    private static extern bool saveToGallery( string path );
	
	#endif

	static void SaveTextureToFile(  Texture2D texture ,string filename)
	{
		byte[] bytes = texture.EncodeToPNG();
		File.WriteAllBytes(filename, bytes);
	}
	
	public static IEnumerator Save(string fileName, string albumName = "MyScreenshots", bool callback = false)
	{
		bool photoSaved = false;
		
		string date =  System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
		ScreenshotManager.ScreenShotNumber++;
		
		string screenshotFilename = fileName + "_" + ScreenshotManager.ScreenShotNumber + "_" + date + ".png";
		
		Debug.Log("Save screenshot " + screenshotFilename); 

		int w = Mathf.RoundToInt(1080f *  (Screen.width / 1080f));
		int h = Mathf.RoundToInt(1490f *  (Screen.height / 1920f));
		Debug.Log (w + " x " + h);
		Texture2D m_Texture = new Texture2D(w, h, TextureFormat.RGB24, false);
		yield return new WaitForEndOfFrame ();
		m_Texture.ReadPixels(new Rect(0f, 211f * (Screen.height / 1920f), w, h), 0, 0, false);
		m_Texture.Apply();
		//Debug.Log (Application.persistentDataPath);


		#if UNITY_IPHONE
		
			if(Application.platform == RuntimePlatform.IPhonePlayer) 
			{
				Debug.Log("iOS platform detected");
				
				string iosPath = Application.persistentDataPath + "/" + screenshotFilename;
				SaveTextureToFile (m_Texture, iosPath);
				//Application.CaptureScreenshot(screenshotFilename);
				
				while(!photoSaved) 
				{
					photoSaved = saveToGallery( iosPath );
					
					yield return new WaitForSeconds(.5f);
				}
			
				UnityEngine.iOS.Device.SetNoBackupFlag( iosPath );
			
			} else {
			
				//Application.CaptureScreenshot(screenshotFilename);
				SaveTextureToFile (m_Texture, Application.persistentDataPath + "/" + screenshotFilename);
			}
			
		#elif UNITY_ANDROID	
				
			if(Application.platform == RuntimePlatform.Android) 
			{
				Debug.Log("Android platform detected");
				
				string androidPath = "/../../../../DCIM/" + albumName + "/" + screenshotFilename;

				string path = Application.persistentDataPath + androidPath;
				string pathonly = Path.GetDirectoryName(path);
				Directory.CreateDirectory(pathonly);
				//Application.CaptureScreenshot(androidPath);
				SaveTextureToFile (m_Texture, path);
				AndroidJavaClass obj = new AndroidJavaClass("com.ryanwebb.androidscreenshot.MainActivity");
				
				while(!photoSaved) 
				{
					photoSaved = obj.CallStatic<bool>("scanMedia", path);
				
					yield return new WaitForSeconds(.5f);
				}
		
			} else {
				SaveTextureToFile (m_Texture, Application.persistentDataPath + "/" + screenshotFilename);
				//Application.CaptureScreenshot(screenshotFilename);
		
			}
		#else
			
			while(!photoSaved) 
			{
				yield return new WaitForSeconds(.5f);
		
				Debug.Log("Screenshots only available in iOS/Android mode!");
			
				photoSaved = true;
			}
		
		#endif
		
		if(callback)
			ScreenshotFinishedSaving();
	}
	
	
	public static IEnumerator SaveExisting(string filePath, bool callback = false)
	{
		bool photoSaved = false;
		
		Debug.Log("Save existing file to gallery " + filePath);

		#if UNITY_IPHONE
		
			if(Application.platform == RuntimePlatform.IPhonePlayer) 
			{
				Debug.Log("iOS platform detected");
				
				while(!photoSaved) 
				{
					photoSaved = saveToGallery( filePath );
					
					yield return new WaitForSeconds(.5f);
				}
			
				UnityEngine.iOS.Device.SetNoBackupFlag( filePath );
			}
			
		#elif UNITY_ANDROID	
				
			if(Application.platform == RuntimePlatform.Android) 
			{
				Debug.Log("Android platform detected");

				AndroidJavaClass obj = new AndroidJavaClass("com.ryanwebb.androidscreenshot.MainActivity");
					
				while(!photoSaved) 
				{
					photoSaved = obj.CallStatic<bool>("scanMedia", filePath);
							
					yield return new WaitForSeconds(.5f);
				}
			
			}
		
		#else
			
			while(!photoSaved) 
			{
				yield return new WaitForSeconds(.5f);
		
				Debug.Log("Save existing file only available in iOS/Android mode!");

				photoSaved = true;
			}
		
		#endif
		
		if(callback)
			ImageFinishedSaving();
	}
	
	
	public static int ScreenShotNumber 
	{
		set { PlayerPrefs.SetInt("screenShotNumber", value); }
	
		get { return PlayerPrefs.GetInt("screenShotNumber"); }
	}
}
