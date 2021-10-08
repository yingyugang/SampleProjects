using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Common;
using Assets.Scripts.Farm;

public class TechnogyController : MonoBehaviour
{

    // Use this for initialization
    public UIGrid gridCommunication;
    public UILabel[] labelInfor;
    public GameObject[] icon;
    private List<int> iDChange;
    private List<Texture> listTextture;
    public int iDSelected;
    public int levelSelected;
    public int maxSelected;
    public UILabel maxLevel;
    public UIPanel panelInfor;
    public UITexture iconObject;
    public UIScrollView scrollViewForm;
    public UITexture iconProduct;
    private int cost;
    private float timeDelay;
    private float timeCount;

    private List<int> listFarm;
    private List<int> listMachine;
    private List<int> levelOfObject;
    private List<int> levelMax;
    private bool isAdd;

    public GameObject ContainerCoinEfectPrefabs;
    private GameObject ContainerCoinEfectClone;
    public GameObject timerPrefabs;
    private GameObject timerClone;
    private bool isUpgrade;

    private bool isChangePos;
    private int tempChange;
    private Vector2 perChangePos;
    private Vector2 perChangeOff;

    AudioControl audioControl;
    void OnEnable()
    {
        CommonObjectScript.isViewPoppup = true;
        if (TownScenesController.isHelp)
        {
            if (VariableSystem.mission == 15)
                scrollViewForm.GetComponent<UIScrollView>().enabled = false;
        }
        else
        {
            scrollViewForm.GetComponent<UIScrollView>().enabled = true;
        }

        if (listTextture == null)
        {
            listTextture = new List<Texture>();
            foreach (Texture tempTexture in Resources.LoadAll<Texture>("Common/Images"))
            {
                listTextture.Add(tempTexture);
            }
            foreach (Texture tempTexture in Resources.LoadAll<Texture>("Factory/Button/Images/Machine"))
            {
                listTextture.Add(tempTexture);
            }
        }
        iDChange = new List<int>();
        iDChange.Clear();

        ReadID();
        ChangeID(listFarm, listMachine);
        CreateIcon();

        //khởi tạo bắt đầu vào
        this.iDSelected = iDChange[0];
        SetDataButtonItem(iDChange[0], levelOfObject[0], levelMax[0]);
    }
    void Start()
    {
        audioControl = GameObject.Find("AudioControl").GetComponent<AudioControl>();
        labelInfor[9].text = TownScenesController.languageTowns["Upgrade"];
        labelInfor[10].text = TownScenesController.languageTowns["TECHNOGY"];
        ControlViewHelp();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || CommonObjectScript.isEndGame || CommonObjectScript.isViewComplete)
        {
            if (!TownScenesController.isHelp)
                Close_Click();
        }
        if (isUpgrade)
        {
            if (timeCount < timeDelay)
            {
                timeCount += Time.deltaTime;
            }
            else
            {
                this.gameObject.GetComponent<Animator>().Play("Invisible");

            }
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
    void ReadID()
    {
        listFarm = new List<int>();
        listFarm.Clear();
        levelOfObject = new List<int>();
        levelOfObject.Clear();
        levelMax = new List<int>();
        levelMax.Clear();

        foreach (FieldFarm fieldFarms in MissionData.farmDataMission.fieldFarms)
        {
            listFarm.Add(fieldFarms.idField);
            levelOfObject.Add(fieldFarms.currentLevel);
            levelMax.Add(fieldFarms.maxLevel);
        }

        listMachine = new List<int>();
        listMachine.Clear();
        if (FactoryScenesController.ListMachineHaved != null)
        {
            for (int countListMachineHaved = 0; countListMachineHaved < FactoryScenesController.ListMachineHaved.Count; countListMachineHaved++)
            {
                isAdd = true;
                for (int countListMachine = 0; countListMachine < listMachine.Count; countListMachine++)
                {
                    if (FactoryScenesController.ListMachineHaved[countListMachineHaved] == listMachine[countListMachine])
                    {
                        isAdd = false;
                        break;
                    }
                }
                if (isAdd)
                {
                    listMachine.Add(FactoryScenesController.ListMachineHaved[countListMachineHaved]);
                }
            }
            listMachine.Sort();
        }

        for (int countListMachine = 0; countListMachine < listMachine.Count; countListMachine++)
        {
            for (int count = 0; count < MissionData.factoryDataMission.machinedatas.Count; count++)
            {
                if (listMachine[countListMachine] == (MissionData.factoryDataMission.machinedatas[count].iDMachine))
                {
                    levelOfObject.Add(MissionData.factoryDataMission.machinedatas[count].currentLevel);
                    levelMax.Add(MissionData.factoryDataMission.machinedatas[count].maxLevel);
                }
            }
        }
    }

    void CreateIcon()
    {

        for (int i = 0; i < iDChange.Count; i++)
        {
            icon[i].SetActive(true);
            int countTexture = (iDChange[i] - 1) * 4 + levelOfObject[i] - 1;
            icon[i].transform.Find("IconObject").GetComponent<UITexture>().mainTexture = listTextture[countTexture];
            icon[i].transform.Find("Label").GetComponent<UILabel>().text = TownScenesController.languageTowns[TownScenesController.ListTechnogyData[iDChange[i] - 1].name];
            icon[i].GetComponent<ItemTechnogyController>().iDButton = iDChange[i];
            icon[i].GetComponent<ItemTechnogyController>().levelObject = levelOfObject[i];
            icon[i].GetComponent<ItemTechnogyController>().maxLevel = levelMax[i];
        }

        for (int i = iDChange.Count; i < icon.Length; i++)
        {
            icon[i].SetActive(false);
        }

        // print("vao day nha ---------------");
    }

    void ChangeID(List<int> listFarmTemp, List<int> listMachineTemp)
    {
        foreach (int temp in listFarmTemp)
        {
            iDChange.Add(temp);
        }
        foreach (int temp in listMachineTemp)
        {
            iDChange.Add(temp + 3);
        }
    }

    public void BtnItem_Click()
    {
        audioControl.PlaySound("Click 1");
        if (!TownScenesController.isHelp)
        {
            SetDataButtonItem(this.iDSelected, this.levelSelected, this.maxSelected);
        }
        else
        {
            if (VariableSystem.mission == 15)
            {
                if (this.iDSelected == 1)
                {
                    SetDataButtonItem(this.iDSelected, this.levelSelected, this.maxSelected);
                    ChangeHelpPosition("CircleHelp", new Vector3(124, -270, 0));
                    ChangeHelpPosition("HandHelp", new Vector3(124, -270, 0));
                    CreatAndControlPanelHelp.countClickHelpPanel = 4;
                }
            }
        }
    }

    public void Upgrade_Click()
    {
        if (!TownScenesController.isHelp)
        {
            if (!isUpgrade)
            {
                if (CommonObjectScript.dollar >= cost)
                {
                    timeDelay = 0.8f;
                    timeCount = 0;
                    TownScenesController.townsBusy[5] = true;
                    isUpgrade = true;
                    CreateContainerCoinEfect();
                    CreateTimer();
                    AddCommonObject(-cost, 0);
                }
                else
                {
                    Close_Click();
                    GameObject.Find("CommonObject").GetComponent<CommonObjectScript>().ChangeDolar(cost - CommonObjectScript.dollar);
                }
                audioControl.PlaySound("Click 1");
            }
        }
        else
        {
            if (VariableSystem.mission == 15)
            {
                if (CreatAndControlPanelHelp.countClickHelpPanel == 4)
                {
                    if (!isUpgrade)
                    {
                        timeDelay = 0.8f;
                        timeCount = 0;
                        TownScenesController.townsBusy[5] = true;
                        isUpgrade = true;
                        CreateContainerCoinEfect();
                        CreateTimer();
                        AddCommonObject(-cost, 0);
                        DestroyObjecHelp("CircleHelp");
                        DestroyObjecHelp("HandHelp");
                        audioControl.PlaySound("Click 1");
                    }
                }
            }
        }
    }

    public void Close_Click()
    {
        if (!TownScenesController.isHelp)
            this.gameObject.GetComponent<Animator>().Play("Invisible");
        audioControl.PlaySound("Click 1");
    }

    public void Ivisible()
    {
        CommonObjectScript.isViewPoppup = false;
        this.gameObject.SetActive(false);
        isUpgrade = false;
        CreatTownScenesController.isDenyContinue = false;
        if (TownScenesController.isHelp && CreatAndControlPanelHelp.countClickHelpPanel == 4)
            CreatAndControlPanelHelp.countClickHelpPanel = 5;
    }

    void SetDataButtonItem(int iDButton, int level, int maxlevel)
    {
        if (level < maxlevel)
        {
            panelInfor.gameObject.SetActive(true);
            maxLevel.gameObject.SetActive(false);
        }
        else
        {
            panelInfor.gameObject.SetActive(false);
            maxLevel.gameObject.SetActive(true);
            maxLevel.text = TownScenesController.languageTowns["MaxLevel"];
        }

        if (iDButton <= 3)
        {
            foreach (FieldFarm fieldFarm in MissionData.farmDataMission.fieldFarms)
            {
                if (fieldFarm.idField == iDButton)
                {
                    cost = fieldFarm.currentNumber * ((TownScenesController.ListTechnogyData[iDButton - 1].cost + (level >= 3 ? ((level - 2) * TownScenesController.ListTechnogyData[iDButton - 1].deltaCost) : 0)));
                    break;
                }
            }

            labelInfor[2].text = TownScenesController.languageTowns["Productivity"];
            labelInfor[4].gameObject.SetActive(false);
        }
        else
        {
            for (int count = 0; count < MissionData.factoryDataMission.machinedatas.Count; count++)
            {
                if ((iDButton - 3) == (MissionData.factoryDataMission.machinedatas[count].iDMachine))
                {
                    // print("Loai may " + iDButton);
                    if (MissionData.factoryDataMission.machinedatas[count].currentLevel != 4)
                    {
                        int idPro = TownScenesController.ListTechnogyData[iDButton - 1].newProducts[MissionData.factoryDataMission.machinedatas[count].currentLevel - 1];
                        // print("Loai san pham " + idPro);
                        if (idPro >= 0)
                        {
                            //add ảnh
                            string linkImg = "Factory/Button/Images/Product/" + (idPro > 9 ? "" : "0") + idPro.ToString();
                            iconProduct.mainTexture = Resources.Load(linkImg) as Texture;
                            labelInfor[4].gameObject.SetActive(true);
                        }
                        else
                        {
                            //print("K co sp");
                            labelInfor[4].gameObject.SetActive(false);
                        }
                    }
                    cost = MissionData.factoryDataMission.machinedatas[count].currentNumber * ((TownScenesController.ListTechnogyData[iDButton - 1].cost + (level >= 3 ? ((level - 2) * TownScenesController.ListTechnogyData[iDButton - 1].deltaCost) : 0)));
                    break;
                }
            }
            labelInfor[2].text = TownScenesController.languageTowns["Queue"];
        }

        int countTexture = (iDButton - 1) * 4 + level - 1;
        iconObject.mainTexture = listTextture[countTexture];

        labelInfor[7].text = TownScenesController.languageTowns[TownScenesController.ListTechnogyData[iDButton - 1].name];
        labelInfor[8].text = TownScenesController.languageTowns["Level"] + level;

        labelInfor[0].text = TownScenesController.languageTowns["NextLevel"];
        labelInfor[1].text = TownScenesController.languageTowns["Level"] + (level + 1);
        labelInfor[3].text = "+ 2";
        labelInfor[5].text = TownScenesController.languageTowns["Cost"];
        labelInfor[6].text = cost.ToString();

    }

    void CreateContainerCoinEfect()
    {
        ContainerCoinEfectClone = (GameObject)Instantiate(ContainerCoinEfectPrefabs, transform.position, transform.rotation);
        ContainerCoinEfectClone.GetComponent<CoinEfectController>().timeDelay = 0f;
        ContainerCoinEfectClone.GetComponent<CoinEfectController>().costProduct = cost;
        ContainerCoinEfectClone.transform.parent = labelInfor[5].transform;
        ContainerCoinEfectClone.transform.localPosition = new Vector3(180f, -50f, 0);
        ContainerCoinEfectClone.transform.localScale = new Vector3(1f, 1f, 1f);
        ContainerCoinEfectClone.name = ContainerCoinEfectPrefabs.name;
    }
    void CreateTimer()
    {
        if (!TownScenesController.isHelp)
        {


            timerClone = (GameObject)Instantiate(timerPrefabs, transform.localPosition, transform.rotation);
            timerClone.GetComponent<TimerController>().time = DialogShop.BoughtItem[8] ? 0 : 48.0f;
            timerClone.GetComponent<TimerController>().minRation = this.iDSelected;
            timerClone.GetComponent<TimerController>().maxRation = this.iDSelected;
            print(timerClone.GetComponent<TimerController>().maxRation);
            timerClone.transform.position = new Vector3(0.99f, 0.41f, 0);
            timerClone.transform.localScale = new Vector3(-0.0028f, 0.0028f, 0.0028f);
            timerClone.name = "TimerTechnogy";
        }
        else
        {
            if (VariableSystem.mission == 15)
            {
                timerClone = (GameObject)Instantiate(timerPrefabs, transform.localPosition, transform.rotation);
                timerClone.GetComponent<TimerController>().time = DialogShop.BoughtItem[8] ? 0 : 48.0f;
                timerClone.GetComponent<TimerController>().minRation = 1;
                timerClone.GetComponent<TimerController>().maxRation = 1;
                print(timerClone.GetComponent<TimerController>().maxRation);
                timerClone.transform.position = new Vector3(0.99f, 0.41f, 0);
                timerClone.transform.localScale = new Vector3(-0.0028f, 0.0028f, 0.0028f);
                timerClone.name = "TimerTechnogy";
            }
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
            if (VariableSystem.mission == 15)
            {
                if (CreatAndControlPanelHelp.countClickHelpPanel == 3)
                {
                    CreatObjectHelp("CircleHelp", new Vector3(40f, 40f, 40f), new Vector3(-270, 135, 0));
                    CreatObjectHelp("HandHelp", new Vector3(100f, 100f, 100f), new Vector3(-260, 120, 0));
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

    void ChangeScroll(Vector2 perPos, Vector2 perOff, bool Add = false)
    {
        if (isChangePos)
        {
            if (tempChange < 20)
            {
                if (Add)
                {
                    scrollViewForm.transform.localPosition = new Vector2(scrollViewForm.transform.localPosition.x + perPos.x, scrollViewForm.transform.localPosition.y + perPos.y);
                    scrollViewForm.GetComponent<UIPanel>().clipOffset = new Vector2(scrollViewForm.GetComponent<UIPanel>().clipOffset.x + perOff.x, scrollViewForm.GetComponent<UIPanel>().clipOffset.y + perOff.y);
                }
                else
                {
                    scrollViewForm.transform.localPosition = new Vector2(scrollViewForm.transform.localPosition.x - perPos.x, scrollViewForm.transform.localPosition.y - perPos.y);
                    scrollViewForm.GetComponent<UIPanel>().clipOffset = new Vector2(scrollViewForm.GetComponent<UIPanel>().clipOffset.x - perOff.x, scrollViewForm.GetComponent<UIPanel>().clipOffset.y - perOff.y);
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

