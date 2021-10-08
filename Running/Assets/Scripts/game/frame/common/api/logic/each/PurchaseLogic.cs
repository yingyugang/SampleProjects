using UnityEngine;
using System.Text;

public enum PurchaseType
{
	Coin,
	AD
}

public class PurchaseLogic : UpdateLogic
{
	[HideInInspector]
	public string currency;
	[HideInInspector]
	public string signed_data;
	[HideInInspector]
	public string signature;
	[HideInInspector]
	public string receipt;
	[HideInInspector]
	public string product_id;

	public PurchaseType purchaseType;

	protected override void SetAPI ()
	{
		byteArray = Encoding.UTF8.GetBytes (JsonUtility.ToJson (new UpdatePurchaseData () {
			currency = currency,
			signed_data = signed_data,
			signature = signature,
			receipt = receipt,
			product_id = product_id
		}));
		apiPath = purchaseType == PurchaseType.Coin ? APIConstant.CHARGE_UPDATE_COIN : APIConstant.CHARGE_AD;
	}
}
