using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class ShortLoading : Loading
{
	private const float ROTATE_INTERVAL = -2f;
	public Image image;
	private Dictionary<string,Image> dictionary;
	private const string IMAGE_NAME = "loading_short_icon";

	private void Awake ()
	{
		image.sprite = AssetBundleResourcesLoader.loadingLocalDictionary [IMAGE_NAME];
	}

	private void Update ()
	{
		image.rectTransform.Rotate (0, 0, ROTATE_INTERVAL);
	}
}
