using UnityEngine;
using System.Collections;
using DG.Tweening;
public class Game12_Root : MonoBehaviour {

	void Awake(){
		gameObject.AddComponent<Rigidbody2D>().velocity = new Vector2(Random.Range(5.0f, 8.0f), Random.Range(10.0f, 20.0f));
		gameObject.GetComponent<Rigidbody2D>().AddForce(Random.onUnitSphere, ForceMode2D.Impulse);
		transform.DORotate(new Vector3(.0f, .0f, Random.Range(180, 300)), Random.Range(1.0f, 1.5f));
	}
	void Start(){
		Destroy(gameObject, 3.0f);
	}
}
