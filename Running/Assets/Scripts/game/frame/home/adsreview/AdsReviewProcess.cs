using System;
using UnityEngine;

namespace Scripts.Home.AdsRev
{
    public class AdsReviewProcess
    {
        public delegate void ProcessCallback();

        private ProcessCallback m_processEnd;

        private AdsReviewProcess(ProcessCallback processEnd)
        {
            this.m_processEnd = processEnd;
        }

        public static void Start(ProcessCallback processEnd, PopupLoader popupLoader)
        {
            AdsReviewProcess process = new AdsReviewProcess(processEnd);
            // ready for ads pro

            if (process.CheckReview())
            {
                popupLoader.Popup(
                    PopupEnum.Review, 
                    delegate
                    {
                        process.m_processEnd();
                    },
                    null
                );
                return;
            }
            AdsResult result = process.CheckAdsPro();
            Debug.Log(result);
            switch (result)
            {
                case AdsResult.None:
                    {
                        process.m_processEnd();
                        break;
                    }
                case AdsResult.FullAds:
                    {
                        if (!process.isAdsUnClock())
                        {
                            AdsManager.ShowFCBanner();
                        }
                        process.m_processEnd();
                        break;
                    }
                case AdsResult.Maio:
                    {
                        if (process.CheckUserLife())
                        {
                            popupLoader.Popup(
                                PopupEnum.Ads, 
                                delegate
                                {
                                    process.m_processEnd();
                                },
                                new System.Collections.Generic.List<object>(){ popupLoader, AdsManager.AdsType.Maio }
                            );
                        }
                        break;
                    }
                case AdsResult.AdColony:
                    {
                        if (process.CheckUserLife())
                        {
                            popupLoader.Popup(
                                PopupEnum.Ads, 
                                delegate
                                {
                                    process.m_processEnd();
                                },
                                new System.Collections.Generic.List<object>(){ popupLoader, AdsManager.AdsType.Colony }
                            );
                        }
                        break;
                    }
                default:
                    break;
            }
        }

        #region Review Logic

        // レビューの抽選
        public bool CheckReview()
        {
//            PlayerPrefs.SetInt("CanShowReview",1);
            int _canShowReview = PlayerPrefs.GetInt("CanShowReview", 1);
            // レビュー画⾯を出さない設定が⼊っていない。出さないなら0%
            if (_canShowReview == 1)
            {
                if (CheckReviewHistory())
                {
                    return true;
                }
                else
                {
                    if (CheckReviewPro())
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            else
            {
                return false;
            }
        }

        // 5回⽬のプレイしているか？している場合は100%表⽰ Max=100
        private bool CheckReviewHistory()
        {
            int _gameHistory = PlayerPrefs.GetInt("GameHistory", 0);
            if (_gameHistory >= 4)
            {
                PlayerPrefs.SetInt("GameHistory", 0);
                return true;
            }
            else
            {
                PlayerPrefs.SetInt("GameHistory", ++_gameHistory);
                return false;
            }
        }

        // 5%の確率でレビューのお願いを表⽰ Max=100
        private bool CheckReviewPro()
        {
            int _orgPro = AdParameter.GetInstance.review_pro;
            int _randPro = UnityEngine.Random.Range(1, 101);
            if (_randPro <= _orgPro)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        // 課金した広告解除が適用されている、また確率で計算する　Max=100
        private bool isAdsUnClock()
        {
            return PlayerPrefs.GetInt("AdUnlock", 0) == 1;
        }

        #endregion

        #region Ads Logic

        enum AdsResult
        {
            None,
            FullAds,
            Maio,
            AdColony
        }

        // ライフが残り3以下である。
        private bool CheckUserLife()
        {
            if (ApMediator.currentRecovery < ApMediator.MAX_RECOVERY)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        // 15%の確率で動画広告表⽰
        private AdsResult CheckAdsPro()
        {
            int fullPro = AdParameter.GetInstance.ads_imobile_fc_pro;
            int miaoPro = AdParameter.GetInstance.ads_maio_pro + fullPro;
            int colonyPro = AdParameter.GetInstance.ads_colony_pro + miaoPro;
            int randomCount = UnityEngine.Random.Range(0, 101);
            Debug.Log("CheckAdsPro");
            Debug.Log(randomCount);
            if (randomCount < fullPro)
            {
                return AdsResult.FullAds;
            }
            else if (randomCount < miaoPro)
            {
                return AdsResult.Maio;
            }
            else if (randomCount < colonyPro)
            {
                return AdsResult.AdColony;
            }
            else
            {
                return AdsResult.None;
            }
        }

        #endregion
    }
}

