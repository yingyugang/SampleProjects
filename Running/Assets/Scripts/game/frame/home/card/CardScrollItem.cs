using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Linq;
using System.Collections.Generic;

public class CardScrollItem : RogerInteractiveScrollItem
{
	public UnityAction<int, UnityAction<CardCSVStructure>> updateData;
	public UnityAction<CardCSVStructure> callBack;
	public CardCSVStructure cardCSVStructure;
	public Image unknownImage;
	public Image unknownFrame;
	public Image unknownRate;
	public Image image;
	public Image frame;
	public Image rate;
	private int index;
	public CardGameIcon cardGameIcon;
	public GameObject skill;
	public GameObject printImage;

	public void InitData (CardCSVStructure cardCSVStructure)
	{
		this.cardCSVStructure = cardCSVStructure;
		unknownImage.sprite = AssetBundleResourcesLoader.cardFrameThumbnailDictionary[GameConstant.UNKNOWN_IMAGE_NAME];
		unknownFrame.sprite = AssetBundleResourcesLoader.cardFrameThumbnailDictionary[string.Format ("{0}{1}", cardCSVStructure.rarity, LanguageJP.CARD_THUMBNAIL_SUFFIX)];
		unknownRate.sprite = AssetBundleResourcesLoader.cardFrameThumbnailDictionary[string.Format ("{0}{1}", cardCSVStructure.rarity, LanguageJP.CARD_RATE_SUFFIX)];
		CreateImage (index);
		printImage.SetActive (cardCSVStructure.can_print == 1);
	}

	protected override void UpdateData (int index)
	{
		this.index = index;
		if (updateData != null)
		{
			updateData (index, InitData);
		}
	}

	private void CreateImage (int index)
	{
		Card card = UpdateInformation.GetInstance.card_list.FirstOrDefault (result => cardCSVStructure.id == result.m_card_id);
		if (card != null)
		{
			StartCoroutine (ComponentConstant.ASSETBUNDLE_RESOURCES_GETTER.GetResource<Texture2D> (AssetBundleName.card_thumbnail.ToString (), int.Parse (cardCSVStructure.image_resource).ToString (LanguageJP.FOUR_MASK) + LanguageJP.CARD_THUMBNAIL_SUFFIX, GetResource<Texture2D>, false));
			frame.sprite = AssetBundleResourcesLoader.cardFrameThumbnailDictionary[string.Format ("{0}{1}", cardCSVStructure.rarity, LanguageJP.CARD_THUMBNAIL_SUFFIX)];
			rate.sprite = AssetBundleResourcesLoader.cardFrameThumbnailDictionary[string.Format ("{0}{1}", cardCSVStructure.rarity, LanguageJP.CARD_RATE_SUFFIX)];
		}
		else {
			image.sprite = null;
			frame.sprite = null;
			rate.sprite = null;
			image.gameObject.SetActive (false);
		}
		SetIcon (cardCSVStructure, card != null);
	}

	private void SetIcon (CardCSVStructure cardCSVStructure, bool hasOwn)
	{
		skill.SetActive (CheatController.GetInstance ().cheatCardIDList.Contains (cardCSVStructure.id));
		if (cardCSVStructure.up_type == 0)
		{
			cardGameIcon.gameObject.SetActive (false);
			return;
		}
		int up_type = cardCSVStructure.up_type;
		cardGameIcon.SetIcon (true, hasOwn, up_type == 1 ? 0 : cardCSVStructure.up_game_id, cardCSVStructure.up_value, up_type);
	}

	private void GetResource<T> (T t)
	{
		image.sprite = TextureToSpriteConverter.ConvertToSprite (t as Texture2D);
		image.gameObject.SetActive (true);
	}

	protected override void OnDestroy ()
	{
		base.OnDestroy ();
		updateData = null;
		callBack = null;
	}
}
