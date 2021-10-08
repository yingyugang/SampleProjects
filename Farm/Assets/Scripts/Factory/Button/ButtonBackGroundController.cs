using UnityEngine;
using System.Collections;
using Assets.Scripts.Common;
using System.Collections.Generic;

public class ButtonBackGroundController : MonoBehaviour
{

    // Use this for initialization
    public GameObject[] buttonBackGroundClick;
    private GameObject factoryScrenesControllerPrefabs;
    private GameObject factoryScrenesControllerClone;
    private int cellUnLock;
    private bool isUpdate;

    void Awake()
    {
        CreatFactoryScrenesController();
    }
    void Start()
    {
        isUpdate = false;
        cellUnLock = MissionData.factoryDataMission.positionUnlock.positionUnLockBegin;
        //cellUnLock = 4;
        UnlockCell();
    
    }

   
    void Update()
    {
        if (!isUpdate)
        {
            SetStatusLock();
            isUpdate = true;
        }
        ControlViewHelp();
    }
    void UnlockCell()
    {
        
        for (int countButtonGivenUnlock = 0; countButtonGivenUnlock < cellUnLock; countButtonGivenUnlock++ )
            buttonBackGroundClick[countButtonGivenUnlock].GetComponent<ClickBackGround>().isUnLock = true;

        if (FactoryScenesController.listUnlockByPlayer != null)
        {
            foreach (int iDUnlock in FactoryScenesController.listUnlockByPlayer)
            {
               // print(iDUnlock);
                buttonBackGroundClick[iDUnlock].GetComponent<ClickBackGround>().isUnLock = true;
            }
        }

        for (int countButtonBackGround = 0; countButtonBackGround < 6; countButtonBackGround++)
            buttonBackGroundClick[countButtonBackGround].GetComponent<ClickBackGround>().CreatLock();
    }

    void SetStatusLock()
    {
        if (FactoryScenesController.IDCreatMachine != null)
        {
            foreach (int idHave in FactoryScenesController.IDCreatMachine) // kiểm tra xem trên vùng đang chọn đã có máy hay chưa
            {
                Transform LockPre = buttonBackGroundClick[(6 - idHave) % 6].transform.Find("Lock");
                if (LockPre != null)
                    Destroy(LockPre.gameObject);
            }
        }
    }



    void CreatFactoryScrenesController()
    {
        if (!FactoryScenesController.isCreat)
        {
            factoryScrenesControllerPrefabs = (GameObject)Resources.Load("Factory/CommonFactoryPrefabs/FactoryScenesController");
            factoryScrenesControllerClone = (GameObject)Instantiate(factoryScrenesControllerPrefabs, transform.position, transform.rotation);
            factoryScrenesControllerClone.name = factoryScrenesControllerPrefabs.name;
            if (FactoryScenesController.listUnlockByPlayer != null)
                FactoryScenesController.listUnlockByPlayer.Clear();
            else
            {
                FactoryScenesController.listUnlockByPlayer = new List<int>();
                FactoryScenesController.listUnlockByPlayer.Clear();
               // print("lam lai nha " + FactoryScenesController.listUnlockByPlayer.Count);
            }
        }
    }

    void CreatObjectHelp(string nameObject, Vector3 vectorScale)
    {
        Transform objectPre = transform.Find(nameObject);
        if (objectPre == null)
        {
            GameObject objectPrefabs = (GameObject)Resources.Load("Help/" + nameObject);
            GameObject objectClone = (GameObject)Instantiate(objectPrefabs,
                new Vector3(transform.position.x + 1.98f, transform.position.y + 0.46f, objectPrefabs.transform.position.z),
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
            if (VariableSystem.mission == 8)
            {
                if (CreatAndControlPanelHelp.countClickHelpPanel == 1)
                {
                    CreatObjectHelp("CircleHelp", new Vector3(.5f, .5f, .5f));
                    CreatObjectHelp("HandHelp", new Vector3(1f, 1f, 1f));
                    DisableButton(false);
                    EnableOneButton(4);
                }
                else if (CreatAndControlPanelHelp.countClickHelpPanel == 2)
                {
                    DestroyObjecHelp("CircleHelp");
                    DestroyObjecHelp("HandHelp");
                }
                else if (CreatAndControlPanelHelp.countClickHelpPanel == 5)
                {
                    DisableButton(true);
                    FactoryScenesController.isHelp = false;

                    EndHelp();
                    //kết thúc hướng dẫn mở rộng ô đất
                }
            }
        }
    }

    void DisableButton(bool isDisable)
    {
        foreach (GameObject button in buttonBackGroundClick)
        {
            button.GetComponent<PolygonCollider2D>().enabled = isDisable;
        }
    }
    void EnableOneButton(int id)
    {
        buttonBackGroundClick[id].GetComponent<PolygonCollider2D>().enabled = true;
    }

    void EndHelp()
    {
        CommonObjectScript.CompleteGuide();
        GameObject.Find("CommonObject").GetComponent<CommonObjectScript>().setActiveButton(true, true, true, true, true, true, true, true);
    }
}
