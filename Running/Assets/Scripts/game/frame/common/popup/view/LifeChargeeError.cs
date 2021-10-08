using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LifeChargeeError : PopupContentWithDefaultAction
{
	public Text coin;
	public Image man;

	private void OnEnable ()
	{
		man.sprite = null;
		StartCoroutine (ComponentConstant.ASSETBUNDLE_RESOURCES_GETTER.GetResource<Texture2D> (AssetBundleName.confirm_dialogue.ToString (), Random.Range (1, 7).ToString (), GetResource<Texture2D>, false));
	}

	private void GetResource<T> (T t)
	{
		man.sprite = TextureToSpriteConverter.ConvertToSprite (t as Texture2D);
		man.gameObject.SetActive (true);
	}
}
