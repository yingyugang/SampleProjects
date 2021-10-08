using UnityEngine;
using System.Collections;
using BaPK;

public class CountController : MonoBehaviour {

	// Use this for initialization
    public static Animator animator;
    public Label label;
    public int costMachine;
	void Start () {
        animator = GetComponent<Animator>();
        label.GetComponent<New3FontRead>().New3Read("ButtonBG2", 3, TextAlignment.Left, costMachine.ToString(), 0f, 0f);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    void Destroy()
    {
        Destroy(gameObject);
    }
}
