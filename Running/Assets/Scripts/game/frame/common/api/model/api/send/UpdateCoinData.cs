using System;

[Serializable]
public class UpdatePurchaseData
{
	public string currency;
	public string signed_data;
	public string signature;
	public string receipt;
	public string product_id;
}
