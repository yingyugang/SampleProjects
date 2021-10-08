using UnityEngine;
using System.Collections;

public class MaintenanceMediator : PopupContentMediator
{
	private Maintenance maintenance;

	protected override void OKButtonOnClickHandler ()
	{
		base.OKButtonOnClickHandler ();
		ComponentConstant.API_MANAGER.ReSendAPI ();
	}

	private void OnEnable ()
	{
		maintenance = popupContent as Maintenance;
		maintenance.content.text = objectList [0] as string;
	}
}
