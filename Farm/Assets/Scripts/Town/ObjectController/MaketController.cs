using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.IO;
using Assets.Scripts.Common;
using Assets.Scripts.Store;

public class MaketController : MonoBehaviour {

   // private List<int> IDAllowViewProductInMarket;
    public GameObject[] listProductView;
    public UIScrollView scrollView;
    public static Texture[] listSpriteProduct;
    public UILabel[] listCostProduct;
    public GameObject[] listContainerItemProduct;
    public UILabel labelTop;
    private List<ProductInfomation> listProductInformation;
    private Animator animator;
    private int count;
    AudioControl audioControl;
    void OnEnable()
    {
        //HùngBV
        CommonObjectScript.isViewPoppup = true;
       
    }
	void Start () {
        audioControl = GameObject.Find("AudioControl").GetComponent<AudioControl>();
        animator = GetComponent<Animator>();
        labelTop.text = TownScenesController.languageTowns["MAKET"];
        //IDAllowViewProductInMarket = new List<int> ();
       // IDAllowViewProductInMarket.Clear();
       // SetListProductOfMission();

        listProductInformation = new List<ProductInfomation>();
        listProductInformation.Clear();
        if (FactoryScenesController.isCreat)
        {
            listProductInformation = FactoryScenesController.listProductInformation;
        }
        else
        {
            ReadProductInfor();
        }
        if (listSpriteProduct == null)
            listSpriteProduct = Resources.LoadAll<Texture>("Factory/Button/Images/Product");
        CreatButtonProduct(MissionData.townDataMission.itemsInShop); //cái này mới đúng, sau này dùng
       // CreatButtonProduct(IDAllowViewProductInMarket);
	}
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || CommonObjectScript.isEndGame || CommonObjectScript.isViewComplete)
        {
            if (!TownScenesController.isHelp)
                ButtonCloseClick();
        }
    }
    void OnApplicationPause(bool pauseStatus)
    {
        if (!pauseStatus)
        {
            if (!FactoryScenesController.isHelp)
                ButtonCloseClick();
        }
    }
    void SetListProductOfMission()
    {
        foreach (ProductData PR in MissionData.shopDataMission.listProducts)
        {
            //IDAllowViewProductInMarket.Add(PR.idProduct);
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
            productionTime = float.Parse(levelNodeList.Item(1).Attributes.Item(2).Value);
            productionCostShop = int.Parse(levelNodeList.Item(1).Attributes.Item(3).Value);
            productionCostMarket = int.Parse(levelNodeList.Item(1).Attributes.Item(4).Value);

            levelMachineUnlock = int.Parse(levelNodeList.Item(1).Attributes.Item(5).Value);
            listProductInformation.Add(
                new ProductInfomation(IDProduct, listIDMaterial, productionName,
                    productionTime, productionCostShop, productionCostMarket, levelMachineUnlock));
        }
    }
    
    void CreatButtonProduct(List<int> IDView)
    {
        for (int id = 0; id < IDView.Count; id++) // sinh các button tương ứng với các mission và gán id cho các button
        {
            listProductView[count].GetComponent<UITexture>().mainTexture = listSpriteProduct[IDView[id] - 7];
            listContainerItemProduct[count].GetComponent<ContainerItemProductController>().IDProduct = IDView[id];
            listContainerItemProduct[count].GetComponent<ContainerItemProductController>().costProduct =
                DialogShop.BoughtItem[15] ? ((int)(listProductInformation[IDView[id] - 7].productionCostMarket * 0.85f)) : listProductInformation[IDView[id] - 7].productionCostMarket;
            listCostProduct[count].text = listProductInformation[IDView[id] - 7].productionCostMarket.ToString();
            listContainerItemProduct[count].SetActive(true);
            count++;
        }
    }

    void ResetScrollPosition()
    {
        //scrollView.GetComponentInChildren<UIScrollView>().GetComponent<Transform>().localPosition = new Vector3(0, -25, 0);
        //scrollView.GetComponentInChildren<UIScrollView>().GetComponent<UIPanel>().clipOffset = new Vector2(0, 0);
    }
    void Invisible()
    {
       this.gameObject.SetActive(false);
       CommonObjectScript.isViewPoppup = false;
       CreatTownScenesController.isDenyContinue = false;
    }

    public void ButtonCloseClick()
    {
        audioControl.PlaySound("Click 1");
        this.animator.Play("Invisible");
      
    }
}
