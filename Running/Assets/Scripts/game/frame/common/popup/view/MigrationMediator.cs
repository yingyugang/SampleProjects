using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class MigrationMediator : PopupContentMediator
{
	private Migration migration;
	public UnityAction<string,string> unityAction;

	protected override void YesButtonOnClickHandler ()
	{
		base.YesButtonOnClickHandler ();

		if (unityAction != null) {
			migration = popupContent as Migration;
			unityAction (migration.mailInputField.text, migration.pwInputField.text);
		}
	}
}
