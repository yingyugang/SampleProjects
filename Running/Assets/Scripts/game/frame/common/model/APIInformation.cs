using System;
using System.Collections.Generic;

[Serializable]
public class APIInformation
{
	public string device_id;
	public GameParameter gameparameter;
	public int rank;
	public List<CardInfo> cardinfo_list;
	public int next_clear_gacha;
	public LoginBonus login_bonus;
	public bool can_charge;
	public string maintenance_text;
	public GachaResultInfo gacha_result;
	public RewardInfo clear_reward_info;
	public RewardInfo first_reward_info;
	public List<Mission> clear_mission_list;
	public string review_url;
	public int receipt_status;
	public int result_code;
	public string p_code;
	public ItemBonus item_bonus;

	public static APIInformation GetInstance{ get; set; }

	public APIInformation ()
	{
		GetInstance = this;
	}
}

