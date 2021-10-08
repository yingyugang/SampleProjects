using UnityEngine;
using System.Collections;
using DG.Tweening;

public class RacketController : MonoBehaviour
{
	
	public int SpeedRacket;
	public float MaxWidth;
	public GameObject Bang;
	private Vector2 m_RacketPosStart;
	//Long Racket
	public float NormalCol;
	public float LongCol;
	public Sprite LongRacketImage;
	public Sprite NormalRacketImage;
	private BoxCollider2D m_ColRacket;
	private Animator m_Animator;
	private float m_MouseYLimit = -4.72f;
	// Limit racket position
	private float m_LimitRacketX = 5.4f;
	private Transform m_Ball;
	private bool m_IsDrag;
	void Start ()
	{
		m_Ball = Game8_Manager.instance.BallStart.transform;
		Bang.SetActive (false);
		MaxWidth = GetWidth (1.5f);
		m_Animator = gameObject.GetComponent<Animator> ();
		m_RacketPosStart = transform.position;	
		m_ColRacket = GetComponent<BoxCollider2D> ();
		Reset ();
	}
	void OnMouseDown(){
        m_IsDrag = true;
    }


	void OnMouseUp ()
	{	
		if (!GameCountDownMediator.didEndCountDown) {
			return;
		}
		if (m_IsDrag) {
			Game8_Manager.instance.StartBall = true;
		}
	}

	void OnMouseDrag ()
	{
		if (!GameCountDownMediator.didEndCountDown) {
			return;
		}
		Vector3 paddlePos = new Vector3 (0f, this.transform.position.y, 0f);
		if (Game8_Manager.instance.GameState == GAMESTATE.START || Game8_Manager.instance.GameState == GAMESTATE.PLAYING) {
			if (m_IsDrag) {
				float mousePosInBlocks = Camera.main.ScreenToWorldPoint (Input.mousePosition).x;
				float mouseY = Camera.main.ScreenToWorldPoint (Input.mousePosition).y;

				paddlePos.x = Mathf.Clamp (mousePosInBlocks, -MaxWidth, MaxWidth);
				if (mouseY > m_MouseYLimit) {
					return;	
				}
				this.transform.position = Vector3.MoveTowards (transform.position, paddlePos, SpeedRacket * Time.fixedDeltaTime);
			}
			if (Game8_Manager.instance.GameState == GAMESTATE.START) {
				m_Ball.position = Vector3.MoveTowards (m_Ball.position, new Vector3 (paddlePos.x, m_Ball.position.y, 0f), SpeedRacket * Time.fixedDeltaTime);
			}

		}
	}


	//--<
	//--> get width limit raket
	float GetWidth (float _width)
	{
		return m_LimitRacketX - _width;

	}
	//--<

	public void Reset ()
	{
		m_IsDrag = false;
		m_IsBlinked = false;
		Bang.SetActive (false);
		Bang.transform.localScale = Vector3.one;
		CancelInvoke ("HideBlinking");
		m_Animator.CrossFade (BreackoutConfig.ANIM_RACKETDEFAULT, 0f);
		transform.position = m_RacketPosStart;
		ChangeRacket (1.0f, NormalCol, NormalRacketImage);
		MaxWidth = GetWidth (1.5f);
	}
	//--> Change Sprite Raket When get Item Scale
	public void ChangeRacket (float width, float col, Sprite spr)
	{
		MaxWidth = GetWidth (width);
		m_ColRacket.size = new Vector2 (col, m_ColRacket.size.y);
		GetComponentInChildren<SpriteRenderer> ().sprite = spr;
	}
	//--<

	//--> Delay Raket Show When Next Stage
	public void DelayRacketShow ()
	{
		//Game8_Manager.instance.StartBall = false;
		gameObject.transform.position = m_RacketPosStart;
	}
	//--<

	private bool m_IsBlinked = false;
	//--> Blinking Racket When Collison Boom
	public void RacketBlinking ()
	{
		if (m_IsBlinked)
			return;
		m_IsBlinked = true;
		m_Animator.CrossFade (BreackoutConfig.ANIM_BLINK, 0f);
	}
	//--<

	public void ActiveBang(){
		Game8_Manager.instance.PlaySound (SoundEnum.SE35_bike_explosion);	
		Bang.SetActive (true);
		Bang.transform.DOScale (new Vector3(.1f,.1f,.1f),0.2f);
	}

	public void HideBlinking ()
	{
		Debug.Log ("hide");
		gameObject.SetActive (false);


	}
}


