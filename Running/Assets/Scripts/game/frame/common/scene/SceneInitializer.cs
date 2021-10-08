using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SceneInitializer : MonoBehaviour
{
	virtual protected void Init ()
	{
		
	}

	private IEnumerator Start ()
	{
		if (GameObject.Find ("Coroutine") == null) {
			GameInitialization gameInitialization = GameObject.Instantiate<GameInitialization> (Resources.Load<GameInitialization> (PathConstant.CLIENT_COROUTINE_PATH));
			gameInitialization.gameObject.name = "Coroutine";
			gameInitialization.Init ();
		}

		yield return StartCoroutine (CheckImportantResources ());
		yield return StartCoroutine (CheckSceneResources ());
	}

	virtual protected IEnumerator CheckImportantResources ()
	{
		if (AssetBundleResourcesLoader.loadingLocalDictionary == null) {
			yield return StartCoroutine (ComponentConstant.ASSETBUNDLE_RESOURCES_GETTER.GetAllResourcesList<Texture2D> (AssetBundleName.loading_local.ToString (), (List<Texture2D> list) => {
				AssetBundleResourcesLoader.loadingLocalDictionary = TextureToSpriteConverter.ConvertToSpriteDictionary (list);
			}, true, true));
		}
	}

	virtual protected IEnumerator CheckSceneResources ()
	{
		yield return null;
	}
}
