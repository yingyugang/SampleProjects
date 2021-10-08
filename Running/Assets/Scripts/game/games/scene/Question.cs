using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public enum QuestionStatus{None,Beginning,Ending,NextQuestion,BeforeQuestion,Dialog,Answering,NextStep,SelectQuestionType,SelectCharacter,WaitingTab};

public enum DialogType{
	DIALOG_QUESTION_BEGIN_ID = 1,//ギーム开始时
	DIALOG_ANSWER_CORRECT_ID = 2,//回答正确时
	DIALOG_ANSWER_INCORRECT_ID = 3,//回答不正确时
	DIALOG_1_TO_2_ID = 4,//ステージ 1 と　ステージ　２
	DIALOG_1_TO_2_ID_1 = 41,
	DIALOG_2_TO_3_ID = 5,//ステージ　２　と　ステージ　３
	DIALOG_2_TO_3_ID_1 = 51,
	DIALOG_3_TO_4_ID = 6,//ステージ　３　と　ステージ　４
	DIALOG_QUESTION_TIMEUP_ID = 7,//問題时间到
	/***************  わたしの ****************/
	QUESTION_ASK_ORDER_ID = 8,//问题序号
	QUESTION_ASK_TYPE1_ID = 9,//问题1
	QUESTION_ASK_TYPE2_ID = 10,//问题2
	QUESTION_ASK_TYPE3_ID = 11,//问题3
	QUESTION_ASK_TYPE4_ID = 12//问题4
};


namespace scene{
	
	public class Question : SingleMonoBehaviour<Question> {

		public delegate void OnDialogDone ();
		OnDialogDone onDialogDone;

		public GameObject defaultCardGo;
		public GameObject cardGo;

		public SpriteRenderer card;
		public SpriteRenderer cardFrame;

		public Material maskMat;
		public Material defaultMat;
		public GameObject bigMask;

		public GameObject groupMask;
		private int mMaskType = 0;
		public QuestionStatus status = QuestionStatus.Beginning;
		QuestionData mCurrentQuestion;
		public int currentStep = 0;

		float mQuestionDuration = 10;

		float mQuestionTimeBonusOfStep4= 0;

		float mCurrentStepDuration = 0;
		float mCurrentStepUsed = 0;

		float mCurrentQuestionUsed = 0;
		int mCurrentStepQuestionCount = 0;
		int mCurrentStepQuestionIndex = 0;

		int dialogType = 1;

		QuestionStatus preStatus = QuestionStatus.None;
		QuestionStatus preStatus1 = QuestionStatus.None;
		QuestionStatus dialogDoneStatus;

		protected override void Awake(){
			defaultMat = card.material;
			mInactiveGroupMaskItems = new List<GameObject> ();
			mGroupMaskItems = new List<GameObject> ();
			for(int i=0;i<groupMask.transform.childCount;i++){
				mGroupMaskItems.Add(groupMask.transform.GetChild (i).gameObject);
			}
		}

		void Start(){
			Header.Instance.SetLife (LifeType.Time,Mathf.RoundToInt(GameParams.GetInstance ().paramData.last_stage_bonus_time));
			Header.Instance.isPause = true;
		}
	
		void Update(){
			if (GameManager.GetInstance ().isPause)
				return;
			CheckClick();
			//Enter Status
			if(preStatus1!=status){
				preStatus1 = status;
				switch(status){
				case QuestionStatus.Answering:
					//GameManager.GetInstance ().DisableHeaderBtn ();
					if(currentStep==4){
						Header.Instance.isPause = false;
					}
					UIManager.GetInstance ().EnableControllers (true);
					break;
				case QuestionStatus.SelectQuestionType:
					GameManager.GetInstance ().EnableHeaderBtn ();
					UIManager.GetInstance ().EnableControllers (true);
					break;
				case QuestionStatus.SelectCharacter:
					GameManager.GetInstance ().EnableHeaderBtn ();
					UIManager.GetInstance ().EnableControllers (true);
					break;
				default:
					//GameManager.GetInstance ().EnableHeaderBtn ();
					UIManager.GetInstance ().EnableControllers (false);
					break;
				}
			}

			switch(status){
			case QuestionStatus.Beginning:
				if (GameCountDownMediator.didEndCountDown) {
					BeginDialog ((int)DialogType.DIALOG_QUESTION_BEGIN_ID);
					if (ComponentConstant.SOUND_MANAGER != null) {
						ComponentConstant.SOUND_MANAGER.Play (SoundEnum.bgm12_quiz, GetAudioSource);
					}
				}
				break;
			case QuestionStatus.NextStep:
				if (currentStep >= 4) {
					GameManager.GetInstance ().PreGameOver ();
					status = QuestionStatus.Ending;
					return;
				}
				currentStep++;
				UIManager.GetInstance ().SetCurrentStage (currentStep);
				mCurrentStepUsed = 0;
				mCurrentStepQuestionIndex = 0;
				switch (currentStep) {
				case 1:
					mQuestionDuration = GameParams.GetInstance ().paramData.stage1_time_limit;
					mCurrentStepDuration = GameParams.GetInstance ().paramData.last_stage_bonus_time;
					Header.Instance.isPause = true;
					Header.Instance.SetLifeTime (Mathf.RoundToInt(mCurrentStepDuration));
					Header.Instance.SetLife (LifeType.Time,Mathf.RoundToInt(mCurrentStepDuration));
					//mCurrentStepDuration = GameParams.GetInstance().paramData.stage1_questions * mQuestionDuration;
					mCurrentStepQuestionCount = GameParams.GetInstance().paramData.stage1_questions;
					break;
				case 2:
					mQuestionDuration = GameParams.GetInstance ().paramData.stage2_time_limit;
					Header.Instance.isPause = true;
					//mCurrentStepDuration = GameParams.GetInstance ().paramData.stage2_questions * mQuestionDuration;
					mCurrentStepQuestionCount = GameParams.GetInstance().paramData.stage2_questions;;
					break;
				case 3:
					mQuestionDuration = GameParams.GetInstance ().paramData.stage3_time_limit;
					Header.Instance.isPause = true;
					//mCurrentStepDuration = GameParams.GetInstance ().paramData.stage3_questions * mQuestionDuration;
					mCurrentStepQuestionCount = GameParams.GetInstance().paramData.stage3_questions;;
					break;
				case 4:
					//mCurrentStepDuration = GameParams.GetInstance ().paramData.last_stage_bonus_time + mQuestionTimeBonusOfStep4;
					mCurrentStepQuestionCount = 99999999;
					break;
				}
				if (currentStep == 2) {
					BeginDialog ((int)DialogType.DIALOG_1_TO_2_ID,OnShowQuestionType);
				}
				else if(currentStep == 3){
					BeginDialog ((int)DialogType.DIALOG_2_TO_3_ID,OnShowCharacterType);
				}
				else if(currentStep == 4){
					BeginDialog ((int)DialogType.DIALOG_3_TO_4_ID,null);
					//GameManager.GetInstance ().ShowEffect (mQuestionTimeBonusOfStep4);
				}
				else {
					status = QuestionStatus.NextQuestion;
				}
				break;
			case QuestionStatus.SelectQuestionType:
				break;
			case QuestionStatus.NextQuestion:
				if (mCurrentStepQuestionIndex >= mCurrentStepQuestionCount) {
					status = QuestionStatus.NextStep;
					return;
				}
				if (ComponentConstant.SOUND_MANAGER != null)
					ComponentConstant.SOUND_MANAGER.Play (SoundEnum.se38_quiz_questions);
				BeginDialog ((int)DialogType.QUESTION_ASK_ORDER_ID,OnQuestionNum);
				GameManager.GetInstance ().DisableHeaderBtn ();
				break;
			case QuestionStatus.Dialog:
				UpdateDialog ();
				break;
			case QuestionStatus.Answering:
				UpdateMask ();
				UpdateTime ();
				break;
			case QuestionStatus.WaitingTab:
				break;
			case QuestionStatus.Ending:
				break;
			default:
				break;
			}
			//Exit Status
			if(preStatus != status){
				preStatus = status;
			}
		}

		public void GetAudioSource (AudioSource audio)
		{
			PauseManager.s_Instance.Init (GameManager.GAME_ID, audio);
		}

		float mMinClickInterval = 1f;
		float mPressTime = 0;
		void CheckClick(){
			#if UNITY_EDITOR
			if(Input.GetMouseButtonDown(0)){
				mPressTime = Time.realtimeSinceStartup;
			}
			if(Input.GetMouseButtonUp(0)){
				if(Time.realtimeSinceStartup - mPressTime < mMinClickInterval){
					JumpDialog ();
				}
			}
			#else
			if(Input.touchCount>0){
				if (Input.GetTouch (0).phase == TouchPhase.Began) {
					mPressTime = Time.realtimeSinceStartup;
				}
				if (Input.GetTouch (0).phase == TouchPhase.Ended) {
					if(Time.realtimeSinceStartup - mPressTime < mMinClickInterval){
						JumpDialog ();
					}
				}
			}
			#endif
		}

		void ShowDefaultCard(){
			defaultCardGo.SetActive (true);
			cardGo.SetActive (false);
		}

		bool mShowMask =false;
		void ShowCard(){
			defaultCardGo.SetActive (false);
			cardGo.SetActive (true);
			//mMaskType = Random.Range (0, 3); //TODO，也许这里客户会要求权重随机或者配置文件里面读取。
			//Mask时间要减成2s。TODO
			mShowMask = false;
			if(currentStep == 4 && (mCurrentStepDuration - mCurrentStepUsed) <= GameParams.GetInstance().paramData.item_span_second){
				mShowMask = Random.Range(0,100) < GameParams.GetInstance().paramData.item_appear_percentage ? true : false;
				Debug.Log ("mShowMask:" + mShowMask);
				mMaskType = 2;
				if(mShowMask)
					ResetMask ();
			}
		}

		public void OnSelectQuestionType(int index){
			GameParams.GetInstance ().SetQuestionOfStep2 (index + 1);
			this.BeginDialog ((int)DialogType.DIALOG_1_TO_2_ID_1, OnSelectTypeDone);
		}

		void OnSelectTypeDone(){
			status = QuestionStatus.NextQuestion;
		}

		public void OnSelectCharacter(int index){
			CharacterData data = GameParams.GetInstance ().selectAbleCharaList [index];
			GameParams.GetInstance ().SetQuestionOfStep3 (data.id);
			this.BeginDialog ((int)DialogType.DIALOG_2_TO_3_ID_1, OnSelectTypeDone);
		}

		void OnShowCharacterType(){
			UIManager.GetInstance ().SetQuestionTime (0,1);
			status = QuestionStatus.SelectCharacter;
			UIManager.GetInstance().ShowCharacters();
		}

		void OnShowQuestionType(){
			UIManager.GetInstance ().SetQuestionTime (0,1);
			status = QuestionStatus.SelectQuestionType;
			UIManager.GetInstance ().ShowQuestionType ();
		}

		void OnQuestionNum(){
			mCurrentStepQuestionIndex++;
			mCurrentQuestionUsed = 0;
			mCurrentQuestion = GameParams.GetInstance ().GetQuestion (currentStep);
			GetSprite (GameParams.GetInstance ().cardDic [mCurrentQuestion.card_id]);
			int dialogType = (int)DialogType.QUESTION_ASK_TYPE1_ID;
			switch(mCurrentQuestion.question_type){
			case 1:
				dialogType = (int)DialogType.QUESTION_ASK_TYPE1_ID;
				break;
			case 2:
				dialogType = (int)DialogType.QUESTION_ASK_TYPE2_ID;
				break;
			case 3:
				dialogType = (int)DialogType.QUESTION_ASK_TYPE3_ID;
				break;
			case 4:
				dialogType = (int)DialogType.QUESTION_ASK_TYPE4_ID;
				break;
			}
			mCurrentQuestionUsed = 0;
			UIManager.GetInstance ().SetQuestionTime (mCurrentQuestionUsed,mQuestionDuration);
			UIManager.GetInstance ().ClearControllerText ();
		
			BeginDialog (dialogType,OnQuestion);
		}

		void OnQuestion(){
			ShowCard ();

			status = QuestionStatus.None;
			StartCoroutine (_ShowQuestionText());
		}

		IEnumerator _ShowQuestionText(){
			yield return new WaitForSeconds (0.5f);
			status = QuestionStatus.Answering;
			UIManager.GetInstance ().SetQuestion (mCurrentQuestion);
		}


		float mCurrentMaskDuration = 0;
		float mTotalMaskDuration = 0;
		float mTotalMaskDuration0 = 2;
		float mTotalMaskDuration1 = 2;
		float mTotalMaskDuration2 = 2;
		float mMinMosaicSize = 1;
		float mMaxMosaicSize = 25;

		Vector3 mDefaultBigMaskSize = new Vector3(1.5f,1.5f,1.5f);
		Vector3 mTargetBigMaskSize = Vector3.zero;

		List<GameObject> mGroupMaskItems;
		List<GameObject> mInactiveGroupMaskItems;

		void ResetMask(){
			mCurrentMaskDuration = 0;
			switch(mMaskType){
			case 0:
				card.material = maskMat;
				maskMat.SetFloat ("_MosaicSize", mMaxMosaicSize);
				bigMask.SetActive (false);
				groupMask.SetActive (false);
				mTotalMaskDuration = mTotalMaskDuration0;
				break;
			case 1:
				card.material = defaultMat;
				maskMat.SetFloat ("_MosaicSize", mMinMosaicSize);
				bigMask.SetActive (true);
				bigMask.transform.localScale = mDefaultBigMaskSize;
				groupMask.SetActive (false);
				mTotalMaskDuration = mTotalMaskDuration1;
				break;
			case 2:
				card.material = defaultMat;
				maskMat.SetFloat ("_MosaicSize", mMinMosaicSize);
				bigMask.SetActive (false);
				groupMask.SetActive (true);
				while(mInactiveGroupMaskItems.Count>0){
					mGroupMaskItems.Add (mInactiveGroupMaskItems[0]);
					mInactiveGroupMaskItems.RemoveAt (0);
				}
				for(int i =0;i<mGroupMaskItems.Count;i++){
					mGroupMaskItems [i].SetActive (true);
				}
				mTotalMaskDuration = mTotalMaskDuration2;
				break;
			}
		}

		void UpdateMask(){
			if(!mShowMask){
				return;
			}
			//if (mCurrentMaskDuration > mTotalMaskDuration) {
			//	card.material = defaultMat;
			//	return;
			//}
			//switch(mMaskType){
			//case 0:
			//	mCurrentMaskDuration += Time.deltaTime;
			//	float currentMosaicSize = Mathf.Lerp (mMaxMosaicSize, mMinMosaicSize, mCurrentMaskDuration / mTotalMaskDuration0);
			//	maskMat.SetFloat ("_MosaicSize",(int)currentMosaicSize);
			//	break;
			//case 1:
			//	mCurrentMaskDuration += Time.deltaTime;
			//	bigMask.transform.localScale = Vector3.Lerp (mDefaultBigMaskSize,mTargetBigMaskSize,mCurrentMaskDuration/mTotalMaskDuration1);
			//	break;
			//case 2:
				mCurrentMaskDuration += Time.deltaTime;
				float activeCount = (float)mGroupMaskItems.Count;
				float inactiveCount = (float)mInactiveGroupMaskItems.Count;
				if(mCurrentMaskDuration/mTotalMaskDuration2 > inactiveCount / (inactiveCount + activeCount)){
					if(mGroupMaskItems.Count == 0){
					//	break;
					return;
					}
					int index = Random.Range (0,mGroupMaskItems.Count);
					GameObject item = mGroupMaskItems[index];
					item.SetActive (false);
					mGroupMaskItems.Remove (item);
					mInactiveGroupMaskItems.Add (item);
				}
			//	break;
			//default:
			//	break;
			//}
		}

		void UpdateTime(){
			mCurrentQuestionUsed += Time.deltaTime;

			UIManager.GetInstance ().SetQuestionTime (mCurrentQuestionUsed,mQuestionDuration);
			if(mCurrentQuestionUsed > mQuestionDuration){
				mCurrentQuestionUsed = 0;
				if(mCurrentStepQuestionIndex <= mCurrentStepQuestionCount){
					//BeginDialog ((int)DialogType.DIALOG_QUESTION_TIMEUP_ID,ShowDefaultCard);
					BeginDialog ((int)DialogType.DIALOG_ANSWER_INCORRECT_ID,ShowDefaultCard);
					UIManager.GetInstance ().ShowTimeout ();
					mCurrentContinueCorrectNum = 0;
					if (ComponentConstant.SOUND_MANAGER != null)
						ComponentConstant.SOUND_MANAGER.Play (SoundEnum.se36_quiz_wrong);
					GameManager.GetInstance ().EnableHeaderBtn ();
					Header.Instance.isPause = true;
				}

				if (Header.Instance.GetLifeTime() <= 0) {
					//mCurrentStepUsed = 0;
					Header.Instance.SetLifeTime (0);
					GameManager.GetInstance ().PreGameOver ();
					status = QuestionStatus.Ending;
				}

			}

			if (currentStep == 4) {
				mCurrentStepUsed += Time.deltaTime;
				CheckIsOver ();
			}
		}

		void CheckIsOver(){
			if (Header.Instance.GetLifeTime() <= 0 && mCurrentQuestionUsed >= mQuestionDuration) {
				mCurrentStepUsed = 0;
				//if (currentStep >= 4) {
				GameManager.GetInstance ().PreGameOver ();
				status = QuestionStatus.Ending;
				//} 
			}
		}


		public Dictionary<int,List<SerifData>> serifDic;
		public float askWordInterval = 0.1f;
		float mCurrentAskWordInterval = 0;
		int mCurrentAskIndex = 0;
		SerifData mCurrentSerifData;

		void BeginDialog(int type,OnDialogDone done = null){
			onDialogDone = done;
			dialogType = type;
			GetDialogs (type);
			//mCurrentSerifData = GetNextDialog ();
			//Debug.Log ("type:" + type + ";mCurrentSerifData:" + mCurrentSerifData.serif);
			status = QuestionStatus.Dialog;
			mCurrentAskWordInterval = 0;
			mCurrentAskIndex = 0;
		}

		bool mEndDialog = false;
		public void JumpDialog(){
			if (EventSystem.current.currentSelectedGameObject != null)
				return;
			if (status == QuestionStatus.WaitingTab) {
				status = QuestionStatus.Dialog;
				mCurrentSerifData = null ;
				UIManager.GetInstance ().ResumeCharacterAnim ();
				UIManager.GetInstance ().jumpDialogBtn.gameObject.SetActive (false);
			}	
			else if(status == QuestionStatus.Dialog){
				mEndDialog = true;
			}
		}

		//Dictionary<int,List<SerifData>> mCurrentSerifDic;
		Queue<SerifData> mCurrentSerifList;
		void GetDialogs(int type){
			mCurrentAskIndex = 0;
			mCurrentAskWordInterval = 0;
			mCurrentSerifData = null;
			//mCurrentSerifDic = null;
			if(GameParams.GetInstance().serifTypeOrderDic.ContainsKey(type)){
				Dictionary<int,List<SerifData>> serifDic = GameParams.GetInstance().serifTypeOrderDic[type];
				//mCurrentSerifDic = new Dictionary<int, List<SerifData>> ();
				mCurrentSerifList = new Queue<SerifData> ();
				SerifData serifData = null;
				foreach(int key in serifDic.Keys){
					int totalPercent = 0;
					for(int i=0;i < serifDic[key].Count;i++){
						serifData = serifDic [key] [i];
						totalPercent += serifDic[key][i].serif_percentage;
						serifDic[key][i].temp_percentage = totalPercent;
					}
					int realPercent = Random.Range (0,totalPercent);
					if(serifDic [key].Count > 0){
						for(int i=0; i< serifDic[key].Count; i++){
							if(realPercent < serifDic[key][i].temp_percentage){
								serifData = serifDic[key][i];
								mCurrentSerifList.Enqueue (serifData);
								break;
							}
						}
					}
				}
				Debug.Log ("type:" + type + "mCurrentSerifList:" + serifData.serif);
			}
		}

		bool mIsQuestionNumDialog;
		SerifData mQuestionNumSerifData;

		void UpdateDialog(){
			if (mCurrentSerifData == null) {
				mCurrentSerifData = GetNextDialog ();
			}
			if (mCurrentAskWordInterval >= askWordInterval) {
				mCurrentAskWordInterval = 0;
				mCurrentAskIndex++;
				string askStrFull = mCurrentSerifData.serif;
				if(dialogType == (int)DialogType.QUESTION_ASK_ORDER_ID){
					
					askStrFull = askStrFull.Replace ("[X]",(mCurrentStepQuestionIndex + 1).ToString());
				}

				if(dialogType == (int)DialogType.DIALOG_3_TO_4_ID){
					askStrFull = askStrFull.Replace ("[X]",Mathf.RoundToInt(this.mCurrentStepDuration).ToString());
					//int i = 0;
					//while(askStrFull.IndexOf("[X]")!=-1){
					//	int index = askStrFull.IndexOf ("[X]");
					//	askStrFull = askStrFull.Remove (index,3);
					//	if(i == 0){
						//	askStrFull = askStrFull.Insert (index,GameParams.GetInstance ().paramData.last_stage_bonus_time.ToString());
						//}else if(i == 1){
						//	askStrFull = askStrFull.Insert (index,Mathf.RoundToInt(mQuestionTimeBonusOfStep4).ToString());
						//}else if(i == 2){
					//		askStrFull = askStrFull.Insert (index,Mathf.RoundToInt(GameParams.GetInstance ().paramData.last_stage_bonus_time + mQuestionTimeBonusOfStep4).ToString());
					//	}
					//	i++;
					//}
				}
				if (mCurrentAskIndex <= askStrFull.Length && !mEndDialog) {
					UIManager.GetInstance ().PlayDialogCharacter (mCurrentSerifData);
					string askStr = askStrFull.Substring (0, mCurrentAskIndex);
					UIManager.GetInstance ().SetQuestionText (askStr);
				} else {
					mEndDialog = false;
					UIManager.GetInstance ().PlayDialogCharacter (mCurrentSerifData);
					UIManager.GetInstance ().SetQuestionText (askStrFull);
					if (mCurrentSerifData.tab == 1) {
						Debug.Log ("QuestionStatus.WaitingTab");
						UIManager.GetInstance ().ShowJumpDialogBtn (true);
						status = QuestionStatus.WaitingTab;
						UIManager.GetInstance ().PauseCharacterAnim ();
						return;
					}
					mCurrentSerifData = null;
				}
			} else {
				mCurrentAskWordInterval += Time.deltaTime;
			}
		}

		SerifData GetNextDialog(){
			mCurrentAskWordInterval = 0;
			mCurrentAskIndex = 0;
			Debug.Log ("GetNextDialog");
			if(mCurrentSerifList == null || mCurrentSerifList.Count == 0){
				if (currentStep <= 0) {
					status = QuestionStatus.NextStep;
				} else {
					status = QuestionStatus.NextQuestion;
				}
				UIManager.GetInstance ().CloseAnswers ();
				if(onDialogDone!=null)
					onDialogDone ();
				UIManager.GetInstance ().characterAnim.Pause ();
				return null;
			}
			SerifData serifData = mCurrentSerifList.Dequeue ();
			Debug.Log ("serifData:" + serifData.serif);
			return serifData;
		}

		int mCurrentContinueCorrectNum = 0;
		public float Answer(bool isCorrect){
			GameManager.GetInstance ().EnableHeaderBtn ();
			Header.Instance.isPause = true;
			UIManager.GetInstance ().SetQuestionTime (mQuestionDuration,mQuestionDuration);
			card.material = defaultMat;
			bigMask.SetActive (false);
			groupMask.SetActive (false);
			float time = 0;
			if (isCorrect) {
				mCurrentContinueCorrectNum++;
				GameManager.GetInstance ().TotalCorrectNum++;
				if(mCurrentContinueCorrectNum > 1){
					GameManager.GetInstance ().TotalContinueCorrectNum++;
				}
				if(GameManager.GetInstance ().MaxContinueCorrectNum < mCurrentContinueCorrectNum){
					GameManager.GetInstance ().MaxContinueCorrectNum = mCurrentContinueCorrectNum;
					UIManager.GetInstance ().SetContinueCorrectAnswerSum (mCurrentContinueCorrectNum);
				}

				BeginDialog ((int)DialogType.DIALOG_ANSWER_CORRECT_ID,ShowDefaultCard);
				if (currentStep != 4) {
					mCurrentStepDuration += Mathf.Max(0,mQuestionDuration - mCurrentQuestionUsed);
					Header.Instance.SetLifeTime (Mathf.RoundToInt(mCurrentStepDuration));
					Header.Instance.SetLife (LifeType.Time,Mathf.RoundToInt(mCurrentStepDuration));
					time = Mathf.Max(0,mQuestionDuration - mCurrentQuestionUsed);
				}
				if (ComponentConstant.SOUND_MANAGER != null)
					ComponentConstant.SOUND_MANAGER.Play (SoundEnum.se37_quiz_right);
			} else {
				mCurrentContinueCorrectNum = 0;
				GameManager.GetInstance ().nomiss = 0;
				BeginDialog ((int)DialogType.DIALOG_ANSWER_INCORRECT_ID,ShowDefaultCard);
				if(currentStep == 4){
					mCurrentStepUsed += (mQuestionDuration - mCurrentQuestionUsed) / 2f + 1;
					//Header.Instance.SetLifeTime (Mathf.RoundToInt(mCurrentStepDuration));
					Header.Instance.SetLifeTime (Mathf.Max(0,mCurrentStepDuration - mCurrentStepUsed));
					Header.Instance.UpdateTime ();
				}
				if (ComponentConstant.SOUND_MANAGER != null)
					ComponentConstant.SOUND_MANAGER.Play (SoundEnum.se36_quiz_wrong);
				//if(currentStep == 4){
				//	mCurrentStepUsed += mQuestionDuration;
				//	GameManager.GetInstance ().UpdateHeaderTime (Mathf.Max(0,mCurrentStepDuration - mCurrentStepUsed));
				//}

			}
			status = QuestionStatus.Dialog;
			//if (currentStep != 4) {
				//GameManager.GetInstance ().UpdateHeaderTime ((mCurrentStepQuestionCount - mCurrentStepQuestionIndex) * mQuestionDuration);
				//mCurrentStepUsed = mCurrentStepQuestionCount * mQuestionDuration;
			//}

			if (Header.Instance.GetLifeTime() <= 0) {
				//mCurrentStepUsed = 0;
				Header.Instance.SetLifeTime (0);
				GameManager.GetInstance ().PreGameOver ();
				status = QuestionStatus.Ending;
			}

			return time;
   		}

		CardCSVStructure mCardCSVStructure;
		AssetBundle mAssetBundle;
		public void GetSprite (CardCSVStructure cardCSVStructure)
		{
			if (cardCSVStructure != null) {
				mCardCSVStructure = cardCSVStructure;
				//StartCoroutine (ComponentConstant.ASSETBUNDLE_RESOURCES_GETTER.GetResource<Texture2D> (cardCSVStructure.assetbundle_name.ToString (), int.Parse (cardCSVStructure.image_resource).ToString (LanguageJP.FOUR_MASK) + LanguageJP.CARD_THUMBNAIL_SUFFIX, GetResource<Texture2D>, false));
				//StartCoroutine (ComponentConstant.ASSETBUNDLE_RESOURCES_GETTER.GetResource<Texture2D> (cardCSVStructure.assetbundle_name.ToString (), int.Parse (cardCSVStructure.image_resource).ToString (LanguageJP.FOUR_MASK), GetResource<Texture2D>, true));
				if (mAssetBundle != null){
					mAssetBundle.Unload (true);
					Resources.UnloadUnusedAssets ();
				}
				StartCoroutine (ComponentConstant.ASSETBUNDLE_RESOURCES_GETTER.GetAssetBundle(cardCSVStructure.assetbundle_name.ToString (),GetSprite<AssetBundle>));
			}
		}

		void GetSprite<T>(T t){
			AssetBundle ab = t as AssetBundle;
			mAssetBundle = ab;
			Texture2D txt = ab.LoadAsset<Texture2D> (int.Parse (mCardCSVStructure.image_resource).ToString (LanguageJP.FOUR_MASK));
			GetResource<Texture2D> (txt);
		}

		private void GetResource<T> (T t)
		{
			Sprite sprite = TextureToSpriteConverter.ConvertToSprite (t as Texture2D);
			Sprite preSprite = card.sprite;
			if(preSprite!=null){
				Destroy (preSprite);
				Resources.UnloadUnusedAssets ();
			}
			card.sprite = sprite;
			if (AssetBundleResourcesLoader.cardFrameDetailList == null || AssetBundleResourcesLoader.cardFrameDetailList.Count == 0) {
				StartCoroutine (GetFrame (sprite));
			} else {
				cardFrame.sprite = AssetBundleResourcesLoader.cardFrameDetailList [mCardCSVStructure.rarity - 1];
			}
		}

		private IEnumerator GetFrame (Sprite sprite)
		{
			/*
			yield return StartCoroutine (ComponentConstant.ASSETBUNDLE_RESOURCES_GETTER.GetAllResourcesList<Texture2D> (AssetBundleName.card_detail_frame.ToString (), (List<Texture2D> list) => {
				AssetBundleResourcesLoader.cardFrameThumbnailDictionary = TextureToSpriteConverter.ConvertToSpriteDictionary (list);
				SetFrame (sprite, AssetBundleResourcesLoader.cardFrameThumbnailDictionary [string.Format ("{0}{1}", mCardCSVStructure.rarity, LanguageJP.CARD_THUMBNAIL_SUFFIX)]);
			}, false));
			*/
			yield return StartCoroutine (ComponentConstant.ASSETBUNDLE_RESOURCES_GETTER.GetAllResourcesList<Texture2D> (AssetBundleName.card_detail_frame.ToString (), (List<Texture2D> list) => {
				AssetBundleResourcesLoader.cardFrameDetailList = TextureToSpriteConverter.ConvertToSpriteList (list);
				cardFrame.sprite = AssetBundleResourcesLoader.cardFrameDetailList [mCardCSVStructure.rarity - 1];
			}, false));
		}

	}
}
