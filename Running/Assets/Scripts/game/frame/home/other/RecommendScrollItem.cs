using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class RecommendScrollItem : RogerInteractiveScrollItem
{
	[HideInInspector]
	public int id;
	public Image imageField;
	public NetTextureLoader netTextureLoader;
	[HideInInspector]
	public string image_url;
	[HideInInspector]
	public string url;

	public void Show (int id, string title, string description, string image_url, string url)
	{
		this.id = id;
		this.image_url = image_url;
		this.url = url;
		gameObject.SetActive (true);
		GetImage (image_url);
	}

	private void GetImage (string image_url)
	{
		StartCoroutine (netTextureLoader.Load (image_url, (Texture2D texture2D) => {
			imageField.sprite = TextureToSpriteConverter.ConvertToSprite (texture2D);
			imageField.gameObject.SetActive (true);
		}));
	}
}
