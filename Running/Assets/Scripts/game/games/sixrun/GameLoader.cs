using UnityEngine;
using System.Collections;
namespace SixRun{
	public class GameLoader  : SixRunSingleMono<GameLoader> {

		void Awake(){	
			StartCoroutine (ComponentConstant.ASSETBUNDLE_RESOURCES_GETTER.GetResource<GameObject> ("SixRun", "SixRun",GetResource<GameObject>));
		}

		private void GetResource<T> (T t) where T : Object
		{
			if(t!=null){
				Instantiate (t);
			}
		}
	}
}
