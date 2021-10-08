using UnityEngine;
using System.Collections;

public class PopupContentActivityMediator : PopupContentMediator
{
	override protected void Start ()
	{
		base.Start ();
		CreateActions ();

		(popupContent as PopupContentWithDefaultAction).send = (int buttonNumber) => {
			unityActionArray [buttonNumber] ();
		};
	}

	virtual protected void CreateActions ()
	{

	}

	private void OnDestroy ()
	{
		(popupContent as PopupContentWithDefaultAction).send = null;
	}
}
