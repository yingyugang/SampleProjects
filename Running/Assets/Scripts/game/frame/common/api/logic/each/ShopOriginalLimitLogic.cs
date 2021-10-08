using UnityEngine;
using System.Text;

public class ShopOriginalLimitLogic : UpdateLogic
{
	protected override void SetAPI ()
	{
		byteArray = Encoding.UTF8.GetBytes (JsonUtility.ToJson (""));
		apiPath = APIConstant.SHOP_TICKET_LIST;
	}
}
