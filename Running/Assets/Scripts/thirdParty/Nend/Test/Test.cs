using System;
using UnityEngine;

namespace Plugins.Nend
{
    public class Test : MonoBehaviour
    {
        void Awake()
        {
            DontDestroyOnLoad(this);
        }

        void OnGUI()
        {
            if (GUI.Button(new Rect(0, 0, 100, 100), "Show Ads"))
            {
                AdsManager.ShowBtmBanner();
            }
            if (GUI.Button(new Rect(0, 100, 100, 100), "Hide Ads"))
            {
                AdsManager.HideBtmBanner();
            }
        }
    }
}

