using UnityEngine;
using System.Collections;

public class RogerHugeImage : RogerImage
{
	private void OnEnable ()
	{
		if (AssetBundleResourcesLoader.hugeDictionary != null) {
			image.sprite = AssetBundleResourcesLoader.hugeDictionary [resourceName];
			image.enabled = isAutoEnable;
		}
	}
}
