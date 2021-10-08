using UnityEngine;
using System.Collections;

public class BtnHireController : MonoBehaviour {

	// Use this for initialization
    private Transform objectChild;
	void Start () {
     
	}
	
	// Update is called once per frame
	void Update () {
        ControlViewHelp();
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

            if (nameObject.Equals("CircleHelp"))
            {
                objectChild = objectClone.transform.Find(nameObject);
            }
            else
            {
                objectChild = objectClone.transform.Find("hand");
            }

            objectChild.gameObject.layer = 5;
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
                if (CreatAndControlPanelHelp.countClickHelpPanel == 4)
                {
                    CreatObjectHelp("CircleHelp", new Vector3(80.0f, 40.0f, 80.0f));
                    CreatObjectHelp("HandHelp", new Vector3(80.0f, 80.0f, 80.0f));
                }
                else if (CreatAndControlPanelHelp.countClickHelpPanel == 5)
                {
                    DestroyObjecHelp("CircleHelp");
                    DestroyObjecHelp("HandHelp");
                }
            }
        }
    }
}
