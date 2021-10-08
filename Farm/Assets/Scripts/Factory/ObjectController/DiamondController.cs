using UnityEngine;
using System.Collections;
using BaPK;

public class DiamondController : MonoBehaviour
{

    // Use this for initialization
    public GameObject Diamond;
    AudioControl audioControl;
    public Label label;
    void Start()
    {
        label.GetComponent<New3FontRead>().New3Read("ButtonBG2", 1, TextAlignment.Left, "-1", 0f, 0f);
        audioControl = GameObject.Find("AudioControl").GetComponent<AudioControl>();
        audioControl.PlaySound("Kim cuong roi xuong");
    }

    // Update is called once per frame
    void Update()
    {

    }
    void Destroy()
    {

        Destroy(gameObject);
    }
    void DestroyPre()
    {

        Destroy(Diamond);
        ButtonPayController.isEfect = true;
    }
}
