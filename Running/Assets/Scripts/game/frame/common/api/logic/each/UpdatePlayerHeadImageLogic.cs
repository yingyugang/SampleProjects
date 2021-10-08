using UnityEngine;
using System.Collections;
using System.Text;

public class UpdatePlayerHeadImageLogic : UpdateLogic
{
	[HideInInspector]
	public int m_card_id;

	protected override void SetAPI ()
	{
		byteArray = Encoding.UTF8.GetBytes (JsonUtility.ToJson (new PlayerHeadImageData {
			m_card_id = m_card_id
		}));
		apiPath = APIConstant.PLAYER_UPDATE_HEAD;
	}
}
