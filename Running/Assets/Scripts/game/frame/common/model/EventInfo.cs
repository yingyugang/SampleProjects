using System;
using System.Collections.Generic;

[Serializable]
public class EventInfo
{
	public int id;
	public int m_game_id;
	public string event_name;
	public int total_score;
	public int high_score;
	public int display_at;
	public int start_at;
	public int end_at;
	public string banner_image;
	public int rank1_start;
	public int rank1_end;
	public string rank1_reward;
	public int rank2_start;
	public int rank2_end;
	public string rank2_reward;
	public int rank3_start;
	public int rank3_end;
	public string rank3_reward;
	public int rank4_start;
	public int rank4_end;
	public string rank4_reward;
	public int rank5_start;
	public int rank5_end;
	public string rank5_reward;
	public EventStatusEnum eventStatusEnum;
	public List<EventMission> event_missions;
	public int reward_on;
	public int treward_start;
	public int treward_end;
	public string treward_title;
	public string treward_sonota;
	public string hreward_desc;
	public string treward_desc;
}