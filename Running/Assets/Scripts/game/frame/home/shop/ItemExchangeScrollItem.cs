using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Linq;
using System;

public class ItemExchangeScrollItem : RogerInteractiveScrollItem
{
	[HideInInspector]
	public int m_exchange_id;
	public Text coinNumberField;
	public Text itemNumberField;
	public Text dateField;
	public Text limitField;
	public Image icon;
	public GameObject special;
	public GameObject date;
	public GameObject limit;
	private LimitItem limitItem;

	[HideInInspector]
	public int coinNumber;
	[HideInInspector]
	public int itemNumber;
	

	public void Show (LimitItem limitItem)
	{
		this.limitItem = limitItem;
		m_exchange_id = limitItem.id;
		this.coinNumber = limitItem.need_coin;
		this.itemNumber = limitItem.item_num;
		coinNumberField.text = string.Format ("{0}{1}", coinNumber, LanguageJP.M);
		itemNumberField.text = string.Format ("{0}{1}", itemNumber, LanguageJP.M);
		ItemCSVStructure currentItemCSVStructure = MasterCSV.itemCSV.FirstOrDefault (result => result.id == limitItem.item_id);
		icon.sprite = AssetBundleResourcesLoader.itemIconDictionary[currentItemCSVStructure.image_resource];
		if (limitItem.limit_type == 0)
		{
			date.SetActive (false);
			limit.SetActive (false);
			special.SetActive (false);
		}
		else if (limitItem.limit_type == 1)
		{
			ShowDate ();
			limit.SetActive (false);
			special.SetActive (true);
		}
		else if (limitItem.limit_type == 2)
		{
			ShowLimit ();
			date.SetActive (false);
			special.SetActive (true);
		}
		else if (limitItem.limit_type == 3)
		{
			ShowDate ();
			ShowLimit ();
			special.SetActive (true);
		}
		gameObject.SetActive (true);
	}

	private void ShowDate ()
	{
		int[] array = GetLimitTime (limitItem.end_at);
		dateField.text = string.Format ("{0}{1}{2}{3}", array[0], LanguageJP.DEVIDE, array[1], LanguageJP.PLEASE);
		date.SetActive (true);
	}

	private void ShowLimit ()
	{
		limitField.text = string.Format ("{0}{1}{2}{3}{4}", LanguageJP.RED_COLOR_PREFIX, limitItem.buy_count, LanguageJP.COLOR_SUFFIX, LanguageJP.DEVIDE, limitItem.limit_count);
		limit.SetActive (true);
	}

	private int[] GetLimitTime (int timeStamp)
	{
		DateTime dateTime = TimeUtil.TimestampToDateTime (timeStamp);
		return new int[] { dateTime.Month, dateTime.Day };
	}
}
