using UnityEngine;
using System.Collections;
using System.Linq;
using UnityEngine.Events;
using System.Collections.Generic;

public class SoundManager : MonoBehaviour
{
	public VolumeManager volumeManager;
	public SoundPlayer soundPlayer;
	public Dictionary<string,AudioClip> dictionary;

	private void Start ()
	{
		volumeManager.unityAction = (SoundTypeEnum soundTypeEnum, float value) => {
			soundPlayer.SetVolume (soundTypeEnum, value);
		};
		soundPlayer.SetDefaultVolume (volumeManager.GetValue (SoundTypeEnum.BGM), volumeManager.GetValue (SoundTypeEnum.SE), volumeManager.GetValue (SoundTypeEnum.VOICE));
	}

	public string GetBGMName ()
	{
		if (soundPlayer.firstBGMAudioSource.clip == null) {
			return null;
		} else {
			return soundPlayer.firstBGMAudioSource.clip.name;
		}
	}

	public void Play (SoundEnum soundEnum, UnityAction<AudioSource> callBack = null, bool isForceToReplay = true)
	{
		string soundName = soundEnum.ToString ();
		if (dictionary != null && dictionary.Keys.Contains (soundName)) {
			AudioClip audioClip = dictionary [soundName];
			PlaySound (audioClip, soundName, callBack, isForceToReplay);
		} else {
			GetSoundFromAssetBundle (soundName, callBack, isForceToReplay);
		}
	}

	private void GetSoundFromAssetBundle (string soundName, UnityAction<AudioSource> callBack = null, bool isForceToReplay = true)
	{
		StartCoroutine (ComponentConstant.ASSETBUNDLE_RESOURCES_GETTER.GetResource<AudioClip> (GameConstant.hasDownloaded ? AssetBundleName.sound.ToString () : AssetBundleName.sound_local.ToString (), soundName, (AudioClip audioClip) => {
			PlaySound (audioClip, soundName, callBack, isForceToReplay);
		}, false, !GameConstant.hasDownloaded));
	}

	private void PlaySound (AudioClip audioClip, string soundName, UnityAction<AudioSource> callBack = null, bool isForceToReplay = true)
	{
		SoundCSVStructure soundCSVStructure = MasterCSV.soundCSV.FirstOrDefault (result => soundName == result.name);
		AudioSource audioSource = soundPlayer.Play (audioClip, soundCSVStructure, isForceToReplay);
		if (callBack != null) {
			callBack (audioSource);
		}
	}

	private void Store (List<string> list, UnityAction callBack = null)
	{
		StartCoroutine (ComponentConstant.ASSETBUNDLE_RESOURCES_GETTER.GetResourcesDictionary<AudioClip> (AssetBundleName.sound.ToString (), list, (Dictionary<string,AudioClip> audioClipDictionary) => {
			dictionary = audioClipDictionary;
			if (callBack != null) {
				callBack ();
			}
		}, false));
	}

	public void StoreSounds (List<SoundEnum> list, UnityAction callBack = null)
	{
		List<string> temp = new List<string> ();
		int length = list.Count;
		for (int i = 0; i < length; i++) {
			temp.Add (list [i].ToString ());
		}
		Store (temp, callBack);
	}

	public void StopBGM (bool noFade = false, bool isForceToReplay = false,bool ignoreTimeScale = false)
	{
		soundPlayer.StopBGM (noFade, isForceToReplay,ignoreTimeScale);
	}

	public void Clear ()
	{
		if (dictionary != null) {
			dictionary.Clear ();
			dictionary = null;
		}
	}
}
