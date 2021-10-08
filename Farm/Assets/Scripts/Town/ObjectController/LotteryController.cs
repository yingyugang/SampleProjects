using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Common;
using Assets.Scripts.Store;

public class LotteryController : MonoBehaviour
{

    // Use this for initialization
    public GameObject wheelLuck;
    //  public GameObject navigation;
    public static bool isCompleteSpin;
    public static bool isSpinning;
    private bool isLight;
    private bool isAgain;
    private int iDItemAward;
    public UITexture BG;
    public Texture[] listSpriteBG;

    private float delayTimeChangeTexture;
    private float countDelayChangeTexture;

    public UITexture[] itemProduct;
    private List<int> resultRandomProduct;
    //private List<int> IDAllowViewProductInMarket; // for test

    private static Texture[] listSpriteProductOther;
    private int countAward;
    public GameObject itemAward;
    public GameObject efect;
    public UILabel labelCountAward;

    public UIButton buttonSpin;
    public UILabel labelSpinFree;
    public GameObject spinNotFree;
    public UILabel labelCountSpin;
    public UILabel labelSpin;
    public static int countSpin;

    public GameObject Popup;
    private List<int> IDAllowViewProductInMarket;
    AudioControl audioControl;
    void OnEnable()
    {
       // countSpin = GetCountSpin();
        CommonObjectScript.isViewPoppup = true;
        IDAllowViewProductInMarket = new List<int>(); //for test 
        //print(MissionData.shopDataMission.listProducts.Count + "bbbbbbbbbbbbbbbb");
        foreach (ProductData pr in MissionData.shopDataMission.listProducts)
        {
            IDAllowViewProductInMarket.Add(pr.idProduct);
        }
        print(IDAllowViewProductInMarket.Count);
        resultRandomProduct = new List<int>();
        resultRandomProduct.Clear();

        RandomProduct(IDAllowViewProductInMarket);   // MissionData.townDataMission.itemsInShop; cái này mới đúng, sau này dùng

    }
    void Start()
    {
        audioControl = GameObject.Find("AudioControl").GetComponent<AudioControl>();
        if (MaketController.listSpriteProduct == null)
            MaketController.listSpriteProduct = Resources.LoadAll<Texture>("Factory/Button/Images/Product");
        SetItemProduct();

        SetViewLabelButtonSpin();
        delayTimeChangeTexture = 0.25f;
        if (listSpriteProductOther == null)
            listSpriteProductOther = Resources.LoadAll<Texture>("Town/ImageLoteryItem");
    }
    
    // Update is called once per frame
    void Update()
    {
        if (CommonObjectScript.isEndGame || CommonObjectScript.isViewComplete)
        {
            EndGame();
            if (!TownScenesController.isHelp)
                BtnClose_Click();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!TownScenesController.isHelp)
                BtnClose_Click();
        }
        SetStatesBG();
        if (countSpin == 0)
        {
            SetViewLabelButtonSpin();
        }

    }
    void OnApplicationPause(bool pauseStatus)
    {
        if (!pauseStatus)
        {
            if (!FactoryScenesController.isHelp)
                BtnClose_Click();
        }
    }
    public void SpinWheelLuck() // button Spin Click Event
    {
      
        this.gameObject.GetComponent<Animator>().enabled = false;
        if (isAgain)
        {
            if (!isSpinning)
            {
                audioControl.PlaySound("Quay xo so");
                isSpinning = true;
                isLight = true;
                float zChange = Random.Range(-4150, -2000);
                LeanTween.rotateZ(wheelLuck, zChange, 5).setEase(LeanTweenType.easeInOutQuad).setOnComplete(delegate()
                {
                    isCompleteSpin = true;
                    isLight = false;
                });
                SetStatusButtonSpin(false);
                DialogAchievement.AddDataAchievement(6, 1);
                MissionData.townDataMission.currentNumber += 1;
            }
            isAgain = false;
        }
        else
        {
            if (!isSpinning)
            {

                if (VariableSystem.diamond >= countSpin)
                {
                    audioControl.PlaySound("Quay xo so");
                    isSpinning = true;
                    isLight = true;
                    float zChange = Random.Range(-4150, -2000);
                    LeanTween.rotateZ(wheelLuck, zChange, 5).setEase(LeanTweenType.easeInOutQuad).setOnComplete(delegate()
                    {
                        isCompleteSpin = true;
                        isLight = false;
                    });
                    AddCommonObject(0, -countSpin);
                    countSpin++;
                    SetStatusButtonSpin(false);
                    DialogAchievement.AddDataAchievement(6, 1);
                    MissionData.townDataMission.currentNumber += 1;
                }
                else
                {
                    audioControl.PlaySound("Click 1");
                    DialogInapp.ShowInapp();
                }
            }
        }
       // SaveCountSpin();
    }
    void SetStatesBG()
    {
        if (isLight)
        {
            if (countDelayChangeTexture <= delayTimeChangeTexture)
            {
                BG.GetComponent<UITexture>().mainTexture = listSpriteBG[1];
                countDelayChangeTexture += Time.deltaTime;
            }
            else if (countDelayChangeTexture <= delayTimeChangeTexture * 2)
            {
                BG.GetComponent<UITexture>().mainTexture = listSpriteBG[2];
                countDelayChangeTexture += Time.deltaTime;
            }
            else if (countDelayChangeTexture > delayTimeChangeTexture * 2)
            {
                countDelayChangeTexture = 0;
            }
        }
        else
        {
            BG.GetComponent<UITexture>().mainTexture = listSpriteBG[0];
        }
    }
    void RandomProduct(List<int> iDProductInMaket)
    {
        List<int> iDProductInMaketTemp = iDProductInMaket;

        for (int count = 0; count < 5; count++)
        {
            int tempRandom = Random.Range(0, 1000) % iDProductInMaketTemp.Count;
            resultRandomProduct.Add(iDProductInMaketTemp[tempRandom]);
            iDProductInMaketTemp.RemoveAt(tempRandom);
        }
    }
    void SetItemProduct() // set data and image for item product in wheel
    {
        int count = 0;
        foreach (int id in resultRandomProduct)
        {
            itemProduct[count].GetComponent<UITexture>().mainTexture = MaketController.listSpriteProduct[id - 7];
            itemProduct[count].GetComponent<ItemWheelController>().iDItem = id;
            count++;
        }
    }

    public void ViewAward(int iDItem)
    {
        iDItemAward = iDItem;

        if (iDItem != 28 && iDItem != 29)
        {
            setDataItemAward(iDItem);
            itemAward.transform.localPosition = new Vector3(0, 0, 0);
            itemAward.SetActive(true);
            efect.SetActive(true);

            LeanTween.scale(itemAward, new Vector3(1.5f, 1.5f, 1.5f), 2).setEase(LeanTweenType.linear);
        }
        else
        {
            if (iDItem == 29)
                isAgain = true;
            SetViewLabelButtonSpin();
            SetStatusButtonSpin(true);
            isSpinning = false;
        }
    }
    void setDataItemAward(int iDItem)
    {
        if (iDItem <= 22)
        {
            countAward = Random.Range(0, 1000) % 10 + 1;
            itemAward.GetComponent<UITexture>().mainTexture = MaketController.listSpriteProduct[iDItem - 7];
        }
        else
        {
            itemAward.GetComponent<UITexture>().mainTexture = listSpriteProductOther[iDItem - 23];

            if (iDItem == 23)
                countAward = 3;
            else if (iDItem == 24)
                countAward = 500;
            else if (iDItem == 25)
                countAward = 5;
            else if (iDItem == 26)
                countAward = 300;
            else if (iDItem == 27)
                countAward = 1;
        }
        labelCountAward.text = "+" + countAward;
    }

    public void ItemAward_Click()
    {
        audioControl.PlaySound("Click 1");
        efect.gameObject.SetActive(false);
        LeanTween.scale(itemAward, new Vector3(0.7f, 0.7f, 0.7f), .5f).setEase(LeanTweenType.linear);
        LeanTween.moveLocal(itemAward, new Vector3(0, 200, 0), 0.5f).setEase(LeanTweenType.linear).setOnComplete(delegate()
        {

            UpdateAward(iDItemAward);
            itemAward.SetActive(false);
            SetViewLabelButtonSpin();
            SetStatusButtonSpin(true);
            isSpinning = false;
        });

    }
    void EndGame()
    {
        if (isSpinning)
        {
            itemAward.SetActive(false);
            SetViewLabelButtonSpin();
            SetStatusButtonSpin(true);
            isSpinning = false;
            isCompleteSpin = false;
            LeanTween.cancel(wheelLuck);
        }
    }
    void UpdateAward(int iDItem)
    {
        if (iDItem <= 22)
        {
            CommonObjectScript.arrayProducts[iDItem - 7] += countAward;
            StorageController.checknewProduct(iDItem - 7);
        }
        else
        {
            if (iDItem == 24 || iDItem == 26)
            {
                AddCommonObject(countAward, 0);
            }
            else if (iDItem == 23 || iDItem == 25 || iDItem == 27)
            {
                AddCommonObject(0, countAward);
            }
        }
    }
    void SetViewLabelButtonSpin()
    {
        if (countSpin == 0)
        {
            labelSpinFree.gameObject.SetActive(true);
            labelSpinFree.text = TownScenesController.languageTowns["FreeSpin"];
            spinNotFree.SetActive(false);
        }
        else
        {
            if (isAgain)
            {
                labelSpinFree.gameObject.SetActive(true);
                labelSpinFree.text = TownScenesController.languageTowns["AgainSpin"];
                spinNotFree.SetActive(false);
            }
            else
            {
                labelCountSpin.text = countSpin.ToString();
                labelSpin.text = TownScenesController.languageTowns["AgainSpin"];
                labelSpinFree.gameObject.SetActive(false);
                spinNotFree.SetActive(true);
            }
        }
    }

    void SetStatusButtonSpin(bool isActive)
    {
        if (isActive)
        {
            buttonSpin.pressed = new Color32(183, 163, 123, 255);
            buttonSpin.hover = new Color32(225, 200, 150, 255);
            buttonSpin.disabledColor = new Color32(128, 128, 128, 255);
            buttonSpin.defaultColor = new Color32(255, 255, 255, 255);

            labelSpinFree.color = new Color32(255, 255, 255, 255);
            labelCountSpin.color = new Color32(255, 255, 255, 255);
            labelSpin.color = new Color32(255, 255, 255, 255);
            spinNotFree.GetComponent<UITexture>().color = new Color32(255, 255, 255, 255);
        }
        else
        {
            buttonSpin.defaultColor = new Color32(70, 70, 70, 255);
            buttonSpin.hover = new Color32(70, 70, 70, 255);
            buttonSpin.pressed = new Color32(70, 70, 70, 255);
            buttonSpin.disabledColor = new Color32(70, 70, 70, 255);

            labelSpinFree.color = new Color32(70, 70, 70, 255);
            labelCountSpin.color = new Color32(70, 70, 70, 255);
            labelSpin.color = new Color32(70, 70, 70, 255);
            spinNotFree.GetComponent<UITexture>().color = new Color32(70, 70, 70, 255);
        }
        buttonSpin.GetComponent<UIButtonScale>().enabled = isActive;
    }

    //public void VisibleLotery()
    //{

    //    Popup.gameObject.SetActive(true);
    //    LeanTween.scale(Popup, new Vector3(1, 1, 1), 0.5f).setEase(LeanTweenType.easeOutBack);
    //}
    public void BtnClose_Click()
    {
        audioControl.PlaySound("Click 1");
        if (!isSpinning)
        {
            this.gameObject.GetComponent<Animator>().enabled = true;
            this.gameObject.GetComponent<Animator>().Play("InVisible");
        }
        //LeanTween.scale(Popup, new Vector3(0, 0, 0), 0.5f).setEase(LeanTweenType.easeInBack).setOnComplete(delegate()
        //{
        //    Popup.gameObject.SetActive(false);
        //    this.gameObject.SetActive(false);
        //});
    }
    public void InVisible()
    {
        this.gameObject.SetActive(false);
        CommonObjectScript.isViewPoppup = false;
        CreatTownScenesController.isDenyContinue = false;
    }
    public void EndAnimationVisible()
    {
        // this.gameObject.GetComponent<Animator>().enabled = false;
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

    //public static void SaveCountSpin()
    //{
    //    PlayerPrefs.SetInt("CountSpin", countSpin);
    //    PlayerPrefs.Save();
    //}

    //private int GetCountSpin()
    //{
    //    return PlayerPrefs.GetInt("CountSpin");
    //}
}
