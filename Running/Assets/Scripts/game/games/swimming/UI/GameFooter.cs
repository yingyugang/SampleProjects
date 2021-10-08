using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

namespace Swimming 
{
	[Serializable]
	public class HelperImage
	{
		public HelperType type;
		public HelperItem helperItem;
	}

	public class GameFooter : MonoBehaviour 
	{
		public Text txtTree;
		public Text txtOden;
		public Text txtRice;

		public Slider slider;

		public GameObject particle;

		public HelperImage[] helpers;

		public Sprite helper_cheat;

		private Animator m_Animator;

		private static GameFooter m_Instance;
		public static GameFooter Instance
		{
			get
			{
				return m_Instance;
			}
		}

		void Awake()
		{
			m_Instance = this;
		}

		// Use this for initialization
		void Start () 
		{
			m_Animator = gameObject.GetComponent<Animator>();
		}
		
		public void Init()
		{
			ResetItemScore();
			ResetHelperCounter();
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

		public void ResetHelperCounter()
		{
			foreach( var helper in helpers)
			{
				helper.helperItem.Reset();
				if (CheatController.IsCheated (0) && helper.type == HelperType.KyoshiSawa) {
					helper.helperItem.imgHelper.sprite = helper_cheat;
				}
			}
		}

		public HelperItem GetHelperItemByType(HelperType type)
		{
			foreach (var v in helpers)
			{
				if (v.type == type)
					return v.helperItem;
			}

			return null;
		}

		public void SetHelperItemCount(HelperType type, int count)
		{
			HelperItem helperItem = GetHelperItemByType(type);
			helperItem.SetCount(count);
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
			particle.SetActive(visible);
		}
	}
}