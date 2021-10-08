using UnityEngine;
using System.Collections;
using Assets.Scripts.Common;
using System.Collections.Generic;
using System;

public class DialogResult : MonoBehaviour
{
    public Transform ItemResult;
    Transform dialogTask;
    Transform particleWin;
    Transform missionFail;
    Transform bgBlack, bgMain;
    UILabel lbTitle, lbReward, lbRewardCustomer;
    UIGrid grid;
    Transform star1, star2, star3;
    int reward = 0;
    int star;
    string tag = "Result";
    Transform dialogLoadingScene;

    public Transform DialogConfirm;

    void Awake()
    {
        transform.parent = GameObject.Find("AudioControl").transform;
        dialogLoadingScene = transform.parent.Find("LoadingScene");
        this.gameObject.tag = tag;
        GameObject[] audioControls = GameObject.FindGameObjectsWithTag(tag);
        if (audioControls.Length > 1)
        {
            for (int i = 1; i < audioControls.Length; i++)
            {
                Destroy(audioControls[i]);
            }
        }

        dialogTask = GameObject.Find("DialogTask").transform;
        bgBlack = transform.Find("Black");
        bgMain = transform.Find("Main");
        bgMain.localScale = new Vector3(0, 0, 1);
        bgMain.gameObject.SetActive(false);
        bgBlack.gameObject.SetActive(false);

        particleWin = transform.Find("Particle");

        star1 = bgMain.Find("Star1");
        star2 = bgMain.Find("Star2");
        star3 = bgMain.Find("Star3");
        star1.gameObject.SetActive(false);
        star2.gameObject.SetActive(false);
        star3.gameObject.SetActive(false);
        lbTitle = bgMain.Find("Title").GetComponent<UILabel>();
        lbReward = bgMain.Find("Reward").Find("Label").GetComponent<UILabel>();
        lbRewardCustomer = bgMain.Find("RewardCustomer").Find("Label").GetComponent<UILabel>();
        grid = bgMain.Find("Grid").GetComponent<UIGrid>();
        missionFail = bgMain.Find("MissionFail");
        DontDestroyOnLoad(this);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            ShowResult();
        }        
    }

    public void Show()
    {
        Time.timeScale = 1.0f;
        MissionControl.ResetAllItem();
        LoadingScene.HideLoadingScene();
        CommonObjectScript.isViewPoppup = true;
        //Debug.Log("Show result ----------------------------------");
        missionFail.localScale = new Vector3(8, 8, 1);
        missionFail.gameObject.SetActive(false);
        TweenAlpha.Begin(missionFail.gameObject, 0, 0);
        star = 0;
        if (DialogTask.complete)
        {
            star = 1;
            reward = MissionData.starMission.reward[0];
            if (CommonObjectScript.dollar >= MissionData.starMission.twoStar)
            {
                reward = MissionData.starMission.reward[1];
                star = 2;
            }
            if (CommonObjectScript.dollar >= MissionData.starMission.threeStar)
            {
                reward = MissionData.starMission.reward[2];
                star = 3;
            }
            particleWin.gameObject.SetActive(true);
            ///////////------------------------------------------
            AudioControl.getMonoBehaviour().StartCoroutine(DHS.PostMeCurrentMission(VariableSystem.mission + 1));
            //Cong tim khi thang
            AudioControl.AddHeart(1);

            //Nhac thang
            AudioControl.DPlaySound("Thang");
        }
        else
        {
            //Nhac thua
            AudioControl.DPlaySound("Thua");
        }
        //star = 3;//////////////////////////////////////////////////////
        //particleWin.gameObject.SetActive(true);
        if (Application.loadedLevelName.Equals("Farm"))
        {
            GameObject.Find("UI Root").transform.Find("PanelPlant").GetComponent<PlantControlScript>().BG_Click();
        }
        lbReward.text = "0";
        //Test
        //VariableSystem.dollar = 1000;
        //Thay doi ngon ngu
        bgMain.Find("Target").GetComponent<UILabel>().text = "" + MissionControl.Language["Require"];
        bgMain.Find("Score").GetComponent<UILabel>().text = "" + MissionControl.Language["Your_Score"];
        bgMain.Find("Reward").GetComponent<UILabel>().text = "" + MissionControl.Language["Reward"];
        bgMain.Find("Ok").Find("Label").GetComponent<UILabel>().text = "" + MissionControl.Language["Ok"];
        bgMain.Find("TryAgain").Find("Label").GetComponent<UILabel>().text = "" + MissionControl.Language["Try_Again"];
        bgMain.Find("RewardCustomer").GetComponent<UILabel>().text = "" + MissionControl.Language["Bonus_customer_rate"];
        bgMain.Find("LbStar1").GetComponent<UILabel>().text = "" + MissionControl.Language["All_target"];

        lbTitle.text = MissionControl.Language["MISSION"] + " " + VariableSystem.mission;
        bgMain.Find("LbStar2").GetComponent<UILabel>().text = DString.ConvertString(MissionData.starMission.twoStar);
        bgMain.Find("LbStar3").GetComponent<UILabel>().text = DString.ConvertString(MissionData.starMission.threeStar);
        bgMain.Find("Score").Find("Label").GetComponent<UILabel>().text = DString.ConvertString(CommonObjectScript.dollar);
        bgBlack.gameObject.SetActive(true);
        LeanTween.delayedCall(1.0f, () =>
        {
            Time.timeScale = 0;
        }).setUseEstimatedTime(true);
        bgMain.gameObject.SetActive(true);
        LeanTween.scale(bgMain.gameObject, new Vector3(1, 1, 1), 0.4f).setUseEstimatedTime(true).setEase(LeanTweenType.easeOutBack).setOnComplete(() =>
        {
            star1.localScale = new Vector3(8, 8, 8);
            star2.localScale = new Vector3(8, 8, 8);
            star3.localScale = new Vector3(8, 8, 8);
            //Dieu kien xet sao
            if (star > 0)
            {
                star1.gameObject.SetActive(true);
                LeanTween.scale(star1.gameObject, new Vector3(1, 1, 1), 0.5f).setUseEstimatedTime(true).setEase(LeanTweenType.easeOutExpo).setOnComplete(() =>
                {
                    //Check dieu kien de co sao thu 2
                    if (star > 1)
                    {
                        star2.gameObject.SetActive(true);
                        LeanTween.scale(star2.gameObject, new Vector3(1, 1, 1), 0.5f).setUseEstimatedTime(true).setEase(LeanTweenType.easeOutExpo).setOnComplete(() =>
                        {
                            //Check dieu kien de co sao thu 3
                            if (star > 2)
                            {
                                star3.gameObject.SetActive(true);
                                LeanTween.scale(star3.gameObject, new Vector3(1, 1, 1), 0.5f).setUseEstimatedTime(true).setEase(LeanTweenType.easeOutExpo);
                            }
                        });
                    }
                });
            }
            else
            {
                missionFail.gameObject.SetActive(true);
                LeanTween.scale(missionFail.gameObject, new Vector3(1, 1, 1), 0.5f).setUseEstimatedTime(true).setEase(LeanTweenType.easeInOutQuart);
                TweenAlpha.Begin(missionFail.gameObject, 0.5f, 1);
            }
        });
        //Them task nhiem vu
        //if (VariableSystem.mission == 1)
        //{
        //    AddItemResult("" + MissionControl.Language["Control_guide"], true, null, false);
        //}
        foreach (Transform tf in dialogTask.Find("Grid").GetComponent<UIGrid>().GetChildList())
        {
            AddItemResult(tf.GetComponent<ItemTask>().lbContent.text, tf.GetComponent<ItemTask>().togComplete.value, tf.GetComponent<ItemTask>().item, tf.GetComponent<ItemTask>().typeShow1);
        }
        grid.Reposition();
        transform.Find("SpriteBlack").gameObject.SetActive(true);

        //An bang task khi hien thi result
        GameObject task = GameObject.Find("DialogTask").gameObject;
        if (task != null)
        {
            task.GetComponent<DialogTask>().HideButton();
        }

        GameObject inapp = GameObject.Find("DialogInapp").gameObject;
        if (inapp != null)
        {
            inapp.GetComponent<DialogInapp>().HideDialog();
        }

        //Cap nhat lai phan thuong - Neu so sao moi  <= so sao hien tai => thuong 1 kim cuong
        if (star > 0 && star <= DataCache.dataMissionCache[VariableSystem.mission - 1].Star)
        {
            reward = 1;
        }

        VariableSystem.AddDiamond(reward);
        VariableSystem.AddDiamond(CommonObjectScript.rewardCustomerRate);
        Debug.Log("CONG KIM CUONG -------------------- reward " + reward + " CommonObjectScript.rewardCustomerRate " + CommonObjectScript.rewardCustomerRate);
        DataCache.UpdateMissionScore(CommonObjectScript.dollar, star, VariableSystem.mission, 1);
        bool sendToServer = false;
        if (star > 0)
        {
            if(VariableSystem.mission < DataMissionControlNew.MAX_MISSION)
            {
                DataCache.SetMeCurrentMission(VariableSystem.mission + 1);
            }
            sendToServer = true;
        }
        DataCache.SaveMissionDataCache(sendToServer);
        lbReward.text = "" + reward;
        lbRewardCustomer.text = "" + CommonObjectScript.rewardCustomerRate;
    }

    public static void ShowResult()
    {
        //CameraController.isViewPopup = true;
        CommonObjectScript.isEndGame = true;
        GameObject.Find("DialogResult").GetComponent<DialogResult>().Show();

        //print(" CameraController.isViewPopup: " + CameraController.isViewPopup);


    }

    public void OkButton()
    {
        CommonObjectScript.isViewPoppup = false;
        Time.timeScale = 1;
        print("aaaaaaaaaaa");
        Debug.Log("------------------OK BUTTTON--------------------" + star);
        
        LeanTween.scale(bgMain.gameObject, new Vector3(0, 0, 0), 0.4f).setEase(LeanTweenType.easeInBack).setUseEstimatedTime(true).setOnComplete(() =>
        {
            RemoveAllItem();
            bgMain.gameObject.SetActive(false);
            bgBlack.gameObject.SetActive(false);
            star1.gameObject.SetActive(false);
            star2.gameObject.SetActive(false);
            star3.gameObject.SetActive(false);
            // CameraController.isViewPopup = false;
            transform.Find("SpriteBlack").gameObject.SetActive(false);
            //Application.LoadLevel("Mission");
            LoadingScene.ShowLoadingScene("Mission", true);

            //HungBV 19/01
            //FactoryScenesController.isCreat = false;
            //TownScenesController.isCreat = false;
            //CreatAndControlPanelHelp.countClickHelpPanel = 0;
            Destroy(gameObject);
        });
    }

    public void TryAgainButton()
    {
        CommonObjectScript.isViewPoppup = false;
        //LeanTween.scale(bgMain.gameObject, new Vector3(0.2f, 0.2f, 1), 0.4f).setEase(LeanTweenType.easeInBack).setUseEstimatedTime(true).setOnComplete(() =>
        //{
        //    RemoveAllItem();
        //    bgMain.gameObject.SetActive(false);
        //    bgBlack.gameObject.SetActive(false);
        //    LeanTween.delayedCall(0.3f, delegate() { Application.LoadLevel("Farm"); });
        //});
        LeanTween.scale(bgMain.gameObject, new Vector3(0, 0, 0), 0.4f).setEase(LeanTweenType.easeInBack).setUseEstimatedTime(true).setOnComplete(() =>
        {
            RemoveAllItem();
            bgMain.gameObject.SetActive(false);
            bgBlack.gameObject.SetActive(false);
            Transform confirm = Instantiate(DialogConfirm) as Transform;
            confirm.GetComponent<DialogConfirm>().ShowDialog(MissionControl.Language["Try_Again"], MissionControl.Language["try_again_detail"], () =>
            {
                //Hungbv
                FactoryScenesController.isCreat = false;
                TownScenesController.isCreat = false;
                CreatAndControlPanelHelp.countClickHelpPanel = 0;
                MissionControl.ShowTryAgain();
                Destroy(gameObject);
                Time.timeScale = 1;
            }, () =>
            {
                OkButton();
                Time.timeScale = 1;
            });
        });
        print("aaaaaaaaaaa");
       
    }

    //Item nay giong voi item Task.
    public void AddItemResult(string content, bool finishValue, ItemAbstract itemAbstract, bool typeShow1)
    {
        Transform item = Instantiate(ItemResult) as Transform;
        item.GetComponent<ItemTask>().SetDataForResult(content, itemAbstract);
        item.GetComponent<ItemTask>().typeShow1 = typeShow1;
        grid.AddChild(item);
        item.localScale = new Vector3(1, 1, 1);
        item.Find("Checkbox").GetComponent<UIToggle>().value = finishValue;
    }

    public void RemoveAllItem()
    {
        List<Transform> arrItem = grid.GetChildList();
        for (int i = 0; i < arrItem.Count; i++)
        {
            grid.RemoveChild(arrItem[i]);
            Destroy(arrItem[i].gameObject);
        }
    }
}
