using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class ProductScrollItem : RogerInteractiveScrollItem
{
	[HideInInspector]
	public int id;
	public Image iconField;
	public Text nameField;
	public Text numberField;
	public Text priceField;
	public Text extraNumberField;
	public GameObject specialBg;
	public Text specialNumberField;
	public GameObject balloon;
	[HideInInspector]
	public string productName;
	[HideInInspector]
	public int price;

	public void Show (string imageResource, string name, int coinNumber, int extraCoinNumber, int price, bool isSpecial, int currentSpecialNumber, int totalSpecialNumber)
	{
		iconField.sprite = AssetBundleResourcesLoader.goldIconDictionary [imageResource];
		productName = name;
		//productName = name;
		this.price = price;
		priceField.text = string.Format ("{0}{1}", LanguageJP.YAN, price.ToString ());

		nameField.text = name;
		if (isSpecial) {
			numberField.text = string.Format ("{0}{1}", coinNumber.ToString (), LanguageJP.M);
			int[] array = GetLimitTime (extraCoinNumber);
			extraNumberField.text = string.Format ("{0}{1}{2}{3}", array [0], LanguageJP.DEVIDE, array [1], LanguageJP.PLEASE);
			specialBg.SetActive (true);
			specialNumberField.text = string.Format ("{0}{1}{2}{3}{4}", LanguageJP.RED_COLOR_PREFIX, currentSpecialNumber, LanguageJP.COLOR_SUFFIX, LanguageJP.DEVIDE, totalSpecialNumber);
		} else {
			numberField.text = string.Format ("{0}{1}", coinNumber + extraCoinNumber, LanguageJP.M);
			if (extraCoinNumber != 0) {
				extraNumberField.text = string.Format ("{0}{1}", extraCoinNumber.ToString (), LanguageJP.EXTRA_GET);
			} else {
				extraNumberField.gameObject.SetActive (false);
				balloon.SetActive (false);
			}
		}
		gameObject.SetActive (true);
	}

	private int[] GetLimitTime (int timeStamp)
	{
		DateTime dateTime = TimeUtil.TimestampToDateTime (timeStamp);
		return new int[]{ dateTime.Month, dateTime.Day };
	}
}
