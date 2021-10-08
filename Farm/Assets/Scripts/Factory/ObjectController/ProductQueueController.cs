using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ProductQueueController : MonoBehaviour
{

    public static Animator animator;
    public GameObject[] arrayProductQueue;
    public bool isUpdateProductQueue;
    public Sprite[] arrayProductSprite;
    private GameObject machineSelected;
    int countArrayProductQueue;
    int count;
    void Start()
    {
        animator = GetComponent<Animator>();
        machineSelected = MachineController.machineSelect;
        arrayProductSprite = Resources.LoadAll<Sprite>("Factory/Button/Images/Product");
        CreatProductInQueue();

    }

    // Update is called once per frame
    void Update()
    {
        if (!Application.loadedLevelName.Equals("Factory")) Destroy(gameObject);
    }

    public void CreatProductInQueue()
    {
        countArrayProductQueue = 0;
        arrayProductSprite = Resources.LoadAll<Sprite>("Factory/Button/Images/Product");
        if (machineSelected != null)
        {
            if (machineSelected.GetComponent<MachineController>().IDProductQueue.Count != 0)
            {
                foreach (ProductInfomation pr in machineSelected.GetComponent<MachineController>().IDProductQueue)
                {
                    // print("pr.IDProduct : " + pr.IDProduct);
                    arrayProductQueue[countArrayProductQueue].GetComponent<SpriteRenderer>().sprite = arrayProductSprite[pr.IDProduct - 7];
                    countArrayProductQueue++;
                }
            }
            for (count = machineSelected.GetComponent<MachineController>().IDProductQueue.Count; count < arrayProductQueue.Length; count++)
            {

                if ("Vietnamese".Equals(VariableSystem.language))
                    arrayProductQueue[count].GetComponent<SpriteRenderer>().sprite = arrayProductSprite[17];
                else
                    arrayProductQueue[count].GetComponent<SpriteRenderer>().sprite = arrayProductSprite[16];
            }
        }
    }

    void Destroy()
    {
        Destroy(gameObject);
    }
}
