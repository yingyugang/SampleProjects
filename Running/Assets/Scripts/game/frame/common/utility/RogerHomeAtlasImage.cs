using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class RogerHomeAtlasImage : RogerImage
{
    private void OnEnable ()
    {
        if (AssetBundleResourcesLoader.homeAtlasAssetBundle != null)
        {
            image.sprite = Instantiate<Sprite> (AssetBundleResourcesLoader.homeAtlasAssetBundle.LoadAsset<Sprite> (resourceName));
            image.enabled = isAutoEnable;
        }
    }
}
