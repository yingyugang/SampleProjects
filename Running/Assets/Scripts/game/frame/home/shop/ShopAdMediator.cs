using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using UnityEngine.Purchasing;
using System.Collections.Generic;
using System.Linq;

public class ShopAdMediator : ActivityMediator
{
	private ShopAd shopAd;
	private ProductCSVStructure productCSVStructure;

	private void OnEnable ()
	{
		shopAd = viewWithDefaultAction as ShopAd;
		shopAd.buttonArray [1].interactable = PlayerPrefs.GetInt ("AdUnlock", 0) == 0;

//		ComponentConstant.SHOP_INITIALIZER.SetProduct ();

		List<ProductCSVStructure> productCSVStructureEnumerable = MasterCSV.productCSV.ToList ();
		productCSVStructure = productCSVStructureEnumerable.FirstOrDefault (result => result.is_ad_product == 1 && result.platform_type == ((Application.platform == RuntimePlatform.Android) ? 2 : 1));
		shopAd.price.text = string.Format ("{0}{1}", LanguageJP.YAN, productCSVStructure.jpy_amount);

	}

	protected override void CreateActions ()
	{
		shopAd = viewWithDefaultAction as ShopAd;

		unityActionArray = new UnityAction[] {
			() => {
				ComponentConstant.SOUND_MANAGER.Play (SoundEnum.se02_ng);
				showWindow (0);
			},
			() => {
				ComponentConstant.PURCHASER.BuyProduct (productCSVStructure.name);
			},
			() => {
				ComponentConstant.PURCHASER.RestorePurchases ();
			}
		};
	}
}
