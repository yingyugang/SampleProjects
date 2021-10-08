using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class GameCountDown : PopupContentWithDefaultAction
{
	public Image[] numbers;
	public AssetBundleResourcesLoader assetBundleResourcesLoader;
	public HeaderSpriteGetter headerSpriteGetter;

	private void OnEnable ()
	{
		StartCoroutine (GetResources ());
	}

	private IEnumerator GetResources ()
	{
		yield return StartCoroutine (ComponentConstant.ASSETBUNDLE_RESOURCES_GETTER.GetAllResourcesList<Texture2D> (AssetBundleName.game_countdown.ToString (), (List<Texture2D> list) => {
			List<Sprite> gameCountDownList = TextureToSpriteConverter.ConvertToSpriteList (list);
			int length = numbers.Length;
			for (int i = 0; i < length; i++) {
				numbers [i].sprite = gameCountDownList [i];
			}
		}, false));

		headerSpriteGetter.GetSprite (GetResource);
	}

	private void GetResource (Sprite sprite, Sprite frame, Sprite rate)
	{
		GameConstant.headerSprite = sprite;
		GameConstant.headerFrame = frame;
		GameConstant.headerRate = rate;
	}

	public void SetNumber (int number)
	{
		int length = numbers.Length;
		for (int i = 0; i < length; i++) {
			numbers [i].gameObject.SetActive (false);
		}
		numbers [number].gameObject.SetActive (true);
	}
}
