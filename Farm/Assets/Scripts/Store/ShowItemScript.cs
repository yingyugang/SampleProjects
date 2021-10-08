using System.Collections.Generic;
using UnityEngine;

public class ShowItemScript : MonoBehaviour
{
    public List<GameObject> cakes;
    UILabel[] couters = new UILabel[16];
    SpriteRenderer[] imgs = new SpriteRenderer[16];
    ShopCenterScript shopcenter;
    int i, counter, index;
    int[] counterImg = new int[16] { 6, 9, 6, 6, 1, 1, 1, 1, 9, 6, 9, 6, 6, 1, 6, 9 };
    // Use this for initialization
    void Start()
    {
        shopcenter = GameObject.Find("ShopCenter").GetComponent<ShopCenterScript>();
        for (int i = 0; i < 16; i++)
        {
            couters[i] = cakes[i].GetComponentInChildren<UILabel>();
            imgs[i] = cakes[i].GetComponentInChildren<SpriteRenderer>();
        }
    }

    public void LateUpdate()
    {
        for (i = 0; i < 16; i++)
        {
            cakes[i].SetActive(false);
            counter = CommonObjectScript.arrayProducts[i];
            if (counter <= 0)
            {
                if (cakes[i].activeSelf)
                    cakes[i].SetActive(false);
            }
            else
            {
                if (!cakes[i].activeSelf)
                    cakes[i].SetActive(true);
                if (counterImg[i] == 1) { }
                else if (counter > counterImg[i])
                {
                    if (!imgs[i].sprite.name.EndsWith(counterImg[i].ToString()))
                    {
                        imgs[i].sprite = Resources.Load<Sprite>("Shop/Product/item" + i + "_" + counterImg[i]);
                    }
                }
                else if (!couters[i].text.Equals(counter.ToString()))
                {
                    imgs[i].sprite = Resources.Load<Sprite>("Shop/Product/item" + i + "_" + counter);
                }
                couters[i].text = counter.ToString();
            }
        }
    }
}
