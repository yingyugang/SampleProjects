using UnityEngine;
using System.Collections;
using System.IO;
using Facebook.MiniJSON;
using System;
using System.Collections.Generic;
using System.Xml;
using Assets.Scripts.Common;

public class MissionControl : MonoBehaviour
{
    public static int max_mission;
    public static int countShowDialogLogin = 0;
    public static int countShowRate = 0;

    public Transform DialogConfirm;

    UILabel lbDiamond, lbLife, lbTimeLife;
    //UIButton btnAddHeart;

    Transform dialogMessage;
    Transform dialogSelectFriend;
    Transform dialogLoading;
    Transform loginButton;
    Transform dialogLogin;
    Transform dialogMission;
    Transform dialogSetting;
    Transform dialogAchievement;
    Transform dialogInapp;
    Transform dialogDailyGift;
    Transform dialogSpecialGift;
    Transform dialogRefill;
    Transform dialogInfo;
    Transform dialogEvent;

    public static string IdUserFriends = "";
    bool showLogin;
    //Count message
    float time_count_message;
    UILabel lbCountMessage;
    int countMessage = 0;

    static bool showDialogTryAgain = false;

    public static Dictionary<string, string> Language = null;
    public static Dictionary<string, string> LanguageEN = null;
    public static Dictionary<string, string> LanguageVI = null;
    void Start()
    {
        //VariableSystem.heart = 1;
        GoogleAnalytics.instance.LogScreen("Mission");
        if (String.IsNullOrEmpty(VariableSystem.language))
        {
            VariableSystem.language = "English";
        }
        //VariableSystem.diamond = 100;
        showLogin = false;
        lbCountMessage = transform.Find("Button").Find("ButtonShowMessage").Find("Texture").Find("CountMessage").gameObject.GetComponent<UILabel>();
        dialogSetting = transform.Find("Dialog").Find("DialogSetting");
        dialogMessage = transform.Find("Dialog").Find("DialogMessage");
        dialogSelectFriend = transform.Find("Dialog").Find("DialogSelectFriend");
        dialogLoading = transform.Find("Dialog").Find("DialogLoading");
        dialogLogin = transform.Find("Dialog").Find("DialogLogin");
        loginButton = transform.Find("Button").Find("ButtonLogin");
        dialogMission = transform.Find("Dialog").Find("DialogMission");
        dialogAchievement = transform.Find("Dialog").Find("DialogAchievement");
        dialogInapp = GameObject.Find("DialogInapp").transform;
        dialogDailyGift = transform.Find("Dialog").Find("DialogDailyGift");
        dialogSpecialGift = transform.Find("Dialog").Find("DialogSpecialGift");
        dialogRefill = transform.Find("Dialog").Find("DialogRefill");
        dialogInfo = transform.Find("Dialog").Find("DialogInfo");
        dialogEvent = transform.Find("Dialog").Find("DialogEventFriend");

        lbLife = transform.Find("Button").Find("Life").Find("Icon").Find("Count").GetComponent<UILabel>();
        lbDiamond = transform.Find("Button").Find("Diamond").Find("Count").GetComponent<UILabel>();
        lbTimeLife = transform.Find("Button").Find("Life").Find("Time").GetComponent<UILabel>();
        GameObject common = GameObject.Find("CommonObject");
        if (common != null)
        {
            AudioControl.StopMusic("Nhac nen 2");
            AudioControl.StopMusic("Nhac nen 1");
            GameObject.Destroy(common);
        }


        if (MissionControl.Language == null)
        {
            //Doc xml ngon ngu
            MissionControl.LanguageEN = new Dictionary<string, string>();
            MissionControl.LanguageVI = new Dictionary<string, string>();
            TextAsset xml = Resources.Load<TextAsset>("Mission/Language/EN"); //Read File xml
            XmlDocument xmlDoc = new XmlDocument();
            StringReader reader = new StringReader(xml.text);
            xmlDoc.Load(reader);
            XmlNodeList xmlNodeList = xmlDoc.DocumentElement.ChildNodes;
            reader.Dispose();
            reader.Close();
            foreach (XmlNode node in xmlNodeList)
            {
                MissionControl.LanguageEN.Add(node.Name, node.InnerText);
            }
            //vn
            TextAsset xml1 = Resources.Load<TextAsset>("Mission/Language/VI"); //Read File xml
            XmlDocument xmlDoc1 = new XmlDocument();
            StringReader reader1 = new StringReader(xml1.text);
            xmlDoc1.Load(reader1);
            XmlNodeList xmlNodeList1 = xmlDoc1.DocumentElement.ChildNodes;
            reader1.Dispose();
            reader1.Close();
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
        //Update language button
        transform.Find("Button").Find("ButtonLogin").Find("Label").GetComponent<UILabel>().text = MissionControl.Language["LOGIN"];

        //Show rate
        if (countShowRate >= 7 && !showDialogTryAgain)
        {
            countShowRate = 0;
            transform.Find("Dialog").Find("DialogShare").GetComponent<DialogShare>().ShowDialog();
        }       
        AudioControl.DPlayMusicInstance("Nhac menu", true, true);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            AudioControl.DPlaySound("Click 1");
            if (dialogSetting.gameObject.GetComponent<DialogSetting>().Show
                || dialogMessage.gameObject.GetComponent<DialogMessage>().Show || dialogSelectFriend.gameObject.GetComponent<DialogSelectFriend>().Show
                || dialogLogin.GetComponent<DialogLogin>().Show || dialogAchievement.GetComponent<DialogAchievement>().Show || dialogRefill.GetComponent<DialogRefill>().Show
                || dialogInapp.GetComponent<DialogInapp>().Show || dialogInfo.GetComponent<DialogInfo>().Show || CommonObjectScript.isViewPoppup
                || dialogDailyGift.GetComponent<DialogDailyGift>().Show || dialogSpecialGift.GetComponent<DialogSpecialGift>().Show || dialogEvent.GetComponent<DialogEventFriend>().Show)
            {
                dialogSpecialGift.GetComponent<DialogSpecialGift>().HideDialog();
                dialogSetting.gameObject.GetComponent<DialogSetting>().HideDialogSetting();
                dialogMessage.gameObject.GetComponent<DialogMessage>().HideDialog();
                dialogSelectFriend.gameObject.GetComponent<DialogSelectFriend>().CloseButton();
                dialogAchievement.GetComponent<DialogAchievement>().HideDialog();
                HideLoginDialog();
                dialogRefill.GetComponent<DialogRefill>().HideDialog();
                dialogInapp.GetComponent<DialogInapp>().HideDialog();
                dialogInfo.GetComponent<DialogInfo>().HideDialog();
                dialogDailyGift.GetComponent<DialogDailyGift>().HideDialog();
                dialogEvent.GetComponent<DialogEventFriend>().HideDialog();
            }
            else if (dialogMission.gameObject.GetComponent<DialogMission>().Show)
            {
                Transform dialogShop = transform.Find("Dialog").Find("DialogShop");
                if (dialogShop.GetComponent<DialogShop>().Show)
                {
                    dialogShop.GetComponent<DialogShop>().HideDialog();
                }
                else
                {
                    dialogMission.gameObject.GetComponent<DialogMission>().HideDialogMission();
                }
            }
            else
            {
                //Application.LoadLevel("Menu");
                LoadingScene.ShowLoadingScene("Menu", true);
            }

        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //MissionData.targetCommon.current += 10;
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            dialogDailyGift.GetComponent<DialogDailyGift>().ShowDialog();
        }
       
        //Thoi gian nhan tim       
        if (VariableSystem.heart >= 5)
        {
            VariableSystem.heart = 5;
            lbTimeLife.text = "FULL";
        }
        else
        {
            // Debug.Log("-------------" + timeaddheart + " AudioControl.time_add_heart " + AudioControl.time_add_heart + " DString.GetTimeNow() " + DString.GetTimeNow());
            int totalTime = (int)(DString.GetTimeNow() - AudioControl.time_add_heart);

            if (totalTime > 0)
            {
                //Debug.Log("----------------Thoi gian tu luc thieu tim---------------" + totalTime);
                int tim_cong_them_thua_thoi_gian = (int)(totalTime / AudioControl.max_time_receive_heat);
                int heart_add = tim_cong_them_thua_thoi_gian + 1;
                if (heart_add > 5 - VariableSystem.heart)
                {
                    heart_add = 5 - VariableSystem.heart;
                }
                Debug.Log("---------------------CONG TIM----------------------" + heart_add);
                //VariableSystem.heart += heart_add;
                AudioControl.AddHeart(heart_add);

                //Tru bu thoi gian va hien thi
                int thoi_gian_con_du = (int)totalTime % AudioControl.max_time_receive_heat;//So du chua du 10phut de nhan them tim. so du nay > 0 va < 600
                int thoi_gian_hien_thi = AudioControl.max_time_receive_heat - thoi_gian_con_du;
                lbTimeLife.text = DString.ConvertSecondsToMinute(thoi_gian_hien_thi);

                //Luu lai thoi gian cong tim tiep
                if (VariableSystem.heart < 5)
                {
                    Debug.Log("---------------------Luu thoi gian cong tim tiep----------------------");
                    AudioControl.time_add_heart = (DString.GetTimeNow() + AudioControl.max_time_receive_heat);
                    PlayerPrefs.SetString(AudioControl.key_time_add_heart, "" + AudioControl.time_add_heart);
                }
            }
            else
            {
                int thoi_gian_hien_thi = Math.Abs(totalTime);
                lbTimeLife.text = DString.ConvertSecondsToMinute(thoi_gian_hien_thi);
            }
        }
        if (VariableSystem.heart < 0)
        {
            VariableSystem.heart = 0;
        }
        lbLife.text = "" + VariableSystem.heart;
        lbDiamond.text = "" + VariableSystem.diamond;
        
        if (FB.IsInitialized)
        {
            if (FB.IsLoggedIn)
            {
                time_count_message += Time.deltaTime;
                if (time_count_message > 30)
                {
                    time_count_message = 0;
                    CountMessage();
                    Debug.Log("Count Message");
                }
            }
            if (!showLogin)
            {
                showLogin = true;
                if (FB.IsLoggedIn)
                {
                    HideLoginDialog();
                    CountMessage();
                    loginButton.gameObject.SetActive(false);
                    transform.Find("Button").Find("ButtonShowMessage").gameObject.SetActive(true);
                    transform.Find("Button").Find("ButtonInviteFriend").gameObject.SetActive(true);
                    transform.Find("Button").Find("ButtonHelpFriend").gameObject.SetActive(true);
                }
                else
                {
                    loginButton.gameObject.SetActive(true);
                    if (!dialogLoading.gameObject.activeInHierarchy)
                    {
                        if (countShowDialogLogin > 5 && !showDialogTryAgain)
                        {
                            ShowLoginDialog();
                        }
                    }
                }
            }
        }

        //Show try again
        if (showDialogTryAgain)
        {
            Debug.Log("SHOW TRY AGAIN");
            showDialogTryAgain = false;
            Transform dialogTask = GameObject.Find("DialogTask").transform;
            dialogTask.GetComponent<DialogTask>().RemoveAllItem();
            Debug.Log("READ LEVEL TRY AGAIN" + VariableSystem.mission);
            MissionData.READ_XML(VariableSystem.mission);
            dialogMission.GetComponent<DialogMission>().ShowDialogMision(VariableSystem.mission);
            //--------------------ACHIEVEMENT 3-------------------------
            DialogAchievement.AddDataAchievement(3, 1);
        }

    }

    //void OnGUI()
    //{
        
    //    else
    //    {
    //        GUILayout.Label("Facebook initing... ");
    //    }
    //}

    public void HomeButton()
    {
        LoadingScene.ShowLoadingScene("Menu", true);
        //Application.LoadLevel("Menu");
        AudioControl.DPlaySound("Click 1");
    }

    public void ShowMessageDialogButton()
    {
        AudioControl.DPlaySound("Click 1");
        dialogMessage.gameObject.SetActive(true);
        //LeanTween.scale(dialogMessage.gameObject, new Vector3(1, 1, 1f), 0.3f).setEase(LeanTweenType.easeInOutBack);
        dialogMessage.gameObject.GetComponent<DialogMessage>().ShowDialog();
        dialogLoading.GetComponent<DialogLoading>().ShowLoading();
    }

    public void ShowInviteFriends()
    {
        //LeanTween.scale(dialogSelectFriend.gameObject, new Vector3(1, 1, 1f), 0.3f).setEase(LeanTweenType.easeInOutBack);
        dialogLoading.GetComponent<DialogLoading>().ShowLoading();
        dialogSelectFriend.gameObject.SetActive(true);
        dialogSelectFriend.gameObject.GetComponent<DialogSelectFriend>().ShowDialogInviteFriend();
    }

    public void ShowHelpFriends()
    {

        dialogLoading.GetComponent<DialogLoading>().ShowLoading();
        dialogSelectFriend.gameObject.SetActive(true);
        //LeanTween.scale(dialogSelectFriend.gameObject, new Vector3(1, 1, 1f), 0.3f).setEase(LeanTweenType.easeInOutBack);
        dialogSelectFriend.gameObject.GetComponent<DialogSelectFriend>().ShowDialogHelpFriend();
    }

    public void ShowAskForFriend()
    {
        if (!FB.IsLoggedIn)
        {
            ShowLoginDialog();
            return;
        }
        dialogLoading.GetComponent<DialogLoading>().ShowLoading();
        dialogSelectFriend.gameObject.SetActive(true);
        //LeanTween.scale(dialogSelectFriend.gameObject, new Vector3(1, 1, 1f), 0.3f).setEase(LeanTweenType.easeInOutBack);
        dialogSelectFriend.gameObject.GetComponent<DialogSelectFriend>().ShowDialogAskFriend();
    }

    public void HideLoadingDialog()
    {
        if (dialogMessage.transform.localScale.x < 0.1f &&
            dialogSelectFriend.transform.localScale.x < 0.1f &&
            dialogLogin.transform.localScale.x < 0.1f &&
            dialogMission.transform.localScale.x < 0.1f)
        {
            dialogLoading.GetComponent<DialogLoading>().HideLoading();
            dialogMessage.gameObject.GetComponent<DialogMessage>().HideDialog();
            dialogSelectFriend.gameObject.GetComponent<DialogSelectFriend>().CloseButton();
        }
    }

    public void FriendNonUsingApp()
    {
        {
            //FB.API("me/friends?fields=id,name", Facebook.HttpMethod.GET, Callback);
            FB.API("v2.2/me/invitable_friends?fields=id,name,picture.url", Facebook.HttpMethod.GET, result =>
            {
                Debug.Log(JsonHelper.FormatJson(result.Text));
                var dict = Json.Deserialize(result.Text) as IDictionary;
                List<object> arr = dict["data"] as List<object>;

                for (int i = 0; i < arr.Count; i++)
                {
                    Dictionary<string, object> aa = arr[i] as Dictionary<string, object>;
                    Debug.Log("------------- " + aa["id"]);
                    IdUserFriends += aa["id"] + ",";
                }

            });
        }
    }

    public void InviteNonUser()
    {
        List<object> DanhSachLocJson = null;
        DanhSachLocJson = Facebook.MiniJSON.Json.Deserialize("[\"all\",\"app_users\",\"app_non_users\"]") as List<object>;
        FB.AppRequest(
                "AAAAAAAAAAAAAA",
                IdUserFriends.Split(','),
                DanhSachLocJson,
                null,
                100,
                "DATA",
                "Tieu de",
                result =>
                {
                    Debug.Log(result.Text);
                }
            );
    }

    public void LogoutButton()
    {
        loginButton.gameObject.SetActive(true);
        transform.Find("Button").Find("ButtonShowMessage").gameObject.SetActive(false);
        transform.Find("Button").Find("ButtonInviteFriend").gameObject.SetActive(false);
        transform.Find("Button").Find("ButtonHelpFriend").gameObject.SetActive(false);
    }

    public void LoginButton(bool hideLoading = true)
    {
        //MobilePlugin.getInstance().ShowToast("Click login");
        string error = "";
        //AudioControl.DPlaySound("Click 1");
        try
        {
            if (FB.IsInitialized)
            {
                if (FB.IsLoggedIn)
                {
                    HideLoginDialog();
                    GetFriendsList();
                    loginButton.gameObject.SetActive(false);
                    transform.Find("Button").Find("ButtonShowMessage").gameObject.SetActive(true);
                    transform.Find("Button").Find("ButtonInviteFriend").gameObject.SetActive(true);
                    transform.Find("Button").Find("ButtonHelpFriend").gameObject.SetActive(true);
                }
                else
                {
                    DFB.FBLogin(result =>
                    {
                        //MobilePlugin.getInstance().ShowToast("FB.IsLoggedIn " + FB.IsLoggedIn);
                        if (FB.IsLoggedIn)
                        {
                            loginButton.gameObject.SetActive(false);
                            transform.Find("Button").Find("ButtonShowMessage").gameObject.SetActive(true);
                            transform.Find("Button").Find("ButtonInviteFriend").gameObject.SetActive(true);
                            transform.Find("Button").Find("ButtonHelpFriend").gameObject.SetActive(true);
                            //get friends list
                            GetFriendsList();
                            //count message
                            CountMessage();
                            //Get info user
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
                                            ShowConfirm();
                                            Debug.Log("--------------------catch error StartCoroutine-------------------" + e.Message);
                                            error = "" + e.Message;
                                        }
                                    }
                                });
                            }
                            else
                            {
                                DFB.UserId = PlayerPrefs.GetString(DataCache.FB_ID);
                                DFB.UserName = PlayerPrefs.GetString(DataCache.FB_USER);
                            }
                        }
                        else
                        {
                            ShowConfirm();
                        }
                    });
                }
            }
            else
            {
                DFB.FBInit();
                ShowConfirm();
            }
        }
        catch (Exception e)
        {
            ShowConfirm();
            Debug.Log("--------------------Error Login FB-------------------" + e.Message);
            error = "" + e.Message;
        }
        //MobilePlugin.getInstance().ShowToast("error " + error);
    }

    public void ShowConfirm()
    {
        Transform confirm = Instantiate(DialogConfirm) as Transform;
        confirm.GetComponent<DialogConfirm>().ShowDialogHideCancel(MissionControl.Language["Login"], MissionControl.Language["Check_Network"]);
    }

    public void ShowLoginDialog()
    {
        countShowDialogLogin = 0;
        dialogLoading.GetComponent<DialogLoading>().ShowLoading(false);
        dialogLogin.GetComponent<DialogLogin>().ShowDialog();
    }

    public void HideLoginDialog()
    {
        dialogLogin.GetComponent<DialogLogin>().HideDialog(() => { dialogLoading.GetComponent<DialogLoading>().HideLoading(); });
    }

    public void AddDiamondButton()
    {
        AudioControl.DPlaySound("Click 1");
        //VariableSystem.diamond += 10;
        DialogInapp.ShowInapp();
    }

    public void AddHeartButton()
    {
        AudioControl.DPlaySound("Click 1");
        //VariableSystem.heart += 10;
        if (VariableSystem.heart < 5)
        {
            //ShowAskForFriend();
            dialogRefill.GetComponent<DialogRefill>().ShowDialog();
        }
        else
        {
            Transform confirm = Instantiate(DialogConfirm) as Transform;
            confirm.GetComponent<DialogConfirm>().ShowDialogHideCancel(MissionControl.Language["REFILL"], MissionControl.Language["Heart_full"]);
        }
    }

    public void SettingButton()
    {
        AudioControl.DPlaySound("Click 1");
        dialogLoading.GetComponent<DialogLoading>().ShowLoading(false);
        dialogSetting.gameObject.SetActive(true);
        dialogSetting.gameObject.GetComponent<DialogSetting>().ShowDialogSetting();
    }

    public static void GetFriendsList()
    {
        try
        {
            FB.API("v2.2/me/friends?fields=id,name", Facebook.HttpMethod.GET, result =>
            {
                if (!String.IsNullOrEmpty(result.Error))
                {
                    Debug.Log(result.Error);
                }
                else
                {
                    var dict = Json.Deserialize(result.Text) as IDictionary;
                    if (dict != null && dict["data"] != null)
                    {
                        List<object> arr = dict["data"] as List<object>;
                        for (int i = 0; i < arr.Count; i++)
                        {
                            Dictionary<string, object> aa = arr[i] as Dictionary<string, object>;
                            IdUserFriends += aa["id"] + ",";

                            //don't get current user's data
                            if (i == arr.Count - 1)
                            {
                                IdUserFriends += FB.UserId;
                                //IdUserFriends += "0";
                            }
                        }
                        //Debug.Log("danh sach ban be sau khi login: " + IdUserFriends);
                        //Khi da co danh sach id thi gửi cờ đến MissionItemControl để truy vấn lên server lấy dữ liệu current mision
                        //DataMissionControl.BeginGetCurrentMisson = true;
                        DataMissionControlNew.getFriendlistFinish = true;
                        if (arr.Count > PlayerPrefs.GetInt(DialogEventFriend.key_data_total_friend, 0))
                        {
                            PlayerPrefs.SetInt(DialogEventFriend.key_data_total_friend, arr.Count);
                            DialogEventFriend.hasEvent = true;
                            //Debug.Log("---------------------------" + arr.Count);
                        }
                    }

                }
            });
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
        }
    }

    public void CountMessage()
    {
        if (countMessage > 0)
        {
            lbCountMessage.transform.parent.gameObject.SetActive(false);
        }
        else
        {
            lbCountMessage.transform.parent.gameObject.SetActive(false);
        }
        try
        {
            FB.API("v2.2/me/apprequests?fields=id", Facebook.HttpMethod.GET, result =>
            {
                if (!String.IsNullOrEmpty(result.Error))
                {
                    Debug.Log(result.Error);
                }
                else
                {
                    var dict = Json.Deserialize(result.Text) as IDictionary;
                    if (dict != null && dict["data"] != null)
                    {
                        var data = dict["data"] as List<object>;
                        countMessage = data.Count;
                    }
                    try
                    {
                        lbCountMessage.text = "NEW";
                        if ("Vietnamese".Equals(VariableSystem.language))
                        {
                            lbCountMessage.text = "MỚI";
                        }
                        if (countMessage < 1)
                        {
                            lbCountMessage.transform.parent.gameObject.SetActive(false);
                        }
                        else
                        {
                            lbCountMessage.transform.parent.gameObject.SetActive(true);
                        }
                    }
                    catch (Exception e)
                    {
                        Debug.Log("------Exception-------" + e.Message);
                    }
                }
            });
        }
        catch (Exception e)
        {
            Debug.Log("-----------Exception------------" + e.Message);
        }
    }

    public void ButtonAchievement()
    {
        AudioControl.DPlaySound("Click 1");
        dialogAchievement.GetComponent<DialogAchievement>().ShowDialog();
    }

    public void ButtonEvent()
    {
        AudioControl.DPlaySound("Click 1");
        transform.Find("Dialog").Find("DialogEventFriend").GetComponent<DialogEventFriend>().ShowDialog();
    }

    public void ButtonFreeGem()
    {
        GameObject.Find("Vungle").GetComponent<VungleControl>().ShowVideoAd();
    }

    public static void ShowTryAgain()
    {
        //Application.LoadLevel("Mission");
        LoadingScene.ShowLoadingScene("Mission", true);
        showDialogTryAgain = true;
    }

    public static void ResetAllItem()
    {
        MissionPowerUp.MoreMoney = false;
        MissionPowerUp.MoreTime = false;
        MissionPowerUp.DoubleMoney = false;
        MissionPowerUp.PriceDrop = false;
        MissionPowerUp.CustomerIncrease = false;
        MissionPowerUp.FillRateMeter = false;
        for (int i = 0; i < DialogShop.BoughtItem.Length; i++)
        {
            DialogShop.BoughtItem[i] = false;
        }
    }


    void OnDestroy()
    {
        VariableSystem.SaveData();
        DataCache.SaveAchievementCache();
    }
}
