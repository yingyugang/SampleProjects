﻿using CSV;

public class HelpCSVStructure
{
	[CsvColumn (CanBeNull = true)]
	public int id{ get; set; }

	[CsvColumn (CanBeNull = true)]
	public string menu_name{ get; set; }

	[CsvColumn (CanBeNull = true)]
	public string description{ get; set; }
}
