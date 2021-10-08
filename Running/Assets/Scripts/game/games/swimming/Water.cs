using UnityEngine;
using System.Collections;

namespace Swimming
{
	public class Water : MonoBehaviour 
	{

		public Sprite spriteMorning;
		public Sprite spriteEvening;
		public Sprite spriteNight;

		public Tree[] m_Trees;

		private SpriteRenderer m_Sprite;

		void Start()
		{
			m_Sprite = GetComponent<SpriteRenderer>();
			m_Trees = GetComponentsInChildren<Tree>();
			//Debug.Log("xx" + m_Trees.Length);
		}

		void OnEnable()
		{
			m_Sprite = GetComponent<SpriteRenderer>();
			m_Trees = GetComponentsInChildren<Tree>();
			//Debug.Log("xx" + m_Trees.Length);
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

			foreach (var tree in m_Trees)
				tree.SetSpriteTime(type);
		}

		public void CheckOverlapHelper()
		{
			//foreach( var tree in m_Trees)
			for (int i=0; i<m_Trees.Length; i++)
			{
				Tree tree = m_Trees[i];
				//tree.GetComponent<SpriteRenderer>().color = Color.red;
				//Vector3 pos = tree.transform.position;
				float y = tree.GetWorldPosY();
				//float y = pos.y;
				float helperY = Helper.Instance.transform.position.y;

				float epsilon = 4f;
				if (Swimmer.Instance.IsImmortal())
					epsilon = 6f;
				
				//Debug.DrawLine(Helper.Instance.transform.position, tree.GetWorldPos());
				//Debug.Log("Distance: " + tree.gameObject.GetInstanceID().ToString() + " " + Mathf.Abs(y-helperY));

				//if (tree.GetSprite().bounds.Intersects(Helper.Instance.spriteCharacter.bounds))
				if (Mathf.Abs(y-helperY) < epsilon)
				{
					//Debug.Log("Hide me");
					//tree.GetComponent<SpriteRenderer>().color = Color.red;
					tree.gameObject.SetActive(false);
				}
			}
		}

		public void ActiveAllTree()
		{
			foreach( var tree in m_Trees)
			{
				tree.gameObject.SetActive(true);
			}
		}
	}
}