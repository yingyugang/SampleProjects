using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace scene{
	
	public class Dialog : MonoBehaviour {

		public List<Sprite> dialogSprites;
		public Dictionary<string,Sprite> spriteDic;
		public List<string> currentSprites;
		public SpriteRenderer sr;

		void Awake(){
			spriteDic = new Dictionary<string, Sprite> ();
			for(int i=0;i<dialogSprites.Count;i++){
				spriteDic.Add (dialogSprites[i].name,dialogSprites[i]);
			}
		}

		int mCurrentIndex = 0;
		public float interval = 0.2f;
		float currentInterval = 0;
		bool mIsPlaying = false;
		void Update(){
			if(GameManager.GetInstance().isPause && mIsPlaying){
				return;
			}
			if (currentInterval >= interval) {
				if (currentSprites != null && currentSprites.Count > 0) {
					mCurrentIndex = mCurrentIndex % currentSprites.Count;
					string txt = currentSprites [mCurrentIndex];
					if (spriteDic.ContainsKey (txt)) {
						sr.sprite = spriteDic [txt];
					}
					mCurrentIndex++;
				}
				currentInterval = 0;
			} else {
				currentInterval += Time.deltaTime;
			}
		}
	
		public void Play(List<string> sprites){
			currentSprites = sprites;
			mIsPlaying = true;
		}	
	
		public void Stop(){
			currentInterval = 0;
			mCurrentIndex = 0;
			mIsPlaying = false;
		}



	}
}
