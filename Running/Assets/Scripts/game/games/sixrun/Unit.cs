using UnityEngine;
using System.Collections;
using Daruma;

namespace SixRun{
	
	public class Unit : MonoBehaviour {

		const string CLIP_RUN = "run";
		const string CLIP_MINJUMP = "minJump";
		const string CLIP_BIGJUMP = "bigJump";
		const string CLIP_BIGCHEATJUMP = "bigcheatJump";
		const string CLIP_SLIDE = "slide";
		const string CLIP_TOGGLECOLOR = "color";
		const string CLIP_DEFAULTCOLOR = "default";
		const string CLIP_FLY = "fly";


		public ItemData itemData;
		public Animator anim;

		SpriteRenderer sr;
		Transform trans;

		public GachaPosition posTween;

		public AnimationCurve jumpAnimCurve = AnimationCurve.Linear(0,0,1,1);
		public Vector3 jumpPos;
		public AnimationCurve upAnimCurve= AnimationCurve.Linear(0,0,1,1);
		public Vector3 upPos;
		public AnimationCurve leftAnimCurve= AnimationCurve.Linear(0,0,1,1);
		public Vector3 leftPos;
		public AnimationCurve rightAnimCurve= AnimationCurve.Linear(0,0,1,1);
		public Vector3 rightPos;
		public AnimationCurve downAnimCurve= AnimationCurve.Linear(0,0,1,1);
		public Vector3 downPos;

		public Transform shadow;
		public GachaPosition shadowPosTween;
		public GachaSize shadowSizeTween;

		Vector3 defaultFlyPos = new Vector3(0,7.5f,0);
		Vector3 upFlyPos = new Vector3(0,15f,0);
		Vector3 downFlyPos = Vector3.zero;
		Vector3 defaultPos = Vector3.zero;


		public int itemIndex;
		public bool isCollected;
		bool isFlying;
		public int status = 0;//0 run;1 jump;2 up;3 down;4 left;5 right
		public float nextItemSoundTime;

		void Awake(){
			gameObject.layer = GameManager.PLAYER_LAYER;
			posTween = GetComponent<GachaPosition> ();
			posTween.onTweenDone = PlayRun;
			sr = GetComponentInChildren<SpriteRenderer> (true);
			anim = GetComponent<Animator> ();
			trans = transform;
			shadow = trans.parent.FindChild ("Shadow");
			if (shadow != null) {
				shadowPosTween = shadow.GetComponent<GachaPosition> ();
				shadowSizeTween = shadow.GetComponent<GachaSize> ();
			}
		}

		void Update(){
			shadow.localPosition = new Vector3(trans.localPosition.x,shadow.localPosition.y,shadow.localPosition.z);
		}

		void OnTriggerEnter(Collider other){
			if(other.gameObject.layer == GameManager.OBSTACLE_LAYER){
				Item item = other.transform.parent.GetComponent<Item> ();
				MapDetailData mapDetailData = item.mapDetailData;
				if (item.itemData.effectType == 0 || item.itemData.effectType == 1) {
					if(isFlying){
						item.isCollected = true;
						other.transform.parent.gameObject.SetActive (false);
					}
					else if (!item.isCollected) {
						//other.transform.parent.gameObject.SetActive (false);
						if(!GameManager.GetInstance().imm && Time.time - GameManager.GetInstance().immEndTime > 2 && ComponentConstant.SOUND_MANAGER!=null)
							ComponentConstant.SOUND_MANAGER.Play(SoundEnum.se13_miss);
						item.isCollected = true;
						if (!isToggleColor) {
							GameManager.GetInstance ().ObstacleHit ();
						} else {
							for(int i=0;i<GameManager.GetInstance().players.Count;i++){
								if (!GameManager.GetInstance ().players [i].GetComponent<Unit> ().isToggleColor)
									GameManager.GetInstance ().players [i].GetComponent<Unit> ().ToggleColor ();
								else
									GameManager.GetInstance ().players [i].GetComponent<Unit> ().toggleTime = 0;
							}
						}
					}
				} else if (item.itemData.effectType == 2) {
					if (isToggleColor)
						return;


					if (!item.isCollected){
						#if UNITY_EDITOR
						Debug.Log ("Score");
						#endif
						if (nextItemSoundTime <= Time.time) {
							if (ComponentConstant.SOUND_MANAGER != null) {
								ComponentConstant.SOUND_MANAGER.Play (SoundEnum.se11_scoreitem);
								for(int i=0;i<GameManager.GetInstance().playerUnits.Count;i++){
									GameManager.GetInstance().playerUnits[i].nextItemSoundTime = Time.time + 0.2f;
								}
							}
						}
							
						item.isCollected = true;
						other.transform.parent.gameObject.SetActive (false);
						GameManager.GetInstance ().ItemHit (item);

						//if (GameManager.GetInstance ().curCombo >= 1) {
							
							//Vector3 pos = GameManager.GetInstance ().WorldPosToUIPos (trans.position);
							
							//GameManager.GetInstance ().effectQueue.Enqueue (pos);

							//effect.GetComponent<InfoCombo> ().SetTextCombo (GameManager.GetInstance ().curCombo);
						//}

					}
				} else if (item.itemData.effectType == 3) {
					if (!item.isCollected)	{
						Debug.Log ("Imm");
						other.transform.parent.gameObject.SetActive (false);
						GameManager.GetInstance ().ImmHit (item.mapDetailData);
						item.isCollected = true;
					}
				}
			}
		}

		public void PauseAnim(){
			anim.speed = 0;
		}

		public void ResumeAnim(){
			anim.speed = 1;
		}

		public void PlayJump(){
			anim.Play (CLIP_MINJUMP);
		}

		public void PlayTwiceJump(){
//			anim.Play (CLIP_BIGJUMP);
			CheatData mCheatData = CheatController.GetLastMatchCheat ();
			if (mCheatData!=null && mCheatData.key == "1") {
				anim.Play (CLIP_BIGCHEATJUMP);
			} else {
				anim.Play (CLIP_BIGJUMP);
			}
		}

		public void PlayRun(){
			if (!isFlying) {
				anim.Play (CLIP_RUN);
			}
			status = 0;
		}

		public void PlaySlide(){
			anim.Play (CLIP_SLIDE);
		}

		public void PlayFly(){
			anim.Play (CLIP_FLY);
		}

		public void ToggleColor(){
			StartCoroutine ("_ToggleColor");
		}
		public float toggleTime = 0;
		IEnumerator _ToggleColor(){
			isToggleColor = true;
			PlayColor ();
			toggleTime = 0;
			while(toggleTime < GameParams.GetInstance().GetToggleTime()){
				toggleTime += Time.deltaTime;
				yield return null;
			}
			isToggleColor = false;
			PlayDefaultColor ();
		}
		public bool isToggleColor = false;

		public void PlayColor(){
			anim.Play (CLIP_TOGGLECOLOR,1);
		}

		public void PlayDefaultColor(){
			anim.Play (CLIP_DEFAULTCOLOR,1);
		}

		void Move(AnimationCurve curve,Vector3 toPos,float delay){
			if (posTween != null) {
				//if (posTween.IsPlaying ())
				//	return;
				posTween.ResetToBegin();
				posTween.curve = curve;
				posTween.to = toPos;
				posTween.delay = delay;
				posTween.Play ();
			}
		}

		public void StopFly(){
			isFlying = false;
			posTween.ResetToBegin ();
			posTween.from = defaultPos;
			trans.localPosition = defaultPos;
			PlayRun ();
		}

		public void Fly(){
			isFlying = true;
			status = 0;
			posTween.ResetToBegin ();
			trans.localPosition = defaultFlyPos;
			posTween.from = defaultFlyPos;
			PlayFly ();
		}

		public void FlyMove(Vector2 deltaPos){
			if(isFlying){
				float y = Mathf.Clamp (trans.localPosition.y + deltaPos.y * GameManager.GetInstance().heightFactor,downPos.y,upPos.y);
				//float y = Mathf.Clamp (trans.localPosition.y + deltaPos.y / 10*0.695f ,downPos.y,upPos.y);

				trans.localPosition = new Vector3 (0,y,0);
			}	
		}

		public void Jump(){
			if (isFlying || GameManager.GetInstance().isPaused || (status != 0 && status!=1))
				return;
			//if(GameManager.GetInstance().nextHandleTime <= Time.time){
				status = 1;
				Move (jumpAnimCurve,jumpPos,0);
				if(shadowSizeTween!=null){
					shadowSizeTween.ResetToBegin ();
					shadowSizeTween.curve = jumpAnimCurve;
					shadowSizeTween.to = Vector3.one * 3;
					shadowSizeTween.Play ();
				}
				PlayJump ();
			//}
		}

		public void TwiceJump(){
			if (isFlying|| GameManager.GetInstance().isPaused || (status != 0 && status!=1))
				return;
			Move (upAnimCurve, upPos, 0);
			status = 2;
			if (shadowSizeTween != null) {
				shadowSizeTween.ResetToBegin ();
				shadowSizeTween.curve = upAnimCurve;
				shadowSizeTween.to = Vector3.one * 2;
				shadowSizeTween.Play ();
			}
			PlayTwiceJump ();
		}

		public void MoveDown(){
			if (isFlying|| GameManager.GetInstance().isPaused || (status != 0 && status!=1))
				return;
			status = 3;
			Move (downAnimCurve, downPos, 0);
			shadowSizeTween.ResetToBegin ();
			PlaySlide ();
		}

		public void MoveLeft(){
			if (isFlying|| GameManager.GetInstance().isPaused || (status != 0 && status!=1))
				return;
			status = 4;
			Move (leftAnimCurve,posTween.from + leftPos,0);
			shadowSizeTween.ResetToBegin ();
			/*
			if(shadowPosTween!=null){
				shadowPosTween.curve = leftAnimCurve;
				shadowPosTween.to = shadowPosTween.from + leftPos;
				shadowPosTween.delay = 0;
				shadowPosTween.Play ();
			}*/
			anim.Play (CLIP_RUN);
		}

		public void MoveRight(){
			if (isFlying|| GameManager.GetInstance().isPaused || (status != 0 && status!=1))
				return;
			status = 5;
			Move (rightAnimCurve,posTween.from + rightPos,0);
			shadowSizeTween.ResetToBegin ();
			/*
			if(shadowPosTween!=null){
				shadowPosTween.curve = rightAnimCurve;
				shadowPosTween.to = shadowPosTween.from + rightPos;
				shadowPosTween.delay = 0;
				shadowPosTween.Play ();
			}*/
			anim.Play (CLIP_RUN);
		}


	}
}
