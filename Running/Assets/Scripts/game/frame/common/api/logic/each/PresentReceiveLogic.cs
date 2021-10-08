using UnityEngine;
using System.Text;

public class PresentReceiveLogic : UpdateLogic
{
	[HideInInspector]
	public int id;

	protected override void SetAPI ()
	{
		byteArray = Encoding.UTF8.GetBytes (JsonUtility.ToJson (new PresentData () {
			id = id,
		}));
		apiPath = APIConstant.PRESENT_RECEIVE;
	}
}
