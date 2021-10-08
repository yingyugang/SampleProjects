using UnityEngine;
using System.Collections;

public class DiamondEffect : MonoBehaviour
{

    void Start()
    {
    }

    public void SetData(int count, bool add = true)
    {
        if (add)
        {
            transform.Find("Count").GetComponent<UILabel>().text = "+" + count;
            LeanTween.moveLocalY(this.gameObject, transform.position.y + 120, 1f).setEase(LeanTweenType.easeOutCirc).setOnComplete(delegate()
            {
                Debug.Log(this.gameObject.name);
                Destroy(this.gameObject);
            });
            VariableSystem.AddDiamond(count);
        }
        else
        {
            transform.Find("Count").GetComponent<UILabel>().text = "-" + count;
            LeanTween.moveLocalY(this.gameObject, transform.position.y - 120, 1f).setEase(LeanTweenType.easeOutCirc).setOnComplete(delegate()
            {
                Destroy(this.gameObject);
            });
            VariableSystem.AddDiamond(-count);
        }
    }

    void Update()
    {

    }
}
