using UnityEngine;
using System.Collections;

public class Game12_BackgroundPalaxa : MonoBehaviour
{
	public float speedMove;
	public Transform target;
	public Transform myTransform;

	Vector3 posMoveTo;
	private float deltaX = 0f;
	private float oldX = 0f;
	void Start ()
	{
		oldX = target.position.x;
		posMoveTo.x = target.position.x;
		posMoveTo.y = target.position.y;
		myTransform.position = posMoveTo;
	}
	void Update ()
	{
//		posMoveTo = Vector3.Lerp (posMoveTo, target.position, Time.deltaTime * speedMove);
		//-->| by anhgh
		//calculate background movement speed ratio to fit with player's speed
		deltaX = target.position.x - posMoveTo.x;
		Game12_BGLoop.BG_Global_Speed = (deltaX / Time.deltaTime) * 0.1f;
		//Debug.Log ("Global speed " + Game12_BGLoop.BG_Global_Speed);
		//--<|
		posMoveTo.x = target.position.x;
		//posMoveTo.y = target.position.y;
		myTransform.position = posMoveTo;
		//transform.position = Vector3.Lerp (transform.position, posMoveTo, Time.deltaTime);
	}
}
