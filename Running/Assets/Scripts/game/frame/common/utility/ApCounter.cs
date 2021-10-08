using UnityEngine;
using System.Collections;

public class ApCounter : MonoBehaviour
{
	private int recovery_per_time;
	private int new_recovery_per_time;
//	private int new_recovery_per_time_diff;
//	private int new_recovery_per_time_diff_total;
	public const int MAX_RECOVERY = 6;
	private int currentRemainSeconds;
	public static int currentRecovery;
	private float currentTime;
	private int lastUseApTimeStamp;
	private int currentTimeStamp;
	private const int SECONDS = 60;
	private const int MINUTE = 30;
	private int recoveredSeconds;
	private bool isStart;
	private static bool isFirstTime;
	private bool hasReduced;

	public void StartCount ()
	{
		if (!isFirstTime) {
			isStart = true;
			isFirstTime = true;
			recovery_per_time = new_recovery_per_time = GetReduceSeconds ();
			SetApWithServerData ();
		}
	}

	private void OnApplicationPause (bool isPause)
	{
		if (!isPause) {
			GetRemainSeconds ();
		}
	}

	public void SetApWithServerData ()
	{
		GetRemainSeconds ();
	}

	public void SetNewRecoveryPerTime ()
	{
		new_recovery_per_time = GetReduceSeconds ();
		if (new_recovery_per_time != recovery_per_time) {
			if (currentRemainSeconds > new_recovery_per_time) {
				currentRemainSeconds = new_recovery_per_time;	
			}
//			new_recovery_per_time_diff = Mathf.Abs (new_recovery_per_time - recovery_per_time); 
//			int diff = Mathf.Abs (new_recovery_per_time_diff_total - new_recovery_per_time_diff);
//			currentRemainSeconds -= diff;
//			new_recovery_per_time_diff_total += diff;
			hasReduced = true;
		}
	}

	private void GetRecoveryTime ()
	{
		lastUseApTimeStamp = PlayerLoginInfo.GetInstance.last_use_ap_at;
		currentTimeStamp = TimeUtil.GetCurrentServerTimeStamp ();
		recoveredSeconds = currentTimeStamp - lastUseApTimeStamp;	
	}

	private int GetReduceSeconds ()
	{
		float reduceTime = CardRate.GetTotal (1);
		int time = SECONDS * MINUTE - (int)(reduceTime * SECONDS);
		if (time < 1) {
			time = 1;
		}
		return time; 
	}

	private void GetRemainSeconds ()
	{
		if (!isStart) {
			return;
		}
		GetRecoveryTime ();
		int recoverdAp = (int)(recoveredSeconds / recovery_per_time);
		currentRecovery = PlayerLoginInfo.GetInstance.ap + recoverdAp;
		if (!hasReduced) {
			currentRemainSeconds = recovery_per_time - (recoveredSeconds % recovery_per_time);
		}

		if (currentRecovery >= MAX_RECOVERY) {
			currentRecovery = MAX_RECOVERY;
			SetDefaultTime ();
		} else {
			ShowOrHideTime (true);
		}
		SetAp (currentRecovery);
		SetRemainTime (currentRemainSeconds);
	}

	private void FixedUpdate ()
	{
		if (!isStart) {
			return;
		}
		if (currentRecovery < MAX_RECOVERY) {
			ShowOrHideTime (true);
			currentTime += Time.deltaTime;
			if (currentTime > 1) {
				currentTime = 0;
				currentRemainSeconds--;
				if (currentRemainSeconds <= 0) {
					currentRecovery++;
					SetAp (currentRecovery);
					currentRemainSeconds = recovery_per_time = new_recovery_per_time;
//					new_recovery_per_time_diff_total = 0;
					hasReduced = false;
				}
				SetRemainTime (currentRemainSeconds);
			}
//			Debug.Log (recovery_per_time + " " + recoveredSeconds + " " + new_recovery_per_time);
//			Debug.Log (currentRemainSeconds);
		} else {
			SetDefaultTime ();
		}
	}

	private Ap GetApObject ()
	{
		if (GameObject.Find ("Ap") != null) {
			return GameObject.Find ("Ap").GetComponent<Ap> ();	
		} else {
			return null;
		}
	}

	private void SetDefaultTime ()
	{
		Ap ap = GetApObject ();
		if (ap != null) {
			ap.SetDefaultTime ();	
		}
	}

	private void ShowOrHideTime (bool isActive)
	{
		Ap ap = GetApObject ();
		if (ap != null) {
			ap.ShowOrHideTime (isActive);	
		}
	}

	private void SetAp (int count)
	{
		Ap ap = GetApObject ();
		if (ap != null) {
			ap.SetAp (count);	
		}
	}

	private void SetRemainTime (int remainTime)
	{
		Ap ap = GetApObject ();
		if (ap != null) {
			ap.SetRemainTime (remainTime);	
		}
	}
}
