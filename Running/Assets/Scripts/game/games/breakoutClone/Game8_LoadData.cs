using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Game8_LoadData : MonoBehaviour
{
	//private const int SPEEDDEFAULT = 4;
	public Block_Manager Block;
	public Item_Manager ItemMn;
	public List<Row_Detail> Maps;
	private Ranking[] m_OdenRanking;


	void Awake ()
	{
		//LoadParamsCSV ();
		LoadParams ();
		LoadStageCsv ();
		LoadBasicBlock ();
		Maps = new List<Row_Detail> ();
		LoadBossAcctionPattern ();
		LoadBossAttacked ();
		LoadCommonAttacked ();
		LoadMap ();
		LoadRanking ();
		LoadItemCsv ();

	}

	//load block detail
	public void LoadBasicBlock ()
	{
		List<Dictionary<string,object>> data = CSVReader.Read (BreackoutConfig.CSV_STAGE_ODEN);
		if (data != null) {
			for (var i = 0; i < data.Count; i++) {
				int id = (int)data [i] ["id"];
				int type = (int)data [i] ["type"];
				string image_source = (string)data [i] ["image_resource"];
				int width = (int)data [i] ["width"];
				int height = (int)data [i] ["height"];
				int is_clr = (int)data [i] ["is_clear_condition"];
				int is_boss = (int)data [i] ["is_boss"];
				string face_image_source = (string)data [i] ["face_image_resource"];
				Block.CreateBlock (id, image_source, width, height, 0f, is_clr, is_boss, face_image_source);
			}
		}
	}



	//load all data map in csv
	void LoadMap ()
	{
		List<Dictionary<string,object>> data = CSVReader.Read (BreackoutConfig.CSV_STAGE_DETAIL);
		if (data != null) {
			for (var i = 0; i < data.Count; i++) {
				Row_Detail row = new Row_Detail ();
				row.IdStage = (int)data [i] ["m_oden_block_breaking_stage_id"];
				row.IdBlock = (int)data [i] ["m_oden_block_breaking_stage_oden_id"];
				row.X = (int)data [i] ["x"];
				row.Y = (int)data [i] ["y"];
				row.Attack = (int)data [i] ["attack_num"];
				row.Rotation = (int)data [i] ["rotation"];
				Maps.Add (row);
			}
		}
	}

	//load map to stage
	public void LoadStage (int stage, bool isBoss)
	{
		foreach (Row_Detail row in Maps) {			
			if (row.IdStage == stage) {
				if (!isBoss)
					Block.MappingBlock (row.IdBlock, row.Rotation, row.X, row.Y, row.Attack);
				else
					Block.CreateBoss (row.IdStage, row.IdBlock, row.Rotation, row.Attack);
			}
		}
	}


	// Load Stage Csv
	public void LoadStageCsv ()
	{
		List<Dictionary<string,object>> data = CSVReader.Read (BreackoutConfig.CSV_BREAKING_STAGE);
		if (data != null) {
			for (var i = 0; i < data.Count; i++) {
				int id = (int)data [i] ["id"];
				int is_Boss = (int)data [i] ["is_boss_stage"];
				string boss_Action = (string)data [i] ["m_oden_block_breaking_boss_action_pattern_ids"];
				Block.SavingStage (id, is_Boss, boss_Action);
			}
		}
	}

	// Load Item Csv
	public void LoadItemCsv ()
	{
		List<Dictionary<string,object>> data = CSVReader.Read (BreackoutConfig.CSV_ITEM);
		if (data != null) {
			for (var i = 0; i < data.Count; i++) {
				int	id = (int)data [i] ["id"];
				int appear = (int)data [i] ["appear_probality"];
				//int effect_type = (int)data [i] ["effect_type"];
				int eff_value = (int)data [i] ["effect_value"];
				int stage_reduction = (int)data [i] ["stage_reduction_percentage"];
				ItemMn.SetItemCsv (id, appear, eff_value, stage_reduction);
			}
		}
	}

	void LoadAttacked (string path, bool isboss)
	{
		List<Dictionary<string,object>> data = CSVReader.Read (path);
		if (data != null) {
			for (var i = 0; i < data.Count; i++) {
				int id = (int)data [i] ["id"];
				int min = (int)data [i] ["min"];
				int max = (int)data [i] ["max"];
				string color = (string)data [i] ["color"];
				ColorManager.instance.AddColor (new Color_Block (id, min, max, color, isboss));		
			}
		}
	}

	//Load Common Attacked
	public void LoadCommonAttacked ()
	{
		List<Dictionary<string,object>> data = CSVReader.Read (BreackoutConfig.CSV_COMMON_ATTACKED);
		if (data != null) {
			for (var i = 0; i < data.Count; i++) {
				int id = (int)data [i] ["id"];
				int min = (int)data [i] ["min"];
				int max = (int)data [i] ["max"];
				string color = (string)data [i] ["color"];
				ColorManager.instance.AddColor (new Color_Block (id, min, max, color, false));		
			}
		}
	}

	// Load Boss Attacked
	public void LoadBossAttacked ()
	{
		List<Dictionary<string,object>> data = CSVReader.Read (BreackoutConfig.CSV_BOSS_ATTACKED);
		if (data != null) {
			for (var i = 0; i < data.Count; i++) {
				int id = (int)data [i] ["id"];
				int min = (int)data [i] ["min_percent"];
				int max = (int)data [i] ["max_percent"];
				string color = (string)data [i] ["color"];
				ColorManager.instance.AddColor (new Color_Block (id, min, max, color, true));		
			}
		}
	}

	//-->Load Boss Action Pattern
	public void LoadBossAcctionPattern ()
	{
		List<Dictionary<string,object>> data = CSVReader.Read (BreackoutConfig.CSV_BOSS_ACTION);
		if (data != null) {
			for (var i = 0; i < data.Count; i++) {
				int id = (int)data [i] ["id"];
				int type = (int)data [i] ["type"];
				int x = (int)data [i] ["x"];
				int y = (int)data [i] ["y"];
				ActionBossManager.instance.SetBossAction (id, type, x, y);
			}//-<
		}
	}

	void LoadParams ()
	{
		if (APIInformation.GetInstance != null) {

			if (CheatController.IsCheated (0)) {
				Game8_Manager.instance.Racket.gameObject.SetActive (false);
				Game8_Manager.instance.RacketCheat.gameObject.SetActive (true);
				Game8_Manager.instance.Racket = Game8_Manager.instance.RacketCheat;
			} else {
				Game8_Manager.instance.Racket.gameObject.SetActive (true);
				Game8_Manager.instance.RacketCheat.gameObject.SetActive (false);
			}


			Game8_Manager.instance.Racket.SpeedRacket = APIInformation.GetInstance.gameparameter.flick_sensitivity;
			Game8_Manager.instance.Player_Lifes = APIInformation.GetInstance.gameparameter.player_lifes;
			Game8_Manager.instance.ItemManager.Item_Percentage = APIInformation.GetInstance.gameparameter.item_appear_percentage;
			Game8_Manager.instance.ItemManager.Item_PercentageDown = APIInformation.GetInstance.gameparameter.item_appear_percentage_down;
			Game8_Manager.instance.ItemManager.Boss_ItemPercentage = APIInformation.GetInstance.gameparameter.boss_item_appear_percentage;
			Game8_Manager.instance.ItemManager.Boss_ItemPercentageDown = APIInformation.GetInstance.gameparameter.boss_item_appear_percentage_down;
			Game8_Manager.instance.m_SpeedBallUpStage = APIInformation.GetInstance.gameparameter.player_speed_up_per_stage;
			Game8_Manager.instance.PlayerSpeed = APIInformation.GetInstance.gameparameter.player_speed;
			Game8_Manager.instance.DamageBoss = APIInformation.GetInstance.gameparameter.throw_item_damage4boss;
			Game8_Manager.instance.Stage_Bonus_Rate = APIInformation.GetInstance.gameparameter.stage_clear_bonus_rate;
			Game8_Manager.instance.Boss_Bonus_Rate = APIInformation.GetInstance.gameparameter.boss_destroy_bonus_rate;
			Game8_Manager.instance.Total_Combo = APIInformation.GetInstance.gameparameter.total_comb_var;

		} else {
			LoadParamsCSV ();
		}
	}

	void LoadParamsCSV ()
	{
		List<Dictionary<string,object>> data = CSVReader.Read (BreackoutConfig.CSV_PARAMETER);
		if (data != null) {
			Dictionary<string,float> dataParam = new Dictionary<string, float> ();
			for (int i = 0; i < data.Count; i++) {
				string key = data [i] ["para_name"].ToString ();
				float value = float.Parse (data [i] ["value"].ToString ());
				dataParam.Add (key, value);
			}
			Game8_Manager.instance.Racket.SpeedRacket = (int)dataParam ["flick_sensitivity"];
			Game8_Manager.instance.Player_Lifes = (int)dataParam ["player_lifes"];
			Game8_Manager.instance.ItemManager.Item_Percentage = (float)dataParam ["item_appear_percentage"];
			Game8_Manager.instance.ItemManager.Item_PercentageDown = (float)dataParam ["item_appear_percentage_down"];
			Game8_Manager.instance.ItemManager.Boss_ItemPercentage = (float)dataParam ["boss_item_appear_percentage"];
			Game8_Manager.instance.ItemManager.Boss_ItemPercentageDown = (float)dataParam ["boss_item_appear_percentage_down"];
			Game8_Manager.instance.m_SpeedBallUpStage = (float)dataParam ["player_speed_up_per_stage"];
			Game8_Manager.instance.PlayerSpeed = (float)dataParam ["player_speed"];
			Game8_Manager.instance.DamageBoss = (int)dataParam ["throw_item_damage4boss"];
			Game8_Manager.instance.Stage_Bonus_Rate = (float)dataParam ["stage_clear_bonus_rate"];
			Game8_Manager.instance.Boss_Bonus_Rate = (float)dataParam ["boss_destroy_bonus_rate"];
			Game8_Manager.instance.Total_Combo = (float)dataParam ["total_comb_var"];
		} 
	}

	void LoadRanking ()
	{
		m_OdenRanking = new Ranking[8];
		int index = 0;
		List<Dictionary<string,object>> dataRank = CSVReader.Read (BreackoutConfig.CSV_RANK);
		if (dataRank != null) {
			for (int i = 0; i < dataRank.Count; i++) {
				int id = (int)dataRank [i] ["id"];
				string rank = (string)dataRank [i] ["rank"];
				int min = (int)dataRank [i] ["min"];
				int max = (int)dataRank [i] ["max"];
				int rewardItem = (int)dataRank [i] ["reward_m_item_id"];
				int rewardNum = (int)dataRank [i] ["reward_num"];
				if (index < m_OdenRanking.Length)
					m_OdenRanking [index++] = new Ranking (id, rank, min, max, rewardItem, rewardNum);
			}
		}
	}

	public int GetRanking (int score)
	{
		foreach (Ranking rank in m_OdenRanking) {
			if (rank.CheckRanking (score) > 0) {
				return rank.CheckRanking (score);
			}				
		}
		return 1;
	}

}

class Ranking
{
	private int m_Id;
	private string m_Rank;
	private int m_Min;
	private int m_Max;
	private int m_Reward_Item;
	private int m_Reward_Num;

	public Ranking (int id, string rank, int min, int max, int rewarditem, int rewardnum)
	{
		m_Id = id;
		m_Rank = rank;
		m_Min = min;
		m_Max = max;
		m_Reward_Item = rewarditem;
		m_Reward_Num = rewardnum;
	}

	public int CheckRanking (int score)
	{
		if ((score >= m_Min && score <= m_Max) || m_Max == 0 && score >= m_Min)
			return m_Id;
		return -1;

	}
}

public class Row_Detail
{
	public int IdStage;
	public int IdBlock;
	public int X;
	public int Y;
	public int Attack;
	public int Rotation;

}
