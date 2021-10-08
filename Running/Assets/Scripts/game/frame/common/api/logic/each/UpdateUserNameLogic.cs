using UnityEngine;
using System.Collections;
using System.Text;

public class UpdateUserNameLogic : UpdateLogic
{
	[HideInInspector]
	public string userName;

	protected override void SetAPI ()
	{
		byteArray = Encoding.UTF8.GetBytes (JsonUtility.ToJson (new PlayerNameData {
			name = userName
		}));
		apiPath = APIConstant.PLAYER_UPDATE_NAME;
	}
}
