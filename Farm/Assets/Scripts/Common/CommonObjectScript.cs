using Assets.Scripts.Common;
using System;
using UnityEngine;

public class CommonObjectScript : MonoBehaviour
{
    public static int dollar;//money in game
    public static float maxTimeOfMission; //max time of 1 mission
    public static int idDay;//start is monday

    public static int indexGuide;
    public static bool isGuide, isChangedDollar;
    public bool isOpenStorage, isOpennew;
    public static int rewardCustomerRate;

    int currentStar, targetStar;
    public string weatherDay;

    private float storageActiveTime = 0, starActiveTime = 0, coinActiveTime = 0;
    string[,] nameOfDays = new string[,] { { "Thứ 2", "Mon" }, { "Thứ 3", "Tue" }, { "Thứ 4", "Wed" }, { "Thứ 5", "Thu" }, { "Thứ 6", "Fri" }, { "Thứ 7", "Sat" }, { "CN", "Sun" } };
    public float countTimeOneDay = 0;

    public UnityEngine.Object[] spriteProducts, spriteMaterials;

    public static int[] arrayMaterials = new int[9];
    public static int[] arrayProducts = new int[16];
    UITexture btnStorage, starBar, starIcon, coinIcon, clockBar;
    UISprite iconWeather;
    UILabel[] labelsInCommon;

    public static bool isEndGame;
    public static bool isLeavedFarm;
    public GameObject panelPause;
    public GameObject panelChangeDolar;
    public DialogTask dialogTask;
    public string itemSelected = "";
    public static bool isViewPoppup;
    public static bool isViewComplete;
    GameObject newItemPrefabs;
    public enum Button
    {
        Farm,
        Factory,
        Shop,
        City,
        Store,
        Result
    }
    public GameObject[] arrayButton;

    //Hungbv
    public GameObject maketResearchResult;
    public int typeReasearch;
    UIButton[] buttonsCommon;
    public static AudioControl audioControl;
    bool isPlayHetGio;
    void Awake()
    {
        // see if we've got game music still playing
        GameObject[] commonObject = GameObject.FindGameObjectsWithTag("CommonObject");
        if (commonObject.Length > 1)
        {
            // kill game music
            for (int i = 1; i < commonObject.Length; i++)
            {
                Destroy(commonObject[i]);
            }
        }
        else
        {
            //Screen.SetResolution(854, 480, true);
            //Screen.sleepTimeout = SleepTimeout.NeverSleep;
            buttonsCommon = GetComponentsInChildren<UIButton>();
            isEndGame = false;
            indexGuide = 1;

            #region đặt biến isGuide
            if (VariableSystem.mission > VariableSystem.guidedLevels.Count)
            {
                int temp = VariableSystem.mission - VariableSystem.guidedLevels.Count;
                for (int i = 0; i < temp; i++) VariableSystem.guidedLevels.Add(false);
                isGuide = true;
            }
            else if (VariableSystem.guidedLevels[VariableSystem.mission - 1]) isGuide = false;
            else isGuide = true;
            #endregion

            isViewPoppup = false;
            if (MissionData.targetCommon.startScene == 1)
                isLeavedFarm = false;
            else isLeavedFarm = true;
            weatherDay = "may";
            if (VariableSystem.mission == 9) weatherDay = "nang";
            #region product
            //create new system items
            for (int i = 0; i < 16; i++)
            {
                if (i < 9)
                {
                    arrayMaterials[i] = 0;
                    StorageController.memoryMaterial[i] = -1;
                }
                arrayProducts[i] = 0;
                StorageController.memoryProduct[i] = -1;
            }
            //print("Reset data products");
            #endregion
            rewardCustomerRate = 0;
            audioControl = GameObject.Find("AudioControl").GetComponent<AudioControl>();
            #region auto navigate
            if (MissionData.targetCommon.startScene == 1) { }
            else if (MissionData.targetCommon.startScene == 2)
            {
                isLeavedFarm = true;
                PanelHouseController.nameScreenPre = "Factory";
            }
            else if (MissionData.targetCommon.startScene == 3)
            {
                isLeavedFarm = true;
                PanelHouseController.nameScreenPre = "Store";
            }
            else
            {
                isLeavedFarm = true;
                PanelHouseController.nameScreenPre = "Town";
            }
            #endregion
        }
        // make sure we survive going to different scenes
        DontDestroyOnLoad(transform.gameObject);
    }

    void Start()
    {
        #region lấy các liên kết
        dialogTask = GameObject.Find("DialogTask").GetComponent<DialogTask>();
        newItemPrefabs = (GameObject)Resources.Load("Common/PanelNewItem");
        btnStorage = transform.Find("Btn_Storage").GetComponent<UITexture>();
        starIcon = transform.Find("FrameObject").Find("Fr_star").Find("icon_star").GetComponent<UITexture>();
        coinIcon = transform.Find("FrameObject").Find("Fr_dollar").Find("icon_dollar").GetComponent<UITexture>();
        labelsInCommon = transform.Find("FrameObject").GetComponentsInChildren<UILabel>();
        clockBar = transform.Find("FrameObject").Find("Fr_days").Find("thanh_clock").GetComponent<UITexture>();
        starBar = transform.Find("FrameObject").Find("Fr_star").Find("thanh_star").GetComponent<UITexture>();
        iconWeather = transform.Find("FrameObject").Find("Fr_weather").Find("icon_weather").GetComponent<UISprite>();
        print(weatherDay);
        iconWeather.spriteName = weatherDay;
        if (weatherDay.Equals("nang") || weatherDay.Equals("mua") || weatherDay.Equals("tuyet")) IconWeatherController.setScale(true);
        else IconWeatherController.setScale(false);
        spriteProducts = Resources.LoadAll("Factory/Button/Images/Product");
        spriteMaterials = Resources.LoadAll("Factory/Button/Images/Material");
        #endregion

        #region timeSystem,star
        idDay = 1;
        if (isGuide && VariableSystem.mission == 7)
        {
            idDay = 7;
            //LotteryController.countSpin = 0;
        }
        maxTimeOfMission = MissionData.targetCommon.maxTime;
        if (MissionPowerUp.MoreTime)
        {
            if (maxTimeOfMission % 2 == 0)
                maxTimeOfMission *= 1.5f;
            else maxTimeOfMission = maxTimeOfMission * 1.5f + 0.5f;
        }
        targetStar = MissionData.targetCommon.targetCustomerRate;
        labelsInCommon[0].text = getNameDay(idDay);

        if (VariableSystem.language.Equals("Vietnamese"))
            labelsInCommon[1].text = "Còn lại " + maxTimeOfMission.ToString() + " ngày!";
        else
            labelsInCommon[1].text = (maxTimeOfMission == 1 ? "1 Day " : maxTimeOfMission.ToString() + " Days ") + "to go!";
        currentStar = 0;
        starBar.fillAmount = 0;
        #endregion

        #region moneySystem,diamond,new items
        dollar = MissionData.targetCommon.startMoney;
        if (MissionData.itemsInNew.Count > 0) CallNewItem();
        labelsInCommon[2].text = DString.ConvertToMoneyString(dollar.ToString());
        labelsInCommon[3].text = DString.ConvertToMoneyString(VariableSystem.diamond.ToString());
        #endregion
        CheckItemPowerUp();
        audioControl.PlayMusicInstance("Nhac nen " + (UnityEngine.Random.Range(0, 1250) % 2 + 1).ToString(), true, true, 0.23f);
        if (targetStar == 0) targetStar = getMaxCustomer((int)maxTimeOfMission) * 4;
        isPlayHetGio = false;

        //for (int i = 5; i < 36; i++)  getMaxCustomer(i);
    }


    void Update()
    {
        if (isChangedDollar)
        {
            labelsInCommon[2].text = DString.ConvertToMoneyString(dollar.ToString());
            isChangedDollar = false;
        }
        if (VariableSystem.isNeedUpdateValueDiamond)
        {
            labelsInCommon[3].text = DString.ConvertToMoneyString(VariableSystem.diamond.ToString());
            VariableSystem.isNeedUpdateValueDiamond = false;
        }

        if (isLeavedFarm && !Application.loadedLevelName.Equals("Farm"))//restore position of commonObject
        {
            transform.localPosition = Vector3.zero;
            isLeavedFarm = false;
            if (FarmCenterScript.isNeedWarning)
            {
                WarningVisible(CommonObjectScript.Button.Farm);
            }
        }

        //auto time
        if (!(isOpennew || isGuide || maxTimeOfMission <= 0))
        {
            countTimeOneDay += Time.deltaTime;
            clockBar.fillAmount = 1 - countTimeOneDay / 24;
            if (countTimeOneDay >= 24)
            {
                Nextday();
                weatherDay = getNextDay();
                iconWeather.spriteName = weatherDay;
                countTimeOneDay = 0;
                if (weatherDay.Equals("nang") || weatherDay.Equals("mua") || weatherDay.Equals("tuyet")) IconWeatherController.setScale(true);
                else IconWeatherController.setScale(false);
            }
            if (maxTimeOfMission == 1 && countTimeOneDay >= 14 && !isPlayHetGio)
            {
                audioControl.PlaySound("Het gio");
                isPlayHetGio = true;
            }
        }

        #region auto effect storage, coin, star
        if (storageActiveTime > 0)
        {
            storageActiveTime--;
            if (btnStorage.width > 118)
            {
                btnStorage.width -= 2;
                btnStorage.height -= 2;
            }
        }
        if (starActiveTime > 0)
        {
            starActiveTime--;
            if (starIcon.width > 62)
            {
                starIcon.width -= 1;
                starIcon.height -= 1;
            }
        }
        if (coinActiveTime > 0)
        {
            coinActiveTime--;
            if (coinIcon.width > 62)
            {
                coinIcon.width -= 1;
                coinIcon.height -= 1;
            }
        }
        #endregion

        // Back Button
        BackButton();
    }
    void OnApplicationPause(bool pauseStatus)
    {
        if (!pauseStatus)
        {
            //Time.timeScale = 0;
            if (isGuide) return;
            if (isOpenStorage)
            {
                transform.Find("PanelStorage").GetComponent<StorageController>().Close_Click();
            }
            else if (isOpennew)
            {
                transform.Find("NewItemsPopup").GetComponent<PanelNewItemScript>().OK_Click();
            }
            if (!LotteryController.isSpinning && !isEndGame && !Application.loadedLevelName.Equals("VilageResearch"))
            {
                Pause_Click();
            }
        }
    }

    public void Nextday()//next day 
    {
        maxTimeOfMission--;
        if (maxTimeOfMission == 0)
        {
            if (VariableSystem.language.Equals("Vietnamese")) labelsInCommon[1].text = "Hết giờ";
            else labelsInCommon[1].text = "Time out";
            setActiveButton(false, false, false, false, false, false, false, false);
            DialogResult.ShowResult();
        }
        else
        {
            if (VariableSystem.language.Equals("Vietnamese"))
                labelsInCommon[1].text = "Còn lại " + maxTimeOfMission.ToString() + " ngày!";
            else
                labelsInCommon[1].text = (maxTimeOfMission == 1 ? "1 Day " : maxTimeOfMission.ToString() + " Days ") + "to go!";
            if (maxTimeOfMission > 0 && maxTimeOfMission < 7) labelsInCommon[1].color = new Color32(255, 0, 0, 255);
        }
        idDay++;
        //Hungbv - Bắt đầu vào ngày chủ nhật - Chỉ chạy 1 lần
        if (idDay == 7)
        {
            LotteryController.countSpin = 0;
            // LotteryController.SaveCountSpin();
        }
        if (idDay == 8)
        {
            idDay = 1;
            labelsInCommon[0].color = new Color32(255, 255, 0, 255);
        }
        else if (idDay == 7) labelsInCommon[0].color = new Color32(255, 0, 0, 255);
        labelsInCommon[0].text = getNameDay(idDay);
        DialogAchievement.AddDataAchievement(21, 1); // time of người chơi đã qua
    }
    private string getNameDay(int id)//return name of one day
    {
        if (VariableSystem.language.Equals("Vietnamese"))
        {
            return nameOfDays[id - 1, 0];
        }
        else return nameOfDays[id - 1, 1];
    }
    private string getNextDay()
    {
        if (weatherDay.StartsWith("sap ")) return weatherDay.Substring(4);
        int tempWeather = UnityEngine.Random.Range(0, 1250) % 4;
        if (tempWeather == 0) return "may";
        if (tempWeather == 1) return "sap nang";
        if (tempWeather == 2) return "sap mua";
        return "sap tuyet";
    }
    public void Farm_Click()
    {
        if (!Application.loadedLevelName.Equals("Farm"))
        {
            audioControl.PlaySound("Click 1");
            LoadingScene.ShowLoadingScene("Farm", true);
            PanelHouseController.nameScreenPre = "Farm";
        }
    }
    public void Shop_Click()
    {
        if (!Application.loadedLevelName.Equals("Store"))
        {
            isLeavedFarm = true;
            audioControl.PlaySound("Click 1");
            if (VariableSystem.mission == 1) indexGuide++;
            LoadingScene.ShowLoadingScene("Store", true);
            PanelHouseController.nameScreenPre = "Store";
        }
    }
    public void Factory_Click()
    {
        if (!Application.loadedLevelName.Equals("Factory"))
        {
            audioControl.PlaySound("Click 1");
            isLeavedFarm = true;
            LoadingScene.ShowLoadingScene("Factory", true);
            PanelHouseController.nameScreenPre = "Factory";
        }
    }
    public void City_Click()
    {
        if (!Application.loadedLevelName.Equals("Town"))
        {
            audioControl.PlaySound("Click 1");
            isLeavedFarm = true;
            LoadingScene.ShowLoadingScene("Town", true);
            PanelHouseController.nameScreenPre = "Town";
        }
    }
    public void Task_Click()
    {
        audioControl.PlaySound("Click 1");
        dialogTask.Show();
        if (Application.loadedLevelName.Equals("Farm"))
        {
            GameObject.Find("UI Root").transform.Find("PanelPlant").GetComponent<PlantControlScript>().BG_Click();
            if (isGuide) GameObject.Find("GuideFarmController").GetComponent<GuideFarmScript>().NextGuideText();
        }
    }
    public void Storage_Click()
    {
        audioControl.PlaySound("Click 1");
        transform.Find("PanelStorage").gameObject.SetActive(true);
        isOpenStorage = true;
        if (isGuide) GameObject.Find("GuideFarmController").GetComponent<GuideFarmScript>().NextGuideText();
    }
    public void Result_Click()
    {
        audioControl.PlaySound("Click 1");
        if (Application.loadedLevelName.Equals("Farm"))
        {
            GameObject.Find("UI Root").transform.Find("PanelPlant").GetComponent<PlantControlScript>().BG_Click();
        }
        if (!isGuide)
        {
            if (typeReasearch == 0)
            {
                if (maketResearchResult != null)
                {
                    maketResearchResult.transform.parent = GameObject.Find("UI Root").transform;
                    maketResearchResult.transform.localPosition = new Vector3(0, 0, 0);
                    maketResearchResult.transform.localScale = new Vector3(1, 1, 1);
                    maketResearchResult.gameObject.SetActive(true);
                    maketResearchResult.GetComponent<Animator>().Play("Visible");
                }
            }
            else
            {
                //Application.LoadLevel("VilageResearch");
                LoadingScene.ShowLoadingScene("VilageResearch", true);
            }
        }
        else
        {

            if (TownScenesController.isHelp)
            {
                if (VariableSystem.mission == 11)
                {
                    if (CreatAndControlPanelHelp.countClickHelpPanel == 11)
                    {
                        maketResearchResult.transform.parent = GameObject.Find("UI Root").transform;
                        maketResearchResult.transform.localPosition = new Vector3(0, 0, 0);
                        maketResearchResult.transform.localScale = new Vector3(1, 1, 1);
                        maketResearchResult.gameObject.SetActive(true);
                        maketResearchResult.GetComponent<Animator>().Play("Visible");

                        CreatAndControlPanelHelp.countClickHelpPanel = 12;
                    }
                }
            }
        }

    }
    public void MoreDiamond_Click()
    {
        print("Diamond click !");
        audioControl.PlaySound("Click 1");
        //AddDiamond(10);
        DialogInapp.ShowInapp();
    }
    public void Pause_Click()
    {
        audioControl.PlaySound("Click 1");
        panelPause.SetActive(true);
        if (Application.loadedLevelName.Equals("Farm"))
        {
            GameObject.Find("UI Root").transform.Find("PanelPlant").GetComponent<PlantControlScript>().BG_Click();
        }
    }

    public void Storage_Active()
    {
        storageActiveTime = 25;
        btnStorage.width = 168;
        btnStorage.height = 170;
    }
    public void Star_Active()
    {
        starActiveTime = 20;
        starIcon.width = 82;
        starIcon.height = 82;
    }
    public void Coin_Active()
    {
        coinActiveTime = 20;
        coinIcon.width = 82;
        coinIcon.height = 82;
    }

    public void AddStar(int star)
    {
        if (currentStar < targetStar)
        {
            currentStar += star;
            if (currentStar >= targetStar) rewardCustomerRate = 2;
            else if (currentStar * 1f / targetStar >= 0.8f) rewardCustomerRate = 1;
            else rewardCustomerRate = 0;
        }
        else
        {
            currentStar += star;
        }
        if (currentStar >= targetStar) starBar.fillAmount = 1;
        else starBar.fillAmount = (currentStar * 1f) / targetStar;
        MissionData.targetCommon.currentLevel += star;
        DialogAchievement.AddDataAchievement(20, star);
    }
    public void AddDollar(int dola)
    {
        dollar += dola;
        MissionData.targetCommon.currentNumber = dollar;
        labelsInCommon[2].text = DString.ConvertToMoneyString(dollar.ToString());
    }
    public static void AddDollarStatic(int dola)
    {
        dollar += dola;
        MissionData.targetCommon.currentNumber = dollar;
        isChangedDollar = true;
    }
    public void AddDiamond(int diamon)
    {
        VariableSystem.AddDiamond(diamon);
    }

    public void WarningVisible(Button buttonName)
    {
        audioControl.PlaySound("Canh bao button");
        arrayButton[(int)buttonName].GetComponent<Animator>().Play("Active");
    }
    public void WarningInvisible(Button buttonName)
    {
        arrayButton[(int)buttonName].GetComponent<Animator>().Play("Nomal");
    }
    public void ResultButtonVisible()
    {
        arrayButton[5].SetActive(true);
    }
    public void ChangeDolar(int coinNedd = 200)
    {
        if (ButtonViewController.animator != null)
            ButtonViewController.animator.SetTrigger("Collape");
        if (ProductQueueController.animator != null)
            ProductQueueController.animator.SetTrigger("Collape");

        //CuongVM
        isOpennew = true;

        //CameraController.isViewPopup = true;
        panelChangeDolar.SetActive(true);
        panelChangeDolar.GetComponent<PanelChangeDolarController>().coinNeed = coinNedd;
    }
    private void CallNewItem()
    {
        isOpennew = true;
        GameObject temp = (GameObject)Instantiate(newItemPrefabs);
        temp.name = "NewItemsPopup";
        temp.GetComponent<PanelNewItemScript>().setDataShow(MissionData.itemsInNew, this);
        temp.transform.parent = this.transform;
    }
    public int getMaxCustomer(int maxDay)
    {
        int seconds = maxDay * 24;
        int apart = seconds / 5;
        int number = 0;
        number += apart / 10;
        apart = seconds / 5 + apart % 18;
        number += apart / 8;
        apart = seconds / 5 * 2 + apart % 12;
        number += apart / 6;
        apart = seconds / 5 + apart % 8;
        number += apart / 4;
        //print(maxDay + " : " + number + " !");
        //return number;
        //print("Max customer : " + MissionData.targetCommon.maxCustomer);
        return MissionData.targetCommon.maxCustomer;
    }
    void CheckItemPowerUp()
    {
        if (MissionPowerUp.MoreMoney) { AddDollar(dollar / 2); }
        if (MissionPowerUp.CustomerIncrease) { ShopCenterScript.bonusCustomer = getMaxCustomer((int)maxTimeOfMission) / 2; }
        if (MissionPowerUp.FillRateMeter) { AddStar((int)(targetStar * 0.4f)); }
        if (isGuide)
            switch (VariableSystem.mission)
            {
                case 8:
                case 1: AddDiamond(2); break;
                case 3:
                case 15: AddDiamond(1); break;
                case 11: AddDiamond(3); break;
            }
    }
    public static void CompleteGuide()
    {
        isGuide = false;
        indexGuide = -1;
        VariableSystem.guidedLevels[VariableSystem.mission - 1] = true;
        VariableSystem.SaveGuide();
    }
    private void BackButton()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isGuide) return;

            if (isOpenStorage)
            {
                transform.Find("PanelStorage").GetComponent<StorageController>().Close_Click();
            }
            else if (isOpennew)
            {
                transform.Find("NewItemsPopup").GetComponent<PanelNewItemScript>().OK_Click();
            }
            else if (!isViewPoppup)
            {

                // print(isViewPoppup + "cccccccccccccccc");

                if (!Application.loadedLevelName.Equals("VilageResearch"))
                {

                    //if (panelPause.gameObject.activeInHierarchy)
                    //    panelPause.GetComponent<PanelPauseController>().Play_Click();
                    //else
                    if (!LotteryController.isSpinning && !isEndGame)
                        panelPause.SetActive(true);

                }
            }
        }

        if (isEndGame)
        {
            if (isOpenStorage)
            {
                transform.Find("PanelStorage").GetComponent<StorageController>().Close_Click();
            }
        }
    }

    public void setActiveButton(bool farm, bool factory, bool shop, bool city, bool storage, bool task, bool pause, bool diamond)
    {
        buttonsCommon[0].enabled = city;
        buttonsCommon[1].enabled = shop;
        buttonsCommon[2].enabled = factory;
        buttonsCommon[3].enabled = farm;
        buttonsCommon[4].enabled = pause;
        buttonsCommon[5].enabled = diamond;
        buttonsCommon[6].enabled = task;
        buttonsCommon[7].enabled = storage;
    }
}
