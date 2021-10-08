using UnityEngine;
using System.Text;

public class IssueCodeLogic : UpdateLogic
{
	[HideInInspector]
	public int m_card_id;
	[HideInInspector]
	public int mode;

	protected override void SetAPI ()
	{
		byteArray = Encoding.UTF8.GetBytes (JsonUtility.ToJson (new IssueCodeData () {
			m_card_id = m_card_id,
			mode = mode
		}));
		apiPath = APIConstant.ISSUE_CODE;
	}
}
