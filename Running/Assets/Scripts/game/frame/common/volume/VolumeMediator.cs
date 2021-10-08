using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class VolumeMediator : ActivityMediator
{
	private Volume volume;

	protected override void InitData ()
	{
		volume.BGMVALUE = ComponentConstant.VOLUME_MANAGER.GetValue (SoundTypeEnum.BGM);
		volume.SEVALUE = ComponentConstant.VOLUME_MANAGER.GetValue (SoundTypeEnum.SE);
		volume.VOICEVALUE = ComponentConstant.VOLUME_MANAGER.GetValue (SoundTypeEnum.VOICE);
	}

	protected override void CreateActions ()
	{
		volume = viewWithDefaultAction as Volume;

		BackHandler ();

		volume.BGMAction = (float value) => {
			ComponentConstant.VOLUME_MANAGER.SetValue (SoundTypeEnum.BGM, value);
		};

		volume.SEAction = (float value) => {
			ComponentConstant.VOLUME_MANAGER.SetValue (SoundTypeEnum.SE, value);
		};

		volume.voiceAction = (float value) => {
			ComponentConstant.VOLUME_MANAGER.SetValue (SoundTypeEnum.VOICE, value);
		};
	}

	virtual protected void BackHandler ()
	{

	}
}
