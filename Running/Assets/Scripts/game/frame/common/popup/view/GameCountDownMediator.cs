using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class GameCountDownMediator : PopupContentMediator
{
    public static bool didEndCountDown = true;
         
	private void OnEnable()
	{
        didEndCountDown = false;
        StartCoroutine (CountDown ());
	}

	private IEnumerator CountDown ()
	{
		ComponentConstant.SOUND_MANAGER.Play(SoundEnum.se04_start_countdown);
        (popupContent as GameCountDown).SetNumber (0);
        yield return new WaitForSeconds (1);

		ComponentConstant.SOUND_MANAGER.Play(SoundEnum.se04_start_countdown);
        (popupContent as GameCountDown).SetNumber (1);
        yield return new WaitForSeconds (1);

		ComponentConstant.SOUND_MANAGER.Play(SoundEnum.se04_start_countdown);
        (popupContent as GameCountDown).SetNumber (2);
        yield return new WaitForSeconds (1);

		ComponentConstant.SOUND_MANAGER.Play(SoundEnum.se05_start);
        showOrHideBg(false);
		(popupContent as GameCountDown).SetNumber (3);
        yield return new WaitForSeconds (1);
		ClosePopup ();
        didEndCountDown = true;
    }
}
