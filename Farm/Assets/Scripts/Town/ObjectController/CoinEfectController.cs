using UnityEngine;
using System.Collections;

public class CoinEfectController : MonoBehaviour
{

    // thư viện nên có timedelay dùng chung, object nào không có tiem delay thì gọi timeDelay = 0
    private Animator animator;
    public UITexture coin;
    public UILabel labelCost;
    public int costProduct;
    public float timeDelay;
    public float countTimeDelay;

    void Start()
    {
        animator = GetComponent<Animator>();
        labelCost.text = (-costProduct).ToString();
        countTimeDelay = 0;
    }

    void Update()
    {
        if (countTimeDelay < timeDelay)
        {
            countTimeDelay += Time.deltaTime;
            animator.enabled = false;
        }
        else
        {
            coin.GetComponent<UITexture>().enabled = true;
            labelCost.GetComponent<UILabel>().enabled = true;
            animator.enabled = true;
        }
    }

    public void Invisible()
    {
        Destroy(gameObject);
    }
}
