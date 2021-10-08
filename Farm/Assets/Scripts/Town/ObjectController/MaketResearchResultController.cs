using Assets.Scripts.Common;
using Assets.Scripts.Store;
using System;
using System.Collections.Generic;
using UnityEngine;

public class MaketResearchResultController : MonoBehaviour
{

    // Use this for initialization
    public UIGrid[] gridItem;
    private static Texture[] listSpriteProduct;
    public GameObject[] listProductView;
    public UILabel[] listLabelProductView;
    public UILabel[] labelOther;
    public UIScrollView scrollView;
    public int precisionMax, precisionMin;
    private int count;
    private int totalNumberRequest;

    private List<int> IDAllowViewProduct;
    private List<ProductInforMaketResearchResult> listProductInfor;
    AudioControl audioControl;
    void OnEnable()
    {
        CommonObjectScript.isViewPoppup = true;
      
    }
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    void Start()
    {
        labelOther[0].text = TownScenesController.languageTowns["CONGRATULATIONS"];
        labelOther[1].text = TownScenesController.languageTowns["MaketResearchResult"];
        #region for test
        //precisionMax = 80;
        //precisionMin = 60;
        // MissionData.READ_XML(4);
        #endregion
        audioControl = GameObject.Find("AudioControl").GetComponent<AudioControl>();
       
        if (listSpriteProduct == null)
            listSpriteProduct = Resources.LoadAll<Texture>("Factory/Button/Images/Product");

        IDAllowViewProduct = new List<int>();
        IDAllowViewProduct.Clear();
        SetListProductAllow();

        listProductInfor = new List<ProductInforMaketResearchResult>();
        listProductInfor.Clear();
        SetPercentProduct();

        SetVisibleGridItem();
        CreatItemProduct();
        ControlViewHelp();

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || CommonObjectScript.isEndGame || CommonObjectScript.isViewComplete)
        {
            if (!TownScenesController.isHelp)
                Close_Click();
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
    void SetVisibleGridItem()
    {
        if (IDAllowViewProduct.Count > 6)
        {
            scrollView.GetComponent<UIScrollView>().enabled = true;
        }
        else
        {
            scrollView.GetComponent<UIScrollView>().enabled = false;
        }

        int countListAllow = (IDAllowViewProduct.Count - 1) / 3;
        for (int count = 0; count <= countListAllow; count++)
        {
            gridItem[count].gameObject.SetActive(true);
        }
    }
    void CreatItemProduct()
    {
        count = 0;
        for (int countListProductInfor = listProductInfor.Count - 1; countListProductInfor >= 0; countListProductInfor--)
        {
            if (listProductInfor[countListProductInfor].percentProduct != 0)
            {
                listProductView[count].gameObject.SetActive(true);
                listProductView[count].GetComponent<UITexture>().mainTexture = listSpriteProduct[listProductInfor[countListProductInfor].iDProduct - 7];
                listLabelProductView[count].text = listProductInfor[countListProductInfor].percentProduct.ToString();
                count++;
            }
        }
    }
    void SetListProductAllow()
    {
        foreach (ProductData PR in MissionData.shopDataMission.listProducts)
        {
            totalNumberRequest += PR.numberRequest;
            IDAllowViewProduct.Add(PR.idProduct);
        }
    }

    void SetPercentProduct()
    {
        int result = UnityEngine.Random.Range(precisionMin * 1000, precisionMax * 1000) / 1000;
        print("result: " + result);
        foreach (ProductData PR in MissionData.shopDataMission.listProducts)
        {
            if (PR.numberRequest != 0)
                listProductInfor.Add(new ProductInforMaketResearchResult(PR.idProduct, result * PR.numberRequest / 100));
        }
        listProductInfor.Sort();
    }

    public void Close_Click()
    {
        audioControl.PlaySound("Click 1");
        if (!TownScenesController.isHelp)
            this.gameObject.GetComponent<Animator>().Play("InVisible");
        else
        {
            if (VariableSystem.mission == 11 && CreatAndControlPanelHelp.countClickHelpPanel == 7)
                this.gameObject.GetComponent<Animator>().Play("InVisible");
            DestroyObjecHelp("CircleHelp");
            DestroyObjecHelp("HandHelp");
            CreatAndControlPanelHelp.countClickHelpPanel = 8;
        }
        CommonObjectScript.isViewPoppup = false;
    }
    void Destroy()
    {
        GameObject commonObject = GameObject.Find("CommonObject");

        if (commonObject.transform.Find("MaketResearchResult") != null)
            Destroy(commonObject.transform.Find("MaketResearchResult").gameObject);

        commonObject.GetComponent<CommonObjectScript>().maketResearchResult = this.gameObject;
        gameObject.transform.parent = commonObject.transform;
        gameObject.SetActive(false);
        commonObject.GetComponent<CommonObjectScript>().ResultButtonVisible();
        commonObject.GetComponent<CommonObjectScript>().typeReasearch = 0;

        TownScenesController.townsBusy[3] = false;
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
            if (VariableSystem.mission == 11)
            {
                if (CreatAndControlPanelHelp.countClickHelpPanel == 7)
                {
                    CreatObjectHelp("CircleHelp", new Vector3(30f, 30f, 30f), new Vector3(352, 235, 0));
                    CreatObjectHelp("HandHelp", new Vector3(-100f, 100f, 100f), new Vector3(300, 200, 0));
                }
            }
        }
    }
}
public class ProductInforMaketResearchResult : IComparable<ProductInforMaketResearchResult>
{
    public int iDProduct { set; get; }
    public int percentProduct { set; get; }

    public ProductInforMaketResearchResult(int iDProduct, int percentProduct)
    {
        this.iDProduct = iDProduct;
        this.percentProduct = percentProduct;
    }
    public int CompareTo(ProductInforMaketResearchResult other)
    {
        return this.percentProduct.CompareTo(other.percentProduct);
    }
}