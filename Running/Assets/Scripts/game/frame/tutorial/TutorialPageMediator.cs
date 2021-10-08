using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using System.Collections.Generic;

public class TutorialPageMediator : PageMediator
{
	public Dictionary<string,Sprite> dictionary;
	new public UnityAction unityAction;
	public TutorialGachaMediator tutorialGachaMediator;
	public AssetBundleResourcesLoader assetBundleResourcesLoader;

	protected override void CheckResources ()
	{
		if (dictionary != null) {
			ShowWindow ();
		} else {
			StartCoroutine (GetResources ());
		}
	}

	protected override IEnumerator GetResources ()
	{
		yield return StartCoroutine (LoadResources ());
	}

	private IEnumerator LoadResources ()
	{
		yield return StartCoroutine (ComponentConstant.ASSETBUNDLE_RESOURCES_GETTER.GetAllResourcesList<Texture2D> (AssetBundleName.tutorial.ToString (), (List<Texture2D> list) => {
			dictionary = TextureToSpriteConverter.ConvertToSpriteDictionary (list);
		}, false));

		if (unityAction != null) {
			unityAction ();
		}
		ShowWindow ();
	}
}
