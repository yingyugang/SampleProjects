using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PanelTextMaterial01Controller : MonoBehaviour {

	// Use this for initialization
    public UILabel nameProduct;
    public UILabel countProduct;
	void Start () {
        if (MachineController.machineSelect != null)
        {
            Vector3 pos = new Vector3(MachineController.machineSelect.transform.position.x / 3.6f, (MachineController.machineSelect.transform.position.y +1) / 3.6f, 0);
            gameObject.transform.position = pos;
        }
        SetNameProduct();
        SetValue();
	}
	
	// Update is called once per frame
	void Update () {
	}

    void SetValue()
    {
        if (ButtonInPopupController.buttonProductSelect != null)
        {
            int IDbuttonTemp = ButtonInPopupController.buttonProductSelect.GetComponent<ButtonInPopupController>().IDbutton - 7;
            List<int> listIDMaterialTemp = FactoryScenesController.listProductInformation[IDbuttonTemp].listIDMaterial;
            countProduct.text = listIDMaterialTemp[2].ToString() + "/" + CommonObjectScript.arrayMaterials[listIDMaterialTemp[1] - 1].ToString();
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
