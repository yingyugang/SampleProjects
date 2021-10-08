using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace SixRun{

	public class GameParams : SixRunSingleMono<GameParams> {
		public const string CSV_BACKGROUND = "csv/m_six_run_background";
		public const string CSV_ITEM = "csv/m_six_run_item";
		public const string CSV_MAP = "csv/m_six_run_map";
		public const string CSV_MAPDETAIL = "csv/m_six_run_map_detail";
		public const string CSV_PARAMATER = "csv/m_six_run_parameter";
		public const string CSV_RANK = "csv/m_six_run_rank";

		public float moveSpeed = 10;
		public RankData[] rankDatas; 
		//public ParameterData[] parameterDatas;
		public Dictionary<string,ParameterData> parameterDataDic;
		public List<BackgroundData> backgroundDatas;
		public ItemData[] itemDatas;
		public Dictionary<int,ItemData> itemDataDic;
		public Dictionary<string,ItemData> itemDataDic1;
		public List<MapData> mapDatas;
		public Dictionary<int,MapData> mapDataDic;
		public MapDetailData[] mapDetailDatas;

		protected override void Awake(){
			base.Awake ();
			LoadCsvData();
		}

		void LoadCsvData(){
			LoadCSVRank ();
			//LoadServerParameter ();
			LoadCSVParameter ();
			LoadCSVItem ();
			LoadCSVBackground ();
			LoadCSVMap ();
			LoadCSVMapDetail ();
			//InitMapData ();
		}

		int GetInt(System.Object obj){
			int value = 0;
			int.TryParse (obj.ToString().Trim(), out value);
			return value;
		}

		string GetString(System.Object obj){
			return obj.ToString ().Trim ();
		}

		float GetFloat(System.Object obj){
			float value = 0;
			float.TryParse (obj.ToString().Trim(), out value);
			return value;
		}

		/*
		void InitMapData(){
			for(int i=0;i<mapDatas.Count;i++){
				MapData mapData = mapDatas[i];
				int meter = mapData.appear_distance_end - mapData.appear_distance;
				if(mapData.details!=null && mapData.details.Count >0){
					List<MapDetailData> detailDatas = new List<MapDetailData> ();
					detailDatas.AddRange (mapData.details);
					int endMeter = mapData.details [mapData.details.Count - 1].sub_distance;
					int count = meter % endMeter > 0 ? meter / endMeter + 1 : meter / endMeter;
					for(int j=1;j<count;j++){
						foreach(MapDetailData md in detailDatas){
							MapDetailData clone = MapDetailData.Clone (md);
							clone.sub_distance += j * (endMeter + 1);
							mapData.details.Add (clone);
						}
					}	
				}
			}
		}*/

		public int GetStartDistance(){
			if(GameParams.GetInstance ().parameterDataDic.ContainsKey("start_distance")){
				return GetInt (parameterDataDic ["start_distance"].value);
			}
			return 0;
		}

		public float GetToggleTime(){
			if(GameParams.GetInstance ().parameterDataDic.ContainsKey("comb_second")){
				return GetInt (parameterDataDic ["comb_second"].value);
			}
			return 3;
		}

		public int GetLife(){
			if(GameParams.GetInstance ().parameterDataDic.ContainsKey("game_time")){
				return GetInt (parameterDataDic ["game_time"].value);
			}
			return 100;
		}

		public float GetJumpHeight(){
			if(GameParams.GetInstance ().parameterDataDic.ContainsKey("jump_height")){
				return GetFloat (parameterDataDic ["jump_height"].value);
			}
			return 10;
		}

		public float GetJumpDuration(){
			Debug.Log ("GetJumpDuration");
			if(GameParams.GetInstance ().parameterDataDic.ContainsKey("jump_return")){
				return GetFloat (parameterDataDic ["jump_return"].value);
			}
			return 0.6f;
		}

		public float GetDoubleJumpHeight(){
			if(GameParams.GetInstance ().parameterDataDic.ContainsKey("shee_jump_height")){
				return GetFloat (parameterDataDic ["shee_jump_height"].value);
			}
			return 25;
		}

		public float GetDoubleJumpDuration(){
			if(GameParams.GetInstance ().parameterDataDic.ContainsKey("shee_jump_return")){
				return GetFloat (parameterDataDic ["shee_jump_return"].value);
			}
			return 0.6f;
		}

		public float GetDownHeight(){
			if(GameParams.GetInstance ().parameterDataDic.ContainsKey("down_height")){
				return GetFloat (parameterDataDic ["down_height"].value);
			}
			return 1;
		}

		public float GetDownDuration(){
			if(GameParams.GetInstance ().parameterDataDic.ContainsKey("sliding_return")){
				return GetFloat (parameterDataDic ["sliding_return"].value);
			}
			return 0.6f;
		}

		public float GetFlickDuration(){
			if(GameParams.GetInstance ().parameterDataDic.ContainsKey("flick_return")){
				return GetFloat (parameterDataDic ["flick_return"].value);
			}
			return 0.6f;
		}

		public float GetSpeedRate(){
			if(GameParams.GetInstance ().parameterDataDic.ContainsKey("player_speed_rate")){
				return GetFloat (parameterDataDic ["player_speed_rate"].value);
			}
			return 2;
		}

		public float GetFeverTime(){
			if(GameParams.GetInstance ().parameterDataDic.ContainsKey("fever_time")){
				return GetFloat (parameterDataDic ["fever_time"].value);
			}
			return 6;
		}

		public int GetSpeedUpMeter(){
			if(GameParams.GetInstance ().parameterDataDic.ContainsKey("speed_up_meters")){
				return GetInt (parameterDataDic ["speed_up_meters"].value);
			}
			return 100;
		}

		public int GetSpeedUpRate(){
			if(GameParams.GetInstance ().parameterDataDic.ContainsKey("speed_up_percentage")){
				return GetInt (parameterDataDic ["speed_up_percentage"].value);
			}
			return 10;
		}

		public float GetReverSpeed(){
			if(GameParams.GetInstance ().parameterDataDic.ContainsKey("river_speed") && GetFloat (parameterDataDic ["river_speed"].value) > 0){
				return GetFloat (parameterDataDic ["river_speed"].value);
			}
			return 3;
		}

		public int GetFlickSensitivity(){
			if(GameParams.GetInstance ().parameterDataDic.ContainsKey("flick_sensitivity") && GetInt (parameterDataDic ["flick_sensitivity"].value) > 0){
				return GetInt (parameterDataDic ["flick_sensitivity"].value);
			}
			return 20;
		}

		List<ItemData> mItems;
		int mTotalItemPercent;
		List<ItemData> GetItems(){
			if (mItems != null)
				return mItems;
			mItems = new List<ItemData> ();
			for(int i=0;i<itemDatas.Length;i++){
				if(itemDatas[i].effectType == 2){
					mItems.Add (itemDatas[i]);
					mTotalItemPercent += itemDatas [i].percentage;
				}
			}
			return mItems;
		}

		public ItemData GetRandomItem(){
			GetItems ();
			int total = mTotalItemPercent;
			for(int i=0;i<mItems.Count;i++){
				if(UnityEngine.Random.Range(0,mTotalItemPercent) < mItems[i].percentage){
					return mItems [i];
				}
				total -= mItems [i].percentage;
			}
			return mItems [mItems.Count - 1];
		}


		void LoadCSVRank(){
			List<Dictionary<string, object>> data = CSVReader.Read(CSV_RANK);
			if (data != null)
			{
				rankDatas = new RankData[data.Count];
				for (int i = 0; i < data.Count; i++)
				{
					RankData rank = new RankData();
					rank.id = GetInt(data [i] ["id"]);
					rank.rank = GetString(data [i] ["rank"]);
					rank.mixScore = GetInt(data [i] ["min"]);
					rank.maxScore = GetInt (data [i] ["max"]);
					rank.reward_item_id = GetInt(data [i] ["reward_item_id"]);
					rank.reward_num =GetInt(data [i] ["reward_num"]);
					rankDatas [i] = rank;
				}
			}
		}

		void LoadCSVParameter(){
			//parameterDatas = new ParameterData[data.Count];
			parameterDataDic = new Dictionary<string, ParameterData> ();
			if (APIInformation.GetInstance != null )
			{
				GameParameter parameter = APIInformation.GetInstance.gameparameter;
				ParameterData param = new ParameterData();
				param.name = "game_time";
				param.value = parameter.game_time.ToString();
				Debug.Log (param.value );
				parameterDataDic.Add (param.name.Trim(),param);
				Debug.Log ("param.name:" + param.name + ";" + "param.value:" + param.value);
				param = new ParameterData();
				param.name = "flick_sensitivity";
				param.value = parameter.flick_sensitivity.ToString();
				parameterDataDic.Add (param.name.Trim(),param);
				Debug.Log ("param.name:" + param.name + ";" + "param.value:" + param.value);
				param = new ParameterData();
				param.name = "start_distance";
				param.value = parameter.start_distance.ToString();
				parameterDataDic.Add (param.name.Trim(),param);
				Debug.Log ("param.name:" + param.name + ";" + "param.value:" + param.value);
				param = new ParameterData();
				param.name = "fever_time";
				param.value = parameter.fever_time.ToString();
				parameterDataDic.Add (param.name.Trim(),param);
				Debug.Log ("param.name:" + param.name + ";" + "param.value:" + param.value);
				param = new ParameterData();
				param.name = "player_speed_rate";
				param.value = parameter.player_speed_rate.ToString();
				parameterDataDic.Add (param.name.Trim(),param);
				Debug.Log ("param.name:" + param.name + ";" + "param.value:" + param.value);
				param = new ParameterData();
				param.name = "jump_height";
				param.value = parameter.jump_height.ToString();
				parameterDataDic.Add (param.name.Trim(),param);
				Debug.Log ("param.name:" + param.name + ";" + "param.value:" + param.value);
				param = new ParameterData();
				param.name = "jump_return";
				param.value = parameter.jump_return.ToString();
				parameterDataDic.Add (param.name.Trim(),param);
				Debug.Log ("param.name:" + param.name + ";" + "param.value:" + param.value);
				param = new ParameterData();
				param.name = "shee_jump_height";
				param.value = parameter.shee_jump_height.ToString();
				parameterDataDic.Add (param.name.Trim(),param);
				Debug.Log ("param.name:" + param.name + ";" + "param.value:" + param.value);
				param = new ParameterData();
				param.name = "shee_jump_return";
				param.value = parameter.shee_jump_return.ToString();
				parameterDataDic.Add (param.name.Trim(),param);
				Debug.Log ("param.name:" + param.name + ";" + "param.value:" + param.value);
				param = new ParameterData();
				param.name = "sliding_return";
				param.value = parameter.sliding_return.ToString();
				parameterDataDic.Add (param.name.Trim(),param);
				Debug.Log ("param.name:" + param.name + ";" + "param.value:" + param.value);
				param = new ParameterData();
				param.name = "flick_return";
				param.value = parameter.flick_return.ToString();
				parameterDataDic.Add (param.name.Trim(),param);
				Debug.Log ("param.name:" + param.name + ";" + "param.value:" + param.value);
				param = new ParameterData();
				param.name = "speed_up_meters";
				param.value = parameter.speed_up_meters.ToString();
				parameterDataDic.Add (param.name.Trim(),param);
				Debug.Log ("param.name:" + param.name + ";" + "param.value:" + param.value);
				param = new ParameterData();
				param.name = "speed_up_percentage";
				param.value = parameter.speed_up_percentage.ToString();
				parameterDataDic.Add (param.name.Trim(),param);
				Debug.Log ("param.name:" + param.name + ";" + "param.value:" + param.value);


				param = new ParameterData();
				param.name = "river_speed";
				param.value = parameter.river_speed.ToString();
				parameterDataDic.Add (param.name.Trim(),param);
				Debug.Log ("param.name:" + param.name + ";" + "param.value:" + param.value);

				param = new ParameterData();
				param.name = "comb_second";
				param.value = parameter.comb_second.ToString();
				parameterDataDic.Add (param.name.Trim(),param);
				Debug.Log ("param.name:" + param.name + ";" + "param.value:" + param.value);
			}
			List<Dictionary<string, object>> data = CSVReader.Read(CSV_PARAMATER);
			if (data != null)
			{
				
				for (int i = 0; i < data.Count; i++)
				{
					ParameterData param = new ParameterData();
					//param.id =GetInt(data[i]["id"]);
					param.name = GetString(data[i]["name"]);
					param.value = GetString(data[i]["value"]);
					param.description = GetString(data[i]["description"]);
					//parameterDatas [i] = param;
					if (!parameterDataDic.ContainsKey (param.name.Trim())) {
						parameterDataDic.Add (param.name.Trim(),param);
					}
				}
			}
		}

		void LoadCSVItem(){
			List<Dictionary<string, object>> data = CSVReader.Read(CSV_ITEM);
			if (data != null)
			{
				itemDatas = new ItemData[data.Count];
				itemDataDic = new Dictionary<int, ItemData> ();
				itemDataDic1 = new Dictionary<string, ItemData> ();
				for (int i = 0; i < data.Count; i++)
				{
					ItemData item = new ItemData();
					item.id = GetInt(data[i]["id"]);
					item.name = GetString(data[i]["name"]);
					item.itemResource = GetString(data[i]["item_resource"]);
					item.height_offset = GetFloat(data[i]["height_offset"]);
					item.width = Mathf.Max(0,GetInt(data[i]["width"]));
					item.height =Mathf.Max(0,GetInt(data[i]["height"]));
					item.effectType = GetInt(data [i]["effect_type"]);
					item.effectValue = GetInt(data [i] ["effect_value"]);
					item.percentage = Mathf.Max(0,GetInt(data[i]["percentage"]));
					item.moveSpeed =  Mathf.Max(0,GetFloat(data[i]["move_speed"]));
					itemDatas [i] = item;
					if (!itemDataDic.ContainsKey (item.id)) {
						itemDataDic.Add (item.id,item);
					}
					if (!itemDataDic1.ContainsKey (item.itemResource)) {
						itemDataDic1.Add (item.itemResource,item);
					}
				}
			}
		}

		void LoadCSVBackground(){
			List<Dictionary<string, object>> data = CSVReader.Read(CSV_BACKGROUND);
			if (data != null)
			{
				backgroundDatas = new List<BackgroundData>();
				for (int i = 0; i < data.Count; i++)
				{
					BackgroundData background = new BackgroundData();
					background.id = GetInt(data[i]["id"]);
					background.name =  GetString(data[i]["name"]);
					background.image_resource = GetString(data[i]["image_resource"]);
					background.appear_distance = Mathf.Max(0,GetInt(data[i]["appear_distance"]));
					backgroundDatas.Add(background);
				}
				backgroundDatas.Sort ();
			}
		}

		void LoadCSVMap(){
			List<Dictionary<string, object>> data = CSVReader.Read(CSV_MAP);
			if (data != null)
			{
				mapDatas = new List<MapData>();
				mapDataDic = new Dictionary<int, MapData> ();
				for (int i = 0; i < data.Count; i++)
				{
					MapData mapData = new MapData();
					mapData.id = GetInt(data[i]["id"]);
					mapData.name = GetString(data[i]["name"]);
					mapData.percentage = Mathf.Max(0,GetInt(data[i]["percentage"]));
					mapData.appear_distance = Mathf.Max(0,GetInt(data[i]["appear_distance"]));
					mapData.appear_distance_end = Mathf.Max(mapData.appear_distance,GetInt(data[i]["appear_distance_end"]));
					mapData.description = GetString(data[i]["description"]);
					mapDatas.Add(mapData);
					if(!mapDataDic.ContainsKey(mapData.id)){
						mapDataDic.Add (mapData.id,mapData);
					}
				}

			}
		}

		//must call after LoadCSVMap
		void LoadCSVMapDetail(){
			List<Dictionary<string, object>> data = CSVReader.Read(CSV_MAPDETAIL);
			if (data != null)
			{
				mapDetailDatas = new MapDetailData[data.Count];
				for (int i = 0; i < data.Count; i++)
				{
					MapDetailData mapDetailData = new MapDetailData();
					mapDetailData.id = GetInt(data[i]["id"]);
					mapDetailData.map_id = GetInt(data[i]["map_id"]);
					mapDetailData.sub_distance = GetInt(data[i]["sub_distance"]);
					mapDetailData.height_offset = GetFloat(data[i]["height_offset"]);
					mapDetailData.item_appear_percentage_supplementary = GetInt(data[i]["item_appear_percentage_supplementary"]);
					mapDetailData.column_1_item_id = GetInt(data[i]["column_1_item_id"]);
					mapDetailData.column_2_item_id = GetInt(data[i]["column_2_item_id"]);
					mapDetailData.column_3_item_id = GetInt(data[i]["column_3_item_id"]);
					mapDetailData.column_4_item_id = GetInt(data[i]["column_4_item_id"]);
					mapDetailData.column_5_item_id = GetInt(data[i]["column_5_item_id"]);
					mapDetailData.column_6_item_id = GetInt(data[i]["column_6_item_id"]);
					mapDetailDatas [i] = mapDetailData;
					if(mapDataDic.ContainsKey(mapDetailData.map_id)){
						if (mapDataDic [mapDetailData.map_id].details == null)
							mapDataDic [mapDetailData.map_id].details = new List<MapDetailData> ();
						mapDataDic [mapDetailData.map_id].details.Add (mapDetailData);
					}
				}
				for(int i=0;i<mapDatas.Count;i++){
					if(mapDatas [i].details!=null)
						mapDatas [i].details.Sort ();
				}
				mapDatas.Sort ();
			}
		}

		void LoadServerParameter(){
			if (APIInformation.GetInstance != null && APIInformation.GetInstance.gameparameter.player_lifes > 0)
			{
				GameParameter parameter = APIInformation.GetInstance.gameparameter;

				return;
			}
		}



	}
	[System.Serializable]
	public class RankData{
		public int id;
		public string rank;
		public int mixScore;
		public int maxScore;
		public int reward_item_id;
		public int reward_num;
	}
	[System.Serializable]
	public class ParameterData{
		public int id;
		public string name;
		public string value;
		public string description;
	}
	[System.Serializable]
	public class BackgroundData: IComparable<BackgroundData>{
		public int id;
		public string name;
		public string image_resource;
		public int appear_distance;

		public int CompareTo(BackgroundData obj){
			if (this.appear_distance > obj.appear_distance) return 1;
			if (this.appear_distance < obj.appear_distance) return -1;
			return 0;
		}
	}
	//[System.Serializable]
	public class ItemData
	{
		public int id;
		public string name;
		public float height_offset;
		public int width;
		public int height;
		public int effectType;
		public int effectValue;
		public string itemResource;
		public int percentage;
		public float moveSpeed;
		public string description;

		public void Clone(ItemData item){
			id = item.id;
			name = item.name;
			height_offset = item.height_offset;
			width = item.width;
			height = item.height;
			effectType = item.effectType;
			effectValue = item.effectValue;
			itemResource = item.itemResource;
			percentage = item.percentage;
			moveSpeed = item.moveSpeed;
			description = item.description;
		}
	}
	[System.Serializable]
	public class MapData: IComparable<MapData>{
		public int id;
		public string name;
		public int percentage;
		public int appear_distance;
		public int appear_distance_end;
		public int temp_percentage;
		public string description;
		public List<MapDetailData> details;
		public int CompareTo(MapData obj){
			if (this.appear_distance > obj.appear_distance) return 1;
			if (this.appear_distance < obj.appear_distance) return -1;
			return 0;
		}
		public static MapData Clone(MapData other){
			MapData mapData = new MapData ();
			mapData.id = other.id;
			mapData.name = other.name;
			mapData.percentage = other.percentage;
			mapData.appear_distance = other.appear_distance;
			mapData.appear_distance_end = other.appear_distance_end;
			mapData.description = other.description;
			mapData.details = new List<MapDetailData> ();
			if (other.details != null) {
				for(int i=0;i<other.details.Count;i++){
					mapData.details.Add (other.details[i]);
				}
			}
			return mapData;
		}
	}

	[System.Serializable]
	public class MapDetailData: IComparable<MapDetailData>{
		public int id;
		public int map_id;
		public int sub_distance;
		public float height_offset;
		public int item_appear_percentage_supplementary;
		public int column_1_item_id;
		public int column_2_item_id;
		public int column_3_item_id;
		public int column_4_item_id;
		public int column_5_item_id;
		public int column_6_item_id;
		//public ItemData itemData;


		public ItemData itemData1;
		public ItemData itemData2;
		public ItemData itemData3;
		public ItemData itemData4;
		public ItemData itemData5;
		public ItemData itemData6;


		public int appear_distance;
		//public int appear_column;

		public int CompareTo(MapDetailData obj){
			if (this.sub_distance > obj.sub_distance) return 1;
			if (this.sub_distance < obj.sub_distance) return -1;
			return 0;
		}

		public static MapDetailData Clone(MapDetailData other){
			MapDetailData mapDetailData = new MapDetailData ();
			mapDetailData.id = other.id;
			mapDetailData.map_id = other.map_id;
			mapDetailData.sub_distance = other.sub_distance;
			mapDetailData.height_offset = other.height_offset;
			mapDetailData.item_appear_percentage_supplementary  = other.item_appear_percentage_supplementary;
			mapDetailData.column_1_item_id = other.column_1_item_id;
			mapDetailData.column_2_item_id = other.column_2_item_id;
			mapDetailData.column_3_item_id = other.column_3_item_id;
			mapDetailData.column_4_item_id = other.column_4_item_id;
			mapDetailData.column_5_item_id = other.column_5_item_id;
			mapDetailData.column_6_item_id = other.column_6_item_id;
			mapDetailData.itemData1 = other.itemData1;
			mapDetailData.itemData2 = other.itemData2;
			mapDetailData.itemData3 = other.itemData3;
			mapDetailData.itemData4 = other.itemData4;
			mapDetailData.itemData5 = other.itemData5;
			mapDetailData.itemData6 = other.itemData6;
			mapDetailData.appear_distance = other.appear_distance;
			return mapDetailData;
		}

	}

}
