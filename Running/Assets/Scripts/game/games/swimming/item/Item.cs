using UnityEngine;
using System.Collections;

namespace Swimming
{
	public enum ItemType
	{
		Immortal = 1,
		Tree,
		Oden,
		Rice
	}

	public class Item : MonoBehaviour 
	{
		public ItemType type;

		protected float m_Speed;
		protected int m_Score;

		// Use this for initialization
		protected virtual void Start () 
		{
			//Init();
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

		public virtual void Init()
		{
			RandomPosition();
			m_Speed = GameParams.Instance.GetRelativelyRiverSpeed();
			m_Speed *= GameManager.Instance.GetSpeedUpRatio();
			m_Score = GameParams.Instance.GetItemDataByType(type).effectValue;
			if (Swimmer.Instance.IsImmortal())
				IncreaseSpeed();
		}

		protected virtual void RandomPosition()
		{
			float x = Random.Range(-2.5f, 2.5f);
			float lastEnemyX = EnemySpawner.Instance.lastEnemyPos.x; 
			if (lastEnemyX > 0)
				x = lastEnemyX - 1f;

			else
				x = lastEnemyX + 1f;

			float y = 12f;
			transform.position = new Vector2(x, y);
		}

		protected virtual void OnTriggerEnter2D(Collider2D other)
		{
			if (other.CompareTag(SwimmingConfig.TAG_PLAYER))
			{
				GameManager.Instance.RemoveItem(gameObject);
				ItemEffect();
			}
		}

		protected virtual void ItemEffect()
		{
			
		}

		public void IncreaseSpeed()
		{
			m_Speed *= SwimmingConfig.IMMORTAL_FACTOR;
			//Debug.Log("IncreaseSpeed " + m_Speed);
		}

		public void DecreaseSpeed()
		{
			m_Speed /= SwimmingConfig.IMMORTAL_FACTOR;
		}
	}
}