using UnityEngine;
using System.Collections;
using System.Text;

public class UpdateMigrationLogic : UpdateLogic
{
	[HideInInspector]
	public string mail;
	[HideInInspector]
	public string pw;

	protected override void SetAPI ()
	{
		byteArray = Encoding.UTF8.GetBytes (JsonUtility.ToJson (new MigrationData {
			mail = mail,
			pw = pw
		}));
		apiPath = APIConstant.PLAYER_UPDATE_MIGRATION;
	}
}
