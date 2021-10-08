using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using DG.Tweening;

public class BallController : MonoBehaviour
{
	public int Damge = 1;
	// item regulary block
	public float SpeedBall;
	[HideInInspector]
	public Vector3 Angle;
	// Angle when ball split item
	public bool isRunning;
	public Sprite NormalBall;
	public Sprite MetalBall;

	public Sprite CheatNormalBall;
	public Sprite CheatMetalBall;

	private Vector2 m_Velocity;

	private Rigidbody2D m_Rigid;
	public static int s_Count = 0;
	// Count ball
	private Vector3 m_BallPosStart = new Vector3 (.0f, -4.72f, .0f);


	void Awake ()
	{
		Angle = new Vector3 (1f, 1f, 0f);
		m_Rigid = GetComponent<Rigidbody2D> ();


		if(CheatController.IsCheated(0)){
			if(CheatNormalBall!=null)
				NormalBall = CheatNormalBall;
			if (CheatMetalBall)
				MetalBall = CheatMetalBall;
		}


	}

	public void StartNewLife (float time)
	{
		s_Count++;
		Reset ();
		isRunning = true;
		SpeedBall = Game8_Manager.instance.PlayerSpeed;
		transform.DOLocalMove (m_BallPosStart, time).OnComplete (() => EnableCollider ());
	
	}

	void EnableCollider ()
	{
		gameObject.GetComponent<CircleCollider2D> ().enabled = true;
	}

	public void AddForceBall ()
	{
		Vector2 dir = Angle.normalized;
		m_Rigid.velocity = dir * SpeedBall * BreackoutConfig.SPEEDBALL;
	}
	//--> Init Ball Item Slit
	public void Init ()
	{
		s_Count++;
		isRunning = true;
		EnableCollider ();
		AddForceBall ();
	}
	//--<

	void OnTriggerEnter2D (Collider2D col)
	{
		if (col.gameObject.name == BreackoutConfig.CHECK_WIN) {	
			Reset ();
			Game8_Manager.instance.Racket.Reset ();
			Game8_Manager.instance.NewStage ();		
		}

		if (col.gameObject.name == BreackoutConfig.CHECK_GAMEOVER) {
			Reset ();
			CheckLife ();						
		}
	}

	void CheckLife ()
	{
		Debug.Log ("life: " + s_Count);							
		if (s_Count <= 0) {
			Game8_Manager.instance.PlaySound (SoundEnum.se13_miss);
			Game8_Manager.instance.CheckGameOver ();
		}
	}


	public void Reset ()
	{
		if (m_Rigid == null)
			m_Rigid = GetComponent<Rigidbody2D> ();
		transform.position = Game8_Manager.instance.Balls.ResetPosBall;
		m_Rigid.velocity = Vector3.zero;
		gameObject.GetComponent<CircleCollider2D> ().enabled = false;
		ChangeBall (1, NormalBall);
		Damge = 1;
		Angle = new Vector3 (1f, 1f, 0f);
		if (isRunning) {
			s_Count--;
			isRunning = false;
		}

	}
	//--> Up speed Ball 
	public void UpSpeed (float delta)
	{
		SpeedBall += delta;
	}
	//--<

	public void ChangeBall (int dam, Sprite spr)
	{
		Damge = dam;
		GetComponentInChildren<SpriteRenderer> ().sprite = spr;
	}

	public void PauseBall (bool isPause)
	{
		if (isRunning && Game8_Manager.instance.GameState != GAMESTATE.START) {
			
			if (gameObject.GetComponent<Rigidbody2D> ().velocity != Vector2.zero) {
				m_Velocity = gameObject.GetComponent<Rigidbody2D> ().velocity;
			}
			if (isPause) {
				gameObject.GetComponent<Rigidbody2D> ().isKinematic = true;

			} else {
				gameObject.GetComponent<Rigidbody2D> ().isKinematic = false;
				gameObject.gameObject.GetComponent<Rigidbody2D> ().velocity = m_Velocity;
			}
		}
	}
}
