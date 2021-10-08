using UnityEngine;
using System.Collections;

public class Game12_Ground : MonoBehaviour {

	public Transform sPlayer;
	
	// Update is called once per frame
	void Update () {
		transform.position = new Vector3 (sPlayer.position.x,transform.position.y,transform.position.z);
	}
}
