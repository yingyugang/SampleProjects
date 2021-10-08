using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class RogerSlider : Slider
{
	public UnityAction onPointerUp;

	public override void OnPointerUp (PointerEventData eventData)
	{
		base.OnPointerUp (eventData);
		if (onPointerUp != null) {
			onPointerUp ();
		}
	}
}
