using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class LogicType_GoStraight : LogicBase {
	// Use this for initialization


	override public void Init(List<Vector2> Area)
	{
		m_IsFlicker = true;
		m_IndexFlicker = 0;
		m_TimeFlicker = GameConfig.TIME_FLICKER;
		gameObject.GetComponent<Collider2D>().enabled = false;
		m_Sprite = gameObject.GetComponent<SpriteRenderer>();
		int index = Random.Range (0, Area.Count);
		m_PositionY = (int)Area [index].x;
		m_PositionX = (int)Area [index].y;
		transform.position = new Vector3 (-LoadMap.ScreenSize.x + Map.SizeBlock * m_PositionX + Map.SizeBlock/2, LoadMap.ScreenSize.y - Map.SizeBlock * m_PositionY+Map.SizeBlock/2, 0f);
		m_NextPosition = transform.position;
		m_LstDirectionPosible = new List<Direction> ();
		m_CurrentDirection = Direction.None;
		m_Animator = gameObject.GetComponent<Animator>();
		iconFooter.SetImageIcon();
		speed = basicSpeed;
	}
	// Update is called once per frame
	void Update () {
		if(!GetOut.GameManager.Instance().isGameOver&&!GetOut.GameManager.Instance().isPause)
		{
			if(m_IsFlicker)
				FlickerInit();
			if(GetOut.GameManager.Instance().isClock)
				FlickerClock(false);
			GoStraight();
		}
	}

	//set next step for character
	void SetNextPosition(Direction nowDirection)
	{
		if (IsCenterBlock()) {
				m_NextPosition += Map.DeltaDirection[(int)nowDirection];
				MovePositionMatrix(nowDirection);
		}
	}

	//character can not move with current direction
	protected override void CanNotMove()
	{
		RandomDirection();
		if (m_CurrentDirection != m_NextDirection) {
			if (CheckMovePosible (m_NextDirection) && CheckMovePosible (m_NextDirection)) {
				m_CurrentDirection = m_NextDirection;
			}
		}
	}

	//Function Move to next step
	void Move(Direction direction)
	{
		if(CheckMovePosible(direction))
		{
			SetNextPosition(direction);
		}else
		{
			CanNotMove();
		}
	}


	void GoStraight()
	{
		transform.position = Vector3.MoveTowards (transform.position, m_NextPosition, speed * Time.deltaTime *2f);
		switch (m_CurrentDirection) {
		//If go down
		case (Direction.Down):
			{
				Move(Direction.Down,Direction.None);
				m_Animator.SetInteger(GameConfig.VALUE_ANIMATOR,GameConfig.ANIM_UNDER);
				break;
			}
		//If go up
		case (Direction.Up):
			{
				Move(Direction.Up,Direction.None);
				m_Animator.SetInteger(GameConfig.VALUE_ANIMATOR,GameConfig.ANIM_UP);
				break;
			}
		//If turn left
		case (Direction.Left):
			{
				Move(Direction.Left,Direction.None);
				m_Animator.SetInteger(GameConfig.VALUE_ANIMATOR,GameConfig.ANIM_LEFT);
				break;
			}
		//If turn right
		case (Direction.Right):
			{
				Move(Direction.Right,Direction.None);
				m_Animator.SetInteger(GameConfig.VALUE_ANIMATOR,GameConfig.ANIM_RIGHT);
				break;
			}
		//If don't move
		case Direction.None:
			{
				RandomDirection();
				m_OppositeDirection = Direction.None;
				if (m_CurrentDirection != m_NextDirection)
				{
					m_CurrentDirection = m_NextDirection;
				}
				
				break;
			}
		}
	}
		

	public void RandomDirection ()
	{
		
		m_LstDirectionPosible.Clear ();
		//Check 4 direction
		//down
		if (CheckMovePosible(Direction.Down)) {
			m_LstDirectionPosible.Add (Direction.Down);
		}
		//up
		if (CheckMovePosible(Direction.Up)) {
			m_LstDirectionPosible.Add (Direction.Up);
		}

		//Left
		if (CheckMovePosible(Direction.Left)) {
			m_LstDirectionPosible.Add (Direction.Left);
		} 

		//right
		if (CheckMovePosible(Direction.Right)) {
			m_LstDirectionPosible.Add (Direction.Right);
		}

		//Check next direction isn't opposite direction
		if (m_LstDirectionPosible.Count != 1) {
			for (int i = 0; i < m_LstDirectionPosible.Count; i++) {
				if (m_LstDirectionPosible [i] == m_OppositeDirection) {
					m_LstDirectionPosible.RemoveAt (i);
					break;
				}
			}
		}
		m_NextDirection = m_LstDirectionPosible [Random.Range (0, m_LstDirectionPosible.Count)];
	}
}
