using UnityEngine;
using System.Collections;

public class CicleButtonController : MonoBehaviour
{
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!Application.loadedLevelName.Equals("Factory")) Destroy(gameObject);
    }

    void OnMouseUp()
    {
        if (FactoryScenesController.isHelp)
        {
        }
        else
        {
            if (!ButtonViewController.animator.Equals(null))
                ButtonViewController.animator.SetTrigger("Collape");

            if (ProductQueueController.animator != null)
                ProductQueueController.animator.SetTrigger("Collape");
        }
    }
}
