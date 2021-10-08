using UnityEngine;
using System.Collections;
using Assets.Scripts.Common;

public class PanelComfirmUnlockController : MonoBehaviour
{
    public GameObject tempClick;
    public UILabel[] arrayLabel;
    private GameObject dinamondPrefabs;
    AudioControl audioControl;
    void OnEnable()
    {
        CommonObjectScript.isViewPoppup = true;
    }
    void Start()
    {
        audioControl = GameObject.Find("AudioControl").GetComponent<AudioControl>();
        dinamondPrefabs = (GameObject)Resources.Load("Factory/Queue/Diamond");
        arrayLabel[0].text = FactoryScenesController.languageHungBV["UNLOCKPOSITION"];
        arrayLabel[1].text = FactoryScenesController.languageHungBV["COMFIRUNLOCKPOSITION"];
        arrayLabel[2].text = FactoryScenesController.languageHungBV["AGREE"];
        arrayLabel[3].text = FactoryScenesController.languageHungBV["CANCEL"];

        ControlViewHelp();
    }
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
            if (VariableSystem.mission == 8 && CreatAndControlPanelHelp.countClickHelpPanel == 2)
            {
                ControlOKClick();
                DestroyObjecHelp("CircleHelp");
                DestroyObjecHelp("HandHelp");
                CreatAndControlPanelHelp.countClickHelpPanel = 3;
            }
        }
    }
    void ControlOKClick()
    {
        if (VariableSystem.diamond >= 1)
        {
            tempClick.GetComponent<ClickBackGround>().isUnLock = true;
            tempClick.GetComponent<ClickBackGround>().CreatLock();
            FactoryScenesController.listUnlockByPlayer.Add((6 - tempClick.GetComponent<ClickBackGround>().buttonBGID) % 6);
            AddCommonObject(0, -1);
            Instantiate(dinamondPrefabs, new Vector3(tempClick.transform.position.x + 0.7f, tempClick.transform.position.y + 0.9f, tempClick.transform.position.z), tempClick.transform.rotation);
            MissionData.factoryDataMission.positionUnlock.currentNumber += 1;
        }
        else
        {
            DialogInapp.ShowInapp();
        }
        gameObject.GetComponent<Animator>().Play("Invisible");
    }
    public void CancelButton_Click()
    {
        audioControl.PlaySound("Click 1");
        if (!FactoryScenesController.isHelp)
            gameObject.GetComponent<Animator>().Play("Invisible");
    }

    void DestroyGameObj()
    {
        gameObject.SetActive(false);
        CommonObjectScript.isViewPoppup = false;
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
            if (VariableSystem.mission == 8 && CreatAndControlPanelHelp.countClickHelpPanel == 2)
            {
                CreatObjectHelp("CircleHelp", new Vector3(40f, 40f, 40f));
                CreatObjectHelp("HandHelp", new Vector3(100f, 100f, 100f));
            }
        }
    }
}
