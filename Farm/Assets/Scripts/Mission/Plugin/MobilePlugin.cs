using UnityEngine;
using System.Collections;

public class MobilePlugin
{
    private static MobilePlugin instance;
    public static MobilePlugin getInstance()
    {
        if(instance == null)
        {
            instance = new MobilePlugin();
        }
        return instance;
    }

#if UNITY_ANDROID
    private AndroidJavaClass objJavaClass;
    private AndroidJavaObject objJavaObject;
    private MobilePlugin()
    {
        objJavaClass = new AndroidJavaClass("com.example.androidunityplugin.AndroidUnity");
        objJavaObject = new AndroidJavaObject("com.example.androidunityplugin.AndroidUnity");
    }

    public void ShowToast(string message, bool length_show = false)
    {
        objJavaClass.CallStatic("Toast", message, length_show);
    }

    public void ShowExitConfirm(string title, string message, string ok, string cancel)
    {
        objJavaClass.CallStatic("ShowCofirm", title, message, ok, cancel);
    }
#endif
}
