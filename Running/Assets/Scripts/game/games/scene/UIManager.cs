using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Text;
using DG.Tweening;

namespace scene{
	public class UIManager : SingleMonoBehaviour<UIManager> {

		public List<Button> controllers;
		public List<QuestionData> currentQuestions;
		public int correctIndex = 0;
		public QuestionData currentQuestion;
		public Text questionText;
		public bool isPause;
		public Slider questionTimeSlider;

		public GameObject correctAnswer;
		public GameObject wrongAnswer;
		public GameObject timeoutAnswer;

		public CharacterAnim characterAnim;

		public Text txtCurrentStage;
		public Text txtContinueCorrectAnswerSum;
		public Text txtTimeAsign;
		public Button jumpDialogBtn;

		public GameObject ending;

		public GameObject CanvasFrant;
		public GameObject CanvasBack;

		public GameObject heartEffectPrefab;
		public Transform startPosTrans;
		public Transform endPosTrans;

		protected override void Awake ()
		{
			base.Awake ();

		}

		void Update(){
			if(Input.GetKeyDown(KeyCode.H)){
				HeartEffect (10);
			}
		}

		public void Init(){
			for(int i=0;i<controllers.Count;i++){
				controllers [i].onClick.AddListener (OnControllerClick);
			}
			//if (jumpDialogBtn != null)
				//jumpDialogBtn.onClick.AddListener(new UnityEngine.Events.UnityAction(JumpDialog));
			txtCurrentStage.transform.parent.gameObject.SetActive (false);
			txtContinueCorrectAnswerSum.transform.parent.gameObject.SetActive (false);
			//characterAnim.gameObject.SetActive (false);
			//questionText.gameObject.SetActive (false);
		}


		public void JumpDialog(){
			ShowJumpDialogBtn (false);
			//Question.GetInstance ().JumpDialog ();
		}

		public void ShowJumpDialogBtn(bool isTrue){
			if (jumpDialogBtn != null)
				jumpDialogBtn.gameObject.SetActive (isTrue);
		}

		public void ClearControllerText(){
			for(int i=0;i<controllers.Count;i++){
				controllers [i].GetComponentInChildren<Text> (true).text = "";
			}
		}

		public void SetContinueCorrectAnswerSum(int sum){
			if(txtContinueCorrectAnswerSum!=null){
				//if (sum - 1 > 0) {
				txtContinueCorrectAnswerSum.transform.parent.gameObject.SetActive (true);
				txtContinueCorrectAnswerSum.text = "連続" + sum + "問";
				//} else {
				//	txtContinueCorrectAnswerSum.transform.parent.gameObject.SetActive (false);
				//}
			}
		}

		public void SetTotalCorrectAnswerSum(int sum){
			if (Header.Instance != null) {
				if (sum >= 10 && sum < 100) {
					Header.Instance.SetScore ("正解   " + sum + " 問");
				} else if (sum >= 100) {
					Header.Instance.SetScore ("正解  " + sum + " 問");
				} else {
					Header.Instance.SetScore ("正解    " + sum + " 問");
				}
			}
		}

		public void SetCurrentStage(int stage){
			if(stage>0){
				txtCurrentStage.transform.parent.gameObject.SetActive (true);
			}
			if(txtCurrentStage!=null){
				if (stage == 4) {
					txtCurrentStage.text ="最終ステージ ";
				} else {
					txtCurrentStage.text ="ステージ " + stage.ToString ();
				}
			}
		}

		float GetTextSize(Text text,string txt,int fontSize){
			text.font.RequestCharactersInTexture(txt,fontSize,FontStyle.Normal);
			CharacterInfo characterInfo;
			float width=0f;
			for(int i=0;i<txt.Length;i++){
				text.font.GetCharacterInfo(txt[i],out characterInfo,fontSize);
				width += characterInfo.advance;
			}
			return width;
		}

		public void PlayDialogCharacter(SerifData serif){
			List<SerifImageData> serifImages = serif.images;
			//if(!characterAnim.gameObject.activeInHierarchy)
				//characterAnim.gameObject.SetActive (true);
			characterAnim.Play (serifImages);
		}

		public void PauseCharacterAnim(){
			characterAnim.Pause ();
		}

		public void ResumeCharacterAnim(){
			characterAnim.Resume ();
		}

		public void SetQuestion(QuestionData questionData){
			currentQuestion = questionData;
			correctIndex = Random.Range (0, 4);
			List<string> answers = new List<string> ();
			answers.Add (questionData.otherAnswer1);
			answers.Add (questionData.otherAnswer2);
			answers.Add (questionData.otherAnswer3);
			answers.Insert (correctIndex,questionData.answer);
			Text text;
			for(int i=0;i<controllers.Count;i++){
				//int fontSize = 45;
				text = controllers [i].GetComponentInChildren<Text> ();
				text.lineSpacing = 0.46f;
				text.alignment = TextAnchor.MiddleLeft;
				text.fontSize = Mathf.Min(46, Mathf.FloorToInt(46f / (answers [i].Length / 68f)));
				text.text = answers [i];
				/**
				while(true){
					float txtSize = GetTextSize (text,answers [i],fontSize);
					float width = text.rectTransform.sizeDelta.x;
					int line = Mathf.CeilToInt (txtSize / width);
					Debug.Log (txtSize * line);
					if (txtSize * line > 300) {
						fontSize--;
					} else {
						break;
					}
				}**/
				//int realFontSize = Mathf.FloorToInt(45 / Mathf.Max(1,line / 2f));
				//if(text.fontSize != realFontSize)
				//	text.fontSize = realFontSize;
			}
		}

		public void ShowQuestionType(){
			Text text;
			for(int i=0;i<GameParams.GetInstance().questionTypeList.Count;i++){
				text = controllers [i].GetComponentInChildren<Text> ();
				text.alignment = TextAnchor.MiddleCenter;
				text.text = GameParams.GetInstance().questionTypeList[i].name;
			}
		}

		public void ShowCharacters(){
			List<CharacterData> charas = GameParams.GetInstance ().selectAbleCharaList;
			Text text;
			for(int i=0;i<controllers.Count;i++){
				text = controllers [i].GetComponentInChildren<Text> ();
				text.alignment = TextAnchor.MiddleCenter;
				text.text = charas [i].name;
			}
		}

		public void EnableControllers(bool isTrue){
			for(int i=0;i<controllers.Count;i++){
				controllers [i].enabled = isTrue;
				if (isTrue) {
					controllers [i].GetComponent<Image> ().color = Color.white;
				} else {
					controllers [i].GetComponent<Image> ().color = Color.gray;
				}
			}
		}

		void OnControllerClick(){
			GameObject go = EventSystem.current.currentSelectedGameObject;
			Button btn = go.GetComponent<Button> ();
			int index = controllers.IndexOf (btn);
			if (Question.GetInstance ().status == QuestionStatus.SelectQuestionType) {
				SelectQuestionType (index);
			} else if (Question.GetInstance ().status == QuestionStatus.SelectCharacter) {
				SelectCharacter (index);
			}else {
				Answer (index,go.transform.position);
			}
		}

		void SelectQuestionType(int index){
			Question.GetInstance ().OnSelectQuestionType (index);
			for(int i=0;i<GameParams.GetInstance().questionTypeList.Count;i++){
				controllers [i].GetComponentInChildren<Text> ().text = "";
			}
		}

		void SelectCharacter(int index){
			//CharacterData data = GameParams.GetInstance ().selectAbleCharaList[index];
			Question.GetInstance ().OnSelectCharacter (index);
			for(int i=0;i<GameParams.GetInstance().questionTypeList.Count;i++){
				controllers [i].GetComponentInChildren<Text> ().text = "";
			}
		}

		void Answer(int index,Vector3 pos){
			Debug.Log ("index:" + index);
			if(Question.GetInstance().status != QuestionStatus.Answering){
				return;
			}
			EnableControllers (false);
			if (correctIndex == index) {
				float time = Question.GetInstance ().Answer (true);
				ShowCorrent (pos,time);
				//if(Question.GetInstance().currentStep != 4){
				//	ShowEffect (1,pos);
				//}
			} else {
				Question.GetInstance ().Answer (false);
				ShowWrong (pos);
			}
		}

		 float mEffectSpeed = 1.5f;
		 float mEffectAnimSpeed = 2.5f;
		public GameObject timePrefab;
		public void ShowEffect(float sec,Vector3 pos)
		{
			//Vector3 pos =  Header.Instance.txtScore.GetComponent<RectTransform>().transform.position; 
			GameObject go = Instantiate (timePrefab) as GameObject;
			go.GetComponentInChildren<Animator> (true).speed = mEffectAnimSpeed;
			go.transform.position = pos;
			Vector3 posWorld = Header.Instance.txtLife.GetComponent<RectTransform>().transform.position;
			// Move efffect
			go.transform.DOMove(posWorld,mEffectSpeed);
			go.GetComponentInChildren<TimeToLive> ().timeToLive = mEffectSpeed + 0.1f;
			Destroy (go,10f);
		}

		List<QuestionData> GetStageQuestions(int stageIndex){
			List<QuestionData> questions = new List<QuestionData> ();
			return questions;
		}

		public void SetQuestionText(string str){
			//if(!questionText.gameObject.activeInHierarchy)
			//	questionText.gameObject.SetActive (true);
			/*
			float txtSize = GetTextSize (questionText,str,56);
			float width = questionText.rectTransform.sizeDelta.x;
			int line = Mathf.CeilToInt (txtSize / width);
			int realFontSize = Mathf.FloorToInt(56f / Mathf.Max(1,line / 2f));
			if(questionText.fontSize != realFontSize)
				questionText.fontSize = realFontSize;
			*/


			//if (str.Length > 136) 
			//	questionText.fontSize = 45;
			//else
			//	questionText.fontSize = 50;
			questionText.text = str;
		}

		public void SetQuestionTime(float current,float max){
			if (questionTimeSlider != null)
				questionTimeSlider.value = 1 - current / max;
		}

		void ShowCorrent(Vector3 pos,float time){
			if(time > 0){
				//txtTimeAsign.gameObject.SetActive (true);
				GameObject go = Instantiate (txtTimeAsign.gameObject) as GameObject;
				go.transform.SetParent (txtTimeAsign.transform.parent);
				go.transform.localScale = Vector3.one;
				Text text = go.GetComponent<Text> ();
				GachaColor gachaColor = text.GetComponent<GachaColor>();
				gachaColor.delay = 0.1f;
				gachaColor.duration = 0.4f;
				gachaColor.ResetToBegin();
				gachaColor.Play ();
				float t = Mathf.FloorToInt (time * 10) / 10f;
				text.alignment = TextAnchor.MiddleCenter;
				text.text = "+ " + t + " 秒";
				text.transform.position = pos + new Vector3 (0,0.6f,0);
				text.transform.SetSiblingIndex (99);
				StartCoroutine (_MoveText (go));
			}
			correctAnswer.SetActive (true);
			correctAnswer.transform.position = pos;
			wrongAnswer.SetActive (false);
			timeoutAnswer.SetActive (false);
		}


		IEnumerator _MoveText(GameObject go){
			go.SetActive (true);
			float speed = 0.5f;
			float t = 3;
			float current = 0;
			while(current<t){
				current += Time.deltaTime;
				go.transform.position += new Vector3 (0,1,0) * speed * Time.deltaTime;
				yield return null;
			}
			Destroy (go);
		}


		void ShowWrong(Vector3 pos){
			correctAnswer.SetActive (false);
			wrongAnswer.SetActive (true);
			wrongAnswer.transform.position = pos;
			timeoutAnswer.SetActive (false);
		}

		public void ShowTimeout(){
			correctAnswer.SetActive (false);
			wrongAnswer.SetActive (false);
			timeoutAnswer.SetActive (true);
		}

		public void CloseAnswers(){
			correctAnswer.SetActive (false);
			wrongAnswer.SetActive (false);
			timeoutAnswer.SetActive (false);
		}

		public void ShowEnding()
		{
			Invoke ("_CloseEnding",2);
			if(ending!=null)
				ending.SetActive(true);
		}

		void _CloseEnding(){
			ending.GetComponentInChildren<Button> (true).onClick.AddListener(CloseEnding);
		}

		public void CloseEnding()
		{
			ending.SetActive(false);
			GameManager.GetInstance().SendGameEndingAPI();
			ComponentConstant.SOUND_MANAGER.StopBGM();
		}

		public void HeartEffect(int count){
			StartCoroutine (_SpawnHeart(count));
		}

		IEnumerator _SpawnHeart(int count){
			for(int i=0;i<count;i++){
				GameObject go = Instantiate (heartEffectPrefab) as GameObject;
				go.transform.SetParent (heartEffectPrefab.transform.parent);
				go.transform.position = startPosTrans.position;
				go.transform.localScale = Vector3.one;
				go.SetActive (true);
				StartCoroutine (_MoveHeart(go));
				yield return new WaitForSeconds (0.1f);
			}
		}

		public AnimationCurve curve;
		public float radio = 0.1f;
		IEnumerator _MoveHeart(GameObject go){
			float t = 0;
			float x = Random.Range (-radio,radio);
			float y = -Mathf.Pow (radio*radio - x*x,0.5f);

			Vector2 startPos = new Vector2 (startPosTrans.position.x,startPosTrans.position.y);
			Vector2 centerPos = startPos + new Vector2 (x,y);
			Vector2 endPos = new Vector2 (endPosTrans.position.x,endPosTrans.position.y);
			while(t<1){
				t += Time.deltaTime;
				go.transform.position = Bezier2 (startPos,centerPos,endPos,curve.Evaluate(t)); 
				yield return null;
			}
			go.SetActive (false);
		}

		Vector2 Bezier2(Vector2 start,Vector2 center,Vector2 end,float t){
			Vector2 p0 = Vector2.Lerp (start,center,t);
			Vector2 p1 = Vector2.Lerp (center,end,t);
			return Vector2.Lerp (p0,p1,t);
		}

	}
}
