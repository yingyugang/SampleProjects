using UnityEngine;
using System.Collections;
using BaPK;

public class NameMachineController : MonoBehaviour {

	// Use this for initialization
    public Label nameMachineLabel;

	void Start () {
        nameMachineLabel.GetComponent<New1FontRead>().New1Read("12", 1, TextAlignment.Center, MachineController.machineSelect.GetComponent<MachineController>().nameMachine, 0f, 10f);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
