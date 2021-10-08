using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Facebook.MiniJSON;
using System;

public class DataMissionControlNew : MonoBehaviour
{
    public Transform DialogConfirm;
    public static string key_update_mission_data_from_server = "key_update_mission_data_from_server";
    public static string key_update_achievement_data_from_server = "key_update_achievement_data_from_server";
    //Test mo map san
    public static int MAX_MISSION = 50;
    public static bool test = false;
    public static bool debug = false;

    //1 co de check khi lay danh sach ban be xong thi se truy van den server
    public static bool getFriendlistFinish = false;
    public Transform Avatar;

    //new 
    int currentMissionServer = 0;
    int currentMissionClient = 1;

    void Start()
    {
        ShowStarMission();
        ShowCurrentMission();
    }

    //Show avatar current misison
    void ShowCurrentMission()
    {
        //Dung try catch de bat loi khi nguoi choi ko con o mission
        try
        {
            RemoveAllAvatar();
            //Read xml file
            DataCache.GetCurrentMission();
            Vector3 currentPos = new Vector3();
            for (int i = 0; i < DataCache.dataCurrentMissionCache.Length; i++)
            {
                int miss = Convert.ToInt16(DataCache.dataCurrentMissionCache[i].Mission);
                string parent = GetParentItemMission(miss);
                Transform missTransform = transform.Find(parent).Find("MissionItem").Find("" + miss);
                if ("Me".Equals(DataCache.dataCurrentMissionCache[i].FB_id))
                {
                    currentMissionClient = miss;
                    //Set avata cua nguoi choi luon o vi tri dang nhap nhay. Tranh truong hop khi mo map avata kodung vi tri
                    missTransform.GetComponent<ItemMission>().AddAvatar(Avatar, DataCache.dataCurrentMissionCache[i].FB_id);
                    missTransform.GetComponent<ItemMission>().SetCurrentMission();
                    currentPos.x = -missTransform.localPosition.x;
                }
                else
                {
                    missTransform.GetComponent<ItemMission>().AddAvatar(Avatar, DataCache.dataCurrentMissionCache[i].FB_id);
                }
            }
            //Debug.Log("-----------------------------------POS CURRENT  " + currentPos);
            if (VariableSystem.mission > 0)
            {
                string parent = GetParentItemMission(VariableSystem.mission);
                currentPos.x = -transform.Find(parent).Find("MissionItem").Find("" + VariableSystem.mission).localPosition.x;
            }
            if (currentPos.x > 0)
            {
                currentPos.x = 0;
            }
            //Debug.Log("AAAAAAAAAAAAAAA " + VariableSystem.mission);

            transform.parent.localPosition = currentPos;
            transform.parent.GetComponent<UIPanel>().clipOffset = new Vector2(-currentPos.x, currentPos.y);
        }
        catch (Exception e)
        {
            Debug.Log("LOI " + e.Message);
        }
    }

    private string GetParentItemMission(int mission)
    {
        string parent = "Background1";
        if (mission > 15 && mission < 31)
        {
            parent = "Background2";
        }
        else if (mission >= 31 && mission < 48)
        {
            parent = "Background3";
        }
        else if (mission >= 48 && mission < 57)
        {
            parent = "Background4";
        }
        else if (mission >= 57 && mission < 67)
        {
            parent = "Background5";
        }
        else if (mission >= 67 && mission < 81)
        {
            parent = "Background6";
        }
        else if (mission >= 81 && mission < 87)
        {
            parent = "Background7";
        }
        else if (mission >= 87)
        {
            parent = "Background8";
        }
        //Debug.Log(parent);
        return parent;
    }

    //Show star mission
    void ShowStarMission()
    {

        //string xmlMissionData = Application.persistentDataPath + "/" + DataCache.XML_Data_Mission_Path;
        //Read xml file
        DataCache.GetMissionDataCache();
        int totalStar = 0;
        int totalScore = 0;
        List<MissionDataSave> missionData = new List<MissionDataSave>();
        int maxmiss = DataCache.dataMissionCache.Length;
        if (maxmiss > MAX_MISSION)
        {
            maxmiss = MAX_MISSION;
        }
        for (int i = 0; i < maxmiss; i++)
        {
            missionData.Add(DataCache.dataMissionCache[i]);
        }
        //Dieu kien de mission ko vuot qua so mission tren map
        MissionControl.max_mission = maxmiss;
        for (int i = 0; i < maxmiss; i++)
        {
            string parent = GetParentItemMission(i + 1);
            totalStar += missionData[i].Star;
            totalScore += (int)missionData[i].Score;
            //Debug.Log("Thong tin misison Mission " + missionData[i].Mission +" Star " +missionData[i].Star);
            //Neu mission duoc mo roi thi moi cap nhat
            if (missionData[i].Open == 1)
            {
                Debug.Log("----------------i " + i + " parent " + parent + " name " + missionData[i].Mission);
                //Dung try catch de bat loi khi nguoi choi ko con o mission
                try
                {
                    transform.Find(parent).Find("MissionItem").Find("" + missionData[i].Mission).GetComponent<ItemMission>().SetData(missionData[i]);
                }
                catch (Exception e)
                {
                    Debug.Log("--------Exception----------" + e.Message);
                }
            }
        }
        //--------------Achievement 1------------------------------------
        DialogAchievement.ReplaceDataAchievement(2, totalStar);
        //--------------Achievement 19------------------------------------
        //Debug.Log("--------------------- TOTAL SCORE " + totalScore);
        DialogAchievement.ReplaceDataAchievement(19, totalScore);

    }

    //Lay current mission server
    void CheckDataMissionServer()
    {
        try
        {
            DialogLoadingFB.ShowFBLoading();
            //Debug.Log("Truy van toi server iduser = " + FB.UserId);
            AudioControl.getMonoBehaviour().StartCoroutine(DHS.GetAllCurrentMission(MissionControl.IdUserFriends, www =>
            {
                Debug.Log("All current mission :" + JsonHelper.FormatJson(www.text));
                bool postToServer = true;
                IDictionary dict = Json.Deserialize(www.text) as IDictionary;
                if (dict != null && dict["data"] != null)
                {
                    List<object> lists = dict["data"] as List<object>;
                    string data_save = "";
                    for (int i = 1; i <= lists.Count; i++)
                    {
                        Dictionary<string, object> info = lists[i - 1] as Dictionary<string, object>;
                        string id = "" + info["fb_id"];
                        string mission = "" + info["mission"];
                        string name = "" + info["name"];
                        if (data_save.Length > 0)
                            data_save += ",";
                        if (id.Equals(FB.UserId))
                        {
                            //Debug.Log("Mission hien tai truoc so sanh " + CurrentMissionFromClient + " mission " + mission);
                            //neu mission tren server > mission o client moi cap nhat mission cua nguoi choi
                            currentMissionServer = Convert.ToInt16(mission);
                            Debug.Log("Current mission on server " + currentMissionServer);
                            if (currentMissionServer > currentMissionClient || PlayerPrefs.GetInt(DataMissionControlNew.key_update_mission_data_from_server, 1) == 0)
                            {
                                //Update new current data  
                                data_save += "Me-" + name + "-" + currentMissionServer;
                                UpdateServerToClient();
                                postToServer = false;
                                if (PlayerPrefs.GetInt(DataMissionControlNew.key_update_mission_data_from_server, 1) == 0)
                                {
                                    Debug.Log("---------------Update lai tu server do chua update day du-------------------");
                                }
                            }
                            else
                            {
                                data_save += "Me-" + name + "-" + currentMissionClient;

                            }
                        }
                        else
                        {
                            data_save += id + "-" + name + "-" + mission;
                        }
                    }
                    //Debug.Log("" + data_save);
                    if (!String.IsNullOrEmpty(data_save))
                    {
                        DataCache.SaveCurrentMission(data_save);
                    }
                }
                Debug.Log("currentMissionServer " + currentMissionServer);
                if (postToServer && currentMissionServer < currentMissionClient)
                {
                    UpdateClientToServer();
                }
                else
                {
                    Debug.Log("Du lieu cu => ko up");
                    if (!(currentMissionServer > currentMissionClient))
                    {
                        DialogLoadingFB.HideFBLoading();
                    }
                }
            }, www =>
            {
                DialogLoadingFB.HideFBLoading();
                Debug.Log("Loi response CheckDataMissionServer");
                if (debug)
                {
                    MobilePlugin.getInstance().ShowToast("Loi response CheckDataMissionServer");
                }
                ShowRetryLoad(() =>
                {
                    Debug.Log("Retry CheckDataMissionServer");
                    CheckDataMissionServer();
                }, () =>
                {

                });
            }));
        }
        catch (Exception e)
        {
            Debug.Log("--------------------catch error StartCoroutine-------------------" + e.Message);
        }
    }

    void UpdateClientToServer()
    {
        Debug.Log("----------------------Dua du lieu moi len server----------------------");
        AudioControl.getMonoBehaviour().StartCoroutine(DHS.PostMeCurrentMission(currentMissionClient));
        string data_mission = "";
        for (int i = 0; i < DataCache.dataMissionCache.Length; i++)
        {
            //Chi gui nhung mission da open len server
            if (DataCache.dataMissionCache[i].Open == 1)
            {
                if (data_mission.Length > 0)
                    data_mission += ",";
                data_mission += DataCache.dataMissionCache[i].Mission + "-" + DataCache.dataMissionCache[i].Score + "-" + DataCache.dataMissionCache[i].Star + "-" + DataCache.dataMissionCache[i].Open;
            }
            DialogLoadingFB.HideFBLoading();
        }
        AudioControl.getMonoBehaviour().StartCoroutine(DHS.PostMeInfoMissionUpdate(FB.UserId, "" + data_mission));
        DataCache.SaveAchievementCache(true);
    }

    void UpdateServerToClient()
    {
        try
        {
            DialogLoadingFB.ShowFBLoading();
            PlayerPrefs.SetInt(DataMissionControlNew.key_update_mission_data_from_server, 0);
            PlayerPrefs.SetInt(DataMissionControlNew.key_update_achievement_data_from_server, 0);

            //Them du lieu tam thoi truoc(de phong truong hop ko truy van dc)
            for (int i = 0; i < currentMissionServer; i++)
            {
                DataCache.UpdateMissionScore(0, 0, i + 1, 1);
            }
            //Lay du lieu nguoi dung
            try
            {
                AudioControl.getMonoBehaviour().StartCoroutine(DHS.GetMeInfo(FB.UserId, www =>
                {
                    Debug.Log(JsonHelper.FormatJson(www.text));
                    IDictionary dict = Json.Deserialize(www.text) as IDictionary;
                    if (dict != null && dict["data"] != null)
                    {
                        List<object> lists = dict["data"] as List<object>;
                        if (lists.Count > 0)
                        {
                            Dictionary<string, object> info = lists[0] as Dictionary<string, object>;
                            int diamond = Convert.ToInt32(info["diamond"]);
                            string achievement = Convert.ToString(info["achievement"]);
                            DataCache.RestoreUserData(diamond, achievement);
                            //GameObject.Find("DialogAchievement").GetComponent<DialogAchievement>().CountAchievementFinish();
                            string data_mission = Convert.ToString(info["mission_data"]);
                            //Phan tich du lieu va chi lay nhung du lieu co score, star > o client
                            string[] dataMission = data_mission.Split(',');
                            for (int i = 0; i < dataMission.Length; i++)
                            {
                                string[] info_mission = dataMission[i].Split('-');
                                string mission = info_mission[0];
                                string score = info_mission[1];
                                string star = info_mission[2];
                                string open = info_mission[3];
                                DataCache.UpdateMissionScore(Convert.ToInt32(score), Convert.ToInt16(star), Convert.ToInt16(mission), Convert.ToInt16(open));
                            }
                            DataCache.SaveMissionDataCache();
                            PlayerPrefs.SetInt(DataMissionControlNew.key_update_mission_data_from_server, 1);
                            //Debug.Log("------ Update ------ ");
                            ShowStarMission();
                            ShowCurrentMission();
                            DialogLoadingFB.HideFBLoading();
                        }
                    }
                }, www =>
                {
                    DialogLoadingFB.HideFBLoading();
                    ShowRetryLoad(() =>
                    {
                        UpdateServerToClient();
                    });
                    if (debug)
                    {
                        MobilePlugin.getInstance().ShowToast("Loi response CheckDataMissionServer");
                    }
                }));
            }
            catch (Exception e)
            {
                Debug.Log("--------------------catch error StartCoroutine-------------------" + e.Message);
            }
        }
        catch (Exception e)
        {
            Debug.Log("--------------------catch error StartCoroutine-------------------" + e.Message);
        }
    }

    //remove all avatar  on each item mission
    private void RemoveAllAvatar()
    {
        for (int i = 0; i < MAX_MISSION; i++)
        {
            string parent = GetParentItemMission(i + 1);
            //Debug.Log("i " + i + " parent " + parent);
            try
            {
                transform.Find(parent).Find("MissionItem").Find("" + (i + 1)).GetComponent<ItemMission>().RemoveAllAvatar();
            }
            catch (Exception e)
            {
                Debug.Log("--------Exception----------" + e.Message);
            }
        }
    }

    void Update()
    {
        if (getFriendlistFinish)
        {
            getFriendlistFinish = false;
            Debug.Log("----------------------Bat dau truy van den server----------------------");
            CheckDataMissionServer();
        }
    }


    public void ShowRetryLoad(Action ok, Action cancel = null)
    {
        try
        {
            if (cancel == null)
            {
                cancel = () =>
                {
                    Debug.Log("Cancel => tro ve menu");
                    LoadingScene.ShowLoadingScene("Menu", true);
                };
            }
            Transform confirm = Instantiate(DialogConfirm) as Transform;
            confirm.GetComponent<DialogConfirm>().ShowDialog(MissionControl.Language["Try_Again"], MissionControl.Language["Try_again_reload"], ok, cancel);
        }
        catch (Exception e)
        {
            Debug.Log("--------------------catch error StartCoroutine-------------------" + e.Message);
        }
    }
}
