using UnityEngine;
using System.Text;

public class ExchangeLogic : UpdateLogic
{
	[HideInInspector]
	public int m_exchange_id;

	protected override void SetAPI ()
	{
		byteArray = Encoding.UTF8.GetBytes (JsonUtility.ToJson (new ExchangeData () {
			m_exchange_id = m_exchange_id,
		}));
		apiPath = APIConstant.CHARGE_EXCHANGE;
	}

	protected override void ErrorHandler (string status)
	{
		if (status == "1003") {
			if (error != null) {
				error (status);
			}
		}
	}
}
