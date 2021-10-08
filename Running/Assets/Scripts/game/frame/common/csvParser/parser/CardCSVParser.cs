using System.Linq;
using UnityEngine;
using System.Collections.Generic;

public class CardCSVParser : CSVParser
{
	private const string CARD = "m_card.csv";

	public override void Parse ()
	{
		CardCSV.cardCSV = csvContext.Read<CardCSVStructure> (PathConstant.CLIENT_CSV_PATH + CARD).ToList ();
	}
}
