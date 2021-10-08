using UnityEngine;
using System.Text;

public class OriginalExchangeLogic : UpdateLogic
{
	[HideInInspector]
	public int ticket_id;

	protected override void SetAPI ()
	{
		byteArray = Encoding.UTF8.GetBytes (JsonUtility.ToJson (new TicketExchangeData () {
			ticket_id = ticket_id,
		}));
		apiPath = APIConstant.SHOP_TICKET_EXCHANGE;
	}
}
