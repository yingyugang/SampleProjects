using UnityEngine;
using System.Collections;

public class Item_SuperRacket : Item
{
	public float ScaleRacket;
	private RacketController m_Racket;
	private bool m_IsLoaded;
	private float m_ValueStart;
	void Start ()
	{
		m_ValueStart = EffectValue;
		m_IsLoaded = false;
		Rigid = GetComponent<Rigidbody2D> ();
		m_Racket = Game8_Manager.instance.Racket;
	}

	public override void Effect ()
	{	
		Game8_Manager.instance.PlaySound (SoundEnum.se12_bonusitem);	

		if (m_IsLoaded) {
			
			//CancelInvoke ("Specific_Reset");
			StopAllCoroutines();
			m_ValueStart = EffectValue;
			//Invoke ("Specific_Reset", EffectValue);
			//m_Racket.ChangeRacket (2.0f, m_Racket.LongCol, m_Racket.LongRacketImage);
			StartCoroutine (DelayAction (m_ValueStart, () => Specific_Reset ()));
			Debug.Log ("vaulie" + EffectValue);

			return;
		}
		m_IsLoaded = true;
		m_Racket.ChangeRacket (2.0f, m_Racket.LongCol, m_Racket.LongRacketImage);
		StartCoroutine (DelayAction (m_ValueStart, () => Specific_Reset ()));
	}

	public override void Specific_Reset ()
	{
		m_IsLoaded = false;
		m_Racket.ChangeRacket(1.5f, m_Racket.NormalCol, m_Racket.NormalRacketImage);
		StopAllCoroutines ();
	}		
}
