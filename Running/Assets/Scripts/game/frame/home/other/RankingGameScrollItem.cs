using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using System.Linq;
using System.Collections.Generic;

public class RankingGameScrollItem : RogerInteractiveScrollItem
{
	public GameDetail gameDetail;
	[HideInInspector]
	public int pointNumber;
	[HideInInspector]
	public int rankNumber;
	public Image iconField;
	public Rank rank;
	public Text pointField;
	public Text rankField;
	public GameObject locked;
	[HideInInspector]
	public int rankOrder;

	public void Show (GameDetail gameDetail, int pointNumber, int rankNumber)
	{
		this.gameDetail = gameDetail;
		this.pointNumber = pointNumber;
		this.rankNumber = rankNumber;
		rankOrder = gameDetail.rank;
		rank.SetRank (rankOrder);
		locked.SetActive (gameDetail.islock == 1);
		iconField.sprite = AssetBundleResourcesLoader.gameIconDictionary [string.Format ("{0}{1}", LanguageJP.GAME_ICON_PREFIX, gameDetail.id)];
		pointField.text = gameDetail.score.ToString ();
		rankField.text = rankNumber.ToString ();
		gameObject.SetActive (true);
	}
}
