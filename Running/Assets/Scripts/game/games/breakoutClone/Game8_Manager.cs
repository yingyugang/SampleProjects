using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using DG.Tweening;

public enum GAMESTATE
{
	START,
	PLAYING,
	PAUSE,
	END
}

public class Game8_Manager : MonoBehaviour
{
	// game variavles

	public GameEndLogic gameEndLogic;
	public Item_Manager ItemManager;
	public RacketController Racket;
	public RacketController RacketCheat;
	public BallManager Balls;
	public Block_Manager BlockManager;
	public GateStage GateStage;
	public Game8_LoadData LoadData;
	public OdenChangeBackground ChangeBackground;
	public int Stage;
	public int Player_Lifes;
	public float Total_Combo;
	[HideInInspector]
	public BallController BallStart;
	private bool is_Start;
	private int m_ScoreGame;
	private int m_StageLoad;
	private int m_CheckBackground;
	// check boss num change background
	// Player number lifes
	public int BlockKey;
	public bool isRegularly;
	// number block key on stage
	//Item Score
	public int ItemScoreTree;
	public int ItemScoreOden;
	public int ItemScoreRice;
	public int CountScoreItem;
	// number block key
	public int ScoreBreak;
	private int m_StageBounus;
	private int m_BossBreak;
	// Parameter
	public float Stage_Bonus_Rate;
	public float Boss_Bonus_Rate;
	public int Defeat_Boss_Num;
	// number block breaking
	public bool StartBall;
	public int DamageBoss;
	private int m_DamageNomal;
	private AudioSource m_AudioSource;
	// Speed Player
	public float PlayerSpeed;
	// speed ball up when collison block
	public float m_SpeedBallUpStage;

	private float m_OriginBgVolume;
	// origin background's volume
	private const float VOLUME_BACKGROUND_SOUND = 0.9f;

	public GAMESTATE GameState;
	float cardBonus;
	private static Game8_Manager _instance;

	public static Game8_Manager instance {
		get {
			if (_instance == null)
				_instance = GameObject.FindObjectOfType<Game8_Manager> ();
			return _instance;
		}
	}

	void Awake ()
	{
		_instance = this;
		Input.multiTouchEnabled = false;
		Physics2D.velocityThreshold = BreackoutConfig.VELOCITY;
		Time.fixedDeltaTime = BreackoutConfig.FIXEDTIME;
		if (ComponentConstant.SOUND_MANAGER != null) {
			ComponentConstant.SOUND_MANAGER.StoreSounds (new List<SoundEnum> () {
				SoundEnum.bgm06_oden,
				SoundEnum.bgm07_odenboss,
				SoundEnum.bgm15_title_back
			});
		}
	}

	void Start ()
	{
		//InitUI ();
		Init ();
		UpdateLifeScore ();
		SetFooterUi ();
		cardBonus = GetCardBonus ();
	}

	float GetCardBonus(){
		//CardRate.GetTotal (2,GAME_ID);
		return CardRate.GetTotal (2,BreackoutConfig.GAME_ID);
	}

	void Init ()
	{

		is_Start = true;
		Defeat_Boss_Num = 0;
		m_CheckBackground = Defeat_Boss_Num;
		isRegularly = false;
		ItemScoreOden = 0;
		ItemScoreRice = 0;
		ItemScoreTree = 0;
		m_DamageNomal = 100;
		ScoreBreak = 0;
		CountScoreItem = 0;
		LoadData.LoadStage (Stage, BlockManager.CheckBossStage (Stage));
		GameState = GAMESTATE.START;
		BallStart = Balls.ListBall [0];
		BallStart.StartNewLife (0f);
		// Header
		//if (Header.Instance != null) {
			Header.Instance.SetShowNokori (true);
			Header.Instance.SetLife (LifeType.Number, Player_Lifes);
			if (Header.Instance.popupCountDown)
			Header.Instance.popupCountDown.showOrHideBg += InitBackGroundSound; // Playing sound when ending countdown.
			//Pause
			Header.Instance.SetOnPauseCallback (OnPauseGame);
		//}
		PauseManager.s_Instance.SetOnBackToGameCallback (OnResumeGame);
		//PauseManager.s_Instance.SetOnExitCallback (OnDisable);
		//
		GameResaultManager.Instance.SetImageHeaderPanelResault (BreackoutConfig.GAME_ID);
		if (UpdateInformation.GetInstance != null)
		GameResaultManager.Instance.SetLastLevel (UpdateInformation.GetInstance.player.lv,UpdateInformation.GetInstance.player.exp);
		//StartCoroutine(InitPreloadedSound());
	}

	//	IEnumerator InitPreloadedSound()
	//	{
	//		ComponentConstant.SOUND_MANAGER.StoreSounds(new List<SoundEnum>()
	//			{
	//				SoundEnum.bgm06_oden,
	//				SoundEnum.bgm07_odenboss,
	//				SoundEnum.bgm15_title_back
	//			}, InitUI);
	//		yield return null;
	//	}

	private void GetAudioSource (AudioSource audioSource)
	{
		this.m_AudioSource = audioSource;
		PauseManager.s_Instance.Init (BreackoutConfig.GAME_ID, m_AudioSource);

	}

	public void ResetVolume ()
	{
//		if (m_AudioSource != null) {
//			Debug.Log ("m_OriginBgVolume ResetVolume:" + m_OriginBgVolume);
//			m_AudioSource.volume = m_OriginBgVolume;
//		}
	}

	public void PlaySound (SoundEnum soundEnum)
	{
		if (ComponentConstant.SOUND_MANAGER != null) {
		ComponentConstant.SOUND_MANAGER.Play (soundEnum);
		}

	}


	void PlayGame ()
	{
		GameState = GAMESTATE.PLAYING;
		BallStart.AddForceBall ();
	}

	void InitBackGroundSound (bool value = false)
	{
		if (ComponentConstant.SOUND_MANAGER != null) {
			ComponentConstant.SOUND_MANAGER.Play (SoundEnum.bgm06_oden, (audio) => {
				
				m_AudioSource = audio;

//				Debug.Log ("audio callback: " + audio.volume);
//				m_OriginBgVolume = audio.volume;
//				Debug.Log ("m_OriginBgVolume: " + m_OriginBgVolume);
//				m_AudioSource.volume = VOLUME_BACKGROUND_SOUND;
				if (m_AudioSource != null) {
					PauseManager.s_Instance.Init (BreackoutConfig.GAME_ID, m_AudioSource);
				}
			}
			);
		}
	}

	void Update ()
	{
		UpdateLifeScore ();
		SetFooterUi ();

			if (GameState == GAMESTATE.START && StartBall == true) {
				print ("Mouse clicked, launch ball");
				PlayGame ();

		}
		if (GameState == GAMESTATE.PAUSE) {

			PauseAll ();
		} 
			
	}

	void PauseAll ()
	{
		GUI_8_Manager.Instance.Warning.GetComponent<Animator> ().speed = 0;
		DOTween.PauseAll ();
		ItemManager.PauseAllItem (true);
		Balls.PauseAllBall (true);
		Racket.GetComponent<Animator> ().speed = 0;
	}

	// Open gate when block key breack all
	public void LoseBlock ()
	{
		BlockKey--;
		// detect if no block left
		if (BlockKey <= 0) {
			PlaySound (SoundEnum.SE27_oden_fanfare);	
			GateStage.OpenGate ();
		}
	}


	//
	public void NewStage ()
	{
		ChangeBackGround ();
		//Reset Game stage
		GameState = GAMESTATE.START;
		Stage++;
		PlayerSpeed = PlayerSpeed + m_SpeedBallUpStage;
		GateStage.CloseGate ();
		ItemManager.ResetAllItem ();
		//Reset ball
		Balls.ResetAllBall ();
		if (BlockManager.CheckBossStage (Stage)) {
			StartCoroutine (DelayAction (2.0f, () => BallStart.StartNewLife (.0f)));
		} else {
			BallStart.StartNewLife (2.0f);
		}
		//Reset Racket
		Racket.gameObject.SetActive (false);
		StartCoroutine (DelayAction (2.0f, () => DelayShowRacket ()));
		BlockManager.ResetBlock ();
		ChangeStage ();
		CheckDamage ();
		StartBall = false;
		Debug.Log ("NiumBNB" + Defeat_Boss_Num);
	}

	void ChangeBackGround ()
	{
		if (m_CheckBackground != Defeat_Boss_Num) {
			ChangeBackground.ChangeBackGround ();
			if (ComponentConstant.SOUND_MANAGER != null) {
				ComponentConstant.SOUND_MANAGER.StopBGM ();
				ComponentConstant.SOUND_MANAGER.Play (SoundEnum.bgm06_oden, GetAudioSource); 
			}
		}
		m_CheckBackground = Defeat_Boss_Num;
	}


	// Check stage > 50 reset block map 41 ->
	void CheckStageMax (int stage)
	{
		if (stage > 50) {
			m_StageLoad = 40 + stage % 10;
			if (m_StageLoad == 40)
				m_StageLoad = 50;
		}

	}

	void DelayShowRacket ()
	{
		Racket.gameObject.SetActive (true);
		Racket.DelayRacketShow ();
	}

	public void CheckGameOver ()
	{
		Player_Lifes--;
		ItemManager.ResetAllItem ();
		if (Player_Lifes <= 0) {
			ResetPhysic ();
			Game8_Manager.instance.GameState = GAMESTATE.END;
			SetScoreGameOver ();
			ResetVolume ();
			Racket.Reset ();
			ComponentConstant.SOUND_MANAGER.StopBGM ();
			PlaySound (SoundEnum.bgm15_title_back);
			PlaySound (SoundEnum.se07_timeup);
//			Debug.Log ("Audio SOurce" + m_AudioSource.volume);
			GUI_8_Manager.Instance.OpenEnding ();
			Debug.Log ("GOV");
		} else {
			NewLife ();
		}
	}

	void NewLife ()
	{
		//ItemManager.ResetAllItem ();
		GameState = GAMESTATE.START;
		StartBall = false;
		BallStart.StartNewLife (0f);
		Racket.gameObject.SetActive (true);
		Racket.Reset ();

	}

	void ChangeStage ()
	{
		BlockKey = 0;

		m_StageLoad = Stage;
		CheckStageMax (Stage);
		LoadData.LoadStage (m_StageLoad, BlockManager.CheckBossStage (m_StageLoad));
		if (BlockManager.CheckBossStage (m_StageLoad)) {
			if (ComponentConstant.SOUND_MANAGER != null) {
				ComponentConstant.SOUND_MANAGER.StopBGM ();
				ComponentConstant.SOUND_MANAGER.Play (SoundEnum.bgm07_odenboss, GetAudioSource);
			}
			GUI_8_Manager.Instance.WarningBoss ();		
		} 
	}

	void SetScoreGameOver ()
	{
		ItemManager.CountPoint (ItemScoreTree, ItemScoreOden, ItemScoreRice);
		m_StageBounus = Mathf.RoundToInt ((Stage - 1) * Stage_Bonus_Rate + Defeat_Boss_Num * Boss_Bonus_Rate);
		m_ScoreGame = Mathf.RoundToInt(ScoreBreak*Total_Combo);
		Debug.Log ("Score" + m_ScoreGame + "block" + ScoreBreak + "item" + CountScoreItem);
		Debug.Log ("NumBoss Over" + Defeat_Boss_Num);
		Debug.Log ("Bonus" + m_StageBounus);
			
	}

	void CheckDamage ()
	{
		if (BlockManager.CheckBossStage (Stage)) {
			Balls.MAXDAMGE = DamageBoss;
		} else {
			Balls.MAXDAMGE = m_DamageNomal;
		}
	}


	void SetGameOver ()
	{
		BallController.s_Count = 0;
		Header.Instance.RemoveOnPauseCallback (OnPauseGame);
		PauseManager.s_Instance.RemoveOnBackToGameCallback (OnResumeGame);
		SendGameEndingAPI ();


	}


	public void GameOver ()
	{
		SetGameOver ();
	}

	private void OnPauseGame ()
	{
		//PauseManager.s_Instance.SetVisible(true);
		if (!GameCountDownMediator.didEndCountDown) {
			return;
		}

		if (Game8_Manager.instance.GameState == GAMESTATE.START) {
			is_Start = true;
		} else {
			is_Start = false;
		}
		Game8_Manager.instance.GameState = GAMESTATE.PAUSE;
		PauseManager.s_Instance.SetVisible (true);

	}
	//--<

	private void OnResumeGame ()
	{
		GUI_8_Manager.Instance.Warning.GetComponent<Animator> ().speed = 1;
		Racket.GetComponent<Animator> ().speed = 1;
		DOTween.PlayForwardAll ();
		if (is_Start == true) {
			Game8_Manager.instance.GameState = GAMESTATE.START;
			BallStart.GetComponent<Rigidbody2D> ().isKinematic = false;
		} else {
			Game8_Manager.instance.GameState = GAMESTATE.PLAYING;
			ItemManager.PauseAllItem (false);
			Balls.PauseAllBall (false);
		}

	}


	private void OnDisable ()
	{
		Header.Instance.onPause -= OnPauseGame;
		PauseManager.s_Instance.RemoveOnBackToGameCallback (OnResumeGame);
		BallController.s_Count = 0;
	
	}

	void ResetPhysic ()
	{
		Time.fixedDeltaTime = 0.02f;
		Input.multiTouchEnabled = true;
		Physics2D.velocityThreshold = 1.0f;

	}

	public void GoHome ()
	{
		ComponentConstant.SCENE_COMBINE_MANAGER.LoadScene (SceneEnum.Home);
	}

	void UpdateLifeScore ()
	{
		Header.Instance.UpdateLife (Player_Lifes);
		Header.Instance.SetScore (ScoreBreak.ToString ("") + "コ");
	}

	void SetFooterUi ()
	{
		Game8_Footer.Instance.SetStageFooter (Stage);
		Game8_Footer.Instance.SetNumberScore1Text (ItemScoreTree);
		Game8_Footer.Instance.SetNumberScore2Text (ItemScoreOden);
		Game8_Footer.Instance.SetNumberScore3Text (ItemScoreRice);
	}

	public void SendGameEndingAPI ()
	{
		gameEndLogic.m_game_id = BreackoutConfig.GAME_ID;
		gameEndLogic.stage_taget = Stage;
		gameEndLogic.defeat_boss_num = Defeat_Boss_Num;
		gameEndLogic.broken_block_num = ScoreBreak;
		gameEndLogic.item_get_num = ItemScoreTree + ItemScoreOden + ItemScoreRice;
		gameEndLogic.complete = SendAPICompleteHandler;
		gameEndLogic.error = SendAPIErrorHandler;
		gameEndLogic.score = Mathf.RoundToInt((m_ScoreGame + m_StageBounus + CountScoreItem) * (1 + this.cardBonus / 100));
		gameEndLogic.card_bonus = cardBonus;
		gameEndLogic.card_ids = CardRate.GetAdditionalCardID (2,BreackoutConfig.GAME_ID);
		gameEndLogic.SendAPI ();
	}

	void SendAPICompleteHandler ()
	{
		Debug.Log ("SendAPICompleteHandler!" + UpdateInformation.GetInstance.game_list [1].score);
		Debug.Log ("SendAPICompleteHandler!" + APIInformation.GetInstance.rank);
		Debug.Log ("SendAPICompleteHandler!" + Player.GetInstance.lv);
		Debug.Log ("SendAPICompleteHandler!" + Player.GetInstance.exp);
		Debug.Log ("SentPara");
		GameResaultManager.Instance.SetGameResultInformation (m_ScoreGame, CountScoreItem, m_StageBounus, APIInformation.GetInstance.rank,cardBonus, false, true, true);
	}

	void SendAPIErrorHandler (string str)
	{
		GameResaultManager.Instance.SetGameResultInformation(m_ScoreGame, CountScoreItem, m_StageBounus, APIInformation.GetInstance.rank,cardBonus, false, true, false);
        Debug.Log ("SendAPIErrorHandler! " + str);
	}

	public IEnumerator DelayAction (float dtime, System.Action callback)
	{
		float timeDelay = 0f;
		while (timeDelay < dtime) {
			if (Game8_Manager.instance.GameState != GAMESTATE.PAUSE) {
				timeDelay += Time.deltaTime;
			}
			yield return new WaitForEndOfFrame ();


		}
		yield return new WaitForEndOfFrame ();
		//		yield return new WaitForSeconds(dtime);
		callback ();
	}
}
