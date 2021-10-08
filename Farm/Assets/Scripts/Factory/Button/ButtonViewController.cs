using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Common;

public class ButtonViewController : MonoBehaviour
{
    public static Animator animator;
    public GameObject[] listButtonView; // nơi lưu trữ vị trí các button mặc định của popup
    public int idMachineTypeForReadProduct; // để lấy tham số loại máy đang được chọn để có thể lấy ra các sản phẩm tương ứng, giá trị này được gán bên class MachineController
    //idMachineTypeForReadProduct = MachineController.idMachineType = ButtonInPopupController.(IDbutton - 1) = this. MissionData.factoryDataMission.machinedatas[countListMachineData].iDMachine;

    private Sprite[] listSprite;
    public static Sprite[] listSpriteBlackWhite;
    private int count;

    private GameObject countPrefabs;
    private GameObject countClone;

    public GameObject buttonSell;
    private bool isEnoughtMaterial;
    private int costMachine;
    void Awake()
    {
        if (FactoryScenesController.nameClick.Equals("BGClick"))
        {

            listSprite = Resources.LoadAll<Sprite>("Factory/Button/Images/Machine");
            buttonSell.SetActive(false);
        }
        else if (FactoryScenesController.nameClick.Equals("MachineClick"))
        {

            listSprite = Resources.LoadAll<Sprite>("Factory/Button/Images/Product");
            if (VariableSystem.mission < 4)
                buttonSell.SetActive(false);
            else
                buttonSell.SetActive(true);

        }
        listSpriteBlackWhite = Resources.LoadAll<Sprite>("Factory/Button/Images/ProductBlackWhite");
    }

    void Start()
    {
        animator = GetComponent<Animator>();
        if (FactoryScenesController.nameClick.Equals("BGClick"))
        {
            CreatButtonMachine();
        }
        else if (FactoryScenesController.nameClick.Equals("MachineClick"))
        {
            CreatButtonProduct(FactoryScenesController.ListProductAllowedOfMachine[idMachineTypeForReadProduct]);

        }
    }
    void Update()
    {
        if (!Application.loadedLevelName.Equals("Factory")) Destroy(gameObject);
    }
    void CreatButtonMachine() // 6 máy có 6 id khác nhau  
    {
        for (int countListMachineData = 0; countListMachineData < MissionData.factoryDataMission.machinedatas.Count; countListMachineData++)
        {

            listButtonView[countListMachineData].GetComponent<SpriteRenderer>().sprite = listSprite[(MissionData.factoryDataMission.machinedatas[countListMachineData].iDMachine - 1) * 4 + (FactoryScenesController.ListQueue[countListMachineData] - 1)];
            listButtonView[countListMachineData].AddComponent<BoxCollider2D>();
            //listButtonView[countListMachineData].GetComponent<BoxCollider2D>().size = new Vector2(1.24f, 0.55f);
            //listButtonView[countListMachineData].GetComponent<BoxCollider2D>().center = new Vector2(-0.36f, -0.09f);
            listButtonView[countListMachineData].GetComponent<BoxCollider2D>().size = new Vector2(1.24f, 0.85f);
            listButtonView[countListMachineData].GetComponent<BoxCollider2D>().offset = new Vector2(-0.32f, -0.02f);
            //listButtonView[countListMachineData].GetComponent<CircleCollider2D>().radius = 0.7f;
            //listButtonView[countListMachineData].GetComponent<CircleCollider2D>().center = new Vector2(-0.21f,0);

            int temp = MissionData.factoryDataMission.machinedatas[countListMachineData].iDMachine; // xác định ID
            listButtonView[countListMachineData].GetComponent<ButtonInPopupController>().IDbutton = temp;
            listButtonView[countListMachineData].GetComponent<ButtonInPopupController>().levelMachine = FactoryScenesController.ListQueue[countListMachineData];
            //print( "level: " + FactoryScenesController.ListQueue[countListMachineData]);
            costMachine = MissionPowerUp.PriceDrop ? ((int)(FactoryScenesController.listMachineInfomation[temp - 1].machineCost * 0.75f)) : FactoryScenesController.listMachineInfomation[temp - 1].machineCost;
            listButtonView[countListMachineData].GetComponent<ButtonInPopupController>().costMachine = costMachine;
            listButtonView[countListMachineData].GetComponent<ButtonInPopupController>().nameMachine = "Vietnamese".Equals(VariableSystem.language) ? FactoryScenesController.listMachineInfomation[temp - 1].machineNameVie : FactoryScenesController.listMachineInfomation[temp - 1].machineNameEng;
            listButtonView[countListMachineData].GetComponent<ButtonInPopupController>().indextInList = MissionData.factoryDataMission.machinedatas[countListMachineData].index;
            //  print("  listButtonView[countListMachineData].GetComponent<ButtonInPopupController>().costMachine " + listButtonView[countListMachineData].GetComponent<ButtonInPopupController>().costMachine);
            CreatCountLabel(costMachine, listButtonView[countListMachineData].transform);
        }
    }
    void CreatCountLabel(int costMachine, Transform transformParen)
    {
        countPrefabs = (GameObject)Resources.Load("Factory/Button/Prefabs/Count");
        countClone = (GameObject)Instantiate(countPrefabs);
        countClone.GetComponent<CountController>().costMachine = costMachine;
        countClone.transform.parent = transformParen;
        countClone.transform.localPosition = new Vector3(-0.1f, -0.5f,0);
        countClone.name = countPrefabs.name;

    }
    void CreatButtonProduct(List<ProductForMachine> IDView) //16 sản phẩm id các sản phẩm bắt đầu từ 7 từ 1->6 là các id của máy
    {
        //print(IDView[2].iDType);
        for (int id = IDView.Count - 1; id >= 0; id--) // sinh các button tương ứng với các mission và gán id cho các button
        {
          //  print(IDView[id].iDType);

            if (IDView[id].levelMachineUnlock > MachineController.machineSelect.GetComponent<MachineController>().levelMachine || !CheckEnoughtCreatProduct(IDView[id].iDType))
            {
                listButtonView[count].GetComponent<SpriteRenderer>().sprite = listSpriteBlackWhite[IDView[id].iDType - 7];
            }
            else
            {
                listButtonView[count].GetComponent<SpriteRenderer>().sprite = listSprite[IDView[id].iDType - 7];
            }
            listButtonView[count].AddComponent<CircleCollider2D>();
            listButtonView[count].GetComponent<CircleCollider2D>().radius = 0.65f;
            listButtonView[count].GetComponent<ButtonInPopupController>().IDbutton = IDView[id].iDType;
            listButtonView[count].GetComponent<ButtonInPopupController>().indextInList = IDView[id].indextInList;
            // print("product " + IDView[id].iDType + " have index " + IDView[id].indextInList);
            count++;
        }
    }
    bool CheckEnoughtCreatProduct(int IDbutton)
    {
        List<int> listIDMaterialTemp = FactoryScenesController.listProductInformation[IDbutton - 7].listIDMaterial;
        if (listIDMaterialTemp[0] == 1) // có 1 loại nguyên liệu tạo sản phẩm
        {
            if (CommonObjectScript.arrayMaterials[listIDMaterialTemp[1] - 1] >= listIDMaterialTemp[2])
            {
                isEnoughtMaterial = true;
            }
            else
            {
                isEnoughtMaterial = false;
            }
        }
        else if (listIDMaterialTemp[0] == 2) // có 2 loại nguyên liệu tạo sản phẩm
        {
            if (CommonObjectScript.arrayMaterials[listIDMaterialTemp[1] - 1] >= listIDMaterialTemp[2] &&
                CommonObjectScript.arrayMaterials[listIDMaterialTemp[3] - 1] >= listIDMaterialTemp[4])
            {
                isEnoughtMaterial = true;
            }
            else
            {
                isEnoughtMaterial = false;
            }
        }
        return isEnoughtMaterial;
    }
    void CollapThisObject()
    {
        FindChildAndDestroy("Cicle");
        FindChildAndDestroy("buttonView");
        FindChildAndDestroy("ProductQueue");

        GameObject factoryTextPre = GameObject.Find("FactoryText");
        if (factoryTextPre != null)
        {
            Destroy(factoryTextPre);
        }
    }
    void FindChildAndDestroy(string name)
    {
        Transform Child = transform.parent.Find(name);
        if (Child != null)
            Destroy(Child.gameObject);
    }

    void DestroyCountObject()
    {
        GameObject[] CountPre = GameObject.FindGameObjectsWithTag("Count");
        foreach (GameObject gameObject in CountPre)
            if (gameObject != null)
            {
                // Destroy(gameObject);
            }
    }

}
