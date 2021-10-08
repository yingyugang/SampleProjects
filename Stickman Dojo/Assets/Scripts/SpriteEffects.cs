using UnityEngine;
using System.Collections;

public class SpriteEffects : MonoBehaviour {
public SpriteRenderer[] spritesEffects ;
	// Use this for initialization

	void Start(){
		spritesEffects = GetComponentsInChildren<SpriteRenderer> ();
	}

}
