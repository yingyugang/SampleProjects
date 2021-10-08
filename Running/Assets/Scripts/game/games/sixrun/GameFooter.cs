using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace SixRun{
	
	public class GameFooter : SixRunSingleMono<GameFooter> {

		public Text txtTree;
		public Text txtOden;
		public Text txtRice;


		public GameObject left;

		public Slider slider;

		public GameObject particle;

		private Animator m_Animator;

		// Use this for initialization
		void Start () 
		{
			m_Animator = gameObject.GetComponent<Animator>();
		}

		public void Init()
		{
			ResetItemScore();
		}

		public void SetTree(int value)
		{
			txtTree.text = value.ToString();
		}

		public void SetOden(int value)
		{
			txtOden.text = value.ToString();
		}

		public void SetRice(int value)
		{
			txtRice.text = value.ToString();
		}

		public void ResetItemScore()
		{
			txtTree.text = "0";
			txtOden.text = "0";
			txtRice.text = "0";
		}

		public void SetAnimEffectStarItem(bool isEffect)
		{
			if (isEffect)
				m_Animator.SetBool(SwimmingConfig.VALUE_ANIMATORSTARFOOTER, true);
			else
				m_Animator.SetBool(SwimmingConfig.VALUE_ANIMATORSTARFOOTER, false);
		}

		public void SetSliderValue(float value)
		{
			slider.value = value;
		}

		public void SetParticleVisible(bool visible)
		{
			Debug.Log ("SetParticleVisible");
			particle.SetActive(visible);
			slider.gameObject.SetActive (visible);
			left.SetActive (!visible);
		}
	}
}
