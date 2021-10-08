using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

public class MainMediator : SceneInitializer
{
	public Main main;
	public MainPageMediator mainPageMediator;
	public Dictionary<string,Sprite> menuDictionary;
	public AssetCSVReader assetCSVReader;
	public LocalCSVParser localCSVParser;
	private LongLoadingMediator longLoadingMediator;
	public AssetBundleResourcesLoader assetBundleResourcesLoader;

	private void Awake ()
	{
		if (ComponentConstant.SCENE_COMBINE_MANAGER != null) {
			ComponentConstant.SCENE_COMBINE_MANAGER.unityAction = (LongLoadingMediator longLoadingMediator) => {
				this.longLoadingMediator = longLoadingMediator;
				ComponentConstant.SCENE_COMBINE_MANAGER.unityAction = null;
			};
		}
	}

	private void HideLoading ()
	{
		if (longLoadingMediator != null) {
			longLoadingMediator.Hide ();
		}
	}

	override protected IEnumerator CheckSceneResources ()
	{
		StartCoroutine (GetResources ());
		yield return null;
	}

	private IEnumerator GetResources ()
	{
		StartCoroutine (assetBundleResourcesLoader.LoadForCommon ());
		if (menuDictionary == null) {
			yield return StartCoroutine (ComponentConstant.ASSETBUNDLE_RESOURCES_GETTER.GetAllResourcesList<Texture2D> (AssetBundleName.menu_local.ToString (), (List<Texture2D> list) => {
				menuDictionary = TextureToSpriteConverter.ConvertToSpriteDictionary (list);
			}, false, true));
		}

		if (AssetBundleResourcesLoader.popupDictionary == null) {
			yield return StartCoroutine (ComponentConstant.ASSETBUNDLE_RESOURCES_GETTER.GetAllResourcesList<Texture2D> (AssetBundleName.popup_local.ToString (), (List<Texture2D> list) => {
				AssetBundleResourcesLoader.popupDictionary = TextureToSpriteConverter.ConvertToSpriteDictionary (list);
			}, true, true));
		}
		if (!GameConstant.hasDownloaded) {
			StartCoroutine (ComponentConstant.ASSETBUNDLE_RESOURCES_GETTER.GetAssetBundle (AssetBundleName.csv_local.ToString (), ParseLocalCSV, false, true));
		} else {
			InitMain ();
		}
	}

	private void ParseLocalCSV (AssetBundle assetBundle)
	{
		#if !READ_LOCAL_CSV
		assetCSVReader.Read (assetBundle);
		#endif
		localCSVParser.Parse ();
		StartCoroutine (GetAudioResources ());
	}

	private IEnumerator GetAudioResources ()
	{
		yield return StartCoroutine (ComponentConstant.ASSETBUNDLE_RESOURCES_GETTER.GetAssetBundle (AssetBundleName.sound_local.ToString (), GetAudioResourcesComplete, false, true));
	}

	private void GetAudioResourcesComplete (AssetBundle assetBundle)
	{
		InitMain ();
	}

	private void InitMain ()
	{
		Invoke ("HideLoading", 2);
		mainPageMediator.Init ();
	}
}
