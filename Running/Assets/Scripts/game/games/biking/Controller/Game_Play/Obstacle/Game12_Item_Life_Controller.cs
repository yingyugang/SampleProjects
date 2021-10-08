using UnityEngine;
using System.Collections;

public class Game12_Item_Life_Controller : MonoBehaviour
{
	//hard code wating player with item
	public Game12_Item_Life itemLife;
	public static Game12_Item_Life_Controller instance;
	bool isWatingCreatePlayer = false;

	public static float percentageCreateItem = 0;
	public static float reductionPercentageItem = 0;
	void Awake(){
		instance = this;
	}
	void Start ()
	{
		percentageCreateItem = float.Parse (Game12_GameParams.instance.terrainItemData [0] ["percentage"].ToString ());
		reductionPercentageItem = float.Parse (Game12_GameParams.instance.terrainItemData [0] ["reduction_percentage"].ToString ());
		itemLife.gameObject.SetActive (false);
	}

	public void CreateNewItemIsWatingWithPlayer ()
	{
		//create item wating player move to postion detect
		itemLife.gameObject.transform.position = Game12_Player_Controller.instance.Player.transform.position + new Vector3 (BikingKey.GameConfig.DistanceItemLife * 0.5f, 30f, 0);
		isWatingCreatePlayer = true;
		itemLife.gameObject.SetActive (false);
	}

	void NoUpdate ()
	{
		//Debug.Log (Game12_GameManager.instance.Distance - itemLife.distanceCurrent);
		if ((Game12_GameManager.instance.Distance - itemLife.distanceCurrent) >= BikingKey.GameConfig.DistanceExistsLife
		    && isWatingCreatePlayer == false) {
			CreateNewItemIsWatingWithPlayer ();
			//Debug.Log ("CreateNewItemLife " + Game12_Player_Controller.life_player);
		}

		//check position wating create player
		if (isWatingCreatePlayer) {
			if (Mathf.Abs (Game12_Player_Controller.instance.Player.transform.position.x - itemLife.gameObject.transform.position.x) <= 25) {
				isWatingCreatePlayer = false;
				itemLife.CreateNewItemLife ();
			}
		}
		//check hide item reborn
		if (itemLife.transform.position.y <= -100) {
			itemLife.gameObject.SetActive (false);
		}
	}
}
