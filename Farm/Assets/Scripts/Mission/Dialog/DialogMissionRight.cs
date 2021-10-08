using UnityEngine;
using System.Collections;

public class DialogMissionRight : DialogAbs
{

    public static int IdItem;
    int price;
    Transform tableItem;
    Transform btBuy;

    UILabel lbDesciption;
    UILabel lbPrice;

    void Start()
    {
        btBuy = transform.Find("Buy");
        tableItem = transform.Find("Table");
        lbDesciption = transform.Find("Description").GetComponent<UILabel>();
        lbPrice = transform.Find("Buy").Find("Price").GetComponent<UILabel>();
        lbDesciption.text = MissionControl.Language["Description"];
    }

    void Update()
    {
        if (Show)
        {
            //Debug.Log("DANG CHAY");
            switch (IdItem)
            {
                case 1:
                    lbDesciption.text = MissionControl.Language["Description_item_powerup_1"];
                    price = 7;
                    if (!MissionPowerUp.MoreMoney)
                    {
                        btBuy.gameObject.SetActive(true);
                    }
                    break;
                case 2:
                    lbDesciption.text = MissionControl.Language["Description_item_powerup_2"];
                    price = 5;
                    if (!MissionPowerUp.MoreTime)
                    {
                        btBuy.gameObject.SetActive(true);
                    }
                    break;
                case 3:
                    lbDesciption.text = MissionControl.Language["Description_item_powerup_3"];
                    price = 7;
                    if (!MissionPowerUp.DoubleMoney)
                    {
                        btBuy.gameObject.SetActive(true);
                    }
                    break;
                case 4:
                    lbDesciption.text = MissionControl.Language["Description_item_powerup_4"];
                    price = 5;
                    if (!MissionPowerUp.PriceDrop)
                    {
                        btBuy.gameObject.SetActive(true);
                    }
                    break;
                case 5:
                    lbDesciption.text = MissionControl.Language["Description_item_powerup_5"];
                    price = 7;
                    if (!MissionPowerUp.CustomerIncrease)
                    {
                        btBuy.gameObject.SetActive(true);
                    }
                    break;
                case 6:
                    lbDesciption.text = MissionControl.Language["Description_item_powerup_6"];
                    price = 7;
                    if (!MissionPowerUp.FillRateMeter)
                    {
                        btBuy.gameObject.SetActive(true);
                    }
                    break;
                default:
                    price = 0;
                    btBuy.gameObject.SetActive(false);
                    lbDesciption.text = MissionControl.Language["Description"];
                    break;
            }
            lbPrice.text = "" + price;
        }
    }
    public void BuyButton()
    {
        if (VariableSystem.diamond >= price && price != 0)
        {
            VariableSystem.AddDiamond(-price);
            GoogleAnalytics.instance.LogScreen("Buy PowerUpItem: " + IdItem);
            transform.Find("Table").Find("Item" + IdItem).GetComponent<ItemPowerUp>().Price = price;
            transform.Find("Table").Find("Item" + IdItem).GetComponent<ItemPowerUp>().SetBuy();
            IdItem = 0;
        }
        else
        {
            DialogInapp.ShowInapp();
        }
    }

    public override void ShowDialog(DialogAbs.CallBackShowDialog callback = null)
    {
        transform.Find("Logo").Find("name").GetComponent<UILabel>().text = MissionControl.Language["POWER_UP"];
        Show = true;
        LeanTween.scale(gameObject, new Vector3(1, 1, 1f), 0.2f).setEase(LeanTweenType.easeOutBack).setDelay(0.3f).setOnComplete(() =>
        {
            if (callback!= null)
            {
                callback();
            }
        });

        for (int i = 1; i <= 6; i++ )
        {
            transform.Find("Table").Find("Item" + i).GetComponent<ItemPowerUp>().CheckItemBuy();
        }
    }

    public override void HideDialog(DialogAbs.CallBackHideDialog callback = null)
    {
        Show = false;
        LeanTween.scale(gameObject, new Vector3(0, 0 ,0f), 0.1f).setEase(LeanTweenType.easeInBack);
    }
}
