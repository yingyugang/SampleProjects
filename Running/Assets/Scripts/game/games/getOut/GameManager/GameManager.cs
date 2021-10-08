using UnityEngine;
using System.Collections;
using System.Collections.Generic;
namespace GetOut
{

	public enum LogicType{
		LogicRandom = 0,
		LogicFollow,
		LogicGoStraight,
		LogicFollowWall
	}


	public class GameManager : MonoBehaviour {
		
		private static GameManager _instance;


		public CameraControll cameraControl;
		public GameEndLogic gameEndLogic;
		//value in game
		public int life = 3;					//number life of player
		public float totalTime = 0;				//total time	
		public LoadMap map;						//Map
		public List<EnemyFooterIcon> lstEnemyIconFooter;
		public List<GameObject> lstFamilyTsuObj;
		public List<GameObject> lstEnemyObj;
		public List<LogicBase> lstFamilyTsu;	//List person of family tsu	
		public List<LogicBase> lstEnemy;		//list enemy
		public List<Item> lstItem;				//list item
		public PlayerControll player;	
		public GameObject particleStarEffect;
		//variable game control
		public int hideAreaShow = 0;
		public int numberItemScore1;
		public int numberItemScore2;
		public int numberItemScore3;
		public int numberItemActive;			//number of item is active in the scene
		public int numberEnemyActive;			//number of enemy is active in the scene
		public bool isStar;						//check the star effect when the player get an item star
		public bool isClock;					//check the clock effect when the player get an item clock
		public bool isGameOver;					//game over
		public bool isPause;					//pause
		public bool isInitItem;					//check init item
		public float timeClock;					//time life of effect clock
		public float timeStar;					//time life of effect star
		public float percentageAppearStar;		//percentage appear of star item
		public float percentageAppearClock;		//percentage appear of clock item
		public float percentageAppearScore1;	//percentage appear of score1 item
		public float percentageAppearScore2;	//percentage appear of score2 item
		public float percentageAppearScore3;	//percentage appear of score3 item
		public float timeInitItem;				//time span item
		public bool isGateLeftOpen;
		public bool isGateRightOpen;
		public float timeGateRightClose;
		public float timeGateLeftClose;
		public int idOfShounosuke;
		public int numberItemStar;
		public int numberItemClock;
		private float totalItemAppearPercent;
		public bool isOpening = true;
		public int idGame = 2;
		//variable score
		public int score;						//score of game
		public int scoreBonus;
		public AudioSource audioSourceBackground;

		float cardBonus;

		private bool m_IsClockFirstEffect;			//if play use item clock first it's true
		void Awake()
		{
			if (ComponentConstant.SOUND_MANAGER) {
				ComponentConstant.SOUND_MANAGER.StoreSounds (new List<SoundEnum> () {
					SoundEnum.bgm04_escape,
					SoundEnum.bgm10_invicible,
				});
			}
			_instance = this;
			cardBonus = GetCardBonus ();
		}

		float GetCardBonus(){
			//
			return CardRate.GetTotal (2,idGame);
		}

		void Start()
		{
			InitMap();
//			SoundManager.instance.PlayOneShoot(SoundType.Countdown);
			if(ComponentConstant.SOUND_MANAGER != null)
				ComponentConstant.SOUND_MANAGER.Play(SoundEnum.se04_start_countdown);
		}





		public void GetAudioSource (AudioSource audioSource)
		{
//			Debug.Log (audioSource);
			this.audioSourceBackground = audioSource;
			PauseManager.s_Instance.Init(idGame,audioSourceBackground);
		}

		void SendGameEndingAPI ()
		{
			for(int i = 0; i < lstEnemy.Count; i++)
			{
				if(lstEnemy[i].idEnemy == idOfShounosuke)
				{
					if(totalTime >= lstEnemy[i].timeAppear)
					{
						gameEndLogic.syonosuke_show = 1;
					}
					else
						gameEndLogic.syonosuke_show = 0;
				}
			}
			gameEndLogic.score = Mathf.RoundToInt((score * GetOut.GameParams.Instance.gameParameter.comboVar+scoreBonus) * (1 + this.cardBonus / 100));
			gameEndLogic.card_bonus = cardBonus;
			gameEndLogic.card_ids = CardRate.GetAdditionalCardID (2,idGame);
			gameEndLogic.m_game_id = idGame;
			gameEndLogic.play_time = (int)(totalTime/60);
			gameEndLogic.item_get_num = numberItemScore1+numberItemScore2+numberItemScore3;

			gameEndLogic.hide_area_show = hideAreaShow;
			gameEndLogic.complete = SendAPICompleteHandler;
			gameEndLogic.error = SendAPIErrorHandler;
			gameEndLogic.SendAPI ();

		}

		void SendAPICompleteHandler ()
		{
//			Debug.Log ("SendAPICompleteHandler!" + UpdateInformation.GetInstance.game_list[1].score);
//			Debug.Log ("SendAPICompleteHandler!" +UpdateInformation.GetInstance.game_list[1].rank);
//			Debug.Log ("SendAPICompleteHandler!" + Player.GetInstance.lv);
//			Debug.Log ("SendAPICompleteHandler!" + Player.GetInstance.exp);
			GameResaultManager.Instance.SetGameResultInformation((int)(score*GetOut.GameParams.Instance.gameParameter.comboVar),scoreBonus,0,APIInformation.GetInstance.rank,cardBonus, false, false, true);
		}

		void SendAPIErrorHandler (string str)
		{
//			Debug.Log ("SendAPIErrorHandler! " + str);
			GameResaultManager.Instance.SetGameResultInformation((int)(score*GetOut.GameParams.Instance.gameParameter.comboVar),scoreBonus,0,APIInformation.GetInstance.rank,cardBonus);
		}




		public void InitMap()
		{
			InitParameterOfGame();
			if(Header.Instance.popupCountDown)
				Header.Instance.popupCountDown.showOrHideBg += PlayMusic;
			GameResaultManager.Instance.SetImageHeaderPanelResault(2);
			LoadMap.ScreenSize = Camera.main.ScreenToWorldPoint (new Vector3 (Camera.main.pixelWidth, Camera.main.pixelHeight, 0));
			float delta = -0.5f;
			float ratio = GameConfig.SCREEN_TARGET_WIDTH / GameConfig.SCREEN_TARGET_HEIGHT;

			if (Mathf.Abs ((float)Screen.width / (float)Screen.height - ratio) > 0.01f) {
				delta += Camera.main.rect.x * Camera.main.orthographicSize * (((float)Screen.width / (float)Screen.height) / ratio);
				//Debug.Log (delta);
			}
			LoadMap.ScreenSize.x += delta;
			float PositionYCamera = LoadMap.ScreenSize.y - (Map.WIDTH/2) * Map.SizeBlock;
			Screen.sleepTimeout = SleepTimeout.NeverSleep;
			totalTime = 0f;
			Header.Instance.SetLife(LifeType.Number,life);
			Header.Instance.SetShowNokori(true);
			Header.Instance.UpdateLife(life);
			GameFooter.Instance().Init();
			map.Init();														//init map
			cameraControl.enabled = true;
			GameResaultManager.Instance.SetLastLevel(UpdateInformation.GetInstance.player.lv,UpdateInformation.GetInstance.player.exp);
		}


		public void Init()
		{
			Header.Instance.SetOnPauseCallback(GamePause);
			PauseManager.s_Instance.SetOnBackToGameCallback(UnPause);
			PauseManager.s_Instance.SetOnExitCallback(ExitGame);
			isOpening = false;
			SetParameterForEnemy();
			player.Init();														//init player
			InitFourTsu();														//init family tsu
			if(GameConfig.ITEM_ON)
				InitItem();														//init 8 item
			idOfShounosuke = GetOut.GameParams.Instance.GetIdOfShounosuke();
			totalItemAppearPercent = percentageAppearStar+percentageAppearScore1+percentageAppearClock+percentageAppearScore2+percentageAppearScore3;
		}


		void InitParameterOfGame()
		{
			if(GameParams.Instance == null)
			{
				Debug.Log("null");
				return;
			}
			GameConfig.PLAYER_SPEED_RATE = GetOut.GameParams.Instance.gameParameter.playerSpeedRate;
			life = GetOut.GameParams.Instance.gameParameter.playerLifes;
			GameConfig.SPEED_CLOCK = GetOut.GameParams.Instance.gameParameter.timerSpeedRate;
			GameConfig.TIME_DOOR_CLOSE = GetOut.GameParams.Instance.gameParameter.doorTime;
			GameConfig.TIME_CLOCK = GetOut.GameParams.Instance.GetEffectValueItem(TYPEITEM.Clock);
			GameConfig.TIME_STAR = GetOut.GameParams.Instance.GetEffectValueItem(TYPEITEM.Star);
			GameConfig.TIME_INIT_ITEM = GetOut.GameParams.Instance.gameParameter.itemSpanSecond;
			GameConfig.ITEM_ON = GetOut.GameParams.Instance.gameParameter.itemOn;
			GameConfig.FLICK_SENSITIVITY = GameParams.Instance.gameParameter.flickSensitivity;
			percentageAppearStar = GetOut.GameParams.Instance.GetApearProbalityItem(TYPEITEM.Star);
			percentageAppearScore3 = GetOut.GameParams.Instance.GetApearProbalityItem(TYPEITEM.Score3);
			percentageAppearScore2 = GetOut.GameParams.Instance.GetApearProbalityItem(TYPEITEM.Score2);
			percentageAppearScore1 = GetOut.GameParams.Instance.GetApearProbalityItem(TYPEITEM.Score1);
			percentageAppearClock = GetOut.GameParams.Instance.GetApearProbalityItem(TYPEITEM.Clock);
		}
		void Update()
		{
			if(GameCountDownMediator.didEndCountDown && isOpening)
			{
				isOpening = false;
				Init();
			}
			// Update Gate
			if(isOpening || isPause || isGameOver)
				return;
			if(!isGateRightOpen)
			{
				if(timeGateRightClose == GameConfig.TIME_DOOR_CLOSE)
				{
					map.ActiveDoor(map.objDoorRight);
					timeGateRightClose -= Time.deltaTime;
				}else
					if(timeGateRightClose > 0)
					{
						timeGateRightClose -= Time.deltaTime;
					}else
					{
						isGateRightOpen = true;
						map.UnRenderDoor(map.objDoorRight);
					}
			}

			if(!isGateLeftOpen)
			{
				if(timeGateLeftClose == GameConfig.TIME_DOOR_CLOSE)
				{
					map.ActiveDoor(map.objDoorLeft);
					timeGateLeftClose -= Time.deltaTime;
				}else
					if(timeGateLeftClose > 0)
					{
						timeGateLeftClose -= Time.deltaTime;
					}else
					{
						isGateLeftOpen = true;
						map.UnRenderDoor(map.objDoorLeft);
					}
			}

			//
			totalTime += Time.deltaTime;
			if(numberEnemyActive < 10)
				InitEnemy();											
			Header.Instance.SetScore(CountTime(totalTime));

			//check when it get the item star
			if(isStar)
			{
				if(timeStar <= 0)
				{
					GameFooter.Instance().SetAnimEffectStarItem(false);
					isStar = false;
					SetParticleEffectStar(false);
					if(ComponentConstant.SOUND_MANAGER != null)
					{
						ComponentConstant.SOUND_MANAGER.StopBGM();
						ComponentConstant.SOUND_MANAGER.Play(SoundEnum.bgm04_escape,GetAudioSource);
					}
				}else
				{
					timeStar -= Time.deltaTime;
					if(timeStar > timeClock)
						GameFooter.Instance().SetSliderEffectValue(timeStar,GetOut.GameParams.Instance.GetEffectValueItem(TYPEITEM.Star));
				}
			}

			//check when it get the item clock
			if(isClock)
			{
				if(timeClock <= 0)
				{
					isClock = false;
					SetUnClockSpeed();
					GameFooter.Instance().SetAnimEffectClockItem(false);
					SetClockEffect(false);
				}
				else
				{
					timeClock -= Time.deltaTime;
					if(timeClock > timeStar)
					{
						GameFooter.Instance().SetSliderEffectValue(timeClock,GetOut.GameParams.Instance.GetEffectValueItem(TYPEITEM.Star));
					}
				}
			}

			//if the item have been get or the number of item is active at the scene less than 8 init an item
			if(timeInitItem > 0)
			{
				timeInitItem -= Time.deltaTime;
				isInitItem = true;
			}else
			{
				timeInitItem = GameConfig.TIME_INIT_ITEM;
				if(isInitItem && numberItemActive < 8 && GameConfig.ITEM_ON)
				{
					RandomItem();
					isInitItem = true;
					timeInitItem = GameConfig.TIME_INIT_ITEM;
					numberItemActive++;
				}
					
			}

		}


		public void ExitGame()
		{
			
		}


		//instance
		public static GameManager Instance()
		{
			return _instance;
		}

		//GamePause
		public void GamePause()
		{
			PauseManager.s_Instance.SetVisible(true);
			isPause = true;
			Debug.Log("Pause music");
//			Time.timeScale = 0;
			StopAllAnimationEnemy();
			if(isClock)
				audioSourceBackground.pitch = 1;
			GameFooter.Instance().PauseParticle();
			player.PlayAnimation(true);
		}

		//Resum
		public void UnPause(){
			isPause = false;
			if(isClock)
				SetClockEffect(true);
			GameFooter.Instance().UnPauseParticle();
		}

		//GameOver
		public void GameOver()
		{
			isGameOver = true;
//			Time.timeScale = 0;
			score += (int)totalTime;
			GameFooter.Instance().PauseParticle();
			SendGameEndingAPI();
		}

		//replay
		public void UnGameOver()
		{
			isGameOver = false;
			Time.timeScale = 1;
		}


		public void SetParticleEffectStar(bool state)
		{
			particleStarEffect.SetActive(state);
		}
		//return string total time
		public string CountTime(float time)
		{
			int hour = 0;
			int minute = 0;
			int sec = 0;
			string totalTime;
			hour = (int)time/3600;
			minute = (int)((time%3600) / 60);
			sec = (int)(time % 60);
			totalTime = string.Format("{0:00}:{1:00}:{2:00}", hour, minute,sec);
			return totalTime;
		}

		//init enemy when the total time = it's time appear
		public void InitEnemy()
		{
			for(int i = 0; i < lstEnemy.Count; i++)
			{
				if(lstEnemy[i].timeAppear < totalTime)
				{
					if(!lstEnemy[i].gameObject.activeSelf)
					{
						lstEnemy[i].Init(RandomArea(4));
						lstEnemy[i].gameObject.SetActive(true);
						if(timeClock > 0)
						{
							lstEnemy[i].speed = GameConfig.SPEED_CLOCK;
						}
						numberEnemyActive ++;
					}
				}
			}

		}


		public void SetParameterForEnemy()
		{
			int indexEnemy = 1;
			EnemyData dataEnemy;
			LogicBase logicEnemy;
			for(int i = 0; i < lstFamilyTsuObj.Count; i++)
			{
				dataEnemy = GetOut.GameParams.Instance.GetEnemyData(indexEnemy);
				AddScriptForEnemy (lstFamilyTsuObj [i], dataEnemy.logicType);
				logicEnemy = lstFamilyTsuObj [i].GetComponent<LogicBase> ();
				logicEnemy.speed = dataEnemy.speedBasic;
				logicEnemy.basicSpeed = dataEnemy.speedBasic;
				logicEnemy.percentLogic = dataEnemy.logicValue;
				logicEnemy.timeAppear = dataEnemy.timeApear;
				logicEnemy.iconFooter = lstEnemyIconFooter [indexEnemy-1];
				logicEnemy.idEnemy = dataEnemy.index;
				lstFamilyTsu.Add (lstFamilyTsuObj [i].GetComponent<LogicBase> ());
				indexEnemy++;
			}
			for(int i = 0; i < lstEnemyObj.Count; i++)
			{
				dataEnemy = GetOut.GameParams.Instance.GetEnemyData(indexEnemy);
				AddScriptForEnemy (lstEnemyObj [i], dataEnemy.logicType);
				logicEnemy = lstEnemyObj [i].GetComponent<LogicBase> ();
				logicEnemy.idEnemy = dataEnemy.index;
				logicEnemy.speed = dataEnemy.speedBasic;
				logicEnemy.basicSpeed = dataEnemy.speedBasic;
				logicEnemy.percentLogic = dataEnemy.logicValue;
				logicEnemy.timeAppear = dataEnemy.timeApear;
				logicEnemy.iconFooter = lstEnemyIconFooter [indexEnemy-1];
				lstEnemy.Add ( lstEnemyObj [i].GetComponent<LogicBase> ());
				indexEnemy++;
			}

		}

		public void AddScriptForEnemy(GameObject enemy, int logicType)
		{
			switch ((LogicType)logicType) {
			case LogicType.LogicRandom:
				enemy.AddComponent<LogicType_Random> ();
				break;
			case LogicType.LogicFollow:
				enemy.AddComponent<LogicType_Follow> ();
				break;
			case LogicType.LogicGoStraight:
				enemy.AddComponent < LogicType_GoStraight> ();
				break;
			case LogicType.LogicFollowWall:
				enemy.AddComponent<LogicType_FollowWall> ();
				break;
			}
		}


		//init 4 person in tsu's family in the first play
		public void InitFourTsu()
		{
			List<LogicBase> lstFamilyclone = new List<LogicBase>();

			while(lstFamilyclone.Count < 4)
			{
				lstFamilyclone.Add(lstFamilyTsu[Random.Range(0,lstFamilyTsu.Count)]);
				lstFamilyTsu.Remove(lstFamilyclone[lstFamilyclone.Count-1]);
			}
			lstFamilyTsu = lstFamilyclone;
			for (int i = 0; i < lstFamilyTsu.Count; i++) {
				if(i < 2)
				{
					lstFamilyclone[i].Init(LoadMap.map.arrayAreaA);
					lstFamilyclone[i].gameObject.SetActive(true);
					numberEnemyActive ++;
				}else
					if(i < (lstFamilyclone.Count))
					{
						lstFamilyclone[i].Init(LoadMap.map.arrayAreaD);
						lstFamilyclone[i].gameObject.SetActive(true);
						numberEnemyActive ++;
					}
			}
		}

		//random in max area
		public List<Vector2> RandomArea(int max)
		{
			int i = Random.Range(0,max);
			switch(i)
			{
				case 0:
					return LoadMap.map.arrayAreaA;
				case 1:
					return LoadMap.map.arrayAreaB;
				case 2:
					return LoadMap.map.arrayAreaD;
				case 3:
					return LoadMap.map.arrayAreaC;
			}
			return null;
		}

		//function init 8 item in the first play
		public void InitItem()
		{
			List<Item> lstItemClone = new List<Item>();
			int random;
			int count = 0;

			for(int i = 0; i < lstItem.Count; i++)
			{
				lstItem[i].numberScore = GetOut.GameParams.Instance.GetEffectValueItem(lstItem[i].type);
				lstItem[i].appearProbality = GetOut.GameParams.Instance.GetApearProbalityItem(lstItem[i].type);
				lstItem[i].reductionAppearPercent = GetOut.GameParams.Instance.GetReductionAppearPercentItem(lstItem[i].type);
			}

			for(int i = 0; i < 8; i++)
			{
				lstItemClone.Add(lstItem[i]);

			}

			while(count < 8) {
				random = Random.Range(0,lstItemClone.Count);
				if(count < 2)
				{
					lstItemClone[random].SetArea(LoadMap.map.arrayAreaA);
					lstItemClone.Remove(lstItemClone[random]);
				}else
					if(count < 4)
					{
						lstItemClone[random].SetArea(LoadMap.map.arrayAreaB);
						lstItemClone.Remove(lstItemClone[random]);
					}
					else
						if(count<6)
						{
							lstItemClone[random].SetArea(LoadMap.map.arrayAreaC);
							lstItemClone.Remove(lstItemClone[random]);
						}else
						{
							lstItemClone[random].SetArea(LoadMap.map.arrayAreaD);
							lstItemClone.Remove(lstItemClone[random]);
						}
				count++;
			}
			numberItemActive = count;

		}

		//Set speed of all enemy when the player get a item clock
		public void SetClockSpeed(){
			for(int i = 0; i < lstEnemy.Count; i++)
			{
				if(lstEnemy[i].idEnemy != idOfShounosuke)
					lstEnemy[i].speed = GameConfig.SPEED_CLOCK;
			}

			for(int i = 0; i < lstFamilyTsu.Count; i++)
			{
				lstFamilyTsu[i].speed = GameConfig.SPEED_CLOCK;
			}
		}

		//set default speed of all enemy when the time clock speed is less than 0
		public void SetUnClockSpeed(){
			for(int i = 0; i < lstEnemy.Count; i++)
			{
				lstEnemy[i].speed = lstEnemy[i].basicSpeed;
				if(lstEnemy[i].gameObject.activeSelf )
				{
					lstEnemy[i].FlickerClock(true);
				}
			}

			for(int i = 0; i < lstFamilyTsu.Count; i++)
			{
				lstFamilyTsu[i].speed = lstFamilyTsu[i].basicSpeed;
				lstFamilyTsu[i].FlickerClock(true);
			}
		}

		//Random an item
		public void RandomItem()
		{
			if(numberItemActive < 8)
			{
				int starRate = (int)(percentageAppearStar/totalItemAppearPercent *100);
				int clockRate = (int) (percentageAppearClock/totalItemAppearPercent * 100);
				int scoreRate1 = (int) (percentageAppearScore1/totalItemAppearPercent * 100);
				int scoreRate2 = (int) (percentageAppearScore2/totalItemAppearPercent * 100);
				int random = Random.Range(0,100);
				if(isStar)
					starRate = 0;
				if(isClock)
					clockRate = 0;
				if(random < starRate)
				{
					if(FindItem(TYPEITEM.Star) != null)
						FindItem(TYPEITEM.Star).SetArea(RandomArea(4));
				}else
					if(random >= starRate && random < (starRate+clockRate))
					{
						if(FindItem(TYPEITEM.Clock) != null )
							FindItem(TYPEITEM.Clock).SetArea(RandomArea(4));
					}
					else
						if(random >=(starRate+clockRate) && random < (starRate+clockRate+scoreRate1))
						{
							FindItem(TYPEITEM.Score1).SetArea(RandomArea(4));
						}
						else
							if(random >= (starRate+clockRate+scoreRate1) && random < (starRate+clockRate+scoreRate1+scoreRate2))
							{
								FindItem(TYPEITEM.Score2).SetArea(RandomArea(4));
							}
							else
							{
								FindItem(TYPEITEM.Score3).SetArea(RandomArea(4));
							}
			}
		}

		//Find item in lstItem
		private Item FindItem(TYPEITEM type)
		{
			for(int i = 0; i < lstItem.Count; i++)
			{
				if(lstItem[i].type == type && !lstItem[i].gameObject.activeSelf)
				{
					return lstItem[i];
				}
			}
			return null;
		}

		public void SetClockEffect(bool isEffect)
		{
			if(audioSourceBackground != null)
				if(isEffect)
				{
					audioSourceBackground.pitch = 0.5f;
				}
				else
					audioSourceBackground.pitch = 1f;
		}


		public void StopAllAnimationEnemy()
		{
			for(int i = 0; i < lstEnemy.Count; i++)
			{
				if(lstEnemy[i].gameObject.activeSelf)
					lstEnemy[i].SetIdleAnimation();
			}
			for(int i = 0; i < lstFamilyTsu.Count; i++)
			{
				lstFamilyTsu[i].SetIdleAnimation();
			}
		}

		void PlayMusic(bool value = false)
		{
			if (ComponentConstant.SOUND_MANAGER != null)
			{
				ComponentConstant.SOUND_MANAGER.Play(SoundEnum.bgm04_escape, GetAudioSource);
			}
		}
	}
}
