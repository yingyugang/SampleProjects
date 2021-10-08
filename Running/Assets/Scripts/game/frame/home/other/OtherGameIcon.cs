using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class OtherGameIcon : MonoBehaviour
{
	public Image image;
	public Text rate;

	public void SetData (int up_type, int id = 0)
	{
		image.sprite = AssetBundleResourcesLoader.gameIconDictionary [id == 0 ? LanguageJP.TIME_ICON : string.Format ("{0}{1}", LanguageJP.GAME_ICON_PREFIX, id)];
		if (up_type == 1) {
			rate.text = string.Format ("{0}{1}{2}", LanguageJP.MINUS, CardRate.GetTotal (up_type), LanguageJP.MINUTE);
		} else if (up_type == 2) {
			rate.text = string.Format ("{0}{1}{2}", LanguageJP.PLUS, CardRate.GetTotal (up_type, id), LanguageJP.PERSENTAGE);
		}
	}
}
