using UnityEngine;
using System.Collections;

public class DialogShop : DialogAbs
{
    public Transform SubDiamond;
    public Transform ItemShop;

    string[] en_name = { "Super Seeds", "Grown Faster", "Amazing Machine", "Super Hands", "Radio", "Save Money", "Sale-off", "Automatic",
                       "Machine Guide", "Star Bonus", "Dr. X", "Mechanic", "Take Care", "Best Ads", "Percent +", "15% Off", "Weather Gifts"};
    // string[] vi_name = { "", "Weather Gift" };
    string[] en_detail = { "Decrease 20% the time to plant crop", "Decrease 10% the time to raise cattle", "Decrease 10% the time of producing machine", "Increase 40% the staff's speed",
                         "Increase 20% the time for waiting of customer","Decrease 10% the price of seed, cattle",
                         "Decrease 10% the price of machine","Automatic harvest","Complete the upgrading of all machines immediately",
                         "Add 1 star for each customers who bought product in shop","Automatic heal the crop or cattle",
                         "Automatic fix the machine", "Automatic care all staffs when they have strange expression (lazy, sleepy or sick)",
                         "Increase 10% ability to attract customers by ads",
                         "Increase 10% accurately ability for specialist method and research company method", "Discount 10% the price in Supermarket",
                         "Add 50% to customer's bill if they get weather gifts"};
    string[] vi_detail = { "Giảm 20% thời gian trồng cây", "Giảm 20% thời gian nuôi con vật", "Giảm 10% thời gian sản xuất của máy móc",
                          "Tăng 40% tốc độ phục vụ của nhân viên",
                         "Tăng thêm 20% thời gian chờ đợi của khách","Giảm 10% giá bán của các loại giống, vật nuôi",
                         "Giảm 10% giá bán của các loại máy","Tự động thu hoạch", "Hoàn thành việc nâng cấp toàn bộ máy móc ngay lập tức",
                         "Tăng thêm một sao cho mỗi khách hàng khi họ được phục vụ","Phòng ngừa bệnh tật 100 % cho cây trồng, vật nuôi",
                         "Máy móc không bị hỏng trong quá trình sản xuất", "Tăng 100% sự tập trung, thể lực, tinh thần cho nhân viên",
                         "Tăng thêm 10% khả năng thu hút khách hàng của các loại hình quảng cáo",
                         "Tăng thêm 10% khả năng chính xác của phương pháp hỏi chuyên gia và công ty nghiên cứu", "Giảm giá 10% các mặt hàng trong Super Market",
                         "Cộng thêm 50% tổng hóa đơn của một khách hàng khi nhận được tặng quà"};
    int[] prices = { 2, 2, 2, 3, 3, 2, 2, 2, 2, 3, 3, 3, 3, 3, 3, 2, 2 };
    string[] detail;
    public static int IdItem;
    public static bool[] BoughtItem;

    Transform main;
    Transform bgBlack;
    Transform btBuy;
    int price;
    Transform gridView;
    UILabel lbDesciption;
    UILabel lbPrice;
    UILabel lbIconName;

    void Awake()
    {
        IdItem = 1;
        BoughtItem = new bool[17];
        ResetBoughtItem();
        main = transform.Find("Main");
        bgBlack = transform.Find("BgBlack");
        gridView = main.Find("Scroll View").Find("Grid");
        //gridView.gameObject.SetActive(false);
        lbDesciption = main.Find("Description").GetComponent<UILabel>();
        lbPrice = main.Find("Buy").Find("Price").GetComponent<UILabel>();
        lbIconName = main.Find("BgRight").Find("Name").GetComponent<UILabel>();
        btBuy = main.Find("Buy");

        for (int i = 1; i <= 17; i++)
        {
            Transform item = Instantiate(ItemShop) as Transform;
            item.name = "" + i;
            item.GetComponent<ItemShop>().Id = i;
            item.parent = gridView;
            item.localScale = new Vector3(1, 1, 1);
        }
    }

    void Update()
    {
        if (Show && IdItem != 0)
        {
            lbDesciption.text = detail[IdItem - 1];
            price = prices[IdItem - 1];
            lbPrice.text = "" + price;
            lbIconName.text = en_name[IdItem - 1];
        }
    }

    void ResetBoughtItem()
    {
        for (int i = 0; i < BoughtItem.Length; i++)
        {
            BoughtItem[i] = false;
        }
    }

    public void BuyButton()
    {
        AudioControl.DPlaySound("Click 1");
        if (VariableSystem.diamond >= price && price != 0)
        {
            VariableSystem.AddDiamond(-price);
            GoogleAnalytics.instance.LogScreen("Buy ShopItem: " + vi_detail[IdItem - 1]);
            gridView.Find("" + IdItem).GetComponent<ItemShop>().Price = price;
            gridView.Find("" + IdItem).GetComponent<ItemShop>().SetBuy();

            Transform diamond = Instantiate(SubDiamond) as Transform;
            diamond.parent = this.transform;
            diamond.position = btBuy.position;
            diamond.localScale = new Vector3(1, 1, 1);
            diamond.Find("Count").GetComponent<UILabel>().text = "-" + price;
            LeanTween.moveLocalY(diamond.gameObject, diamond.position.y - 300, 0.5f).setEase(LeanTweenType.easeOutSine).setOnComplete(delegate()
            {
                Destroy(diamond.gameObject);
            });
        }
        else
        {
            Debug.Log("Thieu tien roi");
            DialogInapp.ShowInapp();
        }
    }

    public void AddIconItem()
    {
        Transform old = main.Find("BgRight").Find("Icon").Find("IconItem");
        if (old != null)
        {
            Destroy(old.gameObject);
        }
        Transform icon = Instantiate(ItemShop) as Transform;
        icon.GetComponent<ItemShop>().Id = IdItem;
        icon.GetComponent<ItemShop>().isIcon = true;
        icon.parent = main.Find("BgRight").Find("Icon");
        icon.name = "IconItem";
        icon.localPosition = new Vector3(0, 0, 0);
        icon.localScale = new Vector3(1, 1, 1);
    }

    public override void ShowDialog(DialogAbs.CallBackShowDialog callback = null)
    {
        bgBlack.gameObject.SetActive(true);
        main.gameObject.SetActive(true);
        if (("Vietnamese").Equals(VariableSystem.language))
        {
            detail = vi_detail;
        }
        else
        {
            detail = en_detail;
        }
        Show = true;
        lbDesciption.text = MissionControl.Language["Description"];
        main.Find("BgLeft").Find("Logo").Find("Label").GetComponent<UILabel>().text = MissionControl.Language["ITEM"];

        LeanTween.scale(main.gameObject, new Vector3(1, 1, 1f), 0.3f).setEase(LeanTweenType.easeOutBack).setOnComplete(() =>
        {
            main.Find("Scroll View").gameObject.SetActive(true);
            //gridView.gameObject.SetActive(true);
        });
        IdItem = 1;
        //gridView.GetComponent<UIGrid>().Reposition();
        AddIconItem();
        //gridView.parent.GetComponent<UIScrollView>().

        //Check buy item - Khi nhan thuong o daily gift
        for (int i = 0; i < BoughtItem.Length; i++)
        {
            if (BoughtItem[i])
            {
                gridView.Find("" + (i + 1)).GetComponent<ItemShop>().SetBuy();
            }
        }
    }

    public override void HideDialog(DialogAbs.CallBackHideDialog callback = null)
    {

        //gridView.gameObject.SetActive(false);
        Show = false;
        LeanTween.scale(main.gameObject, new Vector3(0.0f, 0.0f, 0f), 0.3f).setEase(LeanTweenType.easeInBack).setOnComplete(() =>
        {
            transform.parent.Find("DialogMission").gameObject.SetActive(true);
            main.gameObject.SetActive(false);
            main.Find("Scroll View").gameObject.SetActive(false);
            bgBlack.gameObject.SetActive(false);
        });
    }
}
