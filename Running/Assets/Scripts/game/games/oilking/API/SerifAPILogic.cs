using UnityEngine;
using System.Collections;
using System.Text;

public class SerifAPILogic : UpdateLogic {

	[HideInInspector]
	public string current_group;
	[HideInInspector]
	public int serif_id;

	protected override void SetAPI ()
	{
		byteArray = Encoding.UTF8.GetBytes (JsonUtility.ToJson (new SerifData {
			current_group=current_group,
			serif_id=serif_id

		}));
		apiPath = APIConstant.GAME_SERIF;
	}
}
