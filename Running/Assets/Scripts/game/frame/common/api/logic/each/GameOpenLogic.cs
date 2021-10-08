using UnityEngine;
using System.Collections;
using System.Text;

public class GameOpenLogic : UpdateLogic
{
	[HideInInspector]
	public int m_game_id;
	[HideInInspector]
	public int mode;

	protected override void SetAPI ()
	{
		byteArray = Encoding.UTF8.GetBytes (JsonUtility.ToJson (new GameOpenData {
			m_game_id = m_game_id,
			mode = mode
		}));
		apiPath = APIConstant.GAME_OPEN;
	}

	protected override void ErrorHandler (string status)
	{
		if (status == "1003") {
			Debug.Log ("no enough coin");
			if (error != null) {
				error (status);
			}
		}
	}
}
