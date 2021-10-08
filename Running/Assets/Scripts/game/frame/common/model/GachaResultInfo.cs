using System;
using System.Collections.Generic;

[Serializable]
public class GachaResultInfo
{
	public bool show_led;
	public int show_persons;
	public List<CardInfo> cardinfo_list;
	public int original_ticket;

	public static GachaResultInfo GetInstance{ get; set; }

	public GachaResultInfo ()
	{
		GetInstance = this;
	}
}