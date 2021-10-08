using CSV;

public class MissionCSVStructure
{
	[CsvColumn (CanBeNull = true)]
	public int id{ get; set; }

	[CsvColumn (CanBeNull = true)]
	public string name{ get; set; }

	[CsvColumn (CanBeNull = true)]
	public string image_resource{ get; set; }

	[CsvColumn (CanBeNull = true)]
	public int difficulty{ get; set; }

	[CsvColumn (CanBeNull = true)]
	public int reward_type{ get; set; }

	[CsvColumn (CanBeNull = true)]
	public int reward_id{ get; set; }

	[CsvColumn (CanBeNull = true)]
	public int num{ get; set; }

	[CsvColumn (CanBeNull = true)]
	public int mission_type{ get; set; }

	[CsvColumn (CanBeNull = true)]
	public int action_type{ get; set; }

	[CsvColumn (CanBeNull = true)]
	public int value{ get; set; }

	public Mission mission = new Mission ();
}
