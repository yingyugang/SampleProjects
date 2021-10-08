using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using System.Linq;
using System.Collections.Generic;

public class ItemExchangeDetailMediator : PopupContentMediator
{
	private ItemExchangeDetail itemExchangeDetail;

	private void OnEnable ()
	{
		itemExchangeDetail = popupContent as ItemExchangeDetail;
		RogerContainerCleaner.Clean (itemExchangeDetail.container);

		List<LimitItem> list = UpdateInformation.GetInstance.limit_item_list;
		int length = list.Count;
		for (int i = 0; i < length; i++)
		{
			ItemExchangePopupScrollItem itemExchangePopupScrollItem = Instantiator.GetInstance ().Instantiate (itemExchangeDetail.instantiation, Vector2.zero, Vector3.one, itemExchangeDetail.container);

			itemExchangePopupScrollItem.description.text = list[i].description;
			ItemCSVStructure itemCSVStructure = MasterCSV.itemCSV.FirstOrDefault (result => result.id == list[i].item_id);
			itemExchangePopupScrollItem.icon.sprite = AssetBundleResourcesLoader.itemIconDictionary[itemCSVStructure.image_resource];
			itemExchangePopupScrollItem.gameObject.SetActive (true);
		}
	}

	protected override void OKButtonOnClickHandler ()
	{
		ComponentConstant.SOUND_MANAGER.Play (SoundEnum.se02_ng);
		if (ok != null)
		{
			ok ();
		}
		ClosePopup ();
	}
}
