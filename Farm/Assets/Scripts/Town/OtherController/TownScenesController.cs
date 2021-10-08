using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.IO;
using Assets.Scripts.Common;
using System;

public class TownScenesController : MonoBehaviour
{
    #region fortest
    // private int language; // 0 : Eng 1: Vie

    #endregion
    public static bool[] townsBusy;

    public static bool isCreat;
    public static List<int> ListLevelIntroduce;
    public static List<string> ListTextIntroduce;
    public static List<MutilmediaInfomation> ListMutilmediaInfomation;
    public static bool isHelp;
    //public static bool isContruduce;
    private ProductInfomation productTemp;
    private XmlNodeList levelNodeListIntroduce;

    public static List<MaketResearchInforType> MaketResearchInforType;
    public static List<MaketResearchItem> ListMaketResearchItem;
    public static List<Technogy> ListTechnogyData;

    public static Queue<GameObject> queuePopup;

    public static Dictionary<string, string> languageTowns = null;
    public static Dictionary<string, string> languageTownsEN = null;
    public static Dictionary<string, string> languageTownsVI = null;
    void Awake()
    {
        //DontDestroyOnLoad(gameObject);
    }
    void Start()
    {
        #region for test in screen

        //VariableSystem.language = "Vietnamese";
        // VariableSystem.language = "English";
        // FactoryScenesController.SetLaguage();
        // MissionData.READ_XML(6);

        // SetListMachineHaved();
        #endregion


        isCreat = true;
        // isContruduce = false;
        // VariableSystem.mission = 2;
        queuePopup = new Queue<GameObject>();

        //if ("Vietnamese".Equals(VariableSystem.language))
        //    language = 1;
        //else language = 0;

        townsBusy = new bool[6];

        ListLevelIntroduce = new List<int>();
        ListLevelIntroduce.Clear();
        ReadIntroduceLevel();
        SetIsHelp();

        ListTextIntroduce = new List<string>();
        ListTextIntroduce.Clear();
        ReadIntroduceText();

        ListMutilmediaInfomation = new List<MutilmediaInfomation>();
        ListMutilmediaInfomation.Clear();
        ReadMultimediaInformation();

        MaketResearchInforType = new List<MaketResearchInforType>();
        MaketResearchInforType.Clear();
        ReadMaketResearchInforType();

        ListMaketResearchItem = new List<MaketResearchItem>();
        ListMaketResearchItem.Clear();
        ReadMaketResearchItem();

        ListTechnogyData = new List<Technogy>();
        ListTechnogyData.Clear();
        ReadTechnogyData();
        SetLaguage();

    }

    void ReadIntroduceLevel()
    {
        TextAsset xml = Resources.Load<TextAsset>("Town/XMLFile/IntroduceLevel");
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.Load(new StringReader(xml.text));
        XmlNodeList xmlNodeList = xmlDoc.DocumentElement.ChildNodes;
        XmlNodeList levelNodeListLevelIntroduce = xmlNodeList.Item(0).ChildNodes;
        for (int i = 0; i < levelNodeListLevelIntroduce.Count; i++)
        {
            int temp = int.Parse(levelNodeListLevelIntroduce.Item(i).Attributes.Item(0).Value);
            ListLevelIntroduce.Add(temp);
        }
    }

    void SetIsHelp()
    {
        foreach (int levelIntroduce in ListLevelIntroduce)
        {
            if (CommonObjectScript.isGuide && VariableSystem.mission == levelIntroduce)
                
                {
                    isHelp = true;
                }

            if (isHelp && VariableSystem.mission == 7)
            {
                LotteryController.countSpin = 0;
               // LotteryController.SaveCountSpin();
            }
        }
    }

    void ReadIntroduceText()
    {
        if (isHelp)
        {
            TextAsset xml = Resources.Load<TextAsset>("Vietnamese".Equals(VariableSystem.language) ? "Town/XMLFile/IntroduceTownViet" : "Town/XMLFile/IntroduceTownEng");
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(new StringReader(xml.text));
            XmlNodeList xmlNodeList = xmlDoc.DocumentElement.ChildNodes;
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

    void ReadMultimediaInformation()
    {
        TextAsset xml = Resources.Load<TextAsset>("Town/XMLFile/MultimediaInformation");
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.Load(new StringReader(xml.text));
        XmlNodeList xmlNodeList = xmlDoc.DocumentElement.ChildNodes;
        for (int i = 0; i < xmlNodeList.Count; i++)
        {
            ListMutilmediaInfomation.Add
                (
                new MutilmediaInfomation(
                    int.Parse(xmlNodeList.Item(i).Attributes.Item(0).Value),
                    xmlNodeList.Item(i).Attributes.Item(1).Value,
                    xmlNodeList.Item(i).Attributes.Item(2).Value,
                    DialogShop.BoughtItem[13] ? (int.Parse(xmlNodeList.Item(i).Attributes.Item(3).Value) + 10) : int.Parse(xmlNodeList.Item(i).Attributes.Item(3).Value),
                    DialogShop.BoughtItem[13] ? (int.Parse(xmlNodeList.Item(i).Attributes.Item(4).Value) + 10) : int.Parse(xmlNodeList.Item(i).Attributes.Item(4).Value),
                    int.Parse(xmlNodeList.Item(i).Attributes.Item(5).Value),
                    int.Parse(xmlNodeList.Item(i).Attributes.Item(6).Value)
                    )
                );
        }
    }

    void ReadMaketResearchInforType()
    {
        TextAsset xml = Resources.Load<TextAsset>("Town/XMLFile/MaketResearchInforType");
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.Load(new StringReader(xml.text));
        XmlNodeList xmlNodeList = xmlDoc.DocumentElement.ChildNodes;
        XmlNodeList xmlNodeListContent = xmlNodeList.Item("Vietnamese".Equals(VariableSystem.language) ? 1 : 0).ChildNodes;
        for (int countXmlNodeListContent = 0; countXmlNodeListContent < xmlNodeListContent.Count; countXmlNodeListContent++)
        {
            MaketResearchInforType.Add(
                new MaketResearchInforType(
                   int.Parse(xmlNodeListContent.Item(countXmlNodeListContent).Attributes.Item(0).Value),
                   xmlNodeListContent.Item(countXmlNodeListContent).Attributes.Item(1).Value,
                   xmlNodeListContent.Item(countXmlNodeListContent).Attributes.Item(2).Value)
                );
        }
    }

    void ReadMaketResearchItem()
    {
        TextAsset xml = Resources.Load<TextAsset>("Town/XMLFile/MaketReseachItem");
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.Load(new StringReader(xml.text));
        XmlNodeList xmlNodeList = xmlDoc.DocumentElement.ChildNodes;
        for (int countXmlNodeList = 0; countXmlNodeList < xmlNodeList.Count; countXmlNodeList++)
        {
            ListMaketResearchItem.Add(
                new MaketResearchItem(
                   int.Parse(xmlNodeList.Item(countXmlNodeList).Attributes.Item(0).Value),
                   xmlNodeList.Item(countXmlNodeList).Attributes.Item(1).Value,
                   xmlNodeList.Item(countXmlNodeList).Attributes.Item(2).Value,
                   SetPercent(DialogShop.BoughtItem[14] ? (int.Parse(xmlNodeList.Item(countXmlNodeList).Attributes.Item(3).Value) + 10) : int.Parse(xmlNodeList.Item(countXmlNodeList).Attributes.Item(3).Value)),
                   SetPercent(DialogShop.BoughtItem[14] ? (int.Parse(xmlNodeList.Item(countXmlNodeList).Attributes.Item(4).Value) + 10) : int.Parse(xmlNodeList.Item(countXmlNodeList).Attributes.Item(4).Value)),
                   int.Parse(xmlNodeList.Item(countXmlNodeList).Attributes.Item(5).Value),
                   int.Parse(xmlNodeList.Item(countXmlNodeList).Attributes.Item(6).Value),
                   int.Parse(xmlNodeList.Item(countXmlNodeList).Attributes.Item(7).Value)
                    )
                );
        }
    }

    void ReadTechnogyData()
    {

        TextAsset xml = Resources.Load<TextAsset>("Town/XMLFile/TechnogyData");
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.Load(new StringReader(xml.text));
        XmlNodeList xmlNodeList = xmlDoc.DocumentElement.ChildNodes;

        for (int countNodeList = 0; countNodeList < xmlNodeList.Count; countNodeList++)
        {
            ListTechnogyData.Add(
                new Technogy(
                     int.Parse(xmlNodeList.Item(countNodeList).Attributes.Item(0).Value),
                               xmlNodeList.Item(countNodeList).Attributes.Item(1).Value,
                     int.Parse(xmlNodeList.Item(countNodeList).Attributes.Item(2).Value),
                     int.Parse(xmlNodeList.Item(countNodeList).Attributes.Item(3).Value),
                     xmlNodeList.Item(countNodeList).Attributes.Item(4).Value
                    ));
        }
    }
    void SetLaguage()
    {
        if (languageTowns == null)
        {
            //Doc xml ngon ngu
            languageTownsEN = new Dictionary<string, string>();
            languageTownsVI = new Dictionary<string, string>();

            TextAsset xml = Resources.Load<TextAsset>("Town/XMLFile/LanguageTowns"); //Read File xml
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(new StringReader(xml.text));
            XmlNodeList xmlNodeList = xmlDoc.DocumentElement.ChildNodes;
            foreach (XmlNode node in xmlNodeList)
            {
                languageTownsEN.Add(node.Name, node.Attributes.Item(0).Value);
                languageTownsVI.Add(node.Name, node.Attributes.Item(1).Value);
            }
        }

        if ("Vietnamese".Equals(VariableSystem.language))
            languageTowns = languageTownsVI;
        else
            languageTowns = languageTownsEN;
       // print(VariableSystem.language + "cccccccccccccccccccccccccccccc");
    }
    int SetPercent(int percen)
    {
        if (percen > 100)
            return 100;
        else
            return percen;
    }
}

public class MutilmediaInfomation
{
    public int iD { set; get; }
    public string engName { set; get; }
    public string vieName { set; get; }
    public int minRation { set; get; }
    public int maxRation { set; get; }
    public int time { set; get; }
    public int cost { set; get; }

    public MutilmediaInfomation(int iD, string engName, string vieName, int minRation, int maxRation, int time, int cost)
    {
        this.iD = iD;
        this.engName = engName;
        this.vieName = vieName;
        this.minRation = minRation;
        this.maxRation = maxRation;
        this.time = time;
        this.cost = cost;
    }
}

public class MaketResearchInforType
{
    public int iD { set; get; }
    public string name { set; get; }
    public string infor { set; get; }

    public MaketResearchInforType(int iD, string name, string infor)
    {
        this.iD = iD;
        this.name = name;
        this.infor = infor;
    }
}

public class MaketResearchItem
{
    public int iD { set; get; }
    public string engName { set; get; }
    public string vieName { set; get; }
    public int precisionMax { set; get; }
    public int precisionMin { set; get; }
    public int costDiamond { set; get; }
    public int costCoin { set; get; }
    public int time { set; get; }

    public MaketResearchItem(int iD, string engName, string vieName, int precisionMax, int precisionMin, int costDiamond, int costCoin, int time)
    {
        this.iD = iD;
        this.engName = engName;
        this.vieName = vieName;
        this.precisionMax = precisionMax;
        this.precisionMin = precisionMin;
        this.costDiamond = costDiamond;
        this.costCoin = costCoin;
        this.time = time;
    }
}

public class Technogy
{
    public int iD { set; get; }
    public string name { set; get; }
    public int cost { set; get; }
    public int deltaCost { set; get; }
    public int[] newProducts = new int[3];
    public Technogy(int iD, string name, int cost, int deltaCost, string newPro)
    {
        this.iD = iD;
        this.name = name;
        this.cost = cost;
        this.deltaCost = deltaCost;
        if (!newPro.Equals(""))
        {
            string[] temp = newPro.Split(';');
            newProducts[0] = Convert.ToInt16(temp[0]);
            newProducts[1] = Convert.ToInt16(temp[1]);
            newProducts[2] = Convert.ToInt16(temp[2]);
        }
    }
}