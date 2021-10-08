using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using UnityEngine.UI;
using System.Collections.Generic;

public class NoEnoughPoint : PopupContentWithDefaultAction
{
	public Text own;
	public Text cost;
	public Image man;
	public List<Image> ownPointList;
	public List<Image> costPointList;

	private void OnEnable ()
	{
		StartCoroutine (ComponentConstant.ASSETBUNDLE_RESOURCES_GETTER.GetResource<Texture2D> (AssetBundleName.confirm_dialogue.ToString (), "0", GetResource<Texture2D>, false));
	}

	public void ShowIcon (int costType)
	{
		int length = ownPointList.Count;

		for (int i = 0; i < length; i++) {
			ownPointList [i].gameObject.SetActive (false);
			costPointList [i].gameObject.SetActive (false);
		}
		ownPointList [costType].gameObject.SetActive (true);
		costPointList [costType].gameObject.SetActive (true);
	}

	private void GetResource<T> (T t)
	{
		man.sprite = TextureToSpriteConverter.ConvertToSprite (t as Texture2D);
		man.gameObject.SetActive (true);
	}

	protected override void AddActions ()
	{
		unityActionArray = new UnityAction[] {
			() => {
				Send (0);
			}
		};
	}
}
