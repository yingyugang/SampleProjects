using UnityEngine;
using System.Text;

public class GachaLogic : UpdateLogic
{
	[HideInInspector]
	public int m_gacha_id;
	[HideInInspector]
	public int mode;
	[HideInInspector]
	public int cost_type;

	protected override void SetAPI ()
	{
		byteArray = Encoding.UTF8.GetBytes (JsonUtility.ToJson (new GachaData {
			m_gacha_id = m_gacha_id,
			mode = mode,
			cost_type = cost_type
		}));
		apiPath = APIConstant.GACHA_DO;
	}

	protected override void ErrorHandler (string status)
	{
		if (status == "1003") {
			Debug.Log ("no enough coin");
			if (error != null) {
				error (status);
			}
		}
	}
}
