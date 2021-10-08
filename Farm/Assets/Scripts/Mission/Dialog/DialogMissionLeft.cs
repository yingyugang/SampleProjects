using UnityEngine;
using System.Collections;
using Facebook.MiniJSON;
using System.Collections.Generic;
using System;

public class DialogMissionLeft : MonoBehaviour
{

    public void SetData(int level)
    {
      // Debug.Log("aaaaaaaaaaaaaaaaaa " + MissionControl.Language["Rank"]);
        transform.Find("Rank").GetComponent<UILabel>().text = MissionControl.Language["Rank"];
        transform.Find("ButtonLogin").Find("Label").GetComponent<UILabel>().text = MissionControl.Language["LOGIN"];
        if (FB.IsLoggedIn)
        {
            transform.Find("ButtonLogin").gameObject.SetActive(false);
            getRank(true, level);
        }
    }

    public void Hide()
    {
        for (int i = 1; i <= 3; i++)
        {
            Transform rank = transform.Find("ItemRank" + i);
            rank.gameObject.SetActive(false);
        }
    }

    void getRank(bool retry, int level)
    {
        try
        {
            StartCoroutine(DHS.GetRankingMission(level, MissionControl.IdUserFriends, www =>
            {
                Debug.Log(JsonHelper.FormatJson(www.text));
                IDictionary dict = Json.Deserialize(www.text) as IDictionary;
                if (dict != null && dict["data"] != null)
                {
                    List<object> lists = dict["data"] as List<object>;
                    for (int i = 1; i <= lists.Count; i++)
                    {
                        Dictionary<string, object> info = lists[i - 1] as Dictionary<string, object>;
                        Transform rank = transform.Find("ItemRank" + i);
                        rank.Find("Name").GetComponent<UILabel>().text = "" + info["name"];
                        rank.Find("Money").GetComponent<UILabel>().text = DString.ConvertToMoneyString("" + info["score"]);
                        rank.gameObject.GetComponent<Avatar>().SetData(this, "" + info["fb_id"]);
                        rank.gameObject.SetActive(true);
                    }
                }
            }, www =>
            {
                if (retry)
                {
                    Debug.Log("---------------get rank error => retry ---------------");
                    getRank(false,level);
                }
            }));
        }
        catch (Exception e)
        {
            Debug.Log("--------------------catch error StartCoroutine-------------------" + e.Message);
        }  
    }
}
