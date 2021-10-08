using UnityEngine;
using System.Collections;
using System;

public class DialogEventFriend : DialogAbs
{
    public static string key_data_total_friend = "key_data_total_friend";
    public static bool hasEvent = false;
    Transform main, black;
    public Transform DiamondEffect;
    void Awake()
    {
        hasEvent = false;
        main = transform.Find("Main");
        black = transform.Find("BgBlack");
        if (!PlayerPrefs.HasKey(key_data_total_friend))
        {
            PlayerPrefs.SetInt(key_data_total_friend, 0);
        }
        CheckEvent();
    }

    public void CheckEvent()
    {
        int numberFriend = PlayerPrefs.GetInt(key_data_total_friend, 0);
        //Set event 1
        if (!PlayerPrefs.HasKey("EventFriend1") && numberFriend > 1)
        {
            PlayerPrefs.SetInt("EventFriend1", 0);
        }
        //event 2
        if (!PlayerPrefs.HasKey("EventFriend2") && numberFriend > 30)
        {
            PlayerPrefs.SetInt("EventFriend2", 0);
        }
        if (!PlayerPrefs.HasKey("EventFriend3") && numberFriend > 50)
        {
            PlayerPrefs.SetInt("EventFriend3", 0);
        }
        if (!PlayerPrefs.HasKey("EventFriend4") && numberFriend > 100)
        {
            PlayerPrefs.SetInt("EventFriend4", 0);
        }
        if (!PlayerPrefs.HasKey("EventFriend5") && numberFriend > 150)
        {
            PlayerPrefs.SetInt("EventFriend5", 0);
        }
        if ((PlayerPrefs.HasKey("EventFriend1") && PlayerPrefs.GetInt("EventFriend1") != 1) ||
            (PlayerPrefs.HasKey("EventFriend2") && PlayerPrefs.GetInt("EventFriend2") != 1) ||
            (PlayerPrefs.HasKey("EventFriend3") && PlayerPrefs.GetInt("EventFriend3") != 1) ||
            (PlayerPrefs.HasKey("EventFriend4") && PlayerPrefs.GetInt("EventFriend4") != 1) ||
            (PlayerPrefs.HasKey("EventFriend5") && PlayerPrefs.GetInt("EventFriend5") != 1))
        {
            //Bat loi neu nguoi choiko o mission
            try
            {
                transform.parent.parent.Find("Button").Find("ButtonEvent").Find("Count").gameObject.SetActive(true);
            }
            catch (Exception e)
            {
                Debug.Log("-------------ERROR----------------" + e.Message);
            }
        }
        else
        {
            try
            {
                transform.parent.parent.Find("Button").Find("ButtonEvent").Find("Count").gameObject.SetActive(false);
            }
            catch (Exception e)
            {
                Debug.Log("-------------ERROR----------------" + e.Message);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (DialogEventFriend.hasEvent)
        {
            Debug.Log("Has event");
            CheckEvent();
            hasEvent = false;
        }
    }

    public override void ShowDialog(DialogAbs.CallBackShowDialog callback = null)
    {
        main.Find("Button1").Find("Get").Find("Label").GetComponent<UILabel>().text = MissionControl.Language["Get"];
        if (PlayerPrefs.HasKey("EventFriend1"))
        {
            main.Find("Button1").gameObject.SetActive(true);
        }
        else
        {
            main.Find("Button1").gameObject.SetActive(false);
        }
        if (PlayerPrefs.HasKey("EventFriend2"))
        {
            main.Find("Button2").gameObject.SetActive(true);
        }
        else
        {
            main.Find("Button2").gameObject.SetActive(false);
        }
        if (PlayerPrefs.HasKey("EventFriend3"))
        {
            main.Find("Button3").gameObject.SetActive(true);
        }
        else
        {
            main.Find("Button3").gameObject.SetActive(false);
        }
        if (PlayerPrefs.HasKey("EventFriend4"))
        {
            main.Find("Button4").gameObject.SetActive(true);
        }
        else
        {
            main.Find("Button4").gameObject.SetActive(false);
        }
        if (PlayerPrefs.HasKey("EventFriend5"))
        {
            main.Find("Button5").gameObject.SetActive(true);
        }
        else
        {
            main.Find("Button5").gameObject.SetActive(false);
        }
        /////////////////////////////////////////////////////////////////////////////
        if (PlayerPrefs.GetInt("EventFriend1", 0) == 1)
        {
            main.Find("Button1").Find("Complete").gameObject.SetActive(true);
            main.Find("Button1").Find("Get").gameObject.SetActive(false);
        }
        if (PlayerPrefs.GetInt("EventFriend2", 0) == 1)
        {
            main.Find("Button2").Find("Complete").gameObject.SetActive(true);
            main.Find("Button2").Find("Get").gameObject.SetActive(false);
        }
        if (PlayerPrefs.GetInt("EventFriend3", 0) == 1)
        {
            main.Find("Button3").Find("Complete").gameObject.SetActive(true);
            main.Find("Button3").Find("Get").gameObject.SetActive(false);
        }
        if (PlayerPrefs.GetInt("EventFriend4", 0) == 1)
        {
            main.Find("Button4").Find("Complete").gameObject.SetActive(true);
            main.Find("Button4").Find("Get").gameObject.SetActive(false);
        }
        if (PlayerPrefs.GetInt("EventFriend5", 0) == 1)
        {
            main.Find("Button5").Find("Complete").gameObject.SetActive(true);
            main.Find("Button5").Find("Get").gameObject.SetActive(false);
        }

        if ("Vietnamese".Equals(VariableSystem.language))
        {
            main.Find("Vi").gameObject.SetActive(true);
            main.Find("En").gameObject.SetActive(false);
        }
        else
        {
            main.Find("Vi").gameObject.SetActive(false);
            main.Find("En").gameObject.SetActive(true);
        }
        CommonObjectScript.isViewPoppup = true;
        if (!Show)
        {
            Show = true;
            main.gameObject.SetActive(true);
            black.gameObject.SetActive(true);
            LeanTween.scale(main.gameObject, Vector3.one, 0.3f).setEase(LeanTweenType.easeOutBack);
        }
    }

    public override void HideDialog(DialogAbs.CallBackHideDialog callback = null)
    {
        if (Show)
        {
            LeanTween.scale(main.gameObject, Vector3.zero, 0.3f).setEase(LeanTweenType.easeInBack).setOnComplete(() =>
            {
                CommonObjectScript.isViewPoppup = false;
                black.gameObject.SetActive(false);
                Show = false;
                main.gameObject.SetActive(false);
            });
        }
    }

    public void ButtonBuy1()
    {
        PlayerPrefs.SetInt("EventFriend1", 1);
        main.Find("Button1").Find("Complete").gameObject.SetActive(true);
        main.Find("Button1").Find("Get").gameObject.SetActive(false);
        AddDiamond(1, main.Find("Button1"));
    }
    public void ButtonBuy2()
    {
        PlayerPrefs.SetInt("EventFriend2", 1);
        main.Find("Button2").Find("Complete").gameObject.SetActive(true);
        main.Find("Button2").Find("Get").gameObject.SetActive(false);
        AddDiamond(5, main.Find("Button2"));
    }
    public void ButtonBuy3()
    {
        PlayerPrefs.SetInt("EventFriend3", 1);
        main.Find("Button3").Find("Complete").gameObject.SetActive(true);
        main.Find("Button3").Find("Get").gameObject.SetActive(false);
        AddDiamond(15, main.Find("Button3"));
    }
    public void ButtonBuy4()
    {
        PlayerPrefs.SetInt("EventFriend4", 1);
        main.Find("Button4").Find("Complete").gameObject.SetActive(true);
        main.Find("Button4").Find("Get").gameObject.SetActive(false);
        AddDiamond(25, main.Find("Button4"));
    }
    public void ButtonBuy5()
    {
        PlayerPrefs.SetInt("EventFriend5", 1);
        main.Find("Button5").Find("Complete").gameObject.SetActive(true);
        main.Find("Button5").Find("Get").gameObject.SetActive(false);
        AddDiamond(50, main.Find("Button5"));
    }

    void AddDiamond(int di, Transform parent)
    {
        Transform diamond = Instantiate(DiamondEffect) as Transform;
        diamond.position = parent.position;
        diamond.GetComponent<DiamondEffect>().SetData(di);
        diamond.parent = transform;
        diamond.localScale = new Vector3(1, 1, 1);
        CheckEvent();
    }
}
