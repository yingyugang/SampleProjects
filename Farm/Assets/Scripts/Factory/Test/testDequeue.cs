using UnityEngine;
using System.Collections;

public class testDequeue : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    void OnMouseDown()
    {
        GameObject productQueueView;
        productQueueView = GameObject.Find("ProductQueue");
        //if (FactoryScenesController.IDProductQueue.Count > 0)
        //{
        //     FactoryScenesController.IDProductQueue.Dequeue();
        //}
        //print("bbbbb " + FactoryScenesController.IDProductQueue.Count);
        
        productQueueView.GetComponent<ProductQueueController>().CreatProductInQueue();

    }
}
