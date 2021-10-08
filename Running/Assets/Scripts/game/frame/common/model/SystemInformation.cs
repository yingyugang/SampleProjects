using System;
using UnityEngine;

[Serializable]
public class SystemInformation
{
	public int system_code;
	public int current_time;
	public Mark mark;
	public string api_token;

	public static SystemInformation GetInstance{ get; set; }

	public SystemInformation ()
	{
		GetInstance = this;
	}
}
