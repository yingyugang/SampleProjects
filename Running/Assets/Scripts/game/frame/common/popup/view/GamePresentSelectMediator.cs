using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class GamePresentSelectMediator : PopupContentActivityMediator
{
	private GamePresentSelect gamePresentSelect;
	public UnityAction unityAction;
	private int currentID;

	private void OnEnable ()
	{
		EnableSelect (true);
	}

	private void EnableSelect (bool canBeSelected)
	{
		gamePresentSelect = popupContent as GamePresentSelect;
		int length = gamePresentSelect.buttonArray.Length;
		for (int i = 0; i < length; i++) {
			gamePresentSelect.buttonArray [i].enabled = canBeSelected;
		}
	}

	protected override void CreateActions ()
	{
		unityActionArray = new UnityAction[] {
			() => {
				ShowAnimation (0);
			},
			() => {
				ShowAnimation (1);
			},
			() => {
				ShowAnimation (2);
			},
			() => {
				ShowAnimation (3);
			},
			() => {
				ShowAnimation (4);
			},
			() => {
				ShowAnimation (5);
			},
		};
	}

	private void ShowAnimation (int id)
	{
		EnableSelect (false);
		currentID = id;
		if (unityAction != null) {
			unityAction ();
		}
	}

	public void PlayAnimation ()
	{
		gamePresentSelect.buttonArray [currentID].GetComponent<GamePresentButton> ().Play ();
	}
}
