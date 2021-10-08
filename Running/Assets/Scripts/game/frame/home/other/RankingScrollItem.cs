using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine.Events;

public class RankingScrollItem : RogerInteractiveScrollItem
{
	public UnityAction<int,UnityAction<RankingData>> updateData;
	public UnityAction<RankingData> callBack;
	public GameObject currentPlayer;
	public Image headerImage;
	public Image borderImage;
	public Image rateImage;
	public Text nameField;
	public Text pointField;
	public RankOrder rankOrder;
	private int index;

	virtual protected void InitData (RankingData rankingData)
	{
		if (rankingData == null) {
			return;
		}
		rankOrder.SetRankOrder (rankingData.rank);
		if (!string.IsNullOrEmpty (rankingData.head_image)) {
			CardCSVStructure cardCSVStructure = MasterCSV.cardCSV.FirstOrDefault (result => int.Parse (result.image_resource).ToString (LanguageJP.FOUR_MASK) == int.Parse (rankingData.head_image).ToString (LanguageJP.FOUR_MASK));
			if (cardCSVStructure != null) {
				StartCoroutine (ComponentConstant.ASSETBUNDLE_RESOURCES_GETTER.GetResource<Texture2D> (AssetBundleName.card_thumbnail.ToString (), int.Parse (rankingData.head_image).ToString (LanguageJP.FOUR_MASK) + LanguageJP.CARD_THUMBNAIL_SUFFIX, GetResource<Texture2D>, false));
				borderImage.sprite = AssetBundleResourcesLoader.cardFrameThumbnailDictionary [string.Format ("{0}{1}", cardCSVStructure.rarity, LanguageJP.CARD_THUMBNAIL_SUFFIX)];
				rateImage.sprite = AssetBundleResourcesLoader.cardFrameThumbnailDictionary[string.Format ("{0}{1}", cardCSVStructure.rarity, LanguageJP.CARD_RATE_SUFFIX)];
				ShowOrHide (true);
			} else {
				ShowOrHide (false);
			}
		} else {
			ShowOrHide (false);
		}
		nameField.text = rankingData.name;
		pointField.text = rankingData.score.ToString ();
		gameObject.SetActive (true);
		currentPlayer.SetActive (rankingData.player_id == Player.GetInstance.id);
	}

	private void ShowOrHide (bool isShow)
	{
		headerImage.gameObject.SetActive (isShow);
		borderImage.gameObject.SetActive (isShow);
	}

	private void GetResource<T> (T t)
	{
		headerImage.sprite = TextureToSpriteConverter.ConvertToSprite (t as Texture2D);
	}

	protected override void UpdateData (int index)
	{
		this.index = index;
		if (updateData != null) {
			updateData (index, InitData);
		}
	}

	protected override void OnDestroy ()
	{
		base.OnDestroy ();
		updateData = null;
		callBack = null;
	}
}
