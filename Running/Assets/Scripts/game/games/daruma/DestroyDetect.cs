using UnityEngine;
using System.Collections;

namespace Daruma
{
    public class DestroyDetect : MonoBehaviour
    {
        void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.tag == "Block")
            {
                collider.gameObject.GetComponent<Block>().Destroy();
                return;
            }
        }
    }
}

