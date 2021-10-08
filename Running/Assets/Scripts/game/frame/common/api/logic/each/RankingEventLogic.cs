using UnityEngine;
using System.Text;

public class RankingEventLogic : UpdateLogic
{
	[HideInInspector]
	public int event_id;
	[HideInInspector]
	public int pageno;
	[HideInInspector]
	public int mode;

	protected override void SetAPI ()
	{
		byteArray = Encoding.UTF8.GetBytes (JsonUtility.ToJson (new RankingEventData () {
			event_id = event_id,
			pageno = pageno,
			mode = mode
		}));
		apiPath = APIConstant.RANKING_EVENT;
	}
}
