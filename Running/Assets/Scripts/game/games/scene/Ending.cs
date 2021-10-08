using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace scene{
	public class Ending : MonoBehaviour {

		public Sprite[] sprites;
		public Image img;
		public Image imgBack;
		public float inverval = 0.2f;
		public float nextTime = 0;
		public int index = 0;

		void Start(){
	
		}

		void OnEnable(){
			ShowBack ();
		}

		void Update(){
			if (nextTime < Time.time) {
				nextTime = Time.time + inverval;
				index++;
				index = index % sprites.Length;
				img.sprite = sprites [index];
			}
		}

		public void ShowBack(){
			StartCoroutine (_ShowBack());
		}

		IEnumerator _ShowBack(){
			float t = 0;
			while(t < 2f){
				t += Time.deltaTime;
				imgBack.color = new Color (imgBack.color.r,imgBack.color.g,imgBack.color.b,t / 2f * 180f / 255f);
				yield return null;
			}
			for(int i=0;i<transform.childCount;i++){
				transform.GetChild (i).gameObject.SetActive (true);
			}
		}
	}

}
