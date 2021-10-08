using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Game12_GameParams : MonoBehaviour {
	public static Game12_GameParams m_instance;
	public static Game12_GameParams instance{
		get {
			if (m_instance == null)
				m_instance = GameObject.FindObjectOfType<Game12_GameParams> ();
			return m_instance;
		}
	}
	public Game12_Item_Properties[] sItemProperties;
	//--> Character CSV local data
	public List<Dictionary<string,object>> characterData = new List<Dictionary<string,object>>();
	//--< Character CSV local data

	//--> Terrain CSV local data
	public List<Dictionary<string,object>> terrainData = new List<Dictionary<string,object>>();
	public List<Pattern_Info> patternList = new List<Pattern_Info>();
	//--< Character CSV local data

	//--> Terrain Item CSV local data
	public List<Dictionary<string,object>> terrainItemData = new List<Dictionary<string,object>>();
	//--< Character CSV local data

	//--> Terrain Abyss CSV local data
	public List<Dictionary<string,object>> terrainAbyssData = new List<Dictionary<string,object>>();
	//--< Items CSV local data

	//--> Terrain Item CSV local data
	public List<Dictionary<string,object>> dashMap = new List<Dictionary<string,object>>();
	//--< Character CSV local data
	public Dictionary<string,BikeCheatData> bikingCheatDic = new Dictionary<string, BikeCheatData>();

	public void LoadItemSetting(){
		Debug.Log ("LoadItemSetting");
		sItemProperties = new Game12_Item_Properties[BikingKey.GameConfig.MAX_ITEM_ID + 1];
		for (int i = 0; i < terrainItemData.Count; i++) {
			int ID = ((int)terrainItemData [i] ["id"]);
			if (ID <= BikingKey.GameConfig.MAX_ITEM_ID) {
				sItemProperties [ID] = new Game12_Item_Properties();
				sItemProperties [ID].m_ID = ID;
				sItemProperties [ID].m_Name = ((string)terrainItemData [i] ["name"]);
				sItemProperties [ID].m_ImageSource = ((string)terrainItemData [i] ["image_resource"]);
				sItemProperties [ID].m_Type = ((int)terrainItemData [i] ["item_type"]);
				sItemProperties [ID].m_Effect_Value1 = (float.Parse (terrainItemData [i] ["effect_value1"].ToString ()));
				sItemProperties [ID].m_Effect_Value2 = (float.Parse (terrainItemData [i] ["effect_value2"].ToString ()));
				sItemProperties [ID].m_Percentage = ((int)terrainItemData [i] ["percentage"]);
				sItemProperties [ID].m_PercentageReduction = (((int)terrainItemData [i] ["reduction_percentage"]));
			}
		}
	}




	void Awake(){
		m_instance = this;
	}
}

[System.Serializable]
public class BikeCheatData{

	public string cheatKey;
	public string headSprite;
	public string headSprite1;
	public string lifeSprite;
	public string lifeSprite1;
	public string bikeSprite;
	public string bikeSprite1;

}
