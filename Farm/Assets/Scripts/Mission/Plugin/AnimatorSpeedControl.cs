using UnityEngine;
using System.Collections;

public class AnimatorSpeedControl : MonoBehaviour
{
    public float speed = 1;
	void Start () {
        gameObject.GetComponent<Animator>().speed = speed;
	}
	
}
