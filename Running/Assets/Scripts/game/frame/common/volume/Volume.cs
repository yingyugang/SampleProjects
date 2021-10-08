using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using UnityEngine.UI;

public class Volume : ViewWithDefaultAction
{
	public RogerSlider BGMSlider;
	public RogerSlider SESlider;
	public RogerSlider voiceSlider;

	public UnityAction<float> BGMAction;
	public UnityAction<float> SEAction;
	public UnityAction<float> voiceAction;

	private float BGMValue;
	private float SEValue;
	private float voiceValue;

	protected override void AddActions ()
	{
		BackHandler ();

//		BGMSlider.onPointerUp = () => {
//			BGMAction (BGMValue);
//		};
		SESlider.onPointerUp = () => {
			ComponentConstant.SOUND_MANAGER.Play (SoundEnum.se04_start_countdown);
//			SEAction (SEValue);
		};
//		voiceSlider.onPointerUp = () => {
//			voiceAction (voiceValue);
//		};
		BGMSlider.onValueChanged.AddListener (BGMOnValueChangedHandler);
		SESlider.onValueChanged.AddListener (SEOnValueChangedHandler);
//		voiceSlider.onValueChanged.AddListener (VoiceOnValueChangedHandler);
	}

	public float BGMVALUE {
		set {
			BGMSlider.value = BGMValue = value;
		}
	}

	public float SEVALUE {
		set {
			SESlider.value = SEValue = value;
		}
	}

	public float VOICEVALUE {
		set {
			voiceSlider.value = voiceValue = value;
		}
	}

	private void BGMOnValueChangedHandler (float value)
	{
		BGMValue = value;
		BGMAction (BGMValue);
	}

	private void SEOnValueChangedHandler (float value)
	{
		SEValue = value;
		SEAction (SEValue);
	}

//	private void VoiceOnValueChangedHandler (float value)
//	{
//		voiceValue = value;
//	}

	virtual protected void BackHandler ()
	{
		
	}
}
