using UnityEngine;
using System.Collections;

public class BorderManager : MonoBehaviour {
	void OnCollisionEnter2D(Collision2D other){
		if (other.gameObject.tag == BreackoutConfig.TAG_PLAYER) {
			Vector2 playerVelocity = other.gameObject.GetComponent<Rigidbody2D> ().velocity;
			Game8_Manager.instance.PlaySound (SoundEnum.SE22_oden_bound);
			if (playerVelocity.y < 1.5f && playerVelocity.y > -1.5f) {
				playerVelocity.y = Mathf.Sign (playerVelocity.y) * 1.5f;
			}
			other.gameObject.GetComponent<Rigidbody2D> ().velocity = playerVelocity;
		}
		
	}
}
