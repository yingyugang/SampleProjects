using CSV;

public class LoginBonusDetailCSVStructure
{
	[CsvColumn (CanBeNull = true)]
	public int id{ get; set; }

	[CsvColumn (CanBeNull = true)]
	public int m_login_bonus_id{ get; set; }

	[CsvColumn (CanBeNull = true)]
	public int index{ get; set; }

	[CsvColumn (CanBeNull = true)]
	public int m_item_id{ get; set; }

	[CsvColumn (CanBeNull = true)]
	public int num{ get; set; }
}
