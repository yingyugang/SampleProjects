using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class BallManager : MonoBehaviour
{
	public BallController[] ListBall;
	public Vector3 ResetPosBall = new Vector3 (0f, -11f, 0f);
	public int MAXDAMGE;
	void Awake ()
	{
		SetLayer ();
		ResetAllBall ();

	}

	void SetLayer ()
	{
		Physics2D.IgnoreLayerCollision (8, 8);
	}


	public BallController GetBall ()
	{
		foreach (BallController ball in ListBall) {
			if (!ball.isRunning) {
				return ball;
			}
		}
		return ListBall [ListBall.Length - 1];
	}

	public void ResetAllBall ()
	{
		
		foreach (BallController ball in ListBall) {
			ball.Reset ();
		}
	}

	public void PauseAllBall(bool isPause){
		foreach (BallController ball in ListBall) {
			ball.PauseBall (isPause);
		}
	}


	public List<BallController> GetRunningBall ()
	{
		List<BallController> runningball = new List<BallController> ();
		foreach (BallController ball in ListBall) {
			if (ball.isRunning) {
				runningball.Add (ball);
			}
		}
		return runningball;
	}

	public BallController GetFirstRunningBall ()
	{
		foreach (BallController ball in ListBall) {
			if (ball.isRunning) {
				return ball;
			}
		}
		return null;
	}
		
}
