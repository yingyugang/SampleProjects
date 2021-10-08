using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using UnityEngine.Purchasing;
using System.Collections.Generic;

public class ShopPageMediator : PageMediator
{
	protected override void CheckResources ()
	{
		pageNumber = 3;
		ShowWindow ();
	}

	protected override void Awake ()
	{
		ComponentConstant.PURCHASER.unityAction = PurchaseHandler;
		base.Awake ();
	}

	private void Start ()
	{
		ComponentConstant.PURCHASER.CheckProduct (ProductType.Consumable);
	}

	private void PurchaseHandler (bool isSuccess, string info, Product product)
	{
		if (product.definition.type == ProductType.NonConsumable) {
			if (isSuccess) {
				((ShopAdMediator)activityMediatorArray [3]).GetComponent<ShopAd> ().ShowOrHideAd (false);
				PlayerPrefs.SetInt ("AdUnlock", 1);
				page.popupLoader.Popup (PopupEnum.PurchaseFinished,null,new List<object>(){
                    LanguageJP.PURCHASE_AD_CONTENT
                });
			} else {
				ComponentConstant.POPUP_LOADER.Popup (PopupEnum.MessagePopup, null, new List<object> () {
					LanguageJP.PURCHASE_FAIL_TITLE,
					LanguageJP.PURCHASE_FAIL_CONTENT
				});
			}
		} else {
			if (isSuccess) {
				((ShopListMediator)activityMediatorArray [1]).GetComponent<ShopList> ().header.UpdateCoinAndMoney ();
                page.popupLoader.Popup (PopupEnum.PurchaseFinished,null,new List<object>(){
                    LanguageJP.PURCHASE_COIN_CONTENT
                });
			} else {
				ComponentConstant.POPUP_LOADER.Popup (PopupEnum.MessagePopup, null, new List<object> () {
					LanguageJP.PURCHASE_FAIL_TITLE,
					LanguageJP.PURCHASE_FAIL_CONTENT
				});
			}
			(activityMediatorArray [1] as ShopListMediator).Init ();
		}
	}
}
