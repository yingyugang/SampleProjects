using System;

[Serializable]
public class IOSReceiptModel : APIModel
{
	public string Store;
	public string TransactionID;
	public string Payload;
}
