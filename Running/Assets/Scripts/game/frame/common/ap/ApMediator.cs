using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using home;

public class ApMediator : MonoBehaviour
{
	public Ap ap;
	private int recovery_per_time;
	public const int MAX_RECOVERY = 6;
	private int currentRemainSeconds;
	public static int currentRecovery;
	private float currentTime;
	private int lastUseApTimeStamp;
	private int currentTimeStamp;
	private const int SECONDS = 60;
	private const int MINUTE = 30;

	private void Start ()
	{
		SetApWithServerData ();
	}

	private void OnApplicationPause (bool isPause)
	{
		if (!isPause) {
			GetRemainSeconds ();
		}
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

	public void SetApWithServerData ()
	{
		GetRemainSeconds ();
	}

	private void GetRemainSeconds ()
	{
		recovery_per_time = GetReduceSeconds ();
		lastUseApTimeStamp = PlayerLoginInfo.GetInstance.last_use_ap_at;
		currentTimeStamp = TimeUtil.GetCurrentServerTimeStamp ();

		int recoveredSeconds = currentTimeStamp - lastUseApTimeStamp;
		int recoverdAp = (int)(recoveredSeconds / recovery_per_time);
		currentRecovery = PlayerLoginInfo.GetInstance.ap + recoverdAp;
		if (currentRecovery >= MAX_RECOVERY) {
			currentRecovery = MAX_RECOVERY;
			ap.SetDefaultTime ();
		} else {
			ap.ShowOrHideTime (true);
		}
		currentRemainSeconds = recovery_per_time - (recoveredSeconds % recovery_per_time);
		if (currentRemainSeconds <= 0) {
			currentRemainSeconds = recovery_per_time;
		}
		ap.SetAp (currentRecovery);
		ap.SetRemainTime (currentRemainSeconds);
	}

	private void Update ()
	{
		if (currentRecovery < MAX_RECOVERY) {
			ap.ShowOrHideTime (true);
			currentTime += Time.deltaTime;
			if (currentTime > 1) {
				currentTime = 0;
				currentRemainSeconds--;
				if (currentRemainSeconds <= 0) {
					currentRecovery++;
					ap.SetAp (currentRecovery);
					currentRemainSeconds = recovery_per_time;
				}
				ap.SetRemainTime (currentRemainSeconds);
			}
		} else {
			ap.SetDefaultTime ();
		}
	}
}
