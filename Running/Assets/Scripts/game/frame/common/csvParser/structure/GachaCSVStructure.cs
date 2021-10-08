using CSV;

public class GachaCSVStructure
{
	[CsvColumn (CanBeNull = true)]
	public int id{ get; set; }

	[CsvColumn (CanBeNull = true)]
	public string title{ get; set; }

	[CsvColumn (CanBeNull = true)]
	public int gacha_type{ get; set; }

	[CsvColumn (CanBeNull = true)]
	public string start_time{ get; set; }

	[CsvColumn (CanBeNull = true)]
	public string end_time{ get; set; }

	[CsvColumn (CanBeNull = true)]
	public string description{ get; set; }

	[CsvColumn (CanBeNull = true)]
	public string image_resource{ get; set; }

	[CsvColumn (CanBeNull = true)]
	public string single_cost{ get; set; }

	[CsvColumn (CanBeNull = true)]
	public int multi_cost{ get; set; }

	[CsvColumn (CanBeNull = true)]
	public string percentage_desc{ get; set; }

	[CsvColumn (CanBeNull = true)]
	public int gacha_order{ get; set; }
}
