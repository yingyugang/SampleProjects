using UnityEngine;
using System.Collections;

namespace home
{
	public class Home : MonoBehaviour
	{
		public Header header;
		public Footer footer;
		public GameObject ad;


        void Awake()
        {
            if (PlayerPrefs.GetInt("AdUnlock",0) == 0)
            {
                AdsManager.ShowBtmBanner();
            }
            else
            {
                AdsManager.HideBtmBanner();
            }
        }

        void OnEnable()
        {
            if (PlayerPrefs.GetInt("AdUnlock",0) == 0)
            {
                AdsManager.ShowBtmBanner();
            }
            else
            {
                AdsManager.HideBtmBanner();
            }
        }

        void OnDisable()
        {
            AdsManager.HideBtmBanner();
        }

        void OnDestroy()
        {
            AdsManager.HideBtmBanner();
        }
	}
}