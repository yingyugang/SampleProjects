using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ItemMissionComplete : MonoBehaviour
{
    public Image imgIconGame;
    public Text  txtInfoItem;
    public Image imgMoney;
    public Image imgMark;
    public Sprite[] lstImgReward;

    private const int SILVER_ID = 1;
    private const int GOLD_ID = 2;

    public void SetInfo(Sprite iconGame, string infotext, int rewardId = 1)
    {
        imgIconGame.sprite = iconGame;
        txtInfoItem.text = infotext;
        //if (iconMoney != null)
        //{
        //    imgMoney.sprite = iconMoney;
        //}

        if (rewardId > GOLD_ID || rewardId < SILVER_ID)
        {
            rewardId = SILVER_ID;
        }
        imgMoney.sprite = lstImgReward[rewardId - 1];
    }
}
