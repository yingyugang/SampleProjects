using UnityEngine;
using System.Text;

public class LimitLogic : UpdateLogic
{
	[HideInInspector]
	public int price;
	[HideInInspector]
	public int age_range;

	protected override void SetAPI ()
	{
		byteArray = Encoding.UTF8.GetBytes (JsonUtility.ToJson (new LimitData () {
			price = price,
			age_range = age_range
		}));
		apiPath = APIConstant.CHARGE_LIMIT;
	}
}
