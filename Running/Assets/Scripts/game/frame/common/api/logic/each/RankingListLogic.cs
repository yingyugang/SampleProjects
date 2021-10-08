using UnityEngine;
using System.Text;

public class RankingListLogic : UpdateLogic
{
	[HideInInspector]
	public int m_game_id;
	[HideInInspector]
	public int pageno;

	protected override void SetAPI ()
	{
		byteArray = Encoding.UTF8.GetBytes (JsonUtility.ToJson (new RankingListData () {
			m_game_id = m_game_id,
			pageno = pageno
		}));
		apiPath = APIConstant.RANKING_LIST;
	}
}
