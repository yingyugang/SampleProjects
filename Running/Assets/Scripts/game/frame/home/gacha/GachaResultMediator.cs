using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using System.Collections.Generic;
using System.Linq;

public class GachaResultMediator : ActivityMediator
{
	public RogerScrollGridWithText rogerScrollGridWithText;
	private List<GachaItem> gachaItemList;
	private GachaResult gachaResult;
	private List<string> nameList;

	private void OnEnable ()
	{
		gachaResult = viewWithDefaultAction as GachaResult;
		if (ChangeBadgeManager.isChangeTime && !ChangeBadgeManager.hasOccuredByGacha)
		{
			ComponentConstant.POPUP_LOADER.Popup (PopupEnum.ProbabilityValueChange);
			ChangeBadgeManager.hasOccuredByGacha = true;
			ChangeBadgeManager.isChangeTime = true;
		}
		rogerScrollGridWithText.onClick = (GachaItem gachaItem) =>
		{
			ComponentConstant.POPUP_LOADER.Popup (PopupEnum.CardShow, null, new List<object> () {
				gachaItem.cardItem.bigCardSprite,
				gachaItem.cardItem.bigBorderSprite
			});
		};
		rogerScrollGridWithText.onSelect = (GachaItem gachaItem) =>
		{
			CardCSVStructure cardCSVStructure = MasterCSV.cardCSV.FirstOrDefault (result => result.id == gachaItem.id);
			gachaResult.cardNumber.text = cardCSVStructure.number;
			gachaResult.cardName.text = cardCSVStructure.name;
			gachaResult.cardTitle.text = cardCSVStructure.title;
			gachaResult.cardDescription.text = cardCSVStructure.description;

			if (cardCSVStructure.up_type == 0)
			{
				gachaResult.cardSkill.gameObject.SetActive (false);
			}
			else if (cardCSVStructure.up_type == 1)
			{
				gachaResult.additionalField.text = string.Format (LanguageJP.CARD_DETAIL_TIME_DESCRIPTION, cardCSVStructure.up_value);
				gachaResult.gameIcon.sprite = AssetBundleResourcesLoader.gameIconDictionary[LanguageJP.TIME_ICON];
				gachaResult.gameIcon.gameObject.SetActive (true);
				gachaResult.gameObject.SetActive (true);
				gachaResult.cardSkill.gameObject.SetActive (true);
			}
			else if (cardCSVStructure.up_type == 2)
			{
				GameCSVStructure gameCSVStructure = MasterCSV.gameCSV.FirstOrDefault (result => result.id == cardCSVStructure.up_game_id);
				gachaResult.additionalField.text = string.Format (LanguageJP.CARD_DETAIL_CARD_DESCRIPTION, gameCSVStructure.name, cardCSVStructure.up_value);
				gachaResult.gameIcon.sprite = AssetBundleResourcesLoader.gameIconDictionary[string.Format ("{0}{1}", LanguageJP.GAME_ICON_PREFIX, cardCSVStructure.up_game_id)];
				gachaResult.gameIcon.gameObject.SetActive (true);
				gachaResult.gameObject.SetActive (true);
				gachaResult.cardSkill.gameObject.SetActive (true);
			}
		};
		rogerScrollGridWithText.number.SetActive (gachaItemList.Count > 1);
		
		ShowInfo ();
		StartCoroutine (GetCardResources (gachaItemList));
	}

	

	private IEnumerator GetCardResources (List<GachaItem> gachaItemList)
	{
		rogerScrollGridWithText.Init (gachaItemList);
		nameList = new List<string> ();
		int length = gachaItemList.Count;
		for (int i = 0; i < length; i++)
		{
			CardCSVStructure cardCSVStructure = MasterCSV.cardCSV.FirstOrDefault (result => int.Parse (result.image_resource).ToString (LanguageJP.FOUR_MASK) == int.Parse (gachaItemList[i].cardItem.card_image_resource).ToString (LanguageJP.FOUR_MASK));
			string assetBundleName = cardCSVStructure.assetbundle_name;
			if (!nameList.Contains (assetBundleName))
			{
				nameList.Add (assetBundleName);
			}
			yield return null;
		}
	}

	private void ShowInfo ()
	{
		gachaResult = viewWithDefaultAction as GachaResult;
		RecyclePoint oldRecyclePoint = UpdateInformation.GetInstance.recycle_pt.oldRecyclePoint;
		if (oldRecyclePoint == null)
		{
			oldRecyclePoint = new RecyclePoint ();
		}
		RecyclePoint recyclePoint = UpdateInformation.GetInstance.recycle_pt;
		int totalCard = MasterCSV.cardCSV.Count ();
		int previousTotalCard = GameConstant.lastNumOfCard;
        	int currentTotalCard = GameConstant.numOfCard;
		for (int i = 0; i < 5; i++) {
			int current = recyclePoint [i];
			int old = oldRecyclePoint [i];
			gachaResult.scoreTextList [i].text = string.Format ("{0}{1}{2}{3}{4}", old.ToString (LanguageJP.THREE_MASK), LanguageJP.PINK_COLOR_PREFIX, LanguageJP.PLUS, (current - old).ToString (LanguageJP.TWO_MASK), LanguageJP.COLOR_SUFFIX);
		}
		gachaResult.totalCoin.text = string.Format ("{0}{1}{2}{3}{4}{5}{6}{7}", previousTotalCard.ToString (LanguageJP.THREE_MASK), LanguageJP.RED_COLOR_PREFIX, LanguageJP.PLUS, (currentTotalCard - previousTotalCard).ToString (LanguageJP.TWO_MASK), LanguageJP.COLOR_SUFFIX, LanguageJP.DEVIDE, totalCard.ToString (LanguageJP.THREE_MASK), LanguageJP.COIN);
	}

	public void Clean ()
	{
		rogerScrollGridWithText.Reset ();
		AssetBundlePool.Dispose (AssetBundleName.GachaPanel.ToString (), true);
		int length = nameList.Count;
		for (int i = 0; i < length; i++)
		{
			AssetBundlePool.Dispose (nameList[i], true);
		}

		CleanText ();
	}

	private void CleanText ()
	{
		for (int i = 0; i < 5; i++)
		{
			gachaResult.scoreTextList[i].text = string.Empty;
		}
		gachaResult.totalCoin.text = string.Empty;

		gachaResult.cardNumber.text = string.Empty;
		gachaResult.cardName.text = string.Empty;
		gachaResult.cardTitle.text = string.Empty;
		gachaResult.cardDescription.text = string.Empty;
	}

	public void SetWindow (List<GachaItem> list)
	{
		gachaResult = viewWithDefaultAction as GachaResult;
		gachaItemList = list;
	}
}
