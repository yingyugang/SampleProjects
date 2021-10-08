using UnityEngine;
using System.Collections;
//using System.;
public class Game12_Item_Component : MonoBehaviour {

	public Game12_Item_Properties itemProperties = new Game12_Item_Properties();

	int RandomItemWithPercent (int [] ItemList, int [] ItemPercent){
		int Total = 0;
		for (int i = 0; i < ItemList.Length; i++)
			Total += ItemPercent [i];
		for (int i = 0; i < ItemList.Length; i++) {
			int percent = Random.Range (0, Total);
			if (percent < ItemPercent [i])
				return ItemList [i];
		}
		return ItemList[ItemList.Length - 1];
	}
	public void ShowItem(){
		//isTapOnSprings = false;
		if (itemProperties.m_ID == 1) {
			if (Game12_Player_Controller.life_player > 1)
				return;
			if (Game12_Item_Life_Controller.instance.itemLife.gameObject.activeInHierarchy)
				return;
			//Game12_Player_Controller.instance.li
			float dis = Mathf.Abs(transform.position.x - Game12_Player_Controller.instance.transform.position.x);
			Debug.Log ("Distance = " + dis);
			if (dis > 10f) {
				Debug.Log ("item ID = " + itemProperties.m_ID);
				Game12_Item_Life_Controller.instance.itemLife.gameObject.transform.position = gameObject.transform.position;
				Game12_Item_Life_Controller.instance.itemLife.gameObject.SetActive (
					Random.Range (0, 100) < (itemProperties.GetPercentage () * Game12_Player_Controller.instance.playerInfo.GetItemRate ())); 
			}
		} else {
			gameObject.SetActive (false);
			if (itemProperties.m_ID == 11) {
				int []randomItemList = {3,8,9};
				int []randomItemPercent = {50,40,10}; //default
				randomItemPercent[0] = Game12_GameParams.instance.sItemProperties[3].m_Percentage;
				randomItemPercent[1] = Game12_GameParams.instance.sItemProperties[8].m_Percentage;
				randomItemPercent[2] = Game12_GameParams.instance.sItemProperties[9].m_Percentage;
				int itemID = RandomItemWithPercent(randomItemList, randomItemPercent);
//				Debug.Log ("Random scoreitem = " + itemID);
				ReloadProperties (itemID);
				GetComponent<SpriteRenderer> ().sprite = Game12_Item_Manager.instance.GetSpriteFromID (itemID);
			}

			gameObject.SetActive (Random.Range (0, 100) < (itemProperties.GetPercentage () * Game12_Player_Controller.instance.playerInfo.GetItemRate ()));
		}
	}
	void ReloadProperties(int itemID){
		Game12_Item_Properties temp = Game12_GameParams.instance.sItemProperties[itemID];
		itemProperties.m_Name = temp.m_Name;
		itemProperties.m_ImageSource = temp.m_ImageSource;
		itemProperties.m_Type = temp.m_Type;
		itemProperties.m_Effect_Value1 = temp.m_Effect_Value1;
		itemProperties.m_Effect_Value2 = temp.m_Effect_Value2;
		itemProperties.m_PercentageReduction = temp.m_PercentageReduction;

	}
	public void GetItemProperties(int itemID, int percent){

		if(itemID == 101){
			itemProperties.SetID(101); itemProperties.SetName("Abyss"); itemProperties.SetImageSource("None"); itemProperties.SetItemType(101); itemProperties.SetEffectValue1(101.0f);
			itemProperties.SetEffectValue2(101.0f); itemProperties.SetPercentage(100); itemProperties.SetPercentageReduction(0); itemProperties.SetDescription("This is just Abyss");
		}
		else{
			itemProperties.m_ID = itemID;
			ReloadProperties (itemID);
			itemProperties.m_Percentage = Game12_GameParams.instance.sItemProperties [itemID].m_Percentage;
			itemProperties.m_Percentage += percent;
			if (itemID == 1)
				Debug.Log (itemProperties.m_Percentage);
		}
	}

	private int[] m_Obstacle_transform = { 0,0,0,0,0,0,2,1};
	void OnTriggerEnter2D(Collider2D other){
		if(other.gameObject.tag == "Player"){
			int id = itemProperties.GetID ();
			if (!Game12_Player_Controller.instance.IsTitan)
				return;
			if (id > 4 && id < 8) {
				Game12_Item_Manager.instance.ObstacleExplorerOnTitanPower(transform, m_Obstacle_transform[id]);
				transform.gameObject.SetActive(false);
			}
		}
	}
}
