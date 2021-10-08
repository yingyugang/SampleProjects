using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Purchasing;

public class ShopInitializer : MonoBehaviour
{
	private static List<ProductInformation> productInformationList;
	private  bool savedShopStatus;

	private void GetLocalProduct ()
	{
		if (MasterCSV.productCSV != null) {
			List<ProductCSVStructure> productCSVStructureEnumerable = MasterCSV.productCSV.ToList ();
			List<ProductCSVStructure> list = productCSVStructureEnumerable.Where (result => result.platform_type == ((Application.platform == RuntimePlatform.Android) ? 2 : 1)).ToList ();
			for (var i = 0; i < list.Count; i++) {
				ProductCSVStructure item = list [i];
				productInformationList.Add (CreateProduct (item.name, (item.is_ad_product == 0) ? ProductType.Consumable : ProductType.NonConsumable, item.product_id));
			}
		}
	}

	public void SetProduct ()
	{
		productInformationList = new List<ProductInformation> ();
		GetLocalProduct ();
		ComponentConstant.PURCHASER.Init (productInformationList);
	}

	private ProductInformation CreateProduct (string productName, ProductType productType, string productId)
	{
		return new ProductInformation ().Init (productName, productType, productId);
	}

	private void GetServerProduct ()
	{
		List<LimitShop> list = UpdateInformation.GetInstance.limit_shop_list;
		int length = list.Count;
		for (int i = 0; i < length; i++) {
			productInformationList.Add (CreateProduct (list [i].title, ProductType.Consumable, list [i].product_id));
		}
	}
}
