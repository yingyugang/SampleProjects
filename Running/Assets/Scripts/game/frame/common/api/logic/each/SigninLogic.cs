using UnityEngine;
using System.Text;

public class SigninLogic : APILogic
{
	protected override void SetAPI ()
	{
		byteArray = Encoding.UTF8.GetBytes (JsonUtility.ToJson (""));
		apiPath = APIConstant.SIGNIN;
	}
}
