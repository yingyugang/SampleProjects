using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace GetOut
{
	public class ItemData
	{
		public TYPEITEM type;
		public int effectType;
		public int effectValue;
		public float appearProbality;
		public float reductionAppearPercent;
		public ItemData(TYPEITEM type, int effectType, int effectValue, float probality,float reductionAppearPercent)
		{
			this.type = type;
			this.effectType = effectType;
			this.effectValue = effectValue;
			this.appearProbality = probality;
			this.reductionAppearPercent = reductionAppearPercent;
		}

	}

	public class EnemyData
	{
		public string name;
		public int index;
		public float speedBasic;
		public float timeApear;
		public int logicValue;
		public int logicType;
		public EnemyData(int index,float speedBasic, float timeApear,int logicValue,string name, int logicType)
		{
			
			this.index = index;
			this.speedBasic = speedBasic;
			this.timeApear = timeApear;
			this.logicValue = logicValue;
			this.name = name;
			this.logicType = logicType;
		}
	}

	public  class MapBackgroundData
	{
		public string nameFile;
		public int index;
		public string name;
		public int idMap;
		public int appearProbability;

		public MapBackgroundData(int index,string name,int idMap, string nameFile,int appearProbability)
		{
			this.index = index;
			this.name = name;
			this.idMap = idMap;
			this.nameFile = nameFile;
			this.appearProbability = appearProbability;
		}

	}


	public class GameParameter
	{
		public float playerSpeedRate;
		public bool itemOn;
		public float itemSpanSecond;
		public int flickSensitivity;
		public int playerLifes;
		public float timerSpeedRate;
		public float doorTime;
		public float comboVar;
		public GameParameter(float playerSpeedRate,bool itemOn,float itemSpanSecond,int flickSensitivity,int playerLifes,float timerSpeedRate,float doorTime){
			this.playerSpeedRate = playerSpeedRate;
			this.itemOn = itemOn;
			this.itemSpanSecond = itemSpanSecond;
			this.flickSensitivity = flickSensitivity;
			this.playerLifes = playerLifes;
			this.timerSpeedRate = timerSpeedRate;
			this.doorTime = doorTime;
		}
	}


	public class MatrixMap
	{
		private int [,] matrixMapData;

		public MatrixMap(){
			matrixMapData = new int[Map.WIDTH,Map.HEIGHT];
		}


		public int[,] MaTrixMapData
		{
			set{ this.matrixMapData = value;}
			get{ return this.matrixMapData;}
		}
	}


	public class MapStage{
		public MatrixMap matrixMap;
		public MapBackgroundData backGround;
		public int appearPercent;
		public MapStage(MatrixMap matrixMap,MapBackgroundData backGround)
		{
			this.matrixMap = matrixMap;
			this.backGround = backGround;
		}
	}

	public class GameParams : MonoBehaviour 
	{
		public const string CSV_ITEM = "m_escape_flicking_item";
		public const string CSV_ENEMY = "m_escape_flicking_character";
		public const string CSV_MAP_MATRIX = "m_escape_flicking_map_detail";
		public const string CSV_MAP_BACKGROUND = "m_escape_flicking_stage";
		public const string CSV_GAME_PARAMETER = "m_escape_flicking_parameter";

		public const string CSV_MAP_MATRIX_CHEAT = "m_escape_flicking_map_detail_cheat";
		public const string CSV_MAP_BACKGROUND_CHEAT = "m_escape_flicking_stage_cheat";

		public GameParameter gameParameter;
		public ItemData[] arrayItemData;
		public EnemyData[] arrayEnemyData;
		public MapBackgroundData[] arrayBackgroundData;

		public List<MatrixMap> listMatrixMap;

		public MapStage[] arrayMapStage;
		public MapBackgroundData mapBackGround;
		private int m_totalPercentAppearMap;
		private static GameParams m_Instance;
		public static GameParams Instance
		{
			get
			{
				return m_Instance;
			}
		}

		void Awake()
		{
			m_Instance = this;
			listMatrixMap = new List<MatrixMap>();
			LoadCsvItem();
			LoadCsvEnemy();
			LoadCsvMapMatrix();
			LoadCsvMapBackground();
			LoadCsvParameter();
			mapBackGround = RandomMap();
		}
		//-->| by anhgh
		void Start(){
			Debug.Log ("Map id = " + mapBackGround.idMap);	
			if (mapBackGround.idMap == 5)
				GetOut.GameManager.Instance().hideAreaShow = 1;
		}
		// --<| by anhgh 

		public MapBackgroundData RandomMap()
		{
			int randomMap = Random.Range(0,m_totalPercentAppearMap);
			for (int i = 0; i < arrayMapStage.Length; i++) {
				for (int j = i; j >= 0; j--) {
					arrayMapStage [i].appearPercent += arrayMapStage [j].backGround.appearProbability;
				}
			}
			for (int i = 0; i < arrayMapStage.Length; i++) {
				if (randomMap < arrayMapStage [i].appearPercent) {
					Map.s_Matrix = arrayMapStage [i].matrixMap.MaTrixMapData;
					return arrayMapStage [i].backGround ;
				}
			}
			return null;
		}

		public int GetEffectValueItem(TYPEITEM type)
		{
			for(int i = 0; i < arrayItemData.Length; i++)
			{
				if(arrayItemData[i].type == type)
				{
					return arrayItemData[i].effectValue;
				}
			}
			return 0;
		}

		public float GetApearProbalityItem(TYPEITEM type)
		{
			for(int i = 0; i < arrayItemData.Length; i++)
			{
				if(arrayItemData[i].type == type)
				{
					return arrayItemData[i].appearProbality;
				}
			}
			return 0;
		}

		public float GetReductionAppearPercentItem(TYPEITEM type)
		{
			for(int i = 0; i < arrayItemData.Length; i++)
			{
				if(arrayItemData[i].type == type)
				{
					return arrayItemData[i].reductionAppearPercent;
				}
			}
			return 0;
		}


		public EnemyData GetEnemyData(int index)
		{
			for(int i = 0; i < arrayEnemyData.Length; i++)
			{
				if(arrayEnemyData[i].index == index)
					return arrayEnemyData[i];
			}
			return null;
		}

		private void LoadCsvParameter()
		{

			if(APIInformation.GetInstance != null)
			{
				float playerSpeedRate = APIInformation.GetInstance.gameparameter.player_speed_rate;
				bool itemOn = APIInformation.GetInstance.gameparameter.item_on;
				float itemSpanSecond = APIInformation.GetInstance.gameparameter.item_span_second;
				int flickSensitivity = APIInformation.GetInstance.gameparameter.flick_sensitivity;
				int playerLifes = APIInformation.GetInstance.gameparameter.player_lifes;
				float timerSpeedRate = APIInformation.GetInstance.gameparameter.timer_speed_rate;
				float doorTime = APIInformation.GetInstance.gameparameter.door_time;
				float comboVar = APIInformation.GetInstance.gameparameter.total_comb_var;
				gameParameter = new GameParameter(playerSpeedRate,itemOn,itemSpanSecond,flickSensitivity,playerLifes,timerSpeedRate,doorTime);
				gameParameter.comboVar = comboVar;
			}
			else
			{
				List<Dictionary<string, object>> data = CSVReader.Read("csv/" + CSV_GAME_PARAMETER);
				if (data != null)
				{
					Dictionary<string,float> dataParam = new Dictionary<string, float>();
					for (int i = 0; i < data.Count; i++)
					{
						string key = data[i]["para_name"].ToString();
						float value = float.Parse(data[i]["value"].ToString());
						dataParam.Add(key,value);
					}
					float playerSpeedRate = (float) dataParam["player_speed_rate"];
					bool itemOn =  ((int)dataParam["item_on"] > 0);
					float itemSpanSecond = dataParam["item_span_second"];
					int flickSensitivity = (int)dataParam["flick_sensitivity"];
					int playerLifes = (int)dataParam["player_lifes"];
					float timerSpeedRate = dataParam["timer_speed_rate"];
					float doorTime = dataParam["door_time"];
					gameParameter = new GameParameter(playerSpeedRate,itemOn,itemSpanSecond,flickSensitivity,playerLifes,timerSpeedRate,doorTime);
				}
			}
		}


		private void LoadCsvItem()
		{
			List<Dictionary<string, object>> data = CSVReader.Read("csv/" + CSV_ITEM);

			if (data != null)
			{
				arrayItemData = new ItemData[data.Count];
				for (int i = 0; i < data.Count; i++)
				{
					int effectValue = (int) data[i]["effect_value"];
					int appearProbality = (int) data[i]["appear_probability"];
					int effectType = (int) data[i]["effect_type"];
					TYPEITEM type = StringToItemType((string)data[i]["name"]);
					float reductionAppear = float.Parse(data[i]["reduction_percentage"].ToString());

					ItemData item = new ItemData(type, effectType, effectValue, appearProbality,reductionAppear);
					arrayItemData[i] = item;
				}
			}
		}

		private void LoadCsvEnemy()
		{
			List<Dictionary<string, object>> data = CSVReader.Read("csv/" + CSV_ENEMY);
			if (data != null)
			{
				arrayEnemyData = new EnemyData[data.Count];
				for (int i = 0; i < data.Count; i++)
				{
					int index = (int) data[i]["id"];
					float speedBasic = float.Parse( data[i]["speed"].ToString());
					float timeApear = float.Parse( data[i]["appear_time"].ToString());
					int logicValue = 0;
					int logicType =	(int) data [i] ["logic_type"];
					string name = data[i]["name"].ToString();
					if(data[i]["logic_value"].ToString() != "")
						logicValue = (int)(data[i]["logic_value"]);
					EnemyData enemy = new EnemyData(index,speedBasic,timeApear,logicValue,name,logicType);
					arrayEnemyData[i] = enemy;
				}
			}
		}

		private void LoadCsvMapMatrix()
		{
			int mapIndex = 0;
			List<Dictionary<string, object>> data = null;
			//cheat begin
			if (CheatController.IsCheated(0)) {
				data = CSVReader.Read("csv/" + CSV_MAP_MATRIX_CHEAT);
				if(data == null || data.Count == 0){
					data = CSVReader.Read("csv/" + CSV_MAP_MATRIX);
					Debug.LogError (CSV_MAP_MATRIX_CHEAT + " is not existing or is empty");
				}
			} else {
				data = CSVReader.Read("csv/" + CSV_MAP_MATRIX);
			}
			//cheat end
			if(data != null)
			{
				for(int i = 0; i < data.Count; i++)
				{
					if(mapIndex < (int)data[i]["m_escape_flicking_map_id"])
					{
						listMatrixMap.Add(new MatrixMap());
						mapIndex++;
					}
					listMatrixMap[(int)data[i]["m_escape_flicking_map_id"]-1].MaTrixMapData[(int)data[i]["x"]-1,(int)data[i]["y"]-1] = (int)data[i]["type"];
				}
			}
		}


		private void LoadCsvMapBackground()
		{
			List<Dictionary<string, object>> data = null;
			//cheat begin
			if (CheatController.IsCheated(0)) {
				data = CSVReader.Read("csv/" + CSV_MAP_BACKGROUND_CHEAT);
				if(data == null || data.Count == 0){
					data = CSVReader.Read("csv/" + CSV_MAP_BACKGROUND);
					Debug.LogError (CSV_MAP_BACKGROUND_CHEAT + " is not existing or is empty");
				}
			} else {
				data = CSVReader.Read("csv/" + CSV_MAP_BACKGROUND);
			}
			//cheat end
			if(data != null)
			{
				arrayMapStage = new MapStage[data.Count];
				arrayBackgroundData = new MapBackgroundData[data.Count];
				for (int i = 0; i < data.Count; i++)
				{
					int index = (int) data[i]["id"];
					string name = data[i]["name"].ToString();
					int idMap = (int) data[i]["m_escape_flicking_map_id"];
					string nameFile = data[i]["background_image_resource"].ToString();
					int appearProbability =(int) data[i]["appear_probability"];
					m_totalPercentAppearMap += appearProbability;
					arrayBackgroundData[index -1] = new MapBackgroundData(index,name,idMap,nameFile,appearProbability);
					arrayMapStage[index-1] = new MapStage(listMatrixMap[idMap -1],arrayBackgroundData[index - 1]);
				}

			}
		}


		public TYPEITEM StringToItemType(string name)
		{
			TYPEITEM type = TYPEITEM.Star;
			if (name == "star")
				type = TYPEITEM.Star;
			else if (name == "clock")
				type = TYPEITEM.Clock;
			else if (name == "score1")
				type = TYPEITEM.Score1;
			else if (name == "score2")
				type = TYPEITEM.Score2;
			else if(name == "score3")
				type = TYPEITEM.Score3;
			return type;
		}

		public int GetIdOfShounosuke()
		{
			for(int i = 0; i < arrayEnemyData.Length; i++)
			{
				if(arrayEnemyData[i].name == "KiyoshiSawa Shonosuke")
				{
					return arrayEnemyData[i].index;
				}
			}
			return 0;
		}
	}
}