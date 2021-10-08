using UnityEngine;
using System.Collections;
using System;

public class DString
{
    public readonly static string encrypt_key = "12345678901234567890123456789012";
    public readonly static string SEND_LIFE = "Here's a life. Use it well";
    public readonly static string ASK_LIFE = "Help me a life";
    public readonly static string OBJID = "777355942311431";
    public readonly static string DATA_SEND_LIFE = "SendLife";
    public readonly static string DATA_ASK_LIFE = "AskLife";

    public static string ConvertString(long number)
    {
        string str = "" + number;
        int i = 0;
        while (true)
        {
            i++;
            if (str.Length - 3 * i - (i - 1) < 1)
            {
                break;
            }
            str = str.Insert(str.Length - 3 * i - (i - 1), ",");
        }
        return str;
    }

    public static string ConvertToMoneyString(string str)
    {
        int i = 0;
        while (true)
        {
            i++;
            // Debug.Log("String " + str +" -- " + str.Length);
            if (str.Length - 3 * i - (i - 1) < 1)
            {
                break;
            }
            str = str.Insert(str.Length - 3 * i - (i - 1), ",");
        }
        return str;
    }

    public static string ConvertSecondsToMinute(float seconds)
    {
        int minute = (int)(seconds / 60);
        int second = (int)(seconds % 60);
        string str_minute = "" + minute;
        string str_second = "" + second;
        if (minute < 10)
        {
            str_minute = "0" + minute;
        }
        if (second < 10)
        {
            str_second = "0" + second;
        }
        string time = str_minute + ":" + str_second;
        return time;
    }

    public static long GetTimeNow()
    {
        DateTime date1 = DateTime.UtcNow;
        DateTime date2 = DateTime.Parse("01/01/1970");
        TimeSpan ts = date1 - date2;
        int totalSeconds = Convert.ToInt32(ts.TotalSeconds);
        return totalSeconds;
    }
}
