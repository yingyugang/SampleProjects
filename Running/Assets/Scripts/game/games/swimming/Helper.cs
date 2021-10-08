using UnityEngine;
using System.Collections;
using System;

namespace Swimming
{
	public enum HelperAlign
	{
		Left = 1,
		Right
	}

	public enum HelperType
	{
		Osomatsu,
		Karamatsu,
		Chocomatsu,
		Ichimatsu,
		Todomatsu,
		Dayon,
		Dekapan,
		Chibita,
		Iyami,
		Totoko,
		Hatabou,
		KyoshiSawa,
		Count
	}

	[Serializable]
	public class HelperSprite
	{
		public HelperType type;
		public Sprite sprite;
	}

	public class Helper : MonoBehaviour 
	{
		public const float LEFT_POS = -4.7f;
		public const float RIGHT_POS = 4.7f;
		public const float TIME_REACTIVE_TREE = 4f;

		public HelperType type;
		public TimeType bgTime;
		public SpriteRenderer spriteCharacter;
		public GameObject leftLine;
		public GameObject rightLine;

		public HelperSprite[] sprites;

		public Sprite helper_cheat;

		protected float m_Speed;

		private static Helper m_Instance;
		public static Helper Instance
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
		protected virtual void Start () 
		{
			
		}

		protected virtual void OnEnable () 
		{
			
		}

		// Update is called once per frame
		protected virtual void Update () 
		{
			if (GameManager.Instance.IsPause ())
				return;
			
			UpdateMovement();
		}

		void UpdateMovement()
		{
			transform.position += m_Speed * Time.deltaTime * Vector3.down;
		}

		protected virtual void Init()
		{
			m_Speed = GameParams.Instance.GetRelativelyRiverSpeed();
			m_Speed *= GameManager.Instance.GetSpeedUpRatio();
			if (Swimmer.Instance.IsImmortal())
				IncreaseSpeed();
		}

		protected virtual void OnTriggerEnter2D(Collider2D other)
		{
			if (other.CompareTag(SwimmingConfig.TAG_PLAYER))
			{
				DoHelp();
			}
		}

		protected virtual void DoHelp()
		{
			Debug.Log("DoHelp " + type.ToString());
			GameManager.Instance.SetCurrentTime(bgTime);
			Swimmer.Instance.IncreaseHelper(type);
			//Destroy(gameObject);
			gameObject.SetActive(false);
			//add by sya 20160329
			//ComponentConstant.SOUND_MANAGER.Play(SoundEnum.se14_alert);
			ComponentConstant.SOUND_MANAGER.Play(SoundEnum.SE32_bike_hitchhike);
			float timeReset = Swimmer.Instance.IsImmortal() ? TIME_REACTIVE_TREE/2 :  TIME_REACTIVE_TREE;
			Invoke("ActiveTree", timeReset);
		}

		void ActiveTree()
		{
			ScrollingScript.Instance.ActiveAllTree();
		}

		public void IncreaseSpeed()
		{
			m_Speed *= SwimmingConfig.IMMORTAL_FACTOR;
		}

		public void DecreaseSpeed()
		{
			m_Speed /= SwimmingConfig.IMMORTAL_FACTOR;
		}

		public void Init(HelperType type, float posY)
		{

			this.type = type;
			HelperData data = GameParams.Instance.GetHelperDataByType(type);

			HelperAlign align = data.align;
			this.bgTime = data.bgTime;

			float x = align == HelperAlign.Right ? RIGHT_POS : LEFT_POS;
			float y = posY;

			transform.position = new Vector2(x, y);

			if (align == HelperAlign.Left)
			{
				leftLine.SetActive(false);
				rightLine.SetActive(true);
			}
			else
			{
				leftLine.SetActive(true);
				rightLine.SetActive(false);
			}

			Debug.Log(type.ToString());
			spriteCharacter.sprite = GetSpriteByType(type);
			if(CheatController.IsCheated(0) && type == HelperType.KyoshiSawa){
				spriteCharacter.sprite = helper_cheat;
			}
			Init();
		}

		Sprite GetSpriteByType (HelperType type)
		{
			//Debug.Log("WTF " + sprites.Length);
			foreach (var v in sprites) {
				if (v.type == type)
					return v.sprite;
			}


			return null;
		}
	}
}
