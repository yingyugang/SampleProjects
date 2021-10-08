using UnityEngine;
using System.Collections;
using System;

public class ItemDailyGift : MonoBehaviour {

    UILabel lbDay, lbReward;
    Transform finish, buttonGet, glow;
    int id;

    int[] rewards = { 1, 1, 1, 2, 1, 1, 1, 4, 1, 1, 1, 8 };
	void Awake () {
        lbDay = transform.Find("Day").GetComponent<UILabel>();
        lbReward = transform.Find("Reward").Find("Label").GetComponent<UILabel>();
        id = Convert.ToInt16(gameObject.name);
        lbDay.text = "Day " + id;
        lbReward.text = "x" + rewards[id - 1];
	}
	
    public void SetData(bool isFinish = false)
    {
        finish = transform.Find("Finish");
        buttonGet = transform.Find("ButtonGet");
        finish.gameObject.SetActive(isFinish);
        GetComponent<UITexture>().color = Color.white;
        buttonGet.gameObject.SetActive(!isFinish);
        glow = transform.Find("Glow");
        glow.gameObject.SetActive(!isFinish);
        //Debug.Log("AAA " + !isFinish);
    }

	// Update is called once per frame
	void Update () {
	
	}
   
    public void ButtonGet()
    {
        bool[] dataDailyGift = PlayerPrefsX.GetBoolArray(DialogDailyGift.key_data_daily_gift);
        dataDailyGift[id - 1] = true;
        //Luu lai
        PlayerPrefsX.SetBoolArray(DialogDailyGift.key_data_daily_gift, dataDailyGift);
        SetData(true);
        switch (id)
        {
            case 1:
            default:
                VariableSystem.AddDiamond(1);
                break;  
            case 2://super seed
                DialogShop.BoughtItem[0] = true;
                break;
            case 3://amazing machine
                DialogShop.BoughtItem[2] = true;
                break;
            case 4:
                VariableSystem.AddDiamond(2);
                break;
            case 5://radio
                DialogShop.BoughtItem[4] = true;
                break;
            case 6://save money
                DialogShop.BoughtItem[5] = true;
                break;
            case 7://super hand
                DialogShop.BoughtItem[3] = true;
                break;
            case 8:
                VariableSystem.AddDiamond(4);
                break;
            case 9://fill rate meter
                MissionPowerUp.FillRateMeter = true;
                break;
            case 10://price drop
                MissionPowerUp.PriceDrop = true;
                break;
            case 11://More time
                MissionPowerUp.MoreTime = true;
                break;
            case 12:
                VariableSystem.AddDiamond(8);
                break;
        }
    }
}
