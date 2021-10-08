using UnityEngine;
using System.Collections;

public class VungleControl : MonoBehaviour
{
    public Transform DiamondEffect;
#if UNITY_ANDROID

    void OnEnable()
    {
        
    }


    void OnDisable()
    {
        
    }

    void onAdStartEvent()
    {
        Debug.Log("onAdStartEvent");
    }


    void onAdEndEvent()
    {
        Debug.Log("onAdEndEvent");
        Transform dm = Instantiate(DiamondEffect) as Transform;
        dm.GetComponent<DiamondEffect>().SetData(2);
    }


    void onCachedAdAvailableEvent()
    {
        Debug.Log("onCachedAdAvailableEvent");
    }


    void onVideoViewEvent(double watched, double length)
    {
        Debug.Log("onVideoViewEvent. watched: " + watched + ", length: " + length);
        // MobilePlugin.getInstance().ShowToast("onVideoViewEvent. watched: " + watched + ", length: " + length);
    }

#endif

    public void ShowVideoAd()
    {
        Debug.Log("sHOW VIDEO");
#if UNITY_ANDROID
      
        //MobilePlugin.getInstance().ShowToast("SHOW VIDEO VungleAndroid.isVideoAvailable() " + VungleAndroid.isVideoAvailable());
#endif
    }
}
