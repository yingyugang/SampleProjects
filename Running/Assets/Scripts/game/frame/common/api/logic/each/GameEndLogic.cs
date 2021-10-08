using UnityEngine;
using System.Collections;
using System.Text;

public class GameEndLogic : UpdateLogic
{

	[HideInInspector]
	public int m_game_id;

	//add by sya
	private int _score;
	[HideInInspector]
	public int score{
		get{
			return _score >> 1;
		}
		set {
			_score = value << 1;
		}
	}

	[HideInInspector]
	public int play_time;
	[HideInInspector]
	public int combo_num;
	[HideInInspector]
	public int blocks_num;
	[HideInInspector]
	public int nomiss;
	[HideInInspector]
	public int item_get_num;
	[HideInInspector]
	public int syonosuke_show;
	[HideInInspector]
	public int hide_area_show;
	[HideInInspector]
	public int swim_metre;
	[HideInInspector]
	public int all_muster;
	[HideInInspector]
	public int over_2weeks;
	[HideInInspector]
	public int broken_block_num;
	[HideInInspector]
	public int stage_taget;
	[HideInInspector]
	public int over_sys_num;
	[HideInInspector]
	public int fever_rush;
	[HideInInspector]
	public int move_metre;
	[HideInInspector]
	public int hitchhike1;
	[HideInInspector]
	public int hitchhike2;
	[HideInInspector]
	public int jungle_reach;
	[HideInInspector]
	public int akatsuka_reach;
	[HideInInspector]
	public int defeat_boss_num;
	[HideInInspector]
	public float card_bonus;
	[HideInInspector]
	public string card_ids;
	[HideInInspector]
	public string current_group;
	[HideInInspector]
	public int serif_id;

	protected override void SetAPI ()
	{
		byteArray = Encoding.UTF8.GetBytes (JsonUtility.ToJson (new GameEndData {
			m_game_id = m_game_id,
			score = score,
			play_time = play_time,
			combo_num = combo_num,
			blocks_num = blocks_num,
			nomiss = nomiss,
			item_get_num = item_get_num,
			syonosuke_show = syonosuke_show,
			swim_metre = swim_metre,
			hide_area_show = hide_area_show,
			all_muster = all_muster,
			over_2weeks = over_2weeks,
			broken_block_num = broken_block_num,
			stage_taget = stage_taget,
			over_sys_num = over_sys_num,
			fever_rush = fever_rush,
			move_metre = move_metre,
			hitchhike1 = hitchhike1,
			hitchhike2 = hitchhike2,
			jungle_reach = jungle_reach,                
			akatsuka_reach = akatsuka_reach,
			defeat_boss_num = defeat_boss_num,
			card_bonus = card_bonus,
			card_ids = card_ids,
			current_group = current_group,
			serif_id=serif_id
		}));
		apiPath = APIConstant.GAME_OVER;
	}
}
