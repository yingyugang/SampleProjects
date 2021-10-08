using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using Daruma;

namespace SixRun{
	//[ExecuteInEditMode]
	public class GameManager : SixRunSingleMono<GameManager> {

		public const float defaultHeight = 1334f;
		public float heightFactor = 1f;

		public GameEndLogic gameEndLogic;
		public Transform ground;
		public Transform groundMask;

		public GameObject testGround;
		public GroundSpawner groundSpawner;

		public GameObject comboEffect;
		public Camera uiCamera;
		public Camera worldCamera;

		public const int GAME_ID = 7;
		public const int OBSTACLE_LAYER = 15;
		public const int PLAYER_LAYER = 17;

		public List<SixRunBtn> playerBtns;
		public List<GameObject> players;
		public List<GameObject> cheatPlayers;
		public List<Unit> playerUnits;

		public List<GameObject> cheatGirls;

		public GameObject stageRoot;
		public GachaTweenBase[] stages;


		public AnimationCurve jumpAnim = AnimationCurve.Linear(0,0,1,1);
		public AnimationCurve upAnim = AnimationCurve.Linear(0,0,1,1);
		public AnimationCurve leftAnim = AnimationCurve.Linear(0,0,1,1);
		public AnimationCurve rightAnim = AnimationCurve.Linear(0,0,1,1);
		public AnimationCurve downAnim = AnimationCurve.Linear(0,0,1,1);

		public float speedBase = 2;//1 grid per second;
		public float immSpeed = 1;
		public float speedUp = 1;

		//public float playerDuration;

		public Vector3 forward;
		public float distance = 0;
		public float gridLength;
		//public float realSpeed;

		EnemySpawner spawner;
		public List<MapDetailData> spawnList;
		float baseDot;
		//AnisotropicFiltering defaultAnisotropicFiltering;

		//public float handleInterval = 2;
		//public float nextHandleTime;

		float immDuration = 5;
		public float immEndTime;

		int _oden;
		int _tree;
		int _rice;
		int itemScore;
		float mScoreInterval;
		int nomiss  = 0;
		int hitchhike1 = 0;
		int hitchhike2 = 0;

		//int currentLife = 9999;
		public bool isPaused = false;
		public bool imm = false;
		/*
		public Queue<Vector3> effectQueue;
		public Queue<int> effectValue;
*/

		public float cardBonus ;

		void Start(){
			if (testGround)
				testGround.SetActive (false);
			heightFactor = defaultHeight / (float)Screen.height / 10f;
			for(int i=0;i<players.Count;i++){
				players [i].transform.parent.gameObject.SetActive (false);
			}
			for(int i=0;i<cheatPlayers.Count;i++){
				cheatPlayers [i].transform.parent.gameObject.SetActive (false);
			}
			CheatData mCheatData = CheatController.GetLastMatchCheat ();
			if (mCheatData!=null && mCheatData.key != "1") {
				players = cheatPlayers;
			}
			for(int i=0;i<players.Count;i++){
				players [i].transform.parent.gameObject.SetActive (true);
			}
			//effectQueue = new Queue<Vector3> ();
			//effectValue = new Queue<int> ();
			stages = stageRoot.GetComponentsInChildren<GachaTweenBase>(true);
			ground.GetComponent<MeshRenderer> ().sortingOrder = -1;
			#if UNITY_EDITOR
			Shader shader = Shader.Find ("Unlit/VertColor");
			GroundSpawner gs = ground.GetComponent<GroundSpawner> ();
			gs.GetComponent<Renderer> ().material.shader = shader;
			Shader shader1 = Shader.Find ("Unlit/Transparent");
			groundMask.GetComponent<Renderer>().material.shader = shader1;
			#endif
			Input.multiTouchEnabled = true;
			//read config
			distance = GameParams.GetInstance ().GetStartDistance ();
			//currentLife = GameParams.GetInstance ().GetLife ();
			preDistance = GameParams.GetInstance ().GetStartDistance () - 1;
			//speed = GameParams.GetInstance ().GetSpeed ();



			isPaused = true;
			GUIManager.GetInstance ().isPaused = true;


			groundSpawner = ground.GetComponent<GroundSpawner> ();
			forward = ground.forward;
			gridLength = groundSpawner.scale * (groundSpawner.border + groundSpawner.defaultWidth);
			//realSpeed = speed * gridLength;
			playerBtns =  GUIFrant.GetInstance().btns;
			spawner = EnemySpawner.GetInstance ();
			baseDot = Mathf.Cos(45f/180 * Mathf.PI);
			StartCoroutine (_GetSpawnDic());
			for(int i=0;i<players.Count;i++){
				SetPlayer (i);
			}
			if (GameFooter.GetInstance () != null) {
				GameFooter.GetInstance ().SetOden (oden);
				GameFooter.GetInstance ().SetRice (rice);
				GameFooter.GetInstance ().SetTree (tree);
			}
			if (Header.Instance != null) {
				//Header.Instance.SetLife (LifeType.Number, currentLife);
				Header.Instance.SetLifeTime (GameParams.GetInstance().GetLife());
				Header.Instance.SetOnPauseCallback (ShowPausePanel);
				Header.Instance.SetLife (LifeType.Time, GameParams.GetInstance().GetLife());
				Header.Instance.SetShowNokori (true);
				Header.Instance.SetScore (0  + " コ");
			}
			if (PauseManager.s_Instance != null) {
				PauseManager.s_Instance.SetOnBackToGameCallback (ResumeGame);
				//PauseManager.s_Instance.SetOnExitCallback (ReStart);
			}
			if (GameResaultManager.Instance != null) {
				GameResaultManager.Instance.SetImageHeaderPanelResault (GAME_ID);//TODO
				if(UpdateInformation.GetInstance!=null)
					GameResaultManager.Instance.SetLastLevel (UpdateInformation.GetInstance.player.lv, UpdateInformation.GetInstance.player.exp);
			}
			foreach(GachaTweenBase tt in stages){
				tt.duration = tt.duration / GameParams.GetInstance ().GetReverSpeed ();
				tt.delay = tt.delay / GameParams.GetInstance ().GetReverSpeed ();
			}
//			cardBonus = GetCardBonus ();
			cardBonus = 1;
			//Time.timeScale = 0;
			//StartCoroutine (_Start());
		}

		public Vector3 WorldPosToUIPos(Vector3 pos){
			Vector3 wPos = worldCamera.WorldToScreenPoint (pos);
			wPos = uiCamera.ScreenToWorldPoint (wPos);
			return wPos;
		}


		float GetCardBonus(){
			//CardRate.GetTotal (2,GAME_ID);
			return CardRate.GetTotal(2,GAME_ID);
		}
		/*
		IEnumerator _Start(){
			yield return new WaitForSeconds (3);
			isPaused = false;
			GUIManager.GetInstance ().isPaused = false;
		}
*/
		public int oden{
			get{ 
				return _oden >> 1;
			}
			set{
				_oden = value << 1;
			}
		}

		public int tree{
			get{ 
				return _tree >> 1;
			}
			set{
				_tree = value << 1;
			}
		}

		public int rice{
			get{ 
				return _rice >> 1;
			}
			set{
				_rice = value << 1;
			}
		}

		int preDistance{
			get{ 
				return _preDistance >> 1;
			}
			set{
				_preDistance = value << 1;
			}
		}

		void OnDisable(){
			Time.timeScale = 1;
		}

		void OnDestory(){
			Time.timeScale = 1;
		}

		public float RealSpeed(){
			return speedBase * immSpeed * speedUp * GameParams.GetInstance ().GetSpeedRate () * gridLength;
		}

		Vector3 downPos;
		Vector3 upPos;
		int _preDistance;
		Vector3 prePos;
		List<ItemData> scoreItems;
		bool mStarted = false;
		public bool isPress = false;
		bool mIsSpeedUped = false;
		public bool load;
		void Update(){
			if(load){
				load = false;
				stages = GetComponentsInChildren<GachaTweenBase> (true);
			}


			if (GameCountDownMediator.didEndCountDown && !mStarted) {
				mStarted = true;
				BeginGame ();
			}
			if (isPaused)
				return;
			/*
			if(effectQueue.Count > 0){
				Vector3 pos = effectQueue.Dequeue ();
				GameObject effect = EnemySpawner.GetInstance ().SpawnComboEffect ();
				effect.transform.position = pos;
				int v = effectValue.Dequeue ();
				effect.GetComponentInChildren<InfoCombo> ().SetTextCombo(v.ToString());
			}
*/
			ground.position += ground.forward * RealSpeed() * Time.deltaTime;
			distance += Time.deltaTime * speedBase * immSpeed * speedUp * GameParams.GetInstance ().GetSpeedRate ();
			GUIManager.GetInstance().distance = distance;
			if (imm) {
				if (immEndTime < Time.time) {
					immSpeed = 1;
					//realSpeed = speed * gridLength;
					GameFooter.GetInstance ().SetParticleVisible (false);
					for(int i=0;i<players.Count;i++ ){
						players [i].GetComponent<Unit> ().StopFly ();
					}
					imm = false;
					if (ComponentConstant.SOUND_MANAGER != null) {
						ComponentConstant.SOUND_MANAGER.Play (SoundEnum.bgm11_sixrun, GetAudioSource);
					}
				} else {
					GameFooter.GetInstance ().SetSliderValue ((immEndTime-Time.time) / immDuration);
				}
			}
			Controll ();
			while(distance - preDistance >= 1){
				if(preDistance >= GameParams.GetInstance().GetSpeedUpMeter() && !mIsSpeedUped){
					mIsSpeedUped = true;
					speedUp = 1 + GameParams.GetInstance ().GetSpeedUpRate () / 100f;
				}
				bool rowSpawned = false;
				preDistance += 1;


				for(int i=0;i<spawnList.Count;i++){
					if(spawnList[i].appear_distance < preDistance){
						spawnList.RemoveAt (i);
						i--;
					}
					else{
						if (spawnList [i].appear_distance == preDistance) {
							//Debug.Log ("spawnList [i].appear_distance:" + spawnList [i].appear_distance + ";preDistance:" + preDistance);
							HashSet<int> holeIndex = EnemySpawner.GetInstance ().SpawnObstacle (spawnList [i]);
							GroundSpawner.GetInstance ().SpawnRow (holeIndex);
							rowSpawned = true;
							spawnList.RemoveAt (i);
							i--;
						}
						break;
					}
				}
				if(!rowSpawned)
					GroundSpawner.GetInstance ().SpawnRow (null);
			}

			if (Header.Instance.GetLifeTime() <= 0)
				PreGameOver ();

		}

		void Controll(){
			#if UNITY_EDITOR
			if(Input.GetMouseButtonDown(0)){
				isPress = true;
				for(int i=0;i<playerUnits.Count;i++){
					if(playerUnits[i].status != 0 && playerUnits[i].status!=1){
						isPress = false;
						break;
					}
				}
				//if(UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject()){
				//	isPress = false;
				//}
				downPos = Input.mousePosition ;
				prePos = downPos;
			}
			if(Input.GetMouseButton(0)){
				if (imm) {
					for (int i = 0; i < players.Count; i++) {
						players [i].GetComponent<Unit> ().FlyMove (Input.mousePosition - prePos);
					}
				}
				prePos = Input.mousePosition;
			}
			if(Input.GetMouseButtonUp(0)){
				if (isPress) {
					isPress = false;
					upPos = Input.mousePosition;
					OnTouchUp (downPos, upPos);
				}
			}
			#else
			if(Input.touchCount > 0){
				if(Input.GetTouch(0).phase == TouchPhase.Began){
					isPress = true;
					for(int i=0;i<playerUnits.Count;i++){
						if(playerUnits[i].status != 0 && playerUnits[i].status!=1){
							isPress = false;
							break;
						}
					}
					downPos = Input.GetTouch (0).position;
					prePos = downPos;
				}
				if(Input.GetTouch(0).phase == TouchPhase.Moved){
					if (imm) {
						for (int i = 0; i < players.Count; i++) {
							players [i].GetComponent<Unit> ().FlyMove (new Vector3(Input.GetTouch (0).position.x,Input.GetTouch (0).position.y,0) - prePos);
						}
					}
					prePos =  Input.GetTouch (0).position;
				}
				if(Input.GetTouch(0).phase == TouchPhase.Ended){
					if (isPress) {
						isPress = false;
						upPos = Input.GetTouch (0).position;
						OnTouchUp (downPos, upPos);
					}
				}
			}
			#endif
		}
			

		List<MapData> mCloneMapDatas;
		int mRoundMeter = 0;

		IEnumerator _GetSpawnDic(){

			//clone mapdata
			List<MapData> realMapDatas = GameParams.GetInstance ().mapDatas;
			mCloneMapDatas = new List<MapData>();
			//mCloneMapDatas.AddRange (realMapDatas);
			int mEndMeter = 0;
			for(int i=0;i<realMapDatas.Count;i++){
				mCloneMapDatas.Add (MapData.Clone(realMapDatas[i]));
				if (realMapDatas [i].appear_distance_end > mEndMeter)
					mEndMeter = realMapDatas [i].appear_distance_end;
			}
			Debug.Log ("mEndMeter:" + mEndMeter );
			if(spawnList==null)
				spawnList = new List<MapDetailData> ();
			int dis = 0;
			if (mRoundMeter == 0) {
				dis = GameParams.GetInstance ().GetStartDistance ();
			} else {
				dis = 0;
			}
			bool run = true;
			MapData mapData = null;


			List<MapDetailData> mapDetailDatas = new List<MapDetailData> ();

			while(run){
				if (mRoundMeter + dis - preDistance > 100) {
					yield return null;
				} else {
					if (dis >= mEndMeter) {
						mRoundMeter += mEndMeter;
						StartCoroutine (_GetSpawnDic ());
						run = false;
					} else {
						if (mapData == null || mapData.appear_distance_end <= dis) {
							mapData = GetMapData (dis);
							mapDetailDatas = null;
						}
						if (mapDetailDatas ==null || mapDetailDatas.Count == 0) {
							mapDetailDatas = GetCurrentMapDetails (dis);
						}
						//if (mapData != null && mapData.details != null) {
						MapDetailData mapDetailData = null;
					    for (int i = 0; i <mapDetailDatas.Count; i++) {
							if (mapDetailDatas [i].sub_distance < dis) {
								mapDetailDatas.RemoveAt (0);
								i--;
							} else if (mapDetailDatas [i].sub_distance>= dis) {
								if (mapDetailDatas [i].sub_distance == dis) {
									mapDetailData = mapDetailDatas [i];
								}
								break;
							}
						 }
							//Random Item
							if (mapDetailData != null) {
								ItemData itemData = null;
								mapDetailData.appear_distance = mapDetailData.sub_distance;
								bool spawnAble = false;
								if (mapDetailData.column_1_item_id > 0) {
									mapDetailData.itemData1 = GameParams.GetInstance ().itemDataDic [mapDetailData.column_1_item_id];
									spawnAble = true;
								}
								if (mapDetailData.column_2_item_id > 0) {
									mapDetailData.itemData2 = GameParams.GetInstance ().itemDataDic [mapDetailData.column_2_item_id];
									spawnAble = true;
								}
								if (mapDetailData.column_3_item_id > 0) {
									mapDetailData.itemData3 = GameParams.GetInstance ().itemDataDic [mapDetailData.column_3_item_id];
									spawnAble = true;
								}
								if (mapDetailData.column_4_item_id > 0) {
									mapDetailData.itemData4 = GameParams.GetInstance ().itemDataDic [mapDetailData.column_4_item_id];
									spawnAble = true;
								}
								if (mapDetailData.column_5_item_id > 0) {
									mapDetailData.itemData5 = GameParams.GetInstance ().itemDataDic [mapDetailData.column_5_item_id];
									spawnAble = true;
								}
								if (mapDetailData.column_6_item_id > 0) {
									mapDetailData.itemData6 = GameParams.GetInstance ().itemDataDic [mapDetailData.column_6_item_id];
									spawnAble = true;
								}
								if (spawnAble) {
									MapDetailData clone = MapDetailData.Clone (mapDetailData);
									clone.appear_distance += mRoundMeter;
									spawnList.Add (clone);
								}
        							}
						//}
					}
					dis++;
					yield return null;
				}
					/*
					int totalPercent;
					List<MapData> mapDatas = GetCurrentMapData (dis, out totalPercent);
					if (dis >= mEndMeter) {
						mRoundMeter += mEndMeter;
						StartCoroutine (_GetSpawnDic ());
						run = false;
					} else {
						int random = Random.Range (0, totalPercent);
						int value = 0;
						//Random Map Data
						MapData currentMapData = null;
						for (int i = 0; i < mapDatas.Count; i++) {
							int value1 = value + mapDatas [i].percentage;
							if (random < value1 && random >= value) {
								currentMapData = mapDatas [i];
								break;
							}
							value = value1;
						}
						//Random Map Detail Data
						MapDetailData mapDetailData = null;
						if (currentMapData != null && currentMapData.details != null) {
							for (int i = 0; i < currentMapData.details.Count; i++) {
								if (currentMapData.details [i].sub_distance + currentMapData.appear_distance < dis) {
									currentMapData.details.RemoveAt (0);
									i--;
								} else if (currentMapData.details [i].sub_distance + currentMapData.appear_distance >= dis) {
									if (currentMapData.details [i].sub_distance + currentMapData.appear_distance == dis) {
										mapDetailData = currentMapData.details [i];
									}
									break;
								}
							}
						}

						//Random Item
						if (mapDetailData != null) {
							ItemData itemData = null;
							mapDetailData.appear_distance = mapDetailData.sub_distance + currentMapData.appear_distance;
							bool spawnAble = false;
							if (mapDetailData.column_1_item_id > 0) {
								mapDetailData.itemData1 = GameParams.GetInstance ().itemDataDic [mapDetailData.column_1_item_id];
								spawnAble = true;
							}
							if (mapDetailData.column_2_item_id > 0) {
								mapDetailData.itemData2 = GameParams.GetInstance ().itemDataDic [mapDetailData.column_2_item_id];
								spawnAble = true;
							}
							if (mapDetailData.column_3_item_id > 0) {
								mapDetailData.itemData3 = GameParams.GetInstance ().itemDataDic [mapDetailData.column_3_item_id];
								spawnAble = true;
							}
							if (mapDetailData.column_4_item_id > 0) {
								mapDetailData.itemData4 = GameParams.GetInstance ().itemDataDic [mapDetailData.column_4_item_id];
								spawnAble = true;
							}
							if (mapDetailData.column_5_item_id > 0) {
								mapDetailData.itemData5 = GameParams.GetInstance ().itemDataDic [mapDetailData.column_5_item_id];
								spawnAble = true;
							}
							if (mapDetailData.column_6_item_id > 0) {
								mapDetailData.itemData6 = GameParams.GetInstance ().itemDataDic [mapDetailData.column_6_item_id];
								spawnAble = true;
							}
							if (spawnAble) {
								MapDetailData clone = MapDetailData.Clone (mapDetailData);
								clone.appear_distance += mRoundMeter;
								spawnList.Add (clone);
							}
						}

					}*/

			}
		}

		public List<MapDetailData> GetCurrentMapDetails(int dis){
			MapData mapData = GetMapData (dis);
			List<MapDetailData> details = new List<MapDetailData> ();
			if (mapData != null) {
				for(int i=0;i<mapData.details.Count;i++){
					MapDetailData detail = MapDetailData.Clone (mapData.details [i]);
					detail.sub_distance += dis;
					details.Add (detail);
				}
			}
			return details;
		}

		MapData GetMapData(int currentDis){
			List<MapData> datas = new List<MapData> ();
			int totalPercent = 0;
			List<MapData> mapDatas = mCloneMapDatas;
			for(int i=0;i < mapDatas.Count;i++){
				if(mapDatas [i].appear_distance_end <= currentDis){
					mapDatas.RemoveAt (i);
					i--;
					continue;
				}
				if (mapDatas [i].appear_distance <= currentDis) {
					if (mapDatas [i].appear_distance_end >= currentDis) {
						datas.Add (mapDatas [i]);
						if (mapDatas [i].percentage > 0) {
							mapDatas [i].temp_percentage = totalPercent + mapDatas [i].percentage;
							totalPercent += mapDatas [i].percentage;
						}
					} 
				} else {
					break;
				}
			}
			MapData mapData = null;
			int random = Random.Range (0,totalPercent);
//			Debug.Log ("random:" + random + ";totalPercent:" + totalPercent);
			for(int i=0;i<mapDatas.Count;i++){
				if(mapDatas[i].temp_percentage > random){
					mapData = mapDatas [i];
					break;
				}
			}
//			if ( mapData !=null )
//				Debug.Log("currentDis::" + "mapData::   " + currentDis + ":" + mapData.id);
			return mapData;
		}

		List<MapData> GetCurrentMapData(int currentDis,out int totalPercent){
			List<MapData> datas = new List<MapData> ();
			totalPercent = 0;
			List<MapData> mapDatas = mCloneMapDatas;
			for(int i=0;i < mapDatas.Count;i++){
				if(mapDatas [i].appear_distance_end < currentDis){
					mapDatas.RemoveAt (i);
					i--;
					continue;
				}
				if (mapDatas [i].appear_distance <= currentDis) {
					if (mapDatas [i].appear_distance_end >= currentDis) {
						datas.Add (mapDatas [i]);
						if (mapDatas [i].percentage > 0)
							totalPercent += mapDatas [i].percentage;
					} 
				} else {
					break;
				}
			}
			return datas;
		}

		AudioSource m_Audio;
		void BeginGame(){
			ResumeGame ();
			for(int i=0;i<playerUnits.Count;i++){
				playerUnits [i].ResumeAnim ();
			}
			if (ComponentConstant.SOUND_MANAGER != null) {
				ComponentConstant.SOUND_MANAGER.Play (SoundEnum.bgm11_sixrun, GetAudioSource);
			}
		}

		public void GetAudioSource (AudioSource audio)
		{
			if (m_Audio == null || m_Audio != audio) {
				m_Audio = audio;
				PauseManager.s_Instance.Init (GAME_ID, m_Audio);
			}
		}

		void ShowPausePanel(){
			PauseGame ();
			if(PauseManager.s_Instance!=null)
				PauseManager.s_Instance.SetVisible (true);
		}

		public void PauseGame(){
			Time.timeScale = 0;
			isPaused = true;
			isPress = false;
			for(int i=0;i<playerUnits.Count;i++){
				playerUnits [i].anim.enabled = false;
			}
		}

		public void ResumeGame(){
			isPaused = false;
			Time.timeScale = 1;
			isPress = false;
			for(int i=0;i<playerUnits.Count;i++){
				playerUnits [i].anim.enabled = true;
			}
			//GameFooter.Instance.SetParticleVisible(true);
		}

		public void ReStart(){
			UnityEngine.SceneManagement.SceneManager.LoadScene ("Sixrun");
		}

		public void SetPlayer(int index){
			GameObject player = players[index];
			player.SetActive (true);
			Unit unit = player.GetComponent<Unit> ();
			unit.PauseAnim ();
			playerUnits.Add (unit);
			SixRunBtn btn = playerBtns [index];
			btn.unit = unit;
			unit.posTween.from = unit.posTween.transform.localPosition;
			unit.jumpAnimCurve = jumpAnim;
			unit.jumpPos = unit.posTween.from + new Vector3(0,GameParams.GetInstance ().GetJumpHeight (),0);
			unit.upAnimCurve = upAnim;
			unit.upPos = unit.posTween.from + new Vector3(0,GameParams.GetInstance ().GetDoubleJumpHeight (),0);
			unit.downAnimCurve = downAnim;
			unit.downPos = unit.posTween.from + new Vector3(0,GameParams.GetInstance ().GetDownHeight (),0);
			unit.leftAnimCurve = leftAnim;
			unit.leftPos = players [0].transform.position - unit.posTween.transform.position;
			unit.rightAnimCurve = rightAnim;
			unit.rightPos = players [5].transform.position - unit.posTween.transform.position;
			unit.PlayRun ();
		}

		public void ObstacleHit(){
			if(nomiss==0)
				nomiss = preDistance;
			if (Time.time - immEndTime < 2)//飞行结束后有2s免疫，防止掉下来刚好碰到obs
				return;
			curCombo = 0;
			GUIFrant.GetInstance ().comboCur.text = curCombo.ToString ();
			for(int i=0;i<GameManager.GetInstance().players.Count;i++){
				GameManager.GetInstance ().players [i].GetComponent<Unit> ().ToggleColor ();
			}
			//currentLife--;
			//Header.Instance.SetLife (LifeType.Number, currentLife);
		}

		int _curCombo = 0;
		public int curCombo{
			get{
				return _curCombo >> 1;
				}
			set{
				_curCombo = value << 1;
			}
		}

		int _maxCombo = 0;
		public int maxCombo{
			get{
				return _maxCombo >> 1;
			}
			set{
				_maxCombo = value << 1;
			}
		}

		public void ItemHit(Item item){
			//Debug.Log (type);
			itemScore += item.itemData.effectValue;
			curCombo++;
			if(curCombo > maxCombo){
				maxCombo = curCombo;
				GUIFrant.GetInstance ().comboMax.text = maxCombo.ToString ();
			}
			//GameManager.GetInstance ().effectValue.Enqueue (curCombo);
			GUIFrant.GetInstance ().comboCur.text = curCombo.ToString ();

			GameObject effect = EnemySpawner.GetInstance ().SpawnComboEffect ();
			Transform body = item.transform.FindChild ("bodyPos");
			if (body == null)
				body = item.transform;
			Vector3 pos = WorldPosToUIPos (body.position);
			effect.transform.position = pos;

			switch(item.itemData.itemResource){
			case "tree":
				tree++;
				GameFooter.GetInstance().SetTree (tree);
				break;
			case "oden":
				oden++;
				GameFooter.GetInstance().SetOden (oden);
				break;
			case "rice":
				rice++;
				GameFooter.GetInstance().SetRice (rice);
				break;
			default:
				break;
			}
			Header.Instance.SetScore (tree + oden + rice  + " コ");
		}

		public void ImmHit(MapDetailData data){
			if (ComponentConstant.SOUND_MANAGER != null)
				ComponentConstant.SOUND_MANAGER.Play (SoundEnum.bgm10_invicible,GetAudioSource);
			GUIManager.GetInstance ().CutIn ();
			immSpeed = 2;
			//realSpeed = speed * gridLength;
			imm = true;
			immDuration = GameParams.GetInstance().GetFeverTime();
			immEndTime = Time.time + immDuration;
			GameFooter.GetInstance ().SetSliderValue (1);
			GameFooter.GetInstance ().SetParticleVisible (true);
			for(int i=0;i<players.Count;i++ ){
				players [i].GetComponent<Unit> ().Fly ();
			}
		}

		void OnTouchUp(Vector3 pos0,Vector3 pos1){
			Vector3 dic = pos1 - pos0;
			float dist = dic.magnitude;
			if(dist > Mathf.Max(5,GameParams.GetInstance().GetFlickSensitivity())){
				if (imm || GameManager.GetInstance().isPaused)
					return;
				//只要有人在操作中，同时的操作就不能进行。
				for (int i = 0; i < playerBtns.Count; i++) {
					Unit unit = players [i].GetComponent<Unit> ();
					if (GameManager.GetInstance ().isPaused || (unit.status != 0 && unit.status != 1))
						return;
				}
				//nextHandleTime = Time.time + handleInterval;
				float dot = Vector3.Dot (dic.normalized,new Vector3(1,0,0));
				if(dot>=baseDot){
					if (dist < GameParams.GetInstance().GetFlickSensitivity() + 20)
						return;

					for(int i=0;i<playerBtns.Count;i++){
						Unit unit = players [i].GetComponent<Unit> ();
						//if (imm|| GameManager.GetInstance().isPaused || unit.status != 0)
						//	continue;
						unit.posTween.duration = GameParams.GetInstance().GetFlickDuration();
						unit.shadowPosTween.duration = GameParams.GetInstance().GetFlickDuration();
						unit.shadowSizeTween.duration =  GameParams.GetInstance().GetFlickDuration();
						players [i].GetComponent<Unit>().MoveRight ();
					}
				}
				else if(dot<=-baseDot){
					if (dist < 50)
						return;
					
					for(int i=0;i<playerBtns.Count;i++){
						Unit unit = players [i].GetComponent<Unit> ();
						//if (imm|| GameManager.GetInstance().isPaused || unit.status != 0)
						//	continue;
						unit.posTween.duration = GameParams.GetInstance().GetFlickDuration();
						unit.shadowPosTween.duration = GameParams.GetInstance().GetFlickDuration();
						unit.shadowSizeTween.duration =  GameParams.GetInstance().GetFlickDuration();
						players [i].GetComponent<Unit>().MoveLeft ();
					}
				}
				else if(dot<= baseDot && dic.y > 0){
					if (ComponentConstant.SOUND_MANAGER != null)
						ComponentConstant.SOUND_MANAGER.Play (SoundEnum.SE34_bike_ramp);
					for(int i=0;i<playerBtns.Count;i++){
						Unit unit = players [i].GetComponent<Unit> ();
						//if (imm|| GameManager.GetInstance().isPaused || unit.status != 0)
						//	continue;
						unit.posTween.duration = GameParams.GetInstance ().GetDoubleJumpDuration ();
						unit.shadowPosTween.duration = GameParams.GetInstance ().GetDoubleJumpDuration ();
						unit.shadowSizeTween.duration = GameParams.GetInstance ().GetDoubleJumpDuration ();
						players [i].GetComponent<Unit>().TwiceJump ();
					}
					CheatData mCheatData = CheatController.GetLastMatchCheat ();
					if (mCheatData!=null && mCheatData.key == "1") {
						foreach(GameObject go in cheatGirls){
							go.GetComponent<GachaPosition> ().duration = GameParams.GetInstance ().GetFlickDuration ();
							go.SetActive (true);
						}
					}
					hitchhike1++;
				}
				else if(dot<= baseDot && dic.y < 0){
					if (ComponentConstant.SOUND_MANAGER != null)
						ComponentConstant.SOUND_MANAGER.Play (SoundEnum.SE16_mutsugo_fall);
					for(int i=0;i<playerBtns.Count;i++){
						Unit unit = players [i].GetComponent<Unit> ();
						//if (imm|| GameManager.GetInstance().isPaused || unit.status != 0)
						//	continue;
						unit.posTween.duration = GameParams.GetInstance ().GetDownDuration ();
						unit.shadowPosTween.duration = GameParams.GetInstance ().GetDownDuration ();
						unit.shadowSizeTween.duration =  GameParams.GetInstance ().GetDownDuration ();
						players [i].GetComponent<Unit>().MoveDown ();
					}
					hitchhike2++;
				}
			}
		}

		public int GetTotalScore()
		{
			return GetScore() + GetScoreBonus () + GetComboBonus ();
		}

		public int GetScore(){
			return (int)((oden + tree + rice) * APIInformation.GetInstance.gameparameter.comb_var2);
		}

		public int GetScoreBonus()
		{
			//int distance = (int) m_Distance;
			int scoreOden = oden * GameParams.GetInstance().itemDataDic1["oden"].effectValue;
			int scoreTree = tree * GameParams.GetInstance().itemDataDic1["tree"].effectValue;
			int scoreRice = rice * GameParams.GetInstance().itemDataDic1["rice"].effectValue;
			return scoreOden + scoreTree + scoreRice;
		}

		public int GetComboBonus(){
			return (int)(maxCombo * APIInformation.GetInstance.gameparameter.comb_var1);
		}


		public int GetTotalItem (){
			return oden + tree + rice;
		}

		public void SendGameEndingAPI ()
		{
			gameEndLogic.m_game_id = GAME_ID;
			//gameEndLogic.score = GetTotalScore();
			gameEndLogic.score = Mathf.RoundToInt(GetTotalScore() * (1 + this.cardBonus / 100));
			gameEndLogic.move_metre = preDistance ;
			gameEndLogic.item_get_num = GetTotalItem();
			if(nomiss==0)nomiss = preDistance;
			gameEndLogic.nomiss = nomiss;
			gameEndLogic.hitchhike1 = hitchhike1;
			gameEndLogic.hitchhike2 = hitchhike2;
			gameEndLogic.combo_num = maxCombo;
			gameEndLogic.card_bonus = this.cardBonus;
			gameEndLogic.card_ids = CardRate.GetAdditionalCardID (2,GAME_ID);
			//gameEndLogic.all_muster = 1;
			//gameEndLogic.over_2weeks = 2;
			gameEndLogic.complete = SendAPICompleteHandler;
			gameEndLogic.error = SendAPIErrorHandler;
			gameEndLogic.SendAPI ();
		}

		void SendAPICompleteHandler ()
		{
			Debug.Log ("SendAPICompleteHandler!" + Player.GetInstance.exp);
			DisplayGameover();
		}

		void SendAPIErrorHandler (string str)
		{
			Debug.Log ("SendAPIErrorHandler! " + str);
			DisplayGameover();
		}

		public void Pause1(){
			isPaused = true;
			isPress = false;
			foreach(Unit unit in playerUnits){
				unit.posTween.enabled = false;
				unit.shadowPosTween.enabled = false;
				unit.shadowSizeTween.enabled = false;
				//unit.anim.Stop ();
			}
			foreach(GachaTweenBase tt in stages){
				tt.enabled = false;
			}
		}

		public void Resume1(){
			isPaused = false;
			foreach(Unit unit in playerUnits){
				unit.posTween.enabled = true;
				unit.shadowPosTween.enabled = true;
				unit.shadowSizeTween.enabled = true;
				//unit.anim.Stop ();
			}
			foreach(GachaTweenBase tt in stages){
				tt.enabled = true;
			}
		}

		public void PreGameOver()
		{
			//Invoke("PlayTimeupSound", 0.2f);
			if(ComponentConstant.SOUND_MANAGER!=null)
				ComponentConstant.SOUND_MANAGER.Play(SoundEnum.se07_timeup);
			if (ComponentConstant.SOUND_MANAGER != null) {
				ComponentConstant.SOUND_MANAGER.StopBGM (false,false,true);
				//ComponentConstant.SOUND_MANAGER.Play (SoundEnum.se07_timeup);
			}
			Invoke ("_PlayEndBgm",2f);
			isPaused = true;
			isPress = false;
			foreach(Unit unit in playerUnits){
				unit.posTween.enabled = false;
				unit.shadowPosTween.enabled = false;
				unit.shadowSizeTween.enabled = false;
				unit.anim.Stop ();
			}
			foreach(GachaTweenBase tt in stages){
				tt.enabled = false;
			}
			GameFooter.GetInstance ().SetParticleVisible (false);
			GUIManager.GetInstance().ShowEnding();
		}

		void _PlayEndBgm(){
			if (ComponentConstant.SOUND_MANAGER != null) {
				ComponentConstant.SOUND_MANAGER.Play (SoundEnum.bgm15_title_back, GetAudioSource);
			}
		}

		void DisplayGameover()
		{
			if (ComponentConstant.SOUND_MANAGER != null) {
				ComponentConstant.SOUND_MANAGER.StopBGM (false,false,true);
			}
			GameResaultManager.Instance.SetGameResultInformation(
				GetScore(), 
				GetScoreBonus(), 
				GetComboBonus(), 
				APIInformation.GetInstance.rank,cardBonus, true, true, APIInformation.GetInstance.rank > 0
			);


			/*
			GameResaultManager.Instance.SetUser(
				UpdateInformation.GetInstance.player.name,
				GameConstant.headerSprite,
				false);*/
		}
		#if UNITY_EDITOR
		GUIStyle s = null;
		void OnGUI(){
			return;
			if (s == null) {
				s = new GUIStyle ();
				s.fontSize = 30;
				s.fontStyle = FontStyle.Bold;
				s.normal.textColor = Color.red;
			}
			GUI.Label (new Rect(10,10,100,50),preDistance.ToString(),s);

			//if(Input.touchCount > 0){
				GUI.Label (new Rect(10,50,800,50),heightFactor.ToString(),s);
			//	GUI.Label (new Rect(10,100,800,50),defaultHeight + "||" + Screen.height,s);
			//}



		}
		#endif



	}
}
