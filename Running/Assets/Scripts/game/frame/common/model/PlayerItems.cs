using System;
using System.Collections.Generic;

[Serializable]
public class PlayerItems
{
	public int original_point;
	public int event_item2;
	public int event_item5;
	public int event_item10;
	public int original_ticket;

	public static PlayerItems GetInstance { get; set; }

	public PlayerItems ()
	{
		GetInstance = this;
	}

	public int this[int index]
	{
		get
		{
			switch (index)
			{
				case 3:
					return original_point;
				case 9:
					return event_item2;
				case 10:
					return event_item5;
				case 11:
					return event_item10;
				case 12:
					return original_ticket;
				default:
					return 0;
			}
		}
	}

	public static int GetGameRate (int itemID)
	{
		switch (itemID)
		{
			case 9:
				return 2;
			case 10:
				return 5;
			case 11:
				return 10;
			default:
				return 1;
		}
	}
}
