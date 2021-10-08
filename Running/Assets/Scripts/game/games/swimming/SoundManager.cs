using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

namespace Swimming
{
	public enum SoundType
	{
		BgInGame,
		BgImmortal,
		Hurt,
		GetItem,
		Start,
		Cutin,
		GameOver
	}

	[Serializable]
	public struct Sound
	{
		public SoundType type;
		public AudioClip clip;
	}

	public class SoundManager : MonoBehaviour
	{
		public AudioSource bgSource = null;
		public AudioSource sfxSource = null;

		public Sound[] Clips;           // List sound

		private static SoundManager m_Instance = null;
		public static SoundManager Instance
		{
			get
			{
				if (m_Instance == null)
				{
					m_Instance = GameObject.FindObjectOfType<SoundManager>();
				}

				return m_Instance;
			}
		}

		void Awake()
		{
			if (m_Instance == null)
			{
				//If I am the first instance, make me the Singleton
				m_Instance = this;
			}
			else {
				//If a Singleton already exists and you find
				//another reference in scene, destroy it!
				if (this != m_Instance)
					Destroy(this.gameObject);
			}
			Init();
		}

		void Init()
		{
			if (sfxSource == null)
			{
				sfxSource = gameObject.AddComponent<AudioSource>();
			}
			if (bgSource == null)
			{
				bgSource = gameObject.AddComponent<AudioSource>();
			}
		}

		AudioClip GetSoundByType(SoundType type)
		{
			foreach (var v in Clips)
			{
				if (v.type == type)
					return v.clip;
			}
			return null;
		}

		#region Sound
		public void PauseSound()
		{
			sfxSource.Pause();
		}

		public void StopSound()
		{
			sfxSource.Stop();
		}

		public void ResumeSound()
		{
			sfxSource.UnPause();
		}

		public void PlaySfx(SoundType type)
		{
			AudioClip clip = GetSoundByType(type);
			if (clip != null)
			{
				sfxSource.PlayOneShot(clip);
			}
		}

		public void SetSoundVolume(float value)
		{
			value = (value < 0) ? 0 : value;
			value = (value > 1) ? 1 : value;
			sfxSource.volume = value;
		}

		#endregion

		#region Background music
		public void PauseMusic()
		{
			bgSource.Pause();
		}

		public void ResumeMusic()
		{
			bgSource.UnPause();
		}

		public void StopMusic()
		{
			bgSource.Stop();
		}

		public void PlayMusic(SoundType type, bool loop = true)
		{
			AudioClip clip = GetSoundByType(type);

			if (clip == null)
			{
				return;
			}

			if (bgSource == null)
			{
				Debug.Log("Null pointer...");
			}
			bgSource.loop = loop;
			bgSource.clip = clip;
			bgSource.Play();
		}

		public void SetMusicVolume(float value)
		{
			value = (value < 0) ? 0 : value;
			value = (value > 1) ? 1 : value;
			bgSource.volume = value;
		}
		#endregion
	}
}
