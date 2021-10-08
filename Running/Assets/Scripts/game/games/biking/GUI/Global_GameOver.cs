using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;
public class Global_GameOver : MonoBehaviour {
	//--> STANDARD SINGLETON INIT FOR GLOBAL_GAMEOVER
	public static Global_GameOver _instance;
	public static Global_GameOver instance{
		get{
		//	if(_instance == null) _instance = GameObject.FindObjectOfType<Global_GameOver>();
			return _instance;
		}
	}
	//--<

	//-->Delegate
	public delegate void OnGameOverFinishDelegate();
	private static event OnGameOverFinishDelegate OnGameOverFinishEvt;
	//--<Delegate
	public GameOver_Elmp GameOver_Element;
	[HideInInspector]public Global_GUI_Ent GlobalGuiEnt;
	[HideInInspector]public GameOver_Controller GameOver_Controller;
	public Vector2 s_masterDevice = new Vector2(1080.0f, 1920.0f);
	public Image ImgFillBackground;
	//-->Base var
	public RectTransform Avatar;
	public Text Txttitle;
	public RectTransform RectTitleBorder;
	public Text TxtName;
	public RectTransform RectcharacterName_Border;
	public Text TxtScore; 
	public Text TxtScore_Obj; 
	public Text TxtItemScore; 
	public Text TxtItemScore_Obj; 
	public Text TxtComboScore; 
	public Text TxtComboScore_Obj; 
	public Text TxtTotalScore; 
	public Text TxtTotalScore_Obj; 
	public Text TxtHighScore; 
	public Text TxtHighScore_Obj; 
	public Text TxtMoreHighScore; 
	public Text TxtMoreHighScore_Obj; 
	public GameObject RankingObj;
	public GameObject HRankingObj;
	public GameObject sHighScore;
	public GameObject hHighScore;
	public GameObject Okay_Base_Btn;
	public GameObject Base_Grp;
	public GameObject Sub_Grp;
	//--<Base var
	public Image Award_1;
	public Image Award_2;
	//-->Sub var
	public Text TxtExpTitle;
	public Image ExpBar;
	public Text TxtExpLvl;
	//--<Sub var
	public List<IEnumerator> bCoroutine;
	public List<IEnumerator> sCoroutine;
	public void OnGameOverCallBack(OnGameOverFinishDelegate callback){
		OnGameOverFinishEvt += callback;
	}
	public void RemoveGameOverCallBack(OnGameOverFinishDelegate callback){
		OnGameOverFinishEvt -= callback;
	}
	void Awake(){
		_instance = this;
		bCoroutine = new List<IEnumerator>();
		sCoroutine = new List<IEnumerator>();
		OnCheckComponent();
		GameOver_Init(gameObject, .0f, false);
	}
	public void BaseCallMeSkip(){
		for(int i = 0; i < bCoroutine.Count; i++){
			StopCoroutine(bCoroutine[i]);
		}
		TxtScore.text = GameOver_Element.score.ToString();
		TxtItemScore.text = GameOver_Element.itemScore.ToString();
		TxtComboScore.text = GameOver_Element.comboScore.ToString();
		TxtTotalScore.text = GameOver_Controller.TotalScore(GameOver_Element.score, GameOver_Element.itemScore, GameOver_Element.comboScore, TxtTotalScore).ToString();
		TxtHighScore.text = GameOver_Element.highScore.ToString();
		//-->Ranking obj p
		RankingObj.SetActive(true);
		Image _rankSymbol = RankingObj.transform.GetChild(0).GetChild(0).GetComponent<Image>();
		for(int i = 0; i < GameOver_Element.rankSymbol.Length; i++){
			if("score_" + GameOver_Controller.Ranking(GameOver_Element.totalScore) == GameOver_Element.rankSymbol[i].name)_rankSymbol.sprite = GameOver_Element.rankSymbol[i];
			_rankSymbol.SetNativeSize();}
		RankingObj.transform.DOScale(1.0f, .4f);
		//--<Ranking obj p
		if(GameOver_Controller.HighScoreDel(GameOver_Controller.HighScoreCal) != 0){
			hHighScore.SetActive(true);
			Q_MoreHighScore();
		}
	}
	public void SubCallMeSkip(){
		
	}
	public void ShowGameOver_Gr(){
//		Time.timeScale = 0;
		GlobalGuiEnt.DoTweenFix(1.5f);
		GameOver_Init(gameObject, 1.0f, true);
		Q_Base();
		//Add & fire gameover event
		OnGameOverFinishEvt += OnLoad_Result;
		OnGameOverFinishEvt();
	}
	public void HideGameOver_Gr(){
		GameOver_Init(gameObject, .0f, false);
		Q_Base();
		ResetGameOver();
		OnGameOverFinishEvt = null;
	}
	public void GameOver_Init(GameObject obj, float a, bool b){
		obj.GetComponent<CanvasGroup>().alpha = a;
		obj.GetComponent<CanvasGroup>().blocksRaycasts = b;
	}
	public void Q_Base(){
		GameOver_Init(Base_Grp, 1.0f, true);
		GameOver_Init(Sub_Grp, .0f, false);
	}
	public void Q_Sub_Grp(){
		GameOver_Init(Sub_Grp, 1.0f, true);
		GameOver_Init(Base_Grp, .0f, false);
		Q_Sub_EXP();
	}
	public void ResetGameOver(){
		TxtTotalScore.transform.DOLocalMoveY(.0f, .0f, false);
		Destroy(HRankingObj.GetComponent<Canvas>());
		TxtScore.text = "0";
		TxtItemScore.text = "0";
		TxtComboScore.text = "0";
		TxtTotalScore.text = "0";
		TxtHighScore.text = "0";
		Text _rank = RankingObj.GetComponentInChildren<Text>();
		_rank.text = "評定";
		if(GameObject.Find(BikingKey.SSTring.Must_Destroy))Destroy(GameObject.Find(BikingKey.SSTring.Must_Destroy));
		hHighScore.SetActive(false);
		RankingObj.transform.localScale = new Vector3(3.0f, 3.0f, 3.0f);
		RankingObj.SetActive(false);
		HRankingObj.transform.localScale = new Vector3(3.0f, 3.0f, 3.0f);
		HRankingObj.SetActive(false);
		//--> Restart
//		SceneManager.LoadScene("biking");
		//--<
	}
	void OnLoad_Result(){
		// Fill background
		//--> Shutdown touch area
//		Game12_GUI_Manager.instance.G_TouchArea.SetActive(false);
		//--<
		ImgFillBackground.GetComponent<RectTransform>().sizeDelta = s_masterDevice;
		Txttitle.text = GameOver_Element.title;
		IEnumerator c = GlobalGuiEnt.ObjShaking(Avatar);
		bCoroutine.Add(c);
		StartCoroutine(c);
		IEnumerator cs  = GlobalGuiEnt.AutoTypeBorder(GameOver_Element.name, TxtName, RectcharacterName_Border, .02f, "Q_Score");
		bCoroutine.Add(cs);
		StartCoroutine(cs);
	}
	//-->Gui Queue
	// The animation all of gameover'obj with the QUEUE
	public void Q_Score(){

		TxtTimerFire(GlobalGuiEnt.TextTimer(GameOver_Element.score, TxtScore, "Q_ItemScore"));
		ObjShakeFire(GlobalGuiEnt.ObjShaking(TxtScore_Obj.transform));
	}
	public void Q_ItemScore(){
		TxtTimerFire(GlobalGuiEnt.TextTimer(GameOver_Element.itemScore, TxtItemScore, "Q_ComboScore"));
		ObjShakeFire(GlobalGuiEnt.ObjShaking(TxtItemScore_Obj.transform));
	}
	public void Q_ComboScore(){
		TxtTimerFire(GlobalGuiEnt.TextTimer(GameOver_Element.comboScore, TxtComboScore, "Q_TotalScore"));
		ObjShakeFire(GlobalGuiEnt.ObjShaking(TxtComboScore_Obj.transform));
	}
	public void Q_TotalScore(){
		TxtTimerFire(GlobalGuiEnt.TextTimer(GameOver_Controller.TotalScore(GameOver_Element.score, GameOver_Element.itemScore, GameOver_Element.comboScore, TxtTotalScore), TxtTotalScore, "Q_HighScore"));
		ObjShakeFire(GlobalGuiEnt.ObjShaking(TxtTotalScore_Obj.transform));
	}
	public void Q_HighScore(){
		//-->Ranking obj p
		RankingObj.SetActive(true);
		Image _rankSymbol = RankingObj.transform.GetChild(0).GetChild(0).GetComponent<Image>();
		for(int i = 0; i < GameOver_Element.rankSymbol.Length; i++){
			if("score_" + GameOver_Controller.Ranking(GameOver_Element.totalScore) == GameOver_Element.rankSymbol[i].name)_rankSymbol.sprite = GameOver_Element.rankSymbol[i];
			_rankSymbol.SetNativeSize();}
		RankingObj.transform.DOScale(1.0f, .4f);
		//--<Ranking obj p
		if(GameOver_Controller.HighScoreDel(GameOver_Controller.HighScoreCal) != 0){
			hHighScore.SetActive(true);
			TxtTimerFire(GlobalGuiEnt.TextTimer(GameOver_Element.highScore, TxtHighScore, "Q_MoreHighScore"));
			ObjShakeFire(GlobalGuiEnt.ObjShaking(TxtHighScore_Obj.transform));
			
		} 
	}
	public void Q_MoreHighScore(){
		//-->Past p
		TxtTotalScore.transform.DOMove(TxtMoreHighScore.transform.position, 1.0f, false);
		//--<Past p
		// Show high ranking gameobject with highest layer

		Invoke("CerInvo", 1.2f);
		ObjShakeFire(GlobalGuiEnt.ObjShaking(TxtMoreHighScore_Obj.transform));
	}
	//-->Sub area
	public void Q_Sub_EXP(){
		// For test
		ObjShakeFire(GlobalGuiEnt.ObjShaking(TxtExpTitle.transform));
		//Load exp bar state
		GameOver_Controller.EXPBarStateDel(GameOver_Controller.EXPBarStateCal);
	}

	//--<Sub area
	public void GoHome(string name){
		SceneManager.LoadScene(name);
	}
	//--<Gui Queue

	//--<Invo
	void CerInvo(){
		//-->HRanking obj p
		HRankingObj.SetActive(true);
		if(HRankingObj.GetComponent<Canvas>() == null){
			HRankingObj.AddComponent<Canvas>().overrideSorting = true;
			HRankingObj.GetComponent<Canvas>().sortingOrder = 10;
			HRankingObj.transform.DOScale(1.0f, .4f);
		}
		Okay_Base_Btn.SetActive(true);
		//--<HRanking obj p
	}
	//--<Invo

	//-->Coroutine manager
	// Add all coroutine to a list to stop wherenever you need.
	private void TxtTimerFire(IEnumerator txtDel){
		IEnumerator c = txtDel;
		bCoroutine.Add(c);
		StartCoroutine(c);
	}
	private void ObjShakeFire(IEnumerator objDel){
		IEnumerator c = objDel;
		bCoroutine.Add(c);
		StartCoroutine(c);
	}
	//--<Coroutine manager
	private void OnCheckComponent(){
		GlobalGuiEnt = this.GetComponent<Global_GUI_Ent>();
		GameOver_Controller = this.GetComponent<GameOver_Controller>();
		if (GlobalGuiEnt == null )GlobalGuiEnt = this.gameObject.AddComponent<Global_GUI_Ent>();
		if (GameOver_Controller == null )GameOver_Controller = this.gameObject.AddComponent<GameOver_Controller>();
	}
}
