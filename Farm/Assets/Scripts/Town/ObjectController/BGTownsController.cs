using UnityEngine;
using System.Collections;

public class BGTownsController : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        ControlViewHelp();
    }

    void CreatObjectHelp(string nameObject, Vector3 vectorScale, Vector3 localPosition, bool isUI = false)
    {
        Transform objectPre = transform.Find(nameObject);
        if (objectPre == null)
        {
            GameObject objectPrefabs = (GameObject)Resources.Load("Help/" + nameObject);
            GameObject objectClone = (GameObject)Instantiate(objectPrefabs);
            if (isUI)
            {
                Transform[] childs = objectClone.transform.GetComponentsInChildren<Transform>();
                foreach (Transform child in childs)
                {
                    child.gameObject.layer = 5;
                }
            }
            objectClone.transform.parent = gameObject.transform;
            objectClone.transform.localPosition = localPosition;
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
        if (TownScenesController.isHelp)
        {
            if (VariableSystem.mission == 2)
            {
                ControlHelpMission02();
            }
            else if (VariableSystem.mission == 7)
                ControlHelpMission07();
            else if (VariableSystem.mission == 11)
                ControlHelpMission11();
            else if (VariableSystem.mission == 15)
                ControlHelpMission15();
            else if (VariableSystem.mission == 18)
                ControlHelpMission18();
            else if (VariableSystem.mission == 21)
                ControlHelpMission21();
            else if (VariableSystem.mission == 26)
                ControlHelpMission26();
            else if (VariableSystem.mission == 50)
                ControlHelpMission50();
        }
    }

    void ControlHelpMission02()
    {
        if (CreatAndControlPanelHelp.countClickHelpPanel == 1)
        {
            CreatObjectHelp("CircleHelp", new Vector3(.2f, .2f, .2f), new Vector3(0.15f, -.5f, 5), true);
            CreatObjectHelp("HandHelp", new Vector3(.3f, .3f, .3f), new Vector3(.13f, -.6f, 5), true);
        }
        else if (CreatAndControlPanelHelp.countClickHelpPanel == 3)
        {
            print("vào đây");
            DestroyObjecHelp("CircleHelp");
            DestroyObjecHelp("HandHelp");
        }
    }
    void ControlHelpMission07()
    {
        if (CreatAndControlPanelHelp.countClickHelpPanel == 2)
        {
            CreatObjectHelp("CircleHelp", new Vector3(.2f, .2f, .2f), new Vector3(-1.3f, -0.03f, 5), true);
            CreatObjectHelp("HandHelp", new Vector3(-.3f, .3f, .3f), new Vector3(-1.3f, -.15f, 5), true);
        }
        else if (CreatAndControlPanelHelp.countClickHelpPanel == 3)
        {

            DestroyObjecHelp("CircleHelp");
            DestroyObjecHelp("HandHelp");
            TownScenesController.isHelp = false;
            EndHelp();
            //kết thúc hướng dẫn sổ xố
        }
    }
    void ControlHelpMission11()
    {
        if (CreatAndControlPanelHelp.countClickHelpPanel == 3)
        {
            CreatObjectHelp("CircleHelp", new Vector3(0.3f, 0.3f, 0.3f), new Vector3(0.03f, 0.4f, 5), true);
            CreatObjectHelp("HandHelp", new Vector3(0.3f, 0.3f, 0.3f), new Vector3(-0.28f, 0.3f, 5), true);
        }
        else if (CreatAndControlPanelHelp.countClickHelpPanel == 4)
        {
            DestroyObjecHelp("CircleHelp");
            DestroyObjecHelp("HandHelp");
        }
    }
    void ControlHelpMission15()
    {
        if (CreatAndControlPanelHelp.countClickHelpPanel == 2)
        {
            CreatObjectHelp("CircleHelp", new Vector3(.2f, .2f, .2f), new Vector3(.85f, .38f, 6), true);
            CreatObjectHelp("HandHelp", new Vector3(-.3f, .3f, .3f), new Vector3(.8f, .35f, 5), true);
        }
        else if (CreatAndControlPanelHelp.countClickHelpPanel == 3)
        {
            DestroyObjecHelp("CircleHelp");
            DestroyObjecHelp("HandHelp");
        }
        else if (CreatAndControlPanelHelp.countClickHelpPanel == 7)
        {
            TownScenesController.isHelp = false;
            EndHelp();
            //kết thúc hướng dẫn tòa nhà công nghệ
        }
    }
    void ControlHelpMission18()
    {
        if (CreatAndControlPanelHelp.countClickHelpPanel == 2)
        {
            CreatObjectHelp("CircleHelp", new Vector3(.2f, .2f, .2f), new Vector3(-0.56f, -0.33f, 5), true);
            CreatObjectHelp("HandHelp", new Vector3(-.3f, .3f, .3f), new Vector3(-.55f, -.47f, 5), true);
        }
        else if (CreatAndControlPanelHelp.countClickHelpPanel == 3)
        {

            DestroyObjecHelp("CircleHelp");
            DestroyObjecHelp("HandHelp");
            TownScenesController.isHelp = false;
            EndHelp();
            //    //kết thúc hướng dẫn tòa nhà truyền thông
        }
    }
    void ControlHelpMission21()
    {
        if (CreatAndControlPanelHelp.countClickHelpPanel == 2)
        {
            CreatObjectHelp("CircleHelp", new Vector3(.2f, .2f, .2f), new Vector3(1.0f, -0.15f, 5), true);
            CreatObjectHelp("HandHelp", new Vector3(.3f, .3f, .3f), new Vector3(1.0f, -.22f, 5), true);
        }
        else if (CreatAndControlPanelHelp.countClickHelpPanel == 3)
        {

            DestroyObjecHelp("CircleHelp");
            DestroyObjecHelp("HandHelp");
            TownScenesController.isHelp = false;
            EndHelp();
            //    //kết thúc hướng dẫn tòa nhà siêu thị
        }
    }
    void ControlHelpMission26()
    {
        if (CreatAndControlPanelHelp.countClickHelpPanel == 2)
        {
            CreatObjectHelp("CircleHelp", new Vector3(.8f, .8f, .8f), new Vector3(0.7f, -2f, 5));
            CreatObjectHelp("HandHelp", new Vector3(1f, 1f, 1f), new Vector3(-.3f, -3.0f, 5));
        }
        else if (CreatAndControlPanelHelp.countClickHelpPanel == 3)
        {
            DestroyObjecHelp("CircleHelp");
            DestroyObjecHelp("HandHelp");
        }
        else if (CreatAndControlPanelHelp.countClickHelpPanel == 7)
        {
            TownScenesController.isHelp = false;
            EndHelp();
            //kết thúc hướng dẫn update nhân viên
        }
    }
    void ControlHelpMission50()
    {
        if (CreatAndControlPanelHelp.countClickHelpPanel == 1)
        {
            CreatObjectHelp("CircleHelp", new Vector3(0.3f, 0.3f, 0.3f), new Vector3(0.03f, 0.4f, 5), true);
            CreatObjectHelp("HandHelp", new Vector3(0.3f, 0.3f, 0.3f), new Vector3(-0.28f, 0.3f, 5), true);
        }
        else if (CreatAndControlPanelHelp.countClickHelpPanel == 2)
        {
            DestroyObjecHelp("CircleHelp");
            DestroyObjecHelp("HandHelp");
        }
    }
    void EndHelp()
    {
        CommonObjectScript.CompleteGuide();
        GameObject.Find("CommonObject").GetComponent<CommonObjectScript>().setActiveButton(true, true, true, true, true, true, true, true);
    }
}
