using Assets.Scripts.Common;
using Assets.Scripts.Farm;
using System.Collections.Generic;
using UnityEngine;

public class PanelNewItemScript : MonoBehaviour
{
    int countSeat;
    public GameObject[] listItems;
    public UITexture[] listTextures = new UITexture[6];
    public UILabel[] listLabels = new UILabel[8];
    public UISprite[] listSprites = new UISprite[6];

    public Texture[] methodMultimedia;
    public Texture[] methodResearchs;

    string tempStr;
    CommonObjectScript common;
    string[] staffNames = new string[] { "sakura", "peter", "sarah", "nam" };
    string[] houseNames = new string[] { "staff", "superMarket", "multimedia", "marketResearch", "lottery", "technology" };

    bool ischangeLanguage;
    // Use this for initialization
    void Start()
    {
        if (VariableSystem.language.Equals("Vietnamese"))
        {
            ischangeLanguage = true;
            listLabels[6].text = "CHÚC MỪNG!";
            listLabels[7].text = "Bạn đã được mở khóa:";
            listLabels[8].text = "Đóng";
        }
        for (int i = 0; i < 6; i++)
        {
            listSprites[i].gameObject.SetActive(!listSprites[i].gameObject.activeSelf);
            if (ischangeLanguage) listLabels[i].text = "MỚI!";
        }

    }

    public void setSeat(int number) { countSeat = number; setItemPosition(); }
    void setSize(int index, int width, int height)
    {
        listSprites[index].width = width;
        listSprites[index].height = height;
        listTextures[index].width = width;
        listTextures[index].height = height;
    }

    public void setDataShow(List<NewItem> datas, CommonObjectScript common)
    {
        this.common = common;
        setSeat(datas.Count);
        for (int i = 0; i < datas.Count; i++)
        {
            if (datas[i].type.Equals("field"))
            {
                #region field/cage
                if (datas[i].id == 2)//cage
                {
                    tempStr = "Common/Images/chuong" + datas[i].level;
                    listTextures[i].mainTexture = Resources.Load(tempStr) as Texture;
                    setSize(i, 180, 90);
                }
                else if (datas[i].id == 1)
                {
                    tempStr = "dat" + datas[i].level;
                    listTextures[i].gameObject.SetActive(false);
                    listSprites[i].gameObject.SetActive(false);
                    listSprites[i].spriteName = tempStr;
                    setSize(i, 150, 80);
                }
                else
                {
                    tempStr = "chuongca" + datas[i].level;
                    listTextures[i].gameObject.SetActive(false);
                    listSprites[i].gameObject.SetActive(false);
                    listSprites[i].spriteName = tempStr;
                    setSize(i, 120, 120);
                }
                #endregion
            }
            else if (datas[i].type.Equals("breed"))
            {
                #region breed
                tempStr = Breed.getName(datas[i].id);
                listTextures[i].mainTexture = Resources.Load("Farm/Icon/" + datas[i].id + "." + tempStr) as Texture;
                setSize(i, 120, 120);
                #endregion
            }
            else if (datas[i].type.Equals("machine"))
            {
                #region machine
                tempStr = "Factory/Button/Images/Machine/" + datas[i].id + "-lv" + datas[i].level;
                listTextures[i].mainTexture = Resources.Load(tempStr) as Texture;
                setSize(i, 150, 150);
                #endregion
            }
            else if (datas[i].type.Equals("staff"))
            {
                #region staff
                tempStr = "Town/fullavatarStaff/" + staffNames[datas[i].id - 1] + datas[i].level;
                listTextures[i].mainTexture = Resources.Load(tempStr) as Texture;
                setSize(i, (datas[i].id == 4 ? 75 : 90), 140);
                #endregion
            }
            else if (datas[i].type.Equals("product"))
            {
                #region product
                tempStr = "0" + (datas[i].id - 7).ToString();
                if (tempStr.Length == 3) tempStr = tempStr.Substring(1);
                tempStr = "Factory/Button/Images/Product/" + tempStr;
                listTextures[i].mainTexture = Resources.Load(tempStr) as Texture;
                setSize(i, 120, 120);
                #endregion
            }
            else if (datas[i].type.Equals("building"))
            {
                #region building
                tempStr = "Town/Deco/" + houseNames[datas[i].id] + "/1";
                listTextures[i].mainTexture = Resources.Load(tempStr) as Texture;
                setSize(i, 105, 130);
                #endregion
            }
            else
            {
                #region method
                if (datas[i].id / 10 == 2)
                {
                    listTextures[i].mainTexture = methodMultimedia[datas[i].id % 10];
                }
                else
                {
                    listTextures[i].mainTexture = methodResearchs[datas[i].id % 10];
                }
                setSize(i, 128, 91);
                #endregion
            }
        }
    }

    void setItemPosition()
    {
        if (countSeat == 1)
        {
            listItems[0].transform.localPosition = new Vector3(0, -60);
            listItems[1].SetActive(false);
            listItems[2].SetActive(false);
            listItems[3].SetActive(false);
            listItems[4].SetActive(false);
            listItems[5].SetActive(false);
        }
        else if (countSeat == 2)
        {
            listItems[0].transform.localPosition = new Vector3(-100, -60);
            listItems[1].transform.localPosition = new Vector3(100, -60);
            listItems[2].SetActive(false);
            listItems[3].SetActive(false);
            listItems[4].SetActive(false);
            listItems[5].SetActive(false);
        }
        else if (countSeat == 3)
        {
            listItems[0].transform.localPosition = new Vector3(-180, -60);
            listItems[1].transform.localPosition = new Vector3(0, -60);
            listItems[2].transform.localPosition = new Vector3(180, -60);
            listItems[3].SetActive(false);
            listItems[4].SetActive(false);
            listItems[5].SetActive(false);
        }
        else if (countSeat == 4)
        {
            listItems[0].transform.localPosition = new Vector3(-100, 15);
            listItems[1].transform.localPosition = new Vector3(100, 15);
            listItems[2].transform.localPosition = new Vector3(-100, -140);
            listItems[3].transform.localPosition = new Vector3(100, -140);
            listItems[4].SetActive(false);
            listItems[5].SetActive(false);
        }
        else if (countSeat == 5)
        {
            listItems[0].transform.localPosition = new Vector3(-180, 15);
            listItems[1].transform.localPosition = new Vector3(0, 15);
            listItems[2].transform.localPosition = new Vector3(180, 15);
            listItems[3].transform.localPosition = new Vector3(-100, -140);
            listItems[4].transform.localPosition = new Vector3(100, -140);
            listItems[5].SetActive(false);
        }
        else
        {
            listItems[0].transform.localPosition = new Vector3(-180, 15);
            listItems[1].transform.localPosition = new Vector3(0, 15);
            listItems[2].transform.localPosition = new Vector3(180, 15);
            listItems[3].transform.localPosition = new Vector3(-180, -140);
            listItems[4].transform.localPosition = new Vector3(0, -140);
            listItems[5].transform.localPosition = new Vector3(180, -140);
        }
    }

    public void OK_Click()
    {
        GetComponent<Animator>().Play("EndShowNewItem");
    }
    public void Destroy()
    {
        common.isOpennew = false;
        GameObject.Destroy(this.gameObject);
    }
}
