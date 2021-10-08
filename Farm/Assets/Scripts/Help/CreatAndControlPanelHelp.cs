using UnityEngine;
using System.Collections;

public class CreatAndControlPanelHelp : MonoBehaviour
{

    // Use this for initialization
    public GameObject panelHelpPrefabs;
    public GameObject panelHelpClone;
    private bool isUpdate;
    public static int countClickHelpPanel; // do dùng chung panel help nên cái này để điều khiển xem đã đếm giao diện thứ mấy của phần help
    void Start()
    {
        countClickHelpPanel = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Application.loadedLevelName.Equals("Factory"))
            PanelHelpFactory();
        else if (Application.loadedLevelName.Equals("Town"))
            PanelHelpTown();
        else if (Application.loadedLevelName.Equals("VilageResearch"))
            PanelHelpVilage();
    }
    void CreatPanel(string RightToLeftOrLeftToRight)
    {
        if (!isUpdate)
        {
            panelHelpClone = (GameObject)Instantiate(panelHelpPrefabs, new Vector3(0, 0, 0), transform.rotation);
            panelHelpClone.transform.parent = gameObject.transform;
            panelHelpClone.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            panelHelpClone.name = panelHelpPrefabs.name;
            panelHelpClone.GetComponent<PanelHelpController>().setOrientationPanel(RightToLeftOrLeftToRight);
            isUpdate = true;
        }
    }
    void ControlPanelHelpFactory01()
    {
        if (panelHelpClone != null)
        {
            // if (VariableSystem.mission == 1)
            {
                int temp = panelHelpClone.GetComponent<PanelHelpController>().countClick;
                if (countClickHelpPanel < 3)
                {
                    countClickHelpPanel = temp;
                    panelHelpClone.GetComponent<PanelHelpController>().setTextView(FactoryScenesController.ListTextIntroduce[temp]);
                }
                else if (countClickHelpPanel == 3)
                {
                    panelHelpClone.GetComponent<PanelHelpController>().SetStatusHelp(false);
                }
                else if (countClickHelpPanel == 7)
                {
                    ResetPanelHelp(true, 3, 8);
                }
                else if (countClickHelpPanel == 8)
                {
                    if (temp == 4)
                    {
                        ResetPanelHelp(true, 4, 9);
                    }
                }
                else if (countClickHelpPanel == 9)
                {
                    if (temp == 5)
                    {
                        ResetPanelHelp(false, 4, 10);
                    }
                }
                else if (countClickHelpPanel == 13)
                {
                    ResetPanelHelp(true, 5, 14);
                }
                else if (countClickHelpPanel == 14)
                {
                    if (temp == 6)
                    {
                        ResetPanelHelp(false, 5, 15);
                    }
                }
                else if (countClickHelpPanel == 16)
                {
                    ResetPanelHelp(true, 6, 17);
                }
                else if (countClickHelpPanel == 17)
                {
                    if (temp == 7)
                    {
                        ResetPanelHelp(false, 6, 18);
                    }
                }
                else if (countClickHelpPanel == 19)
                {
                    ResetPanelHelp(true, 7, 20);
                }
                else if (countClickHelpPanel == 20)
                {
                    if (temp == 8)
                    {
                        ResetPanelHelp(false, 7, 21);
                    }
                }

            }
        }
    }
    void ControlPanelHelpFactory04()
    {
        if (panelHelpClone != null)
        {
            int temp = panelHelpClone.GetComponent<PanelHelpController>().countClick;
            if (countClickHelpPanel < 1)
            {
                countClickHelpPanel = temp;
                panelHelpClone.GetComponent<PanelHelpController>().setTextView(FactoryScenesController.ListTextIntroduce[0]);
            }
            else if (countClickHelpPanel == 1)
            {
                panelHelpClone.GetComponent<PanelHelpController>().SetStatusHelp(false);
            }
        }
    }
    void ControlPanelHelpFactory08()
    {
        if (panelHelpClone != null)
        {
            int temp = panelHelpClone.GetComponent<PanelHelpController>().countClick;
            if (countClickHelpPanel < 1)
            {
                countClickHelpPanel = temp;
                panelHelpClone.GetComponent<PanelHelpController>().setTextView(FactoryScenesController.ListTextIntroduce[0]);
            }
            else if (countClickHelpPanel == 1)
            {
                panelHelpClone.GetComponent<PanelHelpController>().SetStatusHelp(false);
            }
            else if (countClickHelpPanel == 3)
            {
                ResetPanelHelp(true, 1, 4);
            }
            else if (countClickHelpPanel == 4)
            {
                if (temp == 2)
                {
                    panelHelpClone.GetComponent<PanelHelpController>().SetStatusHelp(false);
                    countClickHelpPanel = 5;
                }
            }
        }
    }
    void ControlPanelHelpFactory13()
    {
        if (panelHelpClone != null)
        {
            int temp = panelHelpClone.GetComponent<PanelHelpController>().countClick;
            if (countClickHelpPanel < 1)
            {
                StartHelp();
                countClickHelpPanel = temp;
                panelHelpClone.GetComponent<PanelHelpController>().setTextView(FactoryScenesController.ListTextIntroduce[0]);
            }
            else if (countClickHelpPanel == 1)
            {
                panelHelpClone.GetComponent<PanelHelpController>().SetStatusHelp(false);
            }
        }
    }
    void PanelHelpFactory()
    {
        if (FactoryScenesController.isHelp)
        {
            // print("FactoryScenesController.isHelp" + FactoryScenesController.isHelp);
            if (VariableSystem.mission == 1)
            {
                CreatPanel("RightToLeft");
                ControlPanelHelpFactory01();
            }
            else if (VariableSystem.mission == 4)
            {
                CreatPanel("LeftToRight");
                ControlPanelHelpFactory04();
            }
            else if (VariableSystem.mission == 8)
            {
                CreatPanel("LeftToRight");
                ControlPanelHelpFactory08();
            }
            else if (VariableSystem.mission == 13)
            {
                CreatPanel("RightToLeft");
                ControlPanelHelpFactory13();
            }
        }
    }

    void ControlPanelHelpTown02()
    {
        if (panelHelpClone != null)
        {
            int temp = panelHelpClone.GetComponent<PanelHelpController>().countClick;
            //print(countClickHelpPanel);
            if (countClickHelpPanel < 1)
            {
                countClickHelpPanel = temp;
                panelHelpClone.GetComponent<PanelHelpController>().setTextView(TownScenesController.ListTextIntroduce[temp]);
            }
            else if (countClickHelpPanel == 1)
            {
                panelHelpClone.GetComponent<PanelHelpController>().SetStatusHelp(false);
            }
            else if (countClickHelpPanel == 2)
            {
                panelHelpClone.GetComponent<PanelHelpController>().SetStatusHelp(true);
                panelHelpClone.GetComponent<PanelHelpController>().setTextView(TownScenesController.ListTextIntroduce[1]);
                countClickHelpPanel = 3;
            }
            else if (countClickHelpPanel == 3)
            {
                if (temp == 2)
                {
                    ResetPanelHelp(false, 1, 4);
                }
            }
        }
    }
    void ControlPanelHelpTown07()
    {
        if (panelHelpClone != null)
        {
            int temp = panelHelpClone.GetComponent<PanelHelpController>().countClick;

            if (countClickHelpPanel <= 1)
            {
                countClickHelpPanel = temp;
                if (temp <= 1)
                    panelHelpClone.GetComponent<PanelHelpController>().setTextView(TownScenesController.ListTextIntroduce[temp]);
            }
            else if (countClickHelpPanel > 1)
            {
                panelHelpClone.GetComponent<PanelHelpController>().SetStatusHelp(false);
            }
        }
    }
    void ControlPanelHelpTown11()
    {
        if (panelHelpClone != null)
        {
            int temp = panelHelpClone.GetComponent<PanelHelpController>().countClick;
            if (countClickHelpPanel <= 2)
            {
                countClickHelpPanel = temp;
                if (temp <= 2)
                    panelHelpClone.GetComponent<PanelHelpController>().setTextView(TownScenesController.ListTextIntroduce[temp]);
            }
            else if (countClickHelpPanel == 3)
            {
                panelHelpClone.GetComponent<PanelHelpController>().SetStatusHelp(false);
            }
            else if (countClickHelpPanel == 8)
            {
                ResetPanelHelpTown(true, 3, 9);
            }
            else if (countClickHelpPanel == 9)
            {
                if (temp == 4)
                    ResetPanelHelpTown(true, 4, 10);
            }
            else if (countClickHelpPanel == 10)
            {
                if (temp == 5)
                {
                    ResetPanelHelpTown(false, 4, 11);
                }
            }
        }
    }
    void ControlPanelHelpTown15()
    {
        if (panelHelpClone != null)
        {
            int temp = panelHelpClone.GetComponent<PanelHelpController>().countClick;
            if (countClickHelpPanel <= 1)
            {
                countClickHelpPanel = temp;
                if (temp <= 1)
                    panelHelpClone.GetComponent<PanelHelpController>().setTextView(TownScenesController.ListTextIntroduce[temp]);
            }
            else if (countClickHelpPanel == 2)
            {
                panelHelpClone.GetComponent<PanelHelpController>().SetStatusHelp(false);
            }
            else if (countClickHelpPanel == 5)
            {
                ResetPanelHelpTown(true, 2, 6);
            }
            else if (countClickHelpPanel == 6)
            {
                if (temp == 3)
                {
                    ResetPanelHelpTown(false, 2, 7);
                }
            }
        }
    }
    void ControlPanelHelpTown18()
    {
        if (panelHelpClone != null)
        {
            int temp = panelHelpClone.GetComponent<PanelHelpController>().countClick;

            if (countClickHelpPanel <= 1)
            {
                countClickHelpPanel = temp;
                if (temp <= 1)
                    panelHelpClone.GetComponent<PanelHelpController>().setTextView(TownScenesController.ListTextIntroduce[temp]);
            }
            else if (countClickHelpPanel > 1)
            {
                panelHelpClone.GetComponent<PanelHelpController>().SetStatusHelp(false);
            }
        }
    }
    void ControlPanelHelpTown21()
    {
        if (panelHelpClone != null)
        {
            int temp = panelHelpClone.GetComponent<PanelHelpController>().countClick;

            if (countClickHelpPanel <= 1)
            {
                countClickHelpPanel = temp;
                if (temp <= 1)
                    panelHelpClone.GetComponent<PanelHelpController>().setTextView(TownScenesController.ListTextIntroduce[temp]);
            }
            else if (countClickHelpPanel > 1)
            {
                panelHelpClone.GetComponent<PanelHelpController>().SetStatusHelp(false);
            }
        }
    }
    void ControlPanelHelpTown26()
    {
        if (panelHelpClone != null)
        {
            int temp = panelHelpClone.GetComponent<PanelHelpController>().countClick;
            if (countClickHelpPanel <= 1)
            {
                countClickHelpPanel = temp;
                if (temp <= 1)
                    panelHelpClone.GetComponent<PanelHelpController>().setTextView(TownScenesController.ListTextIntroduce[temp]);
            }
            else if (countClickHelpPanel == 2)
            {
                panelHelpClone.GetComponent<PanelHelpController>().SetStatusHelp(false);
            }
            else if (countClickHelpPanel == 5)
            {
                print("vào đây");
                ResetPanelHelpTown(true, 2, 6);
            }
            else if (countClickHelpPanel == 6)
            {
                if (temp == 3)
                {
                    ResetPanelHelpTown(false, 2, 7);
                }
            }
        }
    }
    void ControlPanelHelpTown50()
    {
        //if (panelHelpClone != null)
        //{
        //    int temp = panelHelpClone.GetComponent<PanelHelpController>().countClick;
        //    if (countClickHelpPanel <= 1)
        //    {
        //        countClickHelpPanel = temp;
        //        if (temp <= 1)
        //            panelHelpClone.GetComponent<PanelHelpController>().setTextView(TownScenesController.ListTextIntroduce[temp]);
        //    }
        //    else if (countClickHelpPanel == 2)
        //    {
        //        panelHelpClone.GetComponent<PanelHelpController>().SetStatusHelp(false);
        //    }
        //    else if (countClickHelpPanel == 5)
        //    {
        //        print("vào đây");
        //        ResetPanelHelpTown(true, 2, 6);
        //    }
        //    else if (countClickHelpPanel == 6)
        //    {
        //        if (temp == 3)
        //        {
        //            ResetPanelHelpTown(false, 2, 7);
        //        }
        //    }
        //}
        if (countClickHelpPanel == 0)
            countClickHelpPanel = 1;
    }
    void PanelHelpTown()
    {
        if (TownScenesController.isHelp)
        {
            if (VariableSystem.mission == 2)
            {
                CreatPanel("LeftToRight");
                ControlPanelHelpTown02();
            }
            else if (VariableSystem.mission == 7)
            {
                CreatPanel("RightToLeft");
                ControlPanelHelpTown07();
            }
            else if (VariableSystem.mission == 11)
            {
                CreatPanel("LeftToRight");
                ControlPanelHelpTown11();
            }
            else if (VariableSystem.mission == 15)
            {
                CreatPanel("LeftToRight");
                ControlPanelHelpTown15();
            }
            else if (VariableSystem.mission == 18)
            {
                CreatPanel("RightToLeft");
                ControlPanelHelpTown18();
            }
            else if (VariableSystem.mission == 21)
            {
                CreatPanel("LeftToRight");
                ControlPanelHelpTown21();
            }
            else if (VariableSystem.mission == 26)
            {
                CreatPanel("LeftToRight");
                ControlPanelHelpTown26();
            }
            else if (VariableSystem.mission == 50)
            {
                //CreatPanel("LeftToRight");
                ControlPanelHelpTown50();
            }
        }
    }

    void ControlPanelHelpVilage50()
    {
       
        int temp = panelHelpClone.GetComponent<PanelHelpController>().countClick;
        if (countClickHelpPanel < 1)
        {
            countClickHelpPanel = temp;
            if (temp <= 0)
                panelHelpClone.GetComponent<PanelHelpController>().setTextView(TownScenesController.ListTextIntroduce[temp]);
        }
        else if (countClickHelpPanel == 1)
        {
            panelHelpClone.GetComponent<PanelHelpController>().SetStatusHelp(false);
            countClickHelpPanel = 2;
        }
        else if (countClickHelpPanel == 7)
        {
            panelHelpClone.GetComponent<PanelHelpController>().SetStatusHelp(true);
            panelHelpClone.GetComponent<PanelHelpController>().setTextView(TownScenesController.ListTextIntroduce[1]);
            countClickHelpPanel = 8;
        }
        else if (countClickHelpPanel == 8)
        {
            if (temp == 2)
            {
                panelHelpClone.GetComponent<PanelHelpController>().setTextView(TownScenesController.ListTextIntroduce[2]);
                countClickHelpPanel = 9;
               
            }
        }
        else if (countClickHelpPanel == 9)
        {
            panelHelpClone.GetComponent<PanelHelpController>().SetStatusHelp(false);
            EndHelp();
            TownScenesController.isHelp = false;
            // kết thúc hướng dẫn vilage
        }
    }
    void PanelHelpVilage()
    {
        if (TownScenesController.isHelp)
        {
            if (VariableSystem.mission == 50)
            {
                CreatPanel("RightToLeft");
                ControlPanelHelpVilage50();
            }
        }
    }
    void ResetPanelHelp(bool isView, int countText, int countClick)
    {
        if (panelHelpClone != null)
        {
            panelHelpClone.GetComponent<PanelHelpController>().SetStatusHelp(isView);
            if (isView)
            {
                panelHelpClone.GetComponent<PanelHelpController>().setTextView(FactoryScenesController.ListTextIntroduce[countText]);
            }
            countClickHelpPanel = countClick;
        }
    }
    void ResetPanelHelpTown(bool isView, int countText, int countClick)
    {
        if (panelHelpClone != null)
        {
            panelHelpClone.GetComponent<PanelHelpController>().SetStatusHelp(isView);
            if (isView)
            {
                panelHelpClone.GetComponent<PanelHelpController>().setTextView(TownScenesController.ListTextIntroduce[countText]);
            }
            countClickHelpPanel = countClick;
        }
    }
    void EndHelp()
    {
        CommonObjectScript.CompleteGuide();
        GameObject.Find("CommonObject").GetComponent<CommonObjectScript>().setActiveButton(true, true, true, true, true, true, true, true);
    }
    void StartHelp()
    {
        CommonObjectScript.isGuide = true;
        GameObject.Find("CommonObject").GetComponent<CommonObjectScript>().setActiveButton(false, false, false, false, false, false, false, false);
    }
}
