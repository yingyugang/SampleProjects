using UnityEngine;
using System.Text;

public class GachaListLogic : UpdateLogic
{
	protected override void SetAPI ()
	{
		byteArray = Encoding.UTF8.GetBytes (JsonUtility.ToJson (""));
		apiPath = APIConstant.GACHA_LIST;
	}
}
