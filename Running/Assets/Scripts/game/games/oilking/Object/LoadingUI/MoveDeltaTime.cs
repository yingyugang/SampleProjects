using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MoveDeltaTime : MonoBehaviour {

	public Color rayColor = Color.white;
	public List<Transform> path_objs = new List<Transform> ();
	Transform[] theArrays;

	void OnDrawGizmos(){
		Gizmos.color = rayColor;
		theArrays = GetComponentsInChildren<Transform> ();
		path_objs.Clear ();

		foreach (Transform path_Obj in theArrays) {
			if (path_Obj != this.transform) {
				path_objs.Add (path_Obj);
			}
		}

		for (int i = 0; i < path_objs.Count; i++) {
			Vector3 position = path_objs [i].position;
			if (i > 0) {
				Vector3 previous = path_objs [i - 1].position;
				Gizmos.DrawLine (previous, position);
				Gizmos.DrawSphere (position, 0.3f);
			}
		}
	}
}
