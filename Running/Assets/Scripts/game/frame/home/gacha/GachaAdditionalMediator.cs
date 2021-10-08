using UnityEngine;
using System.Collections;
using System.Linq;

public class GachaAdditionalMediator : ActivityMediator
{
	private GachaAdditional gachaAdditional;
	public CardAdditionalItem instantiation;

	private void OnEnable ()
	{
		gachaAdditional = viewWithDefaultAction as GachaAdditional;
		RogerContainerCleaner.Clean (gachaAdditional.container);
		Instantiator instantiator = Instantiator.GetInstance ();
		float[] totalArray = CardRate.cardTotalArray;
		float[] additionalArray = CardRate.cardAdditionalArray;
		int length = totalArray.Length;
		for (int i = 0; i < length; i++) {
			CardAdditionalItem cardAdditionalItem = instantiator.Instantiate (instantiation, Vector2.zero, Vector3.one, gachaAdditional.container);
			cardAdditionalItem.SetItem (totalArray [i], additionalArray [i], i, i == 0 ? 1 : 2);
			cardAdditionalItem.gameObject.SetActive (true);
		}
		CardRate.AddToOriginalRate ();
	}
}
