using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Common;

public class MutilmediaController : MonoBehaviour
{


    public UILabel[] labelDescription;
    public UILabel[] listlabelData;
    public UILabel[] textFrame;
    // private List<int> unlockType;

    public UIButton[] listType;
    public UILabel[] listLabelNameType;
    public UITexture[] texttureLockForms;

    public UIScrollView scrollView;

    public GameObject timerPrefabs;
    private GameObject timerClone;

    public GameObject PanelLabel;
    public GameObject ContainerCoinEfectPrefabs;
    private GameObject ContainerCoinEfectClone;
    public UILabel labelLock;

    private bool isApply;
    private float timeDelay;
    private float timeCount;

    private int idType;
    private UIButton buttonSelected;
    private int iDPre;
    AudioControl audioControl;

    private bool isChangePos;
    private int tempChange;
    private Vector2 perChangePos;
    private Vector2 perChangeOff;

    void OnEnable()
    {
        CommonObjectScript.isViewPoppup = true;
    }
    void Start()
    {
        //unlockType = new List<int> { 0, 1, 2 };
        audioControl = GameObject.Find("AudioControl").GetComponent<AudioControl>();
        setLockType();
        setUnLockType();
        SetDescription();
        SetData(0);
        isApply = false;
        SetDataClickEven(listType[0], 0, 9);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || CommonObjectScript.isEndGame || CommonObjectScript.isViewComplete)
        {
            if (!TownScenesController.isHelp)
                Close_Click();
        }
        if (isApply)
        {
            if (timeCount < timeDelay)
            {
                timeCount += Time.deltaTime;
            }
            else
            {
                this.gameObject.GetComponent<Animator>().Play("InVisible");
                isApply = false;
            }
        }

        if (idType == 0)
        {
            ChangeScroll(perChangePos, perChangeOff);
        }
        else if (idType == 4)
        {
            ChangeScroll(perChangePos, perChangeOff, true);
        }
    }
    void OnApplicationPause(bool pauseStatus)
    {
        if (!pauseStatus)
        {
            if (!TownScenesController.isHelp)
                Close_Click();
        }
    }
    public void Close_Click()
    {
        audioControl.PlaySound("Click 1");
        this.gameObject.GetComponent<Animator>().Play("InVisible");
    }
    public void Apply_Click()
    {
        audioControl.PlaySound("Click 1");
        if (!isApply)
        {
            if (CommonObjectScript.dollar >= TownScenesController.ListMutilmediaInfomation[idType].cost)
            {
                timeDelay = 0.8f;
                timeCount = 0;
                TownScenesController.townsBusy[2] = true;
                isApply = true;
                CreateContainerCoinEfect();
                CreateTimer();
                AddCommonObject(-TownScenesController.ListMutilmediaInfomation[idType].cost, 0);
            }
            else
            {
                Close_Click();
                GameObject.Find("CommonObject").GetComponent<CommonObjectScript>().ChangeDolar(TownScenesController.ListMutilmediaInfomation[idType].cost - CommonObjectScript.dollar);
            }


        }
    }

    void CreateTimer()
    {
        timerClone = (GameObject)Instantiate(timerPrefabs, transform.localPosition, transform.rotation);
        timerClone.GetComponent<TimerController>().time = TownScenesController.ListMutilmediaInfomation[idType].time;
        timerClone.GetComponent<TimerController>().minRation = TownScenesController.ListMutilmediaInfomation[idType].minRation;
        timerClone.GetComponent<TimerController>().maxRation = TownScenesController.ListMutilmediaInfomation[idType].maxRation;
        timerClone.transform.position = new Vector3(-0.48f, 0.05f, 0);
        timerClone.transform.localScale = new Vector3(-0.0028f, 0.0028f, 0.0028f);
        timerClone.name = "TimerMutilMedia";
    }
    void CreateContainerCoinEfect()
    {
        ContainerCoinEfectClone = (GameObject)Instantiate(ContainerCoinEfectPrefabs, transform.position, transform.rotation);
        ContainerCoinEfectClone.GetComponent<CoinEfectController>().timeDelay = 0f;
        ContainerCoinEfectClone.GetComponent<CoinEfectController>().costProduct = TownScenesController.ListMutilmediaInfomation[idType].cost;
        ContainerCoinEfectClone.transform.parent = PanelLabel.transform;
        ContainerCoinEfectClone.transform.localPosition = new Vector3(180f, -146f, 0);
        ContainerCoinEfectClone.transform.localScale = new Vector3(1f, 1f, 1f);
        ContainerCoinEfectClone.name = ContainerCoinEfectPrefabs.name;
    }

    public void Flyer_Click()
    {
        //scrollView.transform.localPosition = new Vector2(-275f, 143);
        // scrollView.GetComponent<UIPanel>().clipOffset = new Vector2(0, -143);
        SetPer(scrollView.transform.gameObject, new Vector2(-275, 143), new Vector2(0, -143));
        isChangePos = true;

        SetDataClickEven(listType[0], 0, 18);
    }

    public void Magazine_Click()
    {
        SetDataClickEven(listType[1], 1, 53);
    }

    public void Internet_Click()
    {
        SetDataClickEven(listType[2], 2, 57);
    }

    public void Radio_Click()
    {
        SetDataClickEven(listType[3], 3, 64);
    }

    public void Tivi_Click()
    {
        //scrollView.transform.localPosition = new Vector2(-275f, 507);
        //scrollView.GetComponent<UIPanel>().clipOffset = new Vector2(0, -507);

        SetPer(scrollView.transform.gameObject, new Vector2(-275f, 507), new Vector2(0, -507));
        isChangePos = true;

        SetDataClickEven(listType[4], 4, 68);
    }
    void SetData(int idType)
    {
        labelDescription[0].text = TownScenesController.languageTowns["TextTypeMul"] + TownScenesController.languageTowns[TownScenesController.ListMutilmediaInfomation[idType].engName];
        listlabelData[0].text = TownScenesController.ListMutilmediaInfomation[idType].minRation + "-" + TownScenesController.ListMutilmediaInfomation[idType].maxRation + "%";
        listlabelData[1].text = (TownScenesController.ListMutilmediaInfomation[idType].time / 24).ToString() + ((TownScenesController.ListMutilmediaInfomation[idType].time / 24) <= 1 ? FactoryScenesController.languageHungBV["DAY"] : FactoryScenesController.languageHungBV["DAYS"]);
        listlabelData[2].text = TownScenesController.ListMutilmediaInfomation[idType].cost.ToString();
        listlabelData[3].text = TownScenesController.languageTowns[TownScenesController.ListMutilmediaInfomation[idType].engName];
        for (int countType = 0; countType < listLabelNameType.Length; countType++)
        {
            listLabelNameType[countType].text = TownScenesController.languageTowns[TownScenesController.ListMutilmediaInfomation[countType].engName];
        }
    }

    void SetDescription()
    {

        textFrame[0].text = TownScenesController.languageTowns["MULTIMEDIA"];
        textFrame[1].text = TownScenesController.languageTowns["Apply"];
        labelDescription[1].text = TownScenesController.languageTowns["IncreaseCustomer"];
        labelDescription[2].text = TownScenesController.languageTowns["Time"];
        labelDescription[3].text = TownScenesController.languageTowns["Cost"];
    }

    public void DestroyObj()
    {
        isApply = false;
        this.gameObject.SetActive(false);
        CommonObjectScript.isViewPoppup = false;
        CreatTownScenesController.isDenyContinue = false;
    }
    public void ResetScrollPosition()
    {
        scrollView.GetComponentInChildren<UIScrollView>().GetComponent<Transform>().localPosition = new Vector3(-275, 135, 0);
        scrollView.GetComponentInChildren<UIScrollView>().GetComponent<UIPanel>().clipOffset = new Vector2(0, -135);
    }

    void setLockType()
    {
        for (int i = 0; i < listType.Length; i++)
        {
            SetColorLock(listType[i], listLabelNameType[i], texttureLockForms[i]);
        }
    }
    void setUnLockType()
    {
        foreach (int id in MissionData.townDataMission.typesMedia)
        {
            SetColorUnlock(listType[id], listLabelNameType[id], texttureLockForms[id]);
        }
    }

    void SetColorLock(UIButton button, UILabel label, UITexture textureLock)
    {
        button.defaultColor = new Color32(70, 70, 70, 255);
        button.hover = new Color32(70, 70, 70, 255);
        button.pressed = new Color32(70, 70, 70, 255);
        button.disabledColor = new Color32(70, 70, 70, 255);

        button.GetComponent<UIButtonScale>().enabled = false;
        label.color = new Color32(70, 70, 70, 255);
        textureLock.enabled = true;
    }
    void SetColorUnlock(UIButton button, UILabel label, UITexture textureLock)
    {
        button.pressed = new Color32(183, 163, 123, 255);
        button.hover = new Color32(225, 200, 150, 255);
        button.disabledColor = new Color32(128, 128, 128, 255);
        button.defaultColor = new Color32(255, 255, 255, 255);

        button.GetComponent<UIButtonScale>().enabled = true;
        label.color = new Color32(255, 255, 255, 255);
        textureLock.enabled = false;
    }

    void SetDataClickEven(UIButton button, int id, int missionUnlock)
    {
        audioControl.PlaySound("Click 1");
        if (button.defaultColor == new Color32(70, 70, 70, 255))
        {
            labelLock.text = TownScenesController.languageTowns["Unlock"] + missionUnlock;
            labelLock.gameObject.SetActive(true);
            PanelLabel.gameObject.SetActive(false);
            //SetStatusInforUnder(false);
            ResetStatusPreButton();
        }
        else
        {
            labelLock.gameObject.SetActive(false);
            PanelLabel.gameObject.SetActive(true);
            SetData(id);
            idType = id;
            SetStatusButtonAfterClick(button, id);
        }
    }
    void SetStatusButtonAfterClick(UIButton button, int iD)
    {
        ResetStatusPreButton();
        button.defaultColor = new Color32(183, 163, 123, 255);
        listLabelNameType[iD].color = new Color32(183, 163, 123, 255);
        button.GetComponent<UIButton>().enabled = false;
        button.GetComponent<UIButtonScale>().enabled = false;
        button.transform.localScale = new Vector3(1.1f, 1.1f, 1.1f);
        buttonSelected = button;
        iDPre = iD;
    }
    void ResetStatusPreButton()
    {
        if (buttonSelected != null)
        {
            buttonSelected.defaultColor = new Color32(255, 255, 255, 255);
            listLabelNameType[iDPre].color = new Color32(255, 255, 255, 255);
            buttonSelected.GetComponent<UIButton>().enabled = true;
            buttonSelected.GetComponent<UIButtonScale>().enabled = true;
        }
    }

    void AddCommonObject(int dollar, int diamond)
    {
        GameObject commonObject = GameObject.Find("CommonObject");
        if (commonObject != null)
        {

            commonObject.GetComponent<CommonObjectScript>().AddDollar(dollar);
            commonObject.GetComponent<CommonObjectScript>().AddDiamond(diamond);

        }
    }

    void ChangeScroll(Vector2 perPos, Vector2 perOff, bool Add = false)
    {
        if (isChangePos)
        {
            if (tempChange < 20)
            {
                if (Add)
                {
                    scrollView.transform.localPosition = new Vector2(scrollView.transform.localPosition.x + perPos.x, scrollView.transform.localPosition.y + perPos.y);
                    scrollView.GetComponent<UIPanel>().clipOffset = new Vector2(scrollView.GetComponent<UIPanel>().clipOffset.x + perOff.x, scrollView.GetComponent<UIPanel>().clipOffset.y + perOff.y);
                }
                else
                {
                    scrollView.transform.localPosition = new Vector2(scrollView.transform.localPosition.x - perPos.x, scrollView.transform.localPosition.y - perPos.y);
                    scrollView.GetComponent<UIPanel>().clipOffset = new Vector2(scrollView.GetComponent<UIPanel>().clipOffset.x - perOff.x, scrollView.GetComponent<UIPanel>().clipOffset.y - perOff.y);
                }
                tempChange += 1;
            }
            else
            {
                tempChange = 0;
                isChangePos = false;
            }
        }
    }
    void SetPer(GameObject ScrollView, Vector2 targetPos, Vector2 targetOff)
    {
        perChangePos.x = (ScrollView.transform.localPosition.x - targetPos.x) / 20;
        perChangePos.y = (ScrollView.transform.localPosition.y - targetPos.y) / 20;

        perChangeOff.x = (ScrollView.GetComponent<UIPanel>().clipOffset.x - targetOff.x) / 20;
        perChangeOff.y = (ScrollView.GetComponent<UIPanel>().clipOffset.y - targetOff.y) / 20;
    }
}
