using System;
using UnityEngine;

[Serializable]
public class Player
{
	public int id;
	public string name;
	public int lv;
	public int exp;
	public int free_ticket_num;
	public int coin;
	public int free_coin;
	public int age_range;
	public string head_image;
	public int platform;
	public string device_id;
	public int created_at;
	public string migration_mail;
	public bool debug_player;
	public int gacha_up_end;
	public string p_code;

	public static Player GetInstance{ get; set; }

	public Player ()
	{
		GetInstance = this;
	}

	public int ticket_num{ get { return coin + free_coin; } }
}