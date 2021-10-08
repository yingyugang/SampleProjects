using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace Swimming
{

	[Serializable]
	public class EnemyPrefab
	{
		public EnemyType type;
		public GameObject gameObject;
	}

	[Serializable]
	public class ItemPrefab
	{
		public ItemType type;
		public GameObject gameObject;
	}

	public class EnemySpawner : MonoBehaviour 
	{
		public const float HELPER_DISTANCE = 20f;
		public const int HELPER_DISTANCE_INCREASE = 1000;

		public EnemyPrefab[] enemyPrefabs;
		public ItemPrefab[] itemPrefabs;

		public GameObject helper;

		private bool m_SpawnHelper;
		private int m_HelperSpawnCount;
		private int m_HeplerCircle;

		[HideInInspector]
		public Dictionary<EnemyType, int> spawnObstacleCounter;
		public Vector3 lastEnemyPos;

		private static EnemySpawner m_Instance;
		public static EnemySpawner Instance
		{
			get
			{
				return m_Instance;
			}
		}

		void Awake()
		{
			m_Instance = this;
		}

		// Use this for initialization
		void Start () 
		{
			Debug.Log("----------------Start");
			InvokeRepeating("SpawnEnemies", GameParams.Instance.obstacleAppearSpanSecond, GameParams.Instance.obstacleAppearSpanSecond);
			InvokeRepeating("SpawnItem", GameParams.Instance.itemSpanSecond, GameParams.Instance.itemSpanSecond);

			m_SpawnHelper = false;
		}

		public void Init()
		{
			m_HelperSpawnCount = 0;
			m_HeplerCircle = 1;

			//Debug.Log("------   " + GetDistanceToSpawnHelper(m_HelperSpawnCount));

			spawnObstacleCounter = new Dictionary<EnemyType, int> () {
				{EnemyType.Boat1, 0},
				{EnemyType.Boat2, 0},
				{EnemyType.Fish1, 0},
				{EnemyType.Fish2, 0},
				{EnemyType.JellyFish, 0},
				{EnemyType.Rock1, 0},
				{EnemyType.Rock2, 0},
				{EnemyType.Rock3, 0},
				{EnemyType.Wood, 0}
			};
		}

		void ResetSpawnCounter()
		{
			var keys = new List<EnemyType>(spawnObstacleCounter.Keys);
			foreach (var key in keys)
			{
				spawnObstacleCounter [key] = 0;
			}
		}

		public int GetHelperCircle()
		{
			return m_HeplerCircle;
		}
		
		// Update is called once per frame
		void Update () 
		{
			SpawnHelper();
		}

		int GetDistanceToSpawnHelper(int num)
		{
			if (m_HeplerCircle > 1)
			{
				int maxDistance = GameParams.Instance.helperData[GameParams.Instance.helperData.Length-1].appearDistance;
				int numHelper = GameParams.Instance.helperData.Length;
				int distance = maxDistance + (m_HeplerCircle - 2) * GameParams.Instance.notchAfter2 * numHelper + GameParams.Instance.notchAfter2 * (num+1);
				return distance;
			}
			else 
				return GameParams.Instance.helperData[num].appearDistance;

		}

		void SpawnHelper()
		{
			if (GameManager.Instance.IsPause())
				return;

			if (m_SpawnHelper)
				return;

			float ratio = Swimmer.Instance.IsImmortal() ? 10 : 1;

			if (Swimmer.Instance.IsImmortal())
			{
				if (GameParams.Instance.playerSpeed > 2f)
					ratio = 10;
				else
					ratio = 5;
			}

			// spawn before 10m 
			if (!m_SpawnHelper 
				&& !helper.activeSelf
				&& GetDistanceToSpawnHelper(m_HelperSpawnCount) - Swimmer.Instance.GetDistance() - HELPER_DISTANCE > -5f * ratio 
				&& GetDistanceToSpawnHelper(m_HelperSpawnCount) - Swimmer.Instance.GetDistance()  - HELPER_DISTANCE < 5f * ratio)
			{
				Debug.Log("SpawnHelper------------------------- " + Swimmer.Instance.GetDistance());

				float remaining = GetDistanceToSpawnHelper(m_HelperSpawnCount) - Swimmer.Instance.GetDistance();
				float y = Swimmer.Instance.GetPosition().y + remaining;

				helper.SetActive(true);
				//GameObject helper = GetFreeObject("Helper");

				HelperType type = GameParams.Instance.helperData[m_HelperSpawnCount].type;
				helper.GetComponent<Helper>().Init(type, y);
				m_SpawnHelper = true;
				m_HelperSpawnCount ++;
				if (m_HelperSpawnCount == GameParams.Instance.helperData.Length)
				{
					m_HelperSpawnCount = 0;
					m_HeplerCircle ++;
				}

				ScrollingScript.Instance.CheckOverlapHelper();
				Invoke("RecheckOverlap", 0.5f);

				float timeReset = 5f;
				if (Swimmer.Instance.IsImmortal())
				{
					if (GameParams.Instance.playerSpeed > 2f)
						timeReset = 2f;
					else
						timeReset = 3f;
				}

				Invoke("ResetSpawn", timeReset);

			}
		}

		void RecheckOverlap()
		{
			ScrollingScript.Instance.CheckOverlapHelper();
		}

		void ResetSpawn()
		{
			Debug.Log("--------ResetSpawn");
			m_SpawnHelper = false;
		}

		GameObject GetEnemyByType (EnemyType type)
		{
			foreach (var v in enemyPrefabs) {
				if (v.type == type)
					return v.gameObject;
			}
			return null;
		}

		GameObject GetItemByType (ItemType type)
		{
			foreach (var v in itemPrefabs) {
				if (v.type == type)
					return v.gameObject;
			}
			return null;
		}

		void SpawnEnemies()
		{
			if (GameManager.Instance.IsPause())
				return;

			//ResetSpawnCounter ();

			int num = GameParams.Instance.GetObstacleNum(Swimmer.Instance.GetDistance());
			//Debug.Log("SpawnEnemies " + num);
			float delay = 0.8f;
			for (int i=0; i<num; i++)
			{
				delay *= (i+1);
				StartCoroutine(SpawnEnemy(delay));
			}
		}

		IEnumerator SpawnEnemy(float delay = 0f)
		{			
			yield return new WaitForSeconds(delay);

			float ratio = UnityEngine.Random.value * 100;
			if (ratio  > GameParams.Instance.obstacleAppearPercentage)
				yield break;

			//Debug.Log (GameParams.Instance.obstacleAppearPercentage);
			
			string name = GameParams.Instance.RandomObstaceType(Swimmer.Instance.GetDistance());
			EnemyType type = GameParams.Instance.StringToEnemyType(name);

			int appear = spawnObstacleCounter [type];
			int appearMax = GameParams.Instance.obstacles[type].appearMax;

			if (appear >= appearMax)
				yield break;

			spawnObstacleCounter [type] ++;
			MovementType moveType = GameParams.Instance.obstacles[type].moveType;

			if (Swimmer.Instance.IsImmortal())
			{
				//Debug.Log("IsImmortal " + type.ToString() + " ... " + moveType.ToString());
			}

			if (Swimmer.Instance.IsImmortal()
				&& (moveType == MovementType.StraightUp || moveType == MovementType.CurveUp))
			{
				//Debug.Log("WTFFFFFFFFFFF");
				yield break;
			}

			GameObject enemy = GetFreeObject(name);
			enemy.GetComponent<Enemy>().Init();
			GameManager.Instance.AddEnemy(enemy);

			lastEnemyPos = enemy.transform.position;
		}

		void SpawnItem()
		{
			if (GameManager.Instance.IsPause())
				return;
			
			float ratio = UnityEngine.Random.value * 100;
			if (ratio  < GameParams.Instance.itemAppearPercentage)
			{
				string name = GameParams.Instance.RandomItemType();

				if (name == "Immortal" && Swimmer.Instance.IsImmortal())
					return;

				GameObject item = GetFreeObject(name);
				item.GetComponent<Item>().Init();
				GameManager.Instance.AddItem(item);
			}
		}

		void OnDisable()
		{
			CancelInvoke();
		}

		public GameObject GetFreeObject(string name)
		{
			GameObject obj = Daruma.PoolManager.s_Instance.GetFreeObject(name);
			obj.SetActive(true);
			return obj;
		}

		public void Despawn(GameObject obj)
		{
			obj.transform.position = new Vector2(-20,-20);
			obj.SetActive(false);
		}
	}
}