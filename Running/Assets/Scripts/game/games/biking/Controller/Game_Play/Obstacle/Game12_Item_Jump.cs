using UnityEngine;
using System.Collections;

public class Game12_Item_Jump : MonoBehaviour {

	void OnTriggerExit2D(Collider2D other){
		if(other.gameObject.tag == "Player"){
			Game12_Player_Controller.instance.IsSpring = false;
			transform.GetComponent<SpriteRenderer>().sprite = Terrain_Generation.instance.ObstacleSprite[6];
		}
	}
	void OnTriggerEnter2D(Collider2D other){
		if (other.gameObject.tag == "Player") {
			transform.GetComponent<SpriteRenderer> ().sprite = Terrain_Generation.instance.ObstacleSpring_Sprite;
			Game12_Player_Controller.instance.IsSpring = true;
			Game12_Player_Controller.instance.IsJump = false;
			Game12_Player_Controller.instance.IsAir = false;
			Game12_Player_Controller.instance.IsDoubleJump = false;
			//Game12_GUI_Manager.instance.TouchArea.SetActive (false);
			return;
		}
	}
}
