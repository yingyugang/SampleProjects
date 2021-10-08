using UnityEngine;
using System.Collections;
using System.Text;

public class SignupLogic : APILogic
{
	protected override void SetAPI ()
	{
		byteArray = Encoding.UTF8.GetBytes (JsonUtility.ToJson (new PlayerNameData{ name = GameConstant.UserName }));
		apiPath = APIConstant.SIGNUP;
	}

	public override void APICallback (string content)
	{
		SigninModel signinModel = JsonUtility.FromJson<SigninModel> (content);
		SystemConstant.DeviceID = signinModel.device_id;
	}
}
