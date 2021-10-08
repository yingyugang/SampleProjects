using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Swimming
{
	[System.Serializable]
	public class HelperData
	{
		public HelperType type;
		public int appearDistance;
		public TimeType bgTime;
		public HelperAlign align;

		public HelperData(HelperType type, int appearDistance, TimeType bgTime, HelperAlign align)
		{
			this.type = type;
			this.appearDistance = appearDistance;
			this.bgTime = bgTime;
			this.align = align;
		}
	}

	public class ObstacleData
	{
		public MovementType moveType;
		public float speed;
		public float appearDistance;
		public float appearProbality;
		public int appearMax;

		public ObstacleData(MovementType moveType, float speed, float appearDistance, float appearProbality, int appearMax)
		{
			this.moveType = moveType;
			this.speed = speed;
			this.appearDistance = appearDistance;
			this.appearProbality = appearProbality;
			this.appearMax = appearMax;
		}
	}

	public class DistanceData
	{
		public float distance;
		public int num;

		public DistanceData(float distance, int num)
		{
			this.distance = distance;
			this.num = num;
		}
	}

	public class ItemData
	{
		public ItemType type;
		public int effectType;
		public int effectValue;
		public float appearProbality;
		public ItemData(ItemType type, int effectType, int effectValue, float probality)
		{
			this.type = type;
			this.effectType = effectType;
			this.effectValue = effectValue;
			this.appearProbality = probality;
		}
	}

	public class GameParams : MonoBehaviour 
	{
		public const string CSV_FOLDER = "csv/";
		public const string CSV_HELPER = "m_river_swimming_character";
		public const string CSV_OBSTACLE = "m_river_swimming_obstacle";
		public const string CSV_OBSTACLE_NUM = "m_river_swimming_obstacle_num";
		public const string CSV_ITEM = "m_river_swimming_item";
		public const string CSV_PARAMATER = "m_river_swimming_parameter";
		public const string CSV_RANK = "m_river_swimming_rank";

		public float riverSpeed = 1f;
		public float playerSpeed = 1f;
		public int playerLifes = 3;
		public float itemSpanSecond = 5f;
		public float itemAppearPercentage = 20f;

		public float obstacleAppearPercentage = 10f;
		public float obstacleAppearSpanSecond = 10f;
		public int start = 0;
		public int flickSensitivity = 50;
		public int notchAfter2 = 3000;
		public int speedUpMetters = 1500;
		public int speedUpPercentage = 10;

		public float comboVar;

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
			LoadCsvData();
		}

		void Start()
		{
			
		}

		public Dictionary<EnemyType, ObstacleData> obstacles = new Dictionary<EnemyType, ObstacleData>()
		{
			{EnemyType.Rock1, new ObstacleData(MovementType.StraightDown, 1f, 30f, 20f, 3)},
			{EnemyType.Rock2, new ObstacleData(MovementType.StraightDown, 1f, 50f, 20f, 1)},
			{EnemyType.Rock3, new ObstacleData(MovementType.StraightDown, 1f, 50f, 20f, 1)},
			{EnemyType.Wood, new ObstacleData(MovementType.StraightDown, 1f, 50f, 30f, 1)},
			{EnemyType.Boat1, new ObstacleData(MovementType.StraightDown, 1f, 80f, 30f, 1)},
			{EnemyType.Fish1, new ObstacleData(MovementType.CurveDown, 1f, 80f, 30f, 1)},
			{EnemyType.JellyFish, new ObstacleData(MovementType.ZigZag, 1f, 120f, 500f, 1)},
			{EnemyType.Boat2, new ObstacleData(MovementType.StraightUp, 1f, 120f, 10000f, 3)},
			{EnemyType.Fish2, new ObstacleData(MovementType.CurveUp, 1f, 200f, 10000f, 3)}
		};

		public DistanceData[] distanceData = new DistanceData[]
		{
			new DistanceData(20, 1),
			new DistanceData(50, 1),
			new DistanceData(100, 2),
			new DistanceData(200, 3),
			new DistanceData(400, 4),
			new DistanceData(600, 4)
		};

		public HelperData[] helperData = new HelperData[]
		{
			new HelperData(HelperType.Osomatsu, 30, TimeType.Morning, HelperAlign.Left),
			new HelperData(HelperType.Karamatsu, 60, TimeType.Evening, HelperAlign.Right),
			new HelperData(HelperType.Chocomatsu, 500, TimeType.Morning, HelperAlign.Left),
			new HelperData(HelperType.Ichimatsu, 700, TimeType.Morning, HelperAlign.Left),
			new HelperData(HelperType.Todomatsu, 1000, TimeType.Morning, HelperAlign.Left),
			new HelperData(HelperType.Dayon, 1500, TimeType.Evening, HelperAlign.Left),
			new HelperData(HelperType.Dekapan, 2000, TimeType.Morning, HelperAlign.Left),
			new HelperData(HelperType.Chibita, 3000, TimeType.Morning, HelperAlign.Left),
			new HelperData(HelperType.Iyami, 4000, TimeType.Morning, HelperAlign.Left),
			new HelperData(HelperType.Totoko, 5000, TimeType.Night, HelperAlign.Left),
			new HelperData(HelperType.Hatabou, 6000, TimeType.Morning, HelperAlign.Left),
			new HelperData(HelperType.KyoshiSawa, 7000, TimeType.Morning, HelperAlign.Left)
		};

		public ItemData[] itemData = new ItemData[]
		{
			new ItemData(ItemType.Immortal, 2, 30, 2),
			new ItemData(ItemType.Oden, 1, 40, 6),
			new ItemData(ItemType.Rice, 1, 20, 6),
			new ItemData(ItemType.Tree, 1, 20, 6)
		};

		public List<int> listRanking = new List<int>();

		public void LoadCsvData()
		{
			LoadCsvHelper();
			LoadCsvObstacle();
			LoadCsvObstalceNum();
			LoadCsvItem();
			LoadCsvParameter();
			LoadCsvRank();
		}

		private void LoadCsvHelper()
		{
			List<Dictionary<string, object>> data = CSVReader.Read(CSV_FOLDER + CSV_HELPER);
			if (data != null)
			{
				helperData = new HelperData[data.Count];
				for (int i = 0; i < data.Count; i++)
				{
					HelperType type = StringToHelperType((string)data[i]["name"]);
					int appearDistance = (int) data[i]["appear_distance"];
					TimeType bgTime = StringToTimeType((string) data[i]["background_image"]);
					HelperAlign align = (HelperAlign) data[i]["direction"];
					HelperData helper = new HelperData(type, appearDistance, bgTime, align );
					helperData[i] = helper;
				}

				Debug.Log("LoadCsvHelper " + helperData.Length);
			}
		}

		private void LoadCsvObstacle()
		{
			List<Dictionary<string, object>> data = CSVReader.Read(CSV_FOLDER + CSV_OBSTACLE);
			if (data != null)
			{
				obstacles = new Dictionary<EnemyType, ObstacleData>();

				for (int i = 0; i < data.Count; i++)
				{
					EnemyType type = StringToEnemyType((string)data[i]["name"]);
					MovementType moveType = (MovementType) data[i]["move_type"];
					float speed = float.Parse(data[i]["speed"].ToString());
					float appearDistance = float.Parse(data[i]["appear_distance"].ToString());
					float appearProbality = float.Parse(data[i]["appear_probality"].ToString());
					int appearMax = int.Parse(data[i]["appear_max"].ToString());

					ObstacleData obstacle = new ObstacleData(moveType, speed, appearDistance, appearProbality, appearMax);
					obstacles.Add(type, obstacle);
				}

				Debug.Log("LoadCsvObstacle " + obstacles.Count);
			}
		}

		private void LoadCsvItem()
		{
			List<Dictionary<string, object>> data = CSVReader.Read(CSV_FOLDER + CSV_ITEM);
			if (data != null)
			{
				itemData = new ItemData[data.Count];
				for (int i = 0; i < data.Count; i++)
				{
					ItemType type = StringToItemType((string)data[i]["name"]);
					int effectType = (int) data[i]["effect_type"];
					int effectValue = (int) data[i]["effect_value"];
					int appearProbality = (int) data[i]["appear_probality"];
					ItemData item = new ItemData(type, effectType, effectValue, appearProbality );
					itemData[i] = item;
				}

				Debug.Log("LoadCsvItem " + helperData.Length);
			}
		}

		private void LoadCsvObstalceNum()
		{
			List<Dictionary<string, object>> data = CSVReader.Read(CSV_FOLDER + CSV_OBSTACLE_NUM);
			if (data != null)
			{
				distanceData = new DistanceData[data.Count];
				for (int i = 0; i < data.Count; i++)
				{
					float distance = (int) data[i]["distance"];
					int num = (int) data[i]["obstacle_num"];
					DistanceData d = new DistanceData(distance, num);
					distanceData[i] = d;
				}

				Debug.Log("LoadCsvObstalceNum " + distanceData.Length);
			}
		}

		private void LoadCsvParameter()
		{
			if (APIInformation.GetInstance != null && APIInformation.GetInstance.gameparameter.player_lifes > 0)
			{
				GameParameter parameter = APIInformation.GetInstance.gameparameter;

				this.riverSpeed = parameter.river_speed;
				this.playerSpeed = parameter.player_speed;
				this.playerLifes = parameter.player_lifes;
				//#if UNITY_EDITOR
				//this.playerLifes = 99;
				//#endif
				this.itemSpanSecond = parameter.item_span_second;
				this.itemAppearPercentage = parameter.item_appear_percentage;
				this.obstacleAppearPercentage = parameter.obstacle_appear_percentage;
				this.obstacleAppearSpanSecond = parameter.obstacle_appear_span_second;
				this.start =  parameter.start;
				this.flickSensitivity = parameter.flick_sensitivity;
				this.notchAfter2 =  parameter.notch_after_2;
				this.speedUpMetters =  parameter.speed_up_meters;
				this.speedUpPercentage =  parameter.speed_up_percentage;
				this.comboVar = APIInformation.GetInstance.gameparameter.total_comb_var;

				return;
			}

			List<Dictionary<string, object>> data = CSVReader.Read(CSV_FOLDER + CSV_PARAMATER);
			if (data != null)
			{
				Dictionary<string, int> param = new Dictionary<string, int>();

				for (int i = 0; i < data.Count; i++)
				{
					string key = (string) data[i]["param_name"];
					int value =  (int) data[i]["value"];
					param.Add(key, value);
				}

				this.riverSpeed = (float) param["river_speed"];
				this.playerSpeed = (float) param["player_speed"];
				this.playerLifes = (int) param["player_lifes"];
				this.itemSpanSecond = (float) param["item_span_second"];
				this.itemAppearPercentage = (float) param["item_appear_percentage"];
				this.obstacleAppearPercentage = (float) param["obstacle_appear_percentage"];
				this.obstacleAppearSpanSecond = (float) param["obstacle_appear_span_second"];
				this.start =  (int) param["start"];
				this.flickSensitivity =  (int) param["flick_sensivity"];
				this.notchAfter2 =  (int) param["notch_after_2"];
				this.speedUpMetters =  (int) param["speed_up_metters"];
				this.speedUpPercentage =  (int) param["speed_up_percentage"];
				this.comboVar = 1f;

				Debug.Log("LoadCsvParameter " + param.Count);
			}
		}

		public void LoadCsvRank()
		{
			List<Dictionary<string, object>> data = CSVReader.Read(CSV_FOLDER + CSV_RANK);
			for (int i = 0; i < data.Count; i++)
			{
				int max = (int)data[i]["max"];
				listRanking.Add(max);
			}
		}

		private TimeType StringToTimeType(string str)
		{
			TimeType time = TimeType.Morning;
			if (str == "evening")
				time = TimeType.Evening;
			else if (str == "night")
				time = TimeType.Night;

			return time;
		}

		public HelperData GetHelperDataByType(HelperType type)
		{
			foreach ( var v in helperData)
			{
				if (v.type == type)
					return v;
			}

			return null;
		}

		public ItemData GetItemDataByType(ItemType type)
		{
			foreach ( var v in itemData)
			{
				if (v.type == type)
					return v;
			}

			return null;
		}

		public int GetObstacleNum(float distance)
		{
			if (distance < distanceData[0].distance)
				return distanceData[0].num;

			if (distance > distanceData[distanceData.Length-1].distance)
				return distanceData[distanceData.Length-1].num;

			for (int i=0; i<distanceData.Length-1; i++)
				if (distance > distanceData[i].distance && distance < distanceData[i+1].distance)
					return distanceData[i].num;

			return 1;
		}

		public List<EnemyType> GetListObstacle(float distance)
		{
			List<EnemyType> list = new List<EnemyType>();
			foreach(var v in obstacles)
			{
				EnemyType enemyType = v.Key;
				ObstacleData data = v.Value;
				if (distance > data.appearDistance)
					list.Add(enemyType);
			}

			return list;
		}

		public float GetAppearProbality(EnemyType type)
		{
			return obstacles[type].appearProbality;
		}

		public string RandomObstaceType(float distance)
		{
			return EnemyTypeToString(RandomObstace(distance));
		}

		public EnemyType RandomObstace(float distance)
		{
			List<EnemyType> list = GetListObstacle(distance);
			float total = 0f;
			foreach(var v in list)
				total += GetAppearProbality(v);

			float r = Random.value * total;
			float counterRatio = 0f;
			foreach (EnemyType type in list)
			{
				counterRatio += GetAppearProbality(type);
				if (r <= counterRatio)
					return type;
			}

			return EnemyType.Rock1;
		}

		public string RandomItemType()
		{
			return ItemTypeToString(RandomItem());
		}
			
		public ItemType RandomItem()
		{
			float total = 0f;
			foreach(var v in itemData)
				total +=  v.appearProbality;

			float r = Random.value * total;
			float counterRatio = 0f;
			foreach (var v in itemData)
			{
				counterRatio += v.appearProbality;
				if (r <= counterRatio)
					return v.type;
			}

			return ItemType.Oden;
		}

		public int GetImmortalTime()
		{
			ItemData item = GetItemDataByType(ItemType.Immortal);
			return item.effectValue;
		}

		public HelperType StringToHelperType(string name)
		{
			HelperType type = HelperType.Osomatsu;
			if (name == "osomatsu")
				type = HelperType.Osomatsu;
			else if (name == "karamatsu")
				type = HelperType.Karamatsu;
			else if (name == "choromatsu")
				type = HelperType.Chocomatsu;
			else if (name == "ichimatsu")
				type = HelperType.Ichimatsu;
			else if (name == "todomatsu")
				type = HelperType.Todomatsu;
			else if (name == "dayon")
				type = HelperType.Dayon;
			else if (name == "dekapan")
				type = HelperType.Dekapan;
			else if (name == "chibita")
				type = HelperType.Chibita;
			else if (name == "iyami")
				type = HelperType.Iyami;
			else if (name == "totoko")
				type = HelperType.Totoko;
			else if (name == "hatabou")
				type = HelperType.Hatabou;
			else if (name == "shonosuke")
				type = HelperType.KyoshiSawa;

			return type;
		}

		public EnemyType StringToEnemyType(string name)
		{
			name = name.ToLower();
			EnemyType type = EnemyType.Rock1;
			if (name == "rock1")
				type = EnemyType.Rock1;
			else if (name == "rock2")
				type = EnemyType.Rock2;
			else if (name == "rock3")
				type = EnemyType.Rock3;
			else if (name == "fish1")
				type = EnemyType.Fish1;
			else if (name == "fish2")
				type = EnemyType.Fish2;
			else if (name == "jellyfish")
				type = EnemyType.JellyFish;
			else if (name == "boat1")
				type = EnemyType.Boat1;
			else if (name == "boat2")
				type = EnemyType.Boat2;
			else if (name == "wood")
				type = EnemyType.Wood;

			return type;
		}

		private string EnemyTypeToString(EnemyType type)
		{
			string name = "Rock1";
			switch(type)
			{
			case EnemyType.Boat1:
				name = "Boat1";
				break;
			case EnemyType.Boat2:
				name = "Boat2";
				break;
			case EnemyType.Rock1:
				name = "Rock1";
				break;
			case EnemyType.Rock2:
				name = "Rock2";
				break;
			case EnemyType.Rock3:
				name = "Rock3";
				break;
			case EnemyType.Fish1:
				name = "Fish1";
				break;
			case EnemyType.Fish2:
				name = "Fish2";
				break;
			case EnemyType.JellyFish:
				name = "JellyFish";
				break;
			case EnemyType.Wood:
				name = "Wood";
				break;
			}

			return name;
		}

		public ItemType StringToItemType(string name)
		{
			ItemType type = ItemType.Immortal;
			if (name == "immortal")
				type = ItemType.Immortal;
			else if (name == "oden")
				type = ItemType.Oden;
			else if (name == "rice")
				type = ItemType.Rice;
			else if (name == "tree")
				type = ItemType.Tree;

			return type;
		}

		public string ItemTypeToString(ItemType type)
		{
			string name = "Oden";
			switch(type)
			{
			case ItemType.Immortal:
				name = "Immortal";
				break;
			case ItemType.Oden:
				name = "Oden";
				break;
			case ItemType.Rice:
				name = "Rice";
				break;
			case ItemType.Tree:
				name = "Tree";
				break;
			}

			return name;
		}

		public float GetRelativelyRiverSpeed()
		{
			return playerSpeed * SwimmingConfig.SPEED_PER_UNIT;
		}
	}
}