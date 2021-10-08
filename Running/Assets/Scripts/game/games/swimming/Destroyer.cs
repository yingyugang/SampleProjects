using UnityEngine;
using System.Collections;

namespace Swimming
{
	public class Destroyer : MonoBehaviour 
	{

		void OnTriggerEnter2D(Collider2D other)
		{
			//Debug.Log("OnTriggerEnter2D ");
			if (other.CompareTag(SwimmingConfig.TAG_ENEMY))
			{
				GameManager.Instance.RemoveEnemy(other.gameObject);
			}

			if (other.CompareTag(SwimmingConfig.TAG_ITEM))
			{
				GameManager.Instance.RemoveItem(other.gameObject);
			}
		}
	}
}