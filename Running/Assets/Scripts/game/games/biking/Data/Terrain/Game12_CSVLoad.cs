using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Game12_CSVLoad : MonoBehaviour {
	void Awake () {
		LoadCSV();
	}
	void LoadCSV(){
		Game12_GameParams.instance.terrainData = CSVReader.Read ("csv\\m_bike_dash_map_detail");
		Game12_GameParams.instance.terrainItemData = CSVReader.Read ("csv\\m_bike_dash_item");
		Game12_GameParams.instance.dashMap = CSVReader.Read ("csv\\m_bike_dash_map");
		Game12_GameParams.instance.terrainAbyssData = CSVReader.Read ("csv\\m_bike_dash_background");
		Game12_GameParams.instance.characterData = CSVReader.Read ("csv\\m_bike_dash_character");
		ConvertTerrainData();
		LoadData();
		Game12_GameParams.instance.LoadItemSetting ();
		LoadBikeCheatSprite ();
	}


	void AddEmplPattern(int idMap, int index, float start_x, float start_y, float end_x, float end_y ){
		Dictionary<string,object> clone = new Dictionary<string, object>(Game12_GameParams.instance.terrainData[0]);
		clone["m_bike_dash_map_id"] = idMap;
		clone["index"] = index;
		clone["start_x"] = start_x;
		clone["start_y"] = start_y;
		clone["end_x"] = end_x;
		clone["end_y"] = end_y;
		clone["m_bike_dash_item_id"] = 0;
		clone["item_appear_percentage_supplementary"] = 0;
		Game12_GameParams.instance.terrainData.Insert(0, clone);
	}

	void LoadBikeCheatSprite(){
		List<Dictionary<string, object>> data = CSVReader.Read("csv\\m_bike_cheat_sprite");
		if(data!=null){
			Dictionary<string, BikeCheatData> cheatDataDic = new Dictionary<string, BikeCheatData> ();
			for(int i=0;i<data.Count;i++){
				BikeCheatData bikeCheatData = new BikeCheatData ();
				bikeCheatData.cheatKey = GetString (data[i]["key"]);
				bikeCheatData.headSprite = GetString (data[i]["head"]);
				bikeCheatData.headSprite1 = GetString (data[i]["head1"]);
				bikeCheatData.bikeSprite = GetString (data[i]["bike"]);
				bikeCheatData.bikeSprite1 = GetString (data[i]["bike1"]);
				bikeCheatData.lifeSprite = GetString (data[i]["life"]);
				bikeCheatData.lifeSprite1 = GetString (data[i]["life1"]);
				if(!cheatDataDic.ContainsKey(bikeCheatData.cheatKey)){
					cheatDataDic.Add (bikeCheatData.cheatKey,bikeCheatData);
				}
			}
			Game12_GameParams.instance.bikingCheatDic = cheatDataDic;
		}
	}

	string GetString(System.Object obj){
		return obj.ToString ().Trim ();
	}

	private void ConvertTerrainData(){
		//-->Add Map Terrain
		AddEmplPattern(1112, 0, 0, 15, 100, 15);
		AddEmplPattern(1111, 1, 1, 30, 1, 0);
		AddEmplPattern(1111, 0, 0, 30, 1, 30);
		//--<
		//return;
		Debug.Log ("terrainData.Count = " + Game12_GameParams.instance.terrainData.Count);
		int idCheck = -1;
		int count = 0;
		for(int i = 0; i < Game12_GameParams.instance.terrainData.Count; i++){
			int id = (int)Game12_GameParams.instance.terrainData[i]["m_bike_dash_map_id"];
			if(idCheck < 0) 
				idCheck = id;
			if(id != idCheck){
				if(count < BikingKey.Terrain.CSVMeshPoint){
					AddFullPoint(i-1,count);
				}
				i += (BikingKey.Terrain.CSVMeshPoint - count);
				idCheck = id;
				count = 0;
			}
			count++;
		}
		if(count < BikingKey.Terrain.CSVMeshPoint) AddFullPoint(Game12_GameParams.instance.terrainData.Count - 1, count);
	}

	void AddFullPoint(int index,int count){
	//	Debug.Log ("Automatic fill map for " + index);
		for (int i = 0; i < (BikingKey.Terrain.CSVMeshPoint - count); i++) {
			Dictionary<string,object> clone = new Dictionary<string, object>(Game12_GameParams.instance.terrainData[index]);
			clone["m_bike_dash_item_id"] = 0;
			Game12_GameParams.instance.terrainData.Insert(index, clone);
		}
	}

	private void LoadData(){
		int count = 0;
		//--> Load mesh
		List<int> id = new List<int>();
		List<int> mapID = new List<int>();
		List<int> index = new List<int>();
		List<Vector2> points = new List<Vector2>();
		List<KeyValuePair<int, Vector2>> itemPattern = new List<KeyValuePair<int, Vector2>>();

		List<int> itemPercent = new List<int>();
		for(int i = 0; i < Game12_GameParams.instance.terrainData.Count; i++){
			count++;
			// Id
			id.Add((int)Game12_GameParams.instance.terrainData[i]["id"]);
			//Debug.Log (id[i]);
			// Map Id
			mapID.Add((int)Game12_GameParams.instance.terrainData[i]["m_bike_dash_map_id"]);
			// Map Id
			index.Add((int)Game12_GameParams.instance.terrainData[i]["index"]);
			// Vector
			Vector2 start, end;
			start = new Vector2(float.Parse(Game12_GameParams.instance.terrainData[i]["start_x"].ToString()), float.Parse(Game12_GameParams.instance.terrainData[i]["start_y"].ToString()));
			points.Add(start);
			end = new Vector2(float.Parse(Game12_GameParams.instance.terrainData[i]["end_x"].ToString()), float.Parse( Game12_GameParams.instance.terrainData[i]["end_y"].ToString()));
			points.Add(end);
			// Item
			KeyValuePair<int, Vector2> items = new KeyValuePair<int, Vector2>((int)Game12_GameParams.instance.terrainData[i]["m_bike_dash_item_id"],
				new Vector2(
					float.Parse(Game12_GameParams.instance.terrainData[i]["item_relative_x"].ToString()), 
					float.Parse(Game12_GameParams.instance.terrainData[i]["item_relative_y"].ToString()))  + start);
			itemPattern.Add(items);

			itemPercent.Add((int)Game12_GameParams.instance.terrainData[i]["item_appear_percentage_supplementary"]);

			if(count == BikingKey.Terrain.CSVMeshPoint || i == Game12_GameParams.instance.terrainData.Count -1){
//				Debug.Log ("LoadTerrainDataToList " + i);
				LoadTerrainDataToList(id, mapID, index, points, itemPattern, itemPercent);
				count = 0;
				id.Clear();
				mapID.Clear();
				points.Clear();
				itemPattern.Clear();
				itemPercent.Clear ();
			}

		}
		//--< Load mesh
	}
	private void LoadTerrainDataToList(List<int> id, List<int> mapID, List<int> index, List<Vector2> vector, List<KeyValuePair<int, Vector2>> itemPattern, List<int> itemPercent){
		Pattern_Info patternInfo = new Pattern_Info();
		for(int i = 0; i < id.Count; i++){
			patternInfo.id[i] = id[i];
			patternInfo.map_id[i] = mapID[i];
			patternInfo.index[i] = index[i];
		}
		Vector2 mCurrent_point = new Vector2 (0f, 0f);
		Vector2 mLast_point = new Vector2 (-100000f, -100000f);
		for(int i = 0; i < vector.Count; i++){
//			patternInfo.points.Add(vector[i] * BikingKey.Terrain.cordinateMulti);
			mCurrent_point = new Vector2(vector[i].x * BikingKey.Terrain.CordinateMulti, vector[i].y * BikingKey.Terrain.CordinateMulti);
//			if (mCurrent_point == mLast_point)
//				Debug.Log ("Double point : " + vector[i]);
//			else {
//				mLast_point = mCurrent_point;
				//Debug.Log (mCurrent_point);
//			}
			patternInfo.points.Add (mCurrent_point);
		}
		//Debug.Log (itemPattern.Count);
		//Debug.Log (itemPercent.Count);
		for(int i = 0; i < itemPattern.Count; i++){
			patternInfo.itemPattern.Add(itemPattern[i]);
			//Debug.Log (itemPercent[i]);
			patternInfo.itemPercent[i] = itemPercent[i];
		}
		Game12_GameParams.instance.patternList.Add(patternInfo);
	}
	private float GetFloat(string stringValue, float defaultValue){
		float result = defaultValue;
		float.TryParse(stringValue, out result);
		return result;
	}
}
