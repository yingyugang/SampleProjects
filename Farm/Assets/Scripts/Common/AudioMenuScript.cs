using UnityEngine;
using System.Collections;

public class AudioMenuScript : MonoBehaviour
{
    void Awake()
    {
        // see if we've got game music still playing
        GameObject[] audioMenus = GameObject.FindGameObjectsWithTag("AudioMenu");
        if (audioMenus.Length > 1)
        {
            // kill game music
            for (int i = 1; i < audioMenus.Length; i++)
            {
                Destroy(audioMenus[i]);
            }
        }
        // make sure we survive going to different scenes
        DontDestroyOnLoad(transform.gameObject);
    }
}
