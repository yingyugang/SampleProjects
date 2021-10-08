using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class RogerScrollRect : ScrollRect
{
	public UnityAction<PointerEventData> onBeginDrag;
	public UnityAction<PointerEventData> onEndDrag;
	public UnityAction layoutComplete;
	public UnityAction valueChange;

	public override void LayoutComplete ()
	{
		base.LayoutComplete ();
		if (layoutComplete != null) {
			layoutComplete ();
		}
	}

	private void Start ()
	{
		onValueChanged.AddListener (OnValueChangedHandler);
	}

	private void OnDestroy ()
	{
		onValueChanged.RemoveListener (OnValueChangedHandler);
		onBeginDrag = null;
		onEndDrag = null;
		layoutComplete = null;
		valueChange = null;
	}

	private void OnValueChangedHandler (Vector2 vector2)
	{
		if (valueChange != null) {
			valueChange ();
		}
	}

	public override void OnBeginDrag (PointerEventData eventData)
	{
		base.OnBeginDrag (eventData);
		if (onBeginDrag != null) {
			onBeginDrag (eventData);
		}
	}

	public override void OnEndDrag (PointerEventData eventData)
	{
		base.OnEndDrag (eventData);
		if (onEndDrag != null) {
			onEndDrag (eventData);
		}
	}
}
