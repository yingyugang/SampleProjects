using System;

[Serializable]
public class PlayerLoginInfo
{
	public int ap;
	public int last_use_ap_at;
	public int last_logined_at;

	public static PlayerLoginInfo GetInstance{ get; set; }

	public PlayerLoginInfo ()
	{
		GetInstance = this;
	}
}


