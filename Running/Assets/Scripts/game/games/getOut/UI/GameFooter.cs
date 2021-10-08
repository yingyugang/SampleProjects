using UnityEngine;
using UnityEngine.UI;
using System.Collections;
public class GameFooter : MonoBehaviour {
	// Use this for initialization
	private static GameFooter _instance;
	public GameObject particleStar;
	public GameObject particleClock;
	public Text textScore1;
	public Text textScore2;
	public Text textScore3;
	public Slider slider;
	public float percentValueSlider;
	private Animator m_Animator;

	public static GameFooter Instance()
	{
			return _instance;
	}

	void Awake()
	{
		_instance = this;
	}

	public void Init () {
		SetNumberScore1Text();
		SetNumberScore2Text();
		SetNumberScore3Text();
		m_Animator = gameObject.GetComponent<Animator>();
	}



	public void SetSliderEffectValue(float value,float effectValue)
	{
		value = value/effectValue;
		slider.value = value;
	}

	public void SetAnimEffectStarItem(bool isEffect)
	{
		if(isEffect)
		{
			m_Animator.SetBool(GameConfig.VALUE_ANIMATORSTARFOOTER,true);
		}else
			m_Animator.SetBool(GameConfig.VALUE_ANIMATORSTARFOOTER,false);
	}

	public void SetAnimEffectClockItem(bool isEffect)
	{
		if(isEffect)
		{
			m_Animator.SetBool(GameConfig.VALUE_ANIMATORCLOCKFOOTER,true);
		}else
			m_Animator.SetBool(GameConfig.VALUE_ANIMATORCLOCKFOOTER,false);
	}

	public void SetNumberScore2Text()
	{
		textScore2.text = GetOut.GameManager.Instance().numberItemScore2.ToString();
	}

	public void SetNumberScore1Text()
	{
		textScore1.text = GetOut.GameManager.Instance().numberItemScore1.ToString();
	}

	public void SetNumberScore3Text()
	{
		textScore3.text = GetOut.GameManager.Instance().numberItemScore3.ToString();
	}


	public void PauseParticle()
	{
		particleStar.SetActive(false);
		particleClock.SetActive(false);
	}

	public void UnPauseParticle()
	{
		particleStar.SetActive(true);
		particleClock.SetActive(true);
	}
}
