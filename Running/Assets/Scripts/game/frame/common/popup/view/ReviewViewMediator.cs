using System;
using UnityEngine.UI;
using UnityEngine;

public class ReviewViewMediator : PopupContentMediator
{

    public Button checkbox;
    public Image checkboxCon;

    protected override void Start()
    {
        base.Start();
        checkbox.onClick.AddListener(CheckBoxClickHandle);
        CheckBoxChanged();
    }

    protected override void NoButtonOnClickHandler ()
    {
        base.NoButtonOnClickHandler ();
        popupAction();
    }

    //TODO LBDDK replace the url
    #if UNITY_IOS
    const string _marketURL = "https://itunes.apple.com/jp/app/id1090811848";
    #else
    const string _marketURL = "https://play.google.com/store/apps/details?id=com.dtechno.osoparty";
    #endif
    protected override void YesButtonOnClickHandler ()
    {
        popupAction();
        //Open review URL
        Application.OpenURL(_marketURL);
        base.YesButtonOnClickHandler ();
    }

    private void CheckBoxClickHandle()
    {
        int _canShowReview = PlayerPrefs.GetInt("CanShowReview",1);
        PlayerPrefs.SetInt("CanShowReview",_canShowReview == 1 ? 0 : 1);
        CheckBoxChanged();
    }

    private void CheckBoxChanged()
    {
        if (PlayerPrefs.GetInt("CanShowReview",1) == 1)
        {
            checkboxCon.gameObject.SetActive(false);
        }
        else
        {
            checkboxCon.gameObject.SetActive(true);
        }
    }
}

