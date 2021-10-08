using UnityEngine;
using System.Collections;
using ChartboostSDK;

public class AdNetworks : MonoBehaviour {

	// Usage : Setup this script with your IDs, load a reference of this script ( AdNetworks adNetworks = FindObjectOfType<AdNetworks>(); ) 
	// and use the Functions ShowBanner(); HideBanner(); And ShowInterstitial();
	// This code is called by the script GameOverHandler.cs

	AdmobHandler admobHandler;
	ApplovinHandler applovinHandler;
    ChartboostHandler chartboostHandler;

	public bool InitializeNetworksOnStart = true;
	public int LoadInterstitialEveryXCalls = 1; 
	static int interstitialCounter;

	public BannersNetwork bannerNetwork;
	public enum BannersNetwork {
		Admob,Applovin,nothing
	}

	public InterstitialNetwork interstitialNetwork;
	public enum InterstitialNetwork {
		Admob,Applovin,Chartboost,nothing
	}

	public BannersPosition bannersPosition;
	public enum BannersPosition {
		Bottom,Top,BottomLeft,BottomRight,TopLeft,TopRight
	}




	public string Applovin_Sdk_KEY = "Yor Applovin SDK Key";

	public string Admob_Banner_ID = "Your adMob Banner ID";
	public string Admob_Interstitial_ID  = "Your adMob Interstitial ID";

	public string Chartboost_App_ID = "Your ChartBoost App ID";
	public string Chartboost_App_Signature = "Your ChartBoost App Signature";


	
	int caseSwitchInteger;

	
	


	// Use this for initialization
	void Start () {
		interstitialCounter = LoadInterstitialEveryXCalls;

		admobHandler = gameObject.AddComponent<AdmobHandler> ();
		applovinHandler = gameObject.AddComponent<ApplovinHandler> ();
		chartboostHandler = gameObject.AddComponent<ChartboostHandler> ();

		if (InitializeNetworksOnStart) {
			InitializeNetworks ();
		}


	}



	public void InitializeNetworks(){

		// Initialize for Banners

		switch (bannerNetwork) {
		case BannersNetwork.Applovin:
			applovinHandler.Initialize(Applovin_Sdk_KEY);
			break;

		case BannersNetwork.Admob:
			admobHandler.Initialize(Admob_Banner_ID,Admob_Interstitial_ID,false);
			break;
		}

      
		// Initialize for Interstitials

		switch (interstitialNetwork) {
		
	    case InterstitialNetwork.Applovin:
			applovinHandler.Initialize(Applovin_Sdk_KEY);
				break;
		
		case InterstitialNetwork.Admob:
			admobHandler.Initialize(Admob_Banner_ID,Admob_Interstitial_ID,true);
				break;
		
		case InterstitialNetwork.Chartboost:
			chartboostHandler.Initialize(Chartboost_App_ID,Chartboost_App_Signature,this.gameObject);
			    break;


		}


	}

	public void ShowBanner(){

		switch (bannerNetwork) {
		case BannersNetwork.Applovin:
			applovinHandler.ShowBanner(PositionBannerInteger());
			break;

		case BannersNetwork.Admob:
			admobHandler.ShowBanner(PositionBannerInteger());
			break;
		}

	}

	public void HideBanner(){

		switch (bannerNetwork) {
		case BannersNetwork.Applovin:
			applovinHandler.HideBanner();
			break;

		case BannersNetwork.Admob:
			admobHandler.HideBanner();
			break;

		}
	}



	public void ShowInterstitial(){
		if (interstitialCounter > 1) {
			interstitialCounter--;
			Debug.Log("Interstitial Ad will be called in the next " + interstitialCounter + " calls");
			return;
		}
		interstitialCounter = LoadInterstitialEveryXCalls;


		switch (interstitialNetwork) {
		case InterstitialNetwork.Applovin:
			applovinHandler.ShowInterstitial();
			break;
		case InterstitialNetwork.Admob:
			admobHandler.ShowInterstitial();
			break;
		case InterstitialNetwork.Chartboost:
			chartboostHandler.ShowInterstitial();
			break;
		
		}
		
	}





	//--------------------------------------------------------------------------------



	int PositionBannerInteger(){
		switch (bannersPosition) {
		case BannersPosition.Bottom: 
			caseSwitchInteger=1;
			break;
		case BannersPosition.Top: 
			caseSwitchInteger=2;
			break;
		case BannersPosition.BottomLeft: 
			caseSwitchInteger=3;
			break;
		case BannersPosition.BottomRight: 
			caseSwitchInteger=4;
			break;
		case BannersPosition.TopLeft: 
			caseSwitchInteger=5;
			break;
		case BannersPosition.TopRight: 
			caseSwitchInteger=6;
			break;
	
		}
		return caseSwitchInteger;

		
	}










}
