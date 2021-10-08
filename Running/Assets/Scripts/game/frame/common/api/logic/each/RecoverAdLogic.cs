using UnityEngine;
using System.Text;

public class RecoverAdLogic : UpdateLogic
{
	protected override void SetAPI ()
	{
		byteArray = Encoding.UTF8.GetBytes (JsonUtility.ToJson (""));
		apiPath = APIConstant.RECOVER_AD;
	}
}
