using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class LogicBase : MonoBehaviour {
	public float speed ;
	public float basicSpeed;
	public int idEnemy;
	public float timeAppear;								//Time appear of character
	public int percentLogic;
	public EnemyFooterIcon iconFooter;
	protected Direction m_CurrentDirection;					//Current direction of chacracter
	protected Direction m_OppositeDirection;				//Opposite direction of current direction
	protected Direction m_NextDirection;					//next direction of character
	protected int m_PositionY;								//index collumn in matrix MAP
	protected int m_PositionX;								//index row in matrix MAP
	protected Vector3 m_NextPosition;						//next position of character
	protected List<Direction> m_LstDirectionPosible;		// list direction posible at current position to go
	protected Animator m_Animator;							
	protected float m_TimeFlicker = 0.5f;					//Time for once hide or appear
	protected int m_IndexFlicker;							//number of flicker
	protected SpriteRenderer m_Sprite;						//sprite renderer
	protected bool m_IsFlicker;								//Is state flicker
	protected float m_DeltaColorA = 10f;					//deltacolor alpha


	//Check if the character at the center of a block
	protected bool IsCenterBlock()							
	{
		if (Mathf.Abs ((transform.position - m_NextPosition).magnitude) < Map.Eps){
			return true;
		}
		return false;
	}


	//Random next direction
	protected void RandomNextDirection ()
	{
		m_LstDirectionPosible.Clear ();
		Node node;
		//Check 4 direction
		for(int i = 0; i < LoadMap.map.matrix[m_PositionY,m_PositionX].Next.Count; i++)
		{
			node = LoadMap.map.matrix[m_PositionY,m_PositionX].Next[i];
			//direction right
			if(node.Position.x == m_PositionY && node.Position.y > m_PositionX)
				m_LstDirectionPosible.Add(Direction.Right);
			else if(node.Position.x == m_PositionY && node.Position.y < m_PositionX)
				m_LstDirectionPosible.Add(Direction.Left);
			else if(node.Position.y == m_PositionX && node.Position.x < m_PositionY)
				m_LstDirectionPosible.Add(Direction.Up);
			else if(node.Position.y == m_PositionX && node.Position.x > m_PositionY)
				m_LstDirectionPosible.Add(Direction.Down);
		}


		//Check next direction isn't opposite direction
		for (int i = 0; i < m_LstDirectionPosible.Count; i++) {
			if (m_LstDirectionPosible [i] == m_OppositeDirection) {
				m_LstDirectionPosible.RemoveAt (i);
				break;
			}
		}
		if (m_LstDirectionPosible.Count == 0) 
		{
			m_OppositeDirection = m_CurrentDirection;
			m_NextDirection = m_OppositeDirection;
		}
		else{
			m_NextDirection = m_LstDirectionPosible [Random.Range (0, m_LstDirectionPosible.Count)];
		}
	}

	//Init character
	virtual public void Init(List<Vector2> Area){
		m_Sprite = gameObject.GetComponent<SpriteRenderer>();
	}
	//Check next direction can move?
	protected bool CheckMovePosible (Direction direction)
	{
		m_LstDirectionPosible.Clear ();
		Node node;
		for(int i = 0; i < LoadMap.map.matrix[m_PositionY,m_PositionX].Next.Count; i++)
		{
			node = LoadMap.map.matrix[m_PositionY,m_PositionX].Next[i];
			//direction right
			if(node.Position.x == m_PositionY && node.Position.y > m_PositionX)
				m_LstDirectionPosible.Add(Direction.Right);
			else if(node.Position.x == m_PositionY && node.Position.y < m_PositionX)
				m_LstDirectionPosible.Add(Direction.Left);
			else if(node.Position.y == m_PositionX && node.Position.x < m_PositionY)
				m_LstDirectionPosible.Add(Direction.Up);
			else if(node.Position.y == m_PositionX && node.Position.x > m_PositionY)
				m_LstDirectionPosible.Add(Direction.Down);
		}

		if(m_LstDirectionPosible.Contains(direction))
			return true;
		return false;
	}


	// Can't move with the direction
	virtual protected void CanNotMove()
	{
		if (m_CurrentDirection != m_NextDirection) 
			if (CheckMovePosible (m_NextDirection)) 
				m_CurrentDirection = m_NextDirection;
	}


	//Change position in the matrix logic
	protected void MovePositionMatrix(Direction nowDirection)
	{
		switch(nowDirection)
		{
		case Direction.Down:
			m_PositionY += 1;
			break;
		case Direction.Up:
			m_PositionY -= 1;
			break;
		case Direction.Left:
			m_PositionX -= 1;
			break;
		case Direction.Right:
			m_PositionX += 1;
			break;
		}
	}


	//Set next position for character
	virtual protected void SetNextPosition(Direction nowDirection,Direction oppositeDirection)
	{
		if (IsCenterBlock()) {
			if (nowDirection != m_NextDirection) {
				m_CurrentDirection = m_NextDirection;
			} else {
				m_NextPosition += Map.DeltaDirection[(int)nowDirection];
				MovePositionMatrix(nowDirection);
				m_OppositeDirection = oppositeDirection;
			}
		}
	}



	//Move to next step with the direction
	virtual protected void Move(Direction direction,Direction oppositeDirection)
	{
		if(CheckMovePosible(direction))
			SetNextPosition(direction,oppositeDirection);
		else
			CanNotMove();
	}

	//Flicker
	virtual protected void FlickerInit()
	{
		if(m_IndexFlicker <  GameConfig.INDEX_FLICKER)
		{

			if(m_TimeFlicker < 0)
			{
				if(m_IndexFlicker % 2 == 0)
					m_Sprite.color = new Color(Color.white.r,Color.white.g,Color.white.b,0f);
				else
					m_Sprite.color = Color.white;
				m_IndexFlicker ++;
				m_TimeFlicker = GameConfig.TIME_FLICKER;
			}else
				m_TimeFlicker -= Time.deltaTime;
		}else
		{
			m_IsFlicker = false;
			gameObject.GetComponent<Collider2D>().enabled = true;
		}
	}

	public void FlickerClock(bool unClock)
	{
		if(!unClock)
		{
			if(m_TimeFlicker < 0)
			{
				if(m_IndexFlicker % 2 == 0)
					m_Sprite.color = new Color(Color.white.r,Color.white.g,Color.white.b,0f);
				else
					m_Sprite.color = Color.white;
				m_IndexFlicker ++;
				m_TimeFlicker = GameConfig.TIME_FLICKER_CLOCK;
			}else
				m_TimeFlicker -= Time.deltaTime;
		}else
			m_Sprite.color = Color.white;
	}

	public void SetIdleAnimation()
	{
		m_Animator = gameObject.GetComponent<Animator>();
		m_Animator.SetInteger(GameConfig.VALUE_ANIMATOR,GameConfig.ANIM_IDLE_ENEMY);
	}
}
