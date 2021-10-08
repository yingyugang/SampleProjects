using UnityEngine;
using System.Collections;

public class ButtonSellMachineController : MonoBehaviour
{
    private GameObject UIRoot;
    void Start()
    {
        ControlViewHelp();
    }

    void OnMouseDown()
    {
        if (!FactoryScenesController.isHelp)
        {
            ControlDownButton();
        }
        else
        {
            if (VariableSystem.mission == 4 && CreatAndControlPanelHelp.countClickHelpPanel == 2)
            {
                DestroyObjecHelp("CircleHelp");
                DestroyObjecHelp("HandHelp");
                CreatAndControlPanelHelp.countClickHelpPanel = 3;
                ControlDownButton();
            }
        }
    }
    void ControlDownButton()
    {
        this.gameObject.transform.localScale = new Vector3(-2.2f, 2.2f, 2.2f);
        UIRoot = GameObject.Find("UI Root");
    }
    void OnMouseUp()
    {
        if (!FactoryScenesController.isHelp)
        {
            ControlUpButton();
        }
        else
        {
            if (VariableSystem.mission == 4 && CreatAndControlPanelHelp.countClickHelpPanel == 3)
            {
                ControlUpButton();
            }
        }
    }
    void ControlUpButton()
    {
        this.gameObject.transform.localScale = new Vector3(-2f, 2f, 2f);

        if (!MachineController.machineSelect.GetComponent<MachineController>().isFail)
        {
            Transform panelComfirm = UIRoot.transform.Find("PanelComfirm");
            panelComfirm.gameObject.SetActive(true);

            if (ButtonViewController.animator != null)
                ButtonViewController.animator.SetTrigger("Collape");
            if (ProductQueueController.animator != null)
                ProductQueueController.animator.SetTrigger("Collape");

            //CameraController.isViewPopup = true;
        }
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
            if (VariableSystem.mission == 4 && CreatAndControlPanelHelp.countClickHelpPanel == 2)
            {
                CreatObjectHelp("CircleHelp", new Vector3(.3f, .3f, .3f));
                CreatObjectHelp("HandHelp", new Vector3(-1f, 1f, 1f));
            }
        }
    }
}
