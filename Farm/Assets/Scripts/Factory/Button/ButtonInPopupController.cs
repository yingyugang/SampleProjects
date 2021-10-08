using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Common;

public class ButtonInPopupController : MonoBehaviour
{

    // Use this for initialization
    public int IDbutton; // cái này sẽ được gán cho các button riêng biệt ở bên class ButtonViewController để xác định loại máy(từ 1 - 6) và loại sản phẩm (từ 7 - 22)
    public int levelMachine;
    private GameObject machinePrefabs;
    private GameObject machineClone;

    // public int idReadData; // để giúp cho việc xác định xây cái máy nào trong bộ data máy đã cho, giá trị biến này được gán bên phần tạo nút của class ButtonViewController
    private Transform machineSelected;
    private GameObject buttonClickPayPrefabs;
    private GameObject buttonClickPayClone;

    private GameObject materialPrefabs;
    private GameObject materialClone;
    private GameObject materialFallPrefabs;

    private GameObject machineInforPrefabs;
    private GameObject machineInforClone;

    public static GameObject buttonProductSelect;

    private GameObject coinsPrefabs;
    private GameObject coinsClone;

    private GameObject fullQueuePrefabs;
    private GameObject fullQueueClone;

    public int costMachine;
    public string nameMachine;
    private float timeClick;
    private bool isClickButton;

    private bool isAllowCreatProduct;
    public int indextInList;

    AudioControl audioControl;
    void Start()
    {
        audioControl = GameObject.Find("AudioControl").GetComponent<AudioControl>();
        buttonClickPayPrefabs = (GameObject)Resources.Load("Factory/Queue/ButtonClickPay");
        fullQueuePrefabs = (GameObject)Resources.Load("Factory/Queue/FullQueue");

        if (FactoryScenesController.nameClick.Equals("BGClick"))
        {
            coinsPrefabs = (GameObject)Resources.Load("Factory/Machine/Coins");
        }
        else if (FactoryScenesController.nameClick.Equals("MachineClick"))
        {
            //machineSelected = MachineController.machineSelect;
            machineSelected = transform.parent.transform.parent;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isClickButton)
        {
            timeClick += Time.deltaTime;
        }
        if (isClickButton && timeClick > 0.15f)
        {
            if (FactoryScenesController.nameClick.Equals("MachineClick"))
            {
                if (FactoryScenesController.listProductInformation[IDbutton - 7].levelMachineUnlock <= machineSelected.GetComponent<MachineController>().levelMachine)
                {
                    CreatInforMaterialProduct(IDbutton);
                }
                else
                {
                    CreatNote(machineSelected.transform, new Vector3(1.2f, 1.2f, 1), new Vector3(0, 0, 0), FactoryScenesController.languageHungBV["UNLOCKPRODUCT"] + FactoryScenesController.listProductInformation[IDbutton - 7].levelMachineUnlock);
                }
            }
            else if (FactoryScenesController.nameClick.Equals("BGClick"))
            {
                CreatInforMachine();
            }
            isClickButton = false;
        }
        ControlViewHelp();
    }

    void OnMouseDown()
    {
        gameObject.transform.localScale = new Vector3(1.1f, 1.1f, 1f);
        if (FactoryScenesController.nameClick.Equals("BGClick"))
        {
            machinePrefabs = (GameObject)Resources.Load("Factory/Machine/Machine" + IDbutton);
        }
        else if (FactoryScenesController.nameClick.Equals("MachineClick"))
        {
            buttonProductSelect = gameObject;
        }
        isClickButton = true;
    }

    void OnMouseUp()
    {
        gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
        isClickButton = false;
        if (timeClick <= 0.15f)
        {
            if (FactoryScenesController.nameClick.Equals("BGClick"))
            {

                if (FactoryScenesController.isHelp && VariableSystem.mission == 1)
                {
                    if (CreatAndControlPanelHelp.countClickHelpPanel == 6)
                    {
                        CreatMachine();
                        CreatAndControlPanelHelp.countClickHelpPanel = 7;
                    }
                }
                else
                {
                    CreatMachine();
                }
            }
            else if (FactoryScenesController.nameClick.Equals("MachineClick"))
            {
                if (FactoryScenesController.isHelp && VariableSystem.mission == 1)
                {
                    if (CreatAndControlPanelHelp.countClickHelpPanel == 15)
                    {
                        CreatAndControlPanelHelp.countClickHelpPanel = 16;
                        CreatProduct();
                        DestroyObjecHelp("CircleHelp");
                        DestroyObjecHelp("HandHelp");

                    }
                }
                else
                {
                    CreatProduct();
                }
            }
        }
        else
        {
            if (FactoryScenesController.nameClick.Equals("BGClick"))
            {
                DestroyInfoMachine();
                if (FactoryScenesController.isHelp && VariableSystem.mission == 1)
                {
                    if (CreatAndControlPanelHelp.countClickHelpPanel == 5)
                        CreatAndControlPanelHelp.countClickHelpPanel = 6;
                }
            }
            else if (FactoryScenesController.nameClick.Equals("MachineClick"))
            {
                DestroyInforMaterialProduct();
                if (FactoryScenesController.isHelp && VariableSystem.mission == 1)
                {
                    if (CreatAndControlPanelHelp.countClickHelpPanel == 12)
                    {
                        CreatAndControlPanelHelp.countClickHelpPanel = 13;
                    }
                }
            }
        }
        timeClick = 0;

        if (FactoryScenesController.nameClick.Equals("MachineClick"))
            DestroyInforMaterialProduct();
        if (FactoryScenesController.nameClick.Equals("BGClick"))
            DestroyInfoMachine();
    }


    void CreatMachine()
    {
        if (CommonObjectScript.dollar >= costMachine)
        {
            Vector2 position = FactoryScenesController.LPositionMachine[IDbutton - 1][FactoryScenesController.IDBackGroundButton - 1].Position(); // lấy vị trí máy đc fix cứng trong file xml ra cho từng idbackground đang chọn

            machineClone = (GameObject)Instantiate(machinePrefabs, new Vector3(position.x, position.y, 7), transform.rotation);
            machineClone.name = machinePrefabs.name;
            machineClone.GetComponent<MachineController>().idMachineType = IDbutton - 1;
            machineClone.GetComponent<MachineController>().IDProductQueue = new Queue<ProductInfomation>();
            machineClone.GetComponent<MachineController>().sortingLayerID = FactoryScenesController.IDBackGroundButton + 3;
            machineClone.GetComponent<MachineController>().idMachinePosition = FactoryScenesController.IDBackGroundButton;
            machineClone.GetComponent<MachineController>().costMachine = costMachine;
            machineClone.GetComponent<MachineController>().levelMachine = levelMachine;
            machineClone.GetComponent<MachineController>().indextMachine = indextInList;
            machineClone.GetComponent<MachineController>().nameMachine = nameMachine;
            //int indextInList = MissionData.factoryDataMission.machinedatas[IDbutton - 1].index;
            MissionData.factoryDataMission.machinedatas[indextInList].currentNumber += 1;
            //print("currentNumber " + indextInList + " : " + MissionData.factoryDataMission.machinedatas[indextInList].currentNumber);
            coinsClone = (GameObject)Instantiate(coinsPrefabs, machineClone.transform.position, transform.rotation);
            coinsClone.GetComponent<CoinsFallController>().value = costMachine;
            coinsClone.transform.parent = machineClone.transform;

            FindAnDestroyChild("buttonView");
            FindAnDestroyChild("Lock");
            FindAnDestroyChild("Cicle");
            if (MissionData.factoryDataMission.machinedatas[indextInList].maxLevel > MissionData.factoryDataMission.machinedatas[indextInList].startLevel)
            {
                FactoryScenesController.ListMachineHaved.Add(MissionData.factoryDataMission.machinedatas[indextInList].iDMachine);
            }
            FactoryScenesController.IDCreatMachine.Add(FactoryScenesController.IDBackGroundButton); // add id vào list id đã có máy trên đó
            AddCommonObject(-costMachine, 0);
        }
        else
        {
            GameObject commonObject = GameObject.Find("CommonObject");
            if (commonObject != null)
            {
                commonObject.GetComponent<CommonObjectScript>().ChangeDolar(costMachine - CommonObjectScript.dollar);
            }
        }
    }

    void FindAnDestroyChild(string name)
    {
        Transform childObject = transform.parent.transform.parent.Find(name);
        if (childObject != null)
            Destroy(childObject.gameObject);
    }

    void CreatProduct()
    {
        audioControl.PlaySound("Chon sp");
        if (machineSelected.GetComponent<MachineController>().IDProductQueue.Count < ((machineSelected.GetComponent<MachineController>().levelMachine - 1) * 2 + 5))
        {

            if (!machineSelected.GetComponent<MachineController>().isFail)
            {
                if (FactoryScenesController.listProductInformation[IDbutton - 7].levelMachineUnlock <= machineSelected.GetComponent<MachineController>().levelMachine)
                {
                    if (CheckAllowCreatProduct())
                    {
                        machineSelected.GetComponent<MachineController>().IDProductQueue.Enqueue(FactoryScenesController.listProductInformation[IDbutton - 7]); // add sản phẩm vào hàng chờ đang sản xuất
                        machineSelected.GetComponent<MachineController>().listIndext.Add(indextInList);
                        CreateMaterialFall(IDbutton);
                    }
                    else
                    {
                        CreatNote(machineSelected.transform, new Vector3(1.2f, 1.2f, 1), new Vector3(0, 0, 0), FactoryScenesController.languageHungBV["LACKOFMATERIAL"]);
                    }
                }
                else
                {
                    CreatNote(machineSelected.transform, new Vector3(1.2f, 1.2f, 1), new Vector3(0, 0, 0), FactoryScenesController.languageHungBV["UNLOCKPRODUCT"] + FactoryScenesController.listProductInformation[IDbutton - 7].levelMachineUnlock);
                }
            }
            SetDataProductQueue(machineSelected.GetComponent<MachineController>().IDProductQueue);
        }
        else
        {
            // print("full queue");
            //foreach (ProductInfomation pr in machineSelected.GetComponent<MachineController>().IDProductQueue)
            //{
            //    print("Machine" + machineSelected.GetComponent<MachineController>().idMachinePosition + " : ProductID : " + pr.IDProduct);
            //}

            CreatNote(machineSelected.transform, new Vector3(1.2f, 1.2f, 1), new Vector3(0, 0, 0), FactoryScenesController.languageHungBV["FULLQUEU"]);
        }

    }

    void CreatNote(Transform parent, Vector3 localScale, Vector3 localPosition, string text)
    {
        if (fullQueueClone == null)
        {
            fullQueueClone = (GameObject)Instantiate(fullQueuePrefabs);
            fullQueueClone.transform.parent = parent;
            fullQueueClone.transform.localScale = localScale;
            fullQueueClone.transform.localPosition = localPosition;
            fullQueueClone.GetComponent<FullQueuController>().text = text;
        }
    }

    // kiểm tra xem có đủ nguyên liệu để sản xuất sản phẩm hay không
    bool CheckAllowCreatProduct()
    {
        List<int> listIDMaterialTemp = FactoryScenesController.listProductInformation[IDbutton - 7].listIDMaterial;
        if (listIDMaterialTemp[0] == 1) // có 1 loại nguyên liệu tạo sản phẩm
        {
            if (CommonObjectScript.arrayMaterials[listIDMaterialTemp[1] - 1] >= listIDMaterialTemp[2])
            {
                CommonObjectScript.arrayMaterials[listIDMaterialTemp[1] - 1] -= listIDMaterialTemp[2];
                isAllowCreatProduct = true;
            }
            else
            {
                buttonProductSelect.GetComponent<SpriteRenderer>().sprite = ButtonViewController.listSpriteBlackWhite[IDbutton - 7];
                isAllowCreatProduct = false;
            }
        }
        else if (listIDMaterialTemp[0] == 2) // có 2 loại nguyên liệu tạo sản phẩm
        {
            if (CommonObjectScript.arrayMaterials[listIDMaterialTemp[1] - 1] >= listIDMaterialTemp[2] &&
                CommonObjectScript.arrayMaterials[listIDMaterialTemp[3] - 1] >= listIDMaterialTemp[4])
            {
                CommonObjectScript.arrayMaterials[listIDMaterialTemp[1] - 1] -= listIDMaterialTemp[2];
                CommonObjectScript.arrayMaterials[listIDMaterialTemp[3] - 1] -= listIDMaterialTemp[4];
                isAllowCreatProduct = true;
            }
            else
            {
                buttonProductSelect.GetComponent<SpriteRenderer>().sprite = ButtonViewController.listSpriteBlackWhite[IDbutton - 7];
                isAllowCreatProduct = false;
            }
        }
        return isAllowCreatProduct;
    }
    void SetDataProductQueue(Queue<ProductInfomation> IDQueue)
    {
        GameObject productQueueView;
        productQueueView = GameObject.Find("ProductQueue");
        //productQueueView.GetComponent<ProductQueueController>().IDProductQueue.Clear();
        productQueueView.GetComponent<ProductQueueController>().CreatProductInQueue();
    }

    void CreatInforMaterialProduct(int IDProduct)
    {
        if (FactoryScenesController.isHelp && VariableSystem.mission == 1)
        {
            if (CreatAndControlPanelHelp.countClickHelpPanel == 11)
            {
                CreatAndControlPanelHelp.countClickHelpPanel = 12;
                DestroyObjecHelp("HandTapHelp");
            }
        }

        FindAndSetVisibleSell(false);
        materialPrefabs = (GameObject)Resources.Load("Factory/Material/Material0" + FactoryScenesController.listProductInformation[IDProduct - 7].listIDMaterial[0]);
        materialClone = (GameObject)Instantiate(materialPrefabs,
            new Vector3(machineSelected.transform.position.x, machineSelected.transform.position.y + 0.2f, machineSelected.transform.position.z)
            , transform.rotation);
        materialClone.name = "Material";
        if (materialClone != null)
            materialClone.transform.parent = machineSelected.transform;

        //PanelTextMaterialPrefabs = (GameObject)Resources.Load("Factory/Material/PanelTextMaterial0" + FactoryScenesController.listProductInformation[IDProduct - 7].listIDMaterial[0]);
        //PanelTextMaterialClone = (GameObject)Instantiate(PanelTextMaterialPrefabs, machineSelected.transform.position, transform.rotation);
        //PanelTextMaterialClone.name = "PanelTextMaterial";
        //GameObject uiRoot = GameObject.Find("UI Root");
        //PanelTextMaterialClone.transform.parent = uiRoot.transform;
        //PanelTextMaterialClone.transform.localScale = new Vector3(1, 1, 1);
    }
    void CreatInforMachine()
    {
        if (FactoryScenesController.isHelp && VariableSystem.mission == 1)
        {
            if (CreatAndControlPanelHelp.countClickHelpPanel == 4)
            {
                CreatAndControlPanelHelp.countClickHelpPanel = 5;
                DestroyObjecHelp("HandTapHelp");
            }
        }
        machineInforPrefabs = (GameObject)Resources.Load("Factory/Machine/MachineInfor");
        machineInforClone = (GameObject)Instantiate(machineInforPrefabs);
        machineInforClone.GetComponent<ViewInforMachineControler>().iDMachine = IDbutton - 1;
        machineInforClone.GetComponent<ViewInforMachineControler>().levelMachine = levelMachine;
        machineInforClone.transform.parent = this.transform.parent;
        machineInforClone.transform.localPosition = new Vector2(1.1f, 1.6f);
        machineInforClone.name = "MachineInfor";


    }
    void DestroyInfoMachine()
    {
        Transform machinrInforPre = this.transform.parent.Find("MachineInfor");
        if (machinrInforPre != null)
        {
            Destroy(machinrInforPre.gameObject);
        }
    }
    void CreateMaterialFall(int IDProduct)
    {
        materialFallPrefabs = (GameObject)Resources.Load("Factory/Material/MaterialFall0" + FactoryScenesController.listProductInformation[IDProduct - 7].listIDMaterial[0]);
        Instantiate(materialFallPrefabs, machineSelected.transform.position, transform.rotation);
    }
    void FindAndSetVisibleSell(bool isVisible)
    {
        Transform childObject = transform.parent.transform.Find("Sell");
        childObject.gameObject.SetActive(isVisible);
    }
    void DestroyInforMaterialProduct()
    {
        Transform materialPre = machineSelected.transform.Find("Material");
        if (materialPre != null)
        {
            Destroy(materialPre.gameObject);
        }
        if (VariableSystem.mission >= 4)
            FindAndSetVisibleSell(true);
    }

    void CreatObjectHelp(string nameObject, Vector3 vectorScale)
    {
        Transform objectPre = transform.Find(nameObject);
        if (objectPre == null)
        {
            GameObject objectPrefabs = (GameObject)Resources.Load("Help/" + nameObject);
            GameObject objectClone = (GameObject)Instantiate(objectPrefabs,
                new Vector3(transform.position.x, transform.position.y, objectPrefabs.transform.position.z),
               transform.rotation);
            objectClone.transform.parent = gameObject.transform;
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
        if (FactoryScenesController.isHelp)
        {
            if (VariableSystem.mission == 1)
            {
                if (CreatAndControlPanelHelp.countClickHelpPanel == 6 || CreatAndControlPanelHelp.countClickHelpPanel == 15)
                {
                    CreatObjectHelp("CircleHelp", new Vector3(.3f, .3f, .3f));
                    CreatObjectHelp("HandHelp", new Vector3(1f, 1f, 1f));
                }
                else if (CreatAndControlPanelHelp.countClickHelpPanel == 4 || CreatAndControlPanelHelp.countClickHelpPanel == 11)
                {
                    CreatObjectHelp("HandTapHelp", new Vector3(1f, 1f, 1f));
                }
            }
        }
    }

    void AddCommonObject(int dollar, int diamond)
    {
        GameObject commonObject = GameObject.Find("CommonObject");
        if (commonObject != null)
        {
            if (dollar < 0)
            {
                if (CommonObjectScript.dollar >= dollar)
                    commonObject.GetComponent<CommonObjectScript>().AddDollar(dollar);

                //  print("CommonObjectScript.dollar" + CommonObjectScript.dollar);
            }
            else
            {
                commonObject.GetComponent<CommonObjectScript>().AddDollar(dollar);
            }
            if (diamond < 0)
            {
                if (VariableSystem.diamond >= diamond)
                    commonObject.GetComponent<CommonObjectScript>().AddDiamond(diamond);
            }
            else
            {
                commonObject.GetComponent<CommonObjectScript>().AddDiamond(diamond);
            }
        }
    }
}
