using UnityEngine;
using System.Collections;

public class LifeChargeeErrorMediator : PopupContentActivityMediator
{
	private LifeChargeeError lifeChargeeError;

	private void OnEnable ()
	{
		lifeChargeeError = popupContent as LifeChargeeError;
		lifeChargeeError.coin.text = objectList [0].ToString ();
	}
}
