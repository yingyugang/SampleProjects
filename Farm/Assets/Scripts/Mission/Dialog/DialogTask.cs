using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Common;

public class DialogTask : MonoBehaviour
{
    //Bien check da hoan thanh het cac task hay chua
    public static bool complete;
    public static int countStar;
    bool showResult;
    public Transform ItemTask;
    UIGrid grid;
    UILabel tip;
    UILabel lbStar2, lbStar3;
    UITexture star1, star2, star3;
    string tag = "Task";
    Transform bgColider;
    Transform Spriterblack;
    public bool isShow, isEnable;

    bool checkShowAskResult;

    void Awake()
    {
        transform.parent = GameObject.Find("AudioControl").transform;
        checkShowAskResult = false;
        showResult = false;
        countStar = 0;
        this.gameObject.tag = tag;
        GameObject[] audioControls = GameObject.FindGameObjectsWithTag(tag);
        if (audioControls.Length > 1)
        {
            for (int i = 1; i < audioControls.Length; i++)
            {
                Destroy(audioControls[i]);
            }
        }
        lbStar2 = transform.Find("LbStar2").GetComponent<UILabel>();
        lbStar3 = transform.Find("LbStar3").GetComponent<UILabel>();
        star1 = transform.Find("Star1").GetComponent<UITexture>();
        star1.gameObject.SetActive(false);
        star2 = transform.Find("Star2").GetComponent<UITexture>();
        star2.gameObject.SetActive(false);
        star3 = transform.Find("Star3").GetComponent<UITexture>();
        star3.gameObject.SetActive(false);

        grid = transform.Find("Grid").GetComponent<UIGrid>();
        tip = transform.Find("Tip").GetComponent<UILabel>();
        bgColider = transform.Find("BgColider");
        bgColider.gameObject.SetActive(false);
        Spriterblack = transform.Find("Spriterblack");
        Spriterblack.gameObject.SetActive(false);
        tip.gameObject.SetActive(false);
        DontDestroyOnLoad(this);
        //DialogTask.complete = true;
    }

    void Start()
    {
        // this.gameObject.SetActive(false);
    }

    void Update()
    {
        if (!CommonObjectScript.isEndGame)
        {
            DialogTask.complete = grid.GetChildList().Count > 0;
            for (int i = 0; i < grid.GetChildList().Count; i++)
            {
                if (!grid.GetChild(i).GetComponent<ItemTask>().finish)
                {
                    DialogTask.complete = false;
                    break;
                }
            }
        }
        if (isShow)
        {
            if (MissionData.starMission != null && DialogTask.complete)
            {
                countStar = 1;
                star1.gameObject.SetActive(true);
                if (CommonObjectScript.dollar >= MissionData.starMission.twoStar)
                {
                    countStar = 2;
                    star2.gameObject.SetActive(true);
                    if (CommonObjectScript.dollar >= MissionData.starMission.threeStar)
                    {
                        countStar = 3;
                        star3.gameObject.SetActive(true);
                    }
                }
            }
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                HideButton();
            }
        }
        //Debug.Log("AAAAAAAAAAAAAA " + DialogTask.complete + LotteryController.isSpinning + checkShowAskResult + CommonObjectScript.isGuide);

        if (DialogTask.complete && !LotteryController.isSpinning && !checkShowAskResult)
        {
            checkShowAskResult = true;
            if (!CommonObjectScript.isGuide)
            {
                if(!Application.loadedLevelName.Equals("Mission"))
                {
                    GameObject.Find("DialogAskResult").GetComponent<DialogAskResult>().ShowDialog();
                    HideButton();
                }
            }
        }
        //cuongvm
        if (Input.GetMouseButton(0))
        {
            if (Input.mousePosition.x > Screen.width / 2)
            {
                HideButton();
            }
        }
        //if (complete && !showResult && VariableSystem.mission != 1)
        //{
        //    showResult = true;
        //    DialogResult.ShowResult();
        //}
    }

    void OnApplicationPause(bool pauseStatus)
    {
        if (!pauseStatus)
        {
            HideButton();
        }
    }
    public void AddItemTask(string content, ItemAbstract itemAbstract)
    {
        Transform item = Instantiate(ItemTask) as Transform;
        item.GetComponent<ItemTask>().SetData(content, itemAbstract);
        grid.AddChild(item);
        checkShowAskResult = false;
        DialogTask.complete = false;
    }

    public void HideButton()
    {
        if (isShow)
        {
            isShow = false;
            CommonObjectScript.isViewPoppup = false;
            LeanTween.moveX(this.gameObject, -3f, 0.4f).setUseEstimatedTime(true).setEase(LeanTweenType.easeInQuart).setOnComplete(() =>
            {
                bgColider.gameObject.SetActive(false);
                Spriterblack.gameObject.SetActive(false);
                star1.gameObject.SetActive(false);
                star2.gameObject.SetActive(false);
                star3.gameObject.SetActive(false);
                isEnable = false;
                if (CommonObjectScript.isGuide)
                {
                    GameObject.Find("GuideFarmController").GetComponent<GuideFarmScript>().NextGuideText();
                }
            });
        }

    }

    public void Show()
    {
        transform.localPosition = new Vector3(-3, 0, 0);
        CommonObjectScript.isViewPoppup = true;
        isShow = true;
        isEnable = true;
        bgColider.gameObject.SetActive(true);
        Spriterblack.gameObject.SetActive(true);
        //Dieu kien xet sao
        if (MissionData.starMission != null && DialogTask.complete)
        {
            countStar = 1;
            star1.gameObject.SetActive(true);
            if (CommonObjectScript.dollar >= MissionData.starMission.twoStar)
            {
                countStar = 2;
                star2.gameObject.SetActive(true);
                if (CommonObjectScript.dollar >= MissionData.starMission.threeStar)
                {
                    countStar = 3;
                    star3.gameObject.SetActive(true);
                }
            }
        }
        Debug.Log("Star result -------------------------- " + countStar);
        //Check tip text
        if (string.IsNullOrEmpty(MissionData.tip_en))
        {
            tip.gameObject.SetActive(false);
        }
        else
        {
            if ("Vietnamese".Equals(VariableSystem.language))
            {
                tip.text = "Mẹo: " + MissionData.tip_vi;
            }
            else
            {
                tip.text = "Tip: " + MissionData.tip_en;
            }
            tip.gameObject.SetActive(true);
        }
        lbStar2.text = "" + DString.ConvertString(MissionData.starMission.twoStar);
        lbStar3.text = "" + DString.ConvertString(MissionData.starMission.threeStar);
        transform.Find("LbStar1").GetComponent<UILabel>().text = MissionControl.Language["All_target"];
        LeanTween.moveX(this.gameObject, 0f, 0.4f).setEase(LeanTweenType.easeOutQuart);
    }

    public void RemoveAllItem()
    {
        List<Transform> arrItem = grid.GetChildList();
        for (int i = 0; i < arrItem.Count; i++)
        {
            grid.RemoveChild(arrItem[i]);
            Destroy(arrItem[i].gameObject);
        }
        grid.repositionNow = true;
    }

}
