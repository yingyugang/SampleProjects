using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace scene{
	public class PoolManager : SingleMonoBehaviour<PoolManager> {

		public List<GameObject> obstaclePrefabs;
		public Dictionary<string,GameObject> obstaclePrefabDic;
		public Dictionary<GameObject,List<GameObject>> pool;

		public Dictionary<int,List<GameObject>> gameObjectForUnSpawn;
		public float unSpawnDelay = 20;

		protected override void Awake ()
		{
			base.Awake ();
			obstaclePrefabDic = new Dictionary<string, GameObject> ();
			for(int i=0;i<obstaclePrefabs.Count;i++){
				obstaclePrefabDic.Add (obstaclePrefabs[i].name,obstaclePrefabs[i]);
			}
			pool = new Dictionary<GameObject, List<GameObject>> ();
			gameObjectForUnSpawn = new Dictionary<int,List<GameObject>> ();
		}

		int mTimeSecond =0;
		List<GameObject> mCurrentUnSpawnGOs;
		void Update(){
			mTimeSecond = Mathf.RoundToInt (Time.time);
			if(gameObjectForUnSpawn.ContainsKey(mTimeSecond)){
				mCurrentUnSpawnGOs = gameObjectForUnSpawn[mTimeSecond];
				gameObjectForUnSpawn.Remove (mTimeSecond);
				for(int i=0;i<mCurrentUnSpawnGOs.Count;i++){
					mCurrentUnSpawnGOs [i].SetActive (false);
				}
			}
		}

		GameObject Spawn(GameObject prefab){
			GameObject go = null;
			if (pool.ContainsKey (prefab) && pool [prefab] != null && pool [prefab].Count > 0) {
				go = pool [prefab] [0];
				pool [prefab].RemoveAt (0);
			} else {
				go = Instantiate (prefab) as GameObject;
				PoolGameObject pgo = GetOrAddComponent<PoolGameObject> (go);
				pgo.prefab = prefab;
			}
			return go;
		}

		public void UnSpawn(GameObject go){
			PoolGameObject pgo = go.GetComponent<PoolGameObject> ();
		}


		T GetOrAddComponent<T>(GameObject go) where T:MonoBehaviour{
			T t = go.GetComponent<T> ();
			if(t==null){
				t = go.AddComponent<T> ();
			}
			return t;
		}

	}

	public class PoolGameObject:MonoBehaviour{
		public GameObject prefab;
	}


}

