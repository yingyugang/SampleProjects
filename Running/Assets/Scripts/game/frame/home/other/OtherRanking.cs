using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using UnityEngine.UI;

public class OtherRanking : ViewWithDefaultAction
{
	public Text totalPoint;
	public Text totalRank;
	public Text cardNumber;
	public Text cardRank;
	public Transform container;
	public RankingGameScrollItem instantiation;

	protected override void AddActions ()
	{
		unityActionArray = new UnityAction[] {
			() => {
				Send (0);
			}
		};
	}
}
