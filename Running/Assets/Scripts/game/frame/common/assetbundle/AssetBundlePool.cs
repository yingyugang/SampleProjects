using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class AssetBundlePool
{
	private static Dictionary<string,AssetBundle> assetBundlePool = new Dictionary<string,AssetBundle> ();

	public static AssetBundle Get (string str)
	{
		if (assetBundlePool.ContainsKey (str)) {
			return assetBundlePool [str];
		} else {
			return null;
		}
	}

	public static void Add (string str, AssetBundle assetBundle)
	{
		if (!assetBundlePool.ContainsKey (str)) {
			assetBundlePool.Add (str, assetBundle);
		}
	}

	public static void Dispose (string assetBundleName, bool unloadAllLoadedObjects = false)
	{
		if (assetBundlePool.ContainsKey (assetBundleName)) {
			AssetBundle assetBundle = assetBundlePool [assetBundleName];
			assetBundlePool.Remove (assetBundleName);
			if (assetBundle != null) {
				assetBundle.Unload (unloadAllLoadedObjects);
			}
		}
	}

	public static void DisposeAll (bool unloadAllLoadedObjects = false)
	{
		foreach (AssetBundle assetBundle in assetBundlePool.Values) {
			if (assetBundle != null) {
				assetBundle.Unload (unloadAllLoadedObjects);
			}
		}
		assetBundlePool.Clear ();
	}

	public static void DisposeAllExcept (List<string> exceptList, bool unloadAllLoadedObjects = false)
	{
		List<string> list = new List<string> ();
		foreach (KeyValuePair<string,AssetBundle> keyValuePair in assetBundlePool) {
			if (!exceptList.Contains (keyValuePair.Key)) {
				list.Add (keyValuePair.Key);
			}
		}

		int length = list.Count;
		for (int i = 0; i < length; i++) {
			Dispose (list [i], unloadAllLoadedObjects);
		}
	}
}