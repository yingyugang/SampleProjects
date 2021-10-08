using UnityEngine;
using System.Collections;

public class StartScene : MonoBehaviour {

    void Start()
    {
        Screen.SetResolution(1280, 720, true);
        Application.LoadLevel("LoadingScene");
    }
}
