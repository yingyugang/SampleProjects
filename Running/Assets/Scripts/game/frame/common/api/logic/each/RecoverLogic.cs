using UnityEngine;
using System.Text;

public class RecoverLogic : UpdateLogic
{
	protected override void SetAPI ()
	{
		byteArray = Encoding.UTF8.GetBytes (JsonUtility.ToJson (""));
		apiPath = APIConstant.PLAYER_RECOVER;
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
