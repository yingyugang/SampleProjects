using System;

[Serializable]
public class Mark
{
	public int game;
	public int gacha;
	public int shop;
	public int card;
	public int mission;
	public int other_present;
	public int other_infor;

	public static Mark GetInstance{ get; set; }

	public Mark ()
	{
		GetInstance = this;
	}
}
