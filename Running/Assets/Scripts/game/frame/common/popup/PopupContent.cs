using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Events;

public class PopupContent : View
{
	public Button noButton;
	public Button yesButton;
	public Button okButton;

	public UnityAction noButtonOnClicked;
	public UnityAction yesButtonOnClicked;
	public UnityAction okButtonOnClicked;

	protected override void Start ()
	{
		base.Start ();
		AddPopupEventListeners ();
	}

	protected override void OnDestroy ()
	{
		base.OnDestroy ();
		RemovePopupEventListeners ();
	}

	protected void AddPopupEventListeners ()
	{
		if (noButton != null) {
			noButton.onClick.AddListener (NoButtonOnClickHandler);
		}
		if (yesButton != null) {
			yesButton.onClick.AddListener (YesButtonOnClickHandler);
		}
		if (okButton != null) {
			okButton.onClick.AddListener (OKButtonOnClickHandler);
		}
	}

	protected void RemovePopupEventListeners ()
	{
		if (noButton != null) {
			noButton.onClick.RemoveListener (NoButtonOnClickHandler);
			noButton.onClick = null;
		}
		if (yesButton != null) {
			yesButton.onClick.RemoveListener (YesButtonOnClickHandler);
			yesButton.onClick = null;
		}
		if (okButton != null) {
			okButton.onClick.RemoveListener (OKButtonOnClickHandler);
			okButton.onClick = null;
		}
	}

	virtual protected void NoButtonOnClickHandler ()
	{
		if (noButtonOnClicked != null) {
			noButtonOnClicked ();
		}
	}

	virtual protected void YesButtonOnClickHandler ()
	{
		if (yesButtonOnClicked != null) {
			yesButtonOnClicked ();
		}
	}

	virtual protected void OKButtonOnClickHandler ()
	{
		if (okButtonOnClicked != null) {
			okButtonOnClicked ();
		}
	}
}
