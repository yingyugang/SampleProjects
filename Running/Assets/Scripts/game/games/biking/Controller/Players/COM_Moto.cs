using UnityEngine;
using System.Collections;
using DG.Tweening;
public class COM_Moto : MonoBehaviour {

	public bool IsGrounded = false;
	void Awake(){
	}
//	void FixedUpdate(){
//		transform.GetComponent<Rigidbody2D>().AddForce(-transform.up * 100);
//	}

	void OnCollisionEnter2D(Collision2D coll) {
		IsGrounded = true;
	}
	void OnCollisionStay2D(Collision2D coll) {
		IsGrounded = true;
	}
	void OnCollisionExit2D(Collision2D coll){
		IsGrounded = false;
	}

	/*
	void OnTriggerEnter2D(Collider2D other){
		IsGrounded = true;
	}
	void OnTriggerStay2D(Collider2D other){
		IsGrounded = true;
	}
	void OnTriggerExit2D(Collider2D other){
		IsGrounded = false;
	}
	*/
}
