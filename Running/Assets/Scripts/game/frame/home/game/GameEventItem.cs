using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameEventItem : MonoBehaviour
{
    public int itemID;
    public GameObject mask;
    public Text num;
    public Toggle toggle;
    private int itemNum;

    private void OnEnable ()
    {
        SetItem ();
    }

    public void Init (int index)
    {
        itemID = index + 9;
        toggle.onValueChanged.AddListener (ValueChangedHandler);
    }

    private void SetItem ()
    {
        itemNum = PlayerItems.GetInstance[itemID];
        toggle.isOn = false;
        toggle.interactable = itemNum > 0;
        mask.SetActive (itemNum <= 0);
        SetItemNum (false);
    }

    private void OnDestroy ()
    {
        toggle.onValueChanged.RemoveListener (ValueChangedHandler);
    }

    private void ValueChangedHandler (bool isOn)
    {
        SetItemNum (isOn);
    }

    public int GetItemID ()
    {
        return toggle.isOn ? itemID : 0;
    }

    private void SetItemNum (bool isOn)
    {
        if (isOn)
        {
            num.text = string.Format ("{0}{1}{2}{3}", LanguageJP.PINK_COLOR_PREFIX, itemNum - 1, LanguageJP.COLOR_SUFFIX, LanguageJP.GE);
        }
        else
        {
            num.text = string.Format ("{0}{1}", itemNum, LanguageJP.GE);
        }
    }
}
