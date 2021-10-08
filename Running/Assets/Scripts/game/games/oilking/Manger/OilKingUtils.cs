using UnityEngine;
using System.Collections;

public class OilKingUtils
{

	public static bool isRunGame = false;
	public static bool isPauseGame = false;
	public static int numPine = 0;
	public static int numOden = 0;
	public static int numOnigiri = 0;
	public static int scorebonus = 0;
	public static int MY_SCORE = 0;
	public static int MY_COIN = 0;
	public static float MY_TIME = 0;
	public static int THROW_BOMB = 0;
	public static int HIT_BOMB = 0;
	public static string current_group="";
	public static int ID_SERIF=-1; //id serif was choise


	public static bool ResultMissionSixBrothersMeet()
	{
		return PanelTalkOilKing.s_Instance.CheckSixBrothersMeet();
	}

	public static bool ResultMissionDontThrowAnyBomb()
	{
		if (HIT_BOMB == 0 && OilKingGamePlay.Instance.checkThowItem)
			return true;
		return false;
	}
}