using UnityEngine;
using System.Collections;

public class RacketCollision : MonoBehaviour {

	//Change Velocity Ball
	void OnCollisionEnter2D (Collision2D col)
	{
		if (col.gameObject.tag == BreackoutConfig.TAG_PLAYER) {
			Game8_Manager.instance.PlaySound (SoundEnum.SE22_oden_bound);
			float x = hitFactor (col.transform.position, transform.position, GetComponent<BoxCollider2D> ().size.x);
			Vector2 dir = new Vector2 (x, 1).normalized;
			col.gameObject.GetComponent<Rigidbody2D> ().velocity = dir * col.gameObject.GetComponent<BallController> ().SpeedBall*BreackoutConfig.SPEEDBALL;
		}

	}
	//--> Hit factor Ball when collision racket
	float hitFactor (Vector2 ballPos, Vector2 racketPos, float racketWidth)
	{
		return (ballPos.x - racketPos.x) / racketWidth;
	}
}
