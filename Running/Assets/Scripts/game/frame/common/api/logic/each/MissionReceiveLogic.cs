using UnityEngine;
using System.Text;

public class MissionReceiveLogic : UpdateLogic
{
	[HideInInspector]
	public int m_mission_id;

	protected override void SetAPI ()
	{
		byteArray = Encoding.UTF8.GetBytes (JsonUtility.ToJson (new MissionData () {
			m_mission_id = m_mission_id,
		}));
		apiPath = APIConstant.MISSION_RECEIVE;
	}
}
