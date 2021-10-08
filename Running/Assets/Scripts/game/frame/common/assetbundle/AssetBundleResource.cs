using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.Events;

public class AssetBundleResource : MonoBehaviour
{
	public UnityAction unityAction;

	public void GetResources (Dictionary<string,Image> imageDictionary, bool needToUnload = true, bool isStreamingAssetsPath = false)
	{
		Dictionary<string,Sprite> dictionary = new Dictionary<string, Sprite> ();
		List<string> list = imageDictionary.Keys.ToList ();

		StartCoroutine (ComponentConstant.ASSETBUNDLE_RESOURCES_GETTER.GetResourcesList<Texture2D> (AssetBundleName.loading_local.ToString (), list, (List<Texture2D> texture2dList) => {
			dictionary = TextureToSpriteConverter.ConvertToSpriteDictionary (texture2dList);
			int length = dictionary.Count;
			for (int i = 0; i < length; i++) {
				imageDictionary.Values.ElementAt (i).sprite = dictionary.Values.ElementAt (i);
			}
			if (unityAction != null) {
				unityAction ();
			}
		}, needToUnload, isStreamingAssetsPath));
	}
}
