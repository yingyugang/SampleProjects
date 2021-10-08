using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using System.Linq;

public class PresentScrollItem : RogerInteractiveScrollItem
{
	[HideInInspector]
	public int id;
	public Text titleField;
	public Text timeField;
	public Text numberField;
	public Image iconField;
    public Image borderField;

	public void Show (MessageBox messageBox)
	{
		this.id = messageBox.id;
		titleField.text = messageBox.title;
		DateTime dateTime = TimeUtil.TimestampToDateTime (messageBox.end_at);
		timeField.text = string.Format ("{0}{1}{2}{3}{4}", LanguageJP.DEAD_LINE, TimeUtil.TimeStampToDateString (dateTime), " ", TimeUtil.TimeStampToHourAndMinuteString (dateTime), LanguageJP.PLEASE);
		numberField.text = string.Format ("{0}{1}", LanguageJP.X, messageBox.num);
		gameObject.SetActive (true);
		if (messageBox.reward_type == 1) {
			int m_card_id = messageBox.reward_id;
            CardCSVStructure currentCardCSVStructure = MasterCSV.cardCSV.FirstOrDefault (result => result.id == m_card_id);
            borderField.sprite = AssetBundleResourcesLoader.cardFrameThumbnailDictionary[string.Format ("{0}{1}", currentCardCSVStructure.rarity, LanguageJP.CARD_THUMBNAIL_SUFFIX)];
            borderField.gameObject.SetActive (true);
            StartCoroutine (ComponentConstant.ASSETBUNDLE_RESOURCES_GETTER.GetResource<Texture2D> (AssetBundleName.card_thumbnail.ToString (), int.Parse (currentCardCSVStructure.image_resource).ToString (LanguageJP.FOUR_MASK) + LanguageJP.CARD_THUMBNAIL_SUFFIX, GetResource<Texture2D>, false));
		} else if (messageBox.reward_type == 2) {
			borderField.gameObject.SetActive (false);
			int m_item_id = messageBox.reward_id;
			ItemCSVStructure currentItemCSVStructure = MasterCSV.itemCSV.FirstOrDefault (result => result.id == m_item_id);
			iconField.sprite = AssetBundleResourcesLoader.itemIconDictionary [currentItemCSVStructure.image_resource];
		}
	}

	private void GetResource<T> (T t)
	{
        iconField.sprite = TextureToSpriteConverter.ConvertToSprite (t as Texture2D);
        iconField.gameObject.SetActive (true);
	}
}
