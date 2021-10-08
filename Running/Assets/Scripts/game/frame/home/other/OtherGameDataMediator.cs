using UnityEngine;
using System.Collections;

public class OtherGameDataMediator : MonoBehaviour
{
	public OtherGameData otherGameData;

	public void SetData (string title, string rank, int score, int ranking)
	{
		otherGameData.title.text = title;
		otherGameData.rank.text = rank;
		otherGameData.score.text = string.Format ("{0}{1}", score, LanguageJP.PT);
		otherGameData.ranking.text = ranking.ToString ();
	}

}
