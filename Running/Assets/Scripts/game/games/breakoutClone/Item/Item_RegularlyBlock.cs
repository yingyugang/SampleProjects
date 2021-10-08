using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Item_RegularlyBlock : Item
{
	void Start ()
	{
		Rigid = GetComponent<Rigidbody2D> ();

	}


	public override void Effect ()
	{
		Game8_Manager.instance.PlaySound (SoundEnum.se12_bonusitem);	
		Game8_Manager.instance.isRegularly = true;
		StartEffect (Game8_Manager.instance.Balls.MAXDAMGE);
	}
	//--> Change Damage Ball
	void StartEffect (int dam)
	{
		List<BallController> runningball = Game8_Manager.instance.Balls.GetRunningBall ();
		foreach (BallController ball in runningball) {
			ball.ChangeBall (Game8_Manager.instance.Balls.MAXDAMGE, ball.MetalBall);
		}
		StopAllCoroutines ();
		//CancelInvoke ("Specific_Reset");
		//Invoke ("Specific_Reset", EffectValue);
		StartCoroutine(DelayAction(EffectValue, () => Specific_Reset()));
	}
//--<

	public override void Specific_Reset ()
	{
		Debug.Log ("Specific_Reset steal ball");
		Game8_Manager.instance.isRegularly = false;
		List<BallController> runningball = Game8_Manager.instance.Balls.GetRunningBall ();
		foreach (BallController ball in runningball) {
			ball.ChangeBall (1, ball.NormalBall);
		}
		//	CancelInvoke ("Specific_Reset");
		StopAllCoroutines ();
	}
}
