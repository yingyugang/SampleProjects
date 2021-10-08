using UnityEngine;
using System.Collections;

public class ZucksAdStartShowBanner : ZucksAdNetworkBanner {
	
	void Start () {
		Show();
	}

	void OnDestroy () {
		Hide();
	}

}
