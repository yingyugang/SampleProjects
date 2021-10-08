using System;

[Serializable]
public class AndroidReceiptModel : APIModel
{
	public string Store;
	public string TransactionID;
    public PayloadAndroidInformation Payload;
}
public class PayloadAndroidInformation{
    public string signature;
    public string json;
}

public class AndroidReceiptModelTest : APIModel
{
    public string Store;
    public string TransactionID;
    public string Payload;
}
