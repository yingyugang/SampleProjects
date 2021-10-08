using UnityEngine;
using System.Collections;
using home;

public class ChangeBadgeManager : UpdateFromDataManager
{
	private static int deadLine;
	public static bool isChangeTime;
	private GameObject home;
	private static int serverOverTime;
	public static bool hasOccuredByGacha;
	public GachaTopMediator gachaTopMediator;
	public GameObject changeBadge;

	public void UpdateBadgeManager ()
	{
		serverOverTime = Player.GetInstance.gacha_up_end;
		if (!isChangeTime && serverOverTime > SystemInformation.GetInstance.current_time) {
			deadLine = serverOverTime + TimeUtil.GetTimeDifference ();
			isChangeTime = true;
			UpdateUI (true);
			hasOccuredByGacha = false;
		}
	}

	private void Update ()
	{
		if (isChangeTime) {
			if (TimeUtil.GetCurrentLocalTimeStamp () >= deadLine) {
				UpdateUI (false);
				isChangeTime = false;
				if (gachaTopMediator != null) {
					gachaTopMediator.CloseChangeTime ();
				}
			}
		}
	}

	private void UpdateUI (bool isShow)
	{
		SetChangeBadge (isShow);
	}

	public void ShowChangeBadge ()
	{
		SetChangeBadge (true);
	}

	public void SetChangeBadge (bool isShow)
	{
		if (changeBadge != null) {
			changeBadge.SetActive (isShow);
		}
	}
}
