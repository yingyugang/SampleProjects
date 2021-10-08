using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class CardGameIcon : MonoBehaviour
{
    public List<GameObject> iconList;
    public Text text;
    public GameObject icon;

	public void SetIcon (bool showIcon, bool showBanner, int id = 0, float num = 0, int up_type = 1)
    {
        if (showIcon)
        {
            int length = iconList.Count;
            for (int i = 0; i < length; i++)
            {
                iconList[i].SetActive (false);
            }
            iconList[id].SetActive (true);
            gameObject.SetActive (true);
        }
        else
        {
            gameObject.SetActive (false);
            return;
        }
        if (showBanner)
        {
			if (up_type == 1)
            {
				text.text = string.Format ("{0}{1}{2}", LanguageJP.MINUS, num, LanguageJP.MINUTE); 
            }
			else if (up_type == 2)
            {
				text.text = string.Format ("{0}{1}{2}", LanguageJP.PLUS, num, LanguageJP.PERSENTAGE);
            }
			text.gameObject.SetActive (true);
        }
        else
        {
			text.gameObject.SetActive (false);
        }
    }
}
