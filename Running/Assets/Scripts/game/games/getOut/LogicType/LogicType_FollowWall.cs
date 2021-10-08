using UnityEngine;
using System.Collections;
using System.Collections.Generic;
/// <summary>
/// Logic type follow wall.
/// follow clockwise
/// </summary>
public class LogicType_FollowWall : LogicBase {
	// Use this for initialization
	override public void Init(List<Vector2> Area)
	{
		Area = LoadMap.map.arrayEdgeOfMap;
		m_IndexFlicker = 0;
		m_TimeFlicker = GameConfig.TIME_FLICKER;
		m_IsFlicker = true;
		gameObject.GetComponent<Collider2D>().enabled = false;
		m_Sprite = gameObject.GetComponent<SpriteRenderer>();
		int index = Random.Range (0, Area.Count);
		m_PositionY = (int)Area [index].x;
		m_PositionX = (int)Area [index].y;
		transform.position = new Vector3 (-LoadMap.ScreenSize.x + Map.SizeBlock * m_PositionX + Map.SizeBlock/2, LoadMap.ScreenSize.y - Map.SizeBlock * m_PositionY+Map.SizeBlock/2, 0f);
		m_NextPosition = transform.position;
		m_LstDirectionPosible = new List<Direction> ();
		if(m_PositionY == 2)
		{
				m_NextDirection = Direction.Right;
		}else
		{
			if(m_PositionX == 1)
				m_NextDirection = Direction.Up;
			else if(m_PositionY == Map.WIDTH - 2)
				m_NextDirection = Direction.Left;
				else
					m_NextDirection = Direction.Down;
		}
		m_Animator = gameObject.GetComponent<Animator>();
		m_CurrentDirection = Direction.None;
		iconFooter.SetImageIcon();
		speed = basicSpeed;
	}

	// Update is called once per frame
	void Update () {
		if(!GetOut.GameManager.Instance().isPause && !GetOut.GameManager.Instance().isGameOver)
		{
			if(m_IsFlicker)
				FlickerInit();
			RandomNextStep();
		}
	}

	protected void Move(Direction currentDirection, Direction rightOfCurrentDirection,Direction leftOfCurrentDirection, Direction oppositeDirection)
	{
		if(CheckMovePosible(currentDirection))
		{
			SetNextPosition(currentDirection,rightOfCurrentDirection,leftOfCurrentDirection,oppositeDirection);
		}
		else
			CanNotMove();
	}


	protected void SetNextPosition(Direction currentDirection, Direction rightOfCurrentDirection,Direction leftOfCurrentDirection, Direction oppositeDirection)
	{
		if (IsCenterBlock()) {
			
			if (currentDirection != m_NextDirection) {
				m_CurrentDirection = m_NextDirection;
			} else {
				m_NextPosition += Map.DeltaDirection[(int)currentDirection];
				MovePositionMatrix(currentDirection);
				m_OppositeDirection = oppositeDirection;
			}
			CheckTurnRight(currentDirection,rightOfCurrentDirection,leftOfCurrentDirection,oppositeDirection,false);
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
				Move(Direction.Down,Direction.Left,Direction.Right,Direction.Up);
				m_Animator.SetInteger(GameConfig.VALUE_ANIMATOR,GameConfig.ANIM_UNDER);
				break;
			}
			//If go up
		case (Direction.Up):
			{
				Move(Direction.Up,Direction.Right,Direction.Left,Direction.Down);
				m_Animator.SetInteger(GameConfig.VALUE_ANIMATOR,GameConfig.ANIM_UP);
				break;
			}
			//If turn left
		case (Direction.Left):
			{
				Move(Direction.Left,Direction.Up,Direction.Down,Direction.Right);
				m_Animator.SetInteger(GameConfig.VALUE_ANIMATOR,GameConfig.ANIM_LEFT);
				break;
			}
			//If turn right
		case (Direction.Right):
			{
				Move(Direction.Right,Direction.Down,Direction.Up,Direction.Left);
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

	}



	private void CheckTurnRight(Direction currentDirection, Direction rightOfCurrentDirection,Direction leftOfCurrentDirection, Direction oppositeDirection, bool clockWise)
	{
		bool canTurnRight = false;
		bool canGoForward = false;
		bool canTurnLeft = false;
		Node nodeLink;
		for(int i = 0; i < LoadMap.map.matrix[m_PositionY,m_PositionX].Next.Count; i++)
		{
			nodeLink = LoadMap.map.matrix[m_PositionY,m_PositionX].Next[i];;
			if(CheckNodeRightWithDirection(currentDirection,nodeLink))
			{
				canTurnRight = true;
			}
			if(CheckNodeRightWithDirection(oppositeDirection,nodeLink))
				canTurnLeft = true;
			if(CheckNodeForwardtWithDirection(currentDirection,nodeLink))
				canGoForward = true;
		}


		if(clockWise)
		{
		//follow with clockwise
		if(canTurnRight)
		{
			m_NextDirection = rightOfCurrentDirection;
		}else
			if(canGoForward)
				m_NextDirection = m_CurrentDirection;
			else
				if(canTurnLeft)
					m_NextDirection = leftOfCurrentDirection;
				else
					m_NextDirection = oppositeDirection;
		}else

			//don't follow with clockwise
			if(canTurnLeft)
			{
				m_NextDirection = leftOfCurrentDirection;
			}else
				if(canGoForward)
					m_NextDirection = m_CurrentDirection;
				else
					if(canTurnRight)
						m_NextDirection = rightOfCurrentDirection;
					else
						m_NextDirection = oppositeDirection;
	}

	private bool CheckNodeRightWithDirection(Direction direction, Node node)
	{
		switch(direction)
		{
		case Direction.Up:
			if(node.Position.x == m_PositionY && node.Position.y > m_PositionX)
			{
				return true;
			}
			break;
		case Direction.Down:
			if(node.Position.x == m_PositionY && node.Position.y < m_PositionX)
				return true;
			break;
		case Direction.Left:
			if(node.Position.y == m_PositionX && node.Position.x < m_PositionY)
				return true;
			break;
		case Direction.Right:
			if(node.Position.y == m_PositionX && node.Position.x > m_PositionY)
				return true;
			break;
		}
		return false;
	}

	private bool CheckNodeForwardtWithDirection(Direction direction, Node node)
	{
		switch(direction)
		{
		case Direction.Up:
			if(node.Position.x < m_PositionY && node.Position.y == m_PositionX)
			{
				return true;
			}
			break;
		case Direction.Down:
			if(node.Position.x > m_PositionY && node.Position.y == m_PositionX)
				return true;
			break;
		case Direction.Left:
			if(node.Position.x == m_PositionY && node.Position.y < m_PositionX)
				return true;
			break;
		case Direction.Right:
			if(node.Position.x == m_PositionY && node.Position.y > m_PositionX)
				return true;
			break;
		}
		return false;
	}
}
