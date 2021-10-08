using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ParameterServer
{
	public static float GameTime;
	public static float FeverTime;
	public static float TimeSpanByBomb;

	public static float[] lstProbabilityBlock;
	public static float[] lstProbabilityChestItem;

	public static float BlockScore;
	public static string CurrentGroup;
	//two main character was selected
//	public static string Groups;
	public static float FreezeTime;
	public static float DrillRateTime;
	public static string DoneSerif;
	public static bool DoubleCheck = false;
	//string parameter of character on selected
	public static List<int> lstSerifDone=null;
	public static int SerifID=-1;

	public static int GetCharacterSelected (bool mc_throw)
	{
		int characterThrow = -1;
		string[] MC_List = CurrentGroup.Split (new string[1]{ "|" }, System.StringSplitOptions.RemoveEmptyEntries);
		int MC_throw = int.Parse (MC_List [1]);
		int MC_hit = int.Parse (MC_List [0]);

		if (mc_throw) {
			return MC_throw;
		} else {
			return MC_hit;
		}
	}
	public static void UpdateListSerifDone(int IDAdd){
		lstSerifDone = new List<int> ();
		DoubleCheck = false;
		if (ParameterServer.DoneSerif != "") {
			
			string[] lstSplit=ParameterServer.DoneSerif.ToString ().Split (new string[1]{ "|" }, System.StringSplitOptions.RemoveEmptyEntries);
			for (int i = 0; i < lstSplit.Length; i++) {
				lstSerifDone.Add (int.Parse(lstSplit [i].ToString ()));
			}


			for (int i = 0; i < lstSerifDone.Count; i++) {
				if (lstSerifDone[i]==IDAdd) {
					DoubleCheck = true;
					Debug.Log ("full serif");
					break;
				}
			}
			if (!DoubleCheck) {
				Debug.Log ("Add serif"+IDAdd);
				ParameterServer.DoneSerif += "|" + IDAdd;
				lstSerifDone.Add (IDAdd);
			}
		}else
			lstSerifDone.Add (IDAdd);
		
	}
}

public class OilKingManager : MonoBehaviour
{

	private static OilKingManager _s_Instance;

	public static OilKingManager s_Instance {
		get {
			if (_s_Instance == null) {
				_s_Instance = GameObject.FindObjectOfType <OilKingManager> ();
			}
			return _s_Instance;
		}
	}

	public GameEndLogic paramsEndLogic;
	public SerifAPILogic serifAPILogic;
	public bool isRunOnLocal = true;

	public AudioSource m_Audio;
	public OilkingEnding oilkingEnding;

	void Awake ()
	{
		_s_Instance = this;

	}

	// Use this for initialization
	void Start ()
	{
		
		OilKingUtils.numPine = 0;
		OilKingUtils.numOden = 0;
		OilKingUtils.numOnigiri = 0;
		OilKingUtils.isRunGame = false;
		OilKingUtils.isPauseGame = false;
		OilKingUtils.MY_SCORE = 0;
		OilKingUtils.MY_COIN = 0;
		OilKingUtils.MY_TIME = 0;
		OilKingUtils.THROW_BOMB = 0;
		OilKingUtils.HIT_BOMB = 0;
		OilKingUtils.ID_SERIF = -1;
		ParameterServer.lstSerifDone = null;

		//api server
		if (APIInformation.GetInstance == null || isRunOnLocal) {
			//run on my screen
			Debug.Log ("run on my screen");
			ParameterServer.GameTime = 60;
			ParameterServer.FeverTime = 10;
			ParameterServer.TimeSpanByBomb = 5;

			//rock - Fossil - Plaster - bomb - chest
			ParameterServer.lstProbabilityBlock = new float[5]{ 20, 30, 50, 80, 100 };

			//Totoko | Matsu | Oden | Onigiri | Nya | Bomb
			ParameterServer.lstProbabilityChestItem = new float[6]{ 20, 30, 50, 80, 100, 20 };
			ParameterServer.BlockScore = 5.5f;

		} else {
			//run on home screen
			GameParameter parameter_server = new GameParameter ();
			parameter_server = APIInformation.GetInstance.gameparameter;
//			Debug.Log ("time="+parameter_server.game_time); 
			ParameterServer.GameTime = parameter_server.game_time;
			ParameterServer.FeverTime = parameter_server.fever_time;
			ParameterServer.TimeSpanByBomb = parameter_server.item_span_second;

			//rock - Fossil - Plaster - bomb - chest
			Debug.Log (parameter_server.blocks_pro);
			string[] s_ratio = parameter_server.blocks_pro.ToString ().Split (new string[1]{ "|" }, System.StringSplitOptions.RemoveEmptyEntries);
			ParameterServer.lstProbabilityBlock = new float[5];
			for (int i = 0; i < ParameterServer.lstProbabilityBlock.Length; i++) {
				ParameterServer.lstProbabilityBlock [i] = int.Parse (s_ratio [i]);
			}

			//Totoko | Matsu | Oden | Onigiri | Nya | Bomb
			string[] s_ratioItem = parameter_server.items_pro.ToString ().Split (new string[1]{ "|" }, System.StringSplitOptions.RemoveEmptyEntries);
			ParameterServer.lstProbabilityChestItem = new float[6];
			for (int i = 0; i < ParameterServer.lstProbabilityChestItem.Length; i++) {
				ParameterServer.lstProbabilityChestItem [i] = int.Parse (s_ratioItem [i]);
			}

			ParameterServer.BlockScore = parameter_server.block_score;
			//current group from random
//			GetCurrentGroup ();
			ParameterServer.CurrentGroup = parameter_server.current_group;
//			ParameterServer.Groups = parameter_server.groups;
			ParameterServer.DrillRateTime = parameter_server.drill_rate_time;
			ParameterServer.FreezeTime = parameter_server.freeze_time;
//			for (int i = 0; i < 180; i++) {
//				ParameterServer.DoneSerif +=i+"|";
//			}
			ParameterServer.DoneSerif = parameter_server.done_serif;
//			Debug.Log ("Server= CurrentGroup=" + parameter_server.current_group + " \nGroups=" + parameter_server.groups);
//			Debug.Log ("done load from server=");
			ParameterServer.SerifID = parameter_server.serif_id;

		}

		//yield return new WaitForEndOfFrame ();
		Header.Instance.SetOnPauseCallback (OnPauseGame);
		PauseManager.s_Instance.SetOnBackToGameCallback (OnResumeGame);
		GameResaultManager.Instance.SetImageHeaderPanelResault (OilKingConfig.ID_GAME);
		GameResaultManager.Instance.SetLastLevel (UpdateInformation.GetInstance.player.lv, UpdateInformation.GetInstance.player.exp);

		//test
//		ParameterServer.DoneSerif = "1|3|4";

	}

	void OnDisable ()
	{
		Header.Instance.RemoveOnPauseCallback (OnPauseGame);
		PauseManager.s_Instance.RemoveOnBackToGameCallback (OnResumeGame);

		if (isRunOnLocal) {
			return;
		}
		if (Header.Instance)
			Header.Instance.popupCountDown.showOrHideBg -= OnResumeByCountDown;
	}

	void Update ()
	{
		if (Input.GetKeyDown (KeyCode.D)) {
			OnGameOver ();
		}
	}

	public void StartGame ()
	{

		if (isRunOnLocal || APIInformation.GetInstance == null) {
			OnResumeByCountDown (true);
		} else {
			
			Header.Instance.ShowPopupCountDown ();
			Header.Instance.popupCountDown.showOrHideBg += OnResumeByCountDown;

		}
		
	}

	private void OnResumeByCountDown (bool isShow)
	{
		Debug.Log ("OnResumeByCountDown");
		OilKingUtils.isRunGame = true;
		Header.Instance.isPause = false;
		OilKing_PlayUI.Instance.RaycastTargetButtons(true);

		if (ComponentConstant.SOUND_MANAGER != null) {
			ComponentConstant.SOUND_MANAGER.Play (SoundEnum.bgm13_oil, GetAudioSource);
		}
	}

	private void OnPauseGame ()
	{
		if (!OilKingUtils.isPauseGame) {
			OilKingUtils.isPauseGame = true;
			PauseManager.s_Instance.SetVisible (true);
			Header.Instance.isPause = true;
		}
	}

	private void OnResumeGame ()
	{
		OilKingUtils.isPauseGame = false;
		Header.Instance.isPause = false;
	}

	public void OnGameOver ()
	{
		if (OilKingUtils.isRunGame) {
			OilKingUtils.isRunGame = false;

//			GameResaultManager.Instance.SetGameResultInformation (10, 10, 10, 3, 0f, false, false, false);\
			if (isRunOnLocal) {
				int score = OilKingUtils.MY_SCORE;
				int rank = 1;
//				GameResaultManager.Instance.SetGameResultInformation(score, 0, 0, rank,0, true, false, (rank > 0));
			} else {
//				OilKingUtils.MY_SCORE = 1700;
				oilkingEnding.Show ();
			}

		}
	}

	[ContextMenu ("Clear Cache Asset Bundle")]
	public void ClearCacheAssetBundle ()
	{
		Caching.CleanCache ();
	}

	[ContextMenu ("Debug sever")]
	public void ClearCacheAssetBundle11 ()
	{
		for (int i = 0; i < ParameterServer.lstProbabilityBlock.Length; i++) {
			Debug.Log (ParameterServer.lstProbabilityBlock [i]);
		}
		for (int i = 0; i < ParameterServer.lstProbabilityChestItem.Length; i++) {
			Debug.Log (ParameterServer.lstProbabilityChestItem [i]);
		}
	}

	public void SendGameEndingAPI ()
	{
		//Debug.Log ("OilKingUtils.MY_SCORE=" + OilKingUtils.MY_SCORE + " ParameterServer.BlockScore=" + ParameterServer.BlockScore + " OilKingUtils.MY_COIN=" + OilKingUtils.MY_COIN + " OilKingUtils.MY_SCORE=" + OilKingUtils.MY_SCORE+" "+OilKingUtils.ID_SERIF);

		OilKingUtils.MY_SCORE = (int)(ParameterServer.BlockScore * (float)OilKingUtils.MY_COIN + OilKingUtils.MY_SCORE);
		paramsEndLogic.card_bonus = CardRate.GetTotal (2, OilKingConfig.ID_GAME);
		paramsEndLogic.score = Mathf.RoundToInt(OilKingUtils.MY_SCORE * (1f + paramsEndLogic.card_bonus / 100f));
		paramsEndLogic.card_ids = CardRate.GetAdditionalCardID (2, OilKingConfig.ID_GAME);
		paramsEndLogic.m_game_id = OilKingConfig.ID_GAME;
		paramsEndLogic.play_time = (int)(OilKingUtils.MY_TIME);
		paramsEndLogic.item_get_num = OilKingUtils.numPine + OilKingUtils.numOnigiri + OilKingUtils.numOden;

		ItemAttribute item2 = OilKingCSV.s_Instance.getItemAttribute(Block.Item2);
		ItemAttribute item3 = OilKingCSV.s_Instance.getItemAttribute(Block.Item3);
		ItemAttribute item4 = OilKingCSV.s_Instance.getItemAttribute(Block.Item4);
		OilKingUtils.scorebonus = OilKingUtils.numPine * item2.score + 
		                             OilKingUtils.numOnigiri * item3.score +
		                             OilKingUtils.numOden * item4.score;
		
		paramsEndLogic.nomiss = (OilKingUtils.ResultMissionDontThrowAnyBomb () == true) ? 1 : 0;
//		paramsEndLogic.sixcombination = (OilKingUtils.ResultMissionSixBrothersMeet () == true) ? 1 : 0;
//		paramsEndLogic.thrown_bomb = OilKingUtils.THROW_BOMB;
		paramsEndLogic.current_group = OilKingUtils.current_group;
		paramsEndLogic.serif_id = OilKingUtils.ID_SERIF;
		paramsEndLogic.blocks_num = OilKingUtils.THROW_BOMB;
		paramsEndLogic.complete = SendAPICompleteHandler;
		paramsEndLogic.error = SendAPIErrorHandler;
		paramsEndLogic.SendAPI ();
		Debug.Log ("score = " + paramsEndLogic.score);
		Debug.Log ("local nomiss=" + ((OilKingUtils.ResultMissionDontThrowAnyBomb () == true) ? 1 : 0)
		+ " sixcombination=" + ((OilKingUtils.ResultMissionSixBrothersMeet () == true) ? 1 : 0)
		+ " thrown_bomb=" + OilKingUtils.THROW_BOMB);

	}

	private void SetGameOverInfo ()
	{

		if (ComponentConstant.SOUND_MANAGER != null) {
			ComponentConstant.SOUND_MANAGER.StopBGM ();
		}
		int score = OilKingUtils.MY_SCORE;
		int highscore = GameConstant.gameDetail.score; // It's high score.
		if (score > highscore) {
			highscore = score;
		}
//		Debug.Log("game over");
		Debug.Log ("OilKingUtils.MY_SCORE=" + OilKingUtils.MY_SCORE + " " + APIInformation.GetInstance.rank);
		int rank = APIInformation.GetInstance.rank;
		Debug.Log ("rank=" + rank);
		if (rank <= 0) {
			rank = 1;
		}
		GameResaultManager.Instance.SetGameResultInformation (OilKingUtils.MY_SCORE - OilKingUtils.scorebonus, OilKingUtils.scorebonus, 0, rank, paramsEndLogic.card_bonus, false, false, (APIInformation.GetInstance.rank > 0));
	}

	void SendAPICompleteHandler ()
	{
//		Debug.Log("SendAPICompleteHandler!" + UpdateInformation.GetInstance.game_list[ID_GAME].score);
//		Debug.Log("SendAPICompleteHandler!" + APIInformation.GetInstance.rank);
//		Debug.Log("SendAPICompleteHandler!" + Player.GetInstance.lv);
//		Debug.Log("SendAPICompleteHandler!" + Player.GetInstance.exp);
		Debug.Log ("SendAPICompleteHandler");
		SetGameOverInfo ();
	}

	void SendAPIErrorHandler (string str)
	{
		Debug.Log ("SendAPIErrorHandler! " + str);
		SetGameOverInfo ();
	}

	public void GetAudioSource (AudioSource audio)
	{
		if (m_Audio == null || m_Audio != audio) {
			m_Audio = audio;
			PauseManager.s_Instance.Init (OilKingConfig.ID_GAME, m_Audio);
		}
	}
//	public void GetCurrentGroup(){
//		int idThrow = Random.Range (1, 7);
//		int idHit = Random.Range (1, 7);
//		do {
//			idHit = Random.Range (1, 7);
//		} while(idThrow == idHit);
//
//		ParameterServer.CurrentGroup = idHit + "|" + idThrow;
//	}

	public void SendAPISerif(){
		serifAPILogic.serif_id = OilKingUtils.ID_SERIF;
		serifAPILogic.current_group = OilKingUtils.current_group;
		serifAPILogic.complete = SendAPISerifComplete;
		serifAPILogic.error = SendAPISerifError;
		serifAPILogic.SendAPI ();
	}
	void SendAPISerifComplete(){
		Debug.Log ("Serif API Success");
	}
	void SendAPISerifError(string str){
		Debug.Log ("Serif API Error "+str);
	}
}