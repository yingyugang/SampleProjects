using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using UnityEngine.UI;

public class GachaTop : ViewWithDefaultAction
{
	public GameObject information;
	public Text informationText;
	public Transform container;
	public GachaMenuItemMediator gachaMenuItemMediator;

	public void OpenChangeTimeText ()
	{
		informationText.text = string.Format (UpdateInformation.GetInstance.gacha_led_text, TimeUtil.Seconds2DateTimeString (Player.GetInstance.gacha_up_end));
		information.SetActive (Player.GetInstance.gacha_up_end > SystemInformation.GetInstance.current_time);
	}

	public void CloseChangeTimeText ()
	{
		information.SetActive (false);
	}

	protected override void AddActions ()
	{
		unityActionArray = new UnityAction[] {
			() => {
				Send (0);
			},
			() => {
				Send (1);
			},
			() => {
				Send (2);
			}
		};
	}
}
