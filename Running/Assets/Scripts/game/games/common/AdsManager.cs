using System;
using UnityEngine;
//using UnityEngine.Advertisements;
using System.Collections.Generic;
using NendUnityPlugin.AD;

public class AdsManager
{
    #region OUT Static IMG API

    public static void AdsInit()
    {
        // Maio Init
        InitMaioAds();
        // IMobile Init
        InitIMobileAds();
        // Colony Init
        InitColonyAds();
    }

    public static void ShowBtmBanner()
    {
        HideBtmBanner();
        AdsType randAds = GetRandomBTM();
        Debug.Log(randAds);
        switch (randAds)
        {
            case AdsType.IMobile:
                {
                    ShowIMobileBanner();
                    break;
                }
            case AdsType.Nend:
                {
                    if (m_needBanner == null)
                    {
                        m_needBanner = (NendAdBanner)CreateNendBanner();
                    }
                    ShowNendBanner(m_needBanner);
                    break;
                }
            case AdsType.Zucks:
                {
                    if (m_btmBanner == null)
                    {
                        m_btmBanner = CreateZucksBanner();
                    }
                    Margin margin = new Margin();

                    #if UNITY_IOS
                    ShowZucksBanner(m_btmBanner, new Vector2(320, 50), margin, "uwwasxz2ef");
                    #elif UNITY_ANDROID
                    ShowZucksBanner(m_btmBanner, new Vector2(320, 50), margin, "ek2ccbcwme");
                    #endif
                    break;
                }
            default:
                break;
        }
    }

    public static void HideBtmBanner()
    {
        if (m_btmBanner != null)
        {
            HideZucksBanner(m_btmBanner);
        }
        if (m_needBanner != null)
        {
            HideNendBanner(m_needBanner);
        }
        HideIMobileBanner();
    }

    public static void ShowRstBanner()
    {
        if (m_rstBanner == null)
        {
            m_rstBanner = CreateZucksBanner();
        }
        Margin margin = new Margin();
        margin.bottom = 200 * (Screen.height / 1136f);
        ShowZucksBanner(m_rstBanner, new Vector2(320, 250), margin, "qw3da3m3f4");
    }

    public static void HideRstBanner()
    {
        if (m_rstBanner != null)
        {
            HideZucksBanner(m_rstBanner);
        }
    }

    public static void ShowFCBanner()
    {
        ShowIMobileFC();
    }

    public static void HideFCBanner()
    {
        HideIMobileFC();
    }

    #endregion

    #region OUT Video API

    public delegate void VedioFinishedCallback(bool isSuccess = true);
    /* Don't need stop or hide UnityAdsBanner because view all control by user self
     * if videoType not none, then not random videoType
     */
    public static void ShowVedioBanner(VedioFinishedCallback vedioFinished = null, int videoIndex = 0, AdsType videoType = 0)
    {
        AdsType randAds;
        if (videoType != AdsType.None)
        {
            randAds = videoType;
        }
        else
        {
            randAds = GetRandomVideo();
        }
        Debug.Log(randAds);
        switch (randAds)
        {
            case AdsType.Maio:
                ShowMaioAdsVedio(vedioFinished, delegate(bool isSuccess)
                    {
                        if (!isSuccess)
                        {
                            ShowColonyAdsVedio(vedioFinished, delegate(bool isS)
                                {
                                    if (!isS)
                                    {
                                        ShowUnityAdsVedio(vedioFinished, vedioFinished);
                                    }
                                }, videoIndex);
                        }
                    }, videoIndex);
                break;
            case AdsType.Colony:
                ShowColonyAdsVedio(vedioFinished, delegate(bool isSuccess)
                    {
                        if (!isSuccess)
                        {
                            ShowMaioAdsVedio(vedioFinished, delegate(bool isS)
                                {
                                    if (!isS)
                                    {
                                        ShowUnityAdsVedio(vedioFinished, vedioFinished);
                                    }
                                }, videoIndex);
                        }
                    }, videoIndex);
                break;
            default:
                break;
        }

        //1. maioの動画広告を表示
        //2.（maioが広告切れの場合は）Unity Adsを表示する。
        //3.（Unity Adsの広告が切れている場合は）ポップアップを表示する。
//        return;
        //・IOS：Unity Ads 50% + Maio 50%
        //・Android：Unity Ads 100%
//        int tmpRand = UnityEngine.Random.Range(0,101);
//        if (tmpRand < 50 && ShowMaioAdsVedio(vedioFinished,delegate { ShowUnityAdsVedio(vedioFinished); }))
//        {
//            return;
//        }
//        else
//        {
//            ShowUnityAdsVedio(vedioFinished);
//        }
    }

    #endregion

    #region Zucks Ads

    private static ZucksAdStartShowRelativeBanner m_btmBanner;
    //bottom banner
    private static ZucksAdStartShowRelativeBanner m_rstBanner;
    //result page center

    private static ZucksAdStartShowRelativeBanner CreateZucksBanner()
    {
        GameObject obj = new GameObject();
        obj.SetActive(true);
        ZucksAdStartShowRelativeBanner banner = obj.AddComponent<ZucksAdStartShowRelativeBanner>();
        banner.Position = new BannerPosition[]{ BannerPosition.BOTTOM, BannerPosition.CENTER_HORIZONTAL };
        return banner;
    }

    private static void ShowZucksBanner(ZucksAdStartShowRelativeBanner banner, Vector2 size, Margin margin, string id)
    {
        banner.margin = margin;
        banner.ShowRelative(size.x, size.y, id);
    }

    private static void HideZucksBanner(ZucksAdStartShowRelativeBanner banner)
    {
        banner.Hide();
        banner = null;
    }

    #endregion

    #region Unity Ads

    private static bool ShowUnityAdsVedio(VedioFinishedCallback vedioFinished, VedioFinishedCallback vedioFailed)
    {
		return false;
//        if (Advertisement.isInitialized && Advertisement.IsReady())
//        {
//            ShowOptions options = new ShowOptions();
//            options.resultCallback = delegate(ShowResult result)
//            {
//                switch (result)
//                {
//                    case ShowResult.Finished:
//                        if (vedioFinished != null)
//                        {
//                            vedioFinished(true);
//                        }
//                        break;
//                    case ShowResult.Skipped:
//                        if (vedioFinished != null)
//                        {
//                            vedioFinished();
//                        }
//                        break;
//                    case ShowResult.Failed:
//                        if (vedioFailed != null)
//                        {
//                            vedioFailed(false);
//                        }
//                        break;
//                }
//            };
//            Advertisement.Show(null, options);
//            return true;
//        }
//        else
//        {
//            return false;
//        }
    }

    #endregion

    #region Maio Ads

    #if UNITY_IOS
    private const string m_maioVedioID = "mc261cd6439b96b177202fdaf740b273f";






    





#else
    private const string m_maioVedioID = "mf7d1228bc23c0c5f0398f6a441330b38";
    #endif

    private static void InitMaioAds()
    {
        #if DEVELOP
        Maio.SetAdTestMode(true);
        #endif
        Maio.Start(m_maioVedioID);


        Maio.OnFailed += new Maio.FailedEventHandler(delegate(string zoneId, Maio.FailReason reason)
            {
                AudioListener.pause = false;
                if (m_vedioFinished != null)
                {
                    m_vedioFinished(false);
                }
            });
        Maio.OnFinishedAd += new Maio.FinishedAdEventHandler(delegate(string zoneId, int playtime, bool skipped, string rewardParam)
            {
                AudioListener.pause = false;
                if (m_vedioFinished != null)
                {
                    m_vedioFinished(true);
                }
            });
    }

    private static bool ShowMaioAdsVedio(VedioFinishedCallback vedioFinished, VedioFinishedCallback vedioFailed, int index)
    {
        m_vedioFinished = vedioFinished;
        if (Maio.CanShow())
        {
            AudioListener.pause = true;
            Maio.Show();
            return true;
        }
        else
        {
            if (vedioFailed != null)
            {
                vedioFailed(false);
            }
            return false;
        }
    }

    private static VedioFinishedCallback m_vedioFinished;

    #endregion

    #region i-mobile Ads

    #if UNITY_IOS
    private const string IMOBILE_BANNER_PID_BTM = "47178";
    private const string IMOBILE_BANNER_MID_BTM = "267847";
    private const string IMOBILE_BANNER_SID_BTM = "851217";
    private const string IMOBILE_FULLSCREENAD_PID = "47178";
    private const string IMOBILE_FULLSCREENAD_MID = "267847";
    private const string IMOBILE_FULLSCREENAD_SID = "1029690";
    





#else
    private const string IMOBILE_BANNER_PID_BTM = "47178";
    private const string IMOBILE_BANNER_MID_BTM = "267847";
    private const string IMOBILE_BANNER_SID_BTM = "851223";
    private const string IMOBILE_FULLSCREENAD_PID = "47178";
    private const string IMOBILE_FULLSCREENAD_MID = "267848";
    private const string IMOBILE_FULLSCREENAD_SID = "1029695";
    #endif
    private static int m_imobileBannerView = int.MinValue;
    private static int m_imobileFCView = int.MinValue;
    private static bool iMobileAdsInited = false;

    private static void InitIMobileAds()
    {
        #if DEVELOP
//        IMobileSdkAdsUnityPlugin.setTestMode(true);
        #endif
        IMobileSdkAdsUnityPlugin.registerInline(IMOBILE_BANNER_PID_BTM, IMOBILE_BANNER_MID_BTM, IMOBILE_BANNER_SID_BTM);
        IMobileSdkAdsUnityPlugin.registerFullScreen(IMOBILE_FULLSCREENAD_PID, IMOBILE_FULLSCREENAD_MID, IMOBILE_FULLSCREENAD_SID);
        IMobileSdkAdsUnityPlugin.start();
        iMobileAdsInited = true;
    }

    private static void StopIMobileAds()
    {
        if (!iMobileAdsInited)
        {
            return;
        }
        IMobileSdkAdsUnityPlugin.stop();
        iMobileAdsInited = false;
    }

    private static void ShowIMobileBanner()
    {
        if (!iMobileAdsInited)
        {
            InitIMobileAds();
        }
        if (m_imobileBannerView > int.MinValue)
        {
            IMobileSdkAdsUnityPlugin.setVisibility(m_imobileBannerView, true);
        }
        else
        {
            m_imobileBannerView = IMobileSdkAdsUnityPlugin.show(
                IMOBILE_BANNER_SID_BTM,
                IMobileSdkAdsUnityPlugin.AdType.BANNER,
                IMobileSdkAdsUnityPlugin.AdAlignPosition.CENTER,
                IMobileSdkAdsUnityPlugin.AdValignPosition.BOTTOM,
                false
            );
        }
    }

    private static void HideIMobileBanner()
    {
        IMobileSdkAdsUnityPlugin.setVisibility(m_imobileBannerView, false);
    }

    private static void ShowIMobileFC()
    {
        if (!iMobileAdsInited)
        {
            InitIMobileAds();
        }
        IMobileSdkAdsUnityPlugin.show(IMOBILE_FULLSCREENAD_SID);
//        if (m_imobileFCView > int.MinValue)
//        {
//            IMobileSdkAdsUnityPlugin.setVisibility(m_imobileFCView, true);
//        }
//        else
//        {
//            m_imobileFCView = IMobileSdkAdsUnityPlugin.show(
//                IMOBILE_FULLSCREENAD_SID,
//                IMobileSdkAdsUnityPlugin.AdType.BIG_RECTANGLE,
//                IMobileSdkAdsUnityPlugin.AdAlignPosition.CENTER,
//                IMobileSdkAdsUnityPlugin.AdValignPosition.MIDDLE,
//                true
//            );
//        }
    }

    //Maybe unuseful
    private static void HideIMobileFC()
    {
        IMobileSdkAdsUnityPlugin.setVisibility(m_imobileFCView, false);
    }

    #endregion

    #region Nend Ads

    private static NendAdBanner m_needBanner;

    private static NendAd CreateNendBanner()
    {
        GameObject obj = new GameObject();
        obj.SetActive(true);
        NendAdBanner banner = obj.AddComponent<NendAdBanner>();
        return banner;
    }

    private static void ShowNendBanner(NendAd banner)
    {
        banner.Show();
    }

    private static void HideNendBanner(NendAd banner)
    {
        banner.Hide();
        banner = null;
    }

    #endregion

    #region AdColony

    #if UNITY_IOS
    private const string m_colonyAppID = "app2d6a69073904463a88";
    private const string m_colonyLifeID = "vzb82e938df0e94c7e82";
    private const string m_colonyBonusID = "vz3e7719b4465c49b4a5";
    





#else
    private const string m_colonyAppID = "app7b15f516a12b46a680";
    private const string m_colonyLifeID = "vze51a7c1176154246a1";
    private const string m_colonyBonusID = "vz52d079a08e1c42c9a3";
    #endif

    private static void InitColonyAds()
    {
        AdColony.Configure("version:" + SystemConstant.CLIENT_VERSION, m_colonyAppID, m_colonyLifeID, m_colonyBonusID);
//        AdColony.OnVideoFinished = new AdColony.VideoFinishedDelegate(delegate(bool ad_shown)
//            {
//                AudioListener.pause = false;
//                if (m_vedioFinished != null)
//                {
//                    m_vedioFinished(ad_shown);
//                }
//            });
        AdColony.OnV4VCResult = new AdColony.V4VCResultDelegate(delegate(bool success, string name, int amount)
            {
                AudioListener.pause = false;
                if (m_vedioFinished != null)
                {
                    m_vedioFinished(success);
                }
            });
    }

    private static bool ShowColonyAdsVedio(VedioFinishedCallback vedioFinished, VedioFinishedCallback vedioFailed, int index = 0)
    {
        string zoneID = "";
        switch (index)
        {
            case 0:
                zoneID = m_colonyLifeID;
                break;
            case 1:
                zoneID = m_colonyBonusID;
                break;
            default:
                // No ID found then return unable video callback
                break;
        }

        m_vedioFinished = vedioFinished;

        if (!AdColony.IsV4VCAvailable(zoneID) || !AdColony.ShowV4VC(false, zoneID))
        {
            if (vedioFailed != null)
            {
                vedioFailed(false);
            }
            return false;
        }
        else
        {
            AudioListener.pause = true;
            return true;
        }
    }

    #endregion

    #region Help Tools

    public enum AdsType
    {
        None,
        Zucks,
        Unity,
        Maio,
        IMobile,
        Nend,
        Colony
    }

    /// <summary>
    /// Gets the random BTM in imobile nend zucks
    /// </summary>
    /// <returns>The random BTM.</returns>
    public static AdsType GetRandomBTM()
    {
        int imobilePro = AdParameter.GetInstance.ads_imobile_pro;
        int nendPro = AdParameter.GetInstance.ads_nend_pro + imobilePro;
        int zucksPro = AdParameter.GetInstance.ads_zucks_pro + nendPro;
        int randomCount = UnityEngine.Random.Range(0, zucksPro + 1);
        if (randomCount < imobilePro)
        {
            return AdsType.IMobile;
        }
        else if (randomCount < nendPro)
        {
            return AdsType.Nend;
        }
        else
        {
            return AdsType.Zucks;
        }
    }

    public static AdsType GetRandomVideo()
    {
        int miaoPro = AdParameter.GetInstance.item_ads_maio_pro;
        int colonyPro = AdParameter.GetInstance.item_ads_colony_pro + miaoPro;
        int randomCount = UnityEngine.Random.Range(0, colonyPro + 1);
        if (randomCount < miaoPro)
        {
            return AdsType.Maio;
        }
        else if (randomCount < colonyPro)
        {
            return AdsType.Colony;
        }
        else
        {
            return AdsType.Colony;
        }
    }

    #endregion
}


