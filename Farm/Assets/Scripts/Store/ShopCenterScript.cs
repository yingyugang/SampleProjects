using Assets.Scripts.Common;
using Assets.Scripts.Store;
using Assets.Scripts.Town;
using System;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class ShopCenterScript : MonoBehaviour
{
    public int numberOfStaff;//number of staff in shop
    public Customer[,] cusDatas = new Customer[3, 5];//customer in shop
    public Staff[] staffDatas = new Staff[3];//staff in shop
    public StaffData[] listStaffsinShop = new StaffData[4];//staff in city
    public bool[] isOpenStaffs = new bool[4];//staff in city
    public ProductData[] productDatas = new ProductData[16];
    public CommonObjectScript common;
    public float countTimeTraining = 0f;
    public int indexTraining = -1;

    float countTime = 5;//data service waiting of customer
    string tempState;//state to check sleep,sick,lazy
    List<int> availableRow = new List<int>();
    Customer cus;

    public bool isUsingItemWeather;
    public bool[] changeStaffs = new bool[3];
    public int[] priceCakes = new int[16] { 75, 80, 82, 83, 83, 70, 70, 82, 70, 72, 71, 76, 70, 75, 74, 77 };
    public WarningTextView textStaff, textProduct;
    public GameObject valuePrefabs;

    bool isCanSick;

    public ReadXML dataElement;
    public XmlNode data;

    private int speed, health, mental, percent, totalNumberRequest;
    float maxTimeOfMission, valpercent, countTimeBonusCus;
    List<int> tempDataRandomProduct;
    public static int bonusCustomer, maxCustomer;
    static bool isAddCustomer;
    public static bool isNeedWarning;
    void Awake()
    {
        GameObject[] shopObject = GameObject.FindGameObjectsWithTag("ShopObject");
        if (shopObject.Length > 1)
        {
            for (int i = 1; i < shopObject.Length; i++)
            {
                Destroy(shopObject[i]);
            }
        }
        DontDestroyOnLoad(transform.gameObject);
    }
    void OnEnable()
    {
        common = GameObject.Find("CommonObject").GetComponent<CommonObjectScript>();
        isNeedWarning = false;
        valuePrefabs = (GameObject)Resources.Load("Farm/AddValue");
        dataElement = new ReadXML("Town/XMLFile/ElementStaff");
        numberOfStaff = 0;
        for (int i = 0; i < 16; i++)
        {
            if (i < 4)
            {
                listStaffsinShop[i] = new StaffData(i + 1, 1, 4, -1, false);//max 4 staff in city
                isOpenStaffs[i] = false;
                if (i != 3)
                {
                    staffDatas[i] = new Staff(0);//max 3 staff in shop
                    changeStaffs[i] = false;
                }
            }
            if (i != 15) cusDatas[i / 5, i % 5] = new Customer(); //max 15 cus
            productDatas[i] = new ProductData(i, -1, true);//max 16 prodcut
            isUsingItemWeather = true;
        }

        foreach (StaffData staff in MissionData.townDataMission.staffsData)
        {
            listStaffsinShop[staff.idStaff - 1] = staff;
            isOpenStaffs[staff.idStaff - 1] = true;//confirm staff is open
            if (staff.isHired)
            {
                staffDatas[numberOfStaff] = new Staff(staff.startLevel);
                data = dataElement.getDataByValue("id", staff.idStaff.ToString());
                staffDatas[numberOfStaff].setData(staff.startLevel, getValue("speed", staff.startLevel), getValue("health", staff.startLevel), getValue("percent", staff.startLevel), getValue("mental", staff.startLevel), staff.idStaff);
                numberOfStaff++;
            }
        }
        isCanSick = MissionData.townDataMission.isCanSick;
        if (DialogShop.BoughtItem[12]) isCanSick = false;
        isUsingItemWeather = MissionData.shopDataMission.isUsingItem;

    }
    void Start()
    {
        maxTimeOfMission = 10;
        tempDataRandomProduct = new List<int>();
        maxCustomer = common.getMaxCustomer((int)maxTimeOfMission + 1);

        foreach (ProductData data in MissionData.shopDataMission.listProducts)
        {
            productDatas[data.idProduct - 7] = data;
            CommonObjectScript.arrayProducts[data.idProduct - 7] = data.startNumber;

            //print(CommonObjectScript.arrayProducts[data.idProduct - 7]);
        }
        maxTimeOfMission = MissionData.targetCommon.maxTime;
        if (VariableSystem.mission == 6 && CommonObjectScript.isGuide)
        {
            staffDatas[0].statement = "sick";
            changeStaffs[0] = true;
            staffDatas[1].statement = "sleep";
            changeStaffs[1] = true;
            staffDatas[2].statement = "lazy";
            changeStaffs[2] = true;
        }
    }
    public int getValue(string attribute, int level)
    {
        if (attribute.Equals("percent"))
        {
            return Convert.ToInt16(data.ChildNodes[level].Attributes[attribute].Value);
        }
        int val = 0;
        for (int i = 0; i < level; i++)
        {
            string[] temps = data.ChildNodes[i].Attributes[attribute].Value.Split('-');
            if (temps.Length == 1)
            {
                if (attribute.Equals("cost"))
                {
                    if (i == 0) val += Convert.ToInt16(temps[0]);
                    if (level > 1 && (i == level - 1)) val += (int)(Convert.ToInt16(temps[0]) * 1.2f);
                }
                else val += Convert.ToInt16(temps[0]);
            }
            else
            {
                val += UnityEngine.Random.Range(Convert.ToInt16(temps[0]), Convert.ToInt16(temps[1]));
            }
        }
        return val;
    }

    void Update()
    {
        if (!CommonObjectScript.isGuide)
        {
            #region create customer
            #region bonus customer
            if (isAddCustomer)
            {
                isAddCustomer = false;
                for (int i = 0; i < 15; i++)
                {
                    if (bonusCustomer > 0)
                    {
                        if (CreateCustomer())
                        {
                            bonusCustomer--;
                        }
                        else break;
                    }
                    else break;
                }
            }
            else if (bonusCustomer > 0)
            {
                countTimeBonusCus += Time.deltaTime;
                if (countTimeBonusCus >= CommonObjectScript.maxTimeOfMission / (bonusCustomer + 1))
                {
                    if (CreateCustomer())
                    {
                        bonusCustomer--;
                        countTimeBonusCus = 0;
                    }
                }
            }
            #endregion
            #region create normal customer
            countTime += Time.deltaTime;
            valpercent = (CommonObjectScript.maxTimeOfMission + common.countTimeOneDay / 24f) / maxTimeOfMission;
            if ((valpercent > 0.8f && countTime >= (4.8f * maxTimeOfMission / (0.15f * maxCustomer))) ||
                (valpercent > 0.6f && valpercent <= 0.8f && countTime >= (4.8f * maxTimeOfMission / (0.2f * maxCustomer))) ||
                (valpercent > 0.4f && valpercent <= 0.6f && countTime >= (4.8f * maxTimeOfMission / (0.3f * maxCustomer))) ||
                (valpercent > 0.05f && valpercent <= 0.4f && countTime >= (24 * 0.35F * maxTimeOfMission / (0.35f * maxCustomer))))
            {
                if (!CreateCustomer()) bonusCustomer++;
                countTime = 0;
            }
            #endregion
            #endregion
            #region training
            if (countTimeTraining > 0)
            {
                countTimeTraining -= Time.deltaTime;
                if (countTimeTraining <= 0)
                {
                    Upgrade();
                }
            }
            #endregion
            #region Auto service
            for (int i = 0; i < 3; i++)//foreach one row
            {
                if (staffDatas[i].typeStaff != 0)//if have staff
                {
                    #region complete fixing
                    if (staffDatas[i].statement.StartsWith("fix_"))
                    {
                        staffDatas[i].countTimeHealing -= Time.deltaTime;
                        if (staffDatas[i].countTimeHealing < 0)
                        {
                            staffDatas[i].countTimeHealing = 3f;
                            staffDatas[i].statement = "stand";
                            changeStaffs[i] = true;
                            if (textStaff == null) textStaff = new WarningTextView();
                            textStaff.RemoveWarning(3);
                        }
                    }
                    #endregion
                    if (cusDatas[i, 0].indexProduct != -1)//if have customer
                    {
                        if (staffDatas[i].statement.Equals("sell") || staffDatas[i].statement.Equals("training"))//servicing 
                        {
                            staffDatas[i].curtimeService += Time.deltaTime;
                            #region complete service
                            if (staffDatas[i].curtimeService >= staffDatas[i].timeService)
                            {
                                staffDatas[i].curtimeService = 0;
                                CommonObjectScript.arrayProducts[cusDatas[i, 0].indexProduct]--;

                                //add to target sell all product
                                MissionData.shopDataMission.listProducts[productDatas[cusDatas[i, 0].indexProduct].index].currentNumber++;
                                MissionData.shopDataMission.currentNumber++;
                                getAchievement(cusDatas[i, 0].indexProduct);

                                //reward star and coin
                                if (isUsingItemWeather && checkBonusFromWeatherItem())
                                {
                                    isCompleteService(i, cusDatas[i, 0].star, priceCakes[cusDatas[i, 0].indexProduct], true);
                                    MissionData.shopDataMission.currentLevel++;// tăng ::
                                    DialogAchievement.AddDataAchievement(15, 1); // tặng quà khách
                                }
                                else isCompleteService(i, cusDatas[i, 0].star, priceCakes[cusDatas[i, 0].indexProduct]);

                                //check healthy of staff
                                staffDatas[i].countToCheck++;
                                if (staffDatas[i].countToCheck > 3) staffDatas[i].countToCheck = 0;
                                tempState = getStatementAvailable(i);
                                if (!tempState.Equals(staffDatas[i].statement))
                                {
                                    staffDatas[i].statement = tempState;
                                    changeStaffs[i] = true;
                                    if (VariableSystem.language == null || VariableSystem.language.Equals("Vietnamese"))
                                        textStaff = new WarningTextView("Nhân viên gặp vấn đề", 3);
                                    else
                                        textStaff = new WarningTextView("Staff have problem", 3);
                                }
                                else
                                {
                                    #region staff rest when finish service
                                    if (staffDatas[i].statement.Equals("sell"))
                                    {
                                        staffDatas[i].statement = "stand";
                                        changeStaffs[i] = true;
                                    }
                                    else if (staffDatas[i].statement.Equals("training"))
                                    {
                                        staffDatas[i].statement = "training_stand";
                                        changeStaffs[i] = true;
                                    }
                                    #endregion
                                }
                            }
                            #endregion
                        }
                        else if (isAvailableService(i) && CommonObjectScript.arrayProducts[cusDatas[i, 0].indexProduct] > 0)//if can service
                        {
                            #region service
                            if (staffDatas[i].statement.Equals("stand"))
                            {
                                staffDatas[i].statement = "sell";
                                changeStaffs[i] = true;
                            }
                            else if (staffDatas[i].statement.Equals("training_stand"))
                            {
                                staffDatas[i].statement = "training";
                                changeStaffs[i] = true;
                            }
                            #endregion
                        }
                        #region customer waiting
                        cusDatas[i, 0].timeWait -= Time.deltaTime;
                        if (cusDatas[i, 0].timeWait < 0)
                        {
                            cusDatas[i, 0] = new Customer();
                            isCompleteService(i, 0, 0);//hết kiên nhẫn
                            staffDatas[i].curtimeService = 0;
                        }
                        #endregion
                    }
                }
            }
            #endregion
        }
        else if (VariableSystem.mission == 2)
        {
            if (numberOfStaff == 1 && cusDatas[0, 0].indexProduct == -1) CreateCustomer(0);
        }
        if (Application.loadedLevelName.Equals("Mission"))
        {
            bonusCustomer = 0;
            maxCustomer = 0;
            isAddCustomer = false;
            GameObject.Destroy(this.gameObject);
        }
        else if (isNeedWarning && !Application.loadedLevelName.Equals("Store"))
        {
            common.WarningVisible(CommonObjectScript.Button.Shop);
            isNeedWarning = false;
        }
    }

    bool isAvailableService(int temp)
    {
        switch (staffDatas[temp].statement)
        {
            case "stand":
            case "sell":
            case "training":
            case "training_stand": return true;
            default: return false;
        }
    }
    string getStatementAvailable(int temp)
    {
        if (staffDatas[temp].countToCheck == 3)
            if (isCanSick)
            {
                int j = UnityEngine.Random.Range(0, 10000) % 100;
                if (j < staffDatas[temp].percentSick)
                {
                    if (!Application.loadedLevelName.Equals("Store")) common.WarningVisible(CommonObjectScript.Button.Shop);
                    j = UnityEngine.Random.Range(0, 10000) % (staffDatas[temp].speed + staffDatas[temp].health + staffDatas[temp].mental);
                    if (j < staffDatas[temp].speed) return "sleep";
                    else if (j < staffDatas[temp].speed + staffDatas[temp].health) return "sick";
                    else return "lazy";
                }
            }
        return staffDatas[temp].statement;
    }
    bool checkBonusFromWeatherItem()
    {
        if (common.itemSelected.Equals("Kem") && common.weatherDay.Equals("nang")) return true;
        if (common.itemSelected.Equals("O") && common.weatherDay.Equals("mua")) return true;
        if (common.itemSelected.Equals("Gangtay") && common.weatherDay.Equals("tuyet")) return true;
        return false;
    }
    bool CreateCustomer(int typeCus = -1)//add customer
    {
        availableRow.Clear();
        //get list row can put one customer
        if (staffDatas[0].levelStaff != 0 && cusDatas[0, 4].indexProduct == -1) availableRow.Add(0);
        if (staffDatas[1].levelStaff != 0 && cusDatas[1, 4].indexProduct == -1) availableRow.Add(1);
        if (staffDatas[2].levelStaff != 0 && cusDatas[2, 4].indexProduct == -1) availableRow.Add(2);

        if (availableRow.Count != 0)
        {
            int temRow = availableRow[UnityEngine.Random.Range(0, 1250) % availableRow.Count];//random row for new customer (0->2)
            cus = new Customer(typeCus);
            cus.indexProduct = randomProduct();
            cus.indexRow = temRow;
            for (int i = 0; i < 5; i++)
            {
                if (cusDatas[temRow, i].indexProduct == -1)//catch slot empty
                {
                    cusDatas[temRow, i] = cus;// add new customer into slot empty.
                    if (Application.loadedLevelName.Equals("Store"))//when player's using shop scene.
                    {
                        GameObject.Find("UI Root").transform.Find("PanelCustomer").GetComponent<CustomerScript>().CreateCustomer(temRow, i, cus.typeCustomer, cus.indexProduct, true);
                    }
                    break;
                }
            }
            return true;
        }
        return false;
    }
    int randomProduct() //random product request of customer
    {
        int temp;
        tempDataRandomProduct.Clear();//xóa trắng
        totalNumberRequest = 0;//xóa trắng
        for (int i = 0; i < MissionData.shopDataMission.listProducts.Count; i++)
        {
            if (MissionData.shopDataMission.listProducts[i].numberRequest > 0)
            {
                tempDataRandomProduct.Add(i);
                totalNumberRequest += MissionData.shopDataMission.listProducts[i].numberRequest;
            }
        }
        if (totalNumberRequest != 0)
        {
            temp = UnityEngine.Random.Range(0, 1250) % totalNumberRequest;
            totalNumberRequest = 0;
            for (int i = 0; i < tempDataRandomProduct.Count; i++)
            {
                totalNumberRequest += MissionData.shopDataMission.listProducts[tempDataRandomProduct[i]].numberRequest;
                if (temp < totalNumberRequest)
                {
                    MissionData.shopDataMission.listProducts[tempDataRandomProduct[i]].numberRequest--;
                    return MissionData.shopDataMission.listProducts[tempDataRandomProduct[i]].idProduct - 7;
                }
            }
            MissionData.shopDataMission.listProducts[tempDataRandomProduct[0]].numberRequest--;
            return MissionData.shopDataMission.listProducts[tempDataRandomProduct[0]].idProduct - 7;
        }
        else
        {
            for (int i = 0; i < MissionData.shopDataMission.listProducts.Count; i++)
            {
                if (MissionData.shopDataMission.listProducts[i].numberRequest == 0) tempDataRandomProduct.Add(i);
            }
            if (tempDataRandomProduct.Count == 0)
            {
                return MissionData.shopDataMission.listProducts[0].idProduct - 7;
            }
            else
            {
                temp = UnityEngine.Random.Range(0, 1250) % tempDataRandomProduct.Count;
                return MissionData.shopDataMission.listProducts[tempDataRandomProduct[temp]].idProduct - 7;
            }
        }
    }
    void getAchievement(int index)
    {
        DialogAchievement.AddDataAchievement(11, 1); // phục vụ khách hàng
        switch (index)
        {
            case 0: DialogAchievement.AddDataAchievement(7, 1); break; //bánh mì
            case 5: DialogAchievement.AddDataAchievement(16, 1); break; //cá nướng
            case 6: DialogAchievement.AddDataAchievement(10, 1); break; //đùi gà quay
            case 7: DialogAchievement.AddDataAchievement(17, 1); break; //humberger
            case 9: DialogAchievement.AddDataAchievement(18, 1); break; //nho
            case 11: DialogAchievement.AddDataAchievement(9, 1); break; //sữa dâu
            case 12: DialogAchievement.AddDataAchievement(8, 1); break; //sữa tươi
            default: break;
        }
    }

    void isCompleteService(int row, int star, int money, bool isBonus = false)
    {
        //thay đổi hàng chờ
        for (int j = 0; j < 5; j++)
        {
            if (j < 4)
            {
                cusDatas[row, j] = cusDatas[row, j + 1];
            }
            else
            {
                cusDatas[row, j] = new Customer();
            }
        }
        if (star != 0)
        {
            common.AddDollar((int)(money * (isBonus ? 1.5f : 1f) * (MissionPowerUp.DoubleMoney ? 2 : 1)));//double coin
            common.AddStar(star + (DialogShop.BoughtItem[9] ? 1 : 0)); //add 1star
        }
        if (Application.loadedLevelName.Equals("Store"))//when player's using shop scene.
        {
            GameObject.Find("UI Root").transform.Find("PanelCustomer").GetComponent<CustomerScript>().UpdatedRow(row, star + (DialogShop.BoughtItem[9] ? 1 : 0), money * (MissionPowerUp.DoubleMoney ? 2 : 1), isBonus);
        }
    }//when one staff leave off queue because completed service or angry
    public void AddStaff(int level, int speed, int health, int mental, int percent, int type)
    {
        if (staffDatas[0].typeStaff == 0)
        {
            staffDatas[0].setData(level, speed, health, mental, percent, type);
        }
        else if (staffDatas[1].typeStaff == 0)
        {
            staffDatas[1].setData(level, speed, health, mental, percent, type);
        }
        else if (staffDatas[2].typeStaff == 0)
        {
            staffDatas[2].setData(level, speed, health, mental, percent, type);
        }
        else
        {
            Debug.LogError("Maximum staff ! Why do you add me?");
        }
        numberOfStaff++;
        listStaffsinShop[type - 1].isHired = true;
    }//hire staff
    public void Training(int type, int speed, int health, int mental, int percent)
    {
        for (int i = 0; i < 3; i++)
        {
            if (staffDatas[i].typeStaff == type)
            {
                staffDatas[i].Training();
                this.speed = speed;
                this.health = health;
                this.mental = mental;
                this.percent = percent;
                countTimeTraining = 48;
                indexTraining = i;
            }
        }
    }
    public void Upgrade()
    {
        listStaffsinShop[staffDatas[indexTraining].typeStaff - 1].currentLevel++;
        if (VariableSystem.mission != 0 && listStaffsinShop[staffDatas[indexTraining].typeStaff - 1].currentLevel == MissionData.townDataMission.targetTraning.targetLevel)
        {
            MissionData.townDataMission.targetTraning.currentNumber++;
        }
        staffDatas[indexTraining].setData(staffDatas[indexTraining].levelStaff + 1, speed, health, mental, percent);
        if (staffDatas[indexTraining].statement.Equals("training"))
            staffDatas[indexTraining].statement = "sell";
        else staffDatas[indexTraining].statement = "stand";
        countTimeTraining = 0f;
        changeStaffs[indexTraining] = true;
    }
    public static int getNumberCustomer()
    {
        if (VariableSystem.mission < 10) return 5;
        if (VariableSystem.mission < 20) return 6;
        if (VariableSystem.mission < 30) return 7;
        if (VariableSystem.mission < 40) return 8;
        if (VariableSystem.mission < 50) return 9;
        return 10;
    }
    public static void getBonueResearch(int percent)
    {
        bonusCustomer += maxCustomer * percent / 100;
        isAddCustomer = true;
    }
}