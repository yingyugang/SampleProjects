using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using UnityEngine.UI;

public class OtherRankingDetail : ViewWithDefaultAction
{
	public Image iconField;
	public Text pointField;
	public Text rankField;
	public Text titleField;
	public Rank rank;
	public Transform container;
	public RankingScrollItem instantiation;

	protected override void AddActions ()
	{
		unityActionArray = new UnityAction[] {
			() => {
				Send (0);
			}
		};
	}

	public void Show (Sprite iconSprite, int rankOrder, int pointNumber, int rankNumber, string title)
	{
		iconField.sprite = iconSprite;
		rank.SetRank (rankOrder);
		pointField.text = pointNumber.ToString ();
		rankField.text = rankNumber.ToString ();
		titleField.text = title;
	}
}
