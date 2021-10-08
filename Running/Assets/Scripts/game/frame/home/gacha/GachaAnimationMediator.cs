using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

public class GachaAnimationMediator : ActivityMediator
{
	private int gachaAnimationType;
	private GachaAnimation gachaAnimation;
	private int gachaType;
	private int[] pointAnimationTypeArray = new int[] {
		7,
		6,
		5,
		4,
		3
	};

	private void OnEnable ()
	{
		if (setDockActive != null)
		{
			setDockActive (false);
		}
		gachaAnimation = viewWithDefaultAction as GachaAnimation;
		AdsManager.HideBtmBanner ();
		ComponentConstant.GACHA_PLAYER.unityAction = AnimationCompleteHandler;
		ComponentConstant.GACHA_PLAYER.Play (gachaAnimationType, false);
	}

	public void SetWindow (int gachaType, int mode)
	{
		this.gachaType = gachaType;

		if (gachaType == 3)
		{
			this.gachaAnimationType = pointAnimationTypeArray[mode - 6];
		}
		else if (gachaType == 5)
		{
			this.gachaAnimationType = 9;
		}
		else {
			this.gachaAnimationType = gachaType;
		}
		UnityEngine.Analytics.Analytics.CustomEvent ("gacha", new Dictionary<string, object> () {
			{ "gachaType",gachaType }, {
				"mode",
				mode
			}
		});
	}

	private void AnimationCompleteHandler (int type, bool isChanged)
	{
		gachaAnimation = viewWithDefaultAction as GachaAnimation;
		gachaAnimation.home.home.ad.SetActive (PlayerPrefs.GetInt ("AdUnlock", 0) == 0);
		if (PlayerPrefs.GetInt ("AdUnlock", 0) == 0)
			AdsManager.ShowBtmBanner ();
		if (setDockActive != null)
		{
			setDockActive (true);
		}
		if (isChanged)
		{
			SendMessageUpwards ("ShowChangeBadge", SendMessageOptions.DontRequireReceiver);
		}

		if (type == 1)
		{
			gameObject.SetActive (false);
			gotoGame ();
		}
		else {
			if (gachaType != 3)
			{
				showWindow (1);
			}
			else {
				showWindow (5);
			}
		}
	}
}
