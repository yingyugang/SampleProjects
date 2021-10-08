using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Swimming
{

	public enum EnemyType
	{
		Rock1,
		Rock2,
		Rock3,
		Wood,
		Fish1,
		Fish2,
		JellyFish,
		Boat1,
		Boat2
	}

	public enum MovementType
	{
		StraightDown = 1,
		CurveDown,
		ZigZag,
		StraightUp,
		CurveUp
	}

	public class Enemy : MonoBehaviour 
	{
		public EnemyType type;

		protected Animator m_Animator;

		protected float m_Speed;
		protected float m_RotationSpeed;
		protected Vector3 m_Target;
		protected List<Vector3> m_PathFollow;
		protected int m_CurrentNode;
		protected MovementType m_MovementType;

		// Use this for initialization
		protected virtual void Start () 
		{
			m_Animator = GetComponent<Animator>();
		}

		void OnEnable()
		{
			//if (!GameParams.Instance.isLoaded)
				//return;
			
			//Init();
		}

		public virtual void Init()
		{
			m_Animator = GetComponent<Animator>();

			//m_Speed = GameParams.Instance.GetRelativelyRiverSpeed();
			m_RotationSpeed = 0.1f * SwimmingConfig.SPEED_PER_UNIT;

			SetMovementType();
			RandomPosition();
			InitPath();

			SetupSpeed();

			SetAnimation();
		}

		public virtual void SetupSpeed()
		{
			float speed = GameParams.Instance.obstacles[type].speed;
			float riverSpeed = GameParams.Instance.riverSpeed;

			//m_Speed = GameParams.Instance.playerSpeed;
			float playerSpeed = GameParams.Instance.playerSpeed;

			if (m_MovementType == MovementType.StraightUp
			    || m_MovementType == MovementType.CurveUp) {
				m_Speed = speed - riverSpeed;
			} else if (speed > 1)
				m_Speed = playerSpeed + speed;
			else
				m_Speed = playerSpeed;
			
			m_Speed *= SwimmingConfig.SPEED_PER_UNIT;
			m_Speed *= GameManager.Instance.GetSpeedUpRatio();

			if (m_MovementType == MovementType.StraightUp)
				m_Speed *= -1;

			if (Swimmer.Instance.IsImmortal())
				IncreaseSpeed();
		}

		public void SetAnimation()
		{
			var time = GameManager.Instance.GetTimeType();
			switch(time)
			{
			case TimeType.Morning:
				m_Animator.CrossFade(SwimmingConfig.ANIMATION_MORNING, 0f);
				break;
			case TimeType.Evening:
				m_Animator.CrossFade(SwimmingConfig.ANIMATION_AFTERNOON, 0f);
				break;
			case TimeType.Night:
				m_Animator.CrossFade(SwimmingConfig.ANIMATION_NIGHT, 0f);
				break;
			default:
				m_Animator.CrossFade(SwimmingConfig.ANIMATION_MORNING, 0f);
				break;
			}
		}

		protected virtual void RandomPosition()
		{
			bool isTop = (m_MovementType == MovementType.StraightDown || m_MovementType == MovementType.CurveDown || m_MovementType == MovementType.ZigZag) ? true : false;

			float x = Random.Range(-2.5f, 2.5f);
			float y = isTop ? 12f : -12f;
			transform.position = new Vector2(x, y);

			if (m_MovementType == MovementType.CurveUp)
				transform.localScale = new Vector3(1, -1, 1);


			
			//Debug.Log(type.ToString() + " " + m_MovementType.ToString() + " " + isTop +  " " + transform.position.y);
		}

		public void SetMovementType()
		{
			m_MovementType = GameParams.Instance.obstacles[type].moveType;
		}

		protected virtual void RandomMovemenType()
		{
			//m_MovementType = (MovementType)  Random.Range(0, 3);
		}

		void InitPath()
		{
			//if (transform.position.y > 0)
			{
				if (m_MovementType == MovementType.CurveDown)
					m_PathFollow = PathManager.Instance.RandomCurvePathDown(transform.position);
				
				if (m_MovementType == MovementType.ZigZag)
					m_PathFollow = PathManager.Instance.ZigZagPathDown(transform.position);
			}

			//if (transform.position.y < 0)
			{
				if (m_MovementType == MovementType.CurveUp)
					m_PathFollow = PathManager.Instance.RandomCurvePathUp(transform.position);

				//if (m_MovementType == MovementType.ZigZag)
					//m_PathFollow = PathManager.Instance.ZigZagPathUp(transform.position);

				if (m_MovementType == MovementType.StraightUp)
					m_Speed *= -1;
			}

			m_Target = transform.position;
			m_CurrentNode = 0;
		}
		
		// Update is called once per frame
		protected virtual void Update () 
		{
			if (GameManager.Instance.IsPause())
				return;
			
			UpdateMovement();
		}

		protected virtual void UpdateMovement()
		{
			if (m_MovementType == MovementType.StraightDown || m_MovementType == MovementType.StraightUp)
				UpdateMoveStraight();
			else
				UpdateMoveFollowingPath();
		}

		protected virtual void UpdateMoveFollowingPath()
		{
			PathFollowing();
			Steering();
		}

		protected virtual void UpdateMoveStraight()
		{
			transform.position += m_Speed * Time.deltaTime * Vector3.down;
		}

		protected virtual void PathFollowing()
		{
			if (m_PathFollow == null)
				return;

			m_Target = m_PathFollow[m_CurrentNode];

			float epsilon = 1f;
			if (m_Speed > 8f)
				epsilon = 3f;

			if (Vector3.Distance(transform.position, m_Target) < epsilon)
			{
				m_CurrentNode++;
				if (m_CurrentNode >= m_PathFollow.Count)
					m_CurrentNode = m_PathFollow.Count - 1;
			}
		}

		void Steering ()
		{
			Vector3 direction = m_Target - transform.position;
			Quaternion rotation = Quaternion.LookRotation (direction, Vector3.forward);
			rotation.x = 0;
			rotation.y = 0;

			transform.rotation = Quaternion.Slerp (transform.rotation, rotation, m_RotationSpeed);
			transform.Translate(Vector3.down * Time.deltaTime * m_Speed);
		}

		public void IncreaseSpeed()
		{
			if (m_MovementType == MovementType.StraightUp 
				|| m_MovementType == MovementType.CurveUp)
			{
				m_Speed *= -2.5f;
				return;
			}
			
			m_Speed *= SwimmingConfig.IMMORTAL_FACTOR;
			m_RotationSpeed *= SwimmingConfig.IMMORTAL_FACTOR;
			//Debug.Log("IncreaseSpeed " + m_Speed);
		}

		public void DecreaseSpeed()
		{
			if (m_MovementType == MovementType.StraightUp 
				|| m_MovementType == MovementType.CurveUp)
				return;
			
			m_Speed /= SwimmingConfig.IMMORTAL_FACTOR;
			m_RotationSpeed /= SwimmingConfig.IMMORTAL_FACTOR;
		}

		protected virtual void OnTriggerEnter2D(Collider2D other)
		{
			//Debug.Log("OnTriggerEnter2D " + other);
			if (other.CompareTag(SwimmingConfig.TAG_PLAYER))
			{
				if (Swimmer.Instance.IsImmortal())
				{
					Die();
					GameManager.Instance.RemoveEnemy(gameObject);
				}
				else
					Swimmer.Instance.Hurt();
			}
		}

		void Die()
		{
			ComponentConstant.SOUND_MANAGER.Play(SoundEnum.SE17_mutsugo_smash);
			GameObject dieFx = EnemySpawner.Instance.GetFreeObject("DieFX");
			dieFx.transform.position = transform.position;
			Destroy(dieFx, 2f);
		}
	
	}

}