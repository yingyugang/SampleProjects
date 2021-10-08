using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PanelFrameController : MonoBehaviour
{

    public GameObject[] arrayFrameResult;
    public static List<int> iDFinishTalk;

    public static List<int> iDProductResearch;
    public static Texture[] listSpriteProduct;

    public static int iDtalKing;
    public static bool isUpdate;

    private int countIDProductResearch;
    void Start()
    {
        isUpdate = false;
        countIDProductResearch = 0;
        if (listSpriteProduct == null)
            listSpriteProduct = Resources.LoadAll<Texture>("Factory/Button/Images/Product");
        else
            print(listSpriteProduct.Length);
        if (iDFinishTalk == null)
        {
            iDFinishTalk = new List<int>();
            iDProductResearch = new List<int>();
        }
        if (iDFinishTalk.Count != 0)
        {
            foreach (int count in iDFinishTalk)
            {
                arrayFrameResult[count].SetActive(true);
                arrayFrameResult[count].GetComponent<Animator>().Play("Finish");
               
                arrayFrameResult[count].GetComponent<FrameController>().setImage(listSpriteProduct[iDProductResearch[countIDProductResearch] - 7]);
                countIDProductResearch++;
            }
            arrayFrameResult[iDtalKing].SetActive(true);
        }
    }


    void Update()
    {
        if (isUpdate)
        {
            if (iDFinishTalk.Count >= 1)
            {
                arrayFrameResult[iDFinishTalk[iDFinishTalk.Count - 1]].GetComponent<FrameController>().setImage(listSpriteProduct[iDProductResearch[iDProductResearch.Count - 1] - 7]);
                arrayFrameResult[iDFinishTalk[iDFinishTalk.Count - 1]].GetComponent<Animator>().Play("Result");
            }
            arrayFrameResult[iDtalKing].SetActive(true);
            isUpdate = false;
        }
    }
}
