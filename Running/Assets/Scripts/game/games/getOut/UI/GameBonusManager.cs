using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine.UI;
public class LevelExp{
	public int level;
	public int exp;
	public LevelExp(int level, int exp)
	{
		this.level = level;
		this.exp = exp;
	}
}




public class GameBonusManager : MonoBehaviour {

	public GameObject[] arrayTextHeader;
	public Sprite[] arraySpriteItemClear;
	public Sprite[] arraySpriteHeader;
	public Slider sliderExp;
	public GameObject objFirstClearGacha;
	public GameObject objClearGacha;
	public Text textClearGachaChildOfFirstClear;
	public Text textFirstClearGacha;
	public Text txtExp;
	public Text txtExpRemain;
	public Text txtLevel;
	public Text txtFirstClear;
	public Text txtClearBonus;
	public Text txtGacha;
	public Text txtTextDescriptionGaCha;
	public Text txtTextLevelMax;
	public GameObject txtTextLevelUp;
	public Image imageFirstClearItem;
	public Image imageClearBonusItem;
	public Image imageHeader;
	public int level;
	public int lastLevel;
	public int exp;
	public int lastExp;
	private bool m_IsAnimExp;
	private bool m_IsAnimSlider;
	private bool m_IsAnimFirstClear;
	private bool m_IsAnimClearBonus;
	private bool m_IsAnimTextGacha;
	private bool m_IsAnimLevelMax;
	private LevelExp m_NextLevel;
	private LevelExp m_LastLevel;
	private LevelExp[] m_ArrayLevelExp;
	private int m_CountScoreAnim;
	private int m_CountExpAnim;
	private int m_expClone;
	private int m_FirstClear;
	private int m_ClearBonus;
	private Vector3 m_PositionExpDefaultl ;
	private float m_RadiusShaking = 5f;
	private float m_TimeAnimGacha = 3f;
	private float m_TimeAnimLevelMax = 3f;




	private float m_PercentIncrease = 0.02f;

	public const string CSV_LEVEL_EXP = "m_player_rank";

	public void SetLastLevel(int level,int lastExp)
	{
		lastLevel = level;
		this.lastExp = lastExp;
	}


	// Update is called once per frame
	void Update () {
		if(m_IsAnimExp)
		{
			RunAnimExp();
		}
		if(m_IsAnimFirstClear)
		{
			RunAnimFirstClear();
		}
		if(m_IsAnimClearBonus)
		{
			RunAnimClear();
		}
		if(m_IsAnimTextGacha)
		{
			if(m_TimeAnimGacha <= 0)
			{
				m_IsAnimTextGacha = false;
			}
			AnimShakingObject(txtGacha.gameObject);
			AnimShakingObject(txtTextDescriptionGaCha.gameObject);
			m_TimeAnimGacha -= 0.01f;
		}

		if (m_IsAnimLevelMax) {
			m_TimeAnimLevelMax -= Time.deltaTime;
			if (m_TimeAnimLevelMax > 0) {
				AnimShakingObject (txtTextLevelUp.gameObject);
				AnimShakingObject (txtTextLevelMax.gameObject);
			} else
			{
				m_IsAnimLevelMax = false;
			}
		}
	}


	public void InitPanel(int id)
	{
		LoadCsvExpFolder();
		level = UpdateInformation.GetInstance.player.lv;
		Debug.Log("First = "+APIInformation.GetInstance.first_reward_info.reward_num);
		m_expClone = exp = UpdateInformation.GetInstance.player.exp;
		m_FirstClear = APIInformation.GetInstance.first_reward_info.reward_num;
		m_ClearBonus = APIInformation.GetInstance.clear_reward_info.reward_num;
		Debug.Log("Init first clear = "+m_FirstClear);
		arrayTextHeader[id-1].SetActive(true);
		imageHeader.sprite = arraySpriteHeader[id-1];
		gameObject.SetActive(true);
		txtLevel.text = level+"";


		if(lastLevel < m_ArrayLevelExp[m_ArrayLevelExp.Length-1].level)
			m_NextLevel = GetLevelExpInfor(lastLevel+1);
		else
			m_NextLevel = GetLevelExpInfor(lastLevel);	
		m_LastLevel = GetLevelExpInfor (lastLevel);
		sliderExp.value = (lastExp - m_NextLevel.exp)*1.0f/(m_NextLevel.exp - m_LastLevel.exp);

		m_expClone -= m_LastLevel.exp;
		m_CountExpAnim = lastExp;
		m_CountScoreAnim = lastExp - m_LastLevel.exp;
		if(APIInformation.GetInstance.first_reward_info.reward_num > 0)
		{
			objFirstClearGacha.SetActive(true);
			textClearGachaChildOfFirstClear.gameObject.SetActive(false);
			textFirstClearGacha.gameObject.SetActive(true);
			if(m_ClearBonus > 0)
			{
				objClearGacha.SetActive(true);
			}else
				objClearGacha.SetActive(false);
		}
		else
		{
			if(m_ClearBonus > 0)
			{
				objFirstClearGacha.SetActive(true);
				textClearGachaChildOfFirstClear.gameObject.SetActive(true);
				textFirstClearGacha.gameObject.SetActive(false);
				m_FirstClear = m_ClearBonus;
				objClearGacha.SetActive(false);
			}else
			{
				objFirstClearGacha.SetActive(false);
				objClearGacha.SetActive(false);
			}
		}
		m_IsAnimExp = true;
	}


	private void LoadCsvExpFolder()
	{
		GameShareInfor gameShareInfor;
		List<Dictionary<string, object>> data = CSVReader.Read("csv/" + CSV_LEVEL_EXP);
		if (data != null)
		{
			m_ArrayLevelExp = new LevelExp[data.Count];
			Dictionary<string,float> dataParam = new Dictionary<string, float>();
			for (int i = 0; i < data.Count; i++)
			{
				int	id = (int)data[i]["id"];
				int level = (int)data[i]["level"];
				int exp =  (int)data[i]["exp"];
				m_ArrayLevelExp[i] = new LevelExp(level,exp);
			}
		}
	}


	private LevelExp GetLevelExpInfor(int level)
	{
		for(int i = 0; i < m_ArrayLevelExp.Length; i++)
		{
			if(m_ArrayLevelExp[i].level == level)
			{
				return m_ArrayLevelExp[i];
			}
		}
		return null;
	}

	private void RunAnimExp()
	{
		//if anim count exp and exp remain is over
		if((m_CountExpAnim + m_PercentIncrease*exp) >= exp && (m_CountScoreAnim + m_PercentIncrease*m_expClone) >= m_expClone && m_NextLevel.level >= level)
		{
			//if level up
			txtExpRemain.gameObject.transform.localPosition = Vector3.zero;
			txtExp.gameObject.transform.localPosition = Vector3.zero;
			//if the it's the last level
			if (lastLevel == m_ArrayLevelExp [m_ArrayLevelExp.Length - 1].level) {
				m_NextLevel = GetLevelExpInfor (lastLevel);
				m_LastLevel = m_NextLevel;
				txtExpRemain.text = "0";
			} else {
				if (exp >= (m_NextLevel.exp)) {
					m_expClone -= (m_NextLevel.exp - m_LastLevel.exp);
					lastLevel++;
					m_LastLevel = m_NextLevel;
					m_NextLevel = GetLevelExpInfor (lastLevel+1);
					ComponentConstant.SOUND_MANAGER.Play(SoundEnum.se10_levelup);
					txtTextLevelUp.SetActive(true);
					m_IsAnimLevelMax = true;
				}
				txtExpRemain.text = "" + (m_NextLevel.exp - exp);
			}
			//Set anim count exp is false
			m_IsAnimExp = false;
			txtExp.transform.localPosition = m_PositionExpDefaultl;
			m_CountScoreAnim = 0;
			m_CountExpAnim = 0;
			//Start anim clear gacha
			m_IsAnimFirstClear = true;

			//Infor clear Gacha 

			if(APIInformation.GetInstance.first_reward_info.reward_num > 0)
			{
				imageFirstClearItem.gameObject.SetActive(true);
				imageFirstClearItem.sprite = arraySpriteItemClear[APIInformation.GetInstance.first_reward_info.m_item_id-1];
			}else
			{
				imageFirstClearItem.sprite = arraySpriteItemClear [APIInformation.GetInstance.clear_reward_info.m_item_id - 1];
				imageFirstClearItem.gameObject.SetActive (true);
			}
				
			//Set text for level, slider exp and exp remain
			txtLevel.text = m_LastLevel.level+"";

			sliderExp.value = (m_expClone)*1.0f/(m_NextLevel.exp - m_LastLevel.exp);
			txtExp.text = exp.ToString();
		}else
		{
			//inscrease exp and exp remain
			if((int)(m_PercentIncrease*exp) <= 0)
			{
				if(m_CountExpAnim < exp)
					m_CountExpAnim += 1;
				
			}else
			{
				if((m_CountExpAnim+(m_PercentIncrease*exp)) < exp)
					m_CountExpAnim += (int)(m_PercentIncrease*exp);
			}
			if((int)(m_PercentIncrease*m_expClone) <= 0)
			{
				if(m_CountScoreAnim < m_expClone)
					m_CountScoreAnim += 1;
			}else
			{
				if((m_CountScoreAnim+(m_PercentIncrease*m_expClone)) < m_expClone)
					m_CountScoreAnim += (int)(m_PercentIncrease*m_expClone);
			}
			txtExp.text = m_CountExpAnim.ToString();
			txtExpRemain.text = ""+(m_NextLevel.exp - m_LastLevel.exp - m_CountScoreAnim);
			ComponentConstant.SOUND_MANAGER.Play(SoundEnum.se09_gauge);
			if(m_NextLevel != null)
			{
				//Check level up
				CheckLevelUp();
				sliderExp.value = m_CountScoreAnim*1.0f/(m_NextLevel.exp-m_LastLevel.exp);
				txtLevel.text = m_LastLevel.level.ToString();
			}
			AnimShakingObject(txtExpRemain.gameObject);
			AnimShakingObject(txtExp.gameObject);
		}
	}

	//Function check level up
	void CheckLevelUp()
	{
		//if it's level max
		if(m_NextLevel.level == m_ArrayLevelExp[m_ArrayLevelExp.Length-1].level)
		{
			m_TimeAnimLevelMax = 3f;
			txtTextLevelMax.gameObject.SetActive(true);
			txtTextLevelUp.SetActive(false);
		}
		//level up
		if((m_NextLevel.exp - m_CountScoreAnim-m_LastLevel.exp) <= 0 && m_LastLevel.level != m_ArrayLevelExp[m_ArrayLevelExp.Length-1].level)
		{
			m_IsAnimLevelMax = true;
			if(m_NextLevel.level == m_ArrayLevelExp[m_ArrayLevelExp.Length-1].level)
			{
				m_TimeAnimLevelMax = 3f;
				txtTextLevelMax.gameObject.SetActive(true);
				txtTextLevelUp.SetActive(false);
			}else
			{
				txtTextLevelUp.SetActive(true);
				txtTextLevelMax.gameObject.SetActive(false);
			}
			m_TimeAnimLevelMax = 3f;
			m_expClone -= (m_NextLevel.exp-m_LastLevel.exp);
			if(lastLevel < m_ArrayLevelExp[m_ArrayLevelExp.Length-1].level)
				lastLevel +=1;
			m_LastLevel = m_NextLevel;
			if(lastLevel <= m_ArrayLevelExp[m_ArrayLevelExp.Length-1].level)
				m_NextLevel = GetLevelExpInfor(lastLevel);
			ComponentConstant.SOUND_MANAGER.Play(SoundEnum.se10_levelup);
			m_CountScoreAnim = 0;
		}
	}

	//Anim first clear gacha
	void RunAnimFirstClear()
	{
		if((m_CountScoreAnim + m_PercentIncrease*m_FirstClear) >= m_FirstClear)
		{
			txtFirstClear.text = "x"+m_FirstClear.ToString();
			txtFirstClear.transform.localPosition = Vector3.zero;
			m_IsAnimFirstClear = false;
			m_IsAnimClearBonus = true;
			imageClearBonusItem.gameObject.SetActive(false);
			if (APIInformation.GetInstance.first_reward_info.reward_num > 0) {
				imageClearBonusItem.sprite = arraySpriteItemClear [APIInformation.GetInstance.clear_reward_info.m_item_id - 1];
				imageClearBonusItem.gameObject.SetActive (true);
				APIInformation.GetInstance.first_reward_info.reward_num = 0;
			} else {
				imageFirstClearItem.sprite = arraySpriteItemClear [APIInformation.GetInstance.clear_reward_info.m_item_id - 1];
				imageFirstClearItem.gameObject.SetActive (true);
			}
			//else
			//	imageClearBonusItem.gameObject.SetActive(false);
			txtFirstClear.gameObject.transform.localPosition = Vector3.zero;
			m_CountScoreAnim = 0;
		}else
		{
			if((int)(m_PercentIncrease*m_FirstClear) <= 0 && m_FirstClear != 0)
			{
				m_CountScoreAnim += 1;
			}else
			{
				m_CountScoreAnim += (int)(m_PercentIncrease*m_FirstClear);
			}

			txtFirstClear.text = "x"+m_CountScoreAnim.ToString();
			AnimShakingObject(txtFirstClear.gameObject);
		}
	}

	//Anim clear gacha
	void RunAnimClear()
	{
		if((m_CountScoreAnim + m_PercentIncrease*m_ClearBonus) >= m_ClearBonus)
		{
			txtClearBonus.text ="x"+m_ClearBonus.ToString();
			txtClearBonus.transform.localPosition = Vector3.zero;
			m_IsAnimClearBonus = false;
			ShowTextGacha();
			m_CountScoreAnim = 0;
			txtClearBonus.gameObject.transform.localPosition = Vector3.zero;
		}else
		{
			if((int)(m_PercentIncrease*m_ClearBonus) <= 0 && m_ClearBonus != 0)
			{
				m_CountScoreAnim += 1;
			}else
			{
				m_CountScoreAnim += (int)(m_PercentIncrease*m_ClearBonus);
			}
			txtClearBonus.text = "x"+m_CountScoreAnim.ToString();
			AnimShakingObject(txtClearBonus.gameObject);
		}
	}

	//Text gacha
	void ShowTextGacha()
	{
		txtGacha.supportRichText = true;
	
		if(APIInformation.GetInstance.next_clear_gacha.ToString() != "")
		{
			txtGacha.gameObject.SetActive(true);
			txtGacha.text = APIInformation.GetInstance.next_clear_gacha.ToString()+txtGacha.text;
			txtTextDescriptionGaCha.gameObject.SetActive(true);
		}else
		{
			txtGacha.gameObject.SetActive(false);
			txtTextDescriptionGaCha.gameObject.SetActive(false);
		}
		m_IsAnimTextGacha = true;
	}

	//Animation shaking object
	public void AnimShakingObject(GameObject text)
	{
		Vector2 shaking = UnityEngine.Random.insideUnitCircle * m_RadiusShaking;
		text.transform.localPosition = shaking;
	}



}
