using System;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class EmployeeScript : MonoBehaviour
{
    public List<GameObject> staffObjects;
    public CustomerScript customerScript;
    public GameObject giftBox, giftPopup;
    public UILabel[] txtPopupConfirm;

    Animator[] khungItemAnimator = new Animator[4];
    GameObject[] itemStaff = new GameObject[3];
    ShopCenterScript shopcenter;
    public GuideShopScript guideShop;
    GameObject txtWarning;
    GameObject staffPrefabs, staff;
    int indexPopupOpen, indexPopupConfirm;
    int i;

    string tempName;
    Transform tempTransform;
    Vector3 localPos, localScale;
    // Use this for initialization
    void Start()
    {
        shopcenter = GameObject.Find("ShopCenter").GetComponent<ShopCenterScript>();
        if (!shopcenter.isUsingItemWeather)
        {
            giftBox.SetActive(false);
        }
        else
        {
            giftBox.GetComponent<UITexture>().mainTexture = Resources.Load("Shop/ItemWeather/Hop_" + shopcenter.common.itemSelected) as Texture;
        }
        for (i = 0; i < 3; i++)
        {
            khungItemAnimator[i] = staffObjects[i].transform.Find("KhungItem").GetComponent<Animator>();
            itemStaff[i] = staffObjects[i].transform.Find("ItemStaff").gameObject;
        }
        khungItemAnimator[3] = giftBox.GetComponent<Animator>();
        indexPopupOpen = -1;
        indexPopupConfirm = -1;

        txtWarning = transform.Find("TxtNotify").gameObject;
        if (VariableSystem.language.Equals("Vietnamese"))
        {
            txtWarning.GetComponentInChildren<UILabel>().text = "Vào thành phố để thuê nhân viên";
            txtPopupConfirm[0].text = "SA THẢI";
            txtPopupConfirm[1].text = "Bạn muốn sa thải nhân viên này";
            txtPopupConfirm[2].text = "Đồng ý";
            txtPopupConfirm[3].text = "Hủy bỏ";
        }
        if (shopcenter.staffDatas[0].typeStaff != 0)
            txtWarning.SetActive(false);
        if (shopcenter.staffDatas[1].typeStaff != 0)
            txtWarning.SetActive(false);
        if (shopcenter.staffDatas[2].typeStaff != 0)
            txtWarning.SetActive(false);

        OnNavigatedTo();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (shopcenter.common.countTimeOneDay == 0)
        {
            giftBox.GetComponent<UITexture>().mainTexture = Resources.Load("Shop/ItemWeather/Hop_" + shopcenter.common.itemSelected) as Texture;
        }

        for (i = 0; i < 3; i++)
        {
            if (shopcenter.changeStaffs[i])
            {
                shopcenter.changeStaffs[i] = false;
                if (shopcenter.staffDatas[i].typeStaff != 0)
                {
                    if (shopcenter.staffDatas[i].statement.StartsWith("fix_"))
                    {
                        #region healing
                        if (indexPopupOpen == i)//if this staff is selected
                        {
                            khungItemAnimator[i].Play("EndShowListItem");
                            indexPopupOpen = -1;
                        }
                        itemStaff[i].transform.localScale = Vector3.one * 250;
                        itemStaff[i].GetComponent<Animator>().Play(shopcenter.staffDatas[i].statement.Substring(4));
                        #endregion
                    }
                    else
                    {
                        #region upgrade
                        if (!(shopcenter.indexTraining != i || shopcenter.countTimeTraining != 0))
                        {
                            //update staff
                            tempName = shopcenter.staffDatas[i].getName() + (shopcenter.staffDatas[i].levelStaff - 2).ToString();
                            tempTransform = staffObjects[i].transform.Find(tempName);
                            if (tempTransform == null) print("NULL Object");
                            else
                            {
                                localPos = tempTransform.transform.localPosition;
                                localScale = tempTransform.transform.localScale;
                                GameObject.Destroy(tempTransform.gameObject);
                            }
                            tempName = shopcenter.staffDatas[i].getName();
                            staffPrefabs = (GameObject)Resources.Load("Shop/Staff/" + tempName + (shopcenter.staffDatas[i].levelStaff - 1));
                            staff = (GameObject)Instantiate(staffPrefabs);
                            staff.name = tempName + (shopcenter.staffDatas[i].levelStaff - 1).ToString();
                            staff.transform.parent = staffObjects[i].transform;
                            staff.transform.localPosition = localPos;
                            staff.transform.localScale = localScale;
                            staff.GetComponent<Animator>().Play(shopcenter.staffDatas[i].statement);
                            shopcenter.indexTraining = -1;
                        }
                        ChangeState(i);
                        itemStaff[i].transform.localScale = Vector3.zero;
                        #endregion
                    }

                }
                else
                {
                    itemStaff[i].transform.localScale = Vector3.zero;
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.Escape) || CommonObjectScript.isEndGame || CommonObjectScript.isViewComplete)
        {
            #region back action handle
            if (CommonObjectScript.isGuide) return;
            if (CommonObjectScript.isViewPoppup)
            {
                indexPopupConfirm = -1;
                transform.Find("PopupConfirm").gameObject.SetActive(false);
                CommonObjectScript.isViewPoppup = false;
            }
            if (indexPopupOpen != -1)
            {
                if (indexPopupOpen == 3) khungItemAnimator[3].Play("StopItem");
                else
                    khungItemAnimator[indexPopupOpen].Play("EndShowListItem");
                indexPopupOpen = -1;
            }
            #endregion
        }
    }
    void OnApplicationPause(bool isPause)
    {
        if (!isPause)
        {
            #region back action handle
            if (CommonObjectScript.isGuide) return;
            indexPopupConfirm = -1;
            transform.Find("PopupConfirm").gameObject.SetActive(false);
            CommonObjectScript.isViewPoppup = false;
            if (indexPopupOpen != -1)
            {
                if (indexPopupOpen == 3) khungItemAnimator[3].Play("StopItem");
                else
                    khungItemAnimator[indexPopupOpen].Play("EndShowListItem");
                indexPopupOpen = -1;
            }
            #endregion
        }
    }

    void OnNavigatedTo()//update all staffs
    {
        for (i = 0; i < 3; i++)
        {
            if (shopcenter.staffDatas[i].typeStaff != 0)// not empty
            {
                tempName = shopcenter.staffDatas[i].getName();
                staffPrefabs = (GameObject)Resources.Load("Shop/Staff/" + tempName + (shopcenter.staffDatas[i].levelStaff - 1));
                staff = (GameObject)Instantiate(staffPrefabs);
                staff.name = tempName + (shopcenter.staffDatas[i].levelStaff - 1).ToString();
                staff.transform.parent = staffObjects[i].transform;
                if (tempName.Equals("nam") || tempName.Equals("peter"))
                {
                    staff.transform.localPosition = new Vector3(120 - 520 * i, -280 + 200 * i, 3);
                    staff.transform.localScale = new Vector3(470, 470, 95);
                }
                else
                {
                    staff.transform.localPosition = new Vector3(120 - 520 * i, -250 + 200 * i, 3);
                    if (tempName.Equals("sakura"))
                        staff.transform.localScale = new Vector3(500, 500, 105);
                    else
                    {
                        staff.transform.localScale = new Vector3(510, 510, 100);
                    }
                }
                if (!shopcenter.staffDatas[i].statement.StartsWith("fix_"))
                {
                    itemStaff[i].transform.localScale = Vector3.zero;
                    staff.GetComponent<Animator>().Play(shopcenter.staffDatas[i].statement);
                }
                else
                {
                    itemStaff[i].transform.localScale = Vector3.one * 250;
                    staff.GetComponent<Animator>().Play(shopcenter.staffDatas[i].statement.Substring(4));
                    itemStaff[i].GetComponent<Animator>().Play(shopcenter.staffDatas[i].statement.Substring(4));
                }
            }
            else
            {
                itemStaff[i].transform.localScale = Vector3.zero;
            }
        }
    }

    public void FireStaff()
    {
        if (VariableSystem.mission == 4 && guideShop.indexGuide == 4) guideShop.NextGuideText();

        if (indexPopupOpen != -1)
        {
            indexPopupConfirm = indexPopupOpen;
            khungItemAnimator[indexPopupOpen].Play("EndShowListItem");
        }
        else print("indexPopup có vấn đề");
        transform.Find("PopupConfirm").gameObject.SetActive(true);
        indexPopupOpen = -1;
        CommonObjectScript.isViewPoppup = true;
        CommonObjectScript.audioControl.PlaySound("Click 1");
    } //fire

    public void Popup_Fire(GameObject btn)//confirm fire
    {
        if (btn.gameObject.name.Equals("Btn_Ok"))
        {
            if (indexPopupConfirm == shopcenter.indexTraining)//cancel trainning
            {
                shopcenter.countTimeTraining = 0f;
            }
            shopcenter.listStaffsinShop[shopcenter.staffDatas[indexPopupConfirm].typeStaff - 1].isHired = false;
            customerScript.deleteColum(indexPopupConfirm);
            GameObject.Destroy(staffObjects[indexPopupConfirm].transform.Find(shopcenter.staffDatas[indexPopupConfirm].getName() + (shopcenter.staffDatas[indexPopupConfirm].levelStaff - 1).ToString()).gameObject);
            shopcenter.numberOfStaff--;
            shopcenter.staffDatas[indexPopupConfirm] = new Staff(0);
            if (VariableSystem.mission == 4 && guideShop.indexGuide == 5) guideShop.NextGuideText();
        }
        else if (VariableSystem.mission == 4 && guideShop.indexGuide == 5) return;
        indexPopupConfirm = -1;
        CommonObjectScript.audioControl.PlaySound("Click 1");
        transform.Find("PopupConfirm").gameObject.SetActive(false);
    }

    public void Staff_Click(GameObject obj)//when user tap one staff
    {
        if (indexPopupConfirm == -1)
        {
            CommonObjectScript.audioControl.PlaySound("Click 1");
            int id = Convert.ToInt16(obj.name.Substring(7));
            if (CommonObjectScript.isGuide)
            {
                if (VariableSystem.mission == 4)
                    if (id == 1 && guideShop.indexGuide == 3)
                    {
                        guideShop.NextGuideText();
                    }
                    else return;
                else if (VariableSystem.mission == 6)
                {
                    if ((id == 0 && guideShop.indexGuide == 2) ||
                        (id == 1 && guideShop.indexGuide == 3) ||
                        (id == 2 && guideShop.indexGuide == 4))
                        guideShop.NextGuideText();
                    else return;
                }
                else return;
            }
            if (shopcenter.staffDatas[id].typeStaff != 0) // if have staff
            {
                if (indexPopupOpen == id)//if this staff is selected
                {
                    khungItemAnimator[id].Play("EndShowListItem");
                    indexPopupOpen = -1;
                }
                else
                {
                    if (indexPopupOpen != -1)
                    {
                        if (indexPopupOpen == 3)
                            khungItemAnimator[3].Play("HideItem");
                        else
                            khungItemAnimator[indexPopupOpen].Play("StartListItem");
                    }
                    string temp = shopcenter.staffDatas[id].statement;
                    if (temp.StartsWith("fix_"))
                    {
                    }
                    else if (temp.Equals("sleep") || temp.Equals("lazy") || temp.Equals("sick"))
                    {
                        shopcenter.staffDatas[id].statement = "fix_" + temp;
                        itemStaff[id].GetComponent<Animator>().Play(temp);
                        itemStaff[id].transform.localScale = Vector3.one * 250;
                        ChangeState(id);
                        indexPopupOpen = -1;
                    }
                    else
                    {
                        khungItemAnimator[id].Play("ShowListItem");
                        indexPopupOpen = id;
                    }
                }
            }
        }
    }

    public void ChangeState(int i)//change animator of employee
    {
        if (!shopcenter.staffDatas[i].statement.StartsWith("fix_"))
        {
            tempName = shopcenter.staffDatas[i].getName() + (shopcenter.staffDatas[i].levelStaff - 1).ToString();
            tempTransform = staffObjects[i].transform.Find(tempName);
            if (tempTransform == null) print("NULL Object");
            else
                tempTransform.GetComponent<Animator>().Play(shopcenter.staffDatas[i].statement);
        }
    }
    public void GiftBox_Click()
    {
        if (CommonObjectScript.isGuide)
        {
            if (VariableSystem.mission == 9 && guideShop.indexGuide == 5) guideShop.NextGuideText();
            else return;
        }
        CommonObjectScript.audioControl.PlaySound("Click 1");
        if (giftPopup.activeSelf)
        {
            khungItemAnimator[3].Play("StopItem");
            indexPopupOpen = -1;
        }
        else
        {
            if (shopcenter.numberOfStaff > 0)
            {
                if (indexPopupOpen != -1)
                {
                    khungItemAnimator[indexPopupOpen].Play("StartListItem");
                }
                khungItemAnimator[3].Play("StartItem");
                indexPopupOpen = 3;
            }
        }
    }
    public void ItemWeather_Click(GameObject item)
    {
        string name = item.name;
        if (CommonObjectScript.isGuide)
        {
            if (VariableSystem.mission == 9 && guideShop.indexGuide == 6 && name.Equals("Kem")) guideShop.NextGuideText();
            else return;
        }
        khungItemAnimator[3].Play("StopItem");
        indexPopupOpen = -1;
        shopcenter.common.itemSelected = name;
        giftBox.GetComponent<UITexture>().mainTexture = Resources.Load("Shop/ItemWeather/Hop_" + name) as Texture;
        CommonObjectScript.audioControl.PlaySound("Click 1");
    }
    public void BG_Click()
    {
        if (CommonObjectScript.isGuide) return;
        if (indexPopupOpen != -1)
        {
            if (indexPopupOpen == 3) khungItemAnimator[3].Play("StopItem");
            else
                khungItemAnimator[indexPopupOpen].Play("EndShowListItem");
            indexPopupOpen = -1;
        }
    }
    public void NavigateCity()
    {
        if (!CommonObjectScript.isGuide)
            shopcenter.common.City_Click();
    }
}

public class Staff
{
    public int typeStaff;
    public int levelStaff { get; set; }//level (1->4), 0 is empty
    public string statement { get; set; }//trạng thái của nhân viên (sale, rest, sleep, sick)
    public float timeService { get; set; }
    public float curtimeService { get; set; }
    public int percentSick { get; set; }
    public int speed, health, mental;
    public float countTimeHealing;
    public int countToCheck;
    public Staff(int level)
    {
        typeStaff = 0;
        this.levelStaff = level;
        statement = "stand";
        curtimeService = 0;
        countTimeHealing = 3f;
        countToCheck = 0;
        speed = 0;
        health = 0;
        mental = 0;
        percentSick = 0;
        if (DialogShop.BoughtItem[3]) timeService = 3f;
        else timeService = 5f;
    }
    public void setData(int level, int speed, int health, int mental, int percent, int type = -1)
    {
        this.levelStaff = level;
        this.speed = speed;
        this.health = health;
        this.mental = mental;
        this.percentSick = percent;
        if (type != -1) this.typeStaff = type;
    }
    public void Training()
    {
        if (statement.Equals("sell")) statement = "training";
        else statement = "training_stand";
    }
    public string getName()
    {
        switch (typeStaff)
        {
            case 1: return "sakura";
            case 2: return "peter";
            case 3: return "sarah";
            default: return "nam";
        }
    }
}
