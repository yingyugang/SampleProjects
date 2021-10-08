using CSV;

public class ChargeLimitCSVStructure
{
	[CsvColumn (CanBeNull = true)]
	public int id{ get; set; }

	[CsvColumn (CanBeNull = true)]
	public int age_range{ get; set; }

	[CsvColumn (CanBeNull = true)]
	public int charge_limit{ get; set; }
}
