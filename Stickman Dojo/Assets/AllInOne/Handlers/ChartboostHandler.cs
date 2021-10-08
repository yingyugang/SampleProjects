using UnityEngine;
using System.Collections;
using ChartboostSDK;


public class ChartboostHandler : MonoBehaviour {
	string app_ID;
	string app_Signature;

	// Use this for initialization
	public void Initialize(string appId, string appSignature, GameObject mainController ){
		app_ID = appId;
		app_Signature = appSignature;
		CBSettings settings = new CBSettings ();

		settings.SetAndroidAppId (app_ID);
		settings.SetAndroidAppSecret(app_Signature);

		Debug.Log ("Initialized Chartboost with APP_ID: " + CBSettings.getAndroidAppId () + " SIGN_ID: " + CBSettings.getAndroidAppSecret ());

		mainController.AddComponent<Chartboost> ();

		Chartboost.cacheInterstitial (CBLocation.Default);

	}

	public void ShowInterstitial(){
		Chartboost.showInterstitial (CBLocation.Default);
		// caching is useless because Chartboost.setAutoCacheAds (); is true by default

	}


}
