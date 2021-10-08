using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.IO;
using Assets.Scripts.Common;
using Assets.Scripts.Store;
using System;

public class FactoryScenesController : MonoBehaviour
{

    // Chứa các biến toàn cục điều khiển Factory

    public static List<List<PositionMachine>> LPositionMachine;
    public static int IDBackGroundButton;
    public static List<int> IDCreatMachine; // list các id back ground đã có máy
    public static List<int> ListMachineHaved; // Các loại máy có trong nhà máy

    public static List<List<ProductForMachine>> ListProductAllowedOfMachine; // sản phẩm được phép sản xuất theo từng loại máy
    private List<ProductForMachine> ListProductOfMission;
    public static List<MachineInfomation> listMachineInfomation; // Thông tin máy

    public static string nameClick; // click vào máy hay click vào back ground ... để xét item trên button view

    public static List<int> ListQueue; // list số hàng chờ của từng loại máy cũng chính là level của từng loại máy
    public static List<ProductInfomation> listProductInformation; // list thông tin của các loại sản phẩm trong game
    public static bool isChangeLevel;

    public static List<int> ListLevelIntroduce;
    public static List<string> ListTextIntroduce;

    public static bool isCreat;
    public static bool isHelp;
    private XmlNodeList levelNodeListIntroduce;

    private List<MachineGiven> listGiveMachine; // lưu giữ type nao được đặt trước và đặt trước bao nhiêu cái
    private GameObject machineClone;

    public static List<int> listUnlockByPlayer;

    public static Dictionary<string, string> languageHungBV = null;
    public static Dictionary<string, string> languageHungBVEN = null;
    public static Dictionary<string, string> languageHungBVVI = null;
    void Awake()
    {

        //DontDestroyOnLoad(transform.gameObject);
    }
    void Start()
    {
        #region for test in screen
        //VariableSystem.mission = 4;
        //VariableSystem.language = "Vietnamese";
        // VariableSystem.language = "English";
        // MissionData.READ_XML(VariableSystem.mission);
        // CommonObjectScript.dollar = 100000;
        // CommonObjectScript.diamond = 10000;
        //for (int i = 0; i < CommonObjectScript.arrayMaterials.Length; i++)
        //    CommonObjectScript.arrayMaterials[i] = 5;
        //  MissionData.READ_XML(4);
        //VariableSystem.mission = 4;
        //VariableSystem.language = "Vietnamese";
        //// VariableSystem.language = "English";
        //MissionData.READ_XML(VariableSystem.mission);
        //CommonObjectScript.dollar = 100000;
        //VariableSystem.diamond = 10000;
        //for (int i = 0; i < CommonObjectScript.arrayMaterials.Length; i++)
        //    CommonObjectScript.arrayMaterials[i] = 5;
        //MissionData.READ_XML(4);

        SetLaguage();
        #endregion


        isHelp = false;
        isCreat = true;



        LPositionMachine = new List<List<PositionMachine>>();
        LPositionMachine.Clear();
        ListLevelIntroduce = new List<int>();
        ListLevelIntroduce.Clear();

        ReadMachinePosition();
        SetIsHelp();


        IDCreatMachine = new List<int>();
        IDCreatMachine.Clear();

        ListQueue = new List<int>(9);
        ListQueue.Clear();
        SetLevelForMachine();


        listMachineInfomation = new List<MachineInfomation>();
        listMachineInfomation.Clear();
        ReadMachineInformation();

        ListTextIntroduce = new List<string>();
        ListTextIntroduce.Clear();
        ReadIntroduceText();

        listProductInformation = new List<ProductInfomation>();
        listProductInformation.Clear();
        ReadProductInfor();

        ListProductOfMission = new List<ProductForMachine>();
        ListProductOfMission.Clear();
        SetListProductOfMission();
        ListProductAllowedOfMachine = new List<List<ProductForMachine>>();
        ListProductAllowedOfMachine.Clear();
        SetProductForMachine(ListProductOfMission);

        listGiveMachine = new List<MachineGiven>();
        listGiveMachine.Clear();
        SetDataListGivenMachine();
        SetGivenMachine();
    }

    void ReadMachinePosition()
    {
        List<PositionMachine> listPositionTemp;
        TextAsset xml = Resources.Load<TextAsset>("Factory/XMLFile/MachinePosition");
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.Load(new StringReader(xml.text));
        XmlNodeList xmlNodeList = xmlDoc.DocumentElement.ChildNodes;
        XmlNodeList machineAllPostionNodeList = xmlNodeList.Item(0).ChildNodes;
        // XmlNodeList levelNodeList = xmlNodeList.Item( ControllGame.level ).ChildNodes;
        for (int countMachineAllPostionNodeList = 0; countMachineAllPostionNodeList < machineAllPostionNodeList.Count; countMachineAllPostionNodeList++)
        {
            listPositionTemp = new List<PositionMachine>();
            listPositionTemp.Clear();
            XmlNodeList machineOnePostionNodeList = machineAllPostionNodeList.Item(countMachineAllPostionNodeList).ChildNodes;
            for (int countMachineOnePostionNodeList = 0; countMachineOnePostionNodeList < machineOnePostionNodeList.Count; countMachineOnePostionNodeList++)
            {
                listPositionTemp.Add
                    (
                    new PositionMachine(
                        float.Parse(machineOnePostionNodeList.Item(countMachineOnePostionNodeList).Attributes.Item(1).Value),
                        float.Parse(machineOnePostionNodeList.Item(countMachineOnePostionNodeList).Attributes.Item(2).Value)
                        )
                    );
            }
            LPositionMachine.Add(listPositionTemp);
        }

        XmlNodeList levelNodeListLevelIntroduce = xmlNodeList.Item(1).ChildNodes;
        for (int i = 0; i < levelNodeListLevelIntroduce.Count; i++)
        {

            //print(levelNodeListLevelIntroduce.Item(i).Attributes.Item(0).Value);
            int temp = int.Parse(levelNodeListLevelIntroduce.Item(i).Attributes.Item(0).Value);
            ListLevelIntroduce.Add(temp);
        }
    }

    void SetLevelForMachine()
    {
        for (int countListMachineData = 0; countListMachineData < MissionData.factoryDataMission.machinedatas.Count; countListMachineData++)
        {
            ListQueue.Add(MissionData.factoryDataMission.machinedatas[countListMachineData].currentLevel);
        }
    }
    void ReadProductInfor()
    {
        int IDProduct;
        List<int> listIDMaterial;
        string productionName;
        float productionTime;
        int productionCostShop;
        int productionCostMarket;
        int levelMachineUnlock;
        TextAsset xml = Resources.Load<TextAsset>("Factory/XMLFile/ProductInfomation");
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.Load(new StringReader(xml.text));    //Debug.Log("Node root: " + xmlDoc.DocumentElement.Name); //----> xmlDoc.DocumentElement    
        XmlNodeList xmlNodeList = xmlDoc.DocumentElement.ChildNodes; // ----> xmlDoc.DocumentElement.ChildNodes = Tat ca cac man 

        for (int countxmlNodeList = 0; countxmlNodeList < xmlNodeList.Count; countxmlNodeList++)
        {
            XmlNodeList levelNodeList = xmlNodeList.Item(countxmlNodeList).ChildNodes;// ----> levelNodeList = xmlNodeList.Item(0).ChildNodes = Tat ca cac truong cua man1 
            listIDMaterial = new List<int>();
            for (int countlevelNodeList = 0; countlevelNodeList < levelNodeList.Item(0).Attributes.Count; countlevelNodeList++)
            {
                listIDMaterial.Add(int.Parse(levelNodeList.Item(0).Attributes.Item(countlevelNodeList).Value));
            }
            IDProduct = int.Parse(levelNodeList.Item(1).Attributes.Item(0).Value);
            productionName = levelNodeList.Item(1).Attributes.Item(1).Value;
            productionTime = DialogShop.BoughtItem[2] ? (float.Parse(levelNodeList.Item(1).Attributes.Item(2).Value) * 0.9f) : float.Parse(levelNodeList.Item(1).Attributes.Item(2).Value);
            productionCostShop = int.Parse(levelNodeList.Item(1).Attributes.Item(3).Value);
            productionCostMarket = int.Parse(levelNodeList.Item(1).Attributes.Item(4).Value);
            levelMachineUnlock = int.Parse(levelNodeList.Item(1).Attributes.Item(5).Value);
            listProductInformation.Add(
                new ProductInfomation(IDProduct, listIDMaterial, productionName,
                    productionTime, productionCostShop, productionCostMarket, levelMachineUnlock));
        }
    }

    void ReadMachineInformation()
    {
        int IDMachine;
        string machineNameVie;
        string machineNameEng;
        int machineCost;
        List<int> listProductOfMachine;
        List<int> listRationFail;

        TextAsset xml = Resources.Load<TextAsset>("Factory/XMLFile/MachineInformation");
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.Load(new StringReader(xml.text));
        XmlNodeList xmlNodeMachineList = xmlDoc.DocumentElement.ChildNodes;

        for (int countxmlNodeMachineList = 0; countxmlNodeMachineList < xmlNodeMachineList.Count; countxmlNodeMachineList++)
        {
            XmlNodeList InfoNodeList = xmlNodeMachineList.Item(countxmlNodeMachineList).ChildNodes;
            listProductOfMachine = new List<int>();
            listRationFail = new List<int>();

            IDMachine = int.Parse(InfoNodeList.Item(0).Attributes.Item(0).Value);
            machineNameVie = InfoNodeList.Item(0).Attributes.Item(1).Value;
            machineNameEng = InfoNodeList.Item(0).Attributes.Item(2).Value;
            machineCost = DialogShop.BoughtItem[6] ? ((int)(int.Parse(InfoNodeList.Item(0).Attributes.Item(3).Value) * 0.9f)) : int.Parse(InfoNodeList.Item(0).Attributes.Item(3).Value);

            for (int countNodeList = 0; countNodeList < InfoNodeList.Item(1).Attributes.Count; countNodeList++)
            {
                listProductOfMachine.Add(int.Parse(InfoNodeList.Item(1).Attributes.Item(countNodeList).Value));
            }

            for (int countNodeList = 0; countNodeList < InfoNodeList.Item(2).Attributes.Count; countNodeList++)
            {
                listRationFail.Add(int.Parse(InfoNodeList.Item(2).Attributes.Item(countNodeList).Value));
            }

            listMachineInfomation.Add(new MachineInfomation(IDMachine, machineNameVie, machineNameEng, machineCost, listProductOfMachine, listRationFail));
        }

        //foreach (MachineInfomation MI in listMachineInfomation)
        //{
        //    print(MI.listRationFail[0]);
        //}
    }

    void SetListProductOfMission()
    {
        //  print(MissionData.shopDataMission.listProducts.Count);
        foreach (ProductData PR in MissionData.shopDataMission.listProducts)
        {
            ListProductOfMission.Add(new ProductForMachine(PR.idProduct, PR.index, listProductInformation[PR.idProduct - 7].levelMachineUnlock));

        }
        ListProductOfMission.Sort();
    }
    void SetProductForMachine(List<ProductForMachine> listProductAllow)
    {
        List<ProductForMachine> IDViewProduct;
        for (int countMachine = 0; countMachine < 4; countMachine++)
        {
            IDViewProduct = new List<ProductForMachine>();
            for (int countListProductAllow = listProductAllow.Count - 1; countListProductAllow >= 0; countListProductAllow--)
            {
                //print(listProductAllow[countListProductAllow].iDType);
                for (int countListProductOfMachine = 0; countListProductOfMachine < listMachineInfomation[countMachine].listProductOfMachine.Count; countListProductOfMachine++)
                {
                    if (listMachineInfomation[countMachine].listProductOfMachine[countListProductOfMachine] == listProductAllow[countListProductAllow].iDType)
                    {
                        IDViewProduct.Add(listProductAllow[countListProductAllow]);
                        listProductAllow.RemoveAt(countListProductAllow);
                        break;
                    }
                }
            }
            //foreach (ProductForMachine i in IDViewProduct)
            //{
            //    print(i.iDType);
            //}
            ListProductAllowedOfMachine.Add(IDViewProduct);
        }
    }
    void SetIsHelp()
    {
        foreach (int levelIntroduce in ListLevelIntroduce)
        {
            if (CommonObjectScript.isGuide)
                if (VariableSystem.mission == levelIntroduce)
                {

                    isHelp = true;
                }
        }

        if (!isHelp)
        {
            CommonObjectScript.isGuide = false;
            //print("aaaaaaaaaaaaaaaa");
        }
    }

    void ReadIntroduceText()
    {
        if (isHelp)
        {

            TextAsset xml = Resources.Load<TextAsset>(VariableSystem.language.Equals("Vietnamese") ? "Factory/XMLFile/IntroduceFactoryViet" : "Factory/XMLFile/IntroduceFactoryEng");
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(new StringReader(xml.text));
            XmlNodeList xmlNodeList = xmlDoc.DocumentElement.ChildNodes;
            //print(xmlNodeList[1].Attributes.Item(0).Value);
            foreach (XmlNode xmlNode in xmlNodeList)
            {
                if (xmlNode.Attributes.Item(0).Value.Equals(VariableSystem.mission.ToString()))
                {
                    levelNodeListIntroduce = xmlNode.ChildNodes;
                }
            }
            for (int i = 0; i < levelNodeListIntroduce.Count; i++)
            {
                ListTextIntroduce.Add(levelNodeListIntroduce.Item(i).Attributes.Item(0).Value);
            }
        }

    }


    // iDType đặc trưng cho loại máy - là stt trong MissionData.factoryDataMission.machinedatas bat dau tu 0
    void CreatGivenMachine(int iDMachine, int indextInList, int IDBackGroundButton, bool isFail = false)
    {
        GameObject machinePrefabs = (GameObject)Resources.Load("Factory/Machine/Machine" + iDMachine);
        Vector2 position = LPositionMachine[iDMachine - 1][IDBackGroundButton - 1].Position(); // lấy vị trí máy đc fix cứng trong file xml ra cho từng idbackground đang chọn
        machineClone = (GameObject)Instantiate(machinePrefabs, new Vector3(position.x, position.y, 7), transform.rotation);
        machineClone.name = machinePrefabs.name;
        machineClone.GetComponent<MachineController>().idMachineType = iDMachine - 1; // iDmachine là data trong xml bắt đầu từ 1 
        machineClone.GetComponent<MachineController>().IDProductQueue = new Queue<ProductInfomation>();
        machineClone.GetComponent<MachineController>().sortingLayerID = IDBackGroundButton + 3;
        machineClone.GetComponent<MachineController>().idMachinePosition = IDBackGroundButton;
        machineClone.GetComponent<MachineController>().costMachine = MissionPowerUp.PriceDrop ? ((int)(listMachineInfomation[iDMachine - 1].machineCost * 0.75f)) : listMachineInfomation[iDMachine - 1].machineCost;
        machineClone.GetComponent<MachineController>().levelMachine = ListQueue[indextInList];
        machineClone.GetComponent<MachineController>().isFail = isFail;
        machineClone.GetComponent<MachineController>().indextMachine = indextInList;
        machineClone.GetComponent<MachineController>().nameMachine = "Vietnamese".Equals(VariableSystem.language) ?  listMachineInfomation[iDMachine - 1].machineNameVie :   listMachineInfomation[iDMachine - 1].machineNameEng ;
        IDCreatMachine.Add(IDBackGroundButton); // add id vào list id đã có máy trên đó
    }
    void SetDataListGivenMachine()
    {
        for (int countListMachinedatas = 0; countListMachinedatas < MissionData.factoryDataMission.machinedatas.Count; countListMachinedatas++)
        {
            if (MissionData.factoryDataMission.machinedatas[countListMachinedatas].startNumber != 0)
            {
                for (int countStartNumber = 0; countStartNumber < MissionData.factoryDataMission.machinedatas[countListMachinedatas].startNumber; countStartNumber++)
                {
                    listGiveMachine.Add(new MachineGiven(MissionData.factoryDataMission.machinedatas[countListMachinedatas].iDMachine, countListMachinedatas));
                }
            }
        }
    }
    void SetGivenMachine()
    {
        int count = 6;
        if (VariableSystem.mission != 13)
        {
            foreach (MachineGiven machineGiven in listGiveMachine)
            {
                CreatGivenMachine(machineGiven.iDType, machineGiven.indextInList, count);
                count--;
            }
        }
        else
        {
            foreach (MachineGiven machineGiven in listGiveMachine)
            {
                CreatGivenMachine(machineGiven.iDType, machineGiven.indextInList, count, true);
                count--;
            }
        }
    }

    // for test
    public static void SetLaguage()
    {
        if (languageHungBV == null)
        {
            //Doc xml ngon ngu
            languageHungBVEN = new Dictionary<string, string>();
            languageHungBVVI = new Dictionary<string, string>();

            TextAsset xml = Resources.Load<TextAsset>("Factory/XMLFile/EN"); //Read File xml
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(new StringReader(xml.text));
            XmlNodeList xmlNodeList = xmlDoc.DocumentElement.ChildNodes;
            foreach (XmlNode node in xmlNodeList)
            {
                languageHungBVEN.Add(node.Name, node.InnerText);
            }
            //vn
            TextAsset xml1 = Resources.Load<TextAsset>("Factory/XMLFile/VI"); //Read File xml
            XmlDocument xmlDoc1 = new XmlDocument();
            xmlDoc1.Load(new StringReader(xml1.text));
            XmlNodeList xmlNodeList1 = xmlDoc1.DocumentElement.ChildNodes;
            foreach (XmlNode node in xmlNodeList1)
            {
                languageHungBVVI.Add(node.Name, node.InnerText);
            }

            if ("Vietnamese".Equals(VariableSystem.language))
                languageHungBV = languageHungBVVI;
            else
                languageHungBV = languageHungBVEN;
        }
    }
}

public class PositionMachine
{
    private float positionX;
    private float positionY;
    public PositionMachine(float x, float y)
    {
        positionX = x;
        positionY = y;
    }
    public Vector2 Position()
    {
        return new Vector2(positionX, positionY);
    }
}
public class MachineGiven
{
    public int iDType { set; get; }
    public int indextInList { set; get; }
    public MachineGiven(int iDType, int indextInList)
    {
        this.iDType = iDType;
        this.indextInList = indextInList;
    }
}
public class ProductForMachine : IComparable<ProductForMachine>
{
    public int iDType { set; get; }
    public int indextInList { set; get; }
    public int levelMachineUnlock { set; get; }
    public ProductForMachine(int iDType, int indextInList, int levelMachineUnlock)
    {
        this.iDType = iDType;
        this.indextInList = indextInList;
        this.levelMachineUnlock = levelMachineUnlock;
    }
    public int CompareTo(ProductForMachine other)
    {
        return this.levelMachineUnlock.CompareTo(other.levelMachineUnlock);
    }

}
