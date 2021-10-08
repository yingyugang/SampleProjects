using UnityEngine;
using System.Collections;

public class BotSpawner : MonoBehaviour {
	public Transform[]spawns = new Transform[2];
	GuiHandler gui;
	void Start(){
		gui = FindObjectOfType<GuiHandler> ();
	}

	public void SpawnBot(Transform obj){
		gui.Score++;
		Vector3 rot = obj.eulerAngles;
		int spawnNumber = Random.Range (0, spawns.Length);
		obj.position = spawns [spawnNumber].position;


	}
}
