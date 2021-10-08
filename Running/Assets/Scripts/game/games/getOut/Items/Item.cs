using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace GetOut{
	public enum TYPEITEM{
		Score1 = 0,
		Score2 = 1,
		Score3 = 2,
		Star = 3,
		Clock = 4
	}

	public class Item : MonoBehaviour {
		public TYPEITEM type;
		public int numberScore;
		public float appearProbality;
		public float reductionAppearPercent;
		private int m_PositionY;
		private	int m_PositionX;
		private List<Vector2> m_ListArea;

		//Set Area to init item
		public void SetArea(List<Vector2> arrayPosition )
		{
			m_ListArea = arrayPosition;
			Init();
		}

		//init item
		void Init()
		{
			gameObject.SetActive(true);
			int index = Random.Range (1, m_ListArea.Count);
			m_PositionY = (int)m_ListArea [index].x;
			m_PositionX = (int)m_ListArea [index].y;
			transform.position = new Vector3 (-LoadMap.ScreenSize.x + Map.SizeBlock * m_PositionX + Map.SizeBlock/2, LoadMap.ScreenSize.y - Map.SizeBlock * m_PositionY+Map.SizeBlock/2, 0f);
			m_ListArea.Remove(m_ListArea[index]);
		}

		//get effect of item
		public void GetItem()
		{
			switch(type)
			{
			case TYPEITEM.Score1:
				{
					if(GetOut.GameManager.Instance().numberItemScore1 < 99)
						GetOut.GameManager.Instance().numberItemScore1++;
					GameFooter.Instance().SetNumberScore1Text();
					Score();
					break;
				}
			case TYPEITEM.Score2:
				{
					if(GetOut.GameManager.Instance().numberItemScore2 < 99)
						GetOut.GameManager.Instance().numberItemScore2++;
					GameFooter.Instance().SetNumberScore2Text();
					Score();
					break;
				}
			case TYPEITEM.Score3:
				{
					if(GetOut.GameManager.Instance().numberItemScore3 < 99)
						GetOut.GameManager.Instance().numberItemScore3++;
					GameFooter.Instance().SetNumberScore3Text();
					Score();
					break;
				}
			case TYPEITEM.Clock:
				Clock();
				break;
			case TYPEITEM.Star:
				StarAction();
				break;
			}
		}

		//Effect star
		public void StarAction()
		{
			GetOut.GameManager.Instance().isStar = true;
			GetOut.GameManager.Instance().timeClock = 0;
			GetOut.GameManager.Instance().timeStar = GameConfig.TIME_STAR;
			GameFooter.Instance().SetAnimEffectClockItem(false);
			GetOut.GameManager.Instance().SetParticleEffectStar(true);
			GameFooter.Instance().SetAnimEffectStarItem(true);
			GetOut.GameManager.Instance().SetClockEffect(false);
			if(GetOut.GameManager.Instance().numberItemStar < 4)
			{
				GetOut.GameManager.Instance().percentageAppearStar -= GetOut.GameManager.Instance().percentageAppearStar*reductionAppearPercent/100;
			}
			if(ComponentConstant.SOUND_MANAGER != null)
			{
				
				ComponentConstant.SOUND_MANAGER.Play(SoundEnum.se12_bonusitem);
				ComponentConstant.SOUND_MANAGER.StopBGM();
				ComponentConstant.SOUND_MANAGER.Play(SoundEnum.bgm10_invicible,GameManager.Instance().GetAudioSource);
			}

		}

		//Effect clock 
		public void Clock()
		{
			GetOut.GameManager.Instance().isClock = true;
			GetOut.GameManager.Instance().timeClock = GameConfig.TIME_CLOCK;
			GetOut.GameManager.Instance().timeStar = 0;
			GetOut.GameManager.Instance().SetClockSpeed();
			GameFooter.Instance().SetAnimEffectClockItem(true);
			GetOut.GameManager.Instance().SetParticleEffectStar(false);
			if(GetOut.GameManager.Instance().numberItemClock < 4)
			{
				GetOut.GameManager.Instance().percentageAppearStar -= GetOut.GameManager.Instance().percentageAppearClock*reductionAppearPercent/100;
			}
			if(ComponentConstant.SOUND_MANAGER != null)
			{
				ComponentConstant.SOUND_MANAGER.Play(SoundEnum.se12_bonusitem);
				if(GameManager.Instance().isStar)
					ComponentConstant.SOUND_MANAGER.Play(SoundEnum.bgm04_escape,GetOut.GameManager.Instance().GetAudioSource);
				GetOut.GameManager.Instance().SetClockEffect(true);
			}

		}

		//Effect score
		public void Score()
		{
			GetOut.GameManager.Instance().scoreBonus += numberScore;
			if(ComponentConstant.SOUND_MANAGER != null)
				ComponentConstant.SOUND_MANAGER.Play(SoundEnum.se11_scoreitem);
		}

		//Collision
		void OnTriggerEnter2D(Collider2D other)
		{
			if(other.gameObject.tag == GameConfig.TAG_PLAYER)
			{
				GetItem();
				m_ListArea.Add(new Vector2(m_PositionY,m_PositionX));
				gameObject.SetActive(false);
				GetOut.GameManager.Instance().numberItemActive -= 1;
				if(GetOut.GameManager.Instance().timeInitItem <= 0)
					GetOut.GameManager.Instance().timeInitItem = GameConfig.TIME_INIT_ITEM;
			}
		}


	}
}
