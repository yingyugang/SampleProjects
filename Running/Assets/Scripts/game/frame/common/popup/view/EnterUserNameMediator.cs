using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using System.Linq;

public class EnterUserNameMediator : PopupContentMediator
{
	public UnityAction<bool,string> unityAction;

	protected override void OKButtonOnClickHandler ()
	{
		string userName = (popupContent as EnterUserName).userName.text;

		if (Checkout (userName)) {
			GameConstant.UserName = userName;
			if (unityAction != null) {
				unityAction (true, userName);
			}
		} else {
			if (unityAction != null) {
				unityAction (false, userName);
			}
		}

		base.OKButtonOnClickHandler ();
	}

	private bool Checkout (string userName)
	{
		NGCSVStructure ngCSVStructure = MasterCSV.ngCSV.FirstOrDefault (result => result.name == userName);
		if (ngCSVStructure != null) {
			return false;
		} else {
			if (string.IsNullOrEmpty (userName) || userName.Length > 11) {
				return false;
			} else {
				return true;
			}
		}
	}
}
