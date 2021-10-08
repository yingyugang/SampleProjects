using UnityEngine;
using System.Text;

public class ShopLimitLogic : UpdateLogic
{
	protected override void SetAPI ()
	{
		byteArray = Encoding.UTF8.GetBytes (JsonUtility.ToJson (""));
		apiPath = APIConstant.SHOP_LIMIT;
	}
}
