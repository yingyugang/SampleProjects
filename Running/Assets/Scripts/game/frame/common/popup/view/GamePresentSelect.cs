using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using UnityEngine.UI;

public class GamePresentSelect : PopupContentWithDefaultAction
{
	public Text text1;
	public Text text2;
	public Text text3;

	private void OnEnable ()
	{
		SetText (true);
		text1.text = string.Format (LanguageJP.PRESENT_USER_NAME, Player.GetInstance.name);
	}

	protected override void AddActions ()
	{
		unityActionArray = new UnityAction[] {
			() => {
				Send (0);
				SetTextAndSound ();
			},
			() => {
				Send (1);
				SetTextAndSound ();
			},
			() => {
				Send (2);
				SetTextAndSound ();
			},
			() => {
				Send (3);
				SetTextAndSound ();
			},
			() => {
				Send (4);
				SetTextAndSound ();
			},
			() => {
				Send (5);
				SetTextAndSound ();
			}
		};
	}

	private void SetTextAndSound ()
	{
		ComponentConstant.SOUND_MANAGER.Play (SoundEnum.se01_ok);
		SetText ();
	}

	private void SetText (bool isShow = false)
	{
		text1.gameObject.SetActive (isShow);
		text2.gameObject.SetActive (isShow);
		text3.gameObject.SetActive (!isShow);
	}
}
