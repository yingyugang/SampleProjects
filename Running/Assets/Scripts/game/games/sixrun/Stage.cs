using UnityEngine;
using System.Collections;

namespace SixRun{
	public class Stage : MonoBehaviour {

		public float speed;
		public Vector3 direct;
		Vector3 startPos;
		Transform trans;
		public GameObject[] prefabs;



		void Awake(){
			trans = transform;
			startPos = transform.position;
			direct = direct.normalized;
		}

		void OnEnable(){
			//trans.position = startPos;
		}
		float nextSpawn;
		// Update is called once per frame
		void Update () {
			if (GameManager.GetInstance ().isPaused)
				return;
		}

		IEnumerator _Spawn(){
			GameObject prefab = prefabs [Random.Range (0, prefabs.Length)];
			GameObject go = Instantiate (prefab);
			float t = Time.time + 20;
			while(t<Time.time){
				go.transform.position += direct * speed * Time.deltaTime;
				yield return null;
			}

		}


	}
}
