using UnityEngine;
using System.Collections;
using BaPK;

public class HandTapHelpController : MonoBehaviour {

	// Use this for initialization
    public Label    tapAndHoldLabel;
	void Start () {
        tapAndHoldLabel.GetComponent<New1FontRead>().New1Read("12", 1, TextAlignment.Center, FactoryScenesController.languageHungBV["TAPANDHOLD"], 0f, 10f);
        SetIDLayer(12);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    void SetIDLayer(int sortingLayerID)
    {
        Transform[] transforms = gameObject.GetComponentsInChildren<Transform>(true);

        for (int i = 0; i < transforms.Length; i++)
        {
            GameObject gObject = transforms[i].gameObject;
            if (gObject.GetComponent<SpriteRenderer>() != null)
            {
                gObject.GetComponent<SpriteRenderer>().sortingLayerID = sortingLayerID;
            }
        }
    }
}
