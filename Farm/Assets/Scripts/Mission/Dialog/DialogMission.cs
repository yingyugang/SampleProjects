using UnityEngine;
using System.Collections;
using Facebook.MiniJSON;
using System.Collections.Generic;
using Assets.Scripts.Common;

public class DialogMission : DialogAbs
{
    Transform dialogLeft;
    Transform dialogMid;
    Transform dialogRight;
    bool callLogin = false;
    int level = 0;

    void Start()
    {
        dialogLeft = transform.Find("DialogLeft");
        dialogMid = transform.Find("DialogMid");
        dialogRight = transform.Find("DialogRight");
        gameObject.SetActive(false);
    }

    void Update()
    {
        //When user click login button on DialogLeft
        if (callLogin)
        {
            if (FB.IsLoggedIn)
            {
                callLogin = false;
                MissionControl.GetFriendsList();
                dialogLeft.GetComponent<DialogMissionLeft>().SetData(level);
            }
        }
    }

    public void ShowDialogMision(int level)
    {
        if (Show)
        {
            return;
        }
        this.level = level;
        VariableSystem.mission = level;
        dialogLeft.GetComponent<DialogMissionLeft>().Hide();
        print("lEVEL DC GAN " + VariableSystem.mission);
        callLogin = false;
        gameObject.SetActive(true);
        //LeanTween.scale(gameObject, new Vector3(1, 1, 1f), 0.3f).setEase(LeanTweenType.easeInOutBack);
        transform.Find("BgBlack").gameObject.SetActive(true);
        LeanTween.scale(dialogLeft.gameObject, new Vector3(1, 1, 1f), 0.2f).setUseEstimatedTime(true).setEase(LeanTweenType.easeOutBack).setDelay(0.3f);
        dialogRight.GetComponent<DialogMissionRight>().ShowDialog(() => { Show = true; });
        dialogLeft.GetComponent<DialogMissionLeft>().SetData(level);
        dialogMid.GetComponent<DialogMissionMid>().ShowDialog();
        dialogMid.gameObject.GetComponent<DialogMissionMid>().setData(level);
    }

    public void HideDialogMission()
    {
        //LeanTween.scale(gameObject, new Vector3(0, 0, 1f), 0.3f).setEase(LeanTweenType.easeInOutBack);
        dialogLeft.GetComponent<DialogMissionLeft>().Hide();
        dialogMid.GetComponent<DialogMissionMid>().Hide();
        LeanTween.scale(dialogMid.gameObject, new Vector3(0, 0, 0), 0.2f).setUseEstimatedTime(true).setEase(LeanTweenType.easeInBack).setDelay(0.2f).setOnComplete(() =>
        {
            //Chua bo dc cai dialog mission vi 1 so thanh phan cua no ko hien len sau khi goi active lai
            Show = false;
            transform.Find("BgBlack").gameObject.SetActive(false);
        });
        LeanTween.scale(dialogLeft.gameObject, new Vector3(0, 0, 0), 0.1f).setUseEstimatedTime(true).setEase(LeanTweenType.easeInBack);
        dialogRight.GetComponent<DialogMissionRight>().HideDialog();
    }

    public void LoginButton()
    {
        transform.parent.parent.gameObject.GetComponent<MissionControl>().LoginButton();
        callLogin = true;
    }

    public void StartButton()
    {
        LoadingStartMenu.showFullBanner = true;
        //HideDialogMission();
        AudioControl.DPlaySound("Click 1");
        dialogLeft.GetComponent<DialogMissionLeft>().Hide();
        dialogMid.GetComponent<DialogMissionMid>().Hide();
        LeanTween.scale(dialogMid.gameObject, new Vector3(0, 0, 0), 0.2f).setUseEstimatedTime(true).setEase(LeanTweenType.easeInBack).setDelay(0.2f).setOnComplete(() =>
        {
            MissionControl.countShowDialogLogin++;
            MissionControl.countShowRate++;
            Show = false;
            if (VariableSystem.heart < 1)
            {
                transform.parent.parent.GetComponent<MissionControl>().AddHeartButton();
            }
            else
            {
                //Tru tim khi bat dau choi
                AudioControl.AddHeart(-1);
                //Chua bo dc cai dialog mission vi 1 so thanh phan cua no ko hien len sau khi goi active lai
                if (MissionData.targetCommon.startScene == 1)
                    LoadingScene.ShowLoadingScene("Farm", true);
                else if (MissionData.targetCommon.startScene == 2)
                    LoadingScene.ShowLoadingScene("Factory", true);
                else
                    LoadingScene.ShowLoadingScene("Store", true);
                //Destroy music
                AudioControl.StopMusic("Nhac menu");
                GoogleAnalytics.instance.LogScreen("Play level " + level);
            }
            transform.Find("BgBlack").gameObject.SetActive(false);
        });
        LeanTween.scale(dialogLeft.gameObject, new Vector3(0, 0, 0), 0.1f).setUseEstimatedTime(true).setEase(LeanTweenType.easeInBack);
        dialogRight.GetComponent<DialogMissionRight>().HideDialog();

        //HungBV - reset 
        FactoryScenesController.isCreat = false;
        TownScenesController.isCreat = false;
        VilageResearchController.isCreate = false;
        CreatAndControlPanelHelp.countClickHelpPanel = 0;
        LotteryController.countSpin = 1;
    }

    public void ShopButton()
    {
        this.gameObject.SetActive(false);
        transform.parent.Find("DialogShop").gameObject.SetActive(true);
        transform.parent.Find("DialogShop").GetComponent<DialogShop>().ShowDialog();
    }

    public override void ShowDialog(DialogAbs.CallBackShowDialog callback = null)
    {
        throw new System.NotImplementedException();
    }

    public override void HideDialog(DialogAbs.CallBackHideDialog callback = null)
    {
        throw new System.NotImplementedException();
    }

    public void ButtonBlack()
    {
        HideDialogMission();
    }
}
