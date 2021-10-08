using UnityEngine;
using System.Collections;
using System;

public class ItemShop : MonoBehaviour
{

    public bool isIcon;
    public int Id;
    public int Price;
    bool isBought;
    Transform select;
    Transform dialogShopMain;

    void Start()
    {
        select = transform.Find("Select");
        dialogShopMain = transform.parent.parent.parent;
        if (!isIcon)
        {
            select.gameObject.SetActive(false);
        }
        else
        {
            GetComponent<UIButton>().enabled = false;
        }
        // Id = Convert.ToInt16(this.name);
        string num = "" + Id;
        if (Id < 10)
        {
            num = "0" + Id;
        }
        GetComponent<UITexture>().mainTexture = Resources.Load<Texture>("Mission/item shop/item" + num);
        transform.Find("Bought").GetComponent<UITexture>().mainTexture = Resources.Load<Texture>("Mission/item shop/tick");
        transform.Find("Select").GetComponent<UITexture>().mainTexture = Resources.Load<Texture>("Mission/item shop/khung-chon");
        if (Id == 1 && !isIcon)
        {
            ButtonSelect();
        }
    }

    void Update()
    {
        if (!isIcon)
        {
            if (DialogShop.IdItem == Id)
            {
                select.gameObject.SetActive(true);
            }
            else
            {
                select.gameObject.SetActive(false);
            }
        }
    }

    public void ButtonSelect()
    {
        if (dialogShopMain != null)
        {
            dialogShopMain = transform.parent.parent.parent;
        }
        if (!isBought)
        {
            dialogShopMain.Find("Buy").gameObject.SetActive(true);
        }
        else
        {
            dialogShopMain.Find("Buy").gameObject.SetActive(false);
        }
        DialogShop.IdItem = Id;
        AddItemInfo();

    }

    void AddItemInfo()
    {
        if (dialogShopMain != null)
        {
            dialogShopMain = transform.parent.parent.parent;
        }
        dialogShopMain.parent.GetComponent<DialogShop>().AddIconItem();
    }

    public void SetBuy()
    {
        if (dialogShopMain != null)
        {
            dialogShopMain = transform.parent.parent.parent;
        }
        dialogShopMain.Find("Buy").gameObject.SetActive(false);
        transform.Find("Bought").gameObject.SetActive(true);
        //gameObject.GetComponent<UIButton>().enabled = false;
        isBought = true;
        DialogShop.BoughtItem[Id - 1] = true;
    }
}
