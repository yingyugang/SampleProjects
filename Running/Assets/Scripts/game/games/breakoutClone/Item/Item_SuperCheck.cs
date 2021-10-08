using UnityEngine;
using System.Collections;
using DG.Tweening;

public class Item_SuperCheck : Item
{
	public Transform Paddle;
	private bool m_Scalle;
	public float TimeScale;
	public float MinScale;
	private bool m_IsLoaded;
	private float m_ValueStart;

	void Start ()
	{
		TimeScale = 5.0f;
		m_IsLoaded = false;
		Rigid = GetComponent<Rigidbody2D> ();
		m_ValueStart = EffectValue;
	}

	public override void Effect ()
	{
		Game8_Manager.instance.PlaySound (SoundEnum.se12_bonusitem);	
		if (m_IsLoaded) {
			m_ValueStart = EffectValue;
			Paddle.localScale = Vector3.one;
			//CancelInvoke("ScalePaddle");
			DOTween.Kill (BreackoutConfig.PADDLE);
			StopAllCoroutines ();
			//Invoke ("ScalePaddle", EffectValue - TimeScale);
			StartCoroutine (DelayAction (m_ValueStart - TimeScale, () => ScalePaddle ()));
			return;

		}
		if (m_ValueStart < TimeScale) {
			TimeScale = m_ValueStart/2;
		}
		Paddle.gameObject.SetActive (true);
		m_IsLoaded = true;
		StartCoroutine (DelayAction (m_ValueStart - TimeScale, () => ScalePaddle ()));
		//Invoke ("ScalePaddle", EffectValue - TimeScale);
		Debug.Log ("Vaule EFF" + m_ValueStart);
	}

	void ScalePaddle ()
	{
		if (Game8_Manager.instance.GameState == GAMESTATE.PLAYING) {
			Paddle.DOScaleY (MinScale, TimeScale).OnComplete (() => Specific_Reset ()).SetId (BreackoutConfig.PADDLE);
		}
	
	}

	public override void Specific_Reset ()
	{
		Paddle.gameObject.SetActive (false);
		m_IsLoaded = false;
		Paddle.localScale = Vector3.one;
	}
}
