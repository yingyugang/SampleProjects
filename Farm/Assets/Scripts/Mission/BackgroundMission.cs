using UnityEngine;
using System.Collections;

public class BackgroundMission : MonoBehaviour {

    bool isVisible;
    bool objVisible;
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        //Debug.Log(" Local pos " + transform.localPosition + " pos " + transform.position);
        if (transform.position.x < 3.6f && transform.position.x > -3.6f)
        {
            isVisible = true;
        }
        else
        {
            isVisible = false;
        }
        if (isVisible)
        {
            if (!objVisible)
            {
                for (int i = 0; i < transform.childCount; i++)
                {
                    transform.GetChild(i).gameObject.SetActive(true);
                    objVisible = true;
                }
                //Debug.Log("Hien len");
            }
        }
        else
        {
            if (objVisible)
            {
                for (int i = 0; i < transform.childCount; i++)
                {
                    objVisible = false;
                    transform.GetChild(i).gameObject.SetActive(false);
                }
                //Debug.Log("An di");
            }
        }
	}
}
