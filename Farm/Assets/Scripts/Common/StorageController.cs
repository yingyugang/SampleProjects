using UnityEngine;
using System.Collections;
using System;

public class StorageController : MonoBehaviour
{
    public string state = "material";
    public GameObject btnMar, btnPro;
    public GameObject[] Items;
    int count, i, j;
    float countTime = 0f;
    CommonObjectScript common;

    UIScrollView scrollView;
    public static int[] memoryMaterial = new int[9];
    public static int[] memoryProduct = new int[16];
    Texture[] blackwhiteIcon;
    void Awake()
    {
        blackwhiteIcon = Resources.LoadAll<Texture>("Storage/black-white");
    }
    void OnEnable()
    {
        common = transform.parent.GetComponent<CommonObjectScript>();
        scrollView = this.GetComponentInChildren<UIScrollView>();
        Material_Click();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        countTime += Time.deltaTime;
        if (countTime > 0.5f)
        {
            countTime = 0;
            if (state.Equals("material"))
            {
                Create_Material();
            }
            else
            {
                Create_Product();
            }
        }
        if (common.isOpenStorage && Input.GetMouseButton(0))
        {
            if (Input.mousePosition.x > Screen.width / 2)
            {
                Close_Click();
            }
        }
    }
    private void Create_Material()
    {
        count = 0;
        for (i = 0; i < 9; i++)
        {
            if (memoryMaterial[i] >= 0)
            {
                Items[i].SetActive(true);
                if (CommonObjectScript.arrayMaterials[memoryMaterial[i]] < 0)
                {
                    Debug.LogError("Âm nguyên liệu, cần check lại ngay");
                    Debug.LogError("Nguyên liệu " + memoryMaterial[i] + " còn " + CommonObjectScript.arrayMaterials[memoryMaterial[i]]);
                    Items[i].GetComponentInChildren<UILabel>().text = "0";
                }
                else Items[i].GetComponentInChildren<UILabel>().text = CommonObjectScript.arrayMaterials[memoryMaterial[i]].ToString();
                if (CommonObjectScript.arrayMaterials[memoryMaterial[i]] <= 0)
                {
                    Items[i].GetComponent<UITexture>().mainTexture = blackwhiteIcon[16 + memoryMaterial[i]];
                }
                else
                {
                    Items[i].GetComponent<UITexture>().mainTexture = common.spriteMaterials[memoryMaterial[i] * 2] as Texture;
                }
                count++;
            }
            else break;
        }
        for (i = 0; i < 9; i++)
        {
            if (CommonObjectScript.arrayMaterials[i] > 0)
            {
                for (j = 0; j < count; j++)
                    if (i == memoryMaterial[j]) break;
                if (j == count)
                {
                    memoryMaterial[count] = i;
                    Items[count].SetActive(true);
                    Items[count].GetComponent<UITexture>().mainTexture = common.spriteMaterials[i * 2] as Texture;
                    Items[count].GetComponentInChildren<UILabel>().text = CommonObjectScript.arrayMaterials[i].ToString();
                    Items[count].GetComponent<UITexture>().color = Color.white;
                    Items[count].GetComponentInChildren<UILabel>().color = Color.white;
                    count++;
                }
            }
        }
        for (i = count; i < 16; i++) Items[i].SetActive(false);
    }
    private void Create_Product()
    {
        count = 0;
        for (i = 0; i < 16; i++)
        {
            if (memoryProduct[i] >= 0)
            {
                Items[i].SetActive(true);
                if (CommonObjectScript.arrayProducts[memoryProduct[i]] < 0)
                {
                    Debug.LogError("Âm sản phẩm, cần check lại ngay");
                    Debug.LogError("Sản phẩm " + memoryProduct[i] + " còn " + CommonObjectScript.arrayProducts[memoryProduct[i]]);
                    Items[i].GetComponentInChildren<UILabel>().text = "0";
                }
                else Items[i].GetComponentInChildren<UILabel>().text = CommonObjectScript.arrayProducts[memoryProduct[i]].ToString();
                if (CommonObjectScript.arrayProducts[memoryProduct[i]] <= 0)
                {
                    Items[i].GetComponent<UITexture>().mainTexture = blackwhiteIcon[memoryProduct[i]];
                }
                else
                {
                    Items[i].GetComponent<UITexture>().mainTexture = common.spriteProducts[memoryProduct[i] * 2] as Texture;
                }
                count++;
            }
            else break;
        }
        for (i = 0; i < 16; i++)
        {
            if (CommonObjectScript.arrayProducts[i] > 0)
            {
                for (j = 0; j < count; j++)
                    if (i == memoryProduct[j]) break;
                if (j == count)
                {
                    memoryProduct[count] = i;
                    Items[count].SetActive(true);
                    Items[count].GetComponent<UITexture>().mainTexture = common.spriteProducts[i * 2] as Texture;
                    Items[count].GetComponentInChildren<UILabel>().text = CommonObjectScript.arrayProducts[i].ToString();
                    Items[count].GetComponent<UITexture>().color = new Color32(255, 255, 255, 255);
                    Items[count].GetComponentInChildren<UILabel>().color = new Color32(255, 255, 255, 255);
                    count++;
                }
            }
        }
        //for (i = 0; i < 16; i++)
        //{
        //    if (CommonObjectScript.arrayProducts[i] > 0)
        //    {
        //        Items[count].SetActive(true);
        //        Items[count].GetComponent<UITexture>().mainTexture = common.spriteProducts[i * 2] as Texture;
        //        Items[count].GetComponentInChildren<UILabel>().text = CommonObjectScript.arrayProducts[i].ToString();
        //        count++;
        //    }
        //}
        for (i = count; i < 16; i++) Items[i].SetActive(false);
    }
    public void Material_Click()
    {
        state = "material";
        btnMar.SetActive(true);
        btnPro.SetActive(false);
        scrollView.enabled = false;
        if (this.GetComponentInChildren<SpringPanel>() != null)
        {
            this.GetComponentInChildren<SpringPanel>().enabled = false;
        }
        scrollView.transform.localPosition = new Vector3(-525, 75, 0);
        scrollView.GetComponent<UIPanel>().clipOffset = new Vector2(0, 0);
        Create_Material();
    }
    public void Product_Click()
    {
        state = "product";
        btnMar.SetActive(false);
        btnPro.SetActive(true);
        scrollView.enabled = true;
        Create_Product();
    }
    public void Close_Click()
    {
        CommonObjectScript.audioControl.PlaySound("Click 1");
        this.GetComponent<Animator>().Play("StopShowStorage");
    }
    public void DeActive()
    {
        gameObject.SetActive(false);
        common.isOpenStorage = false;
        if (CommonObjectScript.isGuide)
        {
            GameObject.Find("GuideFarmController").GetComponent<GuideFarmScript>().NextGuideText();
        }
    }
    public static void checknewMaterial(int idMaterial)
    {
        //id start 0 to 8
        int id;
        for (id = 0; id < 9; id++)
        {
            if (memoryMaterial[id] == -1) { break; }
            else if (memoryMaterial[id] == idMaterial) return;
        }
        if (id < 9)
        {
            memoryMaterial[id] = idMaterial;
        }
    }
    public static void checknewProduct(int idProduct)
    {
        //id start 0 to 15
        int id;
        for (id = 0; id < 16; id++)
        {
            if (memoryProduct[id] == -1) break;
            else if (memoryProduct[id] == idProduct) return;
        }
        if (id < 16)
        {
            memoryProduct[id] = idProduct;
        }
    }
}
