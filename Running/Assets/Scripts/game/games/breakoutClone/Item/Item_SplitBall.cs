using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Item_SplitBall : Item
{
	private int TotalBall = 3;
	private float Angle = 30f;

	void Start ()
	{
		Rigid = GetComponent<Rigidbody2D> ();
	}

	public override void Effect ()
	{	
		Game8_Manager.instance.PlaySound (SoundEnum.se12_bonusitem);	
		List<BallController> runningball = Game8_Manager.instance.Balls.GetRunningBall();
		if(runningball.Count >= 3 || runningball.Count <= 0) return;

		BallController ballSplit = runningball[Random.Range(0, runningball.Count)];	
		Vector3 dir = ballSplit.GetComponent<Rigidbody2D>().velocity.normalized;

		for(int i = 0; i < (TotalBall - runningball.Count); i++){
			BallController ball = Game8_Manager.instance.Balls.GetBall();
			ball.transform.position = ballSplit.transform.position;
			ball.SpeedBall = ballSplit.SpeedBall;
			if(i == 0) ball.Angle = RotateToAngle(dir, Angle);
			else ball.Angle = RotateToAngle(dir, -Angle);
			ball.Init();
		}
	}

	Vector3 RotateToAngle(Vector3 dir, float angle){
		angle  = Mathf.Deg2Rad * angle;
		return new Vector3(dir.x * Mathf.Cos(angle) - dir.y * Mathf.Sin(angle), dir.x * Mathf.Sin(angle) + dir.y * Mathf.Cos(angle), 0f);
	}
}
