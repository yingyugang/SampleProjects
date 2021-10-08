using System;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShopLegalViewMediator : PopupContentMediator
{
    public Text titleTxt;
    public Text contentTxt;
    public Button linkBtn;

    private void OnEnable ()
    {
        titleTxt.text = objectList[0].ToString();
        contentTxt.text = objectList[1].ToString();
        contentTxt.rectTransform.anchoredPosition = Vector2.zero;
        linkBtn.onClick.AddListener(delegate() {
            if (contentTxt.text.IndexOf("http://") > -1)
            {
                Application.OpenURL(contentTxt.text.Substring(contentTxt.text.IndexOf("http://"),contentTxt.text.Length-contentTxt.text.IndexOf("http://")));
            }
        });
    }

}
