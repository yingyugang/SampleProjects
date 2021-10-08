using CSV;

public class GameCSVStructure
{
	[CsvColumn (CanBeNull = true)]
	public int id{ get; set; }

	[CsvColumn (CanBeNull = true)]
	public string name{ get; set; }

	[CsvColumn (CanBeNull = true)]
	public string description{ get; set; }

	[CsvColumn (CanBeNull = true)]
	public string image_resource{ get; set; }

	[CsvColumn (CanBeNull = true)]
	public int ap{ get; set; }

	[CsvColumn (CanBeNull = true)]
	public int open_need_ticket{ get; set; }

	[CsvColumn (CanBeNull = true)]
	public int open_need_free_ticket{ get; set; }

	[CsvColumn (CanBeNull = true)]
	public int default_open{ get; set; }

	[CsvColumn (CanBeNull = true)]
	public string open_start_time{ get; set; }

	[CsvColumn (CanBeNull = true)]
	public int first_clear_reward_item_id{ get; set; }

	[CsvColumn (CanBeNull = true)]
	public int reward_num{ get; set; }

	[CsvColumn (CanBeNull = true)]
	public int open_animation{ get; set; }
}
