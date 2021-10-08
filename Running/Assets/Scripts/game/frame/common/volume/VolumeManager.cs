using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class VolumeManager : MonoBehaviour
{
	private const float BGM_DEFAULT_VALUE = 0.5f;
	private const float SE_DEFAULT_VALUE = 0.6f;
	public UnityAction<SoundTypeEnum,float> unityAction;

	public float GetValue (SoundTypeEnum soundTypeEnum)
	{
		return PlayerPrefs.GetFloat (soundTypeEnum.ToString (), (soundTypeEnum == SoundTypeEnum.BGM) ? BGM_DEFAULT_VALUE : SE_DEFAULT_VALUE);
	}

	public void SetValue (SoundTypeEnum soundTypeEnum, float value)
	{
		if (unityAction != null) {
			unityAction (soundTypeEnum, value);
		}
		PlayerPrefs.SetFloat (soundTypeEnum.ToString (), value);
	}
}
