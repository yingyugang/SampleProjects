using UnityEngine;
using System.Collections;

public class Block_Detail : MonoBehaviour
{

	public int Id;
	public int Strong;
	public bool IsKeyBlock;
	public SpriteRenderer block;
	public bool IsLoaded;
	public Transform Father;

	private int m_BlockStrong;
	private int m_minStrong;
	private int m_maxStrong;
	private float m_TimeShaking;
	private Vector2 m_StartBlock;
	private bool is_Collison;
	private float m_RadiusShaking;
	private BossController BossCtl;


	void Start ()
	{
		BossCtl = transform.parent.gameObject.GetComponent<BossController> ();
		Init ();
	}

	void Init ()
	{
		is_Collison = false;
		m_RadiusShaking = .05f;
		m_BlockStrong = Strong;
		m_TimeShaking = 0.2f;
	}
	//--> Handing collision with ball
	public void CollisionBlock (BallController ball)
	{
		m_StartBlock = transform.localPosition;
		BlockVibrate ();
		if (Strong != 0) {
			m_BlockStrong -= ball.Damge;
			if (m_BlockStrong <= 0) {
				Game8_Manager.instance.ScoreBreak++;
				BreakBlock ();
			}
			bool isBoss = Game8_Manager.instance.BlockManager.CheckBossStage (Game8_Manager.instance.Stage);
			if (isBoss) {
				block.color = ColorManager.instance.GetColorByID (Mathf.RoundToInt ((float)m_BlockStrong / Strong * 100), isBoss);
			} else {
				block.color = ColorManager.instance.GetColorByID (m_BlockStrong, isBoss);
			}

		} else {
			block.color = Color.grey;
			if (ball.Damge >= Game8_Manager.instance.Balls.MAXDAMGE) {
				m_BlockStrong -= ball.Damge;
				BreakBlock ();
			}
		}
			
		// Check stage Boss 
		if (Game8_Manager.instance.BlockManager.CheckBossStage (Game8_Manager.instance.Stage)) {
			Game8_Manager.instance.PlaySound (SoundEnum.SE25_oden_bossblock);
			BossCtl.BossMove ();
			DropItem ();
		}
	}
	//--<

	void BreakBlock ()
	{
		
		CheckKeyBlock ();
		gameObject.SetActive (false);
		block.color = Color.white;
		DropItem ();
		if (Game8_Manager.instance.BlockManager.CheckBossStage (Game8_Manager.instance.Stage)) {
			Game8_Manager.instance.PlaySound (SoundEnum.SE28_oden_bossfanfare);
			Game8_Manager.instance.Defeat_Boss_Num++;
		} else {
			Game8_Manager.instance.PlaySound (SoundEnum.SE23_oden_break);
		}
	}



	void DropItem ()
	{
		Game8_Manager.instance.ItemManager.DropItem (transform.position);

	}

	void CheckKeyBlock ()
	{
		if (IsKeyBlock) {
			Game8_Manager.instance.LoseBlock ();		
		}
	}

	void OnCollisionEnter2D (Collision2D col)
	{
		//--> If get Item Regularly 
		if (Game8_Manager.instance.isRegularly) {
			if (col.gameObject.tag == BreackoutConfig.TAG_PLAYER) {
				CollisionBlockBreak (col); 
			}
		}
		//--<
	}

	void OnCollisionExit2D (Collision2D col)
	{
		if (Game8_Manager.instance.isRegularly == false) {
			if (col.gameObject.tag == BreackoutConfig.TAG_PLAYER) {
				Game8_Manager.instance.PlaySound (SoundEnum.SE24_oden_block);
				CollisionBlockBreak (col); 
			}
		}
		Invoke ("ResetColor", 0.2f);
	}
	//-->
	void CollisionBlockBreak (Collision2D col)
	{
		BallController ball = col.gameObject.GetComponent<BallController> ();
		CollisionBlock (ball);

	}

	void ResetColor ()
	{
		block.color = Color.white;
	}

	public void BlockVibrate ()
	{
		is_Collison = true;

	}


	void Update ()
	{
		if (is_Collison == true) {
			Shaking ();
		}
	}
	//--> Shaking block
	void Shaking ()
	{

		Vector2 shaking = Random.insideUnitCircle * m_RadiusShaking;
		transform.localPosition = new Vector2 (m_StartBlock.x + shaking.x, m_StartBlock.y + shaking.y);
		Invoke ("ResetBlock", m_TimeShaking);

	}
	//--<

	void ResetBlock ()
	{
		is_Collison = false;
		transform.localPosition = m_StartBlock;
	}
}
