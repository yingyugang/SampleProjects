using UnityEngine;
using System.Collections;

public class ItemPowerUp : MonoBehaviour
{

    public Transform SubDiamond;
    public int Id;
    public int Price;
    bool isBought;

    Transform select;

    void Start()
    {
        select = transform.Find("Select");
    }

    void Update()
    {
        if (DialogMissionRight.IdItem == Id)
        {
            if (!isBought)
            {
                select.gameObject.SetActive(true);
            }
        }
        else
        {
            select.gameObject.SetActive(false);
        }
    }

    public void ButtonSelect()
    {
        if (!isBought)
        {
            select.gameObject.SetActive(true);
            transform.parent.parent.Find("Buy").gameObject.SetActive(true);
        }
        else
        {
            transform.parent.parent.Find("Buy").gameObject.SetActive(false);
        }
        DialogMissionRight.IdItem = Id;
    }

    public void SetBuy()
    {
        transform.parent.parent.Find("Buy").gameObject.SetActive(false);
        Transform diamond = Instantiate(SubDiamond) as Transform;
        diamond.parent = this.transform;
        diamond.position = transform.position;
        diamond.localScale = new Vector3(1, 1, 1);
        diamond.Find("Count").GetComponent<UILabel>().text = "-" + Price;
        LeanTween.moveLocalY(diamond.gameObject, diamond.position.y - 60, 0.5f).setEase(LeanTweenType.easeOutSine).setOnComplete(delegate()
        {
            Destroy(diamond.gameObject);
        });
        transform.Find("Bought").gameObject.SetActive(true);
        //gameObject.GetComponent<UIButton>().enabled = false;
        isBought = true;
        switch (Id)
        {
            case 1:
                MissionPowerUp.MoreMoney = true;
                break;
            case 2:
                MissionPowerUp.MoreTime = true;
                break;
            case 3:
                MissionPowerUp.DoubleMoney = true;
                break;
            case 4:
                MissionPowerUp.PriceDrop = true;
                break;
            case 5:
                MissionPowerUp.CustomerIncrease = true;
                break;
            case 6:
                MissionPowerUp.FillRateMeter = true;
                break;
            default:
                break;
        }
    }

    //ham de check xem item da co chua - goi trong luc hien thu dilogMissionRight
    public void CheckItemBuy()
    {
        switch (Id)
        {
            case 1:
                if (MissionPowerUp.MoreMoney)
                {
                    transform.Find("Bought").gameObject.SetActive(true);
                    isBought = true;
                }
                break;
            case 2:
                if (MissionPowerUp.MoreTime)
                {
                    transform.Find("Bought").gameObject.SetActive(true);
                    isBought = true;
                }
                break;
            case 3:
                if (MissionPowerUp.DoubleMoney)
                {
                    transform.Find("Bought").gameObject.SetActive(true);
                    isBought = true;
                }
                break;
            case 4:
                if (MissionPowerUp.PriceDrop)
                {
                    transform.Find("Bought").gameObject.SetActive(true);
                    isBought = true;
                }
                break;
            case 5:
                if (MissionPowerUp.CustomerIncrease)
                {
                    transform.Find("Bought").gameObject.SetActive(true);
                    isBought = true;
                }
                break;
            case 6:
                if (MissionPowerUp.FillRateMeter)
                {
                    transform.Find("Bought").gameObject.SetActive(true);
                    isBought = true;
                }
                break;
            default:
                break;
        }
    }

}
