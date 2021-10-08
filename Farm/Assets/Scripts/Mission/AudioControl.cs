using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class AudioControl : MonoBehaviour
{
    public static bool soundEnable = true;
    public static bool musicEnable = true;

    public static string key_time_add_heart = "time_add_heart";
    public static int max_time_receive_heat = 600;//600s = 10phut
    public static long time_add_heart;
    static  string tag = "AudioControl";
    static string tag_sound_playing = "SoundPlaying";
    static string tag_music_playing = "MusicPlaying";

    public AudioClip[] Audios;
    Dictionary<string, AudioClip> dictAudio;
    void Awake()
    {
        this.gameObject.tag = tag;
        GameObject[] audioControls = GameObject.FindGameObjectsWithTag(tag);
        if (audioControls.Length > 1)
        {
            for (int i = 1; i < audioControls.Length; i++)
            {
                Destroy(audioControls[i]);
            }
        }
        else
        {
        }
        dictAudio = new Dictionary<string, AudioClip>();
        for (int i = 0; i < Audios.Length; i++)
        {
            dictAudio.Add(Audios[i].name, Audios[i]);
        }
        DontDestroyOnLoad(this.gameObject);

        time_add_heart = Convert.ToInt32(PlayerPrefs.GetString(key_time_add_heart, "0"));
        //Debug.Log("Thoi se duoc tang tim " + time_add_heart);
    }
    void Update()
    {
        if (Application.isLoadingLevel)
            transform.localPosition = Vector3.zero;

        if (CommonObjectScript.isLeavedFarm && !Application.loadedLevelName.Equals("Farm"))//restore position of commonObject
        {
            transform.localPosition = Vector3.zero;
        }
    }
    void OnDestroy()
    {
        //Luu lai thoi gian thoat
        //Debug.Log("Luu lai thoi gian " + DString.GetTimeNow());
        //AudioControl.time_add_heart = (DString.GetTimeNow() + AudioControl.max_time_receive_heat);
        //PlayerPrefs.SetString(AudioControl.key_time_add_heart, "" + AudioControl.time_add_heart);
        VariableSystem.SaveData();
        DataCache.SaveAchievementCache();
    }

    public void PlaySound(string audioName)
    {
        //Debug.Log("Play sound " + audioName);
        try
        {
            if (AudioControl.soundEnable)
            {
                GetComponent<AudioSource>().PlayOneShot(dictAudio[audioName]);
            }
        }
        catch (KeyNotFoundException e)
        {
            Debug.Log("The sound file name \"" + audioName + "\" not found");
        }
    }
    public void PlaySoundInstance(string soundName, bool loop = true, bool hold = false, float volume = 1.0f)
    {
        try
        {
            string nameChild = "SoundLoopInstance(" + soundName + ")";
            GameObject old = GameObject.Find(nameChild);
            if (old != null)
            {
                Destroy(old);
            }
            GameObject gameObject = new GameObject();
            gameObject.name = nameChild;
            //gameObject.transform.parent = this.transform;
            gameObject.AddComponent<AudioSource>();
            gameObject.GetComponent<AudioSource>().clip = dictAudio[soundName];
            gameObject.GetComponent<AudioSource>().volume = volume;
            gameObject.GetComponent<AudioSource>().loop = loop;
            gameObject.tag = tag_sound_playing;
            if (hold)
            {
                DontDestroyOnLoad(gameObject);
            }
            if (AudioControl.soundEnable)
            {
                gameObject.GetComponent<AudioSource>().Play();
            }
            else
            {
                gameObject.GetComponent<AudioSource>().Stop();
            }
        }
        catch (KeyNotFoundException e)
        {
            Debug.Log("The sound file name \"" + soundName + "\" not found");
        }
    }

    public void PlayMusic(string audioName)
    {
        try
        {
            if (AudioControl.soundEnable)
            {
                GetComponent<AudioSource>().PlayOneShot(dictAudio[audioName]);
            }
        }
        catch (KeyNotFoundException e)
        {
            Debug.Log("The sound file name \"" + audioName + "\" not found");
        }
    }
    public void PlayMusicInstance(string audioName, bool loop = true, bool hold = false, float volume = 1.0f)
    {
        try
        {
            string nameChild = "MusicLoopInstance(" + audioName + ")";
            GameObject old = GameObject.Find(nameChild);
            if (old != null)
            {
                Destroy(old);
            }
            GameObject gameObject = new GameObject();
            gameObject.name = nameChild;
            // gameObject.transform.parent = this.transform;
            gameObject.AddComponent<AudioSource>();
            gameObject.GetComponent<AudioSource>().clip = dictAudio[audioName];
            gameObject.tag = tag_music_playing;
            gameObject.GetComponent<AudioSource>().volume = volume;
            gameObject.GetComponent<AudioSource>().loop = loop;
            if (hold)
            {
                DontDestroyOnLoad(gameObject);
            }
            if (AudioControl.musicEnable)
            {
                gameObject.GetComponent<AudioSource>().Play();
            }
            else
            {
                gameObject.GetComponent<AudioSource>().Stop();
            }
        }
        catch (KeyNotFoundException e)
        {
            Debug.Log("The sound file name \"" + audioName + "\" not found");
        }
    }

    //Ham static - chưa chắc có dùng ko. vì nó liên quan đến bộ nhớ
    public static void DPlaySound(string audioName)
    {
        try
        {
            GameObject.Find("AudioControl").GetComponent<AudioControl>().PlaySound(audioName);
        }
        catch (Exception e)
        {
            Debug.Log("Error when playing sound " + audioName + ": " + e.Message);
        }
    }
    public static void DPlaySoundInstance(string soundName, bool loop = true, bool hold = false, float volume = 1.0f)
    {
        try
        {
            GameObject.Find("AudioControl").GetComponent<AudioControl>().PlaySoundInstance(soundName, loop, hold, volume);
        }
        catch (Exception e)
        {
            Debug.Log("Error when playing sound " + soundName + ": " + e.Message);
        }
    }
    public static void DPlayMusic(string audioName)
    {
        try
        {
            GameObject.Find("AudioControl").GetComponent<AudioControl>().PlayMusic(audioName);
        }
        catch (Exception e)
        {
            Debug.Log("Error when playing sound " + audioName + ": " + e.Message);
        }
    }

    public static void DPlayMusicInstance(string audioName, bool loop = true, bool hold = false, float volume = 1.0f)
    {
        try
        {
            GameObject.Find("AudioControl").GetComponent<AudioControl>().PlayMusicInstance(audioName, loop, hold, volume);
        }
        catch (Exception e)
        {
            Debug.Log("Error when playing sound " + audioName + ": " + e.Message);
        }
    }

    public static void StopMusic(string audioName)
    {
        string nameChild = "MusicLoopInstance(" + audioName + ")";
        GameObject old = GameObject.Find(nameChild);
        //Debug.Log("------------------++++++++++++++++++++++---------------------" + old.name);
        if (old != null)
        {
            //Debug.Log("------------------DESTROY---------------------" + old.name);
            Destroy(old);
        }
    }
    public static void StopSound(string audioName)
    {
        string nameChild = "SoundLoopInstance(" + audioName + ")";
        GameObject old = GameObject.Find(nameChild);
        if (old != null)
        {
            Destroy(old);
        }
    }

    public static MonoBehaviour getMonoBehaviour()
    {
        return GameObject.Find("AudioControl").GetComponent<AudioControl>();
    }

    public static void AddHeart(int heart)
    {
        VariableSystem.heart += heart;
        if (heart < 0 && VariableSystem.heart == 4)
        {
            Debug.Log("---------------------Bat dau luu thoi gian cong tim----------------------");
            AudioControl.time_add_heart = (DString.GetTimeNow() + AudioControl.max_time_receive_heat);
            PlayerPrefs.SetString(AudioControl.key_time_add_heart, "" + AudioControl.time_add_heart);
        }
        if (VariableSystem.heart > 5)
        {
            VariableSystem.heart = 5;
        }
        PlayerPrefs.SetInt("heart", heart);

    }

    public static void StopAllSound()
    {
        GameObject[] soundPlaying = GameObject.FindGameObjectsWithTag(tag_sound_playing);
        for (int i = 0; i < soundPlaying.Length; i++ )
        {
            soundPlaying[i].GetComponent<AudioSource>().Stop();
        }
        PlayerPrefs.SetInt("sound_enable", 0);
        AudioControl.soundEnable = false;
    }

    public static void ResumeAllSound()
    {
        GameObject[] soundPlaying = GameObject.FindGameObjectsWithTag(tag_sound_playing);
        for (int i = 0; i < soundPlaying.Length; i++)
        {
            soundPlaying[i].GetComponent<AudioSource>().Play();
        }
        PlayerPrefs.SetInt("sound_enable", 1);
        AudioControl.soundEnable = true ;
    }
    public static void StopAllMusic()
    {
        GameObject[] soundPlaying = GameObject.FindGameObjectsWithTag(tag_music_playing);
        for (int i = 0; i < soundPlaying.Length; i++)
        {
            soundPlaying[i].GetComponent<AudioSource>().Stop();
        }
        PlayerPrefs.SetInt("music_enable", 0);
        AudioControl.musicEnable = false;
    }

    public static void ResumeAllMusic()
    {
        GameObject[] soundPlaying = GameObject.FindGameObjectsWithTag(tag_music_playing);
        for (int i = 0; i < soundPlaying.Length; i++)
        {
            soundPlaying[i].GetComponent<AudioSource>().Play();
        }
        PlayerPrefs.SetInt("music_enable", 1);
        AudioControl.musicEnable = true;
    }
}
