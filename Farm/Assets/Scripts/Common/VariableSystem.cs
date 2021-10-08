using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class VariableSystem
{
    //Đây là nơi đặt các biến chung cho toàn bộ hệ thống game: thời gian, tiền bạc, ....
    public static string language; // Hỗ trợ 2 loại ngôn ngữ English - Vietnamese
    public static int heart; // đơn vị trái tim trong game
    public static int diamond; // đơn vị kim cương trong game để mua trái tim.
    public static int mission; // đang ở mission mấy bắt đầu từ mission 1
    public static bool isNeedUpdateValueDiamond;
    public static List<bool> guidedLevels;
    // Use this for initialization
    public static void Start()
    {
        language = PlayerPrefs.GetString("language", "English");
        heart = PlayerPrefs.GetInt("heart", 5);
        diamond = PlayerPrefs.GetInt("diamond", 8);
        mission = 0;
        isNeedUpdateValueDiamond = true;
        //Screen.SetResolution(1280, 720, true);
        //Screen.sleepTimeout = SleepTimeout.NeverSleep;

        char[] tempSplit = new char[] { ';' };
        string[] temp = PlayerPrefs.GetString("guidedLevels", "").Split(tempSplit, StringSplitOptions.RemoveEmptyEntries);
        guidedLevels = new List<bool>();
        for (int i = 0; i < temp.Length; i++) guidedLevels.Add(Convert.ToBoolean(temp[i]));
    }
    public static void AddDiamond(int value)
    {
        if (diamond + value < 0)
        {
            Debug.LogError("Trừ nhiều tiền quá, không chấp nhận được. Cần check lại ngay !!!!");
            return;
        }
        diamond += value;
        isNeedUpdateValueDiamond = true;
        if (value < 0)
        {
            DialogAchievement.AddDataAchievement(4, 1);
            DialogAchievement.AddDataAchievement(5, -value);
        }
        PlayerPrefs.SetInt("diamond", diamond);
    }
    public static void SaveData()
    {
        PlayerPrefs.SetString("language", language);
        PlayerPrefs.SetInt("heart", heart);
        PlayerPrefs.SetInt("diamond", diamond);
    }
    public static void SaveGuide()
    {
        String tempDatasave = "";
        for (int i = 0; i < guidedLevels.Count; i++) tempDatasave += guidedLevels[i].ToString() + ";";
        PlayerPrefs.SetString("guidedLevels", tempDatasave);
    }
}
