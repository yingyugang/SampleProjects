using UnityEngine;
using System.Collections;

namespace Swimming
{
	public enum TimeType
	{
		Morning,
		Evening,
		Night
	}

	public class Tree : MonoBehaviour 
	{

		public Sprite spriteMorning;
		public Sprite spriteEvening;
		public Sprite spriteNight;

		private SpriteRenderer m_Sprite;

		void Start()
		{
			m_Sprite = GetComponent<SpriteRenderer>();
		}

		void OnEnable()
		{
			m_Sprite = GetComponent<SpriteRenderer>();
		}

		public void SetSpriteTime(TimeType type)
		{
			switch (type)
			{
			case TimeType.Morning:
				m_Sprite.sprite = spriteMorning;
				break;
			case TimeType.Evening:
				m_Sprite.sprite = spriteEvening;
				break;
			case TimeType.Night:
				m_Sprite.sprite = spriteNight;
				break;
			}
		}

		public SpriteRenderer GetSprite()
		{
			return m_Sprite;
		}

		public float GetWorldPosY()
		{
			float y = transform.TransformPoint(Vector3.zero).y;
			return y;
		}
	}
}