using UnityEngine;
using System.Collections;

public class ConventionMediator : PopupContentMediator
{
	private Convention convention;

	private void OnEnable ()
	{
		convention = popupContent as Convention;
		convention.title.text = objectList [0].ToString ();
		convention.content.text = objectList [1].ToString ();
	}
}
