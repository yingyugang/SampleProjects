using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class LogicType_Follow : LogicBase {
	public int sizeInRange = 100;								//Radius range to follow
	private Stack<Node> m_Path;
	private Node m_Node;
	private bool m_IsFollow;
	private Node m_NextNode;						

	override public void Init(List<Vector2> Area)
	{
		m_IndexFlicker = 0;
		m_TimeFlicker = GameConfig.TIME_FLICKER;
		m_IsFlicker = true;
		gameObject.GetComponent<Collider2D>().enabled = false;
		m_Sprite = gameObject.GetComponent<SpriteRenderer>();
		m_Path = new Stack<Node> ();
		int index = Random.Range (0, Area.Count);
		m_PositionY = (int)Area [index].x;
		m_PositionX = (int)Area [index].y;
		transform.position = new Vector3 (-LoadMap.ScreenSize.x + Map.SizeBlock * m_PositionX + Map.SizeBlock/2, LoadMap.ScreenSize.y - Map.SizeBlock * m_PositionY+Map.SizeBlock/2, 0f);
		m_NextPosition = transform.position;
		m_LstDirectionPosible = new List<Direction> ();
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
			if(GetOut.GameManager.Instance().isClock)
				FlickerClock(false);
			RandomNextStep();
		}
	}


	//FindPath to follow player
	public void FindPath(Vector2 target)
	{
		if(LoadMap.map.matrix[(int)target.x,(int)target.y] != null && LoadMap.map.matrix[m_PositionY,m_PositionX] != null)
			m_Path = Find(LoadMap.map.matrix[m_PositionY,m_PositionX],LoadMap.map.matrix[(int)target.x,(int)target.y]);
	}


	//if don't in range then random next step
	public void RandomNextStep()
	{
		if(!CheckDistance())
		{
			RandomNextDirection ();
			m_IsFollow = false;
		}
		else
		{
			m_IsFollow = true;
		}
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
	}



	//Set the next destination for the character
	protected override void SetNextPosition (Direction nowDirection, Direction oppositeDirection)
	{
		if (IsCenterBlock()) {
			if(m_IsFollow)
			{
				Follow();
			}
			if (nowDirection != m_NextDirection && CheckMovePosible (m_NextDirection)) {
				m_CurrentDirection = m_NextDirection;
			} else {
				m_NextPosition += Map.DeltaDirection[(int)nowDirection];
				MovePositionMatrix(nowDirection);
				m_OppositeDirection = oppositeDirection;
			
			}
		}
	}


	//Move with the current direction
	protected override void Move (Direction direction, Direction oppositeDirection)
	{

		if(CheckMovePosible(direction))
		{
			SetNextPosition(direction,oppositeDirection);
		}
		else
		{
			CanNotMove();
		}
	}

	protected override void CanNotMove ()
	{
		if (m_IsFollow) 
		{
			Follow();
		}
		if (m_CurrentDirection != m_NextDirection) {
			if (CheckMovePosible (m_NextDirection)) {
				m_CurrentDirection = m_NextDirection;
			}
		}

	}
	//Check distance player in range can see of character
	public bool CheckDistance()
	{
		Vector2 target = new Vector2(PlayerControll.instance().m_PositionPlayerY,PlayerControll.instance().m_PositionPlayerX);
		Vector2 positionCurrent = new Vector2(m_PositionY,m_PositionX);
		float distance = (target - positionCurrent).sqrMagnitude;
		if(distance <= sizeInRange)
		{
			if(!m_IsFollow)
			{
				if(Percent(percentLogic))
				{
					return true;
				}
			}
			else
			{
				return true;
			}
		}
		else
		{
			return false;
		}
		return false;
	}


	//percent logic
	public bool Percent(int percent)
	{
		int random = Random.Range(0,100);
		if(random < percent)
		{
			return true;
		}
		return false;
	}

	//Follow player
	public void Follow()
	{
		int deltaX = 0;
		int deltaY = 0;
		Vector2 target = new Vector2(PlayerControll.instance().m_PositionPlayerY,PlayerControll.instance().m_PositionPlayerX);
		FindPath(target);
		if(m_Path.Count > 1)
		{
			m_Node=m_Path.Pop();
			m_NextNode = m_Path.Pop();
		}else
		{
			m_Node = m_NextNode;
			m_NextNode = m_Path.Pop();
		}
		if(m_NextNode != null && m_Node != null)
		{
			deltaX = (int)m_NextNode.Position.x - (int)m_Node.Position.x;
			deltaY = (int)m_NextNode.Position.y - (int)m_Node.Position.y;
		}
		if(deltaX != 0)
		{
			if(deltaX > 0)
			{
				m_NextDirection = Direction.Down;
			}else
			{
				m_NextDirection = Direction.Up;
			}
		}else
			if(deltaY > 0)
			{
				m_NextDirection = Direction.Right;
			}else
			{
				m_NextDirection = Direction.Left;
			}
		}
		


	//Find the path follow
	private Stack<Node> Find (Node start, Node Target)
	{
		if (Target == null)
			return null;
		List<Node> Open = new List<Node> ();
		List<Node> Close = new List<Node> ();

		//make open and close is empty
		Open.Clear ();
		Close.Clear ();
		//calculate h,g,f for start
		start.g = 0;//distance between current and start
		start.h = start.Distance (Target);
		start.f = start.h + start.g;

		//add start to open list
		Open.Add (start);
		while (Open.Count != 0) {
			//Find node in open list have min f
			Node current = Open [0];
			foreach (Node i_node in Open) {
				if (i_node.f < current.f)
					current = i_node;
			}
			//Remove current from open list
			Open.Remove (current);
			//Add curent to close list
			Close.Add (current);
			if (current == Target) {//If current is target then return path
				//Return path
				Open.Clear ();
				Close.Clear ();
				return ReconstructPath (start, Target);
			} else {
				//with next node in current.next
				foreach (Node i_node in current.Next) {
					if (Close.Contains (i_node))
						continue;//if node in close will do nothing
					double tmp_current_g = current.g + current.Distance (i_node);
					if (!Open.Contains (i_node) || tmp_current_g < i_node.g) {
						i_node.came_from = current;
						i_node.g = tmp_current_g;
						i_node.h = i_node.Distance (Target);
						i_node.f = i_node.g + i_node.h;
						if (!Open.Contains (i_node))
							Open.Add (i_node);
					}
				}
			}
		}
		return null;
	}

	private Stack<Node> ReconstructPath (Node s,Node t)
	{
		int i = 0;
		Stack<Node> path = new Stack<Node> ();
		path.Clear ();
		Node tmp = t;
		while (tmp != null) {
			i++;
			path.Push (tmp);
			Node c_f = tmp;
			tmp = tmp.came_from;
			c_f.came_from = null;
		}
		return path;
	}

}
