using UnityEngine;
using System.Collections;
using BaPK;

public class TestFontControl : MonoBehaviour {

    public Label[] label;
	void Start () {
       // SocialFontRead scr = new SocialFontRead();
        Contructor();
	}

    void Contructor()
    {
        for (int i = 0; i < label.Length; i ++ )
        {

            label[i].GetComponent<SocialFontRead>().SocialRead("ButtonBG2", 1, TextAlignment.Left, "1111111", 0f, 0f);

        }
       
    }
}
