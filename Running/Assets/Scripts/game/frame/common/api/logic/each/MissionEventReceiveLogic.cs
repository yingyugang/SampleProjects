using UnityEngine;
using System.Text;

public class MissionEventReceiveLogic : UpdateLogic
{
	[HideInInspector]
	public int event_mission_id;

	protected override void SetAPI ()
	{
		byteArray = Encoding.UTF8.GetBytes (JsonUtility.ToJson (new MissionEventData () {
			event_mission_id = event_mission_id,
		}));
		apiPath = APIConstant.MISSION_EVENT_RECEIVE;
	}
}
