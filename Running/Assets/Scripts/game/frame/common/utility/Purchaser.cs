using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.Events;
using System.Linq;
using System.Text;

public class Purchaser : MonoBehaviour, IStoreListener
{
	private static Purchaser _instance;
	private static IStoreController m_storeController;
	public UnityAction<bool,string,Product> unityAction;

	public PurchaseLogic purchaseLogic;

	public enum Status
	{
		ready,
		initing,
		paymenting
	}

	public IStoreController storeController { get { return m_storeController; } }

	private static IExtensionProvider _storeExtensionProvider;
	private Status m_purchaseStatus;
	private List<Product> currentProductsCons = new List<Product> ();
    private List<Product> currentProductsNunCons = new List<Product> ();
    private void AddCurentProduct(Product product)
    {
        if (product.definition.type == ProductType.Consumable && !currentProductsCons.Contains(product))
        {
            currentProductsCons.Add(product);
        }
        else if(product.definition.type == ProductType.NonConsumable && !currentProductsNunCons.Contains(product))
        {
            currentProductsNunCons.Add(product);
        }
    }
    private void RemoveCurrentProduct(Product product)
    {
        if (product.definition.type == ProductType.Consumable)
        {
            currentProductsCons.Remove(product);
        }
        else if(product.definition.type == ProductType.NonConsumable)
        {
            currentProductsNunCons.Remove(product);
        }
    }
	private List<ProductInformation> m_productInfos;

	public static List<string> productIDs = new List<string> ();

    private const string androidKey = "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAqrq4h2zvrpZdKWrNmBgZWQBYVuTd8Ga7pgSr//xzn65D87yOkphWWIF0ZoDj8BQ1QU3BFdJi6W4xzN8QKoCLmHBJ4Q8aZpR3ou2ZLR5suC5ekZ1lq5vwf9Uw6lhoaIczedi4mZsiCdoX7XVz7AcML0NDMIOUBTi+oXCmGssGLNXNT9cm16b8SZM19881DEzgtxfnr54Kzp4saX7Nkz8OhBOYgF8AKy/CJUS2g+AOwLaYsvGrFioPnoYCc2hEHp+5K14n7YTV2vSJfUjQ+MpSjcW+jHvbR5MFObJxo4dqoq0dy3NV37VWMPhci7RLKioVN0D0bXjwLLumfn1rNXXC8wIDAQAB";

	public static bool IsInitialized { get { return m_storeController != null && _storeExtensionProvider != null; } }

	public bool Init (List<ProductInformation> productInfos = null, bool isForce = false)
	{
		if (productInfos == null) {
			return false;
		}

		if (!isForce) {
			if (IsInitialized || m_purchaseStatus == Status.initing) {
				return false;
			}
		}

		m_productInfos = productInfos;

		var builder = ConfigurationBuilder.Instance (StandardPurchasingModule.Instance ());
		builder.Configure<IGooglePlayConfiguration> ().SetPublicKey (androidKey);
		foreach (var item in m_productInfos) {
			builder.AddProduct (item.productName, item.productType, item.ids);
		}
		UnityPurchasing.Initialize (this, builder);
		m_purchaseStatus = Status.initing;
		return true;
	}

	public bool BuyProduct (string productID)
	{
		if (IsInitialized) {
			Product product = m_storeController.products.WithID (productID);
			if (product != null && product.availableToPurchase) {
                if (currentProductsCons.Count == 0) {
					m_storeController.InitiatePurchase (product);
					m_purchaseStatus = Status.paymenting;
				} else {
					CheckProduct (product.definition.type);
				}
				Debug.Log ("BuyProduct: true");
				return true;
			} else {
				currentProductsCons.Remove (product);
				CheckProduct ();
				Debug.Log ("BuyProduct: false !!!");
				return false;
			}
		} else {
			Init ();
			Debug.Log ("BuyProduct: false ???");
			return false;
		}
	}

    public void CheckProduct (ProductType pType = ProductType.Consumable)
	{
		if (unityAction == null)
		{
			return;
		}
        if (pType == ProductType.Consumable && currentProductsCons.Count > 0) 
        {
			SendAPI (currentProductsCons [0]);
		}
        else if (pType == ProductType.NonConsumable && currentProductsNunCons.Count > 0)
        {
            if (PlayerPrefs.GetInt("AdUnlock",0) == 1)
            {
                currentProductsNunCons.Clear();
            }
            else
            {
                SendAPI (currentProductsNunCons [0]);
            }
        }
	}

	private void SendAPI (Product product)
	{
		purchaseLogic.complete = () => {
			CheckAPI (product);
		};
		purchaseLogic.currency = LanguageJP.COIN_UNIT;
		ProductInformation productInformation = m_productInfos.Find (result => result.productName == product.definition.id);
		purchaseLogic.product_id = productInformation.productId;
		if (Application.platform == RuntimePlatform.Android) {
            AndroidReceiptModelTest androidReceiptModel = JsonUtility.FromJson<AndroidReceiptModelTest> (product.receipt);
            PayloadAndroidInformation info = JsonUtility.FromJson<PayloadAndroidInformation>(androidReceiptModel.Payload);
            purchaseLogic.signed_data = Convert.ToBase64String(Encoding.UTF8.GetBytes (info.json));
            purchaseLogic.signature = info.signature;
		} else {
			IOSReceiptModel iosReceiptModel = JsonUtility.FromJson<IOSReceiptModel> (product.receipt);
			purchaseLogic.receipt = iosReceiptModel.Payload;
		}
		ProductCSVStructure productCSVStructure = MasterCSV.productCSV.FirstOrDefault (result => product.definition.id == result.name);
		if (productCSVStructure != null) {
			purchaseLogic.purchaseType = (productCSVStructure.is_ad_product == 0) ? PurchaseType.Coin : PurchaseType.AD;
		}
		purchaseLogic.SendAPI ();
	}

	private void CheckAPI (Product product)
	{
		if (unityAction == null) {
			return;
		}
		switch (APIInformation.GetInstance.receipt_status) {
		case 1201:
			unityAction (false, "Verified fail", product);
			break;
        case 1202:
            if (product.definition.type == ProductType.NonConsumable)
            {
                unityAction(true,"Recover Success",product);
            }
            ConfirmPending (product);
            break;
		default:
            unityAction (true, "Success", product);
            ConfirmPending (product);
			break;
		}
	}

	public void ConfirmPending (Product product)
	{
		if (IsInitialized) {
			m_storeController.ConfirmPendingPurchase (product);
            RemoveCurrentProduct (product);
			CheckProduct (product.definition.type);
		}
	}


	public void RestorePurchases ()
	{
		if (IsInitialized) {
			#if UNITY_IOS || UNITY_OSX
			var apple = _storeExtensionProvider.GetExtension<IAppleExtensions> ();
			apple.RestoreTransactions ((result) => {
				// All Complete Callback
			});
            #elif UNITY_ANDROID
            foreach (var item in m_storeController.products.all) {
                if (item.receipt != null && !string.IsNullOrEmpty(item.receipt) && !currentProductsCons.Contains(item)) {
                    AddCurentProduct (item);
                }
            }
            CheckProduct(ProductType.NonConsumable);
			#endif
		}
	}

//    public static List<string> test = new List<string>();
//    void OnGUI()
//    {
//        int i = 0;
//        foreach (var item in test)
//        {
//            GUI.Label(new Rect(0, i*50,50,50),item);
//        }
//
//    }

	#region IStoreListener Imp

	public void OnInitialized (IStoreController controller, IExtensionProvider extensions)
	{
		m_storeController = controller;
		_storeExtensionProvider = extensions;
		m_purchaseStatus = Status.ready;
        #if UNITY_ANDROID
		RestorePurchases ();
        #endif
	}

	public void OnInitializeFailed (InitializationFailureReason error)
	{
		m_purchaseStatus = Status.ready;
	}

	public void OnPurchaseFailed (Product i, PurchaseFailureReason p)
	{
		m_purchaseStatus = Status.ready;
		if (unityAction != null) {
			unityAction (false, p.ToString (), i);
		}
		Debug.Log (p.ToString ());
		// EngineEventManager.GetInstance().DispatchEvent(new EngineEvent(EngineEventType.PAYMENT_FAILED, p.ToString()));
	}

	public PurchaseProcessingResult ProcessPurchase (PurchaseEventArgs e)
	{
		m_purchaseStatus = Status.ready;
		AddCurentProduct (e.purchasedProduct);
		// EngineEventManager.GetInstance().DispatchEvent(new EngineEvent(EngineEventType.PAYMENT_SUCCESS, currentProducts));
        CheckProduct (e.purchasedProduct.definition.type);
		return PurchaseProcessingResult.Pending;
	}
	#endregion
}