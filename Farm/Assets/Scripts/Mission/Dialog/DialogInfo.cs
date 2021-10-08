using UnityEngine;
using System.Collections;

public class DialogInfo : DialogAbs
{
    public Transform DialogConfirm;
    Transform dialogMain, bgBlack;
    Transform left, right;

    void Start()
    {
        dialogMain = transform.Find("Main");
        bgBlack = transform.Find("BgBlack");
        left = transform.Find("Panel").Find("Left");
        right = transform.Find("Panel").Find("Right");
    }

    void Update()
    {

    }

    public override void ShowDialog(DialogAbs.CallBackShowDialog callback = null)
    {
        if (CommonObjectScript.isViewPoppup)
        {
            return;
        }
        CommonObjectScript.isViewPoppup = true;
        dialogMain.Find("Panel").Find("Logo").Find("Title").GetComponent<UILabel>().text = MissionControl.Language["INFO"];
        if ("Vietnamese".Equals(VariableSystem.language))
        {
            dialogMain.Find("En").gameObject.SetActive(false);
            dialogMain.Find("Vi").gameObject.SetActive(true);
        }
        else
        {
            dialogMain.Find("En").gameObject.SetActive(true);
            dialogMain.Find("Vi").gameObject.SetActive(false);
        }
        Show = true;
        bgBlack.gameObject.SetActive(true);
        dialogMain.gameObject.SetActive(true);
        left.parent.gameObject.SetActive(true);
        LeanTween.scale(dialogMain.gameObject, new Vector3(1, 1, 1), 0.4f).setEase(LeanTweenType.easeOutBack);
        LeanTween.moveLocalY(left.gameObject, -130, 0.4f).setEase(LeanTweenType.easeOutBack);
        LeanTween.moveLocalY(right.gameObject, -130, 0.4f).setEase(LeanTweenType.easeOutBack);
        dialogMain.Find("En").Find("Texture").GetComponent<TweenPosition>().ResetToBeginning();
        dialogMain.Find("Vi").Find("Texture").GetComponent<TweenPosition>().ResetToBeginning();

    }

    public override void HideDialog(DialogAbs.CallBackHideDialog callback = null)
    {
        LeanTween.moveLocalY(left.gameObject, -645, 0.4f).setEase(LeanTweenType.easeOutBack);
        LeanTween.moveLocalY(right.gameObject, -650, 0.4f).setEase(LeanTweenType.easeOutBack);
        LeanTween.scale(dialogMain.gameObject, new Vector3(0, 0, 0), 0.3f).setEase(LeanTweenType.easeInBack).setOnComplete(() =>
        {
            Show = false;
            bgBlack.gameObject.SetActive(false);
            left.parent.gameObject.SetActive(false);
            CommonObjectScript.isViewPoppup = false;
            dialogMain.gameObject.SetActive(false);
        });
    }
}
