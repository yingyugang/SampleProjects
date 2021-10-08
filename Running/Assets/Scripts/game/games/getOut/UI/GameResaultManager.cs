using UnityEngine;
using System.Collections;
using System.IO;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;
using System;

public enum AnimResault{
	idle = 0,
	ScoreAnim = 1,
	ScoreBonus = 2,
	ScoreBonusMax = 3,
	ScoreTotal = 4
}


public class GameResaultManager : MonoBehaviour {




	private static GameResaultManager _instance;

	public GameBonusManager panelGetBonus;
	public GameObject spriteHeader;
	public Sprite[] arraySpriteHeader;
	public GameObject[] arrayTextGame;
	public Sprite[] arrayRankText;
	public GameObject imageRank;
	public GameObject imageJugdment;
	public GameObject imageAvatar;
	public GameObject txtBonus;
	public GameObject txtBonusMax;
	public GameObject txtTextBonusMax;
	public GameObject txtTextScoreBonus;
	public GameObject txtTextStageClear;
	public GameObject txtTextItemBonus;
	public GameObject txtCardScoreTotalTitle;
	public Text warningText;
	//publuc GameObject txtComboBonum

	public Text txtScore;
	public Text txtScoreBonus;
	public Text txtScoreBonusMax;
	public Text txtScoreTotal;
	public Text txtHighScore;
	public Text txtNewHighScore;
	public Text txtUserName;
	public Text txtCardScoreTotal;
	public GameObject imgNewHighScore;
	public GameObject resaultMask;

    public GameObject panelGameResault;
	public GameShareManager gameSharePanel;
    public MissionComplete missionCompletePanel;

    private int m_Rank;
	private int m_IndexGame;
	private int m_Score;
	private int m_ScoreBonus;
	private int m_ScoreTotal;
	private int m_ScoreBonusMax;
	private int m_HighScore;
	private float m_CountScoreAnim;
	private float m_TimeAnimRanking = 3f;
	private bool m_IsScoreAnim;
	private bool m_IsScoreBonusAnim;
	private bool m_IsScoreBonusMaxAnim;
	private bool m_IsScoreTotalAnim;
	private bool m_IsRankingAnim;
	private bool m_HaveCombo;
	private bool m_HaveStage;
	private float m_cardBonus;
	private float m_PercentIncrease = 0.05f;
	private Animator m_Animator;
	private string m_ValueAnimator = "value";
	private string m_ValueRankAnimator = "isRanking";
	private float m_RadiusShaking = 5f;
	private Vector3 positionScoreDefault;
	private Vector3 positionScoreBonusDefault;
	private Vector3 positionScoreBonusMaxDefault;
	private Vector3 positionScoreTotalDefault;
    private bool m_IsSentToserverComplted = false; 
	public static GameResaultManager Instance
	{
		get{ return _instance;}
	}

	// Use this for initialization
	void Awake () {
		_instance = this;
		imageRank.SetActive(false);
		panelGameResault.SetActive(false);
	}

	void Start()
	{
		if (GameConstant.gameDetail != null)
			m_HighScore = GameConstant.gameDetail.score;
		else
			m_HighScore = 0;
	}

	private void Init(int indexGame)
	{
		m_Animator = gameObject.GetComponent<Animator>();
		imgNewHighScore.SetActive(false);
		txtNewHighScore.text = "";
		SetImageHeaderPanelResault(indexGame);
		panelGameResault.SetActive(true);
		positionScoreBonusDefault = txtScoreBonus.transform.localPosition;
		positionScoreDefault = txtScore.transform.localPosition;
		positionScoreBonusMaxDefault = txtScoreBonusMax.transform.localPosition;
		positionScoreTotalDefault = txtScoreTotal.transform.localPosition;
		txtScore.text = "";
		txtScoreBonus.text = "";
		txtScoreBonusMax.text = "";
		txtScoreTotal.text = "";
		txtHighScore.text = "";
		txtNewHighScore.text = "";
		imageRank.SetActive(false);
		m_Animator.SetBool(m_ValueRankAnimator,false);
		m_Animator.SetInteger(m_ValueAnimator,(int)AnimResault.idle);
	}



	// Update is called once per frame
	void Update () {
		if(m_IsScoreAnim)
		{
			RunAnimScore();
		}

		if(m_IsScoreBonusAnim)
		{
			RunAnimScoreBonus();
		}

		if(m_IsScoreBonusMaxAnim)
		{
			RunAnimScoreBonusMax();
		}

		if(m_IsScoreTotalAnim)
		{
			RunAnimScoreTotal();
		}

		if(m_IsRankingAnim)
		{
			m_TimeAnimRanking -= 0.01f;
			if(m_TimeAnimRanking <= 0)
			{
				m_IsRankingAnim = false;
			}
			AnimShakingObject(imageRank);
			AnimShakingObject(imageJugdment);
		}
	}

	public void SetUser(string name,Sprite avatar,bool haveCombo, bool haveStage = false,bool isComboAndItem = false)
	{
		txtUserName.text = name;
		imageAvatar.GetComponent<Image>().sprite = avatar;
		m_HaveCombo = haveCombo;
		if(m_HaveCombo && haveStage){
			txtTextItemBonus.SetActive (true);
			txtTextBonusMax.SetActive (true);
			txtBonusMax.SetActive(true);
		}else if(m_HaveCombo)
		{
			txtBonus.SetActive(true);
			txtBonusMax.SetActive(true);
			txtTextStageClear.SetActive (false);
			txtTextBonusMax.SetActive (true);
			txtTextScoreBonus.SetActive(false);
		}
		else
		{
			txtBonus.SetActive(false);
			txtBonusMax.SetActive(false);
			txtTextScoreBonus.SetActive(true);
			if (haveStage) {
				txtBonusMax.SetActive(true);
				txtTextStageClear.SetActive (true);
				txtTextBonusMax.SetActive (false);
			}
		}
	}

	public void SetImageHeaderPanelResault(int indexGame)
	{
		m_IndexGame = indexGame;
		spriteHeader.GetComponent<Image>().sprite = arraySpriteHeader[indexGame-1];
		arrayTextGame[indexGame -1].SetActive(true);
	}

	public Sprite rankSprite;
	/// <summary>
	/// Sets the game result information.
	/// </summary>
	public void SetGameResultInformation(int score, int scoreBonus, int scoreBonusMax, int rank,float cardBonus, bool isCombo = false, bool haveStage = false, bool sentToSever = false)
	{
        m_IsSentToserverComplted = sentToSever;
		this.m_Score = score;
		this.m_ScoreBonus = scoreBonus;
		this.m_ScoreBonusMax = scoreBonusMax;
		this.m_ScoreTotal = Mathf.RoundToInt((score + scoreBonus + scoreBonusMax) * (1 + cardBonus / 100));
		this.m_Rank = rank;
		this.m_HaveStage = haveStage;
		this.m_cardBonus = cardBonus;
		rankSprite = arrayRankText [m_Rank-1];
		Init(m_IndexGame);
		m_Animator.SetInteger(m_ValueAnimator,(int)AnimResault.ScoreAnim);
		m_IsScoreAnim = true;
		if(UpdateInformation.GetInstance != null)
			SetUser(UpdateInformation.GetInstance.player.name,GameConstant.headerSprite,isCombo,haveStage);
		if (cardBonus > 0)
			ShowCardScoreBonus (cardBonus);
	}
		
	void ShowCardScoreBonus(float bonus){
		Debug.Log ("bonus:" + bonus);
		this.txtCardScoreTotalTitle.SetActive (true);
		this.txtCardScoreTotal.gameObject.SetActive (true);
		this.txtCardScoreTotal.text = "+" + Mathf.RoundToInt(bonus) + "%";
	}

	private void RunAnimScore()
	{
		if((m_CountScoreAnim + m_PercentIncrease*m_Score) >= m_Score)
		{
			m_IsScoreAnim = false;
			m_IsScoreBonusAnim = true;
			txtScore.text = m_Score.ToString();
			txtScore.transform.localPosition = positionScoreDefault;
			m_CountScoreAnim = 0;
			m_Animator.SetInteger(m_ValueAnimator,(int)AnimResault.ScoreBonus);
			ComponentConstant.SOUND_MANAGER.Play(SoundEnum.se08_score);
		}else
		{
			if((int)(m_PercentIncrease*m_Score) <= 0)
			{
				m_CountScoreAnim += 1;
			}else
			{
				m_CountScoreAnim += (int)(m_PercentIncrease*m_Score);
			}
			txtScore.text = m_CountScoreAnim.ToString();
			AnimShakingObject(txtScore.gameObject);
		}
	}

	private void RunAnimScoreBonus()
	{
		if((m_CountScoreAnim + m_PercentIncrease*m_ScoreBonus) >= m_ScoreBonus )
		{
			m_IsScoreBonusAnim = false;
			m_IsScoreBonusMaxAnim = true;
			m_CountScoreAnim = 0;
			txtScoreBonus.text = m_ScoreBonus.ToString();
			txtScoreBonus.transform.localPosition = positionScoreBonusDefault;
			m_Animator.SetInteger(m_ValueAnimator,(int)AnimResault.ScoreBonusMax);
			ComponentConstant.SOUND_MANAGER.Play(SoundEnum.se08_score);
		}else
		{
			if((int)(m_PercentIncrease*m_ScoreBonus) <= 0 && m_ScoreBonus != 0)
			{
				m_CountScoreAnim += 1;
			}else
			{
				m_CountScoreAnim += (int)(m_PercentIncrease*m_ScoreBonus);
			}
			txtScoreBonus.text = m_CountScoreAnim.ToString();
			AnimShakingObject(txtScoreBonus.gameObject);
		}
	}

	private void RunAnimScoreBonusMax()
	{
		if((m_CountScoreAnim + m_PercentIncrease*m_ScoreBonusMax) >= m_ScoreBonusMax)
		{
			txtScoreBonusMax.text = m_ScoreBonusMax.ToString();
			txtScoreBonusMax.transform.localPosition = positionScoreBonusMaxDefault;
			m_IsScoreBonusMaxAnim = false;
			m_IsScoreTotalAnim = true;
			ComponentConstant.SOUND_MANAGER.Play(SoundEnum.se08_score);
			m_CountScoreAnim = 0;
			m_Animator.SetInteger(m_ValueAnimator,(int)AnimResault.ScoreTotal);
		}else
		{
			if((int)(m_PercentIncrease*m_ScoreBonusMax) <= 0 && m_ScoreBonusMax != 0)
			{
				m_CountScoreAnim += 1;
			}else
			{
				m_CountScoreAnim += (int)(m_PercentIncrease*m_ScoreBonusMax);
			}
			txtScoreBonusMax.text = m_CountScoreAnim.ToString();
			AnimShakingObject(txtScoreBonusMax.gameObject);
		}
	}

	private void RunAnimScoreTotal()
	{
		if((m_CountScoreAnim + m_PercentIncrease*m_ScoreTotal) >= m_ScoreTotal ||  (m_PercentIncrease*m_ScoreTotal < 0))
		{
			txtScoreTotal.text = m_ScoreTotal.ToString();
			txtScoreTotal.transform.localPosition = positionScoreTotalDefault;
			m_IsScoreTotalAnim = false;
			m_IsRankingAnim = true;
			ComponentConstant.SOUND_MANAGER.Play(SoundEnum.se08_score);
			SetRank();
			m_Animator.SetInteger(m_ValueAnimator,(int)AnimResault.idle);
			m_Animator.SetBool(m_ValueRankAnimator,true);
			m_CountScoreAnim = 0;
		}else
		{
			if((int)(m_PercentIncrease*m_ScoreTotal) <= 0)
			{
				m_CountScoreAnim += 1;
			}else
			{
				m_CountScoreAnim += (int)(m_PercentIncrease*m_ScoreTotal);
			}
			txtScoreTotal.text = m_CountScoreAnim.ToString();
			AnimShakingObject(txtScoreTotal.gameObject);
		}
	}

	

	private void SetRank()
	{
		SetImage(m_Rank-1);
		imageRank.SetActive(true);
		imageJugdment.SetActive(true);
		//txtHighScore.text = m_HighScore.ToString();
		if(m_ScoreTotal > m_HighScore)
		{
			imgNewHighScore.SetActive(true);
		//	txtNewHighScore.text = m_ScoreTotal.ToString();
		}
		if (GameConstant.currentEventGameID == m_IndexGame) {
			SetHighScoreText ();
			txtNewHighScore.text = (m_ScoreTotal * GameConstant.GameRate).ToString ();
			warningText.gameObject.SetActive (false);
		} else {
			if (GameConstant.currentEventGameID != 0) {
				SetHighScoreText ();
				warningText.text = string.Format (LanguageJP.GAME_PT_WARNING, GameConstant.GameEventBonus);
				warningText.gameObject.SetActive (true);
				txtNewHighScore.text = (Math.Round (m_ScoreTotal * GameConstant.GameRate * (GameConstant.GameEventBonus / 100f))).ToString ();
			} else {
				warningText.gameObject.SetActive (false);
				resaultMask.SetActive (true);
			}
		}
	}

	void SetHighScoreText(){
		resaultMask.SetActive (false);
		if (GameConstant.GameRate > 1) {
			txtHighScore.text = "X " + GameConstant.GameRate.ToString ();
		} else {
			txtHighScore.text = "なし";
		}
	}


	void SetImage(int index)
	{
		imageRank.GetComponent<Image>().sprite = arrayRankText[index];
		imageRank.GetComponent<RectTransform>().sizeDelta = new Vector2(arrayRankText[index].rect.width,arrayRankText[index].rect.height);
	}

	public void BackToHomeScene()
	{
		ComponentConstant.SOUND_MANAGER.Play(SoundEnum.se01_ok);
		Time.timeScale = 1;
        if (APIInformation.GetInstance != null && APIInformation.GetInstance.clear_mission_list != null)
        {
            int count = APIInformation.GetInstance.clear_mission_list.Count;
            Debug.Log("count: " + count);
            Debug.Log("m_IsSentToserverComplted: " + m_IsSentToserverComplted);
            Debug.Log("missionCompletePanel: " + missionCompletePanel != null);

            if (count > 0 && missionCompletePanel != null && m_IsSentToserverComplted)
            {
                missionCompletePanel.gameObject.SetActive(true);
                int index = m_IndexGame - 1;
                StartCoroutine(missionCompletePanel.LoadMissionItems(index));
                missionCompletePanel.imgBanner.sprite = arraySpriteHeader[index];
                missionCompletePanel.listTextGame.transform.GetChild(index).gameObject.SetActive(true);
                return;
            }
        }
		//if(APIInformation.GetInstance.next_clear_gacha == 10)
		if (APIInformation.GetInstance.gacha_result.cardinfo_list != null) {
			ComponentConstant.GACHA_PLAYER.unityAction = AnimationCompleteHandler;
			ComponentConstant.GACHA_PLAYER.Play (8, true);
		}
		else
			ComponentConstant.SCENE_COMBINE_MANAGER.LoadScene(SceneEnum.Home);
		
	}

	private void AnimationCompleteHandler (int type, bool isChanged)
	{
		ComponentConstant.SCENE_COMBINE_MANAGER.LoadScene(SceneEnum.Home);
	}

	public void ShowGetBonusPanel()
	{
		ComponentConstant.SOUND_MANAGER.Play(SoundEnum.se01_ok);
		panelGetBonus.InitPanel(m_IndexGame);
	}



	public void AnimShakingObject(GameObject objectAnim)
	{
		Vector2 shaking = UnityEngine.Random.insideUnitCircle * m_RadiusShaking;
		objectAnim.transform.localPosition = shaking;
	}

	public void ButtonShare()
	{
		ComponentConstant.SOUND_MANAGER.Play(SoundEnum.se01_ok);
		gameSharePanel.Init(m_IndexGame,m_Score,m_ScoreBonus,m_ScoreBonusMax,m_Rank,m_HaveCombo,m_HaveStage,m_cardBonus);
		gameSharePanel.gameObject.SetActive(true);
	}

	public void SetLastLevel(int lastLevel, int lastExp)
	{
		panelGetBonus.SetLastLevel(lastLevel,lastExp);
	}
}
