using CSV;
using System.Collections.Generic;

public class CardCSVStructure
{
	[CsvColumn (CanBeNull = true)]
	public int id{ get; set; }

	[CsvColumn (CanBeNull = true)]
	public string number{ get; set; }

	[CsvColumn (CanBeNull = true)]
	public string character_type{ get; set; }

	[CsvColumn (CanBeNull = true)]
	public string name{ get; set; }

	[CsvColumn (CanBeNull = true)]
	public string title{ get; set; }

	[CsvColumn (CanBeNull = true)]
	public string description{ get; set; }

	[CsvColumn (CanBeNull = true)]
	public int rarity{ get; set; }

	[CsvColumn (CanBeNull = true)]
	public string image_resource{ get; set; }

	[CsvColumn (CanBeNull = true)]
	public string voice_name{ get; set; }

	[CsvColumn (CanBeNull = true)]
	public string assetbundle_name{ get; set; }

	[CsvColumn (CanBeNull = true)]
	public int up_game_id { get; set; }

	[CsvColumn (CanBeNull = true)]
	public float up_value { get; set; }

	[CsvColumn (CanBeNull = true)]
	public int up_type { get; set; }

	[CsvColumn (CanBeNull = true)]
	public int can_print{ get; set;}

	public List<int> GetCharacterType {
		get {
			List<int> intList = new List<int> ();
			if (character_type.Contains (",")) {
				string[] strArray = character_type.Split (',');
				int length = strArray.Length;
				for (int i = 0; i < length; i++) {
					intList.Add (int.Parse (strArray [i]));
				}
			} else {
				intList.Add (int.Parse (character_type));
			}
			return intList;
		}
	}
}
