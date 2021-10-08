using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using System.Linq;
using System;

public class GameReadyMediator : ActivityMediator
{
	private int id;
	private SceneEnum sceneEnum;
	private GameReady gameReady;

	protected override void CreateActions ()
	{
		unityActionArray = new UnityAction[] {
			() => {
				ComponentConstant.SOUND_MANAGER.Play (SoundEnum.se01_ok);
				PlayAnimation ();
			}
		};
	}

	public void SetWindow (int id, SceneEnum sceneEnum)
	{
		Debug.Log ("SetWindow id="+id+" sceneEnum="+sceneEnum.ToString ());
		this.id = id;
		this.sceneEnum = sceneEnum;

			gameReady = viewWithDefaultAction as GameReady;
			gameReady.readme.sprite = GamePageMediator.gameIntroductionResourcesDirectory [id] [LanguageJP.GAME_README];

	}

	private void PlayAnimation ()
	{
		gameReady.sizeAnimation.Play (OnComplete);
		gameReady.mask.rectTransform.SetAsLastSibling ();
		gameReady.mask.gameObject.SetActive (true);
		gameReady.blacks.SetActive (true);
	}

	private void OnComplete ()
	{
		UpdateInformation.GetInstance.recycle_pt.Clone ();
		GameConstant.gameDetail = UpdateInformation.GetInstance.game_list [id - 1];
//		LoadSceneAssetBundle ();
		setDockActive (true);
		GameConstant.isPlayingGame = true;
		ComponentConstant.SCREEN_MANAGER.ShowOrHideBorder (false);
		ComponentConstant.SCENE_COMBINE_MANAGER.LoadScene (sceneEnum);
	}

	private void LoadSceneAssetBundle ()
	{
		StartCoroutine (ComponentConstant.ASSETBUNDLE_RESOURCES_GETTER.GetScene (Enum.GetName (typeof(SceneEnum), id), (AssetBundle assetBundle) => {
			ComponentConstant.SCENE_COMBINE_MANAGER.LoadScene (sceneEnum);
		}));
	}
}
