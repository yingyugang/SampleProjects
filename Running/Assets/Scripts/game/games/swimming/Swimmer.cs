using UnityEngine;
using System.Collections;
using DG.Tweening;
namespace Swimming
{
	public enum AnimState : int
	{
		Idle = 0,
		Swimming = 1,
		Blinking = 2
	}

	public class Swimmer : MonoBehaviour 
	{
		public const string ANIM_STATE_NAME = "State";

		public const float TIME_BLINKING = 3f;
		public const float LEFT_LIMIT = -3.3f;
		public const float RIGHT_LIMIT = 3.3f;
		public const float TOP_LIMIT = 6.3f;
		public const float BOTTOM_LIMIT = -6.3f;
		public const float MAX_SPEED = 3f;

		public int numTreeItem;
		public int numRiceItem;
		public int numOdenItem;

		public GameObject gfxImmortal;
		public GameObject cutIn;

		public bool isCheat = false;

		private float m_SpeedMove;
		private float m_Speed;

		private bool m_IsImmortalMode;
		private float m_Distance;
		private float m_TimeImmortal;
		private float m_TimeMaxImmortal;

		private static Swimmer m_Instance;
		public static Swimmer Instance
		{
			get
			{
				return m_Instance;
			}
		}

		private bool m_NeedMove;
		private Vector3 m_BeginPoint;
		private Vector3 m_StartPoint;
		private Vector3 m_EndPoint;
		private Vector3 m_Target = new Vector3(0, 0, 0);

		private int m_Life;

		private bool m_IsBlinking;

		private Animator m_Animator;

		private int[] m_HelpersCounter = new int[(int)HelperType.Count];

		void Awake()
		{
			m_Instance = this;
		}

		// Use this for initialization
		void Start () 
		{
			Init();
		}

		public void Init()
		{
			m_Animator = GetComponent<Animator>();

			m_SpeedMove = 20f;
			m_Speed = GameParams.Instance.playerSpeed;

			if (m_Speed > MAX_SPEED)
			{
				GameParams.Instance.playerSpeed = MAX_SPEED;
				m_Speed = MAX_SPEED;
			}
			
			m_Life = GameParams.Instance.playerLifes;
			m_IsBlinking = false;
			m_IsImmortalMode = false;

			m_NeedMove = false;

			numTreeItem = 0;
			numRiceItem = 0;
			numOdenItem = 0;

			m_Distance = 0;

			Header.Instance.SetLife(LifeType.Number, m_Life);

			m_EndPoint = transform.position;

			gfxImmortal.SetActive(false);

			Idle();

			ResetHelperCounter();
		}

		public void Play()
		{
			//m_Animator.CrossFade("Swimming", 0f);
			m_Animator.SetInteger(ANIM_STATE_NAME, (int) AnimState.Swimming);
		}

		public void Idle()
		{
			//m_Animator.CrossFade("Idle", 0f);
			m_Animator.SetInteger(ANIM_STATE_NAME, (int) AnimState.Idle);
		}

		// HELPER
		void ResetHelperCounter()
		{
			for (int i=0; i<m_HelpersCounter.Length; i++)
				m_HelpersCounter[i] = 0;

			GameFooter.Instance.Init();
		}

		public void IncreaseHelper(HelperType type)
		{
			m_HelpersCounter[(int)type] ++;

			GameFooter.Instance.SetHelperItemCount(type, m_HelpersCounter[(int)type]);
		}

		
		// Update is called once per frame
		void Update () 
		{
			if (GameManager.Instance.isPaused)
				return;

			if (m_NeedMove)
			{
				transform.position = Vector3.MoveTowards(transform.position, m_Target, m_SpeedMove * Time.deltaTime);
				//transform.position = m_Target;
			}
			
			UpdateDistance();
			UpdateImmortal();
		}

		void UpdateImmortal()
		{
			if(m_IsImmortalMode)
			{
				if(m_TimeImmortal <= 0)
				{
					//GameFooter.Instance.SetAnimEffectStarItem(false);
					//m_IsImmortalMode = false;
					GameManager.Instance.StopImmortalMode();
				}
				else
				{
					m_TimeImmortal -= Time.deltaTime;
					float value = m_TimeImmortal/m_TimeMaxImmortal * 100;
					GameFooter.Instance.SetSliderValue(value);
				}
			}
		}

		public float GetDistance()
		{
			return m_Distance;
		}

		public int GetTotalItem()
		{
			return numTreeItem + numOdenItem + numRiceItem;
		}

		void UpdateDistance()
		{
			float ratio = GameManager.Instance.GetSpeedUpRatio() * SwimmingConfig.SPEED_PER_UNIT;
			float speed = m_Speed; //SwimmingConfig.BG_SPEED

			if (IsImmortal())
			{
				float maxDistance = m_Speed * 1000f ;
				//float immortalRatio = maxDistance / GameParams.Instance.GetImmortalTime();
				//m_Distance += Time.deltaTime * speed * ratio * SwimmingConfig.IMMORTAL_FACTOR;
				//m_Distance += Time.deltaTime * immortalRatio;
				//-->|edited by anhgh
				m_Distance += Time.deltaTime * ratio * speed * SwimmingConfig.IMMORTAL_FACTOR;
				//--<|edited by anhgh
			}
			else
			{
				//if (isCheat)
					//m_Distance += Time.deltaTime * SwimmingConfig.BG_SPEED * 10;
				//else
				m_Distance += Time.deltaTime * ratio * speed;
			}

			int distance = (int) m_Distance;
			Header.Instance.SetScore(distance + "  m");
		}

		public void OnPointerDown()
		{
			m_NeedMove = true;

			m_StartPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			m_StartPoint.z = 0;

			m_BeginPoint = m_StartPoint;

			m_EndPoint = m_StartPoint;
			m_Target = transform.position;

		}

		public void OnPointerUp()
		{
			//m_EndPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			//m_EndPoint.z = 0;

			m_NeedMove = false;
		}

		public void OnDrag()
		{
			m_EndPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			m_EndPoint.z = 0;

			float flickSensitivity = GameParams.Instance.flickSensitivity;
			flickSensitivity /= 100f;
			Vector3 vd = m_EndPoint - m_BeginPoint;
			float distance = vd.magnitude;
			if (distance < flickSensitivity)
				return;

			Vector3 v = m_EndPoint - m_StartPoint;
			m_Target = transform.position;
			m_Target += v;

			if (m_Target.x > RIGHT_LIMIT)
				m_Target.x = RIGHT_LIMIT;

			if (m_Target.x < LEFT_LIMIT)
				m_Target.x = LEFT_LIMIT;

			if (m_Target.y > TOP_LIMIT)
				m_Target.y = TOP_LIMIT;

			if (m_Target.y < BOTTOM_LIMIT)
				m_Target.y = BOTTOM_LIMIT;

			m_StartPoint = m_EndPoint;
		}

		public void Hurt()
		{
			if (m_IsBlinking)
				return;

			if (m_IsImmortalMode)
				return;

			if (isCheat)
				return;
			
			m_Life --;
			if (m_Life < 0)
				m_Life = 0;
			
			ComponentConstant.SOUND_MANAGER.Play(SoundEnum.se13_miss);
			Header.Instance.UpdateLife(m_Life);

			if (m_Life > 0 )
			{
				//m_Animator.CrossFade("Blinking", 0f);
				m_Animator.SetInteger(ANIM_STATE_NAME, (int) AnimState.Blinking);
				m_IsBlinking = true;
				Invoke("HideBlinking", TIME_BLINKING);
				//SoundManager.Instance.PlaySfx(SoundType.Hurt);
			}
			else
			{
				GameManager.Instance.PreGameOver();
			}
		}

		public void HideBlinking()
		{
			m_IsBlinking = false;
			//m_Animator.CrossFade("Swimming", 0f);
			m_Animator.SetInteger(ANIM_STATE_NAME, (int) AnimState.Swimming);
		}

		public void GetImmortalItem()
		{
			ComponentConstant.SOUND_MANAGER.Play(SoundEnum.se12_bonusitem);

			if(IsImmortal())
				return;
			
			GameManager.Instance.OnPause();
			ComponentConstant.SOUND_MANAGER.StopBGM();
			ComponentConstant.SOUND_MANAGER.Play(SoundEnum.bgm10_invicible, GameManager.Instance.GetAudioSource);
			cutIn.SetActive(true);
			StartCoroutine("HideCutIn");
		}

		public IEnumerator HideCutIn()
		{
			yield return StartCoroutine(WaitForRealTime(2f));
			cutIn.SetActive(false);
			GameManager.Instance.OnResume();
			StartImmortalMode();
		}
			

		public void StartImmortalMode()
		{
			m_IsImmortalMode = true;
			//m_Animator.CrossFade("Blinking", 0f);
			m_Animator.SetInteger(ANIM_STATE_NAME, (int) AnimState.Swimming);

			GameFooter.Instance.SetAnimEffectStarItem(true);
			gfxImmortal.SetActive(true);
			GameManager.Instance.StartImmortalMode();
			m_TimeImmortal = GameParams.Instance.GetItemDataByType(ItemType.Immortal).effectValue;
			m_TimeMaxImmortal = m_TimeImmortal;
		}

		public void StopImmortalMode()
		{
			m_IsImmortalMode = false;
			gfxImmortal.SetActive(false);
			GameFooter.Instance.SetAnimEffectStarItem(false);
			HideBlinking();
		}

		public bool IsImmortal()
		{
			return m_IsImmortalMode;
		}

		public Vector3 GetPosition()
		{
			return transform.position;
		}

		public void UpdateScore()
		{
			if (numTreeItem > 99)
				numTreeItem = 99;
			if (numOdenItem > 99)
				numOdenItem = 99;
			if (numRiceItem > 99)
				numRiceItem = 99;
			
			GameFooter.Instance.SetTree(numTreeItem);
			GameFooter.Instance.SetOden(numOdenItem);
			GameFooter.Instance.SetRice(numRiceItem);
		}

		public int GetTotalScore()
		{
			int distance = (int) (m_Distance * GameParams.Instance.comboVar);
			int scoreOden = numOdenItem * GameParams.Instance.GetItemDataByType(ItemType.Oden).effectValue;
			int scoreTree = numTreeItem * GameParams.Instance.GetItemDataByType(ItemType.Tree).effectValue;
			int scoreRice = numRiceItem * GameParams.Instance.GetItemDataByType(ItemType.Rice).effectValue;
			return distance + scoreOden + scoreTree + scoreRice;
		}

		public int GetScoreBonus()
		{
			//int distance = (int) m_Distance;
			int scoreOden = numOdenItem * GameParams.Instance.GetItemDataByType(ItemType.Oden).effectValue;
			int scoreTree = numTreeItem * GameParams.Instance.GetItemDataByType(ItemType.Tree).effectValue;
			int scoreRice = numRiceItem * GameParams.Instance.GetItemDataByType(ItemType.Rice).effectValue;
			return scoreOden + scoreTree + scoreRice;
		}

		public static IEnumerator WaitForRealTime(float delay)
		{
			while (true)
			{
				float pauseEndTime = Time.realtimeSinceStartup + delay;
				while (Time.realtimeSinceStartup < pauseEndTime)
				{
					yield return 0;
				}
				break;
			}
		}
	}

}
