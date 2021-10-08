using System;
using UnityEngine;
using System.Runtime.InteropServices;

namespace Scripts.ThirdParty
{
    public class ClipboardUtil
    {

        [DllImport("__Internal")]
        private static extern void IOSClipboardSet(string str);

        [DllImport("__Internal")]
        private static extern System.String IOSClipboardGet();

        public static void ClipboardSet(string str)
        {

            #if UNITY_EDITOR
            GUIUtility.systemCopyBuffer = str;
            #elif UNITY_ANDROID
            AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject context = jc.GetStatic<AndroidJavaObject>("currentActivity");
            AndroidJavaClass util = new AndroidJavaClass("com.coc.ClipboardUtil");
            util.CallStatic("copyToCB", str, context);
            #elif UNITY_IOS
            IOSClipboardSet(str);
            #endif
        }

        public static string ClipboardGet()
        {
            string result = string.Empty;       
            #if UNITY_EDITOR
            result = GUIUtility.systemCopyBuffer;
            #elif UNITY_ANDROID
            AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject context = jc.GetStatic<AndroidJavaObject>("currentActivity");
            AndroidJavaClass util = new AndroidJavaClass("com.coc.ClipboardUtil");
            result = util.CallStatic<String>("getFromCB", context);
            #elif UNITY_IOS
            result = IOSClipboardGet();
            #endif
            return result;
        }
    }
}

