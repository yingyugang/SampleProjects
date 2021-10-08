using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class GameInVolumeMediator : VolumeMediator
{
    protected override void BackHandler()
    {
        unityActionArray = new UnityAction[] {
        () => {
            if (ComponentConstant.SOUND_MANAGER != null)
            {
                ComponentConstant.SOUND_MANAGER.Play(SoundEnum.se01_ok);
            }

            gameObject.gameObject.SetActive(false);
        }
        };
    }
}
