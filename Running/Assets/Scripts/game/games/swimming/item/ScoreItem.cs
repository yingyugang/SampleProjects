using UnityEngine;
using System.Collections;

namespace Swimming
{
	public class ScoreItem : Item 
	{
		protected virtual void Init()
		{
			base.Init();
			m_Score = 10;
		}

		protected override void ItemEffect()
		{
			//SoundManager.Instance.PlaySfx(SoundType.GetItem);
			ComponentConstant.SOUND_MANAGER.Play(SoundEnum.se11_scoreitem);
			AddItemScore();
		}

		void AddItemScore()
		{
			switch(type)
			{
			case ItemType.Oden:
				Swimmer.Instance.numOdenItem++;
				break;
			case ItemType.Tree:
				Swimmer.Instance.numTreeItem++;
				break;
			case ItemType.Rice:
				Swimmer.Instance.numRiceItem++;
				break;
			}

			Swimmer.Instance.UpdateScore();
		}
	}
}