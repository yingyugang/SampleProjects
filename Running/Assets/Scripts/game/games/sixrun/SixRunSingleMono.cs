using UnityEngine;
using System.Collections;

namespace SixRun{
	public class SixRunSingleMono<T> : MonoBehaviour where T : SixRunSingleMono<T>{

		private static T t;

		public static T GetInstance(){
			if (t == null) {
				t = GameObject.FindObjectOfType(typeof(T)) as T;
				if (t == null) {
					GameObject go = new GameObject (t.name);
					t = go.AddComponent<T> ();
				}
			}
			return t;
		}

		protected virtual void Awake(){
			if(t==null){
				t = gameObject.GetComponent<T> ();
			}
		}
	}
}