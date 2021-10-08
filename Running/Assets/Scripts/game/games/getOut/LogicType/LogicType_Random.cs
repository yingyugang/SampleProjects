using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LogicType_Random : LogicBase
{
	// Use this for initialization

	override public void Init(List<Vector2> Area)
	{
		m_IndexFlicker = 0;
		m_TimeFlicker = GameConfig.TIME_FLICKER;
		m_IsFlicker = true;
		gameObject.GetComponent<Collider2D>().enabled = false;
		m_Sprite = gameObject.GetComponent<SpriteRenderer>();
		int index = Random.Range (0, Area.Count);
		m_PositionY = (int)Area [index].x;
		m_PositionX = (int)Area[index].y;
		transform.position = new Vector3 (-LoadMap.ScreenSize.x + Map.SizeBlock * m_PositionX + Map.SizeBlock/2, LoadMap.ScreenSize.y - Map.SizeBlock * m_PositionY+Map.SizeBlock/2, 0f);
		m_NextPosition = transform.position;
		m_LstDirectionPosible = new List<Direction> ();
		m_CurrentDirection = Direction.None;
		m_Animator = gameObject.GetComponent<Animator>();
		iconFooter.SetImageIcon();
		speed = basicSpeed;
	}


	// Update is called once per frame
	void Update ()
	{
		if(!GetOut.GameManager.Instance().isGameOver && !GetOut.GameManager.Instance().isPause)
		{
			if(m_IsFlicker)
				FlickerInit();
			if(GetOut.GameManager.Instance().isClock)
				FlickerClock(false);
			RandomNextStep ();
		}
	}
	//Random next step for character
	public void RandomNextStep ()
	{
		transform.position = Vector3.MoveTowards (transform.position, m_NextPosition, speed * Time.deltaTime*2f);
		switch (m_CurrentDirection) {
		//If go down
		case (Direction.Down):
			{
				Move(Direction.Down,Direction.Up);
				m_Animator.SetInteger(GameConfig.VALUE_ANIMATOR,GameConfig.ANIM_UNDER);
				break;
			}
		//If go up
		case (Direction.Up):
			{
				Move(Direction.Up,Direction.Down);
				m_Animator.SetInteger(GameConfig.VALUE_ANIMATOR,GameConfig.ANIM_UP);
				break;
			}
		//If turn left
		case (Direction.Left):
			{
				Move(Direction.Left,Direction.Right);
				m_Animator.SetInteger(GameConfig.VALUE_ANIMATOR,GameConfig.ANIM_LEFT);
				break;
			}
		//If turn right
		case (Direction.Right):
			{
				Move(Direction.Right,Direction.Left);
				m_Animator.SetInteger(GameConfig.VALUE_ANIMATOR,GameConfig.ANIM_RIGHT);
				break;
			}
		//If don't move
		case Direction.None:
			{
				m_OppositeDirection = Direction.None;
				if (m_CurrentDirection != m_NextDirection)
					m_CurrentDirection = m_NextDirection;
				break;
			}
		}
		RandomNextDirection ();
	}


}
