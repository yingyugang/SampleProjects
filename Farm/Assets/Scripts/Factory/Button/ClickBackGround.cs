using UnityEngine;
using System.Collections;
using Assets.Scripts.Common;

public class ClickBackGround : MonoBehaviour
{

    // Use this for initialization
    public int buttonBGID;
    public bool isUnLock;
    private GameObject ciclePrefabs;
    private GameObject cicleClone;

    private GameObject buttonViewPrefabs;
    private GameObject buttonViewClone;

    private GameObject iconLockPrefabs;
    private GameObject iconLockClone;

    public static GameObject lockSeclect;
    private bool isCreatMachine;

    public Transform UIRoot;

    private Vector3 posMouseClick;
    private Vector3 posMouseUpdate;

    private GameObject notePrefabs;
    private GameObject noteClone;
    void Start()
    {
        ciclePrefabs = (GameObject)Resources.Load("Factory/Button/Prefabs/Cicle");
        notePrefabs = (GameObject)Resources.Load("Factory/Queue/FullQueue");
    }

    void Update()
    {
        posMouseUpdate = Input.mousePosition;
        ControlViewHelp();
    }
    void OnMouseDown()
    {
        if (FactoryScenesController.isHelp)
        {
            if (VariableSystem.mission == 1)
            {
                if (CreatAndControlPanelHelp.countClickHelpPanel == 3 && buttonBGID == 5)
                {

                    ControllMouseDown();
                    CreatAndControlPanelHelp.countClickHelpPanel = 4;
                }
            }
        }
        else
        {
            posMouseClick = Input.mousePosition;
            ControllMouseDown();
        }
    }

    void OnMouseUp()
    {
        if (FactoryScenesController.isHelp)
        {
            if (isUnLock)
            {
                if (VariableSystem.mission == 1)
                {
                    if (CreatAndControlPanelHelp.countClickHelpPanel == 4 && buttonBGID == 5)
                    {
                        ControllMouseUp();
                        DestroyObjecHelp("CircleHelp");
                        DestroyObjecHelp("HandHelp");
                        isCreatMachine = false;
                    }
                }
            }
            else
            {
                if (VariableSystem.mission == 8)
                {
                    if (CreatAndControlPanelHelp.countClickHelpPanel == 1)
                    {
                        ControllMouseUpLock();
                        CreatAndControlPanelHelp.countClickHelpPanel = 2;
                    }
                }
            }
        }
        else
        {
            if (isUnLock)
            {
                if (Mathf.Abs(posMouseClick.x - posMouseUpdate.x) > 0.5f && Mathf.Abs(posMouseClick.y - posMouseUpdate.y) > 0.5f)
                    CameraController.isDrag = true;
                else
                {
                    ControllMouseUp();
                    CameraController.IDPosition = buttonBGID;
                    CameraController.isDrag = false;
                }
            }
            else
            {
                if (VariableSystem.mission >= 8)
                    ControllMouseUpLock();
                else
                    CreatNote(this.transform, new Vector3(1.2f, 1.2f, 1.2f), new Vector3(0, 0, 0), FactoryScenesController.languageHungBV["LOCKED"]);
            }
        }
    }

    void ControllMouseDown()
    {
        if (isUnLock)
        {
            FindAndDestroyGameObject("Cicle");
            FindAndDestroyGameObject("buttonView");
            FindAnDestroyChild("ProductQueue");
            isCreatMachine = true;
            foreach (int idHave in FactoryScenesController.IDCreatMachine) // kiểm tra xem trên vùng đang chọn đã có máy hay chưa
            {
                if (buttonBGID == idHave) isCreatMachine = false;
            }
        }
    }

    void FindAndDestroyGameObject(string nameGameObject)
    {

        GameObject tempGameObject = GameObject.Find(nameGameObject);
        if (tempGameObject != null)
        {
            Destroy(tempGameObject);
            // print("thang bg gay ra");
        }
    }
    void FindAnDestroyChild(string name)
    {
        if (MachineController.machineSelect != null)
        {
            Transform childObject = MachineController.machineSelect.transform.Find(name);
            if (childObject != null)
                Destroy(childObject.gameObject);
        }
    }
    void FindAndChildUIRoot(string name)
    {
        Transform childObject = UIRoot.Find(name);
        if (childObject != null)
            Destroy(childObject.gameObject);
    }
    void LoadBtView()
    {
        if (isCreatMachine)
        {
            FactoryScenesController.IDBackGroundButton = buttonBGID;
            buttonViewPrefabs = (GameObject)Resources.Load("Factory/Button/Prefabs/BTView" + MissionData.factoryDataMission.machinedatas.Count);
            FactoryScenesController.nameClick = "BGClick";

            cicleClone = (GameObject)Instantiate(ciclePrefabs, new Vector3(transform.position.x, transform.position.y, ciclePrefabs.transform.position.z),
               transform.rotation);
            cicleClone.transform.parent = transform;
            cicleClone.name = "Cicle";
        }
    }

    void ControllMouseUp()
    {
        if (isCreatMachine)
        {
            LoadBtView();
            if (buttonViewPrefabs != null)
            {
                buttonViewClone = (GameObject)Instantiate(buttonViewPrefabs,
                    new Vector3(cicleClone.transform.position.x, cicleClone.transform.position.y,
                        buttonViewPrefabs.transform.position.z), cicleClone.transform.rotation);
                buttonViewClone.transform.parent = transform;
                buttonViewClone.name = "buttonView";
            }
            lockSeclect = gameObject;
        }
    }

    void ControllMouseUpLock()
    {

        if (ButtonViewController.animator != null)
            ButtonViewController.animator.SetTrigger("Collape");
        if (ProductQueueController.animator != null)
            ProductQueueController.animator.SetTrigger("Collape");

        GameObject UIRoot = GameObject.Find("UI Root");
        Transform panelComfirm = UIRoot.transform.Find("PanelComfirmUnlock");
        panelComfirm.gameObject.SetActive(true);
        panelComfirm.GetComponent<PanelComfirmUnlockController>().tempClick = this.gameObject;
       // CameraController.isViewPopup = true;
    }

    public void CreatLock()
    {
        Transform lockPre = this.gameObject.transform.Find("Lock");
        if (lockPre != null)
            Destroy(lockPre.gameObject);

        if (isUnLock)
            iconLockPrefabs = (GameObject)Resources.Load("Factory/Button/Prefabs/UnLock");
        else
            iconLockPrefabs = (GameObject)Resources.Load("Factory/Button/Prefabs/Lock");

        iconLockClone = (GameObject)Instantiate(iconLockPrefabs,
            new Vector3(transform.position.x, transform.position.y, iconLockPrefabs.transform.position.z),
               transform.rotation);
        iconLockClone.name = "Lock";
        iconLockClone.transform.parent = gameObject.transform;
    }
    void CreatNote(Transform parent, Vector3 localScale, Vector3 localPosition, string text)
    {
        if (noteClone == null)
        {
            noteClone = (GameObject)Instantiate(notePrefabs);
            noteClone.transform.parent = parent;
            noteClone.transform.localScale = localScale;
            noteClone.transform.localPosition = localPosition;
            noteClone.GetComponent<FullQueuController>().text = text;
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
            if (VariableSystem.mission == 1)
            {
                if (CreatAndControlPanelHelp.countClickHelpPanel == 0)
                {
                    if (!isUnLock)
                    {
                        CreatObjectHelp("CircleHelp", new Vector3(.5f, .5f, .5f));
                    }
                }
                else if (CreatAndControlPanelHelp.countClickHelpPanel == 1)
                {
                    if (isUnLock)
                    {
                        CreatObjectHelp("CircleHelp", new Vector3(.5f, .5f, .5f));
                    }
                    else
                    {
                        DestroyObjecHelp("CircleHelp");
                    }
                }
                else if (CreatAndControlPanelHelp.countClickHelpPanel == 3)
                {
                    if (isUnLock)
                    {
                        if (buttonBGID == 5)
                        {
                            CreatObjectHelp("HandHelp", new Vector3(1f, 1f, 1f));
                        }
                        else
                        {
                            DestroyObjecHelp("CircleHelp");
                        }
                    }
                }
            }
        }
    }
}
