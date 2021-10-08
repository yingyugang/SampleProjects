using UnityEngine;
using System.Collections;

public class Game12_CameraControl : MonoBehaviour
{

	public static Game12_CameraControl instance;

	public Transform target;
	public Game12_Player_Controller playerController;


	public float speedMoveCam = 5f;
	public float speedMoveCamCurrent = 0;
	public Vector3 offset;

	public Transform posBottomCam;

	Vector3 posMoveto;

	//zoom camera
	public Camera myCamera;
	public float limitYBottomMap = -10f;

	//limit camera
	float distanceY = 0;

	void Awake ()
	{
		instance = this;
	}

	void Start ()
	{
		speedMoveCamCurrent = speedMoveCam;
		posMoveto = target.position + offset;
		transform.position = posMoveto;

	}

	// Update is called once per frame
	void FixedUpdate ()
	{
		if (!GameCountDownMediator.didEndCountDown){// || Game12_GameManager.instance.myState != Game12_GameState.playing) {
			return;
		}

		distanceY = 0;
		speedMoveCamCurrent = speedMoveCam;
		posMoveto = Vector3.Lerp (posMoveto, target.position + offset, Time.fixedDeltaTime * speedMoveCamCurrent);
		posMoveto.x = target.position.x + offset.x;
		if (posMoveto.y < limitYBottomMap)
			posMoveto.y = limitYBottomMap;
		transform.position = posMoveto;
	}
}
