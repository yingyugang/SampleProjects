using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class RogerVerticalScrollBar : MonoBehaviour
{
	public RogerScrollBar rogerScrollBar;
	public RectTransform content;
	public RectTransform view;
	private float contentSize;
	public RogerScrollRect rogerScrollRect;
	public static Vector2 CurrentPosition;
	public static bool IsDragVerticalBar;
	public static bool HasScrollBar;

	public void Init (float contentSize)
	{
		this.contentSize = contentSize;
		SetSize ();
		SetValue ();
	}

	private void OnPointDownHandler (PointerEventData eventData)
	{
		rogerScrollRect.StopMovement ();
		rogerScrollBar.onValueChanged.AddListener (ScrollBarOnValueChangedHandler);
		IsDragVerticalBar = true;
	}

	private void OnPointUpHandler (PointerEventData eventData)
	{
		rogerScrollBar.onValueChanged.RemoveListener (ScrollBarOnValueChangedHandler);
		IsDragVerticalBar = false;
	}

	private void OnDisable ()
	{
		rogerScrollBar.onValueChanged.RemoveListener (ScrollBarOnValueChangedHandler);
		rogerScrollRect.onValueChanged.RemoveListener (ScrollRectOnValueChangedHandler);
		rogerScrollBar.onPointDown = null;
		rogerScrollBar.onPointUp = null;
		HasScrollBar = false;
	}

	private void OnEnable()
	{
		rogerScrollBar.onValueChanged.AddListener (ScrollBarOnValueChangedHandler);
		rogerScrollRect.onValueChanged.AddListener (ScrollRectOnValueChangedHandler);
		rogerScrollBar.onPointDown = OnPointDownHandler;
		rogerScrollBar.onPointUp = OnPointUpHandler;
		HasScrollBar = true;
	}

	public void SetSize ()
	{
		rogerScrollBar.size = view.rect.height / contentSize;
	}

	public void SetValue ()
	{
		rogerScrollBar.value = 1 - (content.anchoredPosition.y / (contentSize - view.rect.height));
	}

	private void ScrollRectOnValueChangedHandler (Vector2 vector2)
	{
		SetValue ();
	}

	private void ScrollBarOnValueChangedHandler (float value)
	{
		CurrentPosition = content.anchoredPosition = new Vector2 (content.anchoredPosition.x, (contentSize - view.rect.height) * (1 - value));
	}
}
