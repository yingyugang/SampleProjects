using UnityEngine;
using System.Collections;
using DG.Tweening;
using System.Collections.Generic;
// Direction
public enum Direction
{
	Right = 0,
	Left = 1,
	Up = 2,
	Down = 3,
	None = 4
}




public class PlayerControll : MonoBehaviour
{
	public int m_PositionPlayerX;				// Current position column of player in matrix MAP
	public int m_PositionPlayerY;				// Current position row of player in matrix MAP
	public float m_Speed = 1f;					//Speed of player
	private static PlayerControll _instance;
	private Animator m_Animator;
	//Swipe
	private float m_FingerStartTime = 0.0f;			//check time swipe
	private Vector2 m_FingerStartPos = Vector2.zero;//position star swipe
	private bool m_IsSwipe;							//check swipe
	private float m_MinSwipeDist = 100.0f;			//min swipe distance flick sensitivity
	private float m_MinSlideScreenDist = 2f;		//
	private float m_MaxSwipeTime = 0.2f;			//if swipe time > max swipe time then it isn't a swipe action
	private float m_GestureTime;					//time in once swipe action
	private float m_GestureDist;					//distance in once swipe action	
	private bool m_IsImmortal;						//if player is in immortal time it's true
	private float m_TimeFlick;
	private int m_IndexFlick = 0;
	private bool m_IsDead;
	private SpriteRenderer sprite;
	//Direction
	private Vector3 m_NextPosition;				// Next Target position of player when move
	public Direction m_CurrentDirection;		// Current direction of player
	private Direction m_NextDirection;			// Next direction of player

	void Awake()
	{
		_instance = this;
	}

	//init
	public void Init()
	{
		m_MinSwipeDist = GameConfig.FLICK_SENSITIVITY;
		m_Speed = GameConfig.PLAYER_SPEED_RATE;
		if(!gameObject.activeSelf)
			gameObject.SetActive(true);
		m_PositionPlayerX = 1;
		m_PositionPlayerY = Map.WIDTH - 2;
		transform.position = new Vector3 (-LoadMap.ScreenSize.x + Map.SizeBlock * m_PositionPlayerX + Map.SizeBlock/2, LoadMap.ScreenSize.y - Map.SizeBlock * m_PositionPlayerY+Map.SizeBlock/2, 0f);
		m_CurrentDirection = Direction.None;
		m_NextDirection = Direction.None;
		m_NextPosition = transform.position;
		m_Animator = gameObject.GetComponent<Animator>();
		sprite = gameObject.GetComponent<SpriteRenderer>();
		m_Animator.SetInteger(GameConfig.VALUE_ANIMATOR,GameConfig.ANIM_STOP_RIGHT);
	}


	public static PlayerControll instance(){
		return _instance;
	}

	//Set next destination for character
	public void SetNextPosition(Direction nowDirection)
	{
		if (IsCenterBlock()) {
			if (nowDirection != m_NextDirection && CheckMovePosible (m_NextDirection)) {
				m_CurrentDirection = m_NextDirection;
			} else {
				PlayAnimation(false);
				m_NextPosition += Map.DeltaDirection[(int)nowDirection];
				MovePositionMatrix(nowDirection);
			}
		}
	}

	//Change position in the matrix logic
	public void MovePositionMatrix(Direction nowDirection)
	{
		switch(nowDirection)
		{
		case Direction.Down:
			m_PositionPlayerY += 1;
			break;
		case Direction.Up:
			m_PositionPlayerY -= 1;
			break;
		case Direction.Left:
			m_PositionPlayerX -= 1;
			break;
		case Direction.Right:
			m_PositionPlayerX += 1;
			break;
		}
	}


	//If it go to the center of the block
	public bool IsCenterBlock()
	{
		if (Mathf.Abs ((transform.position - m_NextPosition).magnitude) < Map.Eps){
			return true;
		}
		return false;
	}

	//Can't move with current direction
	public void CanNotMove()
	{
		if (m_CurrentDirection != m_NextDirection) {
			if (CheckMovePosible (m_NextDirection)) {
				m_CurrentDirection = m_NextDirection;
			}
		}
	}

	//Move to next step with current direction
	public void Move(Direction direction)
	{
		if(CheckMovePosible(direction))
		{
			SetNextPosition(direction);
		}
		else
		{
			CanNotMove();
			if(IsCenterBlock())
			{
				PlayAnimation(true);
			}
		}
	}




	// Update is called once per frame
	void Update ()
	{
		if(!GetOut.GameManager.Instance().isGameOver && !GetOut.GameManager.Instance().isPause && !GetOut.GameManager.Instance().isGameOver)
		{
			if(m_IsImmortal || m_IsDead)
			{
				if(m_IndexFlick < 6)
				{
					Flicker();
				}else
				{
					if(m_IsImmortal)
						m_IsImmortal = false;
					if(m_IsDead)
					{
						m_IsDead = false;
						m_IsImmortal = true;
						m_IndexFlick = 0;
						Init();
					}
				}
			}
			if (Input.GetKeyDown (KeyCode.UpArrow)) {
				m_NextDirection = Direction.Up;
			}
			if(Input.GetKeyDown(KeyCode.DownArrow))
			{
				m_NextDirection = Direction.Down;
			}
			if (Input.GetKeyDown (KeyCode.LeftArrow)) {
				m_NextDirection = Direction.Left;
			}

			if (Input.GetKeyDown (KeyCode.RightArrow)) {
				m_NextDirection = Direction.Right;
			}

			CheckSwipeScreen ();

			if(!m_IsDead)
				transform.position = Vector3.MoveTowards(transform.position,m_NextPosition,m_Speed*Time.deltaTime * 2f);
			switch (m_CurrentDirection) {
			case (Direction.Down):
			{ 
				//Check if player go to the gate
				if(m_PositionPlayerY >= Map.GateY2)
				{
					if(m_PositionPlayerX == Map.GateLeftX && GetOut.GameManager.Instance().isGateLeftOpen && m_NextDirection == Direction.Down)
					{
						m_PositionPlayerY = Map.GateY1;
							transform.position = new Vector3 (-LoadMap.ScreenSize.x + Map.SizeBlock * m_PositionPlayerX + Map.SizeBlock/2, 
								LoadMap.ScreenSize.y - Map.SizeBlock * m_PositionPlayerY+Map.SizeBlock/2, 0f);
						m_NextPosition = transform.position;
						GetOut.GameManager.Instance().isGateLeftOpen = false;
						GetOut.GameManager.Instance().timeGateLeftClose = GameConfig.TIME_DOOR_CLOSE;
					}else
							if(m_PositionPlayerX == Map.GateRightX && GetOut.GameManager.Instance().isGateRightOpen && m_NextDirection == Direction.Down)
							{
								m_PositionPlayerY = Map.GateY1;
								transform.position = new Vector3 (-LoadMap.ScreenSize.x + Map.SizeBlock * m_PositionPlayerX + Map.SizeBlock/2, 
									LoadMap.ScreenSize.y - Map.SizeBlock * m_PositionPlayerY+Map.SizeBlock/2, 0f);
								m_NextPosition = transform.position;
								GetOut.GameManager.Instance().isGateRightOpen = false;
								GetOut.GameManager.Instance().timeGateRightClose = GameConfig.TIME_DOOR_CLOSE;
							}
							else
								CanNotMove();
				}
				Move(Direction.Down);
				break;
			}

			case (Direction.Up):
			{
				//if player go to the gate
				if(m_PositionPlayerY <= Map.GateY1)
				{
					if(m_PositionPlayerX == Map.GateLeftX && GetOut.GameManager.Instance().isGateLeftOpen && m_NextDirection == Direction.Up)
					{
						m_PositionPlayerY = Map.GateY2;
							transform.position = new Vector3 (-LoadMap.ScreenSize.x + Map.SizeBlock * m_PositionPlayerX + Map.SizeBlock/2, 
								LoadMap.ScreenSize.y - Map.SizeBlock * m_PositionPlayerY+Map.SizeBlock/2, 0f);
						m_NextPosition = transform.position;	
						GetOut.GameManager.Instance().isGateLeftOpen = false;
						GetOut.GameManager.Instance().timeGateLeftClose = GameConfig.TIME_DOOR_CLOSE;
					}else
							if(m_PositionPlayerX == Map.GateRightX && GetOut.GameManager.Instance().isGateRightOpen && m_NextDirection == Direction.Up) 
						{
							m_PositionPlayerY = Map.GateY2;
								transform.position = new Vector3 (-LoadMap.ScreenSize.x + Map.SizeBlock * m_PositionPlayerX + Map.SizeBlock/2, 
									LoadMap.ScreenSize.y - Map.SizeBlock * m_PositionPlayerY+Map.SizeBlock/2, 0f);
							m_NextPosition = transform.position;
								GetOut.GameManager.Instance().isGateRightOpen = false;
								GetOut.GameManager.Instance().timeGateRightClose = GameConfig.TIME_DOOR_CLOSE;
						}
							else
								CanNotMove();
				}
				Move(Direction.Up);
				break;
			}

			case (Direction.Left):
			{
				Move(Direction.Left);
				break;
			}

			case (Direction.Right):
			{
				Move(Direction.Right);
				break;
			}

			case Direction.None:
				{
					if (m_CurrentDirection != m_NextDirection)
						m_CurrentDirection = m_NextDirection;
					break;
				}
			}
		}
	}



	//Function check if next step with direction is posible
	bool CheckMovePosible(Direction direction)
	{
		List<Direction> m_LstDirectionPosible = new List<Direction> ();
		m_LstDirectionPosible.Clear ();
		Node node;
		//Check all node is linked with this node of character
		for(int i = 0; i < LoadMap.map.matrix[m_PositionPlayerY,m_PositionPlayerX].Next.Count; i++)
		{
			node = LoadMap.map.matrix[m_PositionPlayerY,m_PositionPlayerX].Next[i];
			if(node.Position.x == m_PositionPlayerY && node.Position.y > m_PositionPlayerX)
				m_LstDirectionPosible.Add(Direction.Right);
			else if(node.Position.x == m_PositionPlayerY && node.Position.y < m_PositionPlayerX)
				m_LstDirectionPosible.Add(Direction.Left);
			else if(node.Position.y == m_PositionPlayerX && node.Position.x < m_PositionPlayerY)
				m_LstDirectionPosible.Add(Direction.Up);
			else if(node.Position.y == m_PositionPlayerX && node.Position.x > m_PositionPlayerY)
				m_LstDirectionPosible.Add(Direction.Down);
		}
		//if this direction is posible then return true
		if(m_LstDirectionPosible.Contains(direction))
			return true;
		//if next direction is down and is on the gate 
		if(m_PositionPlayerY == Map.GateY2 && direction == Direction.Down){
			if(m_PositionPlayerX == Map.GateLeftX && GetOut.GameManager.Instance().isGateLeftOpen)
				return true;
			if(m_PositionPlayerX == Map.GateRightX && GetOut.GameManager.Instance().isGateRightOpen)
				return true;
		}
		//if next direction is up and is on the gate
		if (m_PositionPlayerY == Map.GateY1 && direction == Direction.Up) {
			if (m_PositionPlayerX == Map.GateLeftX && GetOut.GameManager.Instance ().isGateLeftOpen)
				return true;
			if (m_PositionPlayerX == Map.GateRightX && GetOut.GameManager.Instance ().isGateRightOpen)
				return true;
		}
		return false;
	}

	//Check when have a action touch move
	void CheckTouchMove(Touch touch)
	{
		m_GestureTime = (Time.time - m_FingerStartTime);
		m_GestureDist = (touch.position - m_FingerStartPos).magnitude;
		Vector3 direction = touch.position - m_FingerStartPos;
		Vector2 swipeType = Vector2.zero;
		if (Mathf.Abs (direction.x) <= Mathf.Abs (direction.y)) {
			// the swipe is vertical:
			if (m_GestureDist > m_MinSlideScreenDist) {
				direction = touch.position - m_FingerStartPos;
				if (direction.y > 0) {
					//UP
					m_NextDirection = Direction.Up;
				} else {
					//down
					m_NextDirection = Direction.Down;
				}
			}
		} else
			if (m_GestureDist > m_MinSlideScreenDist) {
				direction = touch.position - m_FingerStartPos;
				if (direction.x > 0) {
					//turn right
					m_NextDirection = Direction.Right;
				} else {
					//turn left
					m_NextDirection = Direction.Left;
				}
			}
		m_FingerStartPos = touch.position;
	}



	//Check when have a action touch end
	void CheckTouchEnd(Touch touch)
	{
		m_GestureTime = Time.time - m_FingerStartTime;
		m_GestureDist = (touch.position - m_FingerStartPos).magnitude;

		if (m_IsSwipe && m_GestureTime < m_MaxSwipeTime && m_GestureDist > m_MinSwipeDist) {
			Vector3 direction = touch.position - m_FingerStartPos;
			Vector2 swipeType = Vector2.zero;

			if (Mathf.Abs (direction.x) > Mathf.Abs (direction.y)) {
				// the swipe is horizontal:
				swipeType = Vector2.right * Mathf.Sign (direction.x);
			} else {
				// the swipe is vertical:
				swipeType = Vector2.up * Mathf.Sign (direction.y);
			}

			if (swipeType.x != 0.0f) {
				if (swipeType.x > 0.0f) {
					// MOVE RIGHT
					m_NextDirection = Direction.Right;
				} else {
					// MOVE LEFT
					m_NextDirection = Direction.Left;
				}
			} 
			if (swipeType.y != 0.0f) {
				if (swipeType.y > 0.0f) {
					//MOVE UP
					m_NextDirection = Direction.Up;
				} else {
					//MOVE DOWN
					m_NextDirection = Direction.Down;
				}
			}
		} else {
			//Don't do
		}
		m_GestureDist = 0;
		m_GestureTime = 0;
	}

	//Function to check swipe screen
	void CheckSwipeScreen ()
	{
		if (Input.touchCount > 0) {
			Touch touch = Input.touches [Input.touchCount - 1];
			switch (touch.phase) {
			case TouchPhase.Began:
				/* this is a new touch */
				Vector3 position = touch.position;
				m_IsSwipe = true;
				m_FingerStartTime = Time.time;
				m_FingerStartPos = touch.position;
				break;
			case TouchPhase.Stationary:
				break;
			case TouchPhase.Canceled:
				/* The touch is being canceled */
				m_IsSwipe = false;
				break;
			case TouchPhase.Moved:
				CheckTouchMove(touch);
				break;
			case TouchPhase.Ended:
				CheckTouchEnd(touch);
				break;
			}
		}
	}


	//Collision
	void OnTriggerEnter2D(Collider2D other)
	{
		if(other.gameObject.tag == GameConfig.TAG_ENEMY && !GetOut.GameManager.Instance().isStar && !m_IsImmortal && !m_IsDead)
		{
			if(GetOut.GameManager.Instance().life > 1)
			{
				m_IsDead = true;
				GetOut.GameManager.Instance().life -= 1;
				GetOut.GameManager.Instance().timeClock = 0;
				PlayAnimation(true);
				Header.Instance.UpdateLife(GetOut.GameManager.Instance().life);
				m_IndexFlick = 0;
				m_TimeFlick = GameConfig.TIME_FLICKER;
				if(ComponentConstant.SOUND_MANAGER != null)
					ComponentConstant.SOUND_MANAGER.Play(SoundEnum.se13_miss);
				Flicker();

			}else
			{
				PlayAnimation(true);
				GetOut.GameManager.Instance().isGameOver = true;
				GetOut.GameManager.Instance().StopAllAnimationEnemy();
				GameFooter.Instance().PauseParticle();
				ComponentConstant.SOUND_MANAGER.Play(SoundEnum.se07_timeup);
				UIManager.Instance.ShowEnding();
			}
		}
	}

	/*
	void OnTriggerStay2D(Collider2D other)
	{
		if(other.gameObject.tag == GameConfig.TAG_ENEMY && !GetOut.GameManager.Instance().isStar && !m_IsImmortal && !m_IsDead)
		{
			if(GetOut.GameManager.Instance().life > 1)
			{
				m_IsDead = true;
				PlayAnimation(true);
				GetOut.GameManager.Instance().life -= 1;
				Header.Instance.UpdateLife(GetOut.GameManager.Instance().life);
				GetOut.GameManager.Instance().timeClock = 0;
				m_IndexFlick = 0;
				m_TimeFlick = GameConfig.TIME_FLICKER;
				ComponentConstant.SOUND_MANAGER.Play(SoundEnum.se13_miss);
				Flicker();
			}else
			{
				PlayAnimation(true);
				GetOut.GameManager.Instance().isGameOver = true;
				GetOut.GameManager.Instance().StopAllAnimationEnemy();
				GameFooter.Instance().PauseParticle();
				ComponentConstant.SOUND_MANAGER.Play(SoundEnum.se07_timeup);
				UIManager.Instance.ShowEnding();

			}
		}
	}
	*/




	//Play animation
	public void PlayAnimation(bool isStop)
	{
		if(isStop)
			switch(m_CurrentDirection)
			{
				case Direction.Down:
					m_Animator.SetInteger(GameConfig.VALUE_ANIMATOR,GameConfig.ANIM_STOP_UNDER);
					break;
				case Direction.Up:
					m_Animator.SetInteger(GameConfig.VALUE_ANIMATOR,GameConfig.ANIM_STOP_UP);
					break;
				case Direction.Left:
					m_Animator.SetInteger(GameConfig.VALUE_ANIMATOR,GameConfig.ANIM_STOP_LEFT);
					break;
				case Direction.Right:
					m_Animator.SetInteger(GameConfig.VALUE_ANIMATOR,GameConfig.ANIM_STOP_RIGHT);
				break;
			}
		else
			switch(m_CurrentDirection)
			{
			case Direction.Down:
				m_Animator.SetInteger(GameConfig.VALUE_ANIMATOR,GameConfig.ANIM_UNDER);
				break;
			case Direction.Up:
				m_Animator.SetInteger(GameConfig.VALUE_ANIMATOR,GameConfig.ANIM_UP);
				break;
			case Direction.Left:
				m_Animator.SetInteger(GameConfig.VALUE_ANIMATOR,GameConfig.ANIM_LEFT);
				break;
			case Direction.Right:
				m_Animator.SetInteger(GameConfig.VALUE_ANIMATOR,GameConfig.ANIM_RIGHT);
				break;
			}
	}

	void Flicker()
	{
		if(m_IndexFlick <  GameConfig.INDEX_FLICKER)
		{
			
			if(m_TimeFlick < 0)
			{
				if(m_IndexFlick % 2 == 0)
				{
					sprite.color = Color.red;
				}else
				{
					sprite.color = Color.white;
				}
				m_IndexFlick ++;
				m_TimeFlick = GameConfig.TIME_FLICKER;
			}else
				m_TimeFlick -= Time.deltaTime;
		}
	}
}
