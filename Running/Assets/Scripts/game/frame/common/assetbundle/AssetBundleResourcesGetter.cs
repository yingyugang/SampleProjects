using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

public class AssetBundleResourcesGetter : MonoBehaviour
{
	public AssetBundleLoader assetbundleLoader;

	public IEnumerator GetResourcesList<T> (string assetBundleName, List<string> list, UnityAction<List<T>> unityAction = null, bool needToUnload = true, bool isStreamingAssetsPath = false, string suffix = LanguageJP.ASSETBUNDLE_SUFFIX) where T : Object
	{
		yield return StartCoroutine (Get (assetBundleName, (AssetBundle assetBundle) => {
			StartCoroutine (WaitForGetResourcesList<T> (assetBundle, assetBundleName, list, unityAction, needToUnload, isStreamingAssetsPath));
		}, isStreamingAssetsPath, suffix));
	}

	private IEnumerator WaitForGetResourcesList<T> (AssetBundle assetBundle, string assetBundleName, List<string> list, UnityAction<List<T>> unityAction = null, bool needToUnload = true, bool isStreamingAssetsPath = false) where T : Object
	{
		List<T> newList = new List<T> ();
		int length = list.Count;
		for (int i = 0; i < length; i++) {
			T t = assetBundle.LoadAsset<T> (list [i]);
			newList.Add (t);
			yield return null;
		}
		if (unityAction != null) {
			unityAction (newList);
		}
		ReleaseOrStore (assetBundleName, assetBundle, needToUnload);
	}

	public IEnumerator GetResourcesDictionary<T> (string assetBundleName, List<string> list, UnityAction<Dictionary<string,T>> unityAction = null, bool needToUnload = true, bool isStreamingAssetsPath = false, string suffix = LanguageJP.ASSETBUNDLE_SUFFIX) where T : Object
	{
		yield return StartCoroutine (Get (assetBundleName, (AssetBundle assetBundle) => {
			StartCoroutine (WaitForGetResourcesDictionary<T> (assetBundle, assetBundleName, list, unityAction, needToUnload, isStreamingAssetsPath));
		}, isStreamingAssetsPath, suffix));
	}

	private IEnumerator WaitForGetResourcesDictionary<T> (AssetBundle assetBundle, string assetBundleName, List<string> list, UnityAction<Dictionary<string,T>> unityAction = null, bool needToUnload = true, bool isStreamingAssetsPath = false) where T : Object
	{
		Dictionary<string,T> dictionary = new Dictionary<string, T> ();
		int length = list.Count;
		for (int i = 0; i < length; i++) {
			T t = assetBundle.LoadAsset<T> (list [i]);
			Debug.Log (t);
			dictionary.Add (t.name, t);
			yield return null;
		}
		if (unityAction != null) {
			unityAction (dictionary);
		}
		ReleaseOrStore (assetBundleName, assetBundle, needToUnload);
	}

	public IEnumerator GetResource<T> (string assetBundleName, string resourceName, UnityAction<T> unityAction = null, bool needToUnload = true, bool isStreamingAssetsPath = false, string suffix = LanguageJP.ASSETBUNDLE_SUFFIX) where T : Object
	{
		yield return StartCoroutine (Get (assetBundleName, (AssetBundle assetBundle) => {
			StartCoroutine (WaitForGetResource<T> (assetBundle, assetBundleName, resourceName, unityAction, needToUnload, isStreamingAssetsPath));
		}, isStreamingAssetsPath, suffix));
	}

	private IEnumerator WaitForGetResource<T> (AssetBundle assetBundle, string assetBundleName, string resourceName, UnityAction<T> unityAction = null, bool needToUnload = true, bool isStreamingAssetsPath = false) where T : Object
	{
		T t = assetBundle.LoadAsset<T> (resourceName);
		if (unityAction != null) {
			unityAction (t);
		}
		ReleaseOrStore (assetBundleName, assetBundle, needToUnload);
		yield return null;
	}

	public IEnumerator GetAllResourcesList<T> (string assetBundleName, UnityAction<List<T>> unityAction = null, bool needToUnload = true, bool isStreamingAssetsPath = false) where T : Object
	{
		yield return StartCoroutine (GetAllAssets<T> (assetBundleName, (Object[] array) => {
			List<T> list = new List<T> ();
			int length = array.Length;
			for (int i = 0; i < length; i++) {
				list.Add (array [i] as T);
			}
			if (unityAction != null) {
				unityAction (list);
			}
		}, needToUnload, isStreamingAssetsPath));
	}

	public IEnumerator GetAllResourcesDictionary<T> (string assetBundleName, UnityAction<Dictionary<string,T>> unityAction = null, bool needToUnload = true, bool isStreamingAssetsPath = false) where T : Object
	{
		yield return StartCoroutine (GetAllAssets<T> (assetBundleName, (Object[] array) => {
			Dictionary<string,T> dictionary = new Dictionary<string, T> ();
			int length = array.Length;
			for (int i = 0; i < length; i++) {
				dictionary.Add (array [i].name, array [i] as T);
			}
			if (unityAction != null) {
				unityAction (dictionary);
			}
		}, needToUnload, isStreamingAssetsPath));
	}

	private IEnumerator GetAllAssets<T> (string assetBundleName, UnityAction<Object[]> unityAction = null, bool needToUnload = true, bool isStreamingAssetsPath = false, string suffix = LanguageJP.ASSETBUNDLE_SUFFIX) where T : Object
	{
		yield return StartCoroutine (Get (assetBundleName, (AssetBundle assetBundle) => {
			StartCoroutine (WaitForGetAllAssets<T> (assetBundle, assetBundleName, unityAction, needToUnload));
		}, isStreamingAssetsPath, suffix));
	}

	private IEnumerator WaitForGetAllAssets<T> (AssetBundle assetBundle, string assetBundleName, UnityAction<Object[]> unityAction = null, bool needToUnload = true) where T : Object
	{
		T[] t = assetBundle.LoadAllAssets<T> ();
		if (unityAction != null) {
			unityAction (t);
		}
		ReleaseOrStore (assetBundleName, assetBundle, needToUnload);
		yield return null;
	}

	private IEnumerator Get (string assetBundleName, UnityAction<AssetBundle> unityAction = null, bool isStreamingAssetsPath = false, string suffix = LanguageJP.ASSETBUNDLE_SUFFIX)
	{
		AssetBundle assetBundle = AssetBundlePool.Get (assetBundleName);
		if (assetBundle != null) {
			unityAction (assetBundle);
		} else {
			yield return StartCoroutine (assetbundleLoader.Load (assetBundleName, unityAction, isStreamingAssetsPath, suffix));

		}
	}

	public IEnumerator GetAssetBundle (string assetBundleName, UnityAction<AssetBundle> unityAction, bool needToUnload = true, bool isStreamingAssetsPath = false, string suffix = LanguageJP.ASSETBUNDLE_SUFFIX)
	{
		yield return StartCoroutine (Get (assetBundleName, (AssetBundle assetBundle) => {
			if (unityAction != null) {
				unityAction (assetBundle);
			}
			ReleaseOrStore (assetBundleName, assetBundle, needToUnload);
		}, isStreamingAssetsPath, suffix));
	}

	private void ReleaseOrStore (string assetBundleName, AssetBundle assetBundle, bool needToUnload)
	{
		if (needToUnload) {
			assetBundle.Unload (false);
			AssetBundlePool.Dispose (assetBundleName);
		} else {
			AssetBundlePool.Add (assetBundleName, assetBundle);
		}
	}

	public IEnumerator GetScene (string sceneName, UnityAction<AssetBundle> unityAction, string suffix = LanguageJP.ASSETBUNDLE_SCENE_SUFFIX)
	{
		yield return StartCoroutine (assetbundleLoader.Load (sceneName, unityAction, false, suffix));
	}
}