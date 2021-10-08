using UnityEngine;
using System.Collections;

public class PartcileResult : MonoBehaviour {

    Transform PhaoBay;
    Transform Phao01;
    Transform Phao02;
    Transform Phao03;

	void Start () {
        PhaoBay = transform.Find("PhaoBay");
        Phao01 = transform.Find("Phao01");
        Phao02 = transform.Find("Phao02");
        Phao03 = transform.Find("Phao03");
        float rdY = Random.Range(500f, 700.0f);
        float delay = Random.Range(0.2f, 1.5f);
        float newY = transform.localPosition.y + rdY;
        LeanTween.delayedCall(delay, () =>
        {
            LeanTween.moveLocalY(this.gameObject, newY, 1f).setUseEstimatedTime(true).setEase(LeanTweenType.easeInOutSine).setOnComplete(() =>
            {
                Phao01.gameObject.SetActive(true);
                Phao02.gameObject.SetActive(true);
                Phao03.gameObject.SetActive(true);
                Destroy(PhaoBay.gameObject);
            });
        }).setUseEstimatedTime(true);
       
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
