using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Events;

public class ButtonDetector : MonoBehaviour
{
	public Button button;
	public UnityAction<ButtonDetector> unityAction;

	private void Start ()
	{
		AddEventListeners ();
	}

	private void OnDestroy ()
	{
		RemoveEventListeners ();
	}

	private void AddEventListeners ()
	{
		button.onClick.AddListener (ButtonOnClickHandler);
	}

	private void RemoveEventListeners ()
	{
		button.onClick.RemoveListener (ButtonOnClickHandler);
	}

	private void ButtonOnClickHandler ()
	{
		if (unityAction != null) {
			unityAction (this);
		}
	}
}
