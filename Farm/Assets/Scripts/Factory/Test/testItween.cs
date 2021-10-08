using UnityEngine;
using System.Collections;

public class testItween : MonoBehaviour {

	// Use this for initialization
    float time;
    Vector3[] lstPosition;
    Vector2 targetposition;
	void Start () {
        //Vector3[] lstPosition = iTweenPath.GetPath("path");
        targetposition = new Vector3(-600, -325);
        Vector3[] lstPosition = 
        {
            new Vector3(transform.localPosition.x , transform.localPosition.y ,transform.position.z),
            new Vector3(transform.localPosition.x - 270, transform.localPosition.y + 70,transform.position.z),
            new Vector3(-600f,-325f,0)
           
        };
        //iTween.ScaleTo(gameObject, iTween.Hash("x", 0.5f, "y", 0.5f, "time", 2.5, "delay", .5f, "easetype", iTween.EaseType.linear));
       //iTween.MoveTo(gameObject, iTween.Hash("path", lstPosition, "time", 2, "delay", .5f, "islocal", true, "orienttopath", false, "easetype", iTween.EaseType.linear));
       // iTween.MoveTo(gameObject, new Vector3(-600f, -325f, 0), 3);
	}
	
	// Update is called once per frame
	void Update ()
    {
        transform.localPosition = Vector2.MoveTowards(transform.localPosition, targetposition, 1000.0f * Time.deltaTime);
        //iTween.MoveBy(gameObject, iTween.Hash("path", lstPosition, "time", 2, "delay", .5f, "islocal", true, "orienttopath", false, "easetype", iTween.EaseType.linear));
        print(transform.localPosition);
        //time += Time.deltaTime;
        //if (time < 3)
        //{ 
        //    print(time);
        //}
        //else if (time <= 5 && time >= 3.0f)
        //{
        //    iTween.Pause(gameObject);

        //}
        //else
        //{
        //    iTween.Resume(gameObject);
        //}

	}
}
