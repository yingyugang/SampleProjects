using UnityEngine;
using System.Collections;
using System;

public enum SheeType
{
	PRACTICE,
	INVICIBLE,
	RIGHT,
	MISS,
	BONUS,
	OPEN
}

[Serializable]
public struct SheeSound
{
	public SheeType		type;
	public AudioClip	clip;
}

public class SheeSoundManager : MonoBehaviour {

	private static SheeSoundManager m_Instance;
	public static SheeSoundManager instance{
		get{
			if(m_Instance == null) m_Instance = GameObject.FindObjectOfType<SheeSoundManager>();
			return m_Instance;
		}
	}

	public SheeSound[] background;
	public SheeSound[] effect;

	public AudioSource musics; // Backgrounds
	public AudioSource sounds; // Sound Effects

	AudioClip GetSoundByType(SheeType type, SheeSound[] clips)
	{
		foreach (SheeSound v in clips) {
			if (v.type == type)
				return v.clip;
		}

		return null;
	}

	public void PlayBackground(SheeType type){
		musics.clip = GetSoundByType(type, background);
		musics.Play ();
		musics.loop = true;
	}

	public void PlayEffect(SheeType type){
		sounds.Stop ();
		sounds.PlayOneShot(GetSoundByType(type, effect));
	}
}