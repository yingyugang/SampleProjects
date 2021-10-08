using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

public class AssetBundleResourcesLoader : MonoBehaviour
{
	public static Dictionary<string,Sprite> cardFrameThumbnailDictionary;
	public static List<Sprite> cardFrameDetailList;
	public static Dictionary<string,Sprite> goldIconDictionary;
	public static Dictionary<string,Sprite> gameIconDictionary;
	public static Dictionary<string,Sprite> itemIconDictionary;
	public static Dictionary<string,Sprite> loadingLocalDictionary;
	public static Dictionary<string,Sprite> hugeDictionary;
	public static Dictionary<string,Sprite> popupDictionary;
	public static Dictionary<string,Sprite> gachaBannerDictionary;
	public static Dictionary<string,Sprite> oilking_Dictionary; //hoantt
	public static AssetBundle homeAtlasAssetBundle;
	public static AssetBundle commonAtlasAssetBundle;
	public static AssetBundle oilkingAtlasAssetBundle; //hoantt

	public UnityAction unityAction;

	public IEnumerator LoadForHome ()
	{
		yield return StartCoroutine (ComponentConstant.ASSETBUNDLE_RESOURCES_GETTER.GetAssetBundle (AssetBundleName.card_thumbnail.ToString (), null, false));
		if (gachaBannerDictionary == null) {
			yield return StartCoroutine (ComponentConstant.ASSETBUNDLE_RESOURCES_GETTER.GetAllResourcesList<Texture2D> (AssetBundleName.gacha_banner.ToString (), (List<Texture2D> list) => {
				gachaBannerDictionary = TextureToSpriteConverter.ConvertToSpriteDictionary (list);
			}));
		}
		if (cardFrameThumbnailDictionary == null) {
			yield return StartCoroutine (ComponentConstant.ASSETBUNDLE_RESOURCES_GETTER.GetAllResourcesList<Texture2D> (AssetBundleName.card_thumbnail_frame.ToString (), (List<Texture2D> list) => {
				cardFrameThumbnailDictionary = TextureToSpriteConverter.ConvertToSpriteDictionary (list);
			}));
		}

		yield return StartCoroutine (ComponentConstant.ASSETBUNDLE_RESOURCES_GETTER.GetAssetBundle (AssetBundleName.home.ToString (), (AssetBundle assetBundle) => {
			homeAtlasAssetBundle = assetBundle;
		}, false, false, string.Empty));

        yield return StartCoroutine (LoadCommon ());
		yield return StartCoroutine (LoadForOilkingGame ());

		if (goldIconDictionary == null) {
			yield return StartCoroutine (ComponentConstant.ASSETBUNDLE_RESOURCES_GETTER.GetAllResourcesList<Texture2D> (AssetBundleName.gold_icon.ToString (), (List<Texture2D> list) => {
				goldIconDictionary = TextureToSpriteConverter.ConvertToSpriteDictionary (list);
			}, false));
		}

		if (itemIconDictionary == null) {
			yield return StartCoroutine (ComponentConstant.ASSETBUNDLE_RESOURCES_GETTER.GetAllResourcesList<Texture2D> (AssetBundleName.item_icon.ToString (), (List<Texture2D> list) => {
				itemIconDictionary = TextureToSpriteConverter.ConvertToSpriteDictionary (list);
			}, false));
		}

		if (unityAction != null) {
			unityAction ();
		}
	}

	public IEnumerator LoadForCommon ()
	{
		yield return StartCoroutine (LoadCommon ());

		if (unityAction != null) {
			unityAction ();
		}
	}

	public IEnumerator LoadCommon ()
	{
		if (FileManager.Exists(PathConstant.CLIENT_ASSETS_PATH + AssetBundleName.common.ToString()))
		{
			yield return StartCoroutine (ComponentConstant.ASSETBUNDLE_RESOURCES_GETTER.GetAssetBundle (AssetBundleName.common.ToString (), (AssetBundle assetBundle) => {
				commonAtlasAssetBundle = assetBundle;
			}, false, false, string.Empty));
		}

		if (cardFrameThumbnailDictionary == null && FileManager.Exists (PathConstant.CLIENT_ASSETS_PATH + AssetBundleName.card_thumbnail_frame.ToString () + LanguageJP.ASSETBUNDLE_SUFFIX)) {
			yield return StartCoroutine (ComponentConstant.ASSETBUNDLE_RESOURCES_GETTER.GetAllResourcesList<Texture2D> (AssetBundleName.card_thumbnail_frame.ToString (), (List<Texture2D> list) => {
				cardFrameThumbnailDictionary = TextureToSpriteConverter.ConvertToSpriteDictionary (list);
			}, false));
		}

		if (cardFrameDetailList == null && FileManager.Exists (PathConstant.CLIENT_ASSETS_PATH + AssetBundleName.card_detail_frame.ToString () + LanguageJP.ASSETBUNDLE_SUFFIX)) {
			yield return StartCoroutine (ComponentConstant.ASSETBUNDLE_RESOURCES_GETTER.GetAllResourcesList<Texture2D> (AssetBundleName.card_detail_frame.ToString (), (List<Texture2D> list) => {
				cardFrameDetailList = TextureToSpriteConverter.ConvertToSpriteList (list);
			}, false));
		}
			
        if (hugeDictionary == null && FileManager.Exists (PathConstant.CLIENT_ASSETS_PATH + AssetBundleName.huge.ToString () + LanguageJP.ASSETBUNDLE_SUFFIX))
        {
            yield return StartCoroutine (ComponentConstant.ASSETBUNDLE_RESOURCES_GETTER.GetAllResourcesList<Texture2D> (AssetBundleName.huge.ToString (), (List<Texture2D> list) => {
                hugeDictionary = TextureToSpriteConverter.ConvertToSpriteDictionary (list);
            }, false));
        }

		if (FileManager.Exists (PathConstant.CLIENT_ASSETS_PATH + AssetBundleName.game_icon.ToString () + LanguageJP.ASSETBUNDLE_SUFFIX))
		{
			yield return StartCoroutine (ComponentConstant.ASSETBUNDLE_RESOURCES_GETTER.GetAllResourcesList<Texture2D> (AssetBundleName.game_icon.ToString (), (List<Texture2D> list) => {
				gameIconDictionary = TextureToSpriteConverter.ConvertToSpriteDictionary (list);
			}, false));
			Debug.Log ("gameicon="+gameIconDictionary.Count);
		}
	}

	public IEnumerator LoadForOilkingGame ()
	{
		yield return StartCoroutine (LoadOilKingGame ());

		if (unityAction != null) {
			unityAction ();
		}

	}
	public IEnumerator LoadOilKingGame ()
	{
		if (FileManager.Exists(PathConstant.CLIENT_ASSETS_PATH + AssetBundleName.game_oilking.ToString()))
		{
			yield return StartCoroutine (ComponentConstant.ASSETBUNDLE_RESOURCES_GETTER.GetAssetBundle (AssetBundleName.game_oilking.ToString (), (AssetBundle assetBundle) => {
				oilkingAtlasAssetBundle = assetBundle;
			}, false, false, string.Empty));
		}

		if (oilking_Dictionary == null && FileManager.Exists (PathConstant.CLIENT_ASSETS_PATH + AssetBundleName.game_oilking.ToString () + LanguageJP.ASSETBUNDLE_SUFFIX)) {
			yield return StartCoroutine (ComponentConstant.ASSETBUNDLE_RESOURCES_GETTER.GetAllResourcesList<Texture2D> (AssetBundleName.game_oilking.ToString (), (List<Texture2D> list) => {
				oilking_Dictionary = TextureToSpriteConverter.ConvertToSpriteDictionary (list);
				Debug.Log ("load oilking asset="+oilking_Dictionary.Count);
			}, true));
		}else{
			Debug.Log (FileManager.Exists (PathConstant.CLIENT_ASSETS_PATH + AssetBundleName.game_oilking.ToString () + LanguageJP.ASSETBUNDLE_SUFFIX) 
				+"\n"+PathConstant.CLIENT_ASSETS_PATH + AssetBundleName.game_oilking.ToString () + LanguageJP.ASSETBUNDLE_SUFFIX);
		}
	}

	public static void Dispose ()
	{
		goldIconDictionary = null;
		itemIconDictionary = null;
		cardFrameThumbnailDictionary = null;
		cardFrameDetailList = null;
		gachaBannerDictionary = null;
	}
}
