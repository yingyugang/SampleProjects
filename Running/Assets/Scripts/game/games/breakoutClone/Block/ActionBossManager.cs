using UnityEngine;
using System.Collections;

public class ActionBossManager : MonoBehaviour {

	private static ActionBossManager _instance;

	public static ActionBossManager instance {
		get {
			if (_instance == null)
				_instance = GameObject.FindObjectOfType<ActionBossManager> ();
			return _instance;
		}
	}

	public float 		SpeedBoss;
	public Transform 	CenterPos;
	public ActionBoss[] ActionController;

	public void SetBossAction(int id,int type, int x, int y){
		for (int i = 0; i < ActionController.Length; i++) {
			if(i == id -1){
				ActionController [i].Id = id;
				ActionController [i].Type = type;
				ActionController [i].X_Percent = x;
				ActionController [i].Y_Time = y;
			}
		}
	}

	public ActionBoss GetActionById(int id){
		foreach(ActionBoss action in ActionController){
			if(action.Id == id) return action;
		}
		return null;
	}
}
