using UnityEngine;
using System.Collections;

public class RePairController : MonoBehaviour {

	// Use this for initialization
    public int sortingLayerID;
    private float timeRePair;
    private float countTimerePair;
    private GameObject commonObject;
	void Start () {
        SetIDLayer(this.sortingLayerID);
        timeRePair = 5.0f;
        countTimerePair = 0f;
        commonObject = GameObject.Find("CommonObject");
        commonObject.GetComponent<CommonObjectScript>().WarningInvisible(CommonObjectScript.Button.Factory);
	}
	
	// Update is called once per frame
	void Update () {
        if (countTimerePair < timeRePair)
        {
            countTimerePair += Time.deltaTime;
        }
        else
        {
           
            transform.parent.GetComponent<MachineController>().isFail = false;
            transform.parent.GetComponent<MachineController>().isRePairting = false;
            if (transform.parent.GetComponent<MachineController>().warningFail != null)
                transform.parent.GetComponent<MachineController>().warningFail.RemoveWarning(4);
            else
            {
                WarningTextView warningFail = new WarningTextView();
                warningFail.RemoveWarning(4);
            }
            Destroy(gameObject);
        }
	}

    void SetIDLayer(int sortingLayerID)
    {
        Transform[] transforms = gameObject.GetComponentsInChildren<Transform>(true);

        for (int i = 0; i < transforms.Length; i++)
        {
            GameObject gObject = transforms[i].gameObject;
            if (gObject.GetComponent<SpriteRenderer>() != null)
            {
                gObject.GetComponent<SpriteRenderer>().sortingLayerID = sortingLayerID + 1;
            }
        }
    }
    void FindAnDestroyChild(string name)
    {
        if (MachineController.machineSelect != null)
        {
            Transform childObject = transform.parent.transform.Find(name);
            if (childObject != null)
                Destroy(childObject.gameObject);
        }
    }
}
