using UnityEngine;
using System.Collections;

public class TestParticle : MonoBehaviour {

	// Use this for initialization
	void Start () {
        GetComponent<ParticleSystem>().GetComponent<Renderer>().sortingLayerID = 11;
        GetComponent<ParticleSystem>().GetComponent<Renderer>().sortingOrder = 3;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
