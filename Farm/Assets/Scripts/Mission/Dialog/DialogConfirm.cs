using UnityEngine;
using System.Collections;
using System;

public class DialogConfirm : MonoBehaviour
{

    Action okAction, cancelAction;
    Transform bgBlack, bgMain;
    bool Show;

    void OnEnable()
    {
        transform.parent = GameObject.Find("AudioControl").transform;
        transform.localPosition = Vector3.zero;
        bgMain = transform.Find("Main");
        bgBlack = transform.Find("Black");
    }

    public void ShowDialog(string title, string content, Action okAction = null, Action cancelAction = null)
    {
        Show = true;
        CommonObjectScript.isViewPoppup = true;
        this.okAction = okAction;
        this.cancelAction = cancelAction;
        bgMain.Find("Logo").Find("Title").GetComponent<UILabel>().text = title;
        bgMain.Find("Ok").Find("LabelOK").GetComponent<UILabel>().text = MissionControl.Language["Ok"];
        bgMain.Find("Cancel").Find("LabelCancel").GetComponent<UILabel>().text = MissionControl.Language["Cancel"];
        bgMain.Find("TextShow").GetComponent<UILabel>().text = content;

        bgMain.gameObject.SetActive(true);
        bgBlack.gameObject.SetActive(true);
        LeanTween.scale(bgMain.gameObject, new Vector3(1, 1, 1), 0.4f).setUseEstimatedTime(true).setEase(LeanTweenType.easeOutBack).setUseEstimatedTime(true); ;
    }

    public void ShowDialogHideCancel(string title, string content, Action okAction = null, Action cancelAction = null)
    {
        Show = true;
        bgMain.Find("Ok").transform.localPosition = new Vector3(0, bgMain.Find("Ok").transform.localPosition.y, bgMain.Find("Ok").transform.localPosition.z);
        bgMain.Find("Cancel").gameObject.SetActive(false);
        bgMain.Find("Ok").Find("LabelOK").GetComponent<UILabel>().text = MissionControl.Language["Ok"];
        bgMain.Find("Cancel").Find("LabelCancel").GetComponent<UILabel>().text = MissionControl.Language["Cancel"];

        CommonObjectScript.isViewPoppup = true;
        this.okAction = okAction;
        this.cancelAction = cancelAction;
        bgMain.Find("Logo").Find("Title").GetComponent<UILabel>().text = title;
        bgMain.Find("TextShow").GetComponent<UILabel>().text = content;

        bgMain.gameObject.SetActive(true);
        bgBlack.gameObject.SetActive(true);
        LeanTween.scale(bgMain.gameObject, new Vector3(1, 1, 1), 0.4f).setUseEstimatedTime(true).setEase(LeanTweenType.easeOutBack).setUseEstimatedTime(true);
    }

    public void ButtonCancel()
    {
        LeanTween.scale(bgMain.gameObject, new Vector3(0.0f, 0.0f, 0), 0.4f).setEase(LeanTweenType.easeInBack).setUseEstimatedTime(true).setOnComplete(() =>
        {
            bgMain.gameObject.SetActive(false);
            bgBlack.gameObject.SetActive(false);
            CommonObjectScript.isViewPoppup = false;
            if (cancelAction != null)
            {
                cancelAction();
            }
            Show = false;
            Destroy(this.gameObject);
        });
    }

    public void ButtonOk()
    {
        LeanTween.scale(bgMain.gameObject, new Vector3(0.0f, 0.0f, 0), 0.4f).setEase(LeanTweenType.easeInBack).setUseEstimatedTime(true).setOnComplete(() =>
        {
            CommonObjectScript.isViewPoppup = false;
            bgMain.gameObject.SetActive(false);
            bgBlack.gameObject.SetActive(false);
            if (okAction != null)
            {
                okAction();
            }
            Destroy(this.gameObject);
            Show = false;
        });
    }

    void Update()
    {
        if (Show)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                ButtonCancel();
            }
        }
    }
    void OnApplicationPause(bool pauseStatus)
    {
        if (!pauseStatus && Show)
        {
            ButtonCancel();
        }
    }
}
