using UnityEngine;
using System.Text;

public class InformationLogic : UpdateLogic
{
	protected override void SetAPI ()
	{
		byteArray = Encoding.UTF8.GetBytes (JsonUtility.ToJson (""));
		apiPath = APIConstant.INFORMATION_LIST;
	}
}
