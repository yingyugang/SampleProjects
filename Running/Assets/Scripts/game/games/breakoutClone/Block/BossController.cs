using UnityEngine;
using System.Collections;
using DG.Tweening;

public class BossController : MonoBehaviour
{

	private Transform m_ImgBoss;
	private Transform m_TargetPos;
	private int m_IndexTransf = 0;

	private bool m_IsMoved = false;
	private bool m_IsRotated = false;
	private bool m_IsParaboled = false;

	private float m_SpeedBoss;
	private float m_DeltaAngle = .0f;
	private int[] m_ChosenAction;
	private int m_IndexChosen = 0;

	private ActionBoss m_CurrentAction;

	void Start ()
	{
		
		m_ImgBoss = GetComponentInChildren<SpriteRenderer> ().transform;
		m_SpeedBoss = ActionBossManager.instance.SpeedBoss;
	}

	void Move ()
	{
		if (m_IsMoved) {
//			Debug.Log ("Boss Move");
			transform.position = Vector3.MoveTowards (transform.position, m_TargetPos.position, m_SpeedBoss * Time.deltaTime);
			if (Vector3.Distance (transform.position, m_TargetPos.position) <= float.Epsilon) {
				m_IsMoved = false;
				DelayRate ();
			}
		}
	}

	void Rotate ()
	{
		if (m_IsRotated) {
//			Debug.Log ("Boss Rotate");
			transform.RotateAround (ActionBossManager.instance.CenterPos.position, Vector3.back, m_SpeedBoss);
			m_ImgBoss.transform.Rotate (Vector3.back, -m_SpeedBoss);
			m_DeltaAngle += m_SpeedBoss;

			if (m_TargetPos.localEulerAngles.z - m_DeltaAngle <= 1.0f) {
				m_DeltaAngle = .0f;
				m_IsRotated = false;
				DelayRate ();
//				Debug.Log ("Change");
			}
		}

	}

	void Parabol ()
	{
		if (m_IsParaboled) {
//			Debug.Log ("Boss Parabol");
			transform.RotateAround (m_TargetPos.position, Vector3.back, m_SpeedBoss);
			m_ImgBoss.transform.Rotate (Vector3.back, -m_SpeedBoss);
			m_DeltaAngle += Mathf.Abs (m_SpeedBoss);
	
			if (m_TargetPos.localEulerAngles.z - m_DeltaAngle <= 1.0f) {
				m_DeltaAngle = .0f;
				m_IsParaboled = false;
				DelayRate ();
//				Debug.Log ("Change");
				if (Mathf.Abs (transform.localEulerAngles.z - 180.0f) <= 5.0f) {
					m_SpeedBoss = -m_SpeedBoss;
				}
			}
		}

	}

	void Update ()
	{
		if (Game8_Manager.instance.GameState == GAMESTATE.END)
			return;
		Move ();
		Rotate ();
		Parabol ();
	}

	void DelayRate ()
	{
		if (Random.Range (0, 100) < m_CurrentAction.X_Percent) {
			Invoke ("DelayRate", m_CurrentAction.Y_Time);
		} else {
			SetMove ();
		}
//		SetMove();
	}

	// time delay action
	public IEnumerator DelayAction (float dtime, System.Action callback)
	{
		yield return new WaitForSeconds (dtime);
		callback ();
	}

	void SetMove ()
	{
		m_IndexTransf++;
		if (m_IndexTransf >= m_CurrentAction.ListTransf.Length) {
			m_IndexTransf = 0;
			NextAction ();
		}

		m_TargetPos = m_CurrentAction.ListTransf [m_IndexTransf];

		if (m_IndexTransf >= 2 && m_CurrentAction.Id == 6) {
			m_IsRotated = true;
		} else if (m_IndexTransf >= 1 && m_CurrentAction.Id == 7) {
			m_IsParaboled = true;
		} else {
			m_IsMoved = true;
		}

	}

	void NextAction ()
	{
		if (m_IndexChosen >= m_ChosenAction.Length || m_ChosenAction [m_IndexChosen] <= 0) {
			m_IndexChosen = 0;
		}
//		Debug.Log ("Index Action: " + m_ChosenAction [m_IndexChosen]);
		int id = m_ChosenAction [m_IndexChosen++];
		m_CurrentAction = ActionBossManager.instance.GetActionById (id);
		if (m_CurrentAction == null)
			NextAction ();
	}

	public void InitBoss (int[] pattern)
	{
		transform.localPosition = new Vector3 (5.0f, 6.0f, .0f);
		gameObject.transform.DOMove (ActionBossManager.instance.CenterPos.position, 3.0f, false).OnComplete (TargetPos);
		m_ChosenAction = new int[pattern.Length];
		for (int i = 0; i < pattern.Length; i++) {
			m_ChosenAction [i] = pattern [i];
		}
		NextAction ();
		m_TargetPos = m_CurrentAction.ListTransf [m_IndexTransf];
	}

	void TargetPos ()
	{
		m_IsMoved = true;
	}
	//--> Move Boss When Ball Collison
	public void BossMove ()
	{
		Vector2 posMove = Random.insideUnitCircle * 0.05f;
		transform.position -= new Vector3 (transform.position.x*posMove.x, transform.position.y*posMove.y, .0f);
	}//--<
}
