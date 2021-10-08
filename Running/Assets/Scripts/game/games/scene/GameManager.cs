using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using DG.Tweening;

namespace scene{
	public class GameManager : SingleMonoBehaviour<GameManager> {

		public const int GAME_ID = 8;
		public const int OBSTACLE_LAYER = 15;

		public List<Button> controllers;
		public Dictionary<string,Sprite> userSprites;
		public bool isPause;
		private int mTotalCorrectNum = 0;
		private int mTotalContinueCorrectNum = 0;
		private int mContinueCorrectNum = 0;
		float cardBonus = 0;
		public GameEndLogic gameEndLogic;
		public GameObject timePrefab;
		public int nomiss = 1;

		protected override void Awake(){
			Header.Instance.onPause += OnPause; 
			Header.Instance.SetShowNokori (true);
			Header.Instance.SetScore ("正解    " + 0 + " 問");
			if (PauseManager.s_Instance != null) {
				PauseManager.s_Instance.SetOnBackToGameCallback (OnResume);
				//PauseManager.s_Instance.SetOnExitCallback (ReStart);
			}
			if (gameEndLogic == null)
				gameEndLogic = gameObject.AddComponent<GameEndLogic> ();
		}

		void Start(){
			if (GameResaultManager.Instance != null) {
				GameResaultManager.Instance.SetImageHeaderPanelResault (GAME_ID);
				if(UpdateInformation.GetInstance!=null)
					GameResaultManager.Instance.SetLastLevel (UpdateInformation.GetInstance.player.lv, UpdateInformation.GetInstance.player.exp);
			}
			cardBonus = GetCardBonus ();

		}

		float GetCardBonus(){
			return CardRate.GetTotal(2,GAME_ID);
		}

		//Dictionary<string,Sprite> mCheatDictionary;
	

		void Update () {
			if (isPause)
				return;
		}

		int mMaxContinueCorrectNum;
		public int MaxContinueCorrectNum{
			get{
				return mMaxContinueCorrectNum >> 1;
			}
			set{
				mMaxContinueCorrectNum = value << 1;
			}
		}

		public int TotalCorrectNum{
			get{
				return mTotalCorrectNum >> 1;
			}
			set{ 
				UIManager.GetInstance ().SetTotalCorrectAnswerSum (value);
				mTotalCorrectNum = value << 1;
			}
		}

		public int ContinueCorrectNum{
			get{ 
				return mContinueCorrectNum >> 1;
			}
			set{ 
				
				mContinueCorrectNum = value << 1;
			}
		}

		public int TotalContinueCorrectNum{
			get{ 
				return mTotalContinueCorrectNum >> 1;
			}
			set{ 
				mTotalContinueCorrectNum = value << 1;
			}
		}

		public void OnPause(){
			isPause = true;
			if(PauseManager.s_Instance!=null)
				PauseManager.s_Instance.SetVisible (true);
		}

		public void OnResume(){
			isPause = false;
		}

		public void UpdateHeaderTime(float time){
			Header.Instance.SetLifeTime (time);
		}

		public void SetStepTime(float time){
			Header.Instance.SetLife (LifeType.Time,(int)time);
			Header.Instance.UpdateTime ();
			//Header.Instance.SetLife (LifeType.Time,(int)time);
		}

		public void EnableHeaderBtn(){
			Button btn = Header.Instance.GetComponentInChildren<Button> ();
			if(!btn.interactable){
				btn.interactable = true;
				btn.GetComponent<Image> ().color = btn.colors.normalColor;
			}
		}

		public void PauseHeaderTime(){
			Header.Instance.isPause = true;
		
		}

		public void DisableHeaderBtn(){
			Button btn = Header.Instance.GetComponentInChildren<Button> ();
			if(btn.interactable){
				btn.interactable = false;
				btn.GetComponent<Image> ().color = btn.colors.disabledColor;
			}
		}

		void _PlayEndBgm(){
			if (ComponentConstant.SOUND_MANAGER != null) {
				ComponentConstant.SOUND_MANAGER.Play (SoundEnum.bgm15_title_back, null);
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
			UIManager.GetInstance().ShowEnding();
		}

		void DisplayGameover()
		{
			if (ComponentConstant.SOUND_MANAGER != null) {
				ComponentConstant.SOUND_MANAGER.StopBGM (false,false,true);
			}
			GameResaultManager.Instance.SetGameResultInformation(
				GetScore(), 
				GetTotalContinueScoreBonus(), 
				GetMaxContinueScoreBonus(), 
				APIInformation.GetInstance.rank,cardBonus, true, false, APIInformation.GetInstance.rank > 0
			);
		}

		public int GetTotalScore()
		{
			return GetScore() + GetMaxContinueScoreBonus () + GetTotalContinueScoreBonus ();
		}

		int GetScore(){
			return Mathf.RoundToInt(this.TotalCorrectNum * GameParams.GetInstance ().paramData.comb_var1);
		}

		int GetMaxContinueScoreBonus(){
			Debug.Log (this.MaxContinueCorrectNum);
			return Mathf.RoundToInt (this.MaxContinueCorrectNum * GameParams.GetInstance().paramData.comb_var3);
		}

		int GetTotalContinueScoreBonus(){
			return Mathf.RoundToInt (this.TotalContinueCorrectNum * GameParams.GetInstance().paramData.comb_var2);
		}

		public void SendGameEndingAPI ()
		{
			gameEndLogic.m_game_id = GAME_ID;
			gameEndLogic.score = Mathf.RoundToInt(GetTotalScore() * (1 + this.cardBonus / 100));
			gameEndLogic.card_bonus = this.cardBonus;
			gameEndLogic.card_ids = CardRate.GetAdditionalCardID (2,GAME_ID);
			gameEndLogic.complete = SendAPICompleteHandler;
			gameEndLogic.combo_num = this.MaxContinueCorrectNum;
			gameEndLogic.nomiss = nomiss;
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

	}
}
