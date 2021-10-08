using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

namespace scene{

	public class CharacterAnim : SingleMonoBehaviour<CharacterAnim> {

		public List<SerifImageData> currentImages;
		public Dictionary<string,Sprite> spriteDic;
		public List<Sprite> spriteList;
		public List<Texture2D> txtList;
		public Image img;
		public RectTransform rectTransform;

		void Awake(){
			base.Awake ();
			spriteDic = new Dictionary<string, Sprite> ();
			for(int i=0;i<spriteList.Count;i++){
				spriteDic.Add (spriteList[i].name,spriteList[i]);
			}
			rectTransform = GetComponent<RectTransform>();
			img.gameObject.SetActive (false);
			CheatData cheatData = CheatController.GetLastMatchCheat ();
			if(cheatData!=null){
				LoadCheat (cheatData);
			}
		}

		void LoadCheat(CheatData cheatData){

			StartCoroutine(ComponentConstant.ASSETBUNDLE_RESOURCES_GETTER.GetAssetBundle ("guess_game_cheat_images_" + cheatData.key, GetResource<AssetBundle>));

//			StartCoroutine (ComponentConstant.ASSETBUNDLE_RESOURCES_GETTER.GetAllResourcesList<Texture2D> ("guess_game_cheat_images_" + cheatData.key, (List<Texture2D> list) => {
//				txtList = list;
//				spriteDic = TextureToSpriteConverter.ConvertToSpriteDictionary (list);
//				//;
//			}));
		}

		AssetBundle ab;
		void GetResource<T>(T t){
			ab = t as AssetBundle;
			Texture2D[] txts = ab.LoadAllAssets<Texture2D> ();
			spriteDic = TextureToSpriteConverter.ConvertToSpriteDictionary (new List<Texture2D>(txts));
			//ab.Unload (true);
		}

		public void DestorySprites(){
			currentImages = null;
			//ab.Unload (true);
			foreach(string key in spriteDic.Keys){
				Sprite sprite = spriteDic [key];
				Destroy (sprite);
			}
			Resources.UnloadUnusedAssets ();
			Debug.Log ("DestorySprites");
		}

		void OnDisable(){
			DestorySprites ();
		}

		public void Play(List<SerifImageData> images){
			mIsPlaying = true;
			if(currentImages==images){
				return;
			}
			currentImages = images;
			mCurrentIndex = 0;
			currentInterval = 0;
			SetImage (mCurrentIndex);
		}

		public void Pause(){
			mIsPlaying = false;
		}

		public void Resume(){
			mIsPlaying = true;
		}

		int mCurrentIndex = 0;
		public float interval = 0.2f;
		float currentInterval = 0;
		bool mIsPlaying = false;
		void Update(){
			if((GameManager.GetInstance()!=null && GameManager.GetInstance().isPause) || !mIsPlaying){
				return;
			}
			if (currentInterval >= interval) {
				if (currentImages != null && currentImages.Count > 0) {
					mCurrentIndex = mCurrentIndex % currentImages.Count;
					SetImage (mCurrentIndex);
					/*
					if (spriteDic.ContainsKey (currentImages [mCurrentIndex].image)) {
						img.sprite = spriteDic [currentImages [mCurrentIndex].image];
						img.SetNativeSize ();
						rectTransform.localScale = Vector3.one * currentImages [mCurrentIndex].size;
						rectTransform.anchorMin = new Vector2 (currentImages [mCurrentIndex].offset_x / 100, currentImages [mCurrentIndex].offset_y / 100);
						rectTransform.anchorMax = rectTransform.anchorMin;
						rectTransform.anchoredPosition3D = Vector3.zero;
						if (!img.gameObject.activeInHierarchy) {
							img.gameObject.SetActive (true);
							UIManager.GetInstance ().questionText.transform.parent.gameObject.SetActive (true);
						}
					}*/
					mCurrentIndex++;
				}
				currentInterval = 0;
			} else {
				currentInterval += Time.deltaTime;
			}
		}

		void SetImage(int index){
			if (currentImages == null || currentImages.Count == 0)
				return;
			if (spriteDic.ContainsKey (currentImages [index].image)) {
				img.sprite = spriteDic [currentImages [index].image];
				img.SetNativeSize ();
				rectTransform.localScale = Vector3.one * currentImages [index].size;
				rectTransform.anchorMin = new Vector2 (currentImages [index].offset_x / 100, currentImages [index].offset_y / 100);
				rectTransform.anchorMax = rectTransform.anchorMin;
				rectTransform.anchoredPosition3D = Vector3.zero;
				if (!img.gameObject.activeInHierarchy) {
					img.gameObject.SetActive (true);
					UIManager.GetInstance ().questionText.transform.parent.gameObject.SetActive (true);
				}
			}
		}


	}

}
