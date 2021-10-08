using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PurchaseFinishedMediator : PopupContentMediator
{
    public Text content;

    private void OnEnable ()
    {
        if (objectList != null && objectList.Count > 0 && !string.IsNullOrEmpty(objectList[0].ToString()))
        {
            content.text = objectList[0].ToString();
        }
    }
}
