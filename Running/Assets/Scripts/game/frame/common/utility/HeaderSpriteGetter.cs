using UnityEngine;
using System.Collections;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections.Generic;

public class HeaderSpriteGetter : MonoBehaviour
{
	private UnityAction<Sprite,Sprite,Sprite> unityAction;
	private CardCSVStructure cardCSVStructure;

	public void GetSprite (UnityAction<Sprite,Sprite,Sprite> unityAction)
	{
		this.unityAction = unityAction;
		cardCSVStructure = MasterCSV.cardCSV.FirstOrDefault (result => int.Parse (Player.GetInstance.head_image).ToString (LanguageJP.FOUR_MASK) == int.Parse (result.image_resource).ToString (LanguageJP.FOUR_MASK));
		if (cardCSVStructure != null) {
			StartCoroutine (ComponentConstant.ASSETBUNDLE_RESOURCES_GETTER.GetResource<Texture2D> (AssetBundleName.card_thumbnail.ToString (), int.Parse (cardCSVStructure.image_resource).ToString (LanguageJP.FOUR_MASK) + LanguageJP.CARD_THUMBNAIL_SUFFIX, GetResource<Texture2D>, false));
		}
	}

	private void GetResource<T> (T t)
	{
		Sprite sprite = TextureToSpriteConverter.ConvertToSprite (t as Texture2D);
		if (unityAction != null) {
			if (AssetBundleResourcesLoader.cardFrameThumbnailDictionary == null) {
				StartCoroutine (GetFrameAndRate (sprite));
			} else {
				unityAction (sprite, AssetBundleResourcesLoader.cardFrameThumbnailDictionary [string.Format ("{0}{1}", cardCSVStructure.rarity, LanguageJP.CARD_THUMBNAIL_SUFFIX)], AssetBundleResourcesLoader.cardFrameThumbnailDictionary[string.Format ("{0}{1}", cardCSVStructure.rarity, LanguageJP.CARD_RATE_SUFFIX)]);
			}
		}
	}

	private IEnumerator GetFrameAndRate (Sprite sprite)
	{
		yield return StartCoroutine (ComponentConstant.ASSETBUNDLE_RESOURCES_GETTER.GetAllResourcesList<Texture2D> (AssetBundleName.card_thumbnail_frame.ToString (), (List<Texture2D> list) => {
			AssetBundleResourcesLoader.cardFrameThumbnailDictionary = TextureToSpriteConverter.ConvertToSpriteDictionary (list);
		}, false));

		if (unityAction != null)
		{
			unityAction (sprite, AssetBundleResourcesLoader.cardFrameThumbnailDictionary [string.Format ("{0}{1}", cardCSVStructure.rarity, LanguageJP.CARD_THUMBNAIL_SUFFIX)], AssetBundleResourcesLoader.cardFrameThumbnailDictionary[string.Format ("{0}{1}", cardCSVStructure.rarity, LanguageJP.CARD_RATE_SUFFIX)]);
		}
	}
}
