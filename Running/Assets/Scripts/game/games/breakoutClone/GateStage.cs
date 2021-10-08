using UnityEngine;
using System.Collections;

public class GateStage : MonoBehaviour
{
	public float Speed;
	private bool m_IsMove;
	private Vector2 m_ClosePos;
	private Vector2 m_OpenPos;

	void Start ()
	{
		m_OpenPos = new Vector2 (-12, transform.position.y);
		m_ClosePos = new Vector2 (0, transform.position.y);
	}


	void Update ()
	{
		Move ();
	}

	void Move ()
	{
		if (Game8_Manager.instance.GameState == GAMESTATE.PAUSE)
			return;
		if (!m_IsMove) {
			this.transform.position = Vector2.MoveTowards (transform.position, m_ClosePos, Speed);
		} else {
			this.transform.position = Vector2.MoveTowards (transform.position, m_OpenPos, Speed);
		}
			

	}

	public void OpenGate ()
	{
		m_IsMove = true;
	}

	public void CloseGate ()
	{

		Invoke ("DelayClose", 1.5f);

	}

	void DelayClose ()
	{
		m_IsMove = false;
	}

	void OnCollisionEnter2D (Collision2D other)
	{
		if (other.gameObject.tag == BreackoutConfig.TAG_PLAYER) {
			Game8_Manager.instance.PlaySound (SoundEnum.SE22_oden_bound);

		}

	}
}
