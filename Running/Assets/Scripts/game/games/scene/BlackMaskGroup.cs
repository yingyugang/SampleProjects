using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class BlackMaskGroup : MonoBehaviour {

	public GameObject blackMaskPrefab;

	public bool load;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(load){
			load = false;



		}
	}
}

[System.Serializable]
public class BlackMask{

	public Vector3 pos;
	public float angle;

}