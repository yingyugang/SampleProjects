using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Common;

public class MaketResearchController : MonoBehaviour
{
    // for control button Forms
    public UIButton[] buttonForms;
    public UILabel[] labelOther;
    public UILabel[] labelButtonForms;
    public UITexture[] texttureLockForms;
    private UIButton buttonSelected;
    private int iDPre;
    private int idButton;

    // For control button Item
    public UIButton[] buttonItem;
    private static Texture[] listSpriteItem;
    public Texture[] imageCost;
    public UITexture textureCost;
    private UIButton buttonItemSelected;
    private int iDItemPre;
    private int idButtonItem;
    private string textTimeHours;
    private string textTimeDays;
    private string costButtonItem;

    // For control All
    public UIPanel panelLabelInfor;
    public UIScrollView ScrollItem;
    public UIScrollView ScrollForm;
    public UILabel labelInfor;
    public UILabel[] arrayInforItemLabel;

    // For Apply Button
    public GameObject[] costEfectPrefabs;
    private GameObject costEfectClone;
    private bool isApply; // đã click OK chưa
    private bool isCloseClick; // click vào nút colse hay apply
    private int iDButtonComfirmApply;
    public GameObject panelCofirmApply;
    public GameObject timerPrefabs;
    private GameObject timerClone;
    public UIPanel maketPopup;

    private GameObject commonObject;
    AudioControl audioControl;

    private bool isChangePos;
    private bool isChangePosItem;
    private int tempChange;
    private Vector2 perChangePos;
    private Vector2 perChangeOff;

    // private List<int> listUnlock;
    void OnEnable()
    {
        CommonObjectScript.isViewPoppup = true;
        if (TownScenesController.isHelp)
        {
            if (VariableSystem.mission == 11 || VariableSystem.mission == 50)
            {
                ScrollItem.GetComponent<UIScrollView>().enabled = false;
                ScrollForm.GetComponent<UIScrollView>().enabled = false;
            }
        }
        else
        {
            ScrollItem.GetComponent<UIScrollView>().enabled = true;
            ScrollForm.GetComponent<UIScrollView>().enabled = true;
        }

        if (audioControl == null)
            audioControl = GameObject.Find("AudioControl").GetComponent<AudioControl>();
        listSpriteItem = Resources.LoadAll<Texture>("Town/ImageMaketReseach");

        StartData();
       
    }
    void Start()
    {

        commonObject = GameObject.Find("CommonObject");
        if (TownScenesController.isHelp)
        {
            if (VariableSystem.mission == 50)
            {
               ScrollForm.transform.localPosition = new Vector2(-370,316.8f);
              ScrollForm.GetComponent<UIPanel>().clipOffset = new Vector2(0, -316.8f);
            }
        }
        ControlViewHelp();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || CommonObjectScript.isEndGame || CommonObjectScript.isViewComplete)
        {
            if (!TownScenesController.isHelp)
                Colse_Click();
        }

        if (idButton == 0)
        {
            // ChangeScrollForm(-121, 121);
            ChangeScrollForm(perChangePos, perChangeOff);
        }
        else if (idButton == 3)
        {
            // ChangeScrollForm(338, -338);
            ChangeScrollForm(perChangePos, perChangeOff, true);
        }

        if (idButtonItem == 0)
        {
            ChangeScrollItem(perChangePos, perChangeOff);
        }
        else if (idButtonItem == 3)
        {
            ChangeScrollItem(perChangePos, perChangeOff);
        }
        else if (idButtonItem == 4)
        {
            ChangeScrollItem(perChangePos, perChangeOff);
        }
    }
    void OnApplicationPause(bool pauseStatus)
    {
        if (!pauseStatus)
        {
            if (!FactoryScenesController.isHelp)
                Colse_Click();
        }
    }
    void setTextLabelButtonForms()
    {
        for (int i = 0; i < labelButtonForms.Length; i++)
        {
            labelButtonForms[i].text = TownScenesController.MaketResearchInforType[i].name;
        }
    }
    void setTextLabelOther()
    {
        labelOther[0].text = TownScenesController.languageTowns["MARKETRESEARCH"];
        labelOther[1].text = TownScenesController.languageTowns["Percent"];
        labelOther[2].text = TownScenesController.languageTowns["Time"];
        labelOther[3].text = TownScenesController.languageTowns["Cost"];
        labelOther[4].text = TownScenesController.languageTowns["Apply"];

    }
    void setDataInfor(int iD)
    {

        labelInfor.text = TownScenesController.MaketResearchInforType[iD].infor;
        labelInfor.fontSize = 50;
    }
    public void StartData()
    {
        setTextLabelOther();
        // listUnlock = new List<int> { 0, 1 };
        setTextLabelButtonForms();
        setDataInfor(0);
        setLockButtonForms();
        setUnLockButtonForms();
        SetDataClickEven(buttonForms[0], 0, 11, true);

    }

    void setImagesButtonItem(int iDParent)
    {
        int idTemp = 5 * iDParent;
        if (listSpriteItem != null)
        {
            for (int i = 0; i < buttonItem.Length; i++)
            {

                buttonItem[i].GetComponent<UITexture>().mainTexture = listSpriteItem[idTemp + i];

            }
        }
        else
        {
            print("null nay");
        }
    }

    #region Button Forms Click Even
    public void Specialist_Click()
    {
        if (!TownScenesController.isHelp)
            SetDataClickEven(buttonForms[0], 0, 11, true);
        else
        {
            if (VariableSystem.mission == 11)
            {
                if (CreatAndControlPanelHelp.countClickHelpPanel == 4)
                {
                    SetDataClickEven(buttonForms[0], 0, 14, true);
                    ChangeHelpPosition("CircleHelp", new Vector3(-115, -15, 6));
                    ChangeHelpPosition("HandHelp", new Vector3(-90, -35, 0));
                    CreatAndControlPanelHelp.countClickHelpPanel = 5;
                }
            }
        }

        SetPer(ScrollForm.transform.gameObject, new Vector2(-369.94f, 121f), new Vector2(0, -121f));
        isChangePos = true;
        //this.transform.parent.parent.GetComponent<UIPanel>().clipOffset = new Vector2(0, -118f);
        // ScrollForm.transform.localPosition = new Vector2(-369.94f, 121f);
        //  ScrollForm.GetComponent<UIPanel>().clipOffset = new Vector2(0, -121f);

    }
    public void City_Click()
    {
        if (!TownScenesController.isHelp)
            SetDataClickEven(buttonForms[1], 1, 35, true);
    }
    public void ParticalTrip_Click()
    {
        if (!TownScenesController.isHelp)
            SetDataClickEven(buttonForms[2], 2, 50, false);
        else
        {
            if (VariableSystem.mission == 50)
            {
                if (CreatAndControlPanelHelp.countClickHelpPanel == 2)
                {
                    SetDataClickEven(buttonForms[2], 2, 50, false);
                    ChangeHelpPosition("CircleHelp", new Vector3(125, -300, 6));
                    ChangeHelpPosition("HandHelp", new Vector3(90, -300, 0));
                    CreatAndControlPanelHelp.countClickHelpPanel = 3;
                }
            }
        }
    }
    public void Testing_Click()
    {
        if (!TownScenesController.isHelp)
            SetDataClickEven(buttonForms[3], 3, 73, false);
        //ScrollForm.transform.localPosition = new Vector2(-369.94f, 338f);
        // ScrollForm.GetComponent<UIPanel>().clipOffset = new Vector2(0, -338f);
        SetPer(ScrollForm.transform.gameObject, new Vector2(-369.94f, 338f), new Vector2(0, -338f));
        isChangePos = true;
    }

    void SetDataClickEven(UIButton button, int id, int missionUnlock, bool visible)
    {
        audioControl.PlaySound("Click 1");
        if (button.defaultColor == new Color32(70, 70, 70, 255))
        {
            labelInfor.text = TownScenesController.languageTowns["Unlock"] + missionUnlock;
            labelInfor.fontSize = 90;
            SetStatusInforUnder(false);
            ResetStatusPreButton();
        }
        else
        {

            SetStatusButtonAfterClick(button, id);
            SetStatusInforUnder(visible);
            idButton = id;
            setDataInfor(id);
            if (id == 0 || id == 1)
            {
                setImagesButtonItem(id);
                SetDataEvenClickButtonItem(0);
            }
        }
    }
    void SetStatusInforUnder(bool visible)
    {
        panelLabelInfor.enabled = visible;
        ScrollItem.gameObject.SetActive(visible);
    }
    void SetStatusButtonAfterClick(UIButton button, int iD)
    {
        if (!TownScenesController.isHelp)
        {
            ResetStatusPreButton();
            button.defaultColor = new Color32(183, 163, 123, 255);
            labelButtonForms[iD].color = new Color32(183, 163, 123, 255);

            button.GetComponent<UIButton>().enabled = false;
            button.GetComponent<UIButtonScale>().enabled = false;
            button.transform.localScale = new Vector3(1.1f, 1.1f, 1.1f);
            buttonSelected = button;
            iDPre = iD;
        }
        else
        {
            if (VariableSystem.mission == 11 && CreatAndControlPanelHelp.countClickHelpPanel == 4)
            {
                ResetStatusPreButton();
                button.defaultColor = new Color32(183, 163, 123, 255);
                labelButtonForms[iD].color = new Color32(183, 163, 123, 255);
                button.transform.localScale = new Vector3(1.1f, 1.1f, 1.1f);
                buttonSelected = button;
                iDPre = iD;
            }
        }
    }
    void ResetStatusPreButton()
    {
        if (buttonSelected != null)
        {
            buttonSelected.defaultColor = new Color32(255, 255, 255, 255);
            labelButtonForms[iDPre].color = new Color32(255, 255, 255, 255);
            buttonSelected.GetComponent<UIButton>().enabled = true;
            buttonSelected.GetComponent<UIButtonScale>().enabled = true;
        }
    }

    void setLockButtonForms()
    {
        for (int i = 0; i < buttonForms.Length; i++)
        {
            SetColorLock(buttonForms[i], labelButtonForms[i], texttureLockForms[i]);
        }
    }
    void setUnLockButtonForms()
    {
        // if (TownScenesController.isHelp)
        {

        }
        //  else
        {
            foreach (int id in MissionData.townDataMission.typesResearch)
            {
                SetColorUnlock(buttonForms[id], labelButtonForms[id], texttureLockForms[id]);

            }
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
    #endregion

    #region Button item Click Even
    public void Item1_Click()
    {
        if (!TownScenesController.isHelp)
            SetDataEvenClickButtonItem(0);
        else
        {
            if (VariableSystem.mission == 11 && CreatAndControlPanelHelp.countClickHelpPanel == 5)
            {
                SetDataEvenClickButtonItem(0);
                ChangeHelpPosition("CircleHelp", new Vector3(125, -300, 6));
                ChangeHelpPosition("HandHelp", new Vector3(90, -300, 0));
                CreatAndControlPanelHelp.countClickHelpPanel = 6;
            }
        }
        // ScrollItem.transform.localPosition = new Vector2(131, 122f);
        // ScrollItem.GetComponent<UIPanel>().clipOffset = new Vector2(-2, -135);
        SetPer(ScrollItem.gameObject, new Vector2(131, 122f), new Vector2(-2, -135));
        isChangePosItem = true;
    }
    public void Item2_Click()
    {
        if (!TownScenesController.isHelp)
            SetDataEvenClickButtonItem(1);
    }
    public void Item3_Click()
    {
        if (!TownScenesController.isHelp)
            SetDataEvenClickButtonItem(2);
    }
    public void Item4_Click()
    {
        if (!TownScenesController.isHelp)
            SetDataEvenClickButtonItem(3);
        //ScrollItem.transform.localPosition = new Vector2(-108, 122f);
        //ScrollItem.GetComponent<UIPanel>().clipOffset = new Vector2(237, -135);
        SetPer(ScrollItem.gameObject, new Vector2(-108, 122f), new Vector2(237, -135));
        isChangePosItem = true;
    }
    public void Item5_Click()
    {
        if (!TownScenesController.isHelp)
            SetDataEvenClickButtonItem(4);
        //ScrollItem.transform.localPosition = new Vector2(-108, 122f);
        //ScrollItem.GetComponent<UIPanel>().clipOffset = new Vector2(237, -135);
        SetPer(ScrollItem.gameObject, new Vector2(-108, 122f), new Vector2(237, -135));
        isChangePosItem = true;
    }

    void SetStatusButtonItemAfterClick(UIButton button, int iD)
    {
        if (!TownScenesController.isHelp)
        {
            ResetStatusPreButtonItem();
            button.defaultColor = new Color32(183, 163, 123, 255);
            button.GetComponent<UIButton>().enabled = false;
            button.GetComponent<UIButtonScale>().enabled = false;
            button.transform.localScale = new Vector3(1.1f, 1.1f, 1.1f);
            buttonItemSelected = button;
            iDItemPre = iD;
        }
        else
        {
            ResetStatusPreButtonItem();
            button.defaultColor = new Color32(183, 163, 123, 255);
            button.transform.localScale = new Vector3(1.1f, 1.1f, 1.1f);
            buttonItemSelected = button;
            iDItemPre = iD;
        }
    }
    void ResetStatusPreButtonItem()
    {
        if (buttonItemSelected != null)
        {
            buttonItemSelected.defaultColor = new Color32(255, 255, 255, 255);
            buttonItemSelected.GetComponent<UIButton>().enabled = true;
            buttonItemSelected.GetComponent<UIButtonScale>().enabled = true;
        }
    }
    void SetDataEvenClickButtonItem(int id)
    {
        audioControl.PlaySound("Click 1");
        idButtonItem = idButton * 5 + id;
        SetStatusButtonItemAfterClick(buttonItem[id], id);
        arrayInforItemLabel[0].text = TownScenesController.ListMaketResearchItem[idButtonItem].precisionMin + "-" + TownScenesController.ListMaketResearchItem[idButtonItem].precisionMax + "%";
        arrayInforItemLabel[1].text = ChangeTimeToText(TownScenesController.ListMaketResearchItem[idButtonItem].time);
        arrayInforItemLabel[2].text = SetCostItem(idButtonItem);
    }

    string ChangeTimeToText(int timeLeftClone)
    {

        int dayLeft = (int)(timeLeftClone) / 24;
        int hourLeft = (int)(timeLeftClone) % 24;
        if (hourLeft <= 0)
        {
            textTimeHours = "";
        }
        else if (hourLeft == 1)
        {
            textTimeHours = "1 " + FactoryScenesController.languageHungBV["HOUR"];
        }
        else if (hourLeft > 1)
        {
            textTimeHours = hourLeft.ToString() + " " + FactoryScenesController.languageHungBV["HOURS"];
        }
        if (dayLeft <= 0)
        {
            textTimeDays = "";
        }
        else if (dayLeft == 1)
        {
            textTimeDays = "1 " + FactoryScenesController.languageHungBV["DAY"];
        }
        else if (dayLeft > 1)
        {
            textTimeDays = dayLeft.ToString() + " " + FactoryScenesController.languageHungBV["DAYS"];
        }
        return ((textTimeDays + textTimeHours) != "" ? (textTimeDays + textTimeHours) : TownScenesController.languageTowns["Immediately"]);
    }
    string SetCostItem(int iDButtonItem)
    {
        if (TownScenesController.ListMaketResearchItem[iDButtonItem].costDiamond != 0)
        {
            costButtonItem = TownScenesController.ListMaketResearchItem[iDButtonItem].costDiamond.ToString();
            textureCost.GetComponent<UITexture>().mainTexture = imageCost[0];
            textureCost.transform.localPosition = new Vector3(160f, textureCost.transform.localPosition.y, textureCost.transform.localPosition.z);
        }
        else
        {
            costButtonItem = DString.ConvertToMoneyString(TownScenesController.ListMaketResearchItem[iDButtonItem].costCoin.ToString());
            textureCost.GetComponent<UITexture>().mainTexture = imageCost[1];
            textureCost.transform.localPosition = new Vector3(220f, textureCost.transform.localPosition.y, textureCost.transform.localPosition.z);
        }
        return costButtonItem;
    }
    #endregion

    public void Colse_Click()
    {
        audioControl.PlaySound("Click 1");
        if (!TownScenesController.isHelp)
        {
            isCloseClick = true;
            this.gameObject.GetComponent<Animator>().Play("Invisible");
        }
    }
    void EndEvenAnimation()
    {
        CommonObjectScript.isViewPoppup = false;
        if (!isCloseClick)
        {
            if (commonObject.transform.Find("Btn_Result").gameObject.activeInHierarchy)
            {
                panelCofirmApply.gameObject.SetActive(true);
                panelCofirmApply.GetComponent<Animator>().Play("Visible");
            }
            else
            {
                // print("tiennnnnnnnnnnnnnnnnnnn  " + TownScenesController.ListMaketResearchItem[idButtonItem].costCoin);
                //if (!isApply)
                {
                    if (idButton == 2)
                    {
                        commonObject.GetComponent<CommonObjectScript>().ResultButtonVisible();
                        commonObject.GetComponent<CommonObjectScript>().typeReasearch = 1;

                        //Application.LoadLevel("VilageResearch");
                        VilageResearchController.ResetVilage();
                        LoadingScene.ShowLoadingScene("VilageResearch", true);
                        DialogAchievement.AddDataAchievement(12, 1);
                    }
                    else if (idButton == 3)
                    {
                        maketPopup.gameObject.SetActive(true);
                    }
                    else
                    {
                        if (CommonObjectScript.dollar >= TownScenesController.ListMaketResearchItem[idButtonItem].costCoin && VariableSystem.diamond >= TownScenesController.ListMaketResearchItem[idButtonItem].costDiamond)
                        {
                            TownScenesController.townsBusy[3] = true;
                            if (idButtonItem > 4)
                            {
                                CreateTimer(TownScenesController.ListMaketResearchItem[idButtonItem].time);
                                DialogAchievement.AddDataAchievement(13, 1);
                            }
                            else
                            {
                                CreateTimer(0);
                                DialogAchievement.AddDataAchievement(14, 1);
                            }
                            AddCommonObject(-TownScenesController.ListMaketResearchItem[idButtonItem].costCoin, -TownScenesController.ListMaketResearchItem[idButtonItem].costDiamond);
                            VilageResearchController.ResetVilage();
                        }
                        else
                        {

                            if (TownScenesController.ListMaketResearchItem[idButtonItem].costCoin != 0)
                            {
                                GameObject.Find("CommonObject").GetComponent<CommonObjectScript>().ChangeDolar(TownScenesController.ListMaketResearchItem[idButtonItem].costCoin - CommonObjectScript.dollar);
                            }
                            else
                            {
                                DialogInapp.ShowInapp();
                            }
                        }
                        CreatTownScenesController.isDenyContinue = false;
                    }
                    //isApply = true;
                }
                this.gameObject.SetActive(false);
            }

        }
        else
        {
            this.gameObject.SetActive(false);
            CreatTownScenesController.isDenyContinue = false;
        }
    }
    public void Apply_Click()
    {
        audioControl.PlaySound("Click 1");
        if (!TownScenesController.isHelp)
        {
            isCloseClick = false;
            this.gameObject.GetComponent<Animator>().Play("Invisible");
        }
        else
        {
            if (VariableSystem.mission == 11 && CreatAndControlPanelHelp.countClickHelpPanel == 6)
            {
                DestroyObjecHelp("CircleHelp");
                DestroyObjecHelp("HandHelp");
                CreatAndControlPanelHelp.countClickHelpPanel = 7;
                isCloseClick = false;
                this.gameObject.GetComponent<Animator>().Play("Invisible");

            }
            else
                if (VariableSystem.mission == 50 && CreatAndControlPanelHelp.countClickHelpPanel == 3)
                {
                    DestroyObjecHelp("CircleHelp");
                    DestroyObjecHelp("HandHelp");
                    CreatAndControlPanelHelp.countClickHelpPanel = 4;
                    isCloseClick = false;
                    this.gameObject.GetComponent<Animator>().Play("Invisible");
                }
        }
    }

    public void OKComfirm_ClickComfirm()
    {
        audioControl.PlaySound("Click 1");
        iDButtonComfirmApply = 1;
        panelCofirmApply.GetComponent<Animator>().Play("InVisible");
        if (!isApply)
        {
            if (idButton == 0 || idButton == 1)
            {
                if (CommonObjectScript.dollar >= TownScenesController.ListMaketResearchItem[idButtonItem].costCoin && VariableSystem.diamond >= TownScenesController.ListMaketResearchItem[idButtonItem].costDiamond)
                {

                    TownScenesController.townsBusy[3] = true;
                    if (idButtonItem > 4)
                    {
                        CreateTimer(TownScenesController.ListMaketResearchItem[idButtonItem].time);
                        DialogAchievement.AddDataAchievement(13, 1);
                    }
                    else
                    {
                        CreateTimer(0);
                        DialogAchievement.AddDataAchievement(14, 1);
                    }
                    AddCommonObject(-TownScenesController.ListMaketResearchItem[idButtonItem].costCoin, -TownScenesController.ListMaketResearchItem[idButtonItem].costDiamond);
                    VilageResearchController.ResetVilage();
                }
                else
                {
                    // print(TownScenesController.ListMaketResearchItem[idButtonItem].costCoin + "aaaaaaaaaaaaaa");
                    if (TownScenesController.ListMaketResearchItem[idButtonItem].costCoin != 0)
                        GameObject.Find("CommonObject").GetComponent<CommonObjectScript>().ChangeDolar(TownScenesController.ListMaketResearchItem[idButtonItem].costCoin - CommonObjectScript.dollar);
                    else
                    {
                        DialogInapp.ShowInapp();
                    }
                }
            }
            isApply = true;
        }
    }
    public void CancelComfirm_Click()
    {
        audioControl.PlaySound("Click 1");
        iDButtonComfirmApply = 2;
        panelCofirmApply.GetComponent<Animator>().Play("InVisible");
    }

    public void EndAnimationComrfirm()
    {
        //CommonObjectScript.isViewPoppup = false;
        if (iDButtonComfirmApply == 1)
        {
            this.gameObject.SetActive(false);
            if (idButton == 2)
            {
                commonObject.GetComponent<CommonObjectScript>().ResultButtonVisible();
                commonObject.GetComponent<CommonObjectScript>().typeReasearch = 1;

                //Application.LoadLevel("VilageResearch");
                LoadingScene.ShowLoadingScene("VilageResearch", true);
                VilageResearchController.ResetVilage();
            }
            else if (idButton == 3)
            {
                maketPopup.gameObject.SetActive(true);
            }
            else
                CreatTownScenesController.isDenyContinue = false;
        }
        else if (iDButtonComfirmApply == 2)
        {
            //  this.gameObject.SetActive(false);
        }
        isApply = false;
    }

    void CreateTimer(int time)
    {
        timerClone = (GameObject)Instantiate(timerPrefabs, transform.localPosition, transform.rotation);
        timerClone.GetComponent<TimerController>().time = time;
        timerClone.GetComponent<TimerController>().minRation = TownScenesController.ListMaketResearchItem[idButtonItem].precisionMin;
        timerClone.GetComponent<TimerController>().maxRation = TownScenesController.ListMaketResearchItem[idButtonItem].precisionMax;
        timerClone.transform.position = new Vector3(0.01f, 0.53f, 0);
        timerClone.transform.localScale = new Vector3(-0.0028f, 0.0028f, 0.0028f);
        timerClone.name = "TimerMaketResearch";
    }
    void AddCommonObject(int dollar, int diamond)
    {
        commonObject.GetComponent<CommonObjectScript>().AddDollar(dollar);
        commonObject.GetComponent<CommonObjectScript>().AddDiamond(diamond);
    }

    void CreatObjectHelp(string nameObject, Vector3 vectorScale, Vector3 localPosition)
    {
        Transform objectPre = transform.Find(nameObject);
        if (objectPre == null)
        {
            GameObject objectPrefabs = (GameObject)Resources.Load("Help/" + nameObject);
            GameObject objectClone = (GameObject)Instantiate(objectPrefabs);
            Transform[] child = objectClone.transform.GetComponentsInChildren<Transform>();
            foreach (Transform ts in child)
            {
                ts.gameObject.layer = 5;
            }

            objectClone.transform.parent = gameObject.transform;
            objectClone.transform.localPosition = localPosition;
            objectClone.transform.localScale = vectorScale;
            objectClone.name = objectPrefabs.name;
        }
    }
    void DestroyObjecHelp(string nameObject)
    {
        Transform objectClonePre = transform.Find(nameObject);
        if (objectClonePre != null)
        {
            Destroy(objectClonePre.gameObject);
        }
    }
    void ControlViewHelp()
    {
        if (TownScenesController.isHelp)
        {
            if (VariableSystem.mission == 11)
            {
                if (CreatAndControlPanelHelp.countClickHelpPanel == 4)
                {
                    CreatObjectHelp("CircleHelp", new Vector3(40f, 40f, 40f), new Vector3(-373, 135, 0));
                    CreatObjectHelp("HandHelp", new Vector3(100f, 100f, 100f), new Vector3(-360, 120, 0));
                }
            }
            else
            {
                if (VariableSystem.mission == 50)
                {
                    if (CreatAndControlPanelHelp.countClickHelpPanel == 2)
                    {
                        CreatObjectHelp("CircleHelp", new Vector3(40f, 40f, 40f), new Vector3(-373, -8, 0));
                        CreatObjectHelp("HandHelp", new Vector3(100f, 100f, 100f), new Vector3(-360, -20, 0));
                    }
                }
            }
        }
    }
    void ChangeHelpPosition(string nameObject, Vector3 positionChange)
    {
        Transform objectClonePre = transform.Find(nameObject);
        if (objectClonePre != null)
        {
            objectClonePre.transform.localPosition = positionChange;
        }
    }

    void ChangeScrollForm(Vector2 perPos, Vector2 perOff, bool Add = false)
    {
        if (isChangePos)
        {
            if (tempChange < 20)
            {
                if (Add)
                {
                    ScrollForm.transform.localPosition = new Vector2(ScrollForm.transform.localPosition.x + perPos.x, ScrollForm.transform.localPosition.y + perPos.y);
                    ScrollForm.GetComponent<UIPanel>().clipOffset = new Vector2(ScrollForm.GetComponent<UIPanel>().clipOffset.x + perOff.x, ScrollForm.GetComponent<UIPanel>().clipOffset.y + perOff.y);
                }
                else
                {
                    ScrollForm.transform.localPosition = new Vector2(ScrollForm.transform.localPosition.x - perPos.x, ScrollForm.transform.localPosition.y - perPos.y);
                    ScrollForm.GetComponent<UIPanel>().clipOffset = new Vector2(ScrollForm.GetComponent<UIPanel>().clipOffset.x - perOff.x, ScrollForm.GetComponent<UIPanel>().clipOffset.y - perOff.y);
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
    void ChangeScrollItem(Vector2 perPos, Vector2 perOff, bool Add = false)
    {
        if (isChangePosItem)
        {
            if (tempChange < 20)
            {
                if (Add)
                {
                    ScrollItem.transform.localPosition = new Vector2(ScrollItem.transform.localPosition.x + perPos.x, ScrollItem.transform.localPosition.y + perPos.y);
                    ScrollItem.GetComponent<UIPanel>().clipOffset = new Vector2(ScrollItem.GetComponent<UIPanel>().clipOffset.x + perOff.x, ScrollItem.GetComponent<UIPanel>().clipOffset.y + perOff.y);
                }
                else
                {
                    ScrollItem.transform.localPosition = new Vector2(ScrollItem.transform.localPosition.x - perPos.x, ScrollItem.transform.localPosition.y - perPos.y);
                    ScrollItem.GetComponent<UIPanel>().clipOffset = new Vector2(ScrollItem.GetComponent<UIPanel>().clipOffset.x - perOff.x, ScrollItem.GetComponent<UIPanel>().clipOffset.y - perOff.y);
                }
                tempChange += 1;
            }
            else
            {
                tempChange = 0;
                isChangePosItem = false;
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
    //void CreateContainerCoinEfect()
    //{
    //    if (TownScenesController.ListMaketResearchItem[idButtonItem].costDiamond != 0)
    //    {
    //        costEfectClone = (GameObject)Instantiate(costEfectPrefabs[0], transform.position, transform.rotation);
    //        costEfectClone.transform.parent = panelLabelInfor.transform;
    //        costEfectClone.GetComponent<DiamondEffectScript>().setValueDiamond(-TownScenesController.ListMaketResearchItem[idButtonItem].costDiamond, 3);
    //        costEfectClone.transform.localPosition = new Vector3(150f, -270f, 0);
    //        costEfectClone.transform.localScale = new Vector3(1f, 1f, 1f);
    //    }
    //    else
    //    {
    //        costEfectClone = (GameObject)Instantiate(costEfectPrefabs[1], transform.position, transform.rotation);
    //        costEfectClone.GetComponent<CoinEfectController>().timeDelay = 0f;
    //        costEfectClone.GetComponent<CoinEfectController>().costProduct = TownScenesController.ListMaketResearchItem[idButtonItem].costCoin;
    //        costEfectClone.transform.parent = panelLabelInfor.transform;
    //        costEfectClone.transform.localPosition = new Vector3(180f, -241f, 0);
    //        costEfectClone.transform.localScale = new Vector3(1f, 1f, 1f);
    //    }
    //}
}
