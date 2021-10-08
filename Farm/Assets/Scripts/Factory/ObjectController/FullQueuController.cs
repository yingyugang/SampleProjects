using UnityEngine;
using System.Collections;
using BaPK;

public class FullQueuController : MonoBehaviour {

    public Label fullQueuLabel;
    public string text;
	void Start () {
      
        fullQueuLabel.GetComponent<New1FontRead>().New1Read("12", 1, TextAlignment.Center,text, 0f, 10f);
       
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    void Destroy()
    {
        Destroy(this.gameObject);
    }
}
