using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using System.Collections.Generic;
using UnityEngine.Purchasing;
using System.Linq;

public class ShopListMediator : ActivityMediator
{
	private ShopList shopList;
	public LimitLogic limitLogic;
	private int age_range;
	public ShopLimitLogic shopLimitLogic;
	private const int MAX_LIMIT_MONEY = 5000;
	private List<ProductScrollItem> productScrollItemList;

	protected override void CreateActions ()
	{
		unityActionArray = new UnityAction[] {
			() => {
				ComponentConstant.SOUND_MANAGER.Play (SoundEnum.se02_ng);
				showWindow (0);
			}
		};
	}

	private void OnEnable ()
	{
		Init ();
	}

	private void OnDisable ()
	{
		gameObject.SetActive (false);
	}

	public void Init ()
	{
		shopList = viewWithDefaultAction as ShopList;
		RogerContainerCleaner.Clean (shopList.container);
		SendAPI ();
	}

	private void InitProduct ()
	{
		productScrollItemList = new List<ProductScrollItem> ();
		List<LimitShop> LimitList = UpdateInformation.GetInstance.limit_shop_list;
		List<ProductCSVStructure> productCSVStructureEnumerable = MasterCSV.productCSV.ToList ();
		List<ProductCSVStructure> list = productCSVStructureEnumerable.Where (result => result.jpy_amount < (age_range == 1 ? MAX_LIMIT_MONEY : int.MaxValue) && result.is_ad_product != 1 && result.platform_type == ((Application.platform == RuntimePlatform.Android) ? 2 : 1)).ToList ();
		for (var i = 0; i < list.Count; i++) {
			ProductCSVStructure item = list [i];
			if (item.is_special == 1) {
				LimitShop limitShop = LimitList.FirstOrDefault (result => result.product_id == item.product_id);
				if (limitShop != null && limitShop.buy_count < limitShop.limit_count) {
					item.name = limitShop.title;
					item.free_coin = limitShop.end_at;
					item.limit_shop_id = limitShop.id;
					item.buy_count = limitShop.buy_count;
					item.limit_count = limitShop.limit_count;
					AddProduct (item, true);
				}
			} else {
				AddProduct (item, false);
			}
		}
	}

	private void AddProduct (ProductCSVStructure item, bool isSpecial)
	{
		ProductScrollItem productScrollItem = Instantiator.GetInstance ().Instantiate<ProductScrollItem> (shopList.instantiation, Vector2.zero, Vector3.one, shopList.container);

		productScrollItem.unityAction = (RogerScrollItem rogerScrollItem) => {
			SetAllProducts (false);
			ProductScrollItem currentProductScrollItem = rogerScrollItem as ProductScrollItem;
			CheckAge (currentProductScrollItem.price, currentProductScrollItem.productName);
		};

		productScrollItem.Show (item.image_resouce_name, item.name, item.coin, item.free_coin, item.jpy_amount, isSpecial, item.buy_count, item.limit_count);
		productScrollItemList.Add (productScrollItem);
	}

	private void SetAllProducts (bool isEnable)
	{
		int length = productScrollItemList.Count;
		for (int i = 0; i < length; i++) {
			productScrollItemList [i].button.enabled = isEnable;
		}
	}

	private void CheckAge (int jpy_amount, string productName)
	{
		limitLogic.complete = () => {
			if (APIInformation.GetInstance.can_charge) {
				BuyProduct (productName);
			} else {
				ComponentConstant.POPUP_LOADER.Popup (PopupEnum.MessagePopup, null, new List<object> () {
					LanguageJP.PURCHASE_LIMIT_TITLE,
					LanguageJP.PURCHASE_LIMIT_CONTENT
				});
				SetAllProducts (true);
			}
		};
		limitLogic.price = jpy_amount;
		limitLogic.age_range = age_range;
		limitLogic.SendAPI ();
	}

	private void BuyProduct (string productName)
	{
		ComponentConstant.PURCHASER.BuyProduct (productName);
	}

	private void SendAPI ()
	{
		shopLimitLogic.complete = () => {
//			ComponentConstant.SHOP_INITIALIZER.SetProduct ();
			InitProduct ();
		};
		shopLimitLogic.SendAPI ();
	}

	public void SetWindow (int age_range)
	{
		this.age_range = age_range;
	}
}
