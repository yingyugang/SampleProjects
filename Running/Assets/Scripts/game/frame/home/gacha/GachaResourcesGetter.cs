using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using System.Collections.Generic;
using System.Linq;

public class GachaResourcesGetter : MonoBehaviour
{
	public UnityAction<List<GachaItem>,GachaResultInfo> unityAction;
	private GachaResultInfo gachaResultInfo;
	private List<CardInfo> cardInfoList;
	private int length;
	private List<GachaItem> gachaItemList;

	public void GetResources ()
	{
		gachaResultInfo = GachaResultInfo.GetInstance;
		cardInfoList = gachaResultInfo.cardinfo_list;

		StartCoroutine (CreateResources ());
	}

	private IEnumerator CreateResources ()
	{
		if (AssetBundleResourcesLoader.cardFrameThumbnailDictionary == null)
		{
			yield return StartCoroutine (ComponentConstant.ASSETBUNDLE_RESOURCES_GETTER.GetAllResourcesList<Texture2D> (AssetBundleName.card_thumbnail_frame.ToString (), (List<Texture2D> list) =>
			{
				AssetBundleResourcesLoader.cardFrameThumbnailDictionary = TextureToSpriteConverter.ConvertToSpriteDictionary (list);
			}, false));
		}
		StartCoroutine (CreateSpriteList ());
	}

	private IEnumerator CreateSpriteList ()
	{
		gachaItemList = new List<GachaItem> ();
		length = cardInfoList.Count;
		for (int i = 0; i < length; i++) {
			CardCSVStructure cardCSVStructure = MasterCSV.cardCSV.FirstOrDefault (result => result.id == cardInfoList [i].m_card_id);
			string imageResourceName = string.Format ("{0}{1}", int.Parse (cardCSVStructure.image_resource).ToString (LanguageJP.FOUR_MASK), LanguageJP.CARD_THUMBNAIL_SUFFIX);
			string cardResourceName = cardCSVStructure.image_resource;
			Sprite borderSprite = AssetBundleResourcesLoader.cardFrameThumbnailDictionary [string.Format ("{0}{1}", cardCSVStructure.rarity, LanguageJP.CARD_THUMBNAIL_SUFFIX)];
			Sprite rateSprite = AssetBundleResourcesLoader.cardFrameThumbnailDictionary[string.Format ("{0}{1}", cardCSVStructure.rarity, LanguageJP.CARD_RATE_SUFFIX)];
			yield return StartCoroutine (ComponentConstant.ASSETBUNDLE_RESOURCES_GETTER.GetResource<Texture2D> (AssetBundleName.card_thumbnail.ToString (), imageResourceName, (Texture2D texture2D) => {
				Sprite cardSprite = TextureToSpriteConverter.ConvertToSprite (texture2D);
				CardInfo cardInfo = cardInfoList [i];
				gachaItemList.Add (new GachaItem () {
					id = cardInfo.m_card_id,
					isNew = cardInfo.is_new,
					ball_color = cardInfo.ball_color,
					ball_color_show = cardInfo.ball_color_show,
					cardItem = new CardItem () {
						cardSprite = cardSprite,
						borderSprite = borderSprite,
						rateSprite = rateSprite,
						card_image_resource = cardResourceName,
						rarity = cardCSVStructure.rarity
					}
				});
				if (i == length - 1) {
					if (unityAction != null) {
						unityAction (gachaItemList, gachaResultInfo);
					}
				}
			}, false));
		}
	}
}
