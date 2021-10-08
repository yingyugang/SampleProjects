using UnityEngine;
using System.Text;

public class PresentLogic : UpdateLogic
{
	protected override void SetAPI ()
	{
		byteArray = Encoding.UTF8.GetBytes (JsonUtility.ToJson (""));
		apiPath = APIConstant.PRESENT_LIST;
	}
}
