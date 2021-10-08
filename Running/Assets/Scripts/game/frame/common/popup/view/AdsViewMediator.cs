using System;
using UnityEngine.UI;
using UnityEngine;
using home;
using System.Collections.Generic;

public class AdsViewMediator : PopupContentMediator
{
    public Text contentTxt;

    public Image contentAImg;
    public Image contentBImg;
    public Image contentCImg;
    public HeaderMediator headerMediator;

    protected override void Start()
    {
        base.Start();
        if (AdParameter.GetInstance.review_txt.IndexOf("#") < 0)
        {
            contentTxt.text = AdParameter.GetInstance.review_txt;
        }
        else
        {
            string[] txts = AdParameter.GetInstance.review_txt.Split('#');
            int txtRand = UnityEngine.Random.Range(0, txts.Length);
            contentTxt.text = txts[txtRand];
        }

        int imageRand = UnityEngine.Random.Range(0, 3);
        switch (imageRand)
        {
            case 0:
                contentAImg.gameObject.SetActive(true);
                break;
            case 1:
                contentBImg.gameObject.SetActive(true);
                break;
            case 2:
                contentCImg.gameObject.SetActive(true);
                break;
            default:
                break;
        }
        UnityEngine.Analytics.Analytics.CustomEvent("ad_douga", new Dictionary<string, object>());
    }

    protected override void OKButtonOnClickHandler()
    {
        UnityEngine.Analytics.Analytics.CustomEvent("ad_douga_tap", new Dictionary<string, object>());
        AdsManager.AdsType adsType = (AdsManager.AdsType)objectList[1];
        AdsManager.ShowVedioBanner(delegate(bool isSuccess)
            {
                if (isSuccess)
                {
                    // Send API
                    GameObject obj = new GameObject("RecoverAdLogic");
                    obj.SetActive(true);
                    RecoverAdLogic adLogic = obj.AddComponent<RecoverAdLogic>();
                    adLogic.complete = () =>
                    {
                        headerMediator.UpdateAp();
                        (objectList[0] as PopupLoader).Popup(
                            PopupEnum.Ad, 
                            delegate
                            {
                                popupAction();
                            },
                            new System.Collections.Generic.List<object>(){ LanguageJP.ADS_TITLE, LanguageJP.ADS_AP_UP }
                        );
                    };
                    adLogic.error = (string status) =>
                    {
                        popupAction();
                    };
                    adLogic.SendAPI();
                }
                else
                {
                    (objectList[0] as PopupLoader).Popup(
                        PopupEnum.Ad, 
                        delegate
                        {
                            popupAction();
                        },
                        new System.Collections.Generic.List<object>(){ LanguageJP.ADS_TITLE, LanguageJP.ADS_NOT_EXIST }
                    );
                }
            }, 0, adsType);
        base.OKButtonOnClickHandler();
    }

    protected override void NoButtonOnClickHandler()
    {
        base.NoButtonOnClickHandler();
        popupAction();
    }
}