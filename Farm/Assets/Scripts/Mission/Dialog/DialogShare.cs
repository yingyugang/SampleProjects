using UnityEngine;
using System.Collections;

public class DialogShare : DialogAbs {
    Transform main, black;

	// Use this for initialization
    void Awake()
    {
        main = transform.Find("Main");
        black = transform.Find("BgBlack");
	}

    public override void ShowDialog(DialogAbs.CallBackShowDialog callback = null)
    {
        main.Find("BackGround").Find("Logo").Find("name").GetComponent<UILabel>().text = MissionControl.Language["RATE"];
        main.Find("Content").GetComponent<UILabel>().text = MissionControl.Language["Rate_Content"];
        main.Find("Rate").Find("Label").GetComponent<UILabel>().text = MissionControl.Language["RATE_IT"];
        main.Find("Nothank").Find("Label").GetComponent<UILabel>().text = MissionControl.Language["NO_THANK"];

        CommonObjectScript.isViewPoppup = true;
        if (!Show)
        {
            Show = true;
            main.gameObject.SetActive(true);
            black.gameObject.SetActive(true);
            LeanTween.scale(main.gameObject, Vector3.one, 0.5f).setEase(LeanTweenType.easeOutBack);
        }
    }

    public override void HideDialog(DialogAbs.CallBackHideDialog callback = null)
    {
        if (Show)
        {
            LeanTween.scale(main.gameObject, Vector3.zero, 0.5f).setEase(LeanTweenType.easeInBack).setOnComplete(() =>
            {
                CommonObjectScript.isViewPoppup = false;
                black.gameObject.SetActive(false);
                Show = false;
                main.gameObject.SetActive(false);
            });
        }
    }

    public void ButtonShare()
    {
        Debug.Log("share");
    }

    public void ButtonRate()
    {
        Debug.Log("rate");
#if UNITY_ANDROID
        Application.OpenURL("market://details?id=com.redeye.farmbusiness");
#elif UNITY_IPHONE
     Application.OpenURL("itms-apps://itunes.apple.com/app/com.redeye.farmbusiness");
#endif

    }

    public void ButtonNothank()
    {
        HideDialog();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.S))
        {
            ShowDialog();
        }
    }
}
