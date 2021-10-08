//Đang dùng cho cái rơi biểu tượng tài nguyên khi sản xuất (MaterialFall)
using UnityEngine;
using System.Collections;
using BaPK;
using System.Collections.Generic;

public class FontTextMaterialFall : MonoBehaviour {

  
    private string countMaterial;

    public Label[] labelCountMaterial;

    private static Sprite[] ListMaterialSprite;
    public GameObject[] MaterialIcon;

    private List<int> listIDMaterialTemp;
	void Start () {
        if (ListMaterialSprite == null)
        ListMaterialSprite = Resources.LoadAll<Sprite>("Factory/Button/Images/Material");

        if (ButtonInPopupController.buttonProductSelect != null)
        {
            int IDbuttonTemp = ButtonInPopupController.buttonProductSelect.GetComponent<ButtonInPopupController>().IDbutton - 7;
            listIDMaterialTemp = FactoryScenesController.listProductInformation[IDbuttonTemp].listIDMaterial;
        }

        CreatMaterialFall();
        ViewMaterialIcon();
    }
	
    void CreatMaterialFall()
    {
        for (int i = 0; i < labelCountMaterial.Length; i++)
        {
            string subMaterial = "-" + listIDMaterialTemp[i * 2 + 2];
            labelCountMaterial[i].GetComponent<New3FontRead>().New3Read("ButtonBG2", 1, TextAlignment.Left, subMaterial, 0f, 8f);
           // labelCountMaterial[i].setColor(new Color32(249, 251,108,255));
            //countMaterial = "-" + listIDMaterialTemp[i*2 + 2];
            //bitmapFont = new BitmapFont(textureFont, lines);
            //labelCountMaterial[i].setSortingLayer("ButtonBG2");
            //labelCountMaterial[i].setSortingOrderInLayer(2);
            //labelCountMaterial[i].setAlignment(TextAlignment.Left);
            //labelCountMaterial[i].createLabel(bitmapFont, countMaterial, 0, 8);
        }
    }
    void ViewMaterialIcon()
    {
        for (int i = 0; i < MaterialIcon.Length; i++)
        {
            MaterialIcon[i].GetComponent<SpriteRenderer>().sprite = ListMaterialSprite[listIDMaterialTemp[i * 2 + 1] - 1];
        }
    }
    void Destroy()
    {
        Destroy(gameObject);
    }
}