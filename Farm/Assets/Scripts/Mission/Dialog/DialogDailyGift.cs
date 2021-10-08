using UnityEngine;
using System.Collections;
using System;

public class DialogDailyGift : DialogAbs
{
    public static bool hasGift;
    string key_start_day = "current_day";
    string key_start_year = "current_year";
    public static string key_data_daily_gift = "daily_gift_data";
    Transform bgBlack, bgMain;
    UIGrid grid;

    void Start()
    {
        hasGift = false;
        bgMain = transform.Find("Main");
        bgBlack = transform.Find("Black");
        grid = bgMain.Find("Scroll View").Find("Grid").GetComponent<UIGrid>();

        //Debug.Log(DateTime.Today.DayOfYear + "------------------");
        //Check ngay bat dau choi trong he thong neu chua co thi luu ngay hien tai vao data
        if (!PlayerPrefs.HasKey(key_start_day))
        {
            PlayerPrefs.SetInt(key_start_day, DateTime.Today.DayOfYear);
            PlayerPrefs.SetInt(key_start_year, DateTime.Today.Year);
        }
        //Lay ngay ban dau da luu trong he thong
        int day_start = PlayerPrefs.GetInt(key_start_day);
        int year_start = PlayerPrefs.GetInt(key_start_year);
        //Lay ngay hien tai trong nam
        int day_current = DateTime.Today.DayOfYear;
        int year_current = DateTime.Today.Year;
        int day_open_gift = 1;
        //So sanh nam 
        if (year_current > year_start)
        {
            //Neu nam hien tai > nam bat dau => mo het dailygift
            day_open_gift = 12;
        }
        else if (year_current == year_start)
        {
            day_open_gift = day_current - day_start + 1;
        }
        if (day_open_gift > 12)
        {
            day_open_gift = 12;
        }
        //Check data cua daily gift da co chua, neu chua se cho vao da ta
        if (!PlayerPrefs.HasKey(key_data_daily_gift))
        {
            PlayerPrefsX.SetBoolArray(key_data_daily_gift, new bool[12]);
        }
        bool[] dataDailyGift = PlayerPrefsX.GetBoolArray(key_data_daily_gift);
        for (int i = 0; i < day_open_gift; i++)
        {
            grid.GetChild(i).GetComponent<ItemDailyGift>().SetData(dataDailyGift[i]);
            if (dataDailyGift[i] == false)
            {
                hasGift = true;
            }
        }
        if (DialogDailyGift.hasGift && LoadingStartMenu.showGift)
        {
            Debug.Log("SHOW gift ");
            LoadingStartMenu.showGift = false;
            ShowDialog();
        }
    }

    void Update()
    {

    }

    public override void ShowDialog(DialogAbs.CallBackShowDialog callback = null)
    {
        if (!Show)
        {
            Show = true;
            bgMain = transform.Find("Main");
            bgBlack = transform.Find("Black");
            bgMain.gameObject.SetActive(true);
            bgBlack.gameObject.SetActive(true);
            bgMain.Find("Logo").Find("Title").GetComponent<UILabel>().text = MissionControl.Language["DAILY_GIFT"];
            for (int i = 1; i <= grid.GetChildList().Count; i++)
            {
                grid.GetChild(i - 1).Find("Day").GetComponent<UILabel>().text = MissionControl.Language["Day"] + " " + i;
                grid.GetChild(i - 1).Find("ButtonGet").Find("Label").GetComponent<UILabel>().text = MissionControl.Language["Get"];
            }
            LeanTween.scale(bgMain.gameObject, new Vector3(1, 1, 1), 0.4f).setUseEstimatedTime(true).setEase(LeanTweenType.easeOutBack).setUseEstimatedTime(true); ;
        }
    }

    public override void HideDialog(DialogAbs.CallBackHideDialog callback = null)
    {
        LeanTween.scale(bgMain.gameObject, new Vector3(0.0f, 0.0f, 0), 0.4f).setEase(LeanTweenType.easeInBack).setUseEstimatedTime(true).setOnComplete(() =>
        {
            bgMain.gameObject.SetActive(false);
            bgBlack.gameObject.SetActive(false);
            Show = false;
        });
    }

}
