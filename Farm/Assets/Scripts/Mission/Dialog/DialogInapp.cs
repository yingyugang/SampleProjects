using UnityEngine;
using System.Collections;
using OnePF;
using System.Collections.Generic;

public class DialogInapp : DialogAbs
{

    float[] prices = { 0.99f, 4.99f, 9.99f, 19.99f, 29.99f, 49.99f };
    int[] quatity = { 20, 120, 250, 450, 650, 1100 };
    Transform main, black;
    string tag = "Inapp";
    void Awake()
    {
        transform.parent = GameObject.Find("AudioControl").transform;
        main = transform.Find("Main");
        black = transform.Find("Black");
        for (int i = 0; i < prices.Length; i++)
        {
            transform.Find("Main").Find("package" + (i + 1)).Find("quatity").GetComponent<UILabel>().text = "" + quatity[i];
            transform.Find("Main").Find("package" + (i + 1)).Find("buy").Find("Label").GetComponent<UILabel>().text = "$ " + prices[i];
        }

        this.gameObject.tag = tag;
        GameObject[] dialogs = GameObject.FindGameObjectsWithTag(tag);
        if (dialogs.Length > 1)
        {
            for (int i = 1; i < dialogs.Length; i++)
            {
                Destroy(dialogs[i]);
            }
        }
        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        if (Show)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                HideDialog();
            }
        }
    }
    void OnApplicationPause(bool pauseStatus)
    {
        if (!pauseStatus)
        {
            HideDialog();
        }
    }

    public void ButtonBuy(Transform button)
    {
        OpenIAB.purchaseProduct(button.parent.name);
    }

    public override void ShowDialog(DialogAbs.CallBackShowDialog callback = null)
    {
        CommonObjectScript.isViewPoppup = true;
        if (!Show)
        {
            Show = true;
            main.gameObject.SetActive(true);
            black.gameObject.SetActive(true);
            main.Find("Title").GetComponent<UILabel>().text = MissionControl.Language["RESOURCE"];
            main.Find("package1").GetComponent<UILabel>().text = MissionControl.Language["package1"];
            main.Find("package2").GetComponent<UILabel>().text = MissionControl.Language["package2"];
            main.Find("package3").GetComponent<UILabel>().text = MissionControl.Language["package3"];
            main.Find("package4").GetComponent<UILabel>().text = MissionControl.Language["package4"];
            main.Find("package5").GetComponent<UILabel>().text = MissionControl.Language["package5"];
            main.Find("package6").GetComponent<UILabel>().text = MissionControl.Language["package6"];

            LeanTween.scale(main.gameObject, Vector3.one, 0.3f).setUseEstimatedTime(true).setEase(LeanTweenType.easeOutBack).setOnComplete(() =>
                {
                    //cuongvm
                    Time.timeScale = 0;
                });
        }
    }

    public override void HideDialog(DialogAbs.CallBackHideDialog callback = null)
    {
        if (Show)
        {
            //cuongvm
            Time.timeScale = 1;
            //print("aaaaaaaaaaa");
            LeanTween.scale(main.gameObject, Vector3.zero, 0.3f).setUseEstimatedTime(true).setEase(LeanTweenType.easeInBack).setOnComplete(() =>
            {
                CommonObjectScript.isViewPoppup = false;
                black.gameObject.SetActive(false);
                Show = false;
                main.gameObject.SetActive(false);
            });
        }
    }

    //INAPP
    const string SKU = "sku";
    string _label = "";
    bool _isInitialized = false;
    Inventory _inventory = null;

    private void OnEnable()
    {
        // Listen to all events for illustration purposes
        OpenIABEventManager.billingSupportedEvent += billingSupportedEvent;
        OpenIABEventManager.billingNotSupportedEvent += billingNotSupportedEvent;
        OpenIABEventManager.queryInventorySucceededEvent += queryInventorySucceededEvent;
        OpenIABEventManager.queryInventoryFailedEvent += queryInventoryFailedEvent;
        OpenIABEventManager.purchaseSucceededEvent += purchaseSucceededEvent;
        OpenIABEventManager.purchaseFailedEvent += purchaseFailedEvent;
        OpenIABEventManager.consumePurchaseSucceededEvent += consumePurchaseSucceededEvent;
        OpenIABEventManager.consumePurchaseFailedEvent += consumePurchaseFailedEvent;
    }
    private void OnDisable()
    {
        // Remove all event handlers
        OpenIABEventManager.billingSupportedEvent -= billingSupportedEvent;
        OpenIABEventManager.billingNotSupportedEvent -= billingNotSupportedEvent;
        OpenIABEventManager.queryInventorySucceededEvent -= queryInventorySucceededEvent;
        OpenIABEventManager.queryInventoryFailedEvent -= queryInventoryFailedEvent;
        OpenIABEventManager.purchaseSucceededEvent -= purchaseSucceededEvent;
        OpenIABEventManager.purchaseFailedEvent -= purchaseFailedEvent;
        OpenIABEventManager.consumePurchaseSucceededEvent -= consumePurchaseSucceededEvent;
        OpenIABEventManager.consumePurchaseFailedEvent -= consumePurchaseFailedEvent;
    }
    private void Start()
    {
        // Map skus for different stores       
        OpenIAB.mapSku(InAppController.PACKAGE_1, OpenIAB_Android.STORE_GOOGLE, InAppController.PACKAGE_1);
        OpenIAB.mapSku(InAppController.PACKAGE_2, OpenIAB_Android.STORE_GOOGLE, InAppController.PACKAGE_2);
        OpenIAB.mapSku(InAppController.PACKAGE_3, OpenIAB_Android.STORE_GOOGLE, InAppController.PACKAGE_3);
        OpenIAB.mapSku(InAppController.PACKAGE_4, OpenIAB_Android.STORE_GOOGLE, InAppController.PACKAGE_4);
        OpenIAB.mapSku(InAppController.PACKAGE_5, OpenIAB_Android.STORE_GOOGLE, InAppController.PACKAGE_5);
        OpenIAB.mapSku(InAppController.PACKAGE_6, OpenIAB_Android.STORE_GOOGLE, InAppController.PACKAGE_6);

        //OpenIAB.mapSku(SKU, OpenIAB_iOS.STORE, "sku");
        //OpenIAB.mapSku(SKU, OpenIAB_WP8.STORE, "ammo");

        //Khoi tao inapp
        // Application public key
        if (!_isInitialized)
        {
            var publicKey = "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAlokcbayM/caK81xQ42EPWpiJlPe+NAQ4DYA+Z+amaCP4cLANurjyBYbqF5KisMPzb3XG6TjXHMU3uBMPXHgj5LSLFFTsZDTSZeUNZ22nX9o7U/EwHdxr0ofjC/zZAnzLFQxs8U+Nh5zk7/5xDnmoWdhvuGMxWo4i8lk/2rYWfnYJ/+uXPYK9Z7usTE3eL5fj33JYrPGE3friJ03/l3t3YY7zyR9OoIKCXaXtCzmNHX1kRozn7FpsNec3Ne4qoFDAnWnb219b7Cao9rWjbUZ5XJEqcGwCBZCRwZEw2Nl2li/Mk2txogHPGLBI3RWWEWIcBsVbYFb95bb8AyOB92eyywIDAQAB";
            var options = new Options();
            options.checkInventoryTimeoutMs = Options.INVENTORY_CHECK_TIMEOUT_MS * 2;
            options.discoveryTimeoutMs = Options.DISCOVER_TIMEOUT_MS * 2;
            options.checkInventory = false;
            options.verifyMode = OptionsVerifyMode.VERIFY_SKIP;
            options.prefferedStoreNames = new string[] { OpenIAB_Android.STORE_GOOGLE };
            options.availableStoreNames = new string[] { OpenIAB_Android.STORE_GOOGLE };
            options.storeKeys = new Dictionary<string, string> { { OpenIAB_Android.STORE_GOOGLE, publicKey } };
            options.storeSearchStrategy = SearchStrategy.INSTALLER_THEN_BEST_FIT;

            // Transmit options and start the service
            OpenIAB.init(options);
        }
    }
    private void billingSupportedEvent()
    {
        _isInitialized = true;
        Debug.Log("billingSupportedEvent");
    }
    private void billingNotSupportedEvent(string error)
    {
        Debug.Log("billingNotSupportedEvent: " + error);
    }
    private void queryInventorySucceededEvent(Inventory inventory)
    {
        Debug.Log("queryInventorySucceededEvent: " + inventory);
        if (inventory != null)
        {
            _label = inventory.ToString();

            List<Purchase> prods = inventory.GetAllPurchases();

            foreach (Purchase p in prods) OpenIAB.consumeProduct(p);
        }
    }
    private void queryInventoryFailedEvent(string error)
    {
        Debug.Log("queryInventoryFailedEvent: " + error);
        _label = error;
    }
    private void purchaseSucceededEvent(Purchase purchase)
    {
        Debug.Log("purchaseSucceededEvent: " + purchase);
        _label = "PURCHASED:" + purchase.ToString();
        int diamond = 10;
        switch (purchase.Sku)
        {
            case InAppController.PACKAGE_1:
                diamond = quatity[0];
                break;
            case InAppController.PACKAGE_2:
                diamond = quatity[1];
                break;
            case InAppController.PACKAGE_3:
                diamond = quatity[2];
                break;
            case InAppController.PACKAGE_4:
                diamond = quatity[3];
                break;
            case InAppController.PACKAGE_5:
                diamond = quatity[4];
                break;
            case InAppController.PACKAGE_6:
                diamond = quatity[5];
                break;
            default:
                break;
        }
        Debug.Log("------------------------purchase.Sku " + purchase.Sku + " diamond " + diamond);
        GoogleAnalytics.instance.LogScreen("Buy Inapp: " + diamond + " diamonds!");
        VariableSystem.AddDiamond(diamond);
        //Consume purchase
        OpenIAB.consumeProduct(purchase);
#if UNITY_ANDROID
        MobilePlugin.getInstance().ShowToast("Purchase success! You have got " + diamond);
#endif
    }
    private void purchaseFailedEvent(int errorCode, string errorMessage)
    {
        Debug.Log("purchaseFailedEvent: " + errorMessage);
        _label = "Purchase Failed: " + errorMessage;
    }
    private void consumePurchaseSucceededEvent(Purchase purchase)
    {
        Debug.Log("consumePurchaseSucceededEvent: " + purchase);
        _label = "CONSUMED: " + purchase.ToString();
    }
    private void consumePurchaseFailedEvent(string error)
    {
        Debug.Log("consumePurchaseFailedEvent: " + error);
        _label = "Consume Failed: " + error;
    }

    private void OnGUI()
    {
        // Android specific buttons
        //#if UNITY_ANDROID
        //if (GUI.Button(new Rect(200, 0, 200, 50), "Test Purchase"))
        //{
        //    OpenIAB.purchaseProduct("15kgolds");
        //}

        //if (GUI.Button(new Rect(200, 50, 200, 50), "Test Consume"))
        //{
        //    if (_inventory != null && _inventory.HasPurchase("android.test.purchased"))
        //        OpenIAB.consumeProduct(_inventory.GetPurchase("android.test.purchased"));
        //}

        //if (GUI.Button(new Rect(200, 100, 200, 50), "Test Item Unavailable"))
        //{
        //    OpenIAB.purchaseProduct("25kgolds");
        //}

        //if (GUI.Button(new Rect(200, 150, 200, 50), "Test Purchase Canceled"))
        //{
        //    OpenIAB.purchaseProduct("35kgolds");
        //}
        //if (!_isInitialized)
        //{
        //    if (GUI.Button(new Rect(200, 200, 200, 50), "Init IAB"))
        //    {
        //        var publicKey = "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAtGcaz5PTYPvG0TCu/tTk52+SK3TQOPWIq4DsFtxY8JeGqvKL2jhNLy0NeKebg5G7Ox+pd9PQOov/Dcffuzb6391lnOWm/vDaiiuUVfkS7tK0AUtCeL0MqxAR3YaHacAlaSPJcoMNxYhfjMjnzWQBlgVeOQiu7puvTrXGjBCQUzzGU6iqHRaULJ5+Gqi6gVbab1RNNn41cGXsmXtNCp/hhct5z2vP4YvhtcFy+JgLsGJzV3KZ6Ts2Q8ZQ73uR+6csIqDlJFUwYT7x521Y2iMJu36LCaX1BtOTF4XmzRfUlgkvEVahftIxX6HihTh3xZEGptvuFaXp518hxloR5xVo3wIDAQAB";
        //        var options = new Options();
        //        options.checkInventoryTimeoutMs = Options.INVENTORY_CHECK_TIMEOUT_MS * 2;
        //        options.discoveryTimeoutMs = Options.DISCOVER_TIMEOUT_MS * 2;
        //        options.checkInventory = false;
        //        options.verifyMode = OptionsVerifyMode.VERIFY_SKIP;
        //        options.prefferedStoreNames = new string[] { OpenIAB_Android.STORE_GOOGLE };
        //        options.availableStoreNames = new string[] { OpenIAB_Android.STORE_GOOGLE };
        //        options.storeKeys = new Dictionary<string, string> { { OpenIAB_Android.STORE_GOOGLE, publicKey } };
        //        options.storeSearchStrategy = SearchStrategy.INSTALLER_THEN_BEST_FIT;

        //        // Transmit options and start the service
        //        OpenIAB.init(options);
        //    }
        //}

        //GUI.Label(new Rect(100, 100, 200, 20), _label);
        //#endif
        //Debug.Log("AAAAAAAAAAA");
    }

    public static void ShowInapp()
    {
        GameObject.FindWithTag("Inapp").GetComponent<DialogInapp>().ShowDialog();
    }
}
