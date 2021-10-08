using UnityEngine;
using System.Collections;

public class CreatTownScenesController : MonoBehaviour
{

    // Use this for initialization
    private GameObject townScrenesControllerPrefabs;
    private GameObject townScrenesControllerClone;

    private GameObject townsPrefabs;
    private GameObject townsClone;

    public static bool isDenyContinue;
    void Start()
    {
        isDenyContinue = false;
        CreatFactoryScrenesController();
       
        GameObject.Find("CommonObject").GetComponent<CommonObjectScript>().WarningInvisible(CommonObjectScript.Button.City);
    }

    // Update is called once per frame
    void Update()
    {
        if (!isDenyContinue)
        {
          //  print("vao day aaaaaaaaaaaaaaaaaaaaaaaa");
            if (TownScenesController.queuePopup != null)
            {
                if (TownScenesController.queuePopup.Count != 0)
                {
                   
                    if (!Application.isLoadingLevel)
                    {
                        if (Application.loadedLevelName.Equals("Town"))
                        {
                            GameObject tempGameObject = TownScenesController.queuePopup.Dequeue();
                            tempGameObject.transform.parent = transform;
                            tempGameObject.transform.localPosition = new Vector3(0, 0, 0);
                            tempGameObject.transform.localScale = new Vector3(1, 1, 1);
                            tempGameObject.GetComponent<Animator>().Play("Visible");
                            isDenyContinue = true;
                        }
                    }
                }
            }
        }

        ControlViewHelp();
    }
    void CreatFactoryScrenesController()
    {
        if (!TownScenesController.isCreat)
        {
            //townsPrefabs = (GameObject)Resources.Load("Town/PrefabsConmon/Towns");
            //townsClone = (GameObject)Instantiate(townsPrefabs, townsPrefabs.transform.position, townsPrefabs.transform.rotation);
            //townsClone.name = townsPrefabs.name;

            townScrenesControllerPrefabs = (GameObject)Resources.Load("Town/PrefabsConmon/TownScenesController");
            townScrenesControllerClone = (GameObject)Instantiate(townScrenesControllerPrefabs, townScrenesControllerPrefabs.transform.position, townScrenesControllerPrefabs.transform.rotation);
            townScrenesControllerClone.name = townScrenesControllerPrefabs.name;
        }
    }

    void CreatObjectHelp(string nameObject, Vector3 vectorScale, Vector3 localPosition)
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
            if (VariableSystem.mission == 11)
            {
                if (CreatAndControlPanelHelp.countClickHelpPanel == 11)
                {
                    CreatObjectHelp("CircleHelp", new Vector3(35f, 35f, 35f), new Vector3(-585, -56, 0));
                    CreatObjectHelp("HandHelp", new Vector3(-100f, 100f, 100f), new Vector3(-620, -90, 0));
                }
                else
                    if (CreatAndControlPanelHelp.countClickHelpPanel == 12)
                    {
                        DestroyObjecHelp("CircleHelp");
                        DestroyObjecHelp("HandHelp");
                        TownScenesController.isHelp = false;
                        EndHelp();
                        // kết thúc maket research
                    }
            }
        }
    }

    void EndHelp()
    {
        CommonObjectScript.CompleteGuide();
        GameObject.Find("CommonObject").GetComponent<CommonObjectScript>().setActiveButton(true, true, true, true, true, true, true, true);
    }
}
