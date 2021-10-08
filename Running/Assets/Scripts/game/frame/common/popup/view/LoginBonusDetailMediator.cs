using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LoginBonusDetailMediator : PopupContentMediator
{
	private LoginBonusDetail loginBonusDetail;
	private int dayIndex;
	private int presentNumber;
	private string imageResource;

	private void OnEnable ()
	{
		loginBonusDetail = popupContent as LoginBonusDetail;
		IEnumerable<LoginBonusDetailCSVStructure> loginBonusDetailCSVStructureEnumerable = objectList [0] as IEnumerable<LoginBonusDetailCSVStructure>;
		dayIndex = (int)objectList [1];
		loginBonusDetail.Show (loginBonusDetailCSVStructureEnumerable, dayIndex);
	}
}
