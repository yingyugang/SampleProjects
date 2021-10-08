using UnityEngine;
using System.Collections;

public class DialogRefill : DialogAbs {
    public Transform DialogConfirm;
    Transform dialogMain, bgBlack;

	void Start () {
        dialogMain = transform.Find("Main");
        bgBlack = transform.Find("BgBlack");
	}
	
	void Update () {
	
	}

    public override void ShowDialog(DialogAbs.CallBackShowDialog callback = null)
    {
        dialogMain.Find("Bg1").Find("Texture").Find("Title").GetComponent<UILabel>().text = MissionControl.Language["REFILL"];
        Show = true;
        bgBlack.gameObject.SetActive(true);
        dialogMain.gameObject.SetActive(true);
        dialogMain.Find("AskFriend").Find("Label").GetComponent<UILabel>().text = MissionControl.Language["Ask_friends"];
        dialogMain.Find("Refill").Find("Label").GetComponent<UILabel>().text = MissionControl.Language["Refill"];
        LeanTween.scale(dialogMain.gameObject, new Vector3(1, 1, 1), 0.4f).setUseEstimatedTime(true).setEase(LeanTweenType.easeOutBack);
    }

    public override void HideDialog(DialogAbs.CallBackHideDialog callback = null)
    {
        LeanTween.scale(dialogMain.gameObject, new Vector3(0, 0, 0), 0.2f).setUseEstimatedTime(true).setEase(LeanTweenType.easeInBack).setOnComplete(() =>
        {
            Show = false;
            bgBlack.gameObject.SetActive(false);
            //dialogMain.gameObject.SetActive(false);
        });
    }

    public void ButtonRefill()
    {
        Debug.Log("Button refill");
        Transform confirm = Instantiate(DialogConfirm) as Transform;
        confirm.parent = transform;
        HideDialog();
        confirm.GetComponent<DialogConfirm>().ShowDialog(MissionControl.Language["Refill"], MissionControl.Language["refill_heart"], () =>
        {
           if(VariableSystem.diamond >= 3)
           {
               VariableSystem.AddDiamond(-3);
               AudioControl.AddHeart(5 - VariableSystem.heart);
           }
           else
           {
               DialogInapp.ShowInapp();
           }
        });
    }

    public void ButtonAskFriends()
    {
        HideDialog();
        transform.parent.parent.GetComponent<MissionControl>().ShowAskForFriend();
    }
}
