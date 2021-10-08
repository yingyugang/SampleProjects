using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using System;
using System.Collections.Generic;

public class SceneCombineManager : MonoBehaviour
{
	public LongLoadingMediator longLoadingMediator;
	public UnityAction<LongLoadingMediator> unityAction;

	public void LoadScene (SceneEnum sceneEnum, bool needLoading = true)
	{
		StartCoroutine (Load (sceneEnum, needLoading));
	}

	private IEnumerator Load (SceneEnum sceneEnum, bool needLoading)
	{
		AssetBundleResourcesLoader.Dispose ();
		Clean ();
		if (needLoading) {
			longLoadingMediator.Show ();
		}
		AsyncOperation asyncOperation = SceneManager.LoadSceneAsync (sceneEnum.ToString ());
		while (!asyncOperation.isDone) {
			yield return null;
		}
		if (unityAction != null) {
			unityAction (longLoadingMediator);
		} else {
			longLoadingMediator.Hide ();
		}
	}

	private void Clean ()
	{
		RogerImageSpriteCleaner.Clean ();
		AssetBundlePool.DisposeAllExcept (new List<string> () {
			AssetBundleName.game_icon.ToString(),
            AssetBundleName.huge.ToString (),
            AssetBundleName.sound.ToString (),
            AssetBundleName.common.ToString ()
		}, true);
		ComponentConstant.SOUND_MANAGER.Clear ();
		Resources.UnloadUnusedAssets ();
		GC.Collect ();
	}
}
