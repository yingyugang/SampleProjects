using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Text;
using System.Collections.Generic;

public class LongLoading : Loading
{
	public Image image1;
	public Image image2;
	private Dictionary<string,Image> dictionary;
	private const string IMAGE_NAME_1 = "loadingIyami1";
	private const string IMAGE_NAME_2 = "loadingIyami2";

	private void Awake ()
	{
		image1.sprite = AssetBundleResourcesLoader.loadingLocalDictionary [IMAGE_NAME_1];
		image2.sprite = AssetBundleResourcesLoader.loadingLocalDictionary [IMAGE_NAME_2];
	}

	protected override IEnumerator Show ()
	{
		ShowOrHideImages (false);
		StringBuilder stringBuilder = new StringBuilder (loadingText);
		for (int i = 0; i < 4; i++) {
			text.text = stringBuilder.ToString ();
			if (i == 3) {
				ShowOrHideImages (true);
				yield return new WaitForSeconds (TEXT_INTERVAL * 2);
			} else {
				yield return new WaitForSeconds (TEXT_INTERVAL);
			}
			stringBuilder.Append (dotText);
		}
		StartCoroutine (Show ());
	}

	private void ShowOrHideImages (bool isShow)
	{
		image1.gameObject.SetActive (!isShow);
		image2.gameObject.SetActive (isShow);
	}
}
