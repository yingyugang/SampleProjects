using System;

[Serializable]
public class LoginBonus
{
	public int m_login_bonus_id;
	public int login_num;

	public static LoginBonus GetInstance{ get; set; }

	public LoginBonus ()
	{
		GetInstance = this;
	}
}