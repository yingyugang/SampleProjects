using UnityEngine;
using System.Collections;

public class ApplovinHandler : MonoBehaviour {
	bool AlreadyInitialized;

	public void Initialize(string sdkKey){
		if (AlreadyInitialized) {
			return;
		}
		AppLovin.SetSdkKey (sdkKey);
		AppLovin.InitializeSdk ();
		AlreadyInitialized = true;
	}

	public void ShowBanner(int position){
		if (position==1)
			AppLovin.ShowAd(AppLovin.AD_POSITION_CENTER,AppLovin.AD_POSITION_BOTTOM);
		if (position==2)
			AppLovin.ShowAd(AppLovin.AD_POSITION_CENTER,AppLovin.AD_POSITION_TOP);
		if (position==3)
			AppLovin.ShowAd(AppLovin.AD_POSITION_LEFT,AppLovin.AD_POSITION_BOTTOM);
		if (position==4)
			AppLovin.ShowAd(AppLovin.AD_POSITION_RIGHT,AppLovin.AD_POSITION_BOTTOM);
		if (position==5)
			AppLovin.ShowAd(AppLovin.AD_POSITION_LEFT,AppLovin.AD_POSITION_TOP);
		if (position==6)
			AppLovin.ShowAd(AppLovin.AD_POSITION_RIGHT,AppLovin.AD_POSITION_TOP);
	
	}

	public void ShowInterstitial(){
		AppLovin.ShowInterstitial ();
	}


	public void HideBanner(){
		AppLovin.HideAd ();
	}





}
