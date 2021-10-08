using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using UnityEngine.UI;

public class ShopAd : ViewWithDefaultAction
{
	public GameObject ad;
	public Text price;

	protected override void AddActions ()
	{
		unityActionArray = new UnityAction[] {
			() => {
				Send (0);
			},
			() => {
				Send (1);
			},
			() => {
				Send (2);
			}
		};
	}


	public void ShowOrHideAd (bool isShow)
	{
        if (!isShow)
        {
            AdsManager.HideBtmBanner();
        }
		ad.SetActive (isShow);
	}
}
