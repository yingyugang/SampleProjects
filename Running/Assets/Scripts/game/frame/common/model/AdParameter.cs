using System;

[Serializable]
public class AdParameter
{
    //评价画面出现概率
    public int review_pro;
    //广告画面出现概率
    //TODO show_ad_pro Don't need anymore
    //public int show_ad_pro;
    //评价画面显示文字
    public string review_txt;
    //Zucks出现概率
    public int ads_zucks_pro;
    //Nend出现概率
    public int ads_nend_pro;
    //IMobile Btm出现概率
    public int ads_imobile_pro;
    //AdColony出现概率
    public int ads_colony_pro;
    //Maio出現確率
    public int ads_maio_pro;
    //IMobile FC出現確率
    public int ads_imobile_fc_pro;

    //Maioアイテム出現確率
    public int item_ads_maio_pro;
    //AdColonyアイテム出現確率
    public int item_ads_colony_pro;

    public static AdParameter GetInstance{ get; set; }

    public AdParameter()
    {
        GetInstance = this;
    }
}
