using UnityEngine;
using System.Collections;

public class LongLoadingMediator : LoadingMediator
{
	private void OnEnable ()
	{
		AdsManager.HideBtmBanner ();
	}

	private void OnDisable ()
	{
		if (PlayerPrefs.GetInt ("AdUnlock", 0) == 1) {
			AdsManager.HideBtmBanner ();
		}
	}
}
