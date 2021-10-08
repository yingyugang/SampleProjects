using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class RogerCommonAtlasImage : RogerImage
{
    private void OnEnable ()
    {
        if (AssetBundleResourcesLoader.commonAtlasAssetBundle != null)
        {
            image.sprite = Instantiate<Sprite> (AssetBundleResourcesLoader.commonAtlasAssetBundle.LoadAsset<Sprite> (resourceName));
            image.enabled = isAutoEnable;
        }
    }
}
