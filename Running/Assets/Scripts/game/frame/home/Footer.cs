using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Events;
using DG.Tweening;

public class Footer : View
{
	public Toggle[] toggleArray;
	public UnityAction<int> send;
	public UnityAction<bool>[] unityActionBoolArray;
	private int currentIndex;

	public RectTransform changeBadgeRectTransform;
	public RectTransform eventBadgeRectTransform;

	public override void AddEventListeners ()
	{
		for (int i = 0; i < toggleArray.Length; i++) {
			toggleArray [i].onValueChanged.AddListener (unityActionBoolArray [i]);
		}
	}

	override public void RemoveEventListeners ()
	{
		for (int i = 0; i < toggleArray.Length; i++) {
			toggleArray [i].onValueChanged.RemoveListener (unityActionBoolArray [i]);
			toggleArray [i].onValueChanged = null;
		}

		unityActionBoolArray = null;
	}

	protected override void AddActions ()
	{
		unityActionBoolArray = new UnityAction<bool>[] {
			(bool isSelected) => {
				SetSelected (isSelected, 0);
			},
			(bool isSelected) => {
				SetSelected (isSelected, 1);
			},
			(bool isSelected) => {
				SetSelected (isSelected, 2);
			},
			(bool isSelected) => {
				SetSelected (isSelected, 3);
			},
			(bool isSelected) => {
				SetSelected (isSelected, 4);
			},
			(bool isSelected) => {
				SetSelected (isSelected, 5);
			}
		};
	}

	private void SetSelected (bool isSelected, int i)
	{
		CheatController.ResetCheats ();
		if (isSelected) {
			if (currentIndex != i) {
				ComponentConstant.SOUND_MANAGER.Play (SoundEnum.se01_ok);
				toggleArray [i].targetGraphic.enabled = false;
				toggleArray [i].graphic.rectTransform.anchoredPosition = new Vector2 (0, -65);
				toggleArray [i].graphic.rectTransform.DOPunchAnchorPos (new Vector2 (0, 100), 0.5f, 20, 1);
				if (send != null) {
					send (i);
				}
				currentIndex = i;
				SetBadge (i, Vector2.zero);
			}
		} else {
			toggleArray [i].graphic.rectTransform.anchoredPosition = new Vector2 (0, -180);
			toggleArray [i].targetGraphic.enabled = true;
			SetBadge (i, new Vector2 (0, -65));
		}
	}

	private void SetBadge (int i, Vector2 vector2)
	{
		if (i == 1) {
			changeBadgeRectTransform.anchoredPosition = vector2;
		}
		if (i == 0) {
			eventBadgeRectTransform.anchoredPosition = vector2;
		}
	}
}
