using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PanelTextMaterial02Controller : MonoBehaviour {

    public UILabel nameProduct;
    public UILabel countProduct01;
    public UILabel countProduct02;
	void Start ()
    {
        if (MachineController.machineSelect != null)
        {
            Vector3 pos = new Vector3(MachineController.machineSelect.transform.position.x / 3.6f, (MachineController.machineSelect.transform.position.y + 1 ) / 3.6f, 0);
            gameObject.transform.position = pos;
        }
        SetNameProduct();
        SetValue();
	}
	
    void SetValue()
    {
        if (ButtonInPopupController.buttonProductSelect != null)
        {
            int IDbuttonTemp = ButtonInPopupController.buttonProductSelect.GetComponent<ButtonInPopupController>().IDbutton - 7;
            List<int> listIDMaterialTemp = FactoryScenesController.listProductInformation[IDbuttonTemp].listIDMaterial;
            countProduct01.text = listIDMaterialTemp[2].ToString() + "/" + CommonObjectScript.arrayMaterials[listIDMaterialTemp[1] - 1].ToString();
            countProduct02.text = listIDMaterialTemp[4].ToString() + "/" + CommonObjectScript.arrayMaterials[listIDMaterialTemp[3] - 1].ToString(); ;
        }
    }
    void SetNameProduct()
    {
        if (ButtonInPopupController.buttonProductSelect != null)
        {
            int IDbuttonTemp = ButtonInPopupController.buttonProductSelect.GetComponent<ButtonInPopupController>().IDbutton - 7;
            nameProduct.text = FactoryScenesController.listProductInformation[IDbuttonTemp].productionName;
        }
    }
}
