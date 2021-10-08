using UnityEngine;
using System.Collections;

public class ContainerItemProductController : MonoBehaviour
{

    public int IDProduct;
    public int costProduct;

    public GameObject panelFrame;
    private GameObject objectItem;
    private GameObject objectItemClone;

    private GameObject ContainerCoinEfectPrefabs;
    private GameObject ContainerCoinEfectClone;
    AudioControl audioControl;
    void OnEnable()
    {
        DestroyProducPre();
    }
    void Start()
    {
        audioControl = GameObject.Find("AudioControl").GetComponent<AudioControl>();
        ContainerCoinEfectPrefabs = (GameObject)Resources.Load("Town/PrefabsConmon/ContainerCoinEfect");
        objectItem = (GameObject)Resources.Load("Town/PrefabsProduct/ProductEfectItem");
    }


    void OnClick()
    {
        audioControl.PlaySound("Chon sp");
        if (CommonObjectScript.dollar >= costProduct)
        {
            CreateContainerCoinEfect();
            CreateObjectItem();
            AddProduct();
            AddCommonObject(-costProduct, 0);
        }
        else
        {
            panelFrame.transform.parent.parent.GetComponent<MaketController>().ButtonCloseClick();
            GameObject.Find("CommonObject").GetComponent<CommonObjectScript>().ChangeDolar(costProduct - CommonObjectScript.dollar);
        }
    }
    void CreateContainerCoinEfect()
    {
        ContainerCoinEfectClone = (GameObject)Instantiate(ContainerCoinEfectPrefabs, transform.position, transform.rotation);
        ContainerCoinEfectClone.GetComponent<CoinEfectController>().timeDelay = 1f;
        ContainerCoinEfectClone.GetComponent<CoinEfectController>().costProduct = costProduct;
        ContainerCoinEfectClone.transform.parent = transform.parent;
        ContainerCoinEfectClone.name = "coin";
    }
    void CreateObjectItem()
    {
        objectItemClone = (GameObject)Instantiate(objectItem,
            new Vector3(transform.position.x, transform.position.y, transform.position.z), transform.rotation);
        objectItemClone.GetComponent<ProductEfectController>().IDProduct = IDProduct;
        objectItemClone.transform.parent = panelFrame.transform;
        objectItemClone.transform.localScale = new Vector3(1f, 1f, 1f);
        objectItemClone.name = "product";
    }
    void AddProduct()
    {
        StorageController.checknewProduct(IDProduct - 7);
        CommonObjectScript.arrayProducts[IDProduct - 7] += 1;
        //print("ID : " + IDProduct + " co so luong " + CommonObjectScript.arrayProducts[IDProduct - 7]);
    }

    void AddCommonObject(int dollar, int diamond)
    {
        GameObject commonObject = GameObject.Find("CommonObject");
        if (commonObject != null)
        {
            commonObject.GetComponent<CommonObjectScript>().AddDollar(dollar);
            commonObject.GetComponent<CommonObjectScript>().AddDiamond(diamond);

        }
    }

    void DestroyProducPre()
    {
        foreach (Transform product in panelFrame.GetComponentsInChildren<Transform>())
        {

            if (product != null && product.name == "product")
                Destroy(product.gameObject);
        }
        foreach (Transform coin in transform.parent.GetComponentsInChildren<Transform>())
        {

            if (coin != null && coin.name == "coin")
                Destroy(coin.gameObject);
        }
    }
}
