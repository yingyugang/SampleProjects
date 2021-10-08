//#define cheat_test
using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class CheatController : MonoBehaviour{

	public delegate void OnMatched();
	public OnMatched onMatched;

	public const string CSV_CHEAT = "csv/m_cheat";
	public const string CSV_CHEAT_CARD = "csv/m_cheat_card";


	bool isPress;
	public List<CheatData> cheatListData;
	Dictionary<int,CheatData> cheatDicData;
	Dictionary<int,List<CheatData>> sceneCheats;
	public SceneEnum currentGameEnum;
	public static List<CheatData> currentSceneCheats;
	public List<CheatCardData> cheatCardListData;
	public List<int> cheatCardIDList;
	//public Dictionary<int,CheatCardData> cheatCardDicData;
	public Dictionary<int, Dictionary<int,List<CheatCardData>>> cardGameCheats;//key1 cardId,key2 gameId

	private int game_id;
	private GameDetailTopMediator mGameDetailTopMediator;

	//public List<int> currentMatchHandles;
	//public int matchIndex;
	//public List<int> matchedCheatIds;
	//public static bool isMatched;


	static CheatController instance;
	public static CheatController GetInstance(){
		return instance;
	}

	public static bool IsCheated(int index){
		if(currentSceneCheats!=null && currentSceneCheats.Count > index && currentSceneCheats[index].isMatched){
			return true;
		}
		return false;
	}

	public static CheatData GetFirstMatchCheat(){
		if(currentSceneCheats!=null ){
			foreach(CheatData c in currentSceneCheats){
				if(c.isMatched){
					return c;
				}
			}
		}
		return null;
	}

	public static CheatData GetLastMatchCheat(){
		CheatData cheatData = null;
		if(currentSceneCheats!=null ){
			foreach(CheatData c in currentSceneCheats){
				if(c.isMatched){
					cheatData = c;
				}
			}
		}
		return cheatData;
	}


	public static void ResetCheats(){
		if (currentSceneCheats != null) {
			foreach (CheatData cheat in currentSceneCheats) {
				if (cheat != null) {
					cheat.Reset ();
				}
			}
		}
	}

	void Awake(){
		instance = this;
		cheatDicData = new Dictionary<int, CheatData> ();
		sceneCheats = new Dictionary<int, List<CheatData>> ();
		mGameDetailTopMediator = GetComponentInChildren<GameDetailTopMediator> (true);
		LoadCheatDatas ();
		LoadCheatCardDatas ();
		minMoveDist = Mathf.Min(20,Screen.width/10f);
	}

	//private CardCSVStructure cardCSVStructure;
	//string preHead = null;
	public void SetCurrentGame(int gameId){
		currentSceneCheats = new List<CheatData> ();
		if (!sceneCheats.ContainsKey (gameId))
			return;
		game_id = gameId;
		GetCard ();
		if(cardGameCheats.ContainsKey(preCardId)){
			Dictionary<int,List<CheatCardData>> cardCheats = cardGameCheats [preCardId];
			if(cardCheats.ContainsKey(game_id)){
				List<CheatCardData> cheats = cardCheats[game_id];
				for(int i=0;i<cheats.Count;i++){
					CheatData cd = cheatDicData[cheats[i].cheatId];
					if(!currentSceneCheats.Contains(cd))
						currentSceneCheats.Add (cd);
					cd.currentMatchId = -1;
					cd.isMatched = false;
					cd.isMatchedHandle = false;
				}
				isPress = false;
			}
		}
	}

	void LoadCheatDatas(){
		cheatListData = new List<CheatData> ();
		List<Dictionary<string, object>> data = CSVReader.Read(CSV_CHEAT);
		for (int i = 0; i < data.Count; i++)
		{
			CheatData cheatData = new CheatData ();
			cheatData.cheatId = (int)data [i] ["id"];
			cheatData.gameId = (int)data [i] ["m_game_id"];
			cheatData.key = data [i] ["key"].ToString();
			string handleStr = data [i] ["handles"].ToString ();
			//Debug.Log (handleStr);
			string[] handleStrs = handleStr.Split ('|');
			List<int> handleInts = new List<int> ();
			int handle = 0;
			for (int j = 0; j < handleStrs.Length; j++) {
				if(handleStrs [j] == null || handleStrs [j].Trim()==""){
					continue;
				}
				if (int.TryParse (handleStrs [j].Trim (), out handle)) {
					handleInts.Add (handle);
				} else {
					Debug.Log ("Wrong handle id format : " + handleStrs [j].Trim ());
				}
			}
			cheatData.handles = handleInts;
			cheatListData.Add (cheatData);
			if (!cheatDicData.ContainsKey (cheatData.cheatId))
				cheatDicData.Add (cheatData.cheatId,cheatData);
			else
				Debug.LogError("Dup cheatId : " + cheatData.cheatId);
			//SceneEnum sceneEnum = GetSceneEnumByGameId (c.gameId);
			if(!sceneCheats.ContainsKey(cheatData.gameId)){
				sceneCheats.Add (cheatData.gameId, new List<CheatData> ());
			}
			sceneCheats [cheatData.gameId].Add (cheatData);
		}
	}

	int preCardId = -1;
	void GetCard(){
		if(preCardId!=GameConstant.currentHeadImageID){
			preCardId = GameConstant.currentHeadImageID;
			Debug.Log ("preCardId:" + preCardId);
			if (game_id != -1)
				SetCurrentGame (game_id);
		}
		/*
		if (preHead != Player.GetInstance.head_image) {
			preHead = Player.GetInstance.head_image;
			//cardCSVStructure = MasterCSV.cardCSV.FirstOrDefault (result => int.Parse (Player.GetInstance.head_image).ToString (LanguageJP.FOUR_MASK) == int.Parse (result.image_resource).ToString (LanguageJP.FOUR_MASK));
			//if (cardCSVStructure != null) {
				Debug.Log ("cardCSVStructure:" + "head_image:" + preHead);
				if (game_id != -1)
					SetCurrentGame (game_id);
			//}
		}
		*/
	}

	void LoadCheatCardDatas(){
		cheatCardListData = new List<CheatCardData> ();
		List<Dictionary<string, object>> data = CSVReader.Read(CSV_CHEAT_CARD);
		cardGameCheats = new Dictionary<int, Dictionary<int,List<CheatCardData>>> ();
		cheatCardIDList = new List<int> ();
		for (int i = 0; i < data.Count; i++){
			CheatCardData cheatCardData = new CheatCardData ();
			cheatCardData.id = (int)data[i]["id"];
			cheatCardData.cardId = (int)data[i]["m_card_id"];
			cheatCardData.cheatId = (int)data[i]["m_cheat_id"];
			if(!cheatDicData.ContainsKey(cheatCardData.cheatId)){
				Debug.LogError ("cheatCardData.cheatId:" + cheatCardData.cheatId + " is not exiting!");
				continue;
			}
			cheatCardData.gameId = cheatDicData [cheatCardData.cheatId].gameId;
			cheatCardListData.Add (cheatCardData);
			if(!cardGameCheats.ContainsKey(cheatCardData.cardId)){
				cardGameCheats.Add (cheatCardData.cardId,new  Dictionary<int,List<CheatCardData>>());
			}
			Dictionary<int,List<CheatCardData>> gameCheatDic = cardGameCheats[cheatCardData.cardId];
			if(!gameCheatDic.ContainsKey(cheatCardData.gameId)){
				gameCheatDic.Add (cheatCardData.gameId,new List<CheatCardData>());
			}
			List<CheatCardData> cheatCardDataList = gameCheatDic [cheatCardData.gameId];
			cheatCardDataList.Add (cheatCardData);
			cheatCardIDList.Add (cheatCardData.cardId);
		}
	}

	public int height = 300;
	public float defaultScreenHeight = 1920f;
	int minTouchHeight ;
	Vector2 mDownPos;
	float mDownTime;
	float minPressTime = 6f;
	float mPressCheckInterval = 0.1f;
	float mNextPressCheckTime = 0;
	int touchIndex = 0;
	void Update(){
		GetCard ();


		if (currentSceneCheats == null || currentSceneCheats.Count == 0 || !mGameDetailTopMediator.gameObject.activeInHierarchy)
			return;
		#if UNITY_EDITOR
		if(Input.GetMouseButtonDown(0)){
			isPress = HeightMatch(Input.mousePosition);
		}
		if(Input.GetMouseButton(0)){
			if(isPress && mNextPressCheckTime <= Time.time){
				mNextPressCheckTime = Time.time + mPressCheckInterval;
				OnTouchPress (mDownPos,Input.mousePosition,mDownTime,Time.time);
			}
		}
		if(Input.GetMouseButtonUp(0)){
			if (isPress) {
				isPress = false;
				OnTouchUp (mDownPos,Input.mousePosition,mDownTime,Time.time);
			}
		}
		#endif


		if (Input.touchCount > 0) {
			if (Input.GetTouch (Input.touchCount-1).phase == TouchPhase.Began) {
				isPress = HeightMatch(Input.mousePosition);
			}
			if(Input.GetTouch (Input.touchCount-1).phase == TouchPhase.Moved){
				if(isPress && mNextPressCheckTime <= Time.time){
					mNextPressCheckTime = Time.time + mPressCheckInterval;
					OnTouchPress (mDownPos,Input.mousePosition,mDownTime,Time.time);
				}
			}
			if (Input.GetTouch (Input.touchCount-1).phase == TouchPhase.Ended) {
				if (isPress) {
					isPress = false;
					OnTouchUp (mDownPos,Input.mousePosition,mDownTime,Time.time);
				}
			}
		}
	}

	public float minMoveDist = Screen.width/10f;
	void OnTouchUp(Vector2 startPos, Vector2 endPos,float startTime,float endTime){
		if(preCardId == -1 ){
			//Debug.Log ("cardCSVStructure is null");
			return;
		}
		//if(cardCSVStructure.game_id != game_id){
		//	Debug.Log ("cardCSVStructure.game_id:" + cardCSVStructure.game_id + ";game_id:" + game_id);
		//	return;
		//}
		Vector2 dir = endPos - startPos;
		float dist = dir.magnitude;
		bool isClick = false;
		if(dist < minMoveDist && endTime - startTime < 1){//不超过1s算点击
			isClick = true;
		}
		if (isClick) {
			float pressDuration = endTime - startTime;
			int clickPosIndex = WidthMatch (endPos);
			switch (clickPosIndex) {
			case 0:
				Match (4);
				break;
			case 1:
				Match (5);
				break;
			case 2:
				Match (6);
				break;
			}
		} 
		else 
		{
			if (dir.x < 0) {
				if (Mathf.Abs (dir.y) <= Mathf.Abs (dir.x)) {
					Match (0);
				}
			}
			if (dir.x > 0) {
				if (Mathf.Abs (dir.y) <= Mathf.Abs (dir.x)) {
					Match (1);
				}
			}
			if (dir.y > 0) {
				if (Mathf.Abs (dir.y) >= Mathf.Abs (dir.x)) {
					Match (2);
				}
			}
			if (dir.y < 0) {
				if (Mathf.Abs (dir.y) >= Mathf.Abs (dir.x)) {
					Match (3);
				}
			}
		}


		for(int i=0;i<currentSceneCheats.Count;i++){
			currentSceneCheats [i].isMatchedHandle = false;
		}
	}

	//only for longpress
	void OnTouchPress(Vector2 startPos, Vector2 endPos,float startTime,float endTime){
		if(preCardId == -1){
			return;
		}
		float pressDuration = endTime - startTime;
		if (pressDuration <= 1)
			return;
		//Debug.Log (pressDuration);
		for(int i=0;i<currentSceneCheats.Count;i++){
			if(currentSceneCheats[i].handles.Count <= currentSceneCheats[i].currentMatchId + 1){
				continue;
			}
			if(currentSceneCheats[i].isMatched){
				continue;
			}
			if (currentSceneCheats [i].isMatchedHandle) {
				continue;
			}
			//if(cardCSVStructure.cheat_id>0 && cardCSVStructure.cheat_id!=currentSceneCheats[i].cheatId){
			//	continue;	
			//}
			int curHandle = currentSceneCheats [i].handles [currentSceneCheats [i].currentMatchId + 1];
			if(curHandle > 10){
				int clickPosIndex = WidthMatch (endPos);
				string handleBase = "";
				switch (clickPosIndex) {
					case 0:
					handleBase = "7";
						break;
					case 1:
					handleBase = "8";
						break;
					case 2:
					handleBase = "9";
						break;
				}
				string curHandleBase = curHandle.ToString ().Substring (0,1);
				if (curHandleBase == handleBase) {
					int minDuration = int.Parse (curHandle.ToString ().Remove (0, 1));
					if (pressDuration >= minDuration) {
						currentSceneCheats [i].isMatchedHandle = true;
						currentSceneCheats [i].currentMatchId++;
						Debug.Log ("long press!" + handleBase);
						if (currentSceneCheats [i].handles.Count == currentSceneCheats [i].currentMatchId + 1) {
							Debug.Log ("Success");
							//#if DEVELOP
							if (ComponentConstant.SOUND_MANAGER != null) 
								ComponentConstant.SOUND_MANAGER.Play (SoundEnum.SE31_bike_jump);
							//#endif
							currentSceneCheats [i].isMatched = true;
							if (onMatched != null)
								onMatched ();
						}
					}
				} else {
					currentSceneCheats [i].currentMatchId = -1;
					currentSceneCheats [i].isMatchedHandle = false;
				}

			}
		}
	}

	bool HeightMatch(Vector3 mousePos){
		minTouchHeight = (int)((defaultScreenHeight - height) * (Screen.height / defaultScreenHeight));
		if(mousePos.y >= minTouchHeight){
			mDownPos = Input.mousePosition;
			mDownTime = Time.time;
			return true;
		}
		else
		{
			if (currentSceneCheats != null) {
				foreach (CheatData c in currentSceneCheats) {
					if (c.isMatched)
						continue;
					c.currentMatchId = -1;
					c.isMatched = false;
				}
			}
			return false;
		}
	}

	int WidthMatch(Vector3 mousePos){
		float minSplitWidth = Screen.width / 3.0f;
		if (mousePos.x <= minSplitWidth) {
			return 0;
		} else if (mousePos.x > minSplitWidth && mousePos.x <= minSplitWidth * 2) {
			return 1;
		} else {
			return 2;
		}
	}

	void Match(int direct){
		if(currentSceneCheats==null || currentSceneCheats.Count == 0){
			return;
		}
		Debug.Log (direct);
		for(int i=0;i<currentSceneCheats.Count;i++){
			if(currentSceneCheats[i].isMatched){
				continue;
			}
			if (currentSceneCheats [i].isMatchedHandle) {
				currentSceneCheats [i].isMatchedHandle = false;
				continue;
			}
			//if(cardCSVStructure.cheat_id>0 && cardCSVStructure.cheat_id!=currentSceneCheats[i].cheatId){
			//	continue;	
			//}
			List<int> currentMatchHandles = currentSceneCheats [i].handles;
			int matchDirect = currentMatchHandles [currentSceneCheats [i].currentMatchId + 1];
			if (matchDirect == direct) {
				
				currentSceneCheats[i].currentMatchId++;
				if(currentMatchHandles.Count == currentSceneCheats[i].currentMatchId + 1){
					Debug.Log ("Success");
					//#if DEVELOP
					if (ComponentConstant.SOUND_MANAGER != null) 
						ComponentConstant.SOUND_MANAGER.Play (SoundEnum.SE31_bike_jump);
					//#endif
					currentSceneCheats[i].isMatched = true;
					if (onMatched != null)
						onMatched ();
				}
			} else {
				currentSceneCheats [i].currentMatchId = -1;
			}
		}

	}

	#if cheat_test
	void OnGUI(){
		for(int i=1;i<7;i++){
			if(GUI.Button(new Rect(10 + (i-1)*40,Screen.height - 80,40,40),i.ToString())){
				SetCurrentGame (GetSceneEnumByGameId(i));
			}
		}
	}
	#endif



}

[System.Serializable]
public class CheatData{
	public int cheatId;//primary key
	public int gameId;//0:Daruma,1:GetOut,2:Swimming,3:Shee,4:BreakoutClone,5:Biking;
	public int currentMatchId = -1;
	public bool isMatchedHandle = false;//for long press
	public bool isMatched = false;
	public List<int> handles;//0:left ,1:right, 2:up,3:down,4:click left,5:click middle,6:click right,7:left long press,8:center long press,9:right long press ;
	public string key;
	public void Reset(){
		currentMatchId = -1;
		isMatchedHandle = false;
		isMatched = false;
	}
}

[System.Serializable]
public class CheatCardData{
	public int id;
	public int cardId;
	public int gameId;
	public int cheatId;
}