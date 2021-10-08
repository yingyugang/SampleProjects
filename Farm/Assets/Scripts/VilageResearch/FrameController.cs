using UnityEngine;
using System.Collections;

public class FrameController : MonoBehaviour {

	// Use this for initialization
    private Transform label;
    private Transform imageProduct;

    private float timeCount;
    private float timeTalk;
    void OnEnable()
    {
        label = transform.Find("Label");
        imageProduct = transform.Find("Texture");
    }
	void Start () {
       
	}
	
	// Update is called once per frame
	void Update () {
        if (timeCount <= 1)
        {
            label.GetComponent<UILabel>().text = ".";
        }
        else if (timeCount > 1 && timeCount <= 2)
        {
            label.GetComponent<UILabel>().text = "..";
        }
        else if (timeCount > 2 && timeCount <= 3)
        {
            label.GetComponent<UILabel>().text = "...";
        }
        else
        {
            timeCount = 0;
        }
        timeCount += Time.deltaTime;
	}

    public void setImage(Texture texture)
    {
        //imageProduct.gameObject.SetActive(true);
        imageProduct.GetComponent<UITexture>().mainTexture = texture;
    }
}
