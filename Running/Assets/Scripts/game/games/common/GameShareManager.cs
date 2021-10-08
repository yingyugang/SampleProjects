using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UI;
public class GameShareInfor
{
	public int id;
	public int mGameId;
	public int rank;
	public string stampName;
	public string stampDescription;
	public string stampImageResource;

	public GameShareInfor(int id, int mGameId, int rank, string stampName, string stampDescription, string stampImageResource)
	{
		this.id = id;
		this.mGameId = mGameId;
		this.rank = rank;
		this.stampName = stampName;
		this.stampDescription = stampDescription;
		this.stampImageResource = stampImageResource;
	}
}

public enum StampSprite{
	Stamp01 = 0,
	Stamp02,
	Stamp03,
	Stamp04,
	Stamp05,
	Stamp06,
	Stamp07,
	Stamp08,
	Stamp09,
	Stamp10
}



public class GameShareManager : MonoBehaviour {


	public const string CSV_GAMESHARE = "m_game_share";
	public int idGame;
	public int rank;
	public GameShareInfor gameShareInfor;
	public GameShareInfor [] arrayGameShareInfor;
	public GameObject [] arraySpriteStamp;
	public GameObject [] arrayTextGame;
	public GameObject imgRank;
	public GameObject imgJugdment;
	public GameObject imgAvatar;
	public GameObject comboBonusMax;
	public GameObject txtTextComboMax;
	public GameObject txtTextStageClear;
	public GameObject txtComboTotal;
	public GameObject txtTextScoreBonus;
	public GameObject cardBonusGo;
	public Text txtStampDescription;
	public Text	txtStampName;
	public Text txtScore;
	public Text txtScoreBonus;
	public Text txtScoreBonusMax;
	public Text txtScoreTotal;
	public Text txtDayTimeCurrent;
	public Text txtUserName;
	public Text txtCardBonus;
	private Animator m_Animator;
	private int m_Score;
	private int m_ScoreBonus;
	private int m_ScoreTotal;
	private int m_ScoreBonusMax;
	private float m_CountScoreAnim;
	private bool m_IsScoreAnim;
	private bool m_IsScoreBonusAnim;
	private bool m_IsScoreBonusMaxAnim;
	private bool m_IsScoreTotalAnim;
	private bool m_IsAnimRanking;
	private float m_PercentIncrease = 0.05f;
	private float m_TimeAnimRank = 3f;
	private float m_RadiusShaking = 5f;
	private Vector3 positionScoreDefault;
	private Vector3 positionScoreBonusDefault;
	private Vector3 positionScoreBonusMaxDefault;
	private Vector3 positionScoreTotalDefault;
	private string m_ValueAnimator = "value";
	private string m_ValueRankAnimator = "isRanking"; 
	private string[] arraySpriteStampName = {
		"Stamp01",
		"Stamp02",
		"Stamp03",
		"Stamp04",
		"Stamp05",
		"Stamp06",
		"Stamp07",
		"Stamp08",
		"Stamp09",
		"Stamp10",
	};

	void Start()
	{
		positionScoreBonusDefault = txtScoreBonus.transform.localPosition;
		positionScoreDefault = txtScore.transform.localPosition;
		positionScoreBonusMaxDefault = txtScoreBonusMax.transform.localPosition;
		positionScoreTotalDefault = txtScoreTotal.transform.localPosition;
	}

	void Update () {

		if(m_IsAnimRanking)
		{
			if(m_TimeAnimRank <= 0)
			{
				m_IsAnimRanking = false;
			}
			m_TimeAnimRank -= 0.01f;
			AnimShakingObject(imgRank);
			AnimShakingObject(imgJugdment);
		}


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
	}



	public GameShareInfor GetGameShareInfor()
	{
		
		for (int i = 0; i < arrayGameShareInfor.Length; i++) {
			if(arrayGameShareInfor[i].mGameId == idGame && rank == arrayGameShareInfor[i].rank)
			{
				gameShareInfor = arrayGameShareInfor[i];
				return arrayGameShareInfor[i];
			}
		}
		return gameShareInfor;
	}



	private void LoadCsvGameShare()
	{
		GameShareInfor gameShareInfor;
		List<Dictionary<string, object>> data = CSVReader.Read("csv/" + CSV_GAMESHARE);
		if (data != null)
		{
			arrayGameShareInfor = new GameShareInfor[data.Count];
			Dictionary<string,float> dataParam = new Dictionary<string, float>();
			for (int i = 0; i < data.Count; i++)
			{
				int	id = (int)data[i]["id"];
				int gameId = (int)data[i]["m_game_id"];
				int rank =  (int)data[i]["rank"];
				string stampName = data[i]["stamp_name"].ToString();
				string stampDescription = data[i]["stamp_description"].ToString();
				string stampImageResource = data[i]["stamp_image_resource"].ToString();
				arrayGameShareInfor[i] = new GameShareInfor(id,gameId,rank,stampName,stampDescription,stampImageResource);
			}
		}
	}


	public void ShowInfor()
	{
		if(gameShareInfor == null)
			return;
		txtStampDescription.text = gameShareInfor.stampDescription;
		txtStampName.text = gameShareInfor.stampName;
		txtDayTimeCurrent.text =  System.DateTime.Now.Year+"年"+ System.DateTime.Now.Month+"月"+System.DateTime.Now.Day+"日";
		int StampID = (int)Enum.Parse (typeof(StampSprite), gameShareInfor.stampImageResource);
		if (StampID < arraySpriteStamp.Length)
			arraySpriteStamp[StampID].SetActive(true);
	}




	public void Init(int idGame,int score,int scoreBonus,int scoreBonusMax,int rank,bool haveCombo,bool haveStageClear,float cardBonus)
	{
		for(int i = 0; i < arrayTextGame.Length; i++)
		{
			arrayTextGame[i].SetActive(false);
		}
		LoadCsvGameShare();
		txtUserName.text = UpdateInformation.GetInstance.player.name;
		imgAvatar.GetComponent<Image>().sprite = GameConstant.headerSprite;
		m_Animator = gameObject.GetComponent<Animator>();
		this.m_Score = score;
		this.m_ScoreBonus = scoreBonus;
		this.m_ScoreBonusMax = scoreBonusMax;
		this.m_ScoreTotal = Mathf.RoundToInt((score + scoreBonus + scoreBonusMax) * (1 + cardBonus / 100f));
		this.rank = rank;
		this.idGame = idGame;
		arrayTextGame[idGame-1].SetActive(true);
		GetGameShareInfor();
		m_Animator.SetInteger(m_ValueAnimator,(int)AnimResault.ScoreAnim);
		m_IsScoreAnim = true;
		imgRank.GetComponent<Image>().sprite = GameResaultManager.Instance.rankSprite;
		if (cardBonus > 0) {
			cardBonusGo.SetActive (true);
			txtCardBonus.text = "+" + Mathf.RoundToInt(cardBonus) + "%";
		}

		if(haveCombo && haveStageClear){
			comboBonusMax.SetActive(true);
			txtTextScoreBonus.SetActive (true);
			txtTextComboMax.SetActive (true);
		}
		else if(haveCombo)
		{
			comboBonusMax.SetActive(true);
			txtComboTotal.SetActive(true);
			txtTextScoreBonus.SetActive(false);
			if (haveStageClear) {
				txtTextStageClear.SetActive (true);
				txtTextComboMax.SetActive (false);
			} else {
				txtTextStageClear.SetActive (false);
				txtTextComboMax.SetActive (true);
			}
		}else if(haveStageClear)
		{
			comboBonusMax.SetActive(true);
			txtComboTotal.SetActive(false);
			txtTextScoreBonus.SetActive(true);
			if (haveStageClear) {
				txtTextStageClear.SetActive (true);
				txtTextComboMax.SetActive (false);
			} else {
				txtTextStageClear.SetActive (false);
				txtTextComboMax.SetActive (true);
			}
		}
		else
		{
			comboBonusMax.SetActive(false);
			txtComboTotal.SetActive(false);
			txtTextScoreBonus.SetActive(true);
		}
	}


	private void RunAnimScore()
	{
		if((m_CountScoreAnim + m_PercentIncrease*m_Score) >= m_Score)
		{
			m_IsScoreAnim = false;
			m_IsScoreBonusAnim = true;
			ComponentConstant.SOUND_MANAGER.Play(SoundEnum.se08_score);
			txtScore.text = m_Score.ToString();
			txtScore.transform.localPosition = positionScoreDefault;
			m_CountScoreAnim = 0;
			m_Animator.SetInteger(m_ValueAnimator,(int)AnimResault.ScoreBonus);
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
			ComponentConstant.SOUND_MANAGER.Play(SoundEnum.se08_score);
			m_CountScoreAnim = 0;
			txtScoreBonus.text = m_ScoreBonus.ToString();
			txtScoreBonus.transform.localPosition = positionScoreBonusDefault;
			m_Animator.SetInteger(m_ValueAnimator,(int)AnimResault.ScoreBonusMax);
		}else
		{
			if((int)(m_PercentIncrease*m_ScoreBonus) <= 0)
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
			ComponentConstant.SOUND_MANAGER.Play(SoundEnum.se08_score);
			m_IsScoreTotalAnim = true;
			m_CountScoreAnim = 0;
			m_Animator.SetInteger(m_ValueAnimator,(int)AnimResault.ScoreTotal);
		}else
		{
			if((int)(m_PercentIncrease*m_ScoreBonusMax) <= 0)
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
			imgRank.SetActive(true);
			imgJugdment.SetActive(true);
			ComponentConstant.SOUND_MANAGER.Play(SoundEnum.se08_score);
			m_IsScoreTotalAnim = false;
			m_IsAnimRanking = true;
			m_Animator.SetInteger(m_ValueAnimator,(int)AnimResault.idle);
			ShowInfor();
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


	public void AnimShakingObject(GameObject text)
	{
		Vector2 shaking = UnityEngine.Random.insideUnitCircle * m_RadiusShaking;
		text.transform.localPosition = shaking;
	}

	public void ButtonBack()
	{
		ComponentConstant.SOUND_MANAGER.Play(SoundEnum.se02_ng);
		gameObject.SetActive(false);
		m_TimeAnimRank = 3f;
	}
}
