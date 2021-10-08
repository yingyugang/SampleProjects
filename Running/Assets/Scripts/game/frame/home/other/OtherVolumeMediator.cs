using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class OtherVolumeMediator : VolumeMediator
{
	protected override void BackHandler ()
	{
		unityActionArray = new UnityAction[] {
			() => {
				ComponentConstant.SOUND_MANAGER.Play (SoundEnum.se02_ng);
				showWindow (7);
			}
		};
	}
}
