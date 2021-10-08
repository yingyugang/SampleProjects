using UnityEngine;
using System.Collections;
using System;

public class DialogLogin : DialogAbs {

	// Use this for initialization
	void Start () {
	}

    public override void ShowDialog(DialogAbs.CallBackShowDialog callback = null)
    {
        CommonObjectScript.isViewPoppup = true;
        Show = true;
        gameObject.SetActive(true);
        LeanTween.scale(gameObject, new Vector3(1, 1, 1f), 0.3f).setUseEstimatedTime(true).setEase(LeanTweenType.easeOutBack);
        transform.Find("Content").GetComponent<UILabel>().text = MissionControl.Language["Login_content"];
        transform.Find("Logo").Find("name").GetComponent<UILabel>().text = MissionControl.Language["CONNECT"];
        transform.Find("Login").Find("Label").GetComponent<UILabel>().text = MissionControl.Language["LOGIN"];
        transform.Find("Nothank").Find("Label").GetComponent<UILabel>().text = MissionControl.Language["NO_THANK"];
    }

    public override void HideDialog(DialogAbs.CallBackHideDialog callback = null)
    {
        LeanTween.scale(gameObject, new Vector3(0, 0, 0f), 0.3f).setEase(LeanTweenType.easeInBack).setUseEstimatedTime(true).setOnComplete(() =>
        {
            Show = false;
            CommonObjectScript.isViewPoppup = false;
            if (callback != null)
            {
                callback();
            }
        });
    }
}
