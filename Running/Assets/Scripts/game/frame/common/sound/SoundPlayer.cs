using UnityEngine;
using System.Collections;
using DG.Tweening;
using System.Collections.Generic;
using System;

public class SoundPlayer : MonoBehaviour
{
	public AudioSource instantiation;
	private SoundCSVStructure soundCSVStructure;
	private AudioSource secondBGMAudioSource;
	public AudioSource firstBGMAudioSource;
	private List<AudioSource> seAudioSourceList;
	private List<AudioSource> voiceAudioSourceList;

	private Tweener tweener1;
	private Tweener tweener2;

	private const float FADE_DURATION = 3f;

	private float bgm;
	private float se;
	private float voioce;

	private void Awake ()
	{
		secondBGMAudioSource = CreateAudioSource ();
		firstBGMAudioSource = CreateAudioSource ();
		seAudioSourceList = new List<AudioSource> ();
		voiceAudioSourceList = new List<AudioSource> ();
	}

	private AudioSource CreateAudioSource ()
	{
		return Instantiator.GetInstance ().Instantiate<AudioSource> (instantiation, Vector2.zero, Vector3.one, gameObject.transform);
	}

	public void SetDefaultVolume (float bgm, float se, float volue)
	{
		secondBGMAudioSource.volume = firstBGMAudioSource.volume = this.bgm = bgm;
		this.se = se;
		this.voioce	= volue;
	}

	public void SetVolume (SoundTypeEnum soundTypeEnum, float value)
	{
		if (soundTypeEnum == SoundTypeEnum.BGM) {
			secondBGMAudioSource.volume = firstBGMAudioSource.volume = this.bgm = value;
		} else {
			SetVolumeList (soundTypeEnum, value);
		}
	}

	private void SetVolumeList (SoundTypeEnum soundTypeEnum, float value)
	{
		if (soundTypeEnum == SoundTypeEnum.SE) {
			se = value;
			int length = seAudioSourceList.Count;
			for (int i = 0; i < length; i++) {
				seAudioSourceList [i].volume = value;
			}
		} else {
			voioce = value;
			int length = voiceAudioSourceList.Count;
			for (int i = 0; i < length; i++) {
				voiceAudioSourceList [i].volume = value;
			}
		}
	}

	public AudioSource Play (AudioClip audioClip, SoundCSVStructure soundCSVStructure, bool isForceToReplay = true)
	{
		this.soundCSVStructure = soundCSVStructure;
		bool isLoop = (soundCSVStructure.loop == 1);
		if (ConvertIntToEnum (soundCSVStructure.type) == SoundTypeEnum.BGM) {
			if (!isForceToReplay && firstBGMAudioSource.clip != null && firstBGMAudioSource.clip.name == audioClip.name) {
				return firstBGMAudioSource;
			}

			secondBGMAudioSource.clip = firstBGMAudioSource.clip;
			secondBGMAudioSource.volume = firstBGMAudioSource.volume;
			FadeOut (secondBGMAudioSource);
			FadeIn (firstBGMAudioSource, audioClip, isLoop);
			return firstBGMAudioSource;
		} else {
			AudioSource audioSource = GetAudioSource (ConvertIntToEnum (soundCSVStructure.type), audioClip, isLoop);
			PlayAudioClip (audioSource, ConvertIntToEnum (soundCSVStructure.type), false);
			return audioSource;
		}
	}

	private void FadeIn (AudioSource audioSource, AudioClip audioClip, bool isLoop)
	{
		SetAudioSource (audioSource, audioClip, SoundTypeEnum.BGM, isLoop);
		audioSource.volume = 0;
		tweener1.Kill ();
		StopCoroutine (_StopBGM());
		tweener1 = audioSource.DOFade (bgm, FADE_DURATION);
		audioSource.Play ();
	}

	private void FadeOut (AudioSource audioSource)
	{
		tweener2.Kill ();
		tweener2 = audioSource.DOFade (0, FADE_DURATION).OnComplete (() => {
			audioSource.Pause ();
		});
		audioSource.UnPause ();
	}

	private SoundTypeEnum ConvertIntToEnum (int type)
	{
		return (SoundTypeEnum)Enum.Parse (typeof(SoundTypeEnum), soundCSVStructure.type.ToString ());
	}

	private void PlayAudioClip (AudioSource audioSource, SoundTypeEnum soundTypeEnum, bool isBGM)
	{
		if (audioSource.clip != null && !isBGM) {
			float length = audioSource.clip.length;
			StartCoroutine (Clean (length, soundTypeEnum, audioSource));
			audioSource.Play ();
		}
	}

	private IEnumerator Clean (float length, SoundTypeEnum soundTypeEnum, AudioSource audioSource)
	{
		yield return new WaitForSeconds (length);
		if (soundTypeEnum == SoundTypeEnum.SE) {
			seAudioSourceList.Remove (audioSource);
		} else {
			voiceAudioSourceList.Remove (audioSource);
		}
		Destroy (audioSource.gameObject);
	}

	private void SetAudioSource (AudioSource audioSource, AudioClip audioClip, SoundTypeEnum soundTypeEnum, bool isLoop)
	{
		audioSource.clip = audioClip;
		audioSource.loop = isLoop;
		if (soundTypeEnum == SoundTypeEnum.BGM) {
			audioSource.volume = bgm;
		} else if (soundTypeEnum == SoundTypeEnum.SE) {
			audioSource.volume = se;
			seAudioSourceList.Add (audioSource);
		} else {
			audioSource.volume = voioce;
			voiceAudioSourceList.Add (audioSource);
		}
	}

	private AudioSource GetAudioSource (SoundTypeEnum soundTypeEnum, AudioClip audioClip, bool isLoop)
	{
		AudioSource audioSource = CreateAudioSource ();
		SetAudioSource (audioSource, audioClip, soundTypeEnum, isLoop);
		return audioSource;
	}

	public void StopBGM (bool noFade, bool isForceToReplay = false,bool ignoreTimeScale = false)
	{
		tweener1.Kill ();
		if (!noFade) {
			if (ignoreTimeScale) {
				StopCoroutine (_StopBGM());
				StartCoroutine (_StopBGM());
			} else {
				tweener1 = firstBGMAudioSource.DOFade (0, FADE_DURATION / 2).OnComplete (() => {
					firstBGMAudioSource.Pause ();
					if (isForceToReplay) {
						firstBGMAudioSource.clip = null;
					}
				});
			}
		} else {
			firstBGMAudioSource.Pause ();
		}
	}

	IEnumerator _StopBGM(){
		float volumn = firstBGMAudioSource.volume;
		float t = FADE_DURATION / 2;
		while(t>0){
			t -= Time.unscaledDeltaTime;
			firstBGMAudioSource.volume = Mathf.Min(volumn,t / FADE_DURATION / 2);
			yield return null;
		}
	}

}
