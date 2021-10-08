using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class ADMediator : PopupContentMediator
{
	private AD ad;
    public Text title;
    public Text content;

	private void OnEnable ()
	{
		title.text = objectList[0].ToString();
        content.text = objectList[1].ToString();
	}

    protected override void OKButtonOnClickHandler()
    {
        if (popupAction != null)
        {
            popupAction();
        }
        base.OKButtonOnClickHandler();
    }
}
