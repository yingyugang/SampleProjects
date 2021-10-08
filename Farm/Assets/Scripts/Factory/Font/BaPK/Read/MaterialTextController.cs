
// cái này đang thừa nha
using UnityEngine;
using System.Collections;
using BaPK;
using System.Collections.Generic;

public class MaterialTextController : MonoBehaviour {
    
	// Use this for initialization
    public Label labelNameProduct;
    public Label [] labelCountMaterial;
   // public Label labelAllMaterial;
    public Texture2D textureFont;
    private BitmapFont bitmapFont;

    public string nameProduct;
    private string countMaterial;
    private string countMaterial2;

    public static GameObject buttonSelect;
    public int type;
	void Start () {
       
        #region lines for font
        string[] lines = new string[]{
	"94 58 21 29 31 17 0",
	"177 0 15 28 32 11 1",
	"51 0 21 28 32 17 2",
	"116 0 20 28 32 16 3",
	"134 177 20 28 32 16 4",
	"156 147 19 27 32 15 5",
	"113 179 20 27 32 16 6",
	"175 175 19 28 32 15 7",
	"0 120 23 29 31 19 8",
	"0 179 22 27 32 18 9",
	"73 0 21 28 32 17 A",
	"95 0 20 28 32 16 B",
	"44 179 20 27 32 16 C",
	"135 146 20 28 32 16 D",
	"156 59 19 29 31 15 E",
	"176 58 18 29 31 14 F",
	"72 61 21 28 32 17 G",
	"92 121 21 28 32 17 H",
	"193 0 13 28 32 9 I",
	"136 87 19 28 32 15 J",
	"94 29 21 28 32 17 K",
	"177 29 15 28 32 11 L",
	"0 0 27 29 32 23 M",
	"24 149 22 29 32 18 N",
	"47 149 22 29 31 18 O",
	"116 29 20 28 32 16 P",
	"72 29 21 31 31 17 Q",
	"115 88 20 28 32 16 R",
	"157 0 19 28 32 15 S",
	"176 88 18 28 32 14 T",
	"93 90 21 28 32 17 U",
	"157 29 19 28 32 15 V",
	"0 60 26 29 31 22 W",
	"24 120 23 28 32 19 X",
	"23 179 20 27 32 16 Y",
	"48 118 22 28 32 18 Z",
	"71 92 21 28 32 17 a",
	"92 150 21 28 32 17 b",
	"92 179 20 27 32 16 c",
	"114 148 20 28 32 16 d",
	"155 175 19 29 31 15 e",
	"137 29 19 29 31 15 f",
	"27 60 22 28 32 18 g",
	"70 176 21 28 32 17 h",
	"193 29 13 28 32 9 i",
	"156 89 19 28 32 15 j",
	"70 147 21 28 32 17 k",
	"176 146 16 28 32 12 l",
	"0 30 27 29 32 23 m",
	"50 62 21 29 32 17 n",
	"50 32 21 29 31 17 o",
	"114 119 20 28 32 16 p",
	"28 0 22 31 31 18 q",
	"116 58 20 28 32 16 r",
	"156 118 19 28 32 15 s",
	"176 117 17 28 32 13 t",
	"135 117 20 28 32 16 u",
	"137 0 19 28 32 15 v",
	"0 90 26 29 31 22 w",
	"0 150 23 28 32 19 x",
	"28 32 20 27 32 16 y",
	"27 89 22 28 32 18 z"
};
        #endregion
        //SetName();
        //SetValue();
        //bitmapFont = new BitmapFont(textureFont, lines);

        //SetLabel(labelNameProduct, "ButtonBG1",2,TextAlignment.Center,nameProduct);
        //if (type == 1)
        //{
        //    SetLabel(labelCountMaterial[0], "ButtonBG1", 2, TextAlignment.Left, countMaterial);
        //}
        //else if (type == 2)
        //{
        //    SetLabel(labelCountMaterial[0], "ButtonBG1", 2, TextAlignment.Left, countMaterial);
        //    SetLabel(labelCountMaterial[1], "ButtonBG1", 2, TextAlignment.Left, countMaterial2);
        //}
	}
	
	// Update is called once per frame
	void Update () {
        labelNameProduct.setText(nameProduct);
        labelNameProduct.refresh();

        if (type == 1)
        {
            //labelCountMaterial[0].setText(countMaterial);
            //labelCountMaterial[0].refresh();
        }
        else if (type == 2)
        {
            //labelCountMaterial[0].setText(countMaterial);
            //labelCountMaterial[0].refresh();

            //labelCountMaterial[1].setText(countMaterial);
            //labelCountMaterial[1].refresh();
        }
	}

    void SetValue()
    {
        if (buttonSelect != null)
        {
            int IDbuttonTemp = buttonSelect.GetComponent<ButtonInPopupController>().IDbutton - 7;
            List<int> listIDMaterialTemp = FactoryScenesController.listProductInformation[IDbuttonTemp].listIDMaterial;
            type =listIDMaterialTemp[0];
            if (type.Equals(1))
            {
               countMaterial = listIDMaterialTemp[2].ToString() + " vs 100";
                countMaterial2 = "";
            }
            else if (type.Equals(2))
            {
                 countMaterial = listIDMaterialTemp[2].ToString() + " vs 100";
                 countMaterial2 = listIDMaterialTemp[4].ToString() + " vs 100";
            }
        }
    }

    void SetName()
    {
        if (buttonSelect != null)
        {
            int IDbuttonTemp = buttonSelect.GetComponent<ButtonInPopupController>().IDbutton - 7;
            nameProduct = FactoryScenesController.listProductInformation[IDbuttonTemp].productionName;
        }
    }

    void SetLabel(Label label, string nameSorttingLayer, int sorttingOrderInLayer, TextAlignment Alignment, string value)
    {
        label.setSortingLayer(nameSorttingLayer);
        label.setSortingOrderInLayer(sorttingOrderInLayer);
        label.setAlignment(Alignment);
        label.createLabel(bitmapFont, value, 0, 8);
    }
}
