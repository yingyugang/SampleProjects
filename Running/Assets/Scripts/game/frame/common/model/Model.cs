using System;

[Serializable]
public class Model
{
	public APIInformation API_INFO;
	public UpdateInformation UPDATE_INFO;
	public SystemInformation SYSTEM_INFO;

	public static Model GetInstance{ get; set; }

	public Model ()
	{
		GetInstance = this;
	}
}
