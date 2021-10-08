using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace SixRun{
	public class SixRunBtn : MonoBehaviour {

		public Unit unit;
		Button btn;
		Image img;
		Color disableColor = new Color(0.5f,0.5f,0.5f,1);
		public Sprite cheatSprite;
		EventTrigger et;
		void Awake(){
			//btn = GetComponent<Button> ();
			img = transform.parent.GetComponent<Image> ();
			//btn.onClick.AddListener (OnBtnClick);
			CheatData mCheatData = CheatController.GetLastMatchCheat ();
			if (mCheatData!=null && mCheatData.key != "1") {
				img.sprite = cheatSprite;
			}

		}
		bool mIsEnable;
		void Update(){
			if (unit.status != 0 || GameManager.GetInstance().imm) {
				img.color = disableColor;
				//img.material = GUIManager.GetInstance ().grayMat;
				mIsEnable = false;
			} else {
				img.color = Color.white;
				//img.material = null;
				mIsEnable = true;
			}

		}

		public void OnBtnClick(){
			if (unit.status != 0)
				return;
			unit.posTween.duration = GameParams.GetInstance ().GetJumpDuration();
			unit.shadowPosTween.duration = GameParams.GetInstance ().GetJumpDuration();
			unit.shadowSizeTween.duration = GameParams.GetInstance ().GetJumpDuration();
			Debug.Log (unit.posTween.duration);
			//GameManager.GetInstance ().isPress = false;
			Jump ();
		}

		public void OnBtnEnter(){
			//OnBtnClick ();
			Debug.Log("OnBtnEnter");
		}

		public void Jump(){
			//if(GameManager.GetInstance().nextHandleTime <= Time.time){
				if (ComponentConstant.SOUND_MANAGER != null) 
					ComponentConstant.SOUND_MANAGER.Play (SoundEnum.SE36_sixrun_jump);
				unit.Jump ();
			//}
		}
	}
}