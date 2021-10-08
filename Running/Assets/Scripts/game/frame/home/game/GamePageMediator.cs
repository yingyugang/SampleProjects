using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

public class GamePageMediator : PageMediator
{
	public static Dictionary<int,Dictionary<string,Sprite>> gameIntroductionResourcesDirectory;
	public static Dictionary<int,List<Sprite>> gameAnimationResourcesDirectory;

	new public UnityAction unityAction;

	public void CheckRequiredResources ()
	{
		if (gameIntroductionResourcesDirectory != null && gameAnimationResourcesDirectory != null) {
			if (unityAction != null) {
				unityAction ();
			} else {
				pageNumber = 0;
				ShowWindow ();
			}
		} else {
			StartCoroutine (GetResources ());
		}
	}

	protected override IEnumerator GetResources ()
	{
		gameIntroductionResourcesDirectory = new Dictionary<int, Dictionary<string, Sprite>> ();
		gameAnimationResourcesDirectory = new Dictionary<int, List<Sprite>> ();
		List<GameDetail> gameDetailList = UpdateInformation.GetInstance.game_list;
		int length = gameDetailList.Count;
		for (int i = 1; i <= length; i++) {
			yield return StartCoroutine (CreateGameResources (i));
		}
		if (unityAction != null) {
			unityAction ();
		} else {
			ShowWindow ();
		}
	}

	private IEnumerator CreateGameResources (int id)
	{
		yield return StartCoroutine (ComponentConstant.ASSETBUNDLE_RESOURCES_GETTER.GetAllResourcesList<Texture2D> (string.Format ("{0}{1}", LanguageJP.GAME_INTRODUCTION_PREFIX, id), (List<Texture2D> list) => {
			gameIntroductionResourcesDirectory.Add (id, TextureToSpriteConverter.ConvertToSpriteDictionary (list));
		}, false));

		yield return StartCoroutine (ComponentConstant.ASSETBUNDLE_RESOURCES_GETTER.GetAllResourcesList<Texture2D> (string.Format ("{0}{1}", LanguageJP.GAME_ANIMATION_PREFIX, id), (List<Texture2D> list) => {
			gameAnimationResourcesDirectory.Add (id, TextureToSpriteConverter.ConvertToSpriteList (list));
		}, false));
	}

	private void OnDestroy ()
	{
		gameIntroductionResourcesDirectory = null;
		gameAnimationResourcesDirectory = null;
	}
}
