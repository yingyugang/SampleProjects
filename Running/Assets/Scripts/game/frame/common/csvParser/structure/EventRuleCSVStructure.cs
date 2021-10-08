using CSV;

public class EventRuleCSVStructure
{
	[CsvColumn (CanBeNull = true)]
	public int id{ get; set; }

	[CsvColumn (CanBeNull = true)]
	public string title{ get; set; }

	[CsvColumn (CanBeNull = true)]
	public string description{ get; set; }
}
