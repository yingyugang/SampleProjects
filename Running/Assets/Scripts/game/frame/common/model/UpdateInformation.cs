using System;
using System.Collections.Generic;

[Serializable]
public class UpdateInformation
{
	public Player player;
	public PlayerLoginInfo player_login_info;
	public string game_led_text;
	public string game_led_image;
	public string gacha_led_text;
	public List<GameDetail> game_list;
	public RankingTop ranking_top;
	public RecyclePoint recycle_pt;
	public List<MessageBox> message_box_list;
	public List<Card> card_list;
	public List<Mission> mission_list;
	public List<MissionClearInfo> mission_clear_info_list;
	public List<Information> information_list;
	public List<Gacha> gacha_list;
	public List<Recommend> recommend_list;
	public List<LimitShop> limit_shop_list;
	public List<LimitItem> limit_item_list;
	public int total_page;
	public List<RankingData> ranking_list;
	public AdParameter ad_parameter;
	public int recover_ap_value;
	public int game_total_score;
	public List<EventInfo> event_info_list;
	public PlayerItems player_items;
	public int ad_bonus_num;
	public int issue_1l_value;
	public int issue_2l_value;
	public List<TicketExchange> ticket_exchange_list;

	public static UpdateInformation GetInstance{ get; set; }

	public UpdateInformation ()
	{
		GetInstance = this;
	}
}