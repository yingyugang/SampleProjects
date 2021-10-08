using UnityEngine;
using System.Collections;

public class OnClickHouse : MonoBehaviour
{

    public int iDHouse;
    private Texture tempTextture;
    private bool isClick;
    void Start()
    {
        isClick = true;

    }

    // Update is called once per frame
    void Update()
    {
        ControlViewHelp();
    }
    void OnClick()
    {
        if (!TownScenesController.isHelp)
        {
            for (int count = 0; count < PanelHouseController.listDenyClick.Count; count++)
            {
                if (PanelHouseController.listDenyClick[count] == iDHouse)
                {
                    isClick = false;
                    break;
                }
            }
            if (isClick)
            {
                PanelHouseController.listIDHouseSelected.Add(iDHouse);
                PanelHouseController.listDenyClick.Add(iDHouse);
                tempTextture = gameObject.GetComponent<UITexture>().mainTexture;
                PanelHouseController.listSpriteNomal.Add(tempTextture);
                tempTextture = Resources.Load<Texture>("VilageResearch/House/" + gameObject.GetComponent<UITexture>().mainTexture.name);
                PanelHouseController.listSpriteSelect.Add(tempTextture);
                gameObject.GetComponent<UITexture>().mainTexture = tempTextture;
            }
        }
        else
        {
            //if (CreatAndControlPanelHelp.countClickHelpPanel == 2 )
            {
                for (int count = 0; count < PanelHouseController.listDenyClick.Count; count++)
                {
                    if (PanelHouseController.listDenyClick[count] == iDHouse)
                    {
                        isClick = false;
                        break;
                    }
                }
                if (isClick)
                {

                    if (iDHouse == 0 && CreatAndControlPanelHelp.countClickHelpPanel == 2)
                    {
                        ClickInHelp();
                        ChangeHelpPosition("CircleHelp", new Vector3(-327, -267, 6));
                        ChangeHelpPosition("HandHelp", new Vector3(-302, -286, 0));
                        CreatAndControlPanelHelp.countClickHelpPanel = 3;
                    }
                    else if (iDHouse == 1 && CreatAndControlPanelHelp.countClickHelpPanel == 3)
                    {
                        print("-------------------------------");
                        ClickInHelp();
                        ChangeHelpPosition("CircleHelp", new Vector3(-545, 33, 6));
                        ChangeHelpPosition("HandHelp", new Vector3(-422, 21, 0));
                        CreatAndControlPanelHelp.countClickHelpPanel = 4;
                    }
                    else if (iDHouse == 2 && CreatAndControlPanelHelp.countClickHelpPanel == 4)
                    {
                        ClickInHelp();
                        ChangeHelpPosition("CircleHelp", new Vector3(-299, -74, 6));
                        ChangeHelpPosition("HandHelp", new Vector3(-244, -97, 0));
                        CreatAndControlPanelHelp.countClickHelpPanel = 5;
                    }
                    else if (iDHouse == 3 && CreatAndControlPanelHelp.countClickHelpPanel == 5)
                    {
                        ClickInHelp();
                        ChangeHelpPosition("CircleHelp", new Vector3(-173, -175, 6));
                        ChangeHelpPosition("HandHelp", new Vector3(-103, -189, 0));
                        CreatAndControlPanelHelp.countClickHelpPanel = 6;
                    }
                    else if (iDHouse == 4 && CreatAndControlPanelHelp.countClickHelpPanel == 6)
                    {
                        ClickInHelp();
                        DestroyObjecHelp("CircleHelp");
                        DestroyObjecHelp("HandHelp");
                        CreatAndControlPanelHelp.countClickHelpPanel = 7;
                    }
                }
            }
        }
    }

    void ClickInHelp()
    {
        PanelHouseController.listIDHouseSelected.Add(iDHouse);
        PanelHouseController.listDenyClick.Add(iDHouse);
        tempTextture = gameObject.GetComponent<UITexture>().mainTexture;
        PanelHouseController.listSpriteNomal.Add(tempTextture);
        tempTextture = Resources.Load<Texture>("VilageResearch/House/" + gameObject.GetComponent<UITexture>().mainTexture.name);
        PanelHouseController.listSpriteSelect.Add(tempTextture);
        gameObject.GetComponent<UITexture>().mainTexture = tempTextture;
    }
    void CreatObjectHelp(string nameObject, Vector3 vectorScale, Vector3 localPosition)
    {
        Transform objectPre = transform.parent.Find(nameObject);
        if (objectPre == null)
        {
            GameObject objectPrefabs = (GameObject)Resources.Load("Help/" + nameObject);
            GameObject objectClone = (GameObject)Instantiate(objectPrefabs);
            Transform[] child = objectClone.transform.GetComponentsInChildren<Transform>();
            foreach (Transform ts in child)
            {
                ts.gameObject.layer = 5;
            }

            objectClone.transform.parent = gameObject.transform.parent;
            objectClone.transform.localPosition = localPosition;
            objectClone.transform.localScale = vectorScale;
            objectClone.name = objectPrefabs.name;
        }
    }
    void DestroyObjecHelp(string nameObject)
    {
        Transform objectClonePre = transform.parent.Find(nameObject);
        if (objectClonePre != null)
        {
            Destroy(objectClonePre.gameObject);
        }
    }
    void ControlViewHelp()
    {
        if (TownScenesController.isHelp)
        {
            if (VariableSystem.mission == 50)
            {
                if (CreatAndControlPanelHelp.countClickHelpPanel == 2 && iDHouse == 0)
                {
                    CreatObjectHelp("CircleHelp", new Vector3(40f, 40f, 40f), new Vector3(-442, -170, 0));
                    CreatObjectHelp("HandHelp", new Vector3(100f, 100f, 100f), new Vector3(-376, -181, 0));
                }
            }
        }
    }
    void ChangeHelpPosition(string nameObject, Vector3 positionChange)
    {
        Transform objectClonePre = transform.parent.Find(nameObject);
        if (objectClonePre != null)
        {
            objectClonePre.transform.localPosition = positionChange;
        }
    }
}
