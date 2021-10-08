using CSV;

public class ExchangeCSVStructure
{
	[CsvColumn (CanBeNull = true)]
	public int id{ get; set; }

	[CsvColumn (CanBeNull = true)]
	public int coin_num{ get; set; }

	[CsvColumn (CanBeNull = true)]
	public int ticket_num{ get; set; }
}
