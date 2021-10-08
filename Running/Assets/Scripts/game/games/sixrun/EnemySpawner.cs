using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace SixRun{
	public class EnemySpawner : SixRunSingleMono<EnemySpawner> {

		//public List<GameObject> playerPrefabs;
		public List<GameObject> obstaclePrefabs;
		public Dictionary<string,GameObject> obstaclePrefabDic;

		public List<GameObject> effectPrefabs;

		public List<Transform> spawnPoints;

		public Dictionary<GameObject,List<GameObject>> pool;

		public float unSpawnDelay = 20;

		protected override void Awake ()
		{
			base.Awake ();
			obstaclePrefabDic = new Dictionary<string, GameObject> ();
			for(int i=0;i<obstaclePrefabs.Count;i++){
				obstaclePrefabDic.Add (obstaclePrefabs[i].name,obstaclePrefabs[i]);
			}
			pool = new Dictionary<GameObject, List<GameObject>> ();
		}

		GameObject Spawn(GameObject prefab){
			GameObject go = null;
			if (pool.ContainsKey (prefab) && pool [prefab] != null && pool [prefab].Count > 0) {
				go = pool [prefab] [0];
				pool [prefab].RemoveAt (0);
			} else {
				go = Instantiate (prefab) as GameObject;
			}
			return go;
		}

		IEnumerator UpSpawn(GameObject prefab,GameObject go,float delay){
			float t = 0;
			while(t<delay){
				if(!GameManager.GetInstance().isPaused){
					t += Time.deltaTime;
				}
				yield return null;
			}
			if(!pool.ContainsKey(prefab)){
				pool.Add (prefab,new List<GameObject>());
			}
			go.SetActive (false);
			pool [prefab].Add (go);
		}

		public HashSet<int> SpawnObstacle(MapDetailData mapDetailData){
			//int index = mapDetailData.itemData.id - 1;
			//int line = mapDetailData.appear_column;
			HashSet<int> holeIndex = new HashSet<int>();
			if (mapDetailData.itemData1 != null) {
				SpawnObstacle (mapDetailData, mapDetailData.itemData1, 0, holeIndex);
			}
			if (mapDetailData.itemData2 != null) {
				SpawnObstacle (mapDetailData,mapDetailData.itemData2,1, holeIndex);
			}
			if (mapDetailData.itemData3 != null) {
				SpawnObstacle (mapDetailData,mapDetailData.itemData3,2, holeIndex);
			}
			if (mapDetailData.itemData4 != null) {
				SpawnObstacle (mapDetailData,mapDetailData.itemData4,3, holeIndex);
			}
			if (mapDetailData.itemData5 != null) {
				SpawnObstacle (mapDetailData,mapDetailData.itemData5,4, holeIndex);
			}
			if (mapDetailData.itemData6 != null) {
				SpawnObstacle (mapDetailData,mapDetailData.itemData6,5, holeIndex);
			}
			return holeIndex;
		}

		void SpawnObstacle(MapDetailData mapDetailData,ItemData itemData,int line,HashSet<int> holeIndex){
			if (Random.Range (0, 100) < mapDetailData.item_appear_percentage_supplementary + itemData.percentage) {
				if (itemData.effectType == 0) {
					holeIndex.Add (5-line);
				} 
				if(itemData.effectType == 4){
					ItemData randomItemData = GameParams.GetInstance ().GetRandomItem();
					itemData = randomItemData;
				}
				//Debug.Log (itemData.name);
				GameObject go = SpawnObstacle (itemData.itemResource,line);
				if (go != null) {
					Item item = go.GetComponent<Item> ();
					item.mapDetailData = mapDetailData;
					item.itemData = itemData;
					if (itemData.effectType == 1 || itemData.effectType == 0) {
						item.SetPos (itemData.height_offset, itemData.height, itemData.width);//对于障碍物只计算item里面的默认高度。
					} else {
						item.SetPos (mapDetailData.height_offset + itemData.height_offset, itemData.height, itemData.width);
					}
				}
			}
		}

		public GameObject SpawnObstacle(string obstacleName,int lineIndex){
			Vector3 pos = GroundSpawner.GetInstance().startPosList[5 - lineIndex] - GroundSpawner.GetInstance().transform.forward * (GroundSpawner.startGridCount + 1) * GroundSpawner.GetInstance().scale;
			if (obstacleName==null || obstaclePrefabDic.ContainsKey (obstacleName)) {
				GameObject go = Spawn (obstaclePrefabDic [obstacleName]); 
				go.transform.position = pos;
				go.SetActive (true);
				StartCoroutine (UpSpawn (obstaclePrefabDic [obstacleName], go, unSpawnDelay));
				return go;
			} else {
				return null;
			}
		}
		public float comboUnSpawnDelay = 0.5f;
		public GameObject SpawnComboEffect(){
			GameObject go = Spawn(effectPrefabs[0]);
			go.SetActive (true);
			StartCoroutine(UpSpawn (effectPrefabs[0],go,comboUnSpawnDelay));
			return go;
		}

		public void SpawnDeadEffect(Vector3 pos){
			GameObject go = Spawn(effectPrefabs[0]);
			go.transform.position = pos;
			go.SetActive (true);
			StartCoroutine(UpSpawn (effectPrefabs[0],go,unSpawnDelay));
   		}



	}
}

