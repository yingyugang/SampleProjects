using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
//
//public enum SCREEN {
//	START = 0,
//	INTRO,
//	PLAYING,
//	END
//};

public enum SHEESTATE{
	START,
	PLAYING,
	PAUSE,
	END
}

public class Game10_Manager : MonoBehaviour {

	private static Game10_Manager _instance;
	public static Game10_Manager instance{
		get{
			if(_instance == null) _instance = GameObject.FindObjectOfType<Game10_Manager>();
			return _instance;
		}
	}

	public SHEESTATE GameState;

	//m_sye_practicing_parameter
	public int 		GameTime;	
	public int 		FeverTime;
	public float 	FlickSensitivity;
	public float 	CombSpan;
	[HideInInspector]public float	combvar1;
	[HideInInspector]public float	combvar2;
	[HideInInspector]public float	combvar3;

	public GameObject[] List_Screen;
	public Introduce 	Intro;
	public GamePlay		PlayScreen;

	public GameEndLogic gameEndLogic;

	private int m_Screen;
	private bool m_Openning = false;
	private AudioSource m_Audio;
	private float m_OriginBgVolume;
	// origin background's volume
	private const float VOLUME_BACKGROUND_SOUND = 0.9f;

	[HideInInspector]public int noMiss = 1;
	[HideInInspector]public int isFever = 0;
	[HideInInspector]public int isSyonosukeShow = 0;
	[HideInInspector]public bool isEnded = false;
	[HideInInspector]public bool isAllowTouch = false;

	float cardBonus;

	void Awake(){
		GameState = SHEESTATE.START;
		m_Screen = 0;
		PlayScreen.pauseBtn.GetComponent<Image>().raycastTarget = false;
		cardBonus = GetCardBonus();
	}

	float GetCardBonus(){
		//CardRate.GetTotal (2,SheeResources.GameInfo.ID);
		return CardRate.GetTotal (2,SheeResources.GameInfo.ID);
	}

	void Start(){
		Init();
		Skip_Introduce(); //close Openning
		GameResaultManager.Instance.SetImageHeaderPanelResault(SheeResources.GameInfo.ID);
		GameResaultManager.Instance.SetLastLevel(UpdateInformation.GetInstance.player.lv,UpdateInformation.GetInstance.player.exp);
	}

	void SendGameEndingAPI ()
	{
		//-->Set GameOver Params
		gameEndLogic.m_game_id = SheeResources.GameInfo.ID;

		gameEndLogic.score = Mathf.RoundToInt((PlayScreen.GetScore() * combvar1 + PlayScreen.totalCombo * combvar2 + PlayScreen.MAX_Combo * combvar3) * (1 + this.cardBonus / 100));
		gameEndLogic.card_bonus = cardBonus;
		gameEndLogic.card_ids = CardRate.GetAdditionalCardID (2,SheeResources.GameInfo.ID);
		gameEndLogic.combo_num = PlayScreen.MAX_Combo;// PlayScreen.totalCombo;
		gameEndLogic.over_sys_num = PlayScreen.GetScore();

		gameEndLogic.nomiss = noMiss;
		gameEndLogic.fever_rush = isFever;
		gameEndLogic.syonosuke_show = isSyonosukeShow;
		//--<

		//send api
		gameEndLogic.complete = SendAPICompleteHandler;
		gameEndLogic.error = SendAPIErrorHandler;
		gameEndLogic.SendAPI ();
	}

	void SendAPICompleteHandler ()
	{
		Debug.Log ("SendAPICompleteHandler!" + UpdateInformation.GetInstance.game_list[1].score);
		Debug.Log ("SendAPICompleteHandler!" + APIInformation.GetInstance.rank);
		Debug.Log ("SendAPICompleteHandler!" + Player.GetInstance.lv);
		Debug.Log ("SendAPICompleteHandler!" + Player.GetInstance.exp);
		int score = Mathf.RoundToInt(PlayScreen.GetScore() * combvar1);
		int totalCombo = Mathf.RoundToInt(PlayScreen.totalCombo * combvar2);
		int maxCombo = Mathf.RoundToInt(PlayScreen.MAX_Combo * combvar3);
//		int rank = APIInformation.GetInstance.rank;
//		if("".Equals(rank)) rank = GetComponent<Game10_LoadCSV>().GetRanking(PlayScreen.GetScore()) + "";
		GameResaultManager.Instance.SetGameResultInformation(score, totalCombo, maxCombo, APIInformation.GetInstance.rank,cardBonus,true, false, true);
	}

	void SendAPIErrorHandler (string str)
	{
        int score = Mathf.RoundToInt(PlayScreen.GetScore() * combvar1);
        int totalCombo = Mathf.RoundToInt(PlayScreen.totalCombo * combvar2);
        int maxCombo = Mathf.RoundToInt(PlayScreen.MAX_Combo * combvar3);
		GameResaultManager.Instance.SetGameResultInformation(score, totalCombo, maxCombo, APIInformation.GetInstance.rank,cardBonus, true);
        Debug.Log ("SendAPIErrorHandler! " + str);
	}

	void Update(){
//		Start_Game();
		if(GameState == SHEESTATE.START){
			PlayGame();
		}
		if(isEnded){
			EndGame();
		}

//		if (GameState == SHEESTATE.PAUSE || GameState == SHEESTATE.END) Time.timeScale = 0;
//		else Time.timeScale = 1;
	}

	void PlayGame(){
		if(PlayScreen.IsStartPlay){
//			if(Input.GetMouseButtonDown(0) || Input.touches.Length > 0)
			if(GameCountDownMediator.didEndCountDown)
			{
//				if(ComponentConstant.SOUND_MANAGER != null){
//					ComponentConstant.SOUND_MANAGER.Play(SoundEnum.bgm08_shee, GetAudioSource);
//				}
				Debug.Log("Play");
				PlayScreen.pauseBtn.GetComponent<Image>().raycastTarget = true;
				GameState = SHEESTATE.PLAYING;
				PlayScreen.Run();
				Header.Instance.ResumeTimer();
			}
		}
	}

	void PauseGame(){
		GameState = SHEESTATE.PAUSE;
		Header.Instance.PauseTimer();
		PauseManager.s_Instance.SetVisible(true);
		PlayScreen.FooterParticle.SetActive(false);
		if(PlayScreen.IsFever && PlayScreen.FeverParticle.activeInHierarchy){
			foreach(Transform t in PlayScreen.FeverParticle.transform){
				t.GetComponent<ParticleSystem>().Pause();
			}
		}
	}

	void ResumeGame(){
		GameState = SHEESTATE.PLAYING;
		Header.Instance.ResumeTimer();
		PlayScreen.FooterParticle.SetActive(true);
		if(PlayScreen.IsFever && PlayScreen.FeverParticle.activeInHierarchy){
			foreach(Transform t in PlayScreen.FeverParticle.transform){
				t.GetComponent<ParticleSystem>().Play();
			}
		}
	}

	void EndGame(){
		if(!isAllowTouch) return;
		if(Input.GetMouseButtonDown(0) || Input.touches.Length > 0)
		{
//			NextScreen();
			GameOver();
		}
	}



	void Init(){
		InitPreloadedSound();
		foreach(GameObject screen in List_Screen){
			screen.SetActive(false);
		}
		List_Screen[0].SetActive(true);

		Header.Instance.SetLife(LifeType.Time, GameTime);
		Header.Instance.PauseTimer();
		Header.Instance.SetOnPauseCallback(PauseGame);
		Header.Instance.popupCountDown.showOrHideBg += InitBackGroundSound; // Playing sound when ending countdown.
		PauseManager.s_Instance.Init(SheeResources.GameInfo.ID, m_Audio);
		PauseManager.s_Instance.SetOnBackToGameCallback(ResumeGame);
	} 

	public void PlaySound(SoundEnum soundEnum){
		if(ComponentConstant.SOUND_MANAGER != null){
			ComponentConstant.SOUND_MANAGER.Play(soundEnum);
		}

	}

	void InitPreloadedSound()
	{
		if (ComponentConstant.SOUND_MANAGER == null)
			return;
		ComponentConstant.SOUND_MANAGER.StoreSounds(new List<SoundEnum>()
			{
				SoundEnum.bgm08_shee,
				SoundEnum.bgm10_invicible,
				SoundEnum.bgm15_title_back
			}, null);
	}

	void InitBackGroundSound (bool value = false)
	{
		if (ComponentConstant.SOUND_MANAGER != null) 
		{
			ComponentConstant.SOUND_MANAGER.Play (SoundEnum.bgm08_shee, (audiosource) =>
				{
//					m_OriginBgVolume = audiosource.volume;
					this.m_Audio = audiosource;
//					this.m_Audio.volume = VOLUME_BACKGROUND_SOUND;
					PauseManager.s_Instance.Init (SheeResources.GameInfo.ID, m_Audio);
				}
			);
		}
	}

	public void ResetVolume ()
	{
//		if (m_Audio != null) {
//			this.m_Audio.volume = m_OriginBgVolume;
//		}
	}

	public void GetAudioSource(AudioSource audio){
		if(m_Audio == null || m_Audio != audio){
//			m_OriginBgVolume = audio.volume;
			this.m_Audio = audio;
//			this.m_Audio.volume = VOLUME_BACKGROUND_SOUND;
			PauseManager.s_Instance.Init(SheeResources.GameInfo.ID, m_Audio);
		}
	}

	public void NextScreen(){
		List_Screen[m_Screen].SetActive(false);
		if(m_Screen < List_Screen.Length - 1){
			List_Screen[++m_Screen].SetActive(true);
		}
	}
		
	public void Start_Game(){
		if(GameCountDownMediator.didEndCountDown && !m_Openning){
			Intro.PlayIntro();
			m_Openning = true;
		}
	}

	public void Skip_Introduce(){
		Intro.SkipIntro();
	}

	public void GameOver(){
		if(GameState == SHEESTATE.END) return;
		ResetVolume ();
		ComponentConstant.SOUND_MANAGER.StopBGM();
		PlayScreen.Ending.gameObject.SetActive(false);
		Header.Instance.RemoveOnPauseCallback(PauseGame);
		PauseManager.s_Instance.RemoveOnBackToGameCallback(ResumeGame);
		GameState = SHEESTATE.END;
		SendGameEndingAPI();

	}
}
