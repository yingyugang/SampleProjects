using CSV;

public class ItemCSVStructure
{
	[CsvColumn (CanBeNull = true)]
	public int id{ get; set; }

	[CsvColumn (CanBeNull = true)]
	public string name{ get; set; }

	[CsvColumn (CanBeNull = true)]
	public string image_resource{ get; set; }

	[CsvColumn (CanBeNull = true)]
	public string description{ get; set; }
}
