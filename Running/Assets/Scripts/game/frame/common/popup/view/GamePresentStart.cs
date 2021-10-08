using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Events;

public class GamePresentStart : PopupContentWithDefaultAction
{
	public Text hasTime;
	public Text noTime;
	public Text leftTime;
	public GreyButton greyButton;

	protected override void AddActions ()
	{
		unityActionArray = new UnityAction[] {
			() => {
				ComponentConstant.SOUND_MANAGER.Play (SoundEnum.se01_ok);
				Send (0);
			}
		};
	}
}
