using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using UnityEngine.UI;

public class NoEnoughOriginalPoint : PopupContentWithDefaultAction
{
	public Text own;
	public Text cost;
	public Image man;

	private void OnEnable ()
	{
		StartCoroutine (ComponentConstant.ASSETBUNDLE_RESOURCES_GETTER.GetResource<Texture2D> (AssetBundleName.confirm_dialogue.ToString (), "8", GetResource<Texture2D>, false));
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
