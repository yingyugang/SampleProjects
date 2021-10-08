using UnityEngine;
using System.Text;

public class SystemInfoLogic : APILogic
{
	protected override void SetAPI ()
	{
		byteArray = Encoding.UTF8.GetBytes (JsonUtility.ToJson (""));
		apiPath = APIConstant.SYSTEM_INFO;
	}
}
