using UnityEngine;
using System.Collections;
using BaPK;

public class ButtonPayController : MonoBehaviour
{

    // Use this for initialization
    public GameObject iconButon;
    public Label label;
    public GameObject objectController;

    private GameObject machineSelected;
    private GameObject dinamondPrefabs;
    public static bool isEfect;
    private bool isClick;
    AudioControl audioControl;
    void Start()
    {
        audioControl = GameObject.Find("AudioControl").GetComponent<AudioControl>();
        machineSelected = MachineController.machineSelect;
        label.GetComponent<New3FontRead>().New3Read("ButtonBG2", 1, TextAlignment.Center, "1", 0f, 0f);

    }

    // Update is called once per frame
    void Update()
    {
        if (machineSelected != null)
        {
            if (machineSelected.GetComponent<MachineController>().IDProductQueue.Count > 0)
            {
                objectController.SetActive(true);
                this.gameObject.GetComponent<BoxCollider2D>().enabled = true;
            }
            else
            {
                objectController.SetActive(false);
                this.gameObject.GetComponent<BoxCollider2D>().enabled = false;
            }
        }
        if (isEfect)
        {
            if (FactoryScenesController.isHelp && VariableSystem.mission == 1)
            {
                if (CreatAndControlPanelHelp.countClickHelpPanel == 18)
                {
                    CreatAndControlPanelHelp.countClickHelpPanel = 19;
                    DestroyObjecHelp("CircleHelp");
                    DestroyObjecHelp("HandHelp");
                }
            }
            machineSelected.GetComponent<MachineController>().timeCount = machineSelected.GetComponent<MachineController>().timeProduct;
            isEfect = false;
            isClick = false;
        }
        ControlViewHelp();
    }

    void OnMouseDown()
    {
       // audioControl.PlaySoundInstance("Click 1", false, false, 0.5f);
        audioControl.PlaySound("Click 1");
        if (!isClick)
        {
            iconButon.transform.localScale = new Vector3(1.1f, 1.1f, 1f);
            dinamondPrefabs = (GameObject)Resources.Load("Factory/Queue/Diamond");
        }
    }

    void OnMouseUp()
    {
        if (!isClick)
        {
            if (VariableSystem.diamond >= 1)
            {
                if (machineSelected != null)
                {
                    Instantiate(dinamondPrefabs, new Vector3(machineSelected.transform.position.x - 0.1f, machineSelected.transform.position.y + 0.5f, machineSelected.transform.position.z), machineSelected.transform.rotation);
                    iconButon.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                    isClick = true;
                    AddCommonObject(0, -1);
                }
            }
            else
            {
                DialogInapp.ShowInapp();
            }
        }
    }

    void CreatObjectHelp(string nameObject, Vector3 vectorScale)
    {
        Transform objectPre = iconButon.transform.Find(nameObject);
        if (objectPre == null)
        {
            GameObject objectPrefabs = (GameObject)Resources.Load("Help/" + nameObject);
            GameObject objectClone = (GameObject)Instantiate(objectPrefabs,
                new Vector3(iconButon.transform.position.x, iconButon.transform.position.y, objectPrefabs.transform.position.z),
               transform.rotation);
            objectClone.transform.parent = iconButon.transform;
            objectClone.transform.localScale = vectorScale;
            objectClone.name = objectPrefabs.name;
        }
    }
    void DestroyObjecHelp(string nameObject)
    {
        Transform objectClonePre = iconButon.transform.Find(nameObject);
        if (objectClonePre != null)
        {
            Destroy(objectClonePre.gameObject);
        }
    }
    void ControlViewHelp()
    {
        if (FactoryScenesController.isHelp && VariableSystem.mission == 1)
        {
            if (CreatAndControlPanelHelp.countClickHelpPanel == 18 )
            {
                CreatObjectHelp("CircleHelp", new Vector3(.3f, .3f, .3f));
                CreatObjectHelp("HandHelp", new Vector3(1f, 1f, 1f));
            }
        }
    }

    void AddCommonObject(int dollar, int diamond)
    {
        GameObject commonObject = GameObject.Find("CommonObject");
        if (commonObject != null)
        {
            commonObject.GetComponent<CommonObjectScript>().AddDollar(dollar);
            commonObject.GetComponent<CommonObjectScript>().AddDiamond(diamond);

        }
    }

}
