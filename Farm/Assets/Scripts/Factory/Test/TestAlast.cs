using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TestAlast : MonoBehaviour {

	// Use this for initialization
    private UISprite [] listAlast;
	void Start () {
        listAlast = Resources.LoadAll<UISprite>("Factory/Test/ProductTest");
	}
	
	// Update is called once per frame
	void Update () {
        print(listAlast.Length);
	}
}
