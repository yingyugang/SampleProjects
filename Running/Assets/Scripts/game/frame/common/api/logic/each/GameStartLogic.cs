using UnityEngine;
using System.Collections;
using System.Text;

public class GameStartLogic : UpdateLogic
{
	[HideInInspector]
	public int m_game_id;
	[HideInInspector]
	public int m_item_id;
	[HideInInspector]
	public int is_cheat=0;

	protected override void SetAPI ()
	{
		byteArray = Encoding.UTF8.GetBytes (JsonUtility.ToJson (new GameStartData {
			m_game_id = m_game_id,
			m_item_id = m_item_id,
			is_cheat = is_cheat
		}));
		apiPath = APIConstant.GAME_START;
	}

	protected override void ErrorHandler (string status)
	{
		if (status == "1004") {
			Debug.Log ("no enough ap");
			if (error != null) {
				error (status);
			}
		}
	}
}
