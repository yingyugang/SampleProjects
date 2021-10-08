using System;

[Serializable]
public class PayloadInformation : APIModel
{
	public string signedData;
	public string signature;
}
