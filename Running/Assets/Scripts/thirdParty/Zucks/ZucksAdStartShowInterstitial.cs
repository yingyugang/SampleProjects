using UnityEngine;
using System.Collections;

public class ZucksAdStartShowInterstitial : ZucksAdNetworkInterstitial {

	void Start () {
		ShowInterstitial();
	}

	void OnDestroy () {
		HideInterstitial();
	}
}
