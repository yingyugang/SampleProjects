using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


namespace Swimming
{
	public class GameManager : MonoBehaviour 
	{
		public ScrollingScript scrolling;
		public GameEndLogic gameEndLogic;
		public Button buttonPause;
		public GameObject swimmer;
		public GameObject swimmer_cheat;
		public Sprite cheatCutInSprite;
		public Image cutInImage;
		[HideInInspector]
		public bool isPaused = false;

		private List<GameObject> m_Enemies = new List<GameObject>();
		private List<GameObject> m_Items = new List<GameObject>();

		private TimeType m_CurrentTime;

		//private bool m_ShowOpening = false;
		private bool m_GameStart = false;

		private AudioSource m_BgSource;
		float cardBonus;
		private static GameManager m_Instance;
		public static GameManager Instance
		{
			get
			{
				return m_Instance;
			}
		}

		void Awake()
		{
			m_Instance = this;
			cardBonus = GetCardBonus ();
			//cheat begin
			if(!CheatController.IsCheated (0)){
				if (swimmer != null)
					swimmer.SetActive (true);
				if (swimmer_cheat != null) {
					swimmer_cheat.SetActive (false);
				}
			} else {
				if (swimmer != null)
					swimmer.SetActive (false);
				if (swimmer_cheat != null) {
					swimmer_cheat.SetActive (true);
					if(cheatCutInSprite!=null && cutInImage!=null)
						cutInImage.sprite = cheatCutInSprite;
				}
			}
			//cheat end
		}

		float GetCardBonus(){
			//CardRate.GetTotal (2,SwimmingConfig.GAME_ID);
			return CardRate.GetTotal (2,SwimmingConfig.GAME_ID);
		}

		// Use this for initialization
		void Start () 
		{
			Init();
		}

		void OnEnable()
		{

		}

		public void SendGameEndingAPI ()
		{
			gameEndLogic.m_game_id = SwimmingConfig.GAME_ID;
			gameEndLogic.score = Mathf.RoundToInt(Swimmer.Instance.GetTotalScore() * (1 + this.cardBonus / 100));
			gameEndLogic.swim_metre = (int) Swimmer.Instance.GetDistance();
			gameEndLogic.item_get_num = Swimmer.Instance.GetTotalItem();
			gameEndLogic.all_muster = (EnemySpawner.Instance.GetHelperCircle() > 1) ? 1 : 0;
			gameEndLogic.over_2weeks = (EnemySpawner.Instance.GetHelperCircle() > 2) ? 1 : 0;
			gameEndLogic.card_bonus = this.cardBonus;
			gameEndLogic.card_ids = CardRate.GetAdditionalCardID (2,SwimmingConfig.GAME_ID);
			gameEndLogic.complete = SendAPICompleteHandler;
			gameEndLogic.error = SendAPIErrorHandler;
			gameEndLogic.SendAPI ();
		}

		void SendAPICompleteHandler ()
		{
			//Debug.Log ("SendAPICompleteHandler!" + UpdateInformation.GetInstance.game_list[2].score);
			//Debug.Log ("SendAPICompleteHandler!" + APIInformation.GetInstance.rank);
			//Debug.Log ("SendAPICompleteHandler!" + Player.GetInstance.lv);
			Debug.Log ("SendAPICompleteHandler!" + Player.GetInstance.exp);

			DisplayGameover();
		}

		void SendAPIErrorHandler (string str)
		{
			Debug.Log ("SendAPIErrorHandler! " + str);

			DisplayGameover();
		}

		void DisplayGameover()
		{
			int score = (int) ( Swimmer.Instance.GetDistance() * GameParams.Instance.comboVar);
			int scoreBonus = Swimmer.Instance.GetScoreBonus();
			int scoreBonusMax = 0;

			//ComponentConstant.SOUND_MANAGER.StopBGM();

			GameResaultManager.Instance.SetGameResultInformation(
				score, 
				scoreBonus, 
				scoreBonusMax, 
				APIInformation.GetInstance.rank,cardBonus, false, false, APIInformation.GetInstance.rank > 0
            );

			GameResaultManager.Instance.SetUser(
				UpdateInformation.GetInstance.player.name,
				GameConstant.headerSprite,
				false);
		}

		public void Init()
		{
			Screen.sleepTimeout = SleepTimeout.NeverSleep;

			scrolling.speed = new Vector2(0, GameParams.Instance.GetRelativelyRiverSpeed());

			GUIManager.Instance.Init();
			GameFooter.Instance.Init();
			EnemySpawner.Instance.Init();

			m_Enemies.Clear();
			m_Items.Clear();

			m_CurrentTime = TimeType.Morning;
			ScrollingScript.Instance.SetSpriteTime(TimeType.Morning);

			// Header
			Header.Instance.SetScore("0  m");
			Header.Instance.SetOnPauseCallback(BtnPause);
			Header.Instance.SetShowNokori(true);
			Header.Instance.popupCountDown.showOrHideBg += PlayBgMusic;

			// Pause
			//PauseManager.s_Instance.Init(SwimmingConfig.GAME_ID, m_BgSource);
			PauseManager.s_Instance.SetOnBackToGameCallback(OnResume);

			GameResaultManager.Instance.SetImageHeaderPanelResault(SwimmingConfig.GAME_ID);
			GameResaultManager.Instance.SetLastLevel(UpdateInformation.GetInstance.player.lv,UpdateInformation.GetInstance.player.exp);
			m_GameStart = false;
			buttonPause.interactable = false;
			ShowOpening();
			PreloadSound();
		}

		void PreloadSound()
		{
			ComponentConstant.SOUND_MANAGER.StoreSounds (new List<SoundEnum> () {
				SoundEnum.bgm04_escape,
				SoundEnum.bgm10_invicible,
			});
		}

		public void GetAudioSource(AudioSource source)
		{
			m_BgSource = source;
			PauseManager.s_Instance.Init(SwimmingConfig.GAME_ID, source);
		}

		public void ShowOpening()
		{
			isPaused = true;
			//GUIManager.Instance.ShowOpening();
			GameCountDownMediator.didEndCountDown = false;
		}

		public void CloseOpening()
		{
			if(ComponentConstant.POPUP_LOADER != null){
				ComponentConstant.POPUP_LOADER.Popup(PopupEnum.GameCountDown);
			}
		}

		public void SetCurrentTime(TimeType timeType)
		{
			if (m_CurrentTime != timeType)
			{
				m_CurrentTime = timeType;
				ScrollingScript.Instance.SetSpriteTime(timeType);
			}
		}

		public TimeType GetTimeType()
		{
			return m_CurrentTime;
		}
		
		// Update is called once per frame
		void Update () 
		{
			if (GameCountDownMediator.didEndCountDown && !m_GameStart)
			{
				OnResume();
				m_GameStart = true;
				buttonPause.interactable = true;
				//PlayBgMusic();
			}

			UpdateBackgroundSpeed();
		}

		void PlayBgMusic(bool value = false)
		{
			if (ComponentConstant.SOUND_MANAGER != null)
			{
				ComponentConstant.SOUND_MANAGER.Play(SoundEnum.bgm05_river, (audio) =>
					{
						m_BgSource = audio;
						if (m_BgSource != null)
						{
							PauseManager.s_Instance.Init(SwimmingConfig.GAME_ID, m_BgSource);
						}
					});
			}
		}

		void UpdateBackgroundSpeed()
		{
			if (Swimmer.Instance.IsImmortal())
				return;
			
			float speed = GameParams.Instance.GetRelativelyRiverSpeed();
			speed *= GameManager.Instance.GetSpeedUpRatio();
			scrolling.speed = new Vector2(0, speed);
		}

		public float GetSpeedUpRatio()
		{
			int i = (int) (Swimmer.Instance.GetDistance() / GameParams.Instance.speedUpMetters);
			float percentage = GameParams.Instance.speedUpPercentage / 100f;
			float ratio = Mathf.Pow(1+percentage, i);

			float maxSpeed = 5f;
			float realSpeed = ratio * GameParams.Instance.playerSpeed;
			if (realSpeed > maxSpeed)
				ratio = maxSpeed / GameParams.Instance.playerSpeed;

			return ratio;
		}

		public void BtnPause()
		{
			if (!GameCountDownMediator.didEndCountDown && !m_GameStart)
				return;
			
			PauseManager.s_Instance.SetVisible(true);
			OnPause();
		}

		public void OnPause()
		{
			PauseGame();
		}

		public void OnResume()
		{
			ResumeGame();
		}

		public bool IsPause()
		{
			return isPaused;
		}

		void PauseGame()
		{
			//ComponentConstant.SOUND_MANAGER.StopBGM();
			Swimmer.Instance.Idle();
			isPaused = true;
			//Time.timeScale = 0;
			GameFooter.Instance.SetParticleVisible(false);
		}

		void PauseGameWithoutTimeScale()
		{
			//ComponentConstant.SOUND_MANAGER.StopBGM();
			Swimmer.Instance.Idle();
			isPaused = true;
		}

		void ResumeGame()
		{
			isPaused = false;
			Time.timeScale = 1;
			Swimmer.Instance.Play();
			GameFooter.Instance.SetParticleVisible(true);
		}

		void PlayTimeupSound()
		{
			ComponentConstant.SOUND_MANAGER.Play(SoundEnum.se07_timeup);
		}

		public void PreGameOver()
		{
			Invoke("PlayTimeupSound", 0.2f);
			ComponentConstant.SOUND_MANAGER.StopBGM();
			ComponentConstant.SOUND_MANAGER.Play(SoundEnum.bgm15_title_back);
			PauseGame();
			GUIManager.Instance.ShowEnding();
		}

		public void GameOver()
		{
			GUIManager.Instance.ShowGameOver();
			//SendGameEndingAPI();
		}

		public void StartImmortalMode()
		{
			Debug.Log("StartImmortalMode");

			//SoundManager.Instance.PlayMusic(SoundType.BgImmortal);
			//ComponentConstant.SOUND_MANAGER.Play(SoundEnum.bgm10_invicible);
			scrolling.speed *= SwimmingConfig.IMMORTAL_FACTOR;
			IncreaseSpeed();
			//float timeImmortal = GameParams.Instance.GetItemDataByType(ItemType.Immortal).effectValue;
			//Invoke("StopImmortalMode", timeImmortal);
		}

		public void IncreaseSpeed()
		{
			foreach(var obj in m_Enemies)
			{
				obj.GetComponent<Enemy>().IncreaseSpeed();
			}

			foreach(var obj in m_Items)
			{
				obj.GetComponent<Item>().IncreaseSpeed();
			}

			EnemySpawner.Instance.helper.GetComponent<Helper>().IncreaseSpeed();
		}

		public void DecreaseSpeed()
		{
			foreach(var obj in m_Enemies)
			{
				if (obj != null)
					obj.GetComponent<Enemy>().DecreaseSpeed();
			}

			foreach(var obj in m_Items)
			{
				obj.GetComponent<Item>().DecreaseSpeed();
			}

			EnemySpawner.Instance.helper.GetComponent<Helper>().DecreaseSpeed();
		}

		public void StopImmortalMode()
		{
			scrolling.speed = new Vector2(0, SwimmingConfig.BG_SPEED);
			DecreaseSpeed();
			Swimmer.Instance.StopImmortalMode();

			ComponentConstant.SOUND_MANAGER.Play(SoundEnum.bgm05_river, GameManager.Instance.GetAudioSource);
		}

		public void AddEnemy(GameObject enemy)
		{
			m_Enemies.Add(enemy);
		}

		public void RemoveEnemy(GameObject enemy)
		{
			EnemyType type = enemy.GetComponent<Enemy>().type;
			EnemySpawner.Instance.spawnObstacleCounter[type] --;
			if (EnemySpawner.Instance.spawnObstacleCounter[type] < 0)
				EnemySpawner.Instance.spawnObstacleCounter[type] = 0;

			EnemySpawner.Instance.Despawn(enemy);
			m_Enemies.Remove(enemy);
		}

		public void AddItem(GameObject item)
		{
			m_Items.Add(item);
		}

		public void RemoveItem(GameObject item)
		{
			EnemySpawner.Instance.Despawn(item);
			m_Items.Remove(item);
		}
	}
}