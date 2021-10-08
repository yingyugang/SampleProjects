using UnityEngine;
using System.Collections;

public class Item : MonoBehaviour
{

	public int Id;
	public int AppearProbality;
	public int EffectValue;
	public int ReductionPercentage;
	public SpriteRenderer Img;
	public bool isChosen;
	public bool isRunning = false;
	[HideInInspector]
	public  Rigidbody2D Rigid;
	private  Vector3 ResetPos = new Vector3 (-11f, 0f, 0f);
	private bool m_IsTrigged;

	public virtual void Effect ()
	{
		
	}

	public virtual void Specific_Reset ()
	{
		
	}

	public void ResetItem ()
	{
		General_Reset ();
		Specific_Reset ();
	}
	//--> Check Trigger Item
	void OnTriggerEnter2D (Collider2D col)
	{		
		if (col.gameObject.name == BreackoutConfig.RACKET) {
			if (m_IsTrigged)
				return;
			
			m_IsTrigged = true;
			Effect ();
			General_Reset ();
		}

		if (col.gameObject.name == BreackoutConfig.CHECK_GAMEOVER) {			
			General_Reset ();
		}
	}
//--<

	void OnTriggerExit2D (Collider2D col)
	{
		if (col.gameObject.name == BreackoutConfig.RACKET) {
			m_IsTrigged = false;
		}
	}
	// Reset  item 
	void General_Reset ()
	{
		isRunning = false;
		Rigid.isKinematic = true;
		Rigid.velocity = Vector2.zero;
		transform.localPosition = ResetPos;
		isChosen = false;
	}
	//--> Move Item to Start Position when collision Racket
	public void DropItem (Vector3 pos)
	{
		isRunning = true;
		transform.position = pos;
		Rigid.isKinematic = false;
		isChosen = true;
	}
//--<
	public IEnumerator DelayAction(float dtime, System.Action callback)
	{
		float timeDelay = 0f;
		while (timeDelay < dtime) {
			if(Game8_Manager.instance.GameState == GAMESTATE.PLAYING){
				timeDelay += Time.deltaTime;
			}
			yield return new WaitForEndOfFrame();


		}
		yield return new WaitForEndOfFrame();
		//		yield return new WaitForSeconds(dtime);
		callback();
	}
}