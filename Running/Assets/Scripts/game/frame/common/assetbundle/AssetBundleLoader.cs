using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class AssetBundleLoader : MonoBehaviour
{
	public IEnumerator Load (string assetBundleName, UnityAction<AssetBundle> unityAction, bool isStreamingAssetsPath = false, string suffix = LanguageJP.ASSETBUNDLE_SUFFIX)
	{
		if (isStreamingAssetsPath && Application.platform == RuntimePlatform.Android) {
			WWW www = new WWW (PathConstant.CLIENT_STREAMING_ASSETS_PATH + assetBundleName + suffix);
			yield return www;
			if (www.isDone && string.IsNullOrEmpty (www.error)) {
				unityAction (www.assetBundle);
			}
		} else {
			AssetBundle assetBundle = AssetBundle.LoadFromFile ((isStreamingAssetsPath ? PathConstant.CLIENT_STREAMING_ASSETS_PATH : PathConstant.CLIENT_ASSETS_PATH) + assetBundleName + suffix);
			unityAction (assetBundle);
		}
	}
}