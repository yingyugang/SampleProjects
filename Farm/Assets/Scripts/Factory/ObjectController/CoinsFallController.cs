using UnityEngine;
using System.Collections;
using BaPK;

public class CoinsFallController : MonoBehaviour {

	// Use this for initialization
    public Label label;
    public int value;
	void Start () {
       label.GetComponent<New3FontRead>().New3Read("ButtonBG2", 1, TextAlignment.Left, (-value).ToString(), 0f, 0f);
      // label.GetComponent<MoneyFontRead>().MoneyRead("ButtonBG2", 1, TextAlignment.Left,"assssss"  + "aaaaaaaa", 0f, 0f);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void Destroy()
    {
        Destroy(gameObject);
    }
}
