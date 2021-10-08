using UnityEngine;
using System.Collections;

public class AdmobControl : MonoBehaviour
{

  

    public bool ShowFullOnStart = false;

    void Start()
    {
        
    }

    void OnEnable()
    {
        
    }

    void OnDisable()
    {
        
    }

    void HandleAdLoaded()
    {
#if !UNITY_EDITOR
		
#endif
    }

    void HandleInterstitialLoaded()
    {
#if !UNITY_EDITOR
		
#endif
    }

    public void ShowBanner()
    {
#if !UNITY_EDITOR
        
#endif
    }

    public void HideBanner()
    {
#if !UNITY_EDITOR
        
#endif
    }
}
