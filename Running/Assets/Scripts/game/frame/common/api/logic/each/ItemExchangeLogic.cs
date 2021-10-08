using UnityEngine;
using System.Text;

public class ItemExchangeLogic : UpdateLogic
{
	[HideInInspector]
	public int m_limit_item_id;

	protected override void SetAPI ()
	{
		byteArray = Encoding.UTF8.GetBytes (JsonUtility.ToJson (new ItemExchangeData () {
			m_limit_item_id = m_limit_item_id,
		}));
		apiPath = APIConstant.SHOP_ITEM_EXCHANGE;
	}
}
