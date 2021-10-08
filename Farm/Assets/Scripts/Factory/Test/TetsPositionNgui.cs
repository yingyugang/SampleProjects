using UnityEngine;
using System.Collections;

public class TetsPositionNgui : MonoBehaviour {

	// Use this for initialization
     public GameObject target;
    Vector2 pos = new Vector2();
    public static GameObject selectMachine;
	void Start () {
       // pos = new Vector2(3.925149f, 0.186806f);
        //gameObject.transform.localPosition = pos;
        //NGUITools.MakePixelPerfect(gameObject.transform);
        //NGUIMath.ScreenToPixels(pos, gameObject.transform);
       // gameObject.transform.position= NGUIMath.ScreenToParentPixels(pos, target.transform);
        //NGUITools.AddChild(target,gameObject);
     
      
      
	}
	
	// Update is called once per frame
	void Update () {
        //Vector3 screenPos = camera.WorldToScreenPoint(target.transform.position);
        //print("target is " + screenPos.x + " pixels from the left");
        if (selectMachine != null)
        {
            Vector3 pos = new Vector3(selectMachine.transform.position.x / Screen.width * 1280, selectMachine.transform.position.y / Screen.height * 768, 0);

            //print(gameObject.transform.localPosition = NGUIMath.WorldToLocalPoint(selectMachine.transform.localPosition, Camera.main, UICamera.mainCamera, gameObject.transform));
            gameObject.transform.localPosition = Camera.main.WorldToScreenPoint(pos);
        }
            //gameObject.transform.localPosition = selectMachine.transform.localPosition * 100;
           // gameObject.transform.localPosition = NGUIMath.WorldToLocalPoint(selectMachine.transform.localPosition, Camera.main, UICamera.mainCamera, gameObject.transform);
	}
    public void Display(Vector3 pos)
    {
        // Enable select
        gameObject.SetActive(true);

        // Get screen location of node
        Vector2 screenPos = Camera.main.WorldToScreenPoint(pos);

        // Move to node
        gameObject.transform.position = GetComponent<Camera>().ScreenToWorldPoint(screenPos);
    }
}
