using CSV;

public class SoundCSVStructure
{
	[CsvColumn (CanBeNull = true)]
	public int id{ get; set; }

	[CsvColumn (CanBeNull = true)]
	public string name{ get; set; }

	[CsvColumn (CanBeNull = true)]
	public int type{ get; set; }

	[CsvColumn (CanBeNull = true)]
	public int loop{ get; set; }
}
