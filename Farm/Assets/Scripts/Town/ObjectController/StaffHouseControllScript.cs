using UnityEngine;
using System.Collections;
using System.Xml;
using Assets.Scripts.Common;
using System;

public class StaffHouseControllScript : MonoBehaviour
{
    public UILabel[] labelsInPopup;
    //0-title, 1-Name,2-xName,3-Level,4-xLevel,5-speed,6-xspeed,7-health,8-xhealth,9-mental,10-xmental,11-cost,12-xcost,13-btnHire
    //14,15,16 tempindex

    public UITexture[] KhungValues;
    public Transform BtnInPopup;
    public Transform[] iconStaffs;
    public StaffTimerScript staffTimer;
    private GameObject valueAdd;
    private ShopCenterScript shopcenter;
    private UIPanel listStaffScroll;
    private string language, tempStringBtn;
    private int level, speed, speedTrain, health, percent, percentTrain, healthTrain, mental, mentalTrain, cost, index, oldIndex;
    private bool isHideComplete;
    int[] posiStaffIcon = new int[4] { 130, 150, 300, 330 };
    int numberMove = 0;
    float stepMove;
    void Start()
    {
        language = VariableSystem.language;
        if (language == null || language.Equals("")) language = "Vietnamese";
        if (language.Equals("Vietnamese"))//tiếng việt
        {
            labelsInPopup[0].text = "NHÂN VIÊN";
            labelsInPopup[1].text = "Tên ";
            labelsInPopup[3].text = "Cấp độ ";
            labelsInPopup[5].text = "Tập trung";
            labelsInPopup[7].text = "Thể lực";
            labelsInPopup[9].text = "Tinh thần";
            labelsInPopup[11].text = "Giá:";
        }
        else
        {
            labelsInPopup[0].text = "STAFF";
            labelsInPopup[1].text = "Name ";
            labelsInPopup[3].text = "Level ";
            labelsInPopup[5].text = "Focus";
            labelsInPopup[7].text = "Health";
            labelsInPopup[9].text = "Mental";
            labelsInPopup[11].text = "Cost:";
        }
    }
    void OnEnable()
    {
        listStaffScroll = iconStaffs[0].parent.GetComponent<UIPanel>();
        //HùngBV
        CommonObjectScript.isViewPoppup = true;
        CheckStaffOpen();
    }
    // Update is called once per frame
    void Update()
    {
        if (numberMove > 0)
        {
            numberMove--;
            listStaffScroll.transform.localPosition += Vector3.up * stepMove;
            listStaffScroll.clipOffset = new Vector2(0, -listStaffScroll.transform.localPosition.y);
        }
        if (Input.GetKeyDown(KeyCode.Escape) || CommonObjectScript.isEndGame || CommonObjectScript.isViewComplete)
        {
            if (!TownScenesController.isHelp)
                Exit_Click();
            if (DialogTask.complete) isHideComplete = true;
        }
        ControlViewHelp();
    }
    void OnApplicationPause(bool pauseStatus)
    {
        if (!pauseStatus)
        {
            if (!TownScenesController.isHelp)
                Exit_Click();
        }
    }
    void CheckStaffOpen()
    {
        shopcenter = staffTimer.shopcenter;
        if (staffTimer == null) print("NULL TIMER");
        else if (staffTimer.shopcenter == null) print("NULL SHOPCENTER GỐC");
        else if (shopcenter == null) print("NULL SHOPCENTER");
        for (int i = 0; i < 4; i++)
        {
            if (!shopcenter.isOpenStaffs[i]) disableItemInList(i);
            else if (index == 0)
                staff_Click(null, i + 1);
        }
    }
    private void MoveList()
    {
        numberMove = 20;
        if (index == 0)
        {
            stepMove = 0;
            print("Không chọn thằng nào ! Sao phải di chuyển");
        }
        else
            stepMove = (posiStaffIcon[index - 1] - listStaffScroll.transform.localPosition.y) / numberMove;
    }
    public void Exit_Click()
    {
        if (!TownScenesController.isHelp)
        {
            this.gameObject.GetComponent<Animator>().Play("InVisible");
            CommonObjectScript.audioControl.PlaySound("Click 1");
            CreatTownScenesController.isDenyContinue = false;
        }
    }

    public void staff_Click(GameObject indexStaff, int id = 1)
    {
        if (indexStaff != null && TownScenesController.isHelp) return;

        //if (indexStaff != null && !TownScenesController.isHelp)
        {
            oldIndex = index;
            if (indexStaff == null) index = id;
            else
            {
                CommonObjectScript.audioControl.PlaySound("Click 1");
                index = Convert.ToInt16(indexStaff.name.Substring(5));
            }
            MoveList();
            if (index == -1) index = oldIndex;
            if (oldIndex != 0 && shopcenter.isOpenStaffs[oldIndex - 1]) enableItemInList(oldIndex - 1);

            if (!shopcenter.isOpenStaffs[index - 1])
            {
                disableText();
            }
            else
            {
                selectedItemInList(index - 1);
                enableText();
                enableBtnHire();
                shopcenter.data = shopcenter.dataElement.getDataByValue("id", index.ToString());
                labelsInPopup[2].text = shopcenter.data.Attributes["name"].Value;
                if (shopcenter.listStaffsinShop[index - 1].isHired)
                {
                    int i;
                    for (i = 0; i < 3; i++)
                    {
                        if (shopcenter.staffDatas[i].typeStaff == index) break;
                    }
                    if (i == 3)
                    {
                        Debug.Log("không tìm thấy thằng " + index);
                        return;
                    }
                    level = shopcenter.staffDatas[i].levelStaff;
                    labelsInPopup[4].text = level.ToString();
                    speed = shopcenter.staffDatas[i].speed;
                    labelsInPopup[6].text = speed.ToString() + "/100";
                    KhungValues[0].fillAmount = speed / 100f;
                    health = shopcenter.staffDatas[i].health;
                    labelsInPopup[8].text = health.ToString() + "/100";
                    KhungValues[1].fillAmount = health / 100f;
                    mental = shopcenter.staffDatas[i].mental;
                    labelsInPopup[10].text = mental.ToString() + "/100";
                    KhungValues[2].fillAmount = mental / 100f;
                    if (level < 4)
                    {
                        cost = Convert.ToInt16(shopcenter.data.ChildNodes[level].Attributes["cost"].Value);
                        labelsInPopup[12].text = cost.ToString();
                        //labelsInPopup[13].text = language.Equals("Vietnamese") ? "Đào tạo" : "Training";
                        if (language == null)
                        {
                            print("BỆNH VÃI" + id);
                            language = VariableSystem.language;
                            if (language == null || language.Equals("")) language = "Vietnamese";
                        }
                        labelsInPopup[13].text = language.Equals("Vietnamese") ? "Đào tạo" : "Train";
                        getBonusTraining();
                        labelsInPopup[14].text = "+" + speedTrain;
                        labelsInPopup[15].text = "+" + healthTrain;
                        labelsInPopup[16].text = "+" + mentalTrain;
                        BtnInPopup.localPosition = new Vector3(210, -220, 0);
                    }
                    else
                    {
                        disableBtnHire();
                        //max 4 level, not allow training
                    }
                    if (shopcenter.countTimeTraining != 0)
                    {
                        disableBtnHire();
                        //training someone else, can't traning
                    }
                    if (level >= shopcenter.listStaffsinShop[index - 1].maxLevel)
                    {
                        //maxlevel, can't training
                        disableBtnHire();
                    }
                }
                else
                {
                    level = shopcenter.listStaffsinShop[index - 1].currentLevel;
                    labelsInPopup[4].text = level.ToString();
                    percent = shopcenter.getValue("percent", level);
                    speed = shopcenter.getValue("speed", level);
                    labelsInPopup[6].text = speed.ToString() + "/100";
                    KhungValues[0].fillAmount = speed / 100f;
                    health = shopcenter.getValue("health", level);
                    labelsInPopup[8].text = health.ToString() + "/100";
                    KhungValues[1].fillAmount = health / 100f;
                    mental = shopcenter.getValue("mental", level);
                    labelsInPopup[10].text = mental.ToString() + "/100";
                    KhungValues[2].fillAmount = mental / 100f;
                    cost = shopcenter.getValue("cost", level);
                    labelsInPopup[12].text = cost.ToString();
                    if (language == null)
                    {
                        language = VariableSystem.language;
                        if (language == null || language.Equals("")) language = "Vietnamese";
                    }
                    labelsInPopup[13].text = language.Equals("Vietnamese") ? "Thuê" : "Hire";
                    labelsInPopup[14].text = "";
                    labelsInPopup[15].text = "";
                    labelsInPopup[16].text = "";
                    BtnInPopup.localPosition = new Vector3(-10, -220, 0);
                    if (shopcenter.numberOfStaff == 3)
                        disableBtnHire();
                    //BtnInPopup.gameObject.enableBtnHire(false);
                }
                KhungValues[3].mainTexture = Resources.Load("Town/fullavatarStaff/" + shopcenter.data.Attributes["name"].Value.ToLower() + level) as Texture;
            }
        }
    }
    public void StaffClick()
    {
        staff_Click(null, index);
    }
    private void getBonusTraining()
    {
        speedTrain = shopcenter.getValue("speed", level + 1) - speed;
        healthTrain = shopcenter.getValue("health", level + 1) - health;
        mentalTrain = shopcenter.getValue("mental", level + 1) - mental;
        percentTrain = shopcenter.getValue("percent", level + 1);
    }

    private void selectedItemInList(int index)
    {
        iconStaffs[index].GetComponent<UIButtonScale>().enabled = false;
        iconStaffs[index].transform.localScale = Vector3.one * 1.1f;
        iconStaffs[index].GetComponent<UIButtonColor>().enabled = false;
        iconStaffs[index].GetComponent<UIButton>().defaultColor = new Color32(183, 163, 123, 255);
    }
    private void enableItemInList(int index)
    {
        iconStaffs[index].GetComponent<UIButtonScale>().enabled = true;
        iconStaffs[index].GetComponent<UIButtonColor>().enabled = true;
        iconStaffs[index].GetComponent<UIButton>().defaultColor = new Color32(255, 255, 255, 255);
    }
    private void disableItemInList(int index)
    {
        if (index < 0 || index > 3) Debug.LogError("index khong hop le : " + index);
        else
        {
            iconStaffs[index].GetComponent<UITexture>().color = new Color32(70, 70, 70, 255);
            iconStaffs[index].GetComponent<UIButton>().hover = new Color32(70, 70, 70, 255);
            iconStaffs[index].GetComponent<UIButton>().pressed = new Color32(70, 70, 70, 255);
            iconStaffs[index].GetComponent<UIButton>().defaultColor = new Color32(70, 70, 70, 255);
            iconStaffs[index].GetComponent<UIButtonScale>().enabled = false;
            iconStaffs[index].GetComponentInChildren<UILabel>().color = new Color32(70, 70, 70, 255);
            iconStaffs[index].transform.Find("Lock").gameObject.SetActive(true);
        }
    }
    private void disableBtnHire()
    {
        BtnInPopup.GetComponent<UITexture>().color = new Color32(150, 150, 150, 255);
        BtnInPopup.GetComponent<UIButton>().hover = new Color32(150, 150, 150, 255);
        BtnInPopup.GetComponent<UIButton>().pressed = new Color32(150, 150, 150, 255);
        BtnInPopup.GetComponent<UIButton>().defaultColor = new Color32(150, 150, 150, 255);
        labelsInPopup[13].color = new Color32(150, 150, 150, 255);
    }
    private void enableBtnHire()
    {
        BtnInPopup.GetComponent<UITexture>().color = new Color32(255, 255, 255, 255);
        BtnInPopup.GetComponent<UIButton>().hover = new Color32(255, 255, 255, 255);
        BtnInPopup.GetComponent<UIButton>().pressed = new Color32(255, 255, 255, 255);
        BtnInPopup.GetComponent<UIButton>().defaultColor = new Color32(255, 255, 255, 255);
        labelsInPopup[13].color = new Color32(255, 255, 255, 255);
    }
    private void disableText()
    {
        labelsInPopup[1].transform.parent.gameObject.SetActive(false);
        BtnInPopup.localPosition = new Vector3(-1000, -220, 0);
        KhungValues[3].transform.localScale = Vector3.zero;
        labelsInPopup[17].text = (language.Equals("Vietnamese") ? "Mở khóa ở màn " : " Unlock in level ") + (index == 3 ? "4!" : "29!");
    }
    private void enableText()
    {
        labelsInPopup[1].transform.parent.gameObject.SetActive(true);
        KhungValues[3].transform.localScale = Vector3.one;
        labelsInPopup[17].text = "";
    }

    public void HireOrTrain_Click() // hire or train Button click
    {
        #region for introduce Staff in level 2
        if (TownScenesController.isHelp)
        {
            if (VariableSystem.mission == 2)
            {
                print(CreatAndControlPanelHelp.countClickHelpPanel);
                if (CreatAndControlPanelHelp.countClickHelpPanel == 5)
                {
                    // TownScenesController.isContruduce = true;
                    TownScenesController.isHelp = false;
                    //CreatTownScenesController.isDenyContinue = false;
                    GameObject.Find("CommonObject").GetComponent<CommonObjectScript>().Shop_Click();
                }
            }
            if (VariableSystem.mission == 26)
            {
                if (CreatAndControlPanelHelp.countClickHelpPanel == 4)
                {
                    //CreatTownScenesController.isDenyContinue = false;
                    DestroyObjecHelp("CircleHelp");
                    DestroyObjecHelp("HandHelp");
                    CreatAndControlPanelHelp.countClickHelpPanel = 5;
                }
            }
        }
        #endregion
        CommonObjectScript.audioControl.PlaySound("Click 1");
        tempStringBtn = labelsInPopup[13].text;
        if (tempStringBtn.Equals("Hire") || tempStringBtn.Equals("Thuê")) //Hire
        {
            #region thuê
            if (shopcenter.numberOfStaff < 3)
            {
                if (cost <= CommonObjectScript.dollar)
                {
                    #region đủ tiền
                    shopcenter.common.AddDollar(-cost);
                    CreateAnimationAddValue("coin", "-" + cost.ToString(), true);
                    shopcenter.AddStaff(level, speed, health, mental, percent, index);
                    labelsInPopup[13].text = language.Equals("Vietnamese") ? "Đào tạo" : "Train";
                    BtnInPopup.localPosition = new Vector3(210, -220, 0);
                    if (shopcenter.countTimeTraining != 0) disableBtnHire();
                    if (level >= shopcenter.listStaffsinShop[index - 1].maxLevel)
                    {
                        disableBtnHire();
                    }
                    else
                    {
                        getBonusTraining();
                        labelsInPopup[14].text = "+" + speedTrain;
                        labelsInPopup[15].text = "+" + healthTrain;
                        labelsInPopup[16].text = "+" + mentalTrain;
                    }
                    #endregion
                }
                else
                {
                    GetComponent<Animator>().Play("InVisible");
                    shopcenter.common.ChangeDolar(cost - CommonObjectScript.dollar);
                }
            }
            else
            {
                //maximum staff
                if (BtnInPopup.Find("textShow") == null)
                {
                    if (!language.Equals("Vietnamese"))
                        CreateAnimationAddValue("textShow", "Unable to hire more staff!", false);
                    else
                        CreateAnimationAddValue("textShow", "Không thể thuê thêm nhân viên!", false);
                }
            }
            #endregion
        }
        else //training
        {
            #region train
            if (level >= shopcenter.listStaffsinShop[index - 1].maxLevel)
            {
                if (BtnInPopup.Find("textShow") == null)
                {
                    if (!language.Equals("Vietnamese"))
                        CreateAnimationAddValue("textShow", "No training course!", false);
                    else
                        CreateAnimationAddValue("textShow", "Chưa có khóa đào tạo!", false);
                }
            }
            else if (shopcenter.countTimeTraining == 0)
            {
                if (cost <= CommonObjectScript.dollar)
                {
                    shopcenter.common.AddDollar(-cost);
                    CreateAnimationAddValue("coin", "-" + cost.ToString(), true);
                    shopcenter.Training(index, speed + speedTrain, health + healthTrain, mental + mentalTrain, percentTrain);
                    disableBtnHire();
                    staffTimer.ActiveTimer();
                }
                else
                {
                    GetComponent<Animator>().Play("InVisible");
                    shopcenter.common.ChangeDolar(cost - CommonObjectScript.dollar);
                }
            }
            else
            {
                if (BtnInPopup.Find("textShow") == null)
                {
                    if (!language.Equals("Vietnamese"))
                        CreateAnimationAddValue("textShow", "Unable to train more staff!", false);
                    else
                        CreateAnimationAddValue("textShow", "Không thể đào tạo thêm nhân viên!", false);
                }
            }
            #endregion
        }
    }
    public void CreateAnimationAddValue(string name, string number, bool isShowImg = true)// create animation when add money or item
    {
        valueAdd = (GameObject)Instantiate(shopcenter.valuePrefabs);
        valueAdd.name = name;
        valueAdd.transform.parent = BtnInPopup.parent;
        valueAdd.transform.localPosition = new Vector3(100, -200, 0);
        valueAdd.transform.localScale = Vector3.one;
        valueAdd.GetComponent<AddValueScript>().setValue(number, isShowImg);
        if (isShowImg) valueAdd.GetComponent<AddValueScript>().setSub();
    }
    public void Destroy()
    {
        gameObject.SetActive(false);
        CommonObjectScript.isViewPoppup = false;
        CreatTownScenesController.isDenyContinue = false;
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
            if (VariableSystem.mission == 2)
            {
                if (CreatAndControlPanelHelp.countClickHelpPanel == 4)
                {
                    CreatObjectHelp("CircleHelp", new Vector3(40f, 40f, 40f), new Vector3(-15, -220, 0));
                    CreatObjectHelp("HandHelp", new Vector3(100f, 100f, 100f), new Vector3(10, -215, 0));
                    CreatAndControlPanelHelp.countClickHelpPanel = 5;
                }
            }
            else if (VariableSystem.mission == 26)
            {
                if (CreatAndControlPanelHelp.countClickHelpPanel == 3)
                {
                    CreatObjectHelp("CircleHelp", new Vector3(40f, 40f, 40f), new Vector3(210, -220, 0));
                    CreatObjectHelp("HandHelp", new Vector3(100f, 100f, 100f), new Vector3(200, -215, 0));
                    CreatAndControlPanelHelp.countClickHelpPanel = 4;
                }
            }
        }
    }
}
