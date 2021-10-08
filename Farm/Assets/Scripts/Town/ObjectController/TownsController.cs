using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Common;

public class TownsController : MonoBehaviour
{

    // Use this for initialization
    public UIButton[] listTowns;
    public UIPanel[] arrayPoppup;
    public GameObject UIRoot;
    private int countUnLockTowns;
    private int[] IDButtonUnLock;

    private GameObject poppupPrefabs;
    private GameObject poppupClone;

    public UIPanel panelNote;
    public UILabel labelNote;

    private static Texture[] arrayTexttureTown;
    void Start()
    {
        // print("vao start khi khoi dong");
        // listTowns[0].GetComponent<UITexture>().
        // if (VariableSystem.mission == 1)
        //     IDButtonUnLock = new int[] { };
        // else if (VariableSystem.mission == 2)
        // {   
        //     IDButtonUnLock = new int[] { 0 };
        // }
        // else if (VariableSystem.mission == 3)
        //     IDButtonUnLock = new int[] { 0 };
        // else if (VariableSystem.mission == 4)
        //     IDButtonUnLock = new int[] { 0 };
        // else if (VariableSystem.mission == 5)
        //     IDButtonUnLock = new int[] { 0 };
        //// else //if (VariableSystem.mission == 6)
        //IDButtonUnLock = new int[] { 0, 3, 2, 1,4,5};
        // IDButtonUnLock = MissionData.townDataMission.buildingsOpen;
        //MissionData.townDataMission.buildingsOpen = new List<int>();
        //MissionData.townDataMission.buildingsOpen = new List<int> { 0, 3, 2, 1, 4, 5 };
        if (!TownScenesController.isCreat)
        {
            arrayTexttureTown = new Texture[6];
            SetImageTown(MissionControl.max_mission);

        }
        setLockTowns();
        setUnLockTowns();
    }

    // Update is called once per frame
    void Update()
    {
    }

    void setLockTowns()
    {
        for (int i = 0; i < listTowns.Length; i++)
        {
            listTowns[i].GetComponent<UITexture>().mainTexture = arrayTexttureTown[i];
            listTowns[i].pressed = new Color32(70, 70, 70, 255); ;
            listTowns[i].hover = new Color32(70, 70, 70, 255); ;
            listTowns[i].disabledColor = new Color32(70, 70, 70, 255);
            listTowns[i].defaultColor = new Color32(70, 70, 70, 255);

            listTowns[i].GetComponent<UIButtonScale>().enabled = false;
        }
    }
    void setUnLockTowns()
    {
        foreach (int id in MissionData.townDataMission.buildingsOpen)
        {
            listTowns[id].pressed = new Color32(183, 163, 123, 255); ;
            listTowns[id].hover = new Color32(225, 200, 150, 255);
            listTowns[id].disabledColor = new Color32(128, 128, 128, 255);
            listTowns[id].defaultColor = new Color32(255, 255, 255, 255);

            listTowns[id].GetComponent<UIButtonScale>().enabled = true;
        }
    }

    public void StaffHouse_Click()
    {
        if (TownScenesController.isHelp)
        {
            if (VariableSystem.mission == 2)
            {
                if (CreatAndControlPanelHelp.countClickHelpPanel == 1)
                {
                    CreatAndControlPanelHelp.countClickHelpPanel = 2;
                    // print(CreatAndControlPanelHelp.countClickHelpPanel);
                    arrayPoppup[0].gameObject.SetActive(true);
                }
            }
            else if (VariableSystem.mission == 26)
            {
                if (CreatAndControlPanelHelp.countClickHelpPanel == 2)
                {
                    CreatAndControlPanelHelp.countClickHelpPanel = 3;
                    // print(CreatAndControlPanelHelp.countClickHelpPanel);
                    arrayPoppup[0].gameObject.SetActive(true);
                }
            }
        }
        else
        {
            if (listTowns[0].defaultColor == new Color32(70, 70, 70, 255))
            {
                CreatNote(listTowns[0].transform, 2);
            }
            else
            {
                arrayPoppup[0].gameObject.SetActive(true);
                CreatTownScenesController.isDenyContinue = true;
            }
        }
    }
    public void SupperMarket_Click()
    {
        if (!TownScenesController.isHelp)
        {
            if (listTowns[1].defaultColor == new Color32(70, 70, 70, 255))
            {
                CreatNote(listTowns[1].transform, 21);
            }
            else
            {
                arrayPoppup[1].gameObject.SetActive(true);
                CreatTownScenesController.isDenyContinue = true;
            }
        }
        else
        {
            if (CreatAndControlPanelHelp.countClickHelpPanel == 2 && VariableSystem.mission == 21)
            {
                arrayPoppup[1].gameObject.SetActive(true);
                CreatTownScenesController.isDenyContinue = true;
                CreatAndControlPanelHelp.countClickHelpPanel = 3;
            }
        }
    }
    public void Mutilmedia_Click()
    {
        if (!TownScenesController.isHelp)
        {
            if (listTowns[2].defaultColor == new Color32(70, 70, 70, 255))
            {
                CreatNote(listTowns[2].transform, 18);
            }
            else
            {
                if (!TownScenesController.townsBusy[2])
                {
                    arrayPoppup[2].gameObject.SetActive(true);
                    CreatTownScenesController.isDenyContinue = true;
                }
            }
        }
        else
        {
            if (CreatAndControlPanelHelp.countClickHelpPanel == 2 && VariableSystem.mission == 18)
            {
                if (!TownScenesController.townsBusy[2])
                {
                    arrayPoppup[2].gameObject.SetActive(true);
                    CreatTownScenesController.isDenyContinue = true;
                }
                CreatAndControlPanelHelp.countClickHelpPanel = 3;
            }
        }
    }
    public void MaketResearch_Click()
    {
        if (!TownScenesController.isHelp)
        {
            if (listTowns[3].defaultColor == new Color32(70, 70, 70, 255))
            {
                CreatNote(listTowns[3].transform, 11);
            }
            else
            {
                if (!TownScenesController.townsBusy[3])
                {
                    arrayPoppup[3].gameObject.SetActive(true);
                    CreatTownScenesController.isDenyContinue = true;
                }
            }
        }
        else
        {
            if (VariableSystem.mission == 11)
            {
                if (CreatAndControlPanelHelp.countClickHelpPanel == 3)
                {
                    arrayPoppup[3].gameObject.SetActive(true);
                    CreatTownScenesController.isDenyContinue = true;
                    CreatAndControlPanelHelp.countClickHelpPanel = 4;
                }
            }
            else if (VariableSystem.mission == 50)
            {
                if (CreatAndControlPanelHelp.countClickHelpPanel == 1)
                {
                    arrayPoppup[3].gameObject.SetActive(true);
                    CreatTownScenesController.isDenyContinue = true;
                    CreatAndControlPanelHelp.countClickHelpPanel = 2;
                    //TownScenesController.isHelp = false;
                }
            }
            {

            }
        }
    }
    public void Lottery_Click()
    {
        if (!TownScenesController.isHelp)
        {
            //print(listTowns[4].defaultColor);
            if (listTowns[4].defaultColor == new Color32(70, 70, 70, 255))
            {
                CreatNote(listTowns[4].transform, 7);
            }
            else
            {
                arrayPoppup[4].gameObject.SetActive(true);
                CreatTownScenesController.isDenyContinue = true;
            }
        }
        else
        {
            if (CreatAndControlPanelHelp.countClickHelpPanel == 2 && VariableSystem.mission == 7)
            {
                arrayPoppup[4].gameObject.SetActive(true);
                CreatTownScenesController.isDenyContinue = true;
                CreatAndControlPanelHelp.countClickHelpPanel = 3;
            }
        }
    }
    public void Technology_Click()
    {
        if (TownScenesController.isHelp)
        {
            if (VariableSystem.mission == 15)
            {
                if (CreatAndControlPanelHelp.countClickHelpPanel == 2)
                {
                    if (!TownScenesController.townsBusy[5])
                    {
                        arrayPoppup[5].gameObject.SetActive(true);
                        CreatTownScenesController.isDenyContinue = true;
                    }
                    CreatAndControlPanelHelp.countClickHelpPanel = 3;
                }
            }
        }
        else
        {
            if (listTowns[5].defaultColor == new Color32(70, 70, 70, 255))
            {
                CreatNote(listTowns[5].transform, 15);
            }
            else
            {
                if (!TownScenesController.townsBusy[5])
                {
                    arrayPoppup[5].gameObject.SetActive(true);
                    CreatTownScenesController.isDenyContinue = true;
                }
            }
        }
    }

    void CreatNote(Transform parent, int missionUnlock)
    {
        if (!panelNote.enabled)
        {
            labelNote.text = TownScenesController.languageTowns["Unlock"] + missionUnlock;
            panelNote.transform.parent = parent;
            panelNote.transform.localPosition = new Vector3(0, 0, 0);
            panelNote.transform.localScale = new Vector3(1, 1, 1);
            panelNote.GetComponent<Animator>().Play("Active");
            // panelNote.GetComponent<Animator>().
        }

    }

    void LoadArrayTextureTown(int levelStaff, int levelLotery, int levelMaketResearch, int levelTechnogy, int levelMutilmedia, int levelSupermarket)
    {
        string linkParent = "Town/Deco/";
        arrayTexttureTown[0] = Resources.Load<Texture>(linkParent + "staff/" + levelStaff);
        arrayTexttureTown[4] = Resources.Load<Texture>(linkParent + "lottery/" + levelLotery);
        arrayTexttureTown[3] = Resources.Load<Texture>(linkParent + "marketResearch/" + levelMaketResearch);
        arrayTexttureTown[5] = Resources.Load<Texture>(linkParent + "technology/" + levelTechnogy);
        arrayTexttureTown[2] = Resources.Load<Texture>(linkParent + "multimedia/" + levelMutilmedia);
        arrayTexttureTown[1] = Resources.Load<Texture>(linkParent + "superMarket/" + levelSupermarket);
    }
    void SetImageTown(int curentLevel)
    {
        if (curentLevel < 4)
        {
            LoadArrayTextureTown(1, 1, 1, 1, 1, 1);
        }
        else if (curentLevel >= 4 && curentLevel < 9)
        {
            LoadArrayTextureTown(2, 1, 1, 1, 1, 1);
        }
        else if (curentLevel >= 9 && curentLevel < 13)
        {
            LoadArrayTextureTown(2, 2, 1, 1, 1, 1);
        }
        else if (curentLevel >= 13 && curentLevel < 17)
        {
            LoadArrayTextureTown(2, 2, 2, 1, 1, 1);
        }
        else if (curentLevel >= 17 && curentLevel < 20)
        {
            LoadArrayTextureTown(2, 2, 2, 2, 1, 1);
        }
        else if (curentLevel >= 20 && curentLevel < 23)
        {
            LoadArrayTextureTown(2, 2, 2, 2, 2, 1);
        }
        else if (curentLevel >= 23)
        {
            LoadArrayTextureTown(2, 2, 2, 2, 2, 2);
        }
    }

}
