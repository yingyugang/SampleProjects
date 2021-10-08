using UnityEngine;
using System.Collections;
using DG.Tweening;
public class Game12_Player_Animated : MonoBehaviour {
	public bool isDieByAbyss;
	void Awake(){

	}
	void Start(){
		Destroy(gameObject, 3.0f);
		gameObject.AddComponent<Rigidbody2D>().velocity = new Vector2(2.0f, .0f);
		if(!isDieByAbyss){
			gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(Random.Range(5.0f, 9.0f), Random.Range(14.0f, 20.0f));
			gameObject.GetComponent<Rigidbody2D>().AddForce(Random.onUnitSphere, ForceMode2D.Impulse);
			transform.DORotate(new Vector3(.0f, .0f, Random.Range(1080.0f, 1920.0f)), Random.Range(1.50f, 2.80f), RotateMode.FastBeyond360);
		}
	}
}
