using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class RogerScrollBar : Scrollbar
{
	public UnityAction<PointerEventData> onPointDown;
	public UnityAction<PointerEventData> onPointUp;

	public override void OnPointerDown (PointerEventData eventData)
	{
		base.OnPointerDown (eventData);
	
		if (onPointDown != null) {
			onPointDown (eventData);
		}
	}

	public override void OnPointerUp (PointerEventData eventData)
	{
		base.OnPointerUp (eventData);

		if (onPointUp != null) {
			onPointUp (eventData);
		}
	}
}
