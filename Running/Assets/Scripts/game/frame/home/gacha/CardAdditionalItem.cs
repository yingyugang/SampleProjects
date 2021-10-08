using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CardAdditionalItem : MonoBehaviour
{
	public Text originalValue;
	public Text additionalValue;
	public Image icon;

	public void SetItem (float original, float additional, int id, int up_type)
	{
		additionalValue.gameObject.SetActive (additional != 0f);
		if (up_type == 1) {
			icon.sprite = AssetBundleResourcesLoader.gameIconDictionary [LanguageJP.TIME_ICON];
			originalValue.text = string.Format ("{0}{1}{2}", LanguageJP.MINUS, original, LanguageJP.MINUTE);
			additionalValue.text = string.Format ("{0}{1}{2}", LanguageJP.MINUS, additional, LanguageJP.MINUTE);
		} else if (up_type == 2) {
			icon.sprite = AssetBundleResourcesLoader.gameIconDictionary [string.Format ("{0}{1}", LanguageJP.GAME_ICON_PREFIX, id)];
			originalValue.text = string.Format ("{0}{1}{2}", LanguageJP.PLUS, original, LanguageJP.PERSENTAGE);
			additionalValue.text = string.Format ("{0}{1}{2}", LanguageJP.PLUS, additional, LanguageJP.PERSENTAGE);
		}
		originalValue.gameObject.SetActive (true);
	}
}
