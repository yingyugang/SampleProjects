using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class RogerInteractiveScrollItem : RogerScrollItem
{
	public Button button;

	public override void Init (int index)
	{
		base.Init (index);
	}

	override protected void Start ()
	{
		base.Start ();
		AddEventListeners ();
	}

	protected override void OnDestroy ()
	{
		base.OnDestroy ();
		RemoveListeners ();
	}

	protected void AddEventListeners ()
	{
		button.onClick.AddListener (ButtonOnClickHandler);
	}

	protected void RemoveListeners ()
	{
		button.onClick.RemoveListener (ButtonOnClickHandler);
		button.onClick = null;
	}

	virtual protected void ButtonOnClickHandler ()
	{
		ComponentConstant.SOUND_MANAGER.Play (SoundEnum.se01_ok);
		if (unityAction != null) {
			unityAction (this);
		}
	}
}
