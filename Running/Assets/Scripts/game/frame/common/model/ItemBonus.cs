using System;

[Serializable]
public class ItemBonus
{
	public int reward_type;
	public int reward_id;
	public int num;

	public static ItemBonus GetInstance { get; set; }

	public ItemBonus ()
	{
		GetInstance = this;
	}
}