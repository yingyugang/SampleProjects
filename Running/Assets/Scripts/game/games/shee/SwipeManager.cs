using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;

public enum Swipe {
	None = 0,
	Up,
	Down,
	Left,
	Right
};
public class SwipeManager : MonoBehaviour {
	public GamePlay GameRun;

	//Area NOT Allow Swipe
	public GameObject BottomUI;

	//Flick Sensitivity
	private float m_MinSwipeLength = 200f;
	public  float MinSwipeDelta = 0.5f;

	//Press Position
	Vector2 m_FirstPressPos;
	Vector2 m_SecondPressPos;
	Vector2 m_CurrentSwipe;

	public static Swipe s_SwipeDirection;
	private bool isAllowSwipe = false;

	void Start(){
		m_MinSwipeLength = Game10_Manager.instance.FlickSensitivity;
	}

	void Update ()
	{
		if(!GameRun.IsSwiped || Game10_Manager.instance.GameState != SHEESTATE.PLAYING) return;

		#if UNITY_EDITOR
			MouseSwipe();
		#else
			TouchSwipe();
		#endif
	}

	public void AllowSwipe(){
		isAllowSwipe = true;
	}

	bool CheckDistanceSwipe(){
		//create vector from the two points
		m_CurrentSwipe = new Vector2(m_SecondPressPos.x - m_FirstPressPos.x, m_SecondPressPos.y - m_FirstPressPos.y);
		// Make sure it was a legit swipe, not a click
		if (m_CurrentSwipe.magnitude < m_MinSwipeLength) {
			s_SwipeDirection = Swipe.None;
			return false;
		}
		return true;
	}


	void TouchSwipe ()
	{
		if (Input.touches.Length > 0) {
			Touch t = Input.GetTouch(0);
			if (t.phase == TouchPhase.Began) {				
				m_FirstPressPos = new Vector2(t.position.x, t.position.y);
			}
			if(t.phase == TouchPhase.Moved){
				//Check Area Swipe........
				if(!isAllowSwipe){
					s_SwipeDirection = Swipe.None;
					return;
				}
				//........................
				m_SecondPressPos = new Vector2(t.position.x, t.position.y);
				if(CheckDistanceSwipe()){
					isAllowSwipe = false;
					GetDirection();
				}
			}
			if (t.phase == TouchPhase.Ended) {

				//Check Area Swipe........
				if(!isAllowSwipe){
					s_SwipeDirection = Swipe.None;
					return;
				}
				//........................
				//save ended touch 2d point
				m_SecondPressPos = new Vector2(t.position.x, t.position.y);
				isAllowSwipe = false;
				if(!CheckDistanceSwipe()) return;
				GetDirection();
			}
		} else {
			s_SwipeDirection = Swipe.None;
		}
	}

	void MouseSwipe()
	{
		if(Input.GetMouseButtonDown(0))
		{			
			//save began touch 2d point
			m_FirstPressPos = new Vector2(Input.mousePosition.x,Input.mousePosition.y);
		}
		if(Input.GetMouseButton(0)){
			//Check Area Swipe........
			if(!isAllowSwipe){
				s_SwipeDirection = Swipe.None;
				return;
			}
			//........................
			m_SecondPressPos = new Vector2(Input.mousePosition.x,Input.mousePosition.y);
			if(CheckDistanceSwipe()){
				isAllowSwipe = false;
				GetDirection();
			}

		}
		if(Input.GetMouseButtonUp(0))
		{
			//Check Area Swipe........
			if(!isAllowSwipe){
				s_SwipeDirection = Swipe.None;
				return;
			}
			//........................

			//save ended touch 2d point
			m_SecondPressPos = new Vector2(Input.mousePosition.x,Input.mousePosition.y);
			isAllowSwipe = false;
			if(!CheckDistanceSwipe()) return;
			GetDirection();
		}
	}

	void GetDirection(){
		//normalize the 2d vector
		m_CurrentSwipe.Normalize();
		//swipe upwards
		if(m_CurrentSwipe.y > 0 && m_CurrentSwipe.x > -MinSwipeDelta && m_CurrentSwipe.x < MinSwipeDelta)
		{
			s_SwipeDirection = Swipe.Up;
		}
		//swipe down
		else if(m_CurrentSwipe.y < 0 && m_CurrentSwipe.x > -MinSwipeDelta && m_CurrentSwipe.x < MinSwipeDelta)
		{
			s_SwipeDirection = Swipe.Down;
		}
		//swipe left
		else if(m_CurrentSwipe.x < 0 && m_CurrentSwipe.y > -MinSwipeDelta && m_CurrentSwipe.y < MinSwipeDelta)
		{
			s_SwipeDirection = Swipe.Left;
		}
		//swipe right
		else if(m_CurrentSwipe.x > 0 && m_CurrentSwipe.y > -MinSwipeDelta && m_CurrentSwipe.y < MinSwipeDelta)
		{
			s_SwipeDirection = Swipe.Right;
		}else {
			s_SwipeDirection = Swipe.None;
		}
		GameRun.Answer(s_SwipeDirection);
	}
}