using System.Linq;
using UnityEngine;
using System.Collections.Generic;

public class LocalCSVParser : CSVParser
{
	private const string SOUND = "m_sound_local.csv";

	public override void Parse ()
	{
		MasterCSV.soundCSV = csvContext.Read<SoundCSVStructure> (PathConstant.CLIENT_CSV_PATH + SOUND).ToList ();
	}
}
