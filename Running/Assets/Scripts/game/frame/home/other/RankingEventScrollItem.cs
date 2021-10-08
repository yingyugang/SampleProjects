using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine.Events;

public class RankingEventScrollItem : RankingScrollItem
{
	public GameObject dot;

	override protected void InitData (RankingData rankingData)
	{
		base.InitData (rankingData);
		dot.SetActive (rankingData.player_id == Player.GetInstance.id && rankingData.id == 0);
	}
}
