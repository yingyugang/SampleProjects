using UnityEngine;
using System.Collections;

public class Game12_Item_Life : MonoBehaviour
{
	public SpriteRenderer mySprite;
	public Sprite[] lstLifeSprite;
	//public Sprite[] cheatLifeSprite;
	public Game12_Character categoryItem = Game12_Character.dekapan;
	public int distanceCurrent = 0;

	Rigidbody2D myRigibody;
	void OnEnable(){
		if (Game12_Player_Controller.life_player %2 == 1) {
			if (Game12_Player_Controller.instance.playerInfo.GetID () == (int)Game12_Character.dekapan) {
				categoryItem = Game12_Character.dayon;
				mySprite.sprite = lstLifeSprite [1];
			} else {
				categoryItem = Game12_Character.dekapan;
				mySprite.sprite = lstLifeSprite [0];
			}
			//if(CheatController.IsCheated(0) && cheatLifeSprite.Length > 0){
			//	mySprite.sprite = cheatLifeSprite [Random.Range(0,cheatLifeSprite.Length)];
			//}
		}
	}

	void ResetHitchHike(){
		int Currentpattern = Terrain_Controller.instance.currentPattern.ID;
		Debug.Log ("ResetHitchHike in " + Currentpattern);
		Terrain_Controller.instance.Routes [Currentpattern].GetComponent<Terrain_Elmp>().ShowHitchhike();
	}

	void Update(){
		if (transform.position.x < Game12_Player_Controller.instance.transform.position.x -8f){
			Debug.LogError ("Lifeitem out from view range");
			gameObject.SetActive(false);
			Invoke ("ResetHitchHike",0.1f);
		}
	}

	public void CreateNewItemLife ()
	{
		distanceCurrent = (int)Game12_GameManager.instance.Distance;
		Game12_Item_Life_Controller.percentageCreateItem -= Game12_Item_Life_Controller.reductionPercentageItem;
//		Debug.Log (Game12_Item_Life_Controller.percentageCreateItem + " " + Game12_Item_Life_Controller.reductionPercentageItem);
		//create item insert life in 100m
		if (Game12_Player_Controller.life_player == 1) {
			int rdlife = Random.Range (0, 100);
//			Debug.Log ("Game Params Life " + gameparamsLife);
			if (rdlife <= Game12_Item_Life_Controller.percentageCreateItem) {

				//create item
				gameObject.SetActive (true);
				if (myRigibody == null) {
					myRigibody = gameObject.GetComponent<Rigidbody2D> ();
				}
				myRigibody.velocity = Vector3.zero;
				myRigibody.angularVelocity = 0;

				if (Game12_Player_Controller.life_player == 1) {
					if (Game12_Player_Controller.instance.playerInfo.GetID () == (int)Game12_Character.dekapan) {
						categoryItem = Game12_Character.dayon;
						mySprite.sprite = lstLifeSprite [1];
					} else {
						categoryItem = Game12_Character.dekapan;
						mySprite.sprite = lstLifeSprite [0];
					}
					//if (CheatController.IsCheated (0) && cheatLifeSprite.Length > 0) {
					//	mySprite.sprite = lstLifeSprite [Random.Range (0, cheatLifeSprite.Length)];
					//}
				}
			}
		}
	}


}
