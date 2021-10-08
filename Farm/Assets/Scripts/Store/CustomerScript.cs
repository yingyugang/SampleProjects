using System;
using System.Collections.Generic;
using UnityEngine;

public class CustomerScript : MonoBehaviour
{
    public List<GameObject> cusObjects;
    public List<GameObject> popupServices;
    public Sprite sao1, sao0;

    private ShopCenterScript shopcenter;
    private GameObject HarvestPlantPrefabs, HarvestPlant, customerPrefabs, customer, valueAdd;
    private Transform tempTransform;
    SpriteRenderer[] sprites0, sprites1, sprites2;
    private int i, j;
    private float countTimeTalk = 0;
    private string[] namePrefabs = new string[10] { "oanh", "bubu", "monkey", "anna", "james", "haru", "panda", "adam", "tippi", "kevin" };
    // Use this for initialization
    void Start()
    {
        GoogleAnalytics.instance.LogScreen("Shop Screen");
        shopcenter = GameObject.FindGameObjectWithTag("ShopObject").GetComponent<ShopCenterScript>();
        shopcenter.common.WarningInvisible(CommonObjectScript.Button.Shop);
        HarvestPlantPrefabs = (GameObject)Resources.Load("Farm/HarvestPlant");
        sprites0 = popupServices[0].GetComponentsInChildren<SpriteRenderer>();
        sprites1 = popupServices[1].GetComponentsInChildren<SpriteRenderer>();
        sprites2 = popupServices[2].GetComponentsInChildren<SpriteRenderer>();
        OnNavigatedTo();
    }

    void OnNavigatedTo()
    {
        for (i = 0; i < 3; i++)
        {
            UpdatePositionPopupService(i, shopcenter.cusDatas[i, 0].typeCustomer);
            if (shopcenter.cusDatas[i, 0].indexProduct != -1)
                if (i == 0)
                    sprites0[1].sprite = shopcenter.common.spriteProducts[shopcenter.cusDatas[i, 0].indexProduct * 2 + 1] as Sprite;
                else if (i == 1)
                    sprites1[1].sprite = shopcenter.common.spriteProducts[shopcenter.cusDatas[i, 0].indexProduct * 2 + 1] as Sprite;
                else
                    sprites2[1].sprite = shopcenter.common.spriteProducts[shopcenter.cusDatas[i, 0].indexProduct * 2 + 1] as Sprite;
            for (j = 0; j < 5; j++)
            {
                if (shopcenter.cusDatas[i, j].indexProduct != -1)
                {
                    CreateCustomer(i, j, shopcenter.cusDatas[i, j].typeCustomer, shopcenter.cusDatas[i, j].indexProduct);
                }
            }
        }
    }
    // Update is called once per frame
    void LateUpdate()
    {
        #region waiting service
        UpdatePopupService(0, shopcenter.cusDatas[0, 0].indexProduct, shopcenter.cusDatas[0, 0].timeWait, sprites0);
        UpdatePopupService(1, shopcenter.cusDatas[1, 0].indexProduct, shopcenter.cusDatas[1, 0].timeWait, sprites1);
        UpdatePopupService(2, shopcenter.cusDatas[2, 0].indexProduct, shopcenter.cusDatas[2, 0].timeWait, sprites2);
        #endregion
        if (popupServices[0].activeSelf || popupServices[1].activeSelf || popupServices[2].activeSelf)
        {
            countTimeTalk += Time.deltaTime;
            if (countTimeTalk > 10)
            {
                countTimeTalk = 0;
                if (UnityEngine.Random.Range(0, 1250) % 100 > 50)
                {
                    CommonObjectScript.audioControl.PlaySound("Noi chuyen (Phong the)");
                }
            }
        }
    }

    public void UpdatePopupService(int row, int index, float time, SpriteRenderer[] sprites)
    {
        if (index < 0)
        {
            popupServices[row].SetActive(false);
        }
        else
        {
            popupServices[row].SetActive(true);

            if (time < 16)
            {
                sprites[2].sprite = sao0;
                if (time < 12)
                {
                    sprites[3].sprite = sao0;
                    if (time < 8)
                    {
                        sprites[4].sprite = sao0;
                        if (time < 4)
                        {
                            sprites[5].sprite = sao0;
                            shopcenter.cusDatas[row, 0].star = 1;
                        }
                        else
                        {
                            sprites[5].sprite = sao1;
                            shopcenter.cusDatas[row, 0].star = 2;
                        }
                    }
                    else
                    {
                        sprites[4].sprite = sao1;
                        sprites[5].sprite = sao1;
                        shopcenter.cusDatas[row, 0].star = 3;
                    }
                }
                else
                {
                    sprites[3].sprite = sao1;
                    sprites[4].sprite = sao1;
                    sprites[5].sprite = sao1;
                    shopcenter.cusDatas[row, 0].star = 4;
                }
            }
            else
            {
                sprites[2].sprite = sao1;
                sprites[3].sprite = sao1;
                sprites[4].sprite = sao1;
                sprites[5].sprite = sao1;
            }

            if (time < 10)
                cusObjects[5 * row].GetComponentInChildren<Animator>().Play("angry");
        }
    }

    public void CreateCustomer(int indexR, int indexC, int typeCus, int indexP, bool isNewCus = false)//add Object
    {
        if (isNewCus)
        {
            CommonObjectScript.audioControl.PlaySound("Chuong gio");
        }
        tempTransform = cusObjects[indexR * 5 + indexC].transform.Find("Customer");//destroy previous object
        if (tempTransform != null)
        {
            GameObject.Destroy(tempTransform.gameObject);
        }

        if (indexP != -1)
        {
            customerPrefabs = (GameObject)Resources.Load("Shop/Customer/" + namePrefabs[typeCus]);
            customer = (GameObject)Instantiate(customerPrefabs);
            customer.name = "Customer";
            customer.transform.parent = cusObjects[indexR * 5 + indexC].transform;
            customer.GetComponent<Transform>().localPosition = new Vector3((indexR == 0 ? -480 : (indexR == 1 ? -960 : -1440)) - 330 * indexC, (indexR == 0 ? -1050 : (indexR == 1 ? -855 : -660)) - 220 * indexC, 0);
            customer.GetComponent<Transform>().localScale = Vector3.one * 450;

            SpriteRenderer[] sprites = customer.GetComponentsInChildren<SpriteRenderer>();
            foreach (SpriteRenderer sprite in sprites)
            {
                sprite.GetComponent<Renderer>().sortingLayerID = 5 * (2 - indexR) + indexC;
            }
        }
        if (indexC == 0)
        {
            if (indexR == 0)
                sprites0[1].sprite = shopcenter.common.spriteProducts[shopcenter.cusDatas[indexR, 0].indexProduct * 2 + 1] as Sprite;
            else if (indexR == 1)
                sprites1[1].sprite = shopcenter.common.spriteProducts[shopcenter.cusDatas[indexR, 0].indexProduct * 2 + 1] as Sprite;
            else
                sprites2[1].sprite = shopcenter.common.spriteProducts[shopcenter.cusDatas[indexR, 0].indexProduct * 2 + 1] as Sprite;
            UpdatePositionPopupService(indexR, shopcenter.cusDatas[indexR, 0].typeCustomer);
        }
    }

    public void UpdatedRow(int row, int star, int money, bool isBonus)// update row when one customer is complete waiting.
    {
        //thay đổi hàng chờ
        tempTransform = cusObjects[row * 5].transform.Find("Customer");//destroy object in colum 0
        if (tempTransform != null)
        {
            GameObject.Destroy(tempTransform.gameObject);
        }
        for (i = 1; i < 5; i++)
        {
            tempTransform = cusObjects[row * 5 + i].transform.Find("Customer");
            if (tempTransform == null) break;

            tempTransform.parent = cusObjects[row * 5 + i - 1].transform;//change parent
            tempTransform.localPosition = new Vector3((row == 0 ? -480 : (row == 1 ? -960 : -1440)) - 330 * (i - 1), (row == 0 ? -1050 : (row == 1 ? -855 : -660)) - 220 * (i - 1), 0);

            SpriteRenderer[] sprites = tempTransform.GetComponentsInChildren<SpriteRenderer>();
            foreach (SpriteRenderer sprite in sprites)
            {
                sprite.GetComponent<Renderer>().sortingLayerID = 5 * (2 - row) + i - 1;
            }
        }
        UpdatePositionPopupService(row, shopcenter.cusDatas[row, 0].typeCustomer);
        if (row == 0 && shopcenter.cusDatas[row, 0].indexProduct >= 0)
            sprites0[1].sprite = shopcenter.common.spriteProducts[shopcenter.cusDatas[row, 0].indexProduct * 2 + 1] as Sprite;
        else if (row == 1 && shopcenter.cusDatas[row, 0].indexProduct >= 0)
            sprites1[1].sprite = shopcenter.common.spriteProducts[shopcenter.cusDatas[row, 0].indexProduct * 2 + 1] as Sprite;
        else if (shopcenter.cusDatas[row, 0].indexProduct >= 0)
            sprites2[1].sprite = shopcenter.common.spriteProducts[shopcenter.cusDatas[row, 0].indexProduct * 2 + 1] as Sprite;

        if (money > 0)
        {
            CommonObjectScript.audioControl.PlaySound("Sao tien bay");
            CreateAnimationAddValue(row, "gold", "+" + money);
            CreateAnimationAddValue(row, "star", "+" + star);
            if (isBonus)
                CreateAnimationAddValue(row, "bonus", "+" + money / 2);
        }
    }
    private void UpdatePositionPopupService(int row, int typeCus)
    {
        switch (typeCus)
        {
            case 7: popupServices[row].transform.localPosition = new Vector3(-725 - 475 * row, 400 + 200 * row, 0); break;
            case 3: popupServices[row].transform.localPosition = new Vector3(-725 - 475 * row, 400 + 200 * row, 0); break;
            case 6: popupServices[row].transform.localPosition = new Vector3(-725 - 475 * row, 400 + 200 * row, 0); break;
            case 1: popupServices[row].transform.localPosition = new Vector3(-725 - 475 * row, 000 + 200 * row, 0); break;
            case 9: popupServices[row].transform.localPosition = new Vector3(-725 - 475 * row, 600 + 200 * row, 0); break;
            case 0: popupServices[row].transform.localPosition = new Vector3(-725 - 475 * row, 600 + 200 * row, 0); break;
            case 5: popupServices[row].transform.localPosition = new Vector3(-725 - 475 * row, 650 + 200 * row, 0); break;
            case 2: popupServices[row].transform.localPosition = new Vector3(-725 - 475 * row, 100 + 200 * row, 0); break;
            case 8: popupServices[row].transform.localPosition = new Vector3(-725 - 475 * row, 500 + 200 * row, 0); break;
            default: popupServices[row].transform.localPosition = new Vector3(-550 - 500 * row, 300 + 200 * row, 0); break;
        }
    }

    private void CreateAnimationHarvest(int row, string name, int number)// create animation when user harvest one product
    {
        for (i = 0; i < number; i++)
        {
            HarvestPlant = (GameObject)Instantiate(HarvestPlantPrefabs,
                Vector3.zero,
                Quaternion.identity);
            HarvestPlant.name = name;
            if (name.Equals("gold"))
            {
                HarvestPlant.GetComponent<HarvestPlantScript>().setValue("Common/vang", new Vector3(240, 330, 0), 50);
            }
            else
                HarvestPlant.GetComponent<HarvestPlantScript>().setValue("Common/sao", new Vector3(10, 330, 0), 50);
            HarvestPlant.transform.parent = this.transform;
            HarvestPlant.GetComponent<Transform>().localPosition = cusObjects[5 * row].transform.localPosition + new Vector3(UnityEngine.Random.Range(-40, 40), UnityEngine.Random.Range(-20, 20) + 200, 0);
            HarvestPlant.GetComponent<Transform>().localScale = Vector3.one;
        }
    }
    private void CreateAnimationAddValue(int row, string name, string number)// create animation when add money or item
    {
        valueAdd = (GameObject)Instantiate(shopcenter.valuePrefabs);
        valueAdd.name = name;
        valueAdd.transform.parent = this.transform;
        valueAdd.GetComponent<Transform>().localPosition = cusObjects[5 * row].transform.localPosition + new Vector3(0, 240, 0);
        valueAdd.GetComponent<Transform>().localScale = Vector3.one;
        if (name.Equals("gold"))
        {
            CreateAnimationHarvest(row, name, 5);
            valueAdd.GetComponent<AddValueScript>().setValue(number);
        }
        else if (name.Equals("bonus"))
        {
            valueAdd.GetComponent<AddValueScript>().setValue(number + " tip", ":D");
            valueAdd.GetComponent<Transform>().localPosition = new Vector3(140, -50, 0);
        }
        else
        {
            CreateAnimationHarvest(row, name, Convert.ToInt16(number));
            valueAdd.GetComponent<AddValueScript>().setValue(number, "Common/sao1");
        }
    }
    internal void deleteColum(int indexRowDelete)//delete one row when you fire one staff
    {
        for (i = 0; i < 5; i++)
        {
            if (shopcenter.cusDatas[indexRowDelete, i].indexProduct != -1)
            {
                GameObject.Destroy(cusObjects[indexRowDelete * 5 + i].transform.Find("Customer").gameObject);
                shopcenter.cusDatas[indexRowDelete, i] = new Customer();
            }
        }
    }

    void OnDestroy()
    {
        for (int i = 0; i < 3; i++)
            if (shopcenter.staffDatas[i].typeStaff != 0)
            {
                if (shopcenter.staffDatas[i].statement.Equals("sleep") ||
                    shopcenter.staffDatas[i].statement.Equals("lazy") ||
                    shopcenter.staffDatas[i].statement.Equals("sick"))
                    ShopCenterScript.isNeedWarning = true;
            }
    }
}

public class Customer
{
    public int indexRow { get; set; }//index row of customer (0->2)
    public int indexcolum { get; set; }//index colum of customer (0->4)
    public int typeCustomer { get; set; }//type customer
    public float timeWait { get; set; }//the wait time of customer,
    public bool isWaiting { get; set; }//đang chờ đợi để phục vụ (false khi đang trong hàng chờ, true khi đang chờ phục vụ)
    public int indexProduct { get; set; }//type of item request
    public int star;
    public Customer(int typecus = -1)
    {
        star = 5;
        if (typecus == -1)
            typeCustomer = UnityEngine.Random.Range(0, 1250) % ShopCenterScript.getNumberCustomer();
        else typeCustomer = typecus;
        indexProduct = -1;
        timeWait = 24;
        if (DialogShop.BoughtItem[4]) timeWait *= 1.2f;//Increase 20% of the customer's time for waiting in shop
        isWaiting = false;
    }
}
