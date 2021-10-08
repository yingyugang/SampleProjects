using UnityEngine;
using System.Collections;
using Facebook.MiniJSON;
using System.Collections.Generic;
using System.Xml;
using System.IO;
using System;

public class MenuControll : MonoBehaviour
{
    public Transform DialogConfirm;
    public Texture[] textureButtons;
    public UITexture[] textureBt;
    Transform dialogLogin;
    Transform pig;
    bool isShowLogin;
    bool exitScene;
    UILabel lbTouchStart;
    float time;

    void Awake()
    {
        time = 0;
        //Read xml achievement in data
        GoogleAnalytics.instance.LogScreen("Menu Screen");
        exitScene = false;
        //Xu ly su kien con heo 
        //Debug.Log(GameObject.Find("BackgroundMenu").transform.FindChild("back1"));
        pig = GameObject.Find("BackgroundMenu").transform.Find("back1").Find("heo");
        LeanTween.moveX(pig.gameObject, -2.5f, 5).setOnCompleteOnRepeat(true).setOnComplete(() =>
        {
            pig.transform.localScale = new Vector3(-1 * pig.transform.localScale.x, pig.transform.localScale.y, 1);
            System.Random rd = new System.Random();
            int i = rd.Next(0, 3);
            if (i == 1)
            {
                pig.Find("lon").GetComponent<Animator>().SetTrigger("nhay");
            }
        }).setLoopPingPong();
        //audio
        //audioControl = GameObject.Find("AudioControl").GetComponent<AudioControl>();
        AudioControl.soundEnable = (PlayerPrefs.GetInt("sound_enable", 1) == 1);
        AudioControl.musicEnable = (PlayerPrefs.GetInt("music_enable", 1) == 1);

        isShowLogin = false;
        dialogLogin = transform.parent.Find("DialogLogin");
        //buttonPlay.gameObject.SetActive(false);
        if (MissionControl.Language == null)
        {
            //Doc xml ngon ngu
            MissionControl.LanguageEN = new Dictionary<string, string>();
            MissionControl.LanguageVI = new Dictionary<string, string>();
            TextAsset xml = Resources.Load<TextAsset>("Mission/Language/EN"); //Read File xml
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(new StringReader(xml.text));
            XmlNodeList xmlNodeList = xmlDoc.DocumentElement.ChildNodes;
            foreach (XmlNode node in xmlNodeList)
            {
                MissionControl.LanguageEN.Add(node.Name, node.InnerText);
                //Debug.Log(node.Name);
            }
            //vn
            TextAsset xml1 = Resources.Load<TextAsset>("Mission/Language/VI"); //Read File xml
            XmlDocument xmlDoc1 = new XmlDocument();
            xmlDoc1.Load(new StringReader(xml1.text));
            XmlNodeList xmlNodeList1 = xmlDoc1.DocumentElement.ChildNodes;
            foreach (XmlNode node in xmlNodeList1)
            {
                MissionControl.LanguageVI.Add(node.Name, node.InnerText);
            }
            MissionControl.Language = MissionControl.LanguageEN;
            if ("Vietnamese".Equals(VariableSystem.language))
            {
                MissionControl.Language = MissionControl.LanguageVI;
            }
        }
        //Khong cho click truoc khi di chuyen xong
        GetComponent<UIButton>().enabled = false;

        lbTouchStart = transform.Find("Label").GetComponent<UILabel>();
        lbTouchStart.gameObject.SetActive(false);
        // Khở tạo ngôn ngữ cho HungBV
        SetLaguage();
        transform.Find("Label").GetComponent<UILabel>().text = MissionControl.Language["TOUCH_START"];
        if ("Vietnamese".Equals(VariableSystem.language))
        {
            textureBt[0].mainTexture = textureButtons[0];
            textureBt[1].mainTexture = textureButtons[2];
        }
        else
        {
            textureBt[0].mainTexture = textureButtons[1];
            textureBt[1].mainTexture = textureButtons[3];
        }
    }

    void Start()
    {
        AudioControl.DPlayMusicInstance("Nhac menu", true, true);
#if UNITY_ANDROID && !UNITY_EDITOR
        if (CheckPackage.check())
        {
            Debug.Log("---------------APP HACK CMNR--------------------");
            MobilePlugin.getInstance().ShowToast("HACK CMNR");
            Application.Quit();
        }
        else
        {
            //MobilePlugin.getInstance().ShowToast("-------------APP KO HACK------------");
        }
#endif
    }

    void SetLaguage()
    {
        if (FactoryScenesController.languageHungBV == null)
        {
            //Doc xml ngon ngu
            FactoryScenesController.languageHungBVEN = new Dictionary<string, string>();
            FactoryScenesController.languageHungBVVI = new Dictionary<string, string>();

            TextAsset xml = Resources.Load<TextAsset>("Factory/XMLFile/EN"); //Read File xml
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(new StringReader(xml.text));
            XmlNodeList xmlNodeList = xmlDoc.DocumentElement.ChildNodes;
            foreach (XmlNode node in xmlNodeList)
            {
                FactoryScenesController.languageHungBVEN.Add(node.Name, node.InnerText);
            }
            //vn
            TextAsset xml1 = Resources.Load<TextAsset>("Factory/XMLFile/VI"); //Read File xml
            XmlDocument xmlDoc1 = new XmlDocument();
            xmlDoc1.Load(new StringReader(xml1.text));
            XmlNodeList xmlNodeList1 = xmlDoc1.DocumentElement.ChildNodes;
            foreach (XmlNode node in xmlNodeList1)
            {
                FactoryScenesController.languageHungBVVI.Add(node.Name, node.InnerText);
            }

            if ("Vietnamese".Equals(VariableSystem.language))
                FactoryScenesController.languageHungBV = FactoryScenesController.languageHungBVVI;
            else
                FactoryScenesController.languageHungBV = FactoryScenesController.languageHungBVEN;
        }
    }
    public void PlayButton()
    {
        //audioControl.PlaySound("Click 1");
        AudioControl.DPlaySound("Click 1");
        if (FB.IsLoggedIn)
        {
            MissionControl.GetFriendsList();
            LoadingScene.ShowLoadingScene("Mission", true);
            if (!exitScene)
            {
                exitScene = true;
            }
        }
        else
            ShowLoginDialog();

    }

    ////Mot Coroutine de check FB khoi tao thanhcong va se goi  bang dang nhap
    //IEnumerator AAAAAAAAAAAAA()
    //{
    //    while (true)
    //    {
    //        if (FB.IsInitialized)
    //        {
    //            //Debug.Log("KHOI TAO XONG");
    //            if (!FB.IsLoggedIn)
    //            {
    //                DFB.FBLogin(result =>
    //                {
    //                    if (FB.IsLoggedIn)
    //                    {
    //                        FB.API("me", Facebook.HttpMethod.GET, rsl =>
    //                        {
    //                            IDictionary dict1 = Json.Deserialize(rsl.Text) as IDictionary;
    //                            if (dict1 != null && dict1["id"] != null)
    //                            {
    //                                DFB.UserId = "" + dict1["id"];
    //                                DFB.UserName = "" + dict1["name"];
    //                                try
    //                                {
    //                                    AudioControl.getMonoBehaviour().StartCoroutine(DHS.PostMeInfo(DFB.UserId, DFB.UserName, "" + dict1["locale"], "" + dict1["last_name"]));
    //                                }
    //                                catch (Exception e)
    //                                {
    //                                    Debug.Log("--------------------catch error StartCoroutine-------------------" + e.Message);
    //                                }
    //                            }
    //                        });
    //                        HideLoginDialog();
    //                    }
    //                    else
    //                    {
    //                        isShowLogin = false;
    //                    }
    //                });
    //            }
    //            else
    //            {
    //                HideLoginDialog();
    //            }
    //            yield break;
    //        }
    //        yield return false;
    //        //Debug.Log("CHUA KHOI TAO XONG");
    //    }
    //}

    public void ShowLoginDialog()
    {
        dialogLogin.GetComponent<DialogLogin>().ShowDialog();
    }

    public void HideLoginDialog()
    {
        if (FB.IsLoggedIn)
        {
            MissionControl.GetFriendsList();
        }
        dialogLogin.GetComponent<DialogLogin>().HideDialog(() =>
        {
            LoadingScene.ShowLoadingScene("Mission", true);

        });
        if (!exitScene)
        {
            exitScene = true;
        }
    }

    public void NothankButton()
    {
        AudioControl.DPlaySound("Click 1");
        HideLoginDialog();
    }

    public void LoginButton()
    {
        AudioControl.DPlaySound("Click 1");
        if (FB.IsInitialized)
        {
            if (!isShowLogin)
            {
                isShowLogin = true;
                if (!FB.IsLoggedIn)
                {
                    DFB.FBLogin(result =>
                    {
                        if (FB.IsLoggedIn)
                        {
                            if (String.IsNullOrEmpty(PlayerPrefs.GetString(DataCache.FB_ID, "")))
                            {
                                FB.API("v2.2/me", Facebook.HttpMethod.GET, rsl =>
                                {
                                    IDictionary dict1 = Json.Deserialize(rsl.Text) as IDictionary;
                                    if (dict1 != null && dict1["id"] != null)
                                    {
                                        DFB.UserId = "" + dict1["id"];
                                        DFB.UserName = "" + dict1["name"];
                                        PlayerPrefs.SetString(DataCache.FB_ID, DFB.UserId);
                                        PlayerPrefs.SetString(DataCache.FB_USER, DFB.UserName);
                                        try
                                        {
                                            AudioControl.getMonoBehaviour().StartCoroutine(DHS.PostMeInfo(DFB.UserId, DFB.UserName, "" + dict1["locale"], "" + dict1["last_name"]));
                                        }
                                        catch (Exception e)
                                        {
                                            Debug.Log("--------------------catch error StartCoroutine-------------------" + e.Message);
                                        }
                                    }
                                });
                            }
                            else
                            {
                                DFB.UserId = PlayerPrefs.GetString(DataCache.FB_ID);
                                DFB.UserName = PlayerPrefs.GetString(DataCache.FB_USER);
                            }
                            HideLoginDialog();
                        }
                        else
                        {
                            isShowLogin = false;
                            ShowCofirm();
                        }
                    });
                }
                else
                {
                    HideLoginDialog();
                }
            }
        }
        else
        {
            ShowCofirm();
        }
    }

    void ShowCofirm()
    {
        Transform confirm = Instantiate(DialogConfirm) as Transform;
        confirm.GetComponent<DialogConfirm>().ShowDialogHideCancel(MissionControl.Language["Login"], MissionControl.Language["Check_Network"]);
    }

    public void BackgroundMoveFinish()
    {
        GetComponent<UIButton>().enabled = true;
        lbTouchStart.gameObject.SetActive(true);
    }

    public void Update()
    {
        time += Time.deltaTime;
        if (time > 5)
        {
            time = 0;
            if (!GetComponent<UIButton>().enabled)
            {
                GetComponent<UIButton>().enabled = true;
            }
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (CommonObjectScript.isViewPoppup)
            {
                dialogLogin.GetComponent<DialogLogin>().HideDialog(() =>
                {

                });
                CommonObjectScript.isViewPoppup = false;
            }
            else
            {
#if UNITY_ANDROID
                //Application.Quit();
                MobilePlugin.getInstance().ShowExitConfirm(MissionControl.Language["Quit"], MissionControl.Language["Quit_detail"],
                    MissionControl.Language["Ok"], MissionControl.Language["Cancel"]);
#endif
            }
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            print("Reset all data saved by PlayerPrefs");
            PlayerPrefs.DeleteAll();
            VariableSystem.AddDiamond(-(VariableSystem.diamond - 8));
        }
    }

    public void Btn_Moregame_Click()
    {
#if UNITY_ANDROID
        //store android
        Application.OpenURL("https://play.google.com/store/apps/developer?id=SplayGame");
#elif UNITY_IPHONE && !UNITY_EDITOR
        //store iOs
        Application.OpenURL("https://itunes.apple.com/us/artist/duc-nguyen/id909836176");
#endif
        //store Web
        //Application.OpenURL("http://www.windowsphone.com/vi-VN/store/publishers?publisherId=Cachep%2BStudio&appId=1e49cf99-6499-434e-b0d9-aa6ca9656161");
    }

    public void Btn_Feedback_Click()
    {
        string content = VariableSystem.language.Equals("Vietnamese") ? "Hãy góp ý với chúng tôi: " : "Please send us your advise: ";
        Application.OpenURL("mailto:splaygamemoi@gmail.com?subject=Bussiness Farm Feedback&body=" + content);
    }

    public void CallbackLogoScaleFinish()
    {
        GameObject.Find("BackgroundMenu").transform.Find("back1").Find("Xoay").gameObject.SetActive(true);
    }
}
