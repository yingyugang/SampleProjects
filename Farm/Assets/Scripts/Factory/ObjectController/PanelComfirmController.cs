using UnityEngine;
using System.Collections;

public class PanelComfirmController : MonoBehaviour
{

    // Use this for initialization
    private Animator animator;
    private GameObject ButtonBG;
    private Transform buttonOk;
    private GameObject iconUnLockPrefabs;
    private GameObject iconUnLockClone;
    public UILabel[] labelComfirm;
    public UILabel labelCoin;
    AudioControl audioControl;
    private Vector3 localPositionCoin;
    void OnEnable()
    {
        CommonObjectScript.isViewPoppup = true;
        labelComfirm[3].text = FactoryScenesController.languageHungBV["COMFIRMSELLMACHINE"];
        labelCoin.text = ((int)(MachineController.machineSelect.GetComponent<MachineController>().costMachine * 0.3)).ToString();
    }
    void Start()
    {
        audioControl = GameObject.Find("AudioControl").GetComponent<AudioControl>();
        labelComfirm[0].text = FactoryScenesController.languageHungBV["SELLMACHINE"];
        labelComfirm[1].text = FactoryScenesController.languageHungBV["AGREE"];
        labelComfirm[2].text = FactoryScenesController.languageHungBV["CANCEL"];
        animator = GetComponent<Animator>();

        buttonOk = transform.Find("BtOK");
        ControlViewHelp();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || CommonObjectScript.isEndGame || CommonObjectScript.isViewComplete)
        {
            if (!FactoryScenesController.isHelp)
                CancelButton_Click();
        }
    }

    void OnApplicationPause(bool pauseStatus)
    {
        if (!pauseStatus)
        {
            if (!FactoryScenesController.isHelp)
                CancelButton_Click();
        }
    }
    public void OKButton_Click()
    {
        audioControl.PlaySound("Click 1");
        if (!FactoryScenesController.isHelp)
            ControlOKClick();
        else
        {
            if (VariableSystem.mission == 4 && CreatAndControlPanelHelp.countClickHelpPanel == 3)
            {
                ControlOKClick();
                DestroyObjecHelp("CircleHelp");
                DestroyObjecHelp("HandHelp");
                FactoryScenesController.isHelp = false;
                EndHelp();
                //kết thúc hướng dẫn bán máy
            }
        }
     
    }
    void EndHelp()
    {
        CommonObjectScript.CompleteGuide();
        GameObject.Find("CommonObject").GetComponent<CommonObjectScript>().setActiveButton(true, true, true, true, true, true, true, true);
    }
    void ControlOKClick()
    {
       
        iconUnLockPrefabs = (GameObject)Resources.Load("Factory/Button/Prefabs/UnLock");
        ButtonBG = GameObject.Find("ButtonBG");


        CreateAnimationAddValue(((int)(MachineController.machineSelect.GetComponent<MachineController>().costMachine * 0.3f)).ToString());
        try
        {
            FactoryScenesController.ListMachineHaved.Remove(MachineController.machineSelect.GetComponent<MachineController>().idMachineType + 1);
        }
        catch
        {
        }
        FactoryScenesController.IDCreatMachine.Remove(MachineController.machineSelect.GetComponent<MachineController>().idMachinePosition);
        CreatIconUnLockAgain(ButtonBG.transform.Find(MachineController.machineSelect.GetComponent<MachineController>().idMachinePosition.ToString()));
        AddCommonObject((int)(MachineController.machineSelect.GetComponent<MachineController>().costMachine * 0.3), 0);
        Destroy(MachineController.machineSelect);
        animator.Play("Invisible");

    }
    public void CancelButton_Click()
    {
        audioControl.PlaySound("Click 1");
        if (!FactoryScenesController.isHelp)
            animator.Play("Invisible");
    }

    void DestroyGameObj()
    {
        gameObject.SetActive(false);
        CommonObjectScript.isViewPoppup = false;
    }

    void CreatIconUnLockAgain(Transform ts)
    {
        iconUnLockClone = (GameObject)Instantiate(iconUnLockPrefabs,
          new Vector3(ts.position.x, ts.position.y, iconUnLockPrefabs.transform.position.z),
             ts.rotation);
        iconUnLockClone.name = "Lock";
        iconUnLockClone.transform.parent = ts;
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

    private void CreateAnimationHarvest(int number)// create animation when user harvest one product
    {
        GameObject HarvestPlantPrefabs = (GameObject)Resources.Load("Farm/HarvestPlant");
        for (int i = 0; i < number; i++)
        {
            GameObject HarvestPlant = (GameObject)Instantiate(HarvestPlantPrefabs, Vector3.zero, Quaternion.identity);
            HarvestPlant.GetComponent<HarvestPlantScript>().setValue("Common/vang", new Vector3(240, 330, 0), 50);
            HarvestPlant.transform.parent = this.transform.parent;
            HarvestPlant.GetComponent<Transform>().localPosition = ChangeLocalPosition(MachineController.machineSelect.GetComponent<MachineController>().idMachinePosition) + new Vector3(Random.Range(-40, 40), Random.Range(-20, 20), 0);
            HarvestPlant.GetComponent<Transform>().localScale = Vector3.one;
        }
    }
    private void CreateAnimationAddValue(string number)// create animation when add money or item
    {
        GameObject valuePrefabs = (GameObject)Resources.Load("Farm/AddValue");
        GameObject valueAdd = (GameObject)Instantiate(valuePrefabs, Vector3.zero, Quaternion.identity);
        valueAdd.name = "coin";
        valueAdd.transform.parent = this.transform.parent;
        valueAdd.GetComponent<Transform>().localPosition = ChangeLocalPosition(MachineController.machineSelect.GetComponent<MachineController>().idMachinePosition);
        valueAdd.GetComponent<Transform>().localScale = Vector3.one;
        valueAdd.GetComponent<AddValueScript>().setValue(number);
        CreateAnimationHarvest(4);
    }

    private Vector3 ChangeLocalPosition(int IDMachine)
    {
        switch (IDMachine)
        {
            case 1: localPositionCoin = new Vector3(107f, 24f, 0); break;
            case 2: localPositionCoin = new Vector3(190f, -55f, 0); break;
            case 3: localPositionCoin = new Vector3(255f, -65f, 0); break;
            case 4: localPositionCoin = new Vector3(-188f, -80, 0); break;
            case 5: localPositionCoin = new Vector3(49, -95f, 0); break;
            case 6: localPositionCoin = new Vector3(86f, -110f, 0); break;
        }
        return localPositionCoin;
    }

    void CreatObjectHelp(string nameObject, Vector3 vectorScale)
    {
        Transform objectPre = transform.Find(nameObject);
        if (objectPre == null)
        {
            GameObject objectPrefabs = (GameObject)Resources.Load("Help/" + nameObject);
            GameObject objectClone = (GameObject)Instantiate(objectPrefabs);
            Transform[] child = objectClone.transform.GetComponentsInChildren<Transform>();
            foreach (Transform ts in child)
            {
                ts.gameObject.layer = 5;
            }

            objectClone.transform.parent = gameObject.transform;
            objectClone.transform.localPosition = nameObject.Equals("CircleHelp") ? new Vector3(-144f, -170f, objectPrefabs.transform.position.z) :
                 new Vector3(-118f, -187f, objectPrefabs.transform.position.z);
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
            if (VariableSystem.mission == 4 && CreatAndControlPanelHelp.countClickHelpPanel == 3)
            {
                CreatObjectHelp("CircleHelp", new Vector3(40f, 40f, 40f));
                CreatObjectHelp("HandHelp", new Vector3(100f, 100f, 100f));
            }
        }
    }
}
