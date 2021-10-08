using UnityEngine;
using System.Collections;
using Facebook.MiniJSON;

public class DHS
{
    static int TimeOut = 5;
    private string secretKey = "mySecretKey"; // Edit this value and make sure it's the same as the one stored on the server
    static string hostName = "http://gamefarm.sunkhoai.com/";
    static string addMeURL = hostName + "add_user.php?";
    static string addScoreURL = hostName + "post_score_mission.php?";
    static string highscoreURL = hostName + "get_score_mission.php?";
    static string getLevelURL = hostName + "get_current_mission.php?";
    static string postLevelURL = hostName + "post_current_mission.php?";
    static string getMeInfoURL = hostName + "get_user_info.php?";

    public delegate void WebCallback(WWW www);

    public static IEnumerator PostMeInfo(string fb_id, string name, string locate, string last_name, WebCallback callbackSuccess = null, WebCallback callbackError = null)
    {
        string post_url = addMeURL + "fb_id=" + WWW.EscapeURL("" + fb_id) + "&name=" + WWW.EscapeURL(name) + "&locate=" + WWW.EscapeURL(locate) + "&last_name=" + WWW.EscapeURL(last_name);
        WWW hs_get = new WWW(post_url);
        float tempTime = 0;
        while (!hs_get.isDone && hs_get.error == null && tempTime < TimeOut)
        {
            tempTime += Time.deltaTime;
            //Debug.Log("tempTime...." + tempTime);
            yield return 0;
        }
        if (hs_get.error != null || tempTime >= TimeOut)
        {
            if (callbackError == null)
            {
                if (DataMissionControlNew.test)
                {
                    if (tempTime >= TimeOut)
                    {
                        Debug.LogError("---ERROR RESPONE---: Timeout");
                    }
                    else
                    {
                        Debug.LogError("---ERROR RESPONE---: " + hs_get.error);
                    }
                }
            }
            else
            {
                callbackError(hs_get);
            }
        }
        else
        {
            if (callbackSuccess == null)
            {
                Debug.Log(JsonHelper.FormatJson(hs_get.text));
            }
            else
            {
                callbackSuccess(hs_get);
            }
        }
        //hs_get.Dispose();
    }

    public static IEnumerator PostMeInfoUpdate(string fb_id, string diamond, string achievement, string daily_gift, WebCallback callbackSuccess = null, WebCallback callbackError = null)
    {
        //string hash = Md5Sum(name + score + secretKey);
        //string post_url = addScoreURL + "name=" + WWW.EscapeURL(name) + "&score=" + score + "&hash=" + hash;
        string post_url = addMeURL + "fb_id=" + WWW.EscapeURL("" + fb_id) + "&diamond=" + WWW.EscapeURL(diamond) + "&achievement=" + WWW.EscapeURL(achievement) + "&tag=update1"; ;
        WWW hs_get = new WWW(post_url);
        float tempTime = 0;
        while (!hs_get.isDone && hs_get.error == null && tempTime < TimeOut)
        {
            tempTime += Time.deltaTime;
            //Debug.Log("tempTime...." + tempTime);
            yield return 0;
        }
        if (hs_get.error != null || tempTime >= TimeOut)
        {
            if (callbackError == null)
            {
                if (DataMissionControlNew.test)
                    if (tempTime >= TimeOut)
                    {
                        Debug.LogError("---ERROR RESPONE---: Timeout");
                    }
                    else
                    {
                        Debug.LogError("---ERROR RESPONE---: " + hs_get.error);
                    }
            }
            else
            {
                callbackError(hs_get);
            }
        }
        else
        {
            if (callbackSuccess == null)
            {
                if (DataMissionControlNew.test)
                    Debug.Log(JsonHelper.FormatJson(hs_get.text));
            }
            else
            {
                callbackSuccess(hs_get);
            }
        }
        //hs_get.Dispose();
    }
    public static IEnumerator PostMeInfoMissionUpdate(string fb_id, string mission_data, WebCallback callbackSuccess = null, WebCallback callbackError = null)
    {
        string post_url = addMeURL + "fb_id=" + WWW.EscapeURL("" + fb_id) + "&mission_data=" + WWW.EscapeURL(mission_data) + "&tag=update2"; ;
        WWW hs_get = new WWW(post_url);
        float tempTime = 0;
        while (!hs_get.isDone && hs_get.error == null && tempTime < TimeOut)
        {
            tempTime += Time.deltaTime;
            //Debug.Log("tempTime...." + tempTime);
            yield return 0;
        }
        if (hs_get.error != null || tempTime >= TimeOut)
        {
            if (callbackError == null)
            {
                if (DataMissionControlNew.test)
                    if (tempTime >= TimeOut)
                    {
                        Debug.LogError("---ERROR RESPONE---: Timeout");
                    }
                    else
                    {
                        Debug.LogError("---ERROR RESPONE---: " + hs_get.error);
                    }
            }
            else
            {
                callbackError(hs_get);
            }
        }
        else
        {
            if (callbackSuccess == null)
            {
                if (DataMissionControlNew.test)
                    Debug.Log(JsonHelper.FormatJson(hs_get.text));
            }
            else
            {
                callbackSuccess(hs_get);
            }
        }
        //hs_get.Dispose();
    }
    // remember to use StartCoroutine when calling this function!
    //public static IEnumerator PostMeScoreMission(int mission, string fb_id, string star, string score, WebCallback callbackSuccess = null, WebCallback callbackError = null)
    //{
    //    Debug.Log("Submit data to server mission " + mission + " score " + score + " star  " + star);
    //    //string hash = Md5Sum(name + score + secretKey);
    //    //string post_url = addScoreURL + "name=" + WWW.EscapeURL(name) + "&score=" + score + "&hash=" + hash;
    //    string post_url = addScoreURL + "mission=" + WWW.EscapeURL("" + mission) + "&fb_id=" + WWW.EscapeURL(fb_id) + "&score=" + WWW.EscapeURL(score) + "&star=" + WWW.EscapeURL(star);
    //    WWW hs_post = new WWW(post_url);
    //    yield return hs_post; // Wait until the download is done
    //    if (hs_post.error != null)
    //    {
    //        if (callbackError == null)
    //            Debug.Log("There was an error posting the high score: " + hs_post.error);
    //        else
    //            callbackError(hs_post);
    //    }
    //    else
    //    {
    //        if (callbackSuccess == null)
    //        {
    //            Debug.Log("Post score " + hs_post.text);
    //        }
    //        else
    //        {
    //            callbackSuccess(hs_post);
    //        }
    //    }
    //}

    // Get the scores from the MySQL DB to display in a GUIText.
    // remember to use StartCoroutine when calling this function!
    //public static IEnumerator GetScores(int mission,string fb_ids = "", WebCallback callbackSuccess = null, WebCallback callbackError = null)
    //{
    //    WWW hs_get = new WWW(highscoreURL + "mission=" + WWW.EscapeURL("" + mission) + "&fb_ids=" + WWW.EscapeURL(fb_ids));
    //    yield return hs_get;
    //    if (hs_get.error != null)
    //    {
    //        if (callbackError == null)
    //            Debug.Log("There was an error getting the high score: " + hs_get.error);
    //        else
    //            callbackError(hs_get);
    //    }
    //    else
    //    {
    //        if (callbackSuccess == null)
    //        {
    //            Debug.Log(JsonHelper.FormatJson(hs_get.text));
    //        }
    //        else
    //        {
    //            callbackSuccess(hs_get);
    //        }
    //    }

    //}

    public static IEnumerator GetRankingMission(int mission, string fb_ids = "", WebCallback callbackSuccess = null, WebCallback callbackError = null)
    {
        WWW hs_get = new WWW(highscoreURL + "mission=" + WWW.EscapeURL("" + mission) + "&fb_ids=" + WWW.EscapeURL(fb_ids));
        float tempTime = 0;
        while (!hs_get.isDone && hs_get.error == null && tempTime < TimeOut)
        {
            tempTime += Time.deltaTime;
            //Debug.Log("tempTime...." + tempTime);
            yield return 0;
        }
        if (hs_get.error != null || tempTime >= TimeOut)
        {
            if (callbackError == null)
            {
                if (DataMissionControlNew.test)
                    if (tempTime >= TimeOut)
                    {
                        Debug.LogError("---ERROR RESPONE---: Timeout");
                    }
                    else
                    {
                        Debug.LogError("---ERROR RESPONE---: " + hs_get.error);
                    }
            }
            else
            {
                callbackError(hs_get);
            }
        }
        else
        {
            if (callbackSuccess == null)
            {
                Debug.Log(JsonHelper.FormatJson(hs_get.text));
            }
            else
            {
                callbackSuccess(hs_get);
            }
        }
        //hs_get.Dispose();
    }

    public static IEnumerator PostMeCurrentMission(int mission, WebCallback callbackSuccess = null, WebCallback callbackError = null)
    {
        if (!string.IsNullOrEmpty(FB.UserId))
        {
            WWW hs_get = new WWW(postLevelURL + "mission=" + WWW.EscapeURL("" + mission) + "&fb_id=" + WWW.EscapeURL(FB.UserId));
            float tempTime = 0;
            while (!hs_get.isDone && hs_get.error == null && tempTime < TimeOut)
            {
                tempTime += Time.deltaTime;
                //Debug.Log("tempTime...." + tempTime);
                yield return 0;
            }
            if (hs_get.error != null || tempTime >= TimeOut)
            {
                if (callbackError == null)
                {
                    if (DataMissionControlNew.test)
                        if (tempTime >= TimeOut)
                        {
                            Debug.LogError("---ERROR RESPONE---: Timeout");
                        }
                        else
                        {
                            Debug.LogError("---ERROR RESPONE---: " + hs_get.error);
                        }
                }
                else
                {
                    callbackError(hs_get);
                }
            }
            else
            {
                if (callbackSuccess == null)
                {
                    Debug.Log(JsonHelper.FormatJson(hs_get.text));
                }
                else
                {
                    callbackSuccess(hs_get);
                }
            }
            //hs_get.Dispose();
        }
        else
        {
            if (DataMissionControlNew.test)
                Debug.LogError("---ERROR---: UserId");
        }
    }

    public static IEnumerator GetAllCurrentMission(string fb_ids, WebCallback callbackSuccess = null, WebCallback callbackError = null)
    {
        //Debug.Log("DAnh sach ban be lay current mission " + fb_ids);
        WWW hs_get = new WWW(getLevelURL + "fb_ids=" + WWW.EscapeURL("" + fb_ids));
        float tempTime = 0;
        while (!hs_get.isDone && hs_get.error == null && tempTime < TimeOut)
        {
            tempTime += Time.deltaTime;
            //Debug.Log("tempTime...." + tempTime);
            yield return 0;
        }
        if (hs_get.error != null || tempTime >= TimeOut)
        {
            if (callbackError == null)
            {
                if (DataMissionControlNew.test)
                    if (tempTime >= TimeOut)
                    {
                        Debug.LogError("---ERROR RESPONE---: Timeout");
                    }
                    else
                    {
                        Debug.LogError("---ERROR RESPONE---: " + hs_get.error);
                    }
            }
            else
            {
                callbackError(hs_get);
            }
        }
        else
        {
            if (callbackSuccess == null)
            {
                Debug.Log(JsonHelper.FormatJson(hs_get.text));
            }
            else
            {
                callbackSuccess(hs_get);
            }
        }
        //hs_get.Dispose();
    }

    public static IEnumerator GetMeInfo(string fb_id, WebCallback callbackSuccess = null, WebCallback callbackError = null)
    {
        WWW hs_get = new WWW(getMeInfoURL + "fb_id=" + WWW.EscapeURL("" + fb_id));
        float tempTime = 0;
        while (!hs_get.isDone && hs_get.error == null && tempTime < TimeOut)
        {
            tempTime += Time.deltaTime;
            //Debug.Log("tempTime...." + tempTime);
            yield return 0;
        }
        if (hs_get.error != null || tempTime >= TimeOut)
        {
            if (callbackError == null)
            {
                if (DataMissionControlNew.test)
                    if (tempTime >= TimeOut)
                    {
                        Debug.LogError("---ERROR RESPONE---: Timeout");
                    }
                    else
                    {
                        Debug.LogError("---ERROR RESPONE---: " + hs_get.error);
                    }
            }
            else
            {
                callbackError(hs_get);
            }
        }
        else
        {
            if (callbackSuccess == null)
            {
                Debug.Log(JsonHelper.FormatJson(hs_get.text));
            }
            else
            {
                callbackSuccess(hs_get);
            }
        }
       // hs_get.Dispose();
    }

    public static string Md5Sum(string strToEncrypt)
    {
        System.Text.UTF8Encoding ue = new System.Text.UTF8Encoding();
        byte[] bytes = ue.GetBytes(strToEncrypt);
        // encrypt bytes
        System.Security.Cryptography.MD5CryptoServiceProvider md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
        byte[] hashBytes = md5.ComputeHash(bytes);
        // Convert the encrypted bytes back to a string (base 16)
        string hashString = "";

        for (int i = 0; i < hashBytes.Length; i++)
        {
            hashString += System.Convert.ToString(hashBytes[i], 16).PadLeft(2, '0');
        }
        return hashString.PadLeft(32, '0');
    }
}