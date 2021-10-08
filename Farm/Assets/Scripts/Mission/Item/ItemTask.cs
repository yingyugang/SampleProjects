using UnityEngine;
using System.Collections;

public class ItemTask : MonoBehaviour
{
    public bool finish;
    public UILabel lbContent, lbResult;
    public UIToggle togComplete;
    public ItemAbstract item;
    public bool typeShow1;
    Color oldColor;
    Color newColor;

    void Awake()
    {
        lbContent = transform.Find("Content").GetComponent<UILabel>();
        lbResult = transform.Find("Result").GetComponent<UILabel>();
        togComplete = transform.Find("Checkbox").GetComponent<UIToggle>();
        oldColor = lbResult.color;
        finish = false;
        newColor = Color.green;
    }

    void Update()
    {
        if (item != null)
        {
            if (typeShow1)
            {
                item.typeShow = 1;
            }
            else
            {
                item.typeShow = 0;
            }
            lbResult.text = "(" + item.getCurrent() + "/" + item.getTarget() + ")";
            if (item.getCurrent() >= item.getTarget())
            {
                togComplete.value = true;
                finish = true;
                lbResult.color = newColor;
                //lbResult.color = new Color(20, 150, 20, 255);

            }
            else
            {
                togComplete.value = false;
                finish = false;
                lbResult.color = oldColor;
            }
        }
    }

    public void SetData(string content, ItemAbstract item)
    {
        this.item = item;
        lbContent.text = content;
        if (item.typeShow == 1)
        {
            typeShow1 = true;
        }
        if (item != null)
        {
            lbResult.text = "(0/" + item.getTarget() + ")";
        }
        else
        {
            lbResult.text = "";
        }
        newColor = Color.green;
    }
    public void SetDataForResult(string content, ItemAbstract item)
    {
        this.item = item;
        lbContent.text = content;
        if (item != null)
        {
            lbResult.text = "(0/" + item.getTarget() + ")";
        }
        else
        {
            lbResult.text = "";
        }
        newColor = new Color(20 / 255.0f, 150 / 255.0f, 20 / 255.0f, 1);
    }
}
