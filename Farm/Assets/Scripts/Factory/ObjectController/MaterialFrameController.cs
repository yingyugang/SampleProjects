using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using BaPK;

public class MaterialFrameController : MonoBehaviour
{

    // Use this for initialization
    public GameObject[] MaterialIcon;
    public Label[] laBelMaterial;
    private static Sprite[] ListMaterialSprite;
    private List<int> listIDMaterialTemp;
    private string nameProduct;
    private string countProduct01;
    private string countProduct02;
    void Start()
    {
        if (ListMaterialSprite == null)
            ListMaterialSprite = Resources.LoadAll<Sprite>("Factory/Button/Images/Material");

        laBelMaterial[0].GetComponent<New1FontRead>().New1Read("ButtonBG2", 1, TextAlignment.Center, SetNameProduct(), 0f, 10f);
        //laBelMaterial[0].GetComponent<New1FontRead>().New1Read("ButtonBG2", 1, TextAlignment.Center, "cá nướng", 0f, 10f);
        //laBelMaterial[1].GetComponent<HelpFontRead>().HelpRead("ButtonBG2", 1, TextAlignment.Left, "1/", 0f, 0f);
        //laBelMaterial[1].setColor(new Color32(73, 38, 0, 255));
        //laBelMaterial[2].GetComponent<HelpFontRead>().HelpRead("ButtonBG2", 1, TextAlignment.Left, "0", 0f, 0f);
        //laBelMaterial[2].setColor(new Color(255f, 0, 0));
        if (laBelMaterial.Length < 5)
            SetValue01();
        else
        {
            SetValue02();
        }

        if (ButtonInPopupController.buttonProductSelect != null)
        {
            int IDbuttonTemp = ButtonInPopupController.buttonProductSelect.GetComponent<ButtonInPopupController>().IDbutton - 7;
            listIDMaterialTemp = FactoryScenesController.listProductInformation[IDbuttonTemp].listIDMaterial;
        }
        ViewMaterialIcon();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void ViewMaterialIcon()
    {
        for (int i = 0; i < MaterialIcon.Length; i++)
        {
            MaterialIcon[i].GetComponent<SpriteRenderer>().sprite = ListMaterialSprite[listIDMaterialTemp[i * 2 + 1] - 1];
        }
    }

    string SetNameProduct()
    {
        if (ButtonInPopupController.buttonProductSelect != null)
        {
            int IDbuttonTemp = ButtonInPopupController.buttonProductSelect.GetComponent<ButtonInPopupController>().IDbutton - 7;
            nameProduct = FactoryScenesController.listProductInformation[IDbuttonTemp].productionName;
        }
        return FactoryScenesController.languageHungBV[nameProduct];
    }

    void SetValue01()
    {
        if (ButtonInPopupController.buttonProductSelect != null)
        {
            int IDbuttonTemp = ButtonInPopupController.buttonProductSelect.GetComponent<ButtonInPopupController>().IDbutton - 7;
            List<int> listIDMaterialTemp = FactoryScenesController.listProductInformation[IDbuttonTemp].listIDMaterial;
            // laBelMaterial[1].GetComponent<HelpFontRead>().HelpRead("ButtonBG2", 1, TextAlignment.Left, (listIDMaterialTemp[2] + "/"), 0f, 0f);
            laBelMaterial[1].GetComponent<New3FontRead>().New3Read("ButtonBG2", 1, TextAlignment.Left, (listIDMaterialTemp[2] + "/"), 0f, 0f);
            laBelMaterial[2].GetComponent<New3FontRead>().New3Read("ButtonBG2", 1, TextAlignment.Left, (CommonObjectScript.arrayMaterials[listIDMaterialTemp[1] - 1].ToString()), 0f, 0f);
            if (CommonObjectScript.arrayMaterials[listIDMaterialTemp[1] - 1] < listIDMaterialTemp[2])
                laBelMaterial[2].setColor(new Color(255f, 0, 0));

        }
    }

    void SetValue02()
    {
        if (ButtonInPopupController.buttonProductSelect != null)
        {
            int IDbuttonTemp = ButtonInPopupController.buttonProductSelect.GetComponent<ButtonInPopupController>().IDbutton - 7;
            List<int> listIDMaterialTemp = FactoryScenesController.listProductInformation[IDbuttonTemp].listIDMaterial;
            // countProduct01 = listIDMaterialTemp[2].ToString() + "/" + CommonObjectScript.arrayMaterials[listIDMaterialTemp[1] - 1].ToString();
            laBelMaterial[1].GetComponent<New3FontRead>().New3Read("ButtonBG2", 1, TextAlignment.Left, listIDMaterialTemp[2] + "/", 0f, 0f);
            laBelMaterial[2].GetComponent<New3FontRead>().New3Read("ButtonBG2", 1, TextAlignment.Left, CommonObjectScript.arrayMaterials[listIDMaterialTemp[1] - 1].ToString(), 0f, 0f);
            if (CommonObjectScript.arrayMaterials[listIDMaterialTemp[1] - 1] < listIDMaterialTemp[2])
                laBelMaterial[2].setColor(new Color(255f, 0, 0));

            // countProduct02 = listIDMaterialTemp[4].ToString() + "/" + CommonObjectScript.arrayMaterials[listIDMaterialTemp[3] - 1].ToString();
            laBelMaterial[3].GetComponent<New3FontRead>().New3Read("ButtonBG2", 1, TextAlignment.Left, listIDMaterialTemp[4] + "/", 0f, 0f);
            laBelMaterial[4].GetComponent<New3FontRead>().New3Read("ButtonBG2", 1, TextAlignment.Left, CommonObjectScript.arrayMaterials[listIDMaterialTemp[3] - 1].ToString(), 0f, 0f);
            if (CommonObjectScript.arrayMaterials[listIDMaterialTemp[3] - 1] < listIDMaterialTemp[4])
                laBelMaterial[4].setColor(new Color(255f, 0, 0));
        }
    }
}
