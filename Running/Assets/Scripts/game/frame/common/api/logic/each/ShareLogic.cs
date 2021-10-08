using UnityEngine;
using System.Text;

public class ShareLogic : UpdateLogic
{
	protected override void SetAPI ()
	{
		byteArray = Encoding.UTF8.GetBytes (JsonUtility.ToJson (""));
		apiPath = APIConstant.PLAYER_SHARE;
	}
}
