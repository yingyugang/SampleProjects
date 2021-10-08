using UnityEngine;
using System.Collections;

public class HelpGirlController : MonoBehaviour {

    public static Animator animator;

	
	void Start () {
        animator = GetComponent<Animator>();
        SetIDLayer(7);
	}
	
	
	void Update () {
	}

    void SetIDLayer(int sortingLayerID)
    {
        Transform[] transforms = gameObject.GetComponentsInChildren<Transform>(true);

        for (int i = 0; i < transforms.Length; i++)
        {
            GameObject gObject = transforms[i].gameObject;
            if (gObject.GetComponent<SpriteRenderer>() != null)
            {
                gObject.GetComponent<SpriteRenderer>().sortingLayerID = sortingLayerID;
            }
        }
    }
    void ResetAnimation()
    {
        //animator.SetTrigger("Stand");

    }
}
