using UnityEngine;
using System.Collections;
using BaPK;
using System.Collections.Generic;

public class ViewInforMachineControler : MonoBehaviour
{

    // Use this for initialization
    public static List<List<ProductInfomation>> ListAllProductOfMachine;
    public Label lbnameMachine;
    public Label lblevelMachine;
    public GameObject[] arrayProduct;
    public int iDMachine;
    public int levelMachine;
    private string name;
    private string level;
    private static Sprite[] listSprite;
    private static Sprite[] listSpriteBlackWhite;
    private int count;
    void Start()
    {
        count = 0;
        // if (listSprite != null)
        listSprite = Resources.LoadAll<Sprite>("Factory/Button/Images/Product");
        //if (listSpriteBlackWhite != null)
        listSpriteBlackWhite = Resources.LoadAll<Sprite>("Factory/Button/Images/ProductBlackWhite");

        ListAllProductOfMachine = new List<List<ProductInfomation>>();
        ListAllProductOfMachine.Clear();


        name = "Vietnamese".Equals(VariableSystem.language) ? FactoryScenesController.listMachineInfomation[iDMachine].machineNameVie : FactoryScenesController.listMachineInfomation[iDMachine].machineNameEng;
        level = FactoryScenesController.languageHungBV["Level"] + levelMachine.ToString();
        lbnameMachine.GetComponent<New1FontRead>().New1Read("12", 1, TextAlignment.Center, name, 0f, 10f);
        lblevelMachine.GetComponent<New2FontRead>().New2Read("12", 1, TextAlignment.Center, level, 0f, 10f);
        lblevelMachine.setColor(new Color32(71, 56, 34, 255));
        CreatProductForMachine(iDMachine);
        ChangePositionProduct(FactoryScenesController.listMachineInfomation[iDMachine].listProductOfMachine.Count);
    }

    void CreatProductForMachine(int idMachine)
    {
        for (int countProduct = 0; countProduct < FactoryScenesController.listMachineInfomation[idMachine].listProductOfMachine.Count; countProduct++)
        {
            arrayProduct[count].SetActive(true);
            int idProducOfMachine = FactoryScenesController.listMachineInfomation[idMachine].listProductOfMachine[countProduct] - 7;
            if (FactoryScenesController.listProductInformation[idProducOfMachine].levelMachineUnlock > levelMachine)
            {
                arrayProduct[count].GetComponent<SpriteRenderer>().sprite = listSpriteBlackWhite[idProducOfMachine];
            }
            else
            {
                arrayProduct[count].GetComponent<SpriteRenderer>().sprite = listSprite[idProducOfMachine];
            }
            if (count < arrayProduct.Length)
                count++;
        }
    }
    void ChangePositionProduct(int countProduct)
    {
        if (countProduct == 3)
        {
            arrayProduct[2].transform.localPosition = new Vector2(-0.02f, -0.62f);
        }
        else if (countProduct == 4)
        {
            arrayProduct[2].transform.localPosition = new Vector2(-0.41f, -0.62f);
            arrayProduct[3].transform.localPosition = new Vector2(0.52f, -0.62f);
        }
        else if (countProduct == 5)
        {
            arrayProduct[2].transform.localPosition = new Vector2(-0.83f, -0.62f);
            arrayProduct[3].transform.localPosition = new Vector2(-0.03f, -0.62f);
            arrayProduct[4].transform.localPosition = new Vector2(0.83f, -0.62f);
        }
    }
}
