using UnityEngine;
using System.Collections;

public class CardShowMediator : PopupContentMediator
{
	private CardShow cardShow;

	private void OnEnable ()
	{
		cardShow = popupContent as CardShow;
		cardShow.card.sprite = objectList [0] as Sprite;
		cardShow.border.sprite = objectList [1] as Sprite;
	}
}
