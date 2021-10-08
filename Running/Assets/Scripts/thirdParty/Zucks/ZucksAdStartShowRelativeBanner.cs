using UnityEngine;
using System.Collections;

public class ZucksAdStartShowRelativeBanner : ZucksAdNetworkBanner {
	
	void Start () {
//		ShowRelative();
	}

	void OnDestroy () {
		Hide();
	}
}
