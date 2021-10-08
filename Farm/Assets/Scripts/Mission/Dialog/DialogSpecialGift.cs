using UnityEngine;
using System.Collections;
using System;

public class DialogSpecialGift : DialogAbs
{
    static bool showGift = false;//show gift
    DateTime timer, oldtimer, timer82;
    public UILabel text, info, txt_Diamond;
    Transform bgBlack, bgMain;
    bool isAvailable;
    // Use this for initialization
    void Start()
    {
        bgMain = transform.Find("Main");
        bgBlack = transform.Find("Black");
        //get older get Gift
        string[] tempStr = PlayerPrefs.GetString("SpecicalGift_Weeken", "2-2-2015").Split('-');

        print(PlayerPrefs.GetString("SpecicalGift_Weeken"));

        oldtimer = new DateTime(Convert.ToInt16(tempStr[2]), Convert.ToInt16(tempStr[1]), Convert.ToInt16(tempStr[0]));
        timer82 = new DateTime(2015, 2, 9);

        timer = DateTime.Now;
        if (timer.DayOfWeek.Equals(DayOfWeek.Saturday) || timer.DayOfWeek.Equals(DayOfWeek.Sunday)) //nếu cuối tuần
        {
            print("Weeken " + timer.ToString());
            if (timer.CompareTo(oldtimer) > 0)//chưa nhận quà của tuần đó thì show cho đến khi nào nhận thì thôi
            {
                #region Show popup nhận quà
                isAvailable = true;
                ShowDialog();
                #endregion
            }
        }
        else //ngày thường thì báo sự kiện với xác suất 40%
        {
            if (!showGift)//nếu chưa show thì phải show
            {
                if (UnityEngine.Random.Range(0, 1250) % 100 < 30)
                {
                    isAvailable = false;
                    ShowDialog();
                }
                else showGift = true;
            }
        }
    }

    public void getGift()
    {
        if (timer.CompareTo(timer82) < 0)
            VariableSystem.AddDiamond(50);//add kim cương
        else VariableSystem.AddDiamond(10);//add kim cương

        if (timer.DayOfWeek.Equals(DayOfWeek.Saturday))
            PlayerPrefs.SetString("SpecicalGift_Weeken", (timer.Day + 2) + "-" + timer.Month + "-" + timer.Year);
        else
            PlayerPrefs.SetString("SpecicalGift_Weeken", (timer.Day + 1) + "-" + timer.Month + "-" + timer.Year);
        showGift = true;
        HideDialog();
    }

    public override void ShowDialog(DialogAbs.CallBackShowDialog callback = null)
    {
        if (!Show)
        {
            bgMain = transform.Find("Main");
            bgBlack = transform.Find("Black");
            transform.Find("Main").Find("Bg1").GetComponent<UITexture>().mainTexture = Resources.Load<Texture>("Mission/SpecialGiftWeeken/event-" + (VariableSystem.language.Equals("Vietnamese") ? "vie" : "eng"));

            Show = true;
            bgMain.gameObject.SetActive(true);
            bgBlack.gameObject.SetActive(true);

            if (timer.CompareTo(timer82) < 0)
                txt_Diamond.text = "50";
            else txt_Diamond.text = "10";

            if (!isAvailable) text.transform.parent.gameObject.SetActive(false);
            if (VariableSystem.language.Equals("Vietnamese"))
            {
                text.text = "Nhận";
                if (timer.CompareTo(timer82) < 0)
                    info.text = "*Thời gian sự kiện: Hết 8/2/2015  *Mỗi tài khoản chỉ nhận quà một lần";
                else info.text = "";
                txt_Diamond.transform.localPosition = new Vector3(30, 80);
            }
            else
            {
                text.text = "Get";
                if (timer.CompareTo(timer82) < 0)
                    info.text = "*Event duration: Until Feb.8  *Each user can only get  one reward";
                else info.text = "";
                txt_Diamond.transform.localPosition = new Vector3(0, 80);
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
            if (!isAvailable) showGift = true;
        });
    }
}
