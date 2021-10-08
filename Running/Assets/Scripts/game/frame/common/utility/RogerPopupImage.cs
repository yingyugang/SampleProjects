using UnityEngine;
using System.Collections;

public class RogerPopupImage : RogerImage
{
	private void OnEnable ()
	{
		if (AssetBundleResourcesLoader.popupDictionary != null) {
			image.sprite = AssetBundleResourcesLoader.popupDictionary [resourceName];
			image.enabled = isAutoEnable;
		}
	}
}
