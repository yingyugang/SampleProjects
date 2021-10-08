using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using CSV;
using System.Linq;

namespace scene{
	public class GameParams : SingleMonoBehaviour<GameParams> {

		public const string CSV_SERIF = "csv/m_scene_serif";
		public const string CSV_QUESTION_TYPE = "csv/m_scene_question_type";
		public const string CSV_SERIF_IMAGE = "csv/m_scene_serif_image";
		public const string CSV_QUESTION = "csv/m_scene_answer";
		public const string CSV_CHARACTER = "csv/m_scene_character";
		public const string CSV_PARAMETER = "csv/m_scene_parameter";

		protected override void Awake(){
			base.Awake ();
			LoadCsvData();
			InitQuestionDatas ();
		}

		int GetInt(System.Object obj){
			int value = 0;
			int.TryParse (obj.ToString().Trim(), out value);
			return value;
		}

		string GetString(System.Object obj){
			return obj.ToString ().Trim ();
		}

		float GetFloat(System.Object obj){
			float value = 0;
			float.TryParse (obj.ToString().Trim(), out value);
			return value;
		}

		void LoadCsvData(){
			LoadParameter ();
			LoadCardData ();
			LoadCSVQuestion ();
			LoadCSVQuestionType ();
			LoadCSVSerifImage ();
			LoadCSVSerif ();
			LoadCharacter ();
		}

		public ParamData paramData;
		void LoadParameter(){
			if (APIInformation.GetInstance != null) {
				GameParameter parameter = APIInformation.GetInstance.gameparameter;
				paramData = new ParamData ();
				paramData.stage1_questions = APIInformation.GetInstance.gameparameter.stage1_questions;
				paramData.stage2_questions = APIInformation.GetInstance.gameparameter.stage2_questions;
				paramData.stage3_questions = APIInformation.GetInstance.gameparameter.stage3_questions;
				paramData.stage1_time_limit = APIInformation.GetInstance.gameparameter.stage1_time_limit;
				paramData.stage2_time_limit = APIInformation.GetInstance.gameparameter.stage2_time_limit;
				paramData.stage3_time_limit = APIInformation.GetInstance.gameparameter.stage3_time_limit;
				paramData.last_stage_bonus_time = APIInformation.GetInstance.gameparameter.last_stage_bonus_time;
				paramData.stage_clear_bonus_rate = APIInformation.GetInstance.gameparameter.stage_clear_bonus_rate;
				paramData.comb_var1 = APIInformation.GetInstance.gameparameter.comb_var1;
				paramData.comb_var2 = APIInformation.GetInstance.gameparameter.comb_var2;
				paramData.comb_var3 = APIInformation.GetInstance.gameparameter.comb_var3;
				paramData.item_span_second = APIInformation.GetInstance.gameparameter.item_span_second;
				paramData.item_appear_percentage = APIInformation.GetInstance.gameparameter.item_appear_percentage;
			} else {
				List<Dictionary<string, object>> data = CSVReader.Read(CSV_PARAMETER);
				if(data!=null){
					paramData = new ParamData ();
					for(int i=0;i<data.Count;i++){
						switch(GetString(data[i]["param_name"])){
						case "stage1_questions":
							paramData.stage1_questions = GetInt (data[i]["value"]);
							break;
						case "stage2_questions":
							paramData.stage2_questions = GetInt (data[i]["value"]);
							break;
						case "stage3_questions":
							paramData.stage3_questions = GetInt (data[i]["value"]);
							break;
						case "stage1_time_limit":
							paramData.stage1_time_limit = GetInt (data[i]["value"]);
							break;
						case "stage2_time_limit":
							paramData.stage2_time_limit = GetInt (data[i]["value"]);
							break;
						case "stage3_time_limit":
							paramData.stage3_time_limit = GetInt (data[i]["value"]);
							break;
						case "last_stage_bonus_time":
							paramData.last_stage_bonus_time = GetInt (data[i]["value"]);
							break;
						case "stage_clear_bonus_rate":
							paramData.stage_clear_bonus_rate = GetFloat (data[i]["value"]);
							break;
						case "comb_var1":
							paramData.comb_var1 = GetFloat (data[i]["value"]);
							break;
						case "comb_var2":
							paramData.comb_var2 = GetFloat (data[i]["value"]);
							break;
						case "comb_var3":
							paramData.comb_var3 = GetFloat (data[i]["value"]);
							break;
						case "item_span_second":
							paramData.item_span_second = GetInt (data[i]["value"]);
							break;
						case "item_appear_percentage":
							paramData.item_appear_percentage = GetInt (data[i]["value"]);
							break;
						}
					}
				}
			}
		}

		public Dictionary<int,CardCSVStructure> cardDic;
		public Dictionary<int, Dictionary<int,CardCSVStructure>> cardByTypeDic;
		void LoadCardData(){
			cardDic = new Dictionary<int, CardCSVStructure> ();
			cardByTypeDic = new Dictionary<int, Dictionary<int,CardCSVStructure>> ();
			CsvContext csvContext = new CsvContext ();
			if (FileManager.Exists (PathConstant.CLIENT_CSV_PATH + "m_card.csv")) {
				MasterCSV.cardCSV = csvContext.Read<CardCSVStructure> (PathConstant.CLIENT_CSV_PATH + "m_card.csv").ToList ();
			}
			foreach(CardCSVStructure card in MasterCSV.cardCSV){
				cardDic.Add (card.id,card);
				List<int> types = card.GetCharacterType;
				for(int i=0;i<types.Count;i++){
					if (!cardByTypeDic.ContainsKey (types [i])) {
						cardByTypeDic.Add (types [i],new Dictionary<int, CardCSVStructure>());
					}
					Dictionary<int, CardCSVStructure> cards = cardByTypeDic [types [i]];
					cards.Add (card.id,card);
				}
			}
		}

		public Dictionary<int,CharacterData> charaDic;
		public List<CharacterData> charaList;
		public List<CharacterData> selectAbleCharaList;
		void LoadCharacter(){
			List<Dictionary<string, object>> data = CSVReader.Read(CSV_CHARACTER);
			if(data!=null){
				charaList = new List<CharacterData> ();
				charaDic = new Dictionary<int, CharacterData> ();
				for(int i=0;i<data.Count;i++){
					CharacterData chara = new CharacterData ();
					chara.id = GetInt (data[i]["id"]);
					chara.name = GetString (data[i]["name"]);
					charaList.Add (chara);
					if(!charaDic.ContainsKey(chara.id))
					{
						charaDic.Add (chara.id,chara);
					}
				}
				Debug.Log (charaList.Count);
			}
			selectAbleCharaList = GetRandomeCharacters ();
		}

		public List<SerifData> serifList;
		public Dictionary<int,List<SerifData>> serifTypeDic;//key is part_type;
		public Dictionary<int,Dictionary<int,List<SerifData>>> serifTypeOrderDic;
		public Dictionary<string,Dictionary<int,Dictionary<int,List<SerifData>>>> serifKeyTypeOrderDic;
		void LoadCSVSerif(){
			List<Dictionary<string, object>> data = CSVReader.Read(CSV_SERIF);
			if (data != null)
			{
				serifList = new List<SerifData> ();
				serifTypeDic = new Dictionary<int, List<SerifData>> ();
				serifTypeOrderDic = new Dictionary<int, Dictionary<int, List<SerifData>>> ();
				serifKeyTypeOrderDic = new Dictionary<string, Dictionary<int, Dictionary<int, List<SerifData>>>> ();
				for(int i=0;i<data.Count;i++){
					SerifData serif = new SerifData ();
					serif.id = GetInt (data[i]["id"]);
					serif.part_type = GetInt (data[i]["part_type"]);
					serif.part_order = GetInt (data[i]["part_order"]);
					serif.serif = GetString (data[i]["serif"]);
					serif.serif_percentage = GetInt (data[i]["serif_percentage"]);
					serif.secret_tech = GetString (data[i]["secret_tech_key"]);
					serif.image_group_id = GetInt (data[i]["image_group_id"]);
					serif.tab = GetInt (data[i]["tab"]);
					if(serifImageGroups.ContainsKey(serif.image_group_id)){
						serif.images = serifImageGroups [serif.image_group_id];
					}
					serifList.Add (serif);

					if(!serifKeyTypeOrderDic.ContainsKey(serif.secret_tech)){
						serifKeyTypeOrderDic.Add (serif.secret_tech,new Dictionary<int, Dictionary<int, List<SerifData>>>());
					}
					serifTypeOrderDic = serifKeyTypeOrderDic[serif.secret_tech];
					if(!serifTypeOrderDic.ContainsKey(serif.part_type)){
						serifTypeOrderDic.Add (serif.part_type,new Dictionary<int, List<SerifData>>());
					}
					if (!serifTypeOrderDic [serif.part_type].ContainsKey (serif.part_order)) {
						serifTypeOrderDic [serif.part_type].Add (serif.part_order,new List<SerifData>());
					}
					serifTypeOrderDic [serif.part_type] [serif.part_order].Add (serif);

					if(!serifTypeDic.ContainsKey(serif.part_type)){
						serifTypeDic.Add (serif.part_type,new List<SerifData>());
					}
					serifTypeDic [serif.part_type].Add (serif);

				}
				CheatData cd = CheatController.GetLastMatchCheat ();
				if (cd != null && serifKeyTypeOrderDic.ContainsKey (cd.key)) {
					serifTypeOrderDic = serifKeyTypeOrderDic [cd.key];
				} else {
					serifTypeOrderDic = serifKeyTypeOrderDic ["0"];//default serif
				}
			}
		}

		public List<SerifImageData> serifImages;
		public Dictionary<int,List<SerifImageData>> serifImageGroups;
		void LoadCSVSerifImage(){
			List<Dictionary<string, object>> data = CSVReader.Read(CSV_SERIF_IMAGE);
			if(data != null)
			{
				serifImages = new List<SerifImageData> ();
				serifImageGroups = new Dictionary<int, List<SerifImageData>> ();
				for(int i=0;i<data.Count;i++){
					SerifImageData serifImageData = new SerifImageData ();
					serifImageData.id = GetInt (data[i]["id"]);
					serifImageData.group_id = GetInt (data[i]["group_id"]);
					serifImageData.image = GetString (data[i]["image"]);

					serifImageData.size = GetFloat (data[i]["size"]);
					serifImageData.offset_x = GetFloat (data[i]["offset_x"]);
					serifImageData.offset_y = GetFloat (data[i]["offset_y"]);

					serifImages.Add (serifImageData);
					if(!serifImageGroups.ContainsKey(serifImageData.group_id)){
						serifImageGroups.Add (serifImageData.group_id,new List<SerifImageData>());
					}
					serifImageGroups [serifImageData.group_id].Add (serifImageData);
				}
			}
		}

		public List<QuestionType> questionTypeList;
		public Dictionary<int,QuestionType> questionTypeDic;
		void LoadCSVQuestionType(){
			List<Dictionary<string, object>> data = CSVReader.Read(CSV_QUESTION_TYPE);
			if(data != null)
			{
				questionTypeList = new List<QuestionType> ();
				questionTypeDic = new Dictionary<int, QuestionType> ();
				HashSet<string> answerSet = new HashSet<string> ();
				for(int i=0;i<data.Count;i++){
					QuestionType questionType = new QuestionType ();
					questionType.id = GetInt (data[i]["id"]);
					questionType.name = GetString (data[i]["name"]);
					questionTypeList.Add (questionType);
					questionTypeDic.Add (questionType.id,questionType);
				}
			}
		}

		public List<QuestionData> questions;
		List<QuestionData> questionOfStep1;
		List<QuestionData> questionOfStep2;
		List<QuestionData> questionOfStep3;
		Dictionary<int,List<QuestionData>> questionByCharaIdDic;
		public Dictionary<int,List<string>> otherAnswerDic;//other answer 里面可能会有重复的，必须先把重复排除掉。
		public Dictionary<int,List<QuestionData>> questionByTypes;
		void LoadCSVQuestion(){
			List<Dictionary<string, object>> data = CSVReader.Read(CSV_QUESTION);
			if(data!=null){
				questions = new List<QuestionData> ();
				questionByTypes = new Dictionary<int, List<QuestionData>> ();
				questionOfStep1 = new List<QuestionData> ();
				questionByCharaIdDic = new Dictionary<int, List<QuestionData>> ();
				otherAnswerDic = new Dictionary<int,List<string>> ();
				//otherAnswerCountDic = new Dictionary<int, int> ();
				Dictionary<int,HashSet<string>> tempOtherAnswerDic = new Dictionary<int, HashSet<string>> ();
				for(int i=0;i<data.Count;i++){
					QuestionData questionData = new QuestionData ();
					questionData.id = GetInt (data[i]["id"]);
					questionData.card_id = GetInt (data[i]["card_id"]);
					questionData.question_type = GetInt (data[i]["question_type"]);
					questionData.answer = GetString (data[i]["answer"]);
					questions.Add (questionData);
					if (!questionByTypes.ContainsKey (questionData.question_type)) {
						questionByTypes.Add (questionData.question_type, new List<QuestionData> ());
					}
					questionByTypes [questionData.question_type].Add (questionData);
					if(questionData.question_type == 1 || questionData.question_type == 4){
						questionOfStep1.Add (questionData);
					}
					int cardId = questionData.card_id;
					CardCSVStructure card = cardDic [cardId];
					List<int> charaIds = card.GetCharacterType;
					/*
					for(int j=1;j<8;j++){
						if (!questionByCharaIdDic.ContainsKey (j)) {
							questionByCharaIdDic.Add (j,new List<QuestionData>());
						}
						List<QuestionData> qList = questionByCharaIdDic [j];
						qList.Add (questionData);
					}*/

					for(int j=0;j<charaIds.Count;j++){
						if (charaIds[j] > 6)
							charaIds[j] = 7;
						if (!questionByCharaIdDic.ContainsKey (charaIds[j])) {
							questionByCharaIdDic.Add (charaIds[j],new List<QuestionData>());
						}
						List<QuestionData> qList = questionByCharaIdDic [charaIds [j]];
						qList.Add (questionData);
					}

					if(!tempOtherAnswerDic.ContainsKey(questionData.question_type)){
						tempOtherAnswerDic.Add (questionData.question_type,new HashSet<string>());
					}
					tempOtherAnswerDic [questionData.question_type].Add (questionData.answer);
				}
				foreach(int type in tempOtherAnswerDic.Keys){
					otherAnswerDic.Add (type,new List<string>(tempOtherAnswerDic[type]));
					//otherAnswerCountDic.Add (type,otherAnswerDic[type].Count);
				}
				foreach(int charaId in questionByCharaIdDic.Keys){
					List<QuestionData> qList = questionByCharaIdDic [charaId];
					Debug.Log ("charaId:" + charaId + ";qList:" + qList.Count);
				}
			}
		}

		Dictionary<int,List<QuestionData>> mQuestionDic;//用于随机问题
		Dictionary<int,List<QuestionData>> mOtherQuestionDic;//用于随机其它答案
		Dictionary<int,int> mQuestionDicCount;//用于不重复随机。
		void InitQuestionDatas(){
			mQuestionDic = new Dictionary<int, List<QuestionData>> ();
			mOtherQuestionDic = new Dictionary<int, List<QuestionData>> ();
			mQuestionDicCount = new Dictionary<int, int> ();
			for(int i = 0;i<questions.Count;i++){
				if (!mQuestionDic.ContainsKey (questions [i].question_type)) {
					mQuestionDic.Add (questions [i].question_type,new List<QuestionData>());
					mOtherQuestionDic.Add (questions [i].question_type,new List<QuestionData>());
					mQuestionDicCount.Add (questions [i].question_type,0);
				}
				List<QuestionData> typeQuestions = mQuestionDic[questions [i].question_type];
				typeQuestions.Add (questions[i]);

				typeQuestions = mOtherQuestionDic[questions [i].question_type];
				typeQuestions.Add (questions[i]);
				mQuestionDicCount[questions [i].question_type] = mQuestionDicCount[questions [i].question_type] + 1;
			}
			mQuestionCountOfStep1 = questionOfStep1.Count;
			mQuestionCount = questions.Count;
		}

		int mQuestionCountOfStep1;
		int mQuestionCountOfStep2;
		int mQuestionCountOfStep3;
		int mQuestionCount;
		public void SetQuestionOfStep2(int type){
			questionOfStep2 = questionByTypes [type];
			mQuestionCountOfStep2 = questionOfStep2.Count;
		}

		public void SetQuestionOfStep3(int charaId){
			questionOfStep3 = questionByCharaIdDic [charaId];
			Debug.Log ("charaId:" + charaId + ";questionOfStep3:" + questionOfStep3.Count);
			mQuestionCountOfStep3 = questionOfStep3.Count;
		}

		public List<CharacterData> GetRandomeCharacters(){
			List<CharacterData> charas = new List<CharacterData> ();
			List<CharacterData> cloneList = new List<CharacterData>();
			CharacterData sonota = null;
			//Debug.Log (cloneList.Count);
			//Debug.Log (charaList.Count);
			for(int i=0;i<charaList.Count;i++){
				if (charaList [i].id != 7) {
					cloneList.Add (charaList [i]);
				} else {
					sonota = charaList [i];
				}
			}
			for(int i=0;i<3;i++){
				CharacterData chara = cloneList [Random.Range (0, cloneList.Count)];
				cloneList.Remove (chara);
				charas.Add (chara);
			}
			charas.Add (sonota);

			//sort by chartcater.id asc
			List<CharacterData> newcharas = charas.OrderBy(s => s.id).ToList();

			/*
			for(int i=0;i<4;i++){
				if (charas [i].id == 7) {
					CharacterData c = charas[i];
					CharacterData last = charas [3];
					charas [3] = c;
					charas [i] = last;
				}
			}*/
			return newcharas;
		}

		public QuestionData GetQuestion(int step){
			QuestionData question = null;
			switch(step){
			case 1:
				question = GetQuestionOfStep1 ();
				break;
			case 2:
				question = GetQuestionOfStep2 ();
				break;
			case 3:
				question = GetQuestionOfStep3 ();
				break;
			case 4:
				question = GetQuestionOfStep4 ();
				break;
			}
			return question;
		}

		QuestionData GetQuestionOfStep1(){
			int index = Random.Range (0, mQuestionCountOfStep1);
			QuestionData questionData = questionOfStep1 [index];
			QuickRemove (questionOfStep1,index,ref mQuestionCountOfStep1);
			index = questions.IndexOf (questionData);
			QuickRemove (questions,index,ref mQuestionCount);
			SetOtherAnswers (questionData);
			return questionData;
		}

		QuestionData GetQuestionOfStep2(){
			int index = Random.Range (0, mQuestionCountOfStep2);
			QuestionData questionData = questionOfStep2 [index];
			QuickRemove (questionOfStep2,index,ref mQuestionCountOfStep2);
			index = questions.IndexOf (questionData);
			QuickRemove (questions,index,ref mQuestionCount);
			SetOtherAnswers (questionData);
			return questionData;
		}

		QuestionData GetQuestionOfStep3(){
			int index = Random.Range (0, mQuestionCountOfStep3);
			QuestionData questionData = questionOfStep3 [index];
			QuickRemove (questionOfStep3,index,ref mQuestionCountOfStep3);
			index = questions.IndexOf (questionData);
			QuickRemove (questions,index,ref mQuestionCount);
			SetOtherAnswers (questionData);
			return questionData;
		}

		QuestionData GetQuestionOfStep4(){
			int index = Random.Range (0, mQuestionCount);
			QuestionData questionData = questions [index];
			QuickRemove (questions,index,ref mQuestionCount);
			SetOtherAnswers (questionData);
			return questionData;
		}

		void QuickRemove(List<QuestionData> questionDatas,int index,ref int count){
			if (count == 0) {
				count = questionDatas.Count;
				Debug.LogError ("QuickRemove count is not enough!");
			}
			QuestionData questionData = questionDatas [index];
			QuestionData last = questionDatas [count - 1];
			questionDatas [count - 1] = questionData;
			questionDatas [index] = last;
			count--;
		}

		void SetOtherAnswers(QuestionData questionData){
			List<string> otherAnswers = otherAnswerDic [questionData.question_type];
			int count = otherAnswers.Count;
			int index = otherAnswers.IndexOf (questionData.answer);
			string last = otherAnswers [count - 1];
			otherAnswers [count - 1] = questionData.answer;
			otherAnswers [index] = last;
			count--;
			string other = GetNoDuplicateAnswer (otherAnswers,ref count);

			string additionOtherAnswer = "";
			#if DEVELOP
			additionOtherAnswer = "(wrong)";
			#endif
			if(other!=null){
				questionData.otherAnswer1 = other + additionOtherAnswer;
			}
			other = GetNoDuplicateAnswer (otherAnswers,ref count);
			if(other!=null){
				questionData.otherAnswer2 = other + additionOtherAnswer;
			}
			other = GetNoDuplicateAnswer (otherAnswers,ref count);
			if(other!=null){
				questionData.otherAnswer3 = other + additionOtherAnswer;
			}
		}

		string GetNoDuplicateAnswer(List<string> otherAnswers,ref int count){
			int index = Random.Range (0,count);
			string other = otherAnswers [index];
			string last = otherAnswers [count - 1];
			otherAnswers [count - 1] = other;
			otherAnswers [index] = last;
			count--;
			return other;
		}
	}

	[System.Serializable]
	public class ParamData{
		public int stage1_questions;//ステージ1の問題数
		public int stage2_questions;//ステージ2の問題数
		public int stage3_questions;//ステージ3の問題数
		public int stage1_time_limit;//ステージ1問題の制限時間
		public int stage2_time_limit;//ステージ2問題の制限時間
		public int stage3_time_limit;//ステージ3問題の制限時間
		public int last_stage_bonus_time;//最終ステージの補償時間
		public float stage_clear_bonus_rate;//最終ステージに+される123部の秒数の係数
		public float comb_var1;//スコアの係数
		public float comb_var2;//連続正解ボーナスの係数
		public float comb_var3;//最高連続正解ボーナスの係数 
		public int item_span_second;//最終ステージで残り○秒内黒丸効果発動
		public int item_appear_percentage;//黒丸効果発動確率
	}

	//part_type 1:開始 2:正解時 3:不正解時 4:ジャンル選択 5:推しキャラ選択 6:最終前
	[System.Serializable]
	public class SerifData{
		public int id;
		public int part_type;
		public int part_order;
		public string serif;
		public int serif_percentage;
		public string secret_tech;
		public int image_group_id;
		public int temp_percentage;
		public int tab = 1;
		public List<SerifImageData> images;
	}

	[System.Serializable]
	public class QuestionType{
		public int id;
		public string name;
	}

	[System.Serializable]
	public class SerifImageData{
		public int id;
		public int group_id;
		public string image;
		public float size;
		public float offset_x;
		public float offset_y;
	}

	[System.Serializable]
	public class QuestionData{
		public int id;
		public int question_type;
		public int card_id;
		public string answer;
		public string otherAnswer1;
		public string otherAnswer2;
		public string otherAnswer3;
	}

	[System.Serializable]
	public class CharacterData{
		public int id;
		public string name;
	}

	public class QuickRemoveList<T> where T : new (){
		List<T> mList;
		int mCount = 0;
		public QuickRemoveList()
		{
			mList = new List<T>();
		}
		public T this[int i]
		{
			get { return mList[i]; }
			set { mList[i] = value; }
		}
		public void Add(T t){
			mList.Insert (mCount,t);
			mCount++;
		}
		public void Remove(T t){
			if (mCount == 0) {
				mCount = mList.Count;
				Debug.LogError ("QuickRemove count is not enough!");
			}
			int index = mList.IndexOf (t);
			T last = mList [mCount - 1];
			mList [mCount - 1] = t;
			mList [index] = last;
			mCount--;
		}
	}


}





