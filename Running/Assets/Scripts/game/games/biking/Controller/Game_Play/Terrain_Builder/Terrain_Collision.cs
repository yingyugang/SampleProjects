using UnityEngine;
using System.Collections;

public class Terrain_Collision : MonoBehaviour {
	[HideInInspector]public bool callID = true;
	void OnCollisionEnter2D(Collision2D coll) {
		if (coll.gameObject.CompareTag(BikingKey.Tags.Player)){
			if(callID){
				Terrain_Controller.instance.PatternID = transform.parent.parent.GetComponent<Terrain_Elmp>().ID;
				callID = false;
			}
		}
	}
}
