using UnityEngine;
using System.Collections;

public class ChangeScene : MonoBehaviour {

	public void StartGame(){
		Debug.Log ("startingGame");
		Application.LoadLevel ("scene");

	
	}



	void Update(){
		if (Input.GetMouseButtonDown (0)) {
			StartGame();
		}

	}
}
