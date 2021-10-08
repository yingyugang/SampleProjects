using UnityEngine;
using System.Collections;

public class NavigatorController : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    void OnTriggerStay(Collider other)
    {
        if (LotteryController.isCompleteSpin)
        {
            transform.parent.transform.parent.GetComponent<LotteryController>().ViewAward(other.GetComponent<ItemWheelController>().iDItem);
            LotteryController.isCompleteSpin = false;
        }
    }
}
