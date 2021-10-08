using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Game12_BGLoop : MonoBehaviour
{
	public List<SpriteRenderer> background_obj_sky = new List<SpriteRenderer> ();
	//-->| by anhgh
	public static float BG_Global_Speed = 0.5f;
	public Transform Target;
	private Vector3 Target_las_pos;
	//--<| by anhgh
	float XLimit = 8f;
	float XNext = 12f;
	private float bg_sky_speed = 0.2f;
	private float bg_cloud_speed = 0.6f;
	private float bg_layer1_speed = 0.9f;
	private float bg_layer2_speed = 1.8f;

	public Transform topPos, bottomPos;
	void Start(){
		Target = Game12_CameraControl.instance.target;
		Target_las_pos = transform.position;
	}
	void Update ()
	{
		if (!GameCountDownMediator.didEndCountDown)
			return;

		//if (Game12_GameManager.instance.myState != Game12_GameState.playing)
		//	return;
		/*
		Game12_BGLoop.BG_Global_Speed = (Target.position.x - Target_las_pos.x) / Time.deltaTime * 0.1f;
		Target_las_pos.x = Target.position.x;
		Target_las_pos.y = Target.position.y;
		transform.position = Target_las_pos;
		//if (BG_Global_Speed <= 0)
		//	return;
		*/
		Vector3 speedRate = Vector3.left * Time.deltaTime * BG_Global_Speed;

		background_obj_sky [0].transform.Translate (speedRate * bg_sky_speed, Space.Self);
		background_obj_sky [1].transform.Translate (speedRate * bg_sky_speed, Space.Self);

		background_obj_sky [2].transform.Translate (speedRate * bg_cloud_speed, Space.Self);
		background_obj_sky [3].transform.Translate (speedRate * bg_cloud_speed, Space.Self);

		if (background_obj_sky.Count >= 5) {
			background_obj_sky [4].transform.Translate (speedRate * bg_layer1_speed, Space.Self);
			background_obj_sky [5].transform.Translate (speedRate * bg_layer1_speed, Space.Self);

			background_obj_sky [6].transform.Translate (speedRate * bg_layer2_speed, Space.Self);
			background_obj_sky [7].transform.Translate (speedRate * bg_layer2_speed, Space.Self);
		}
		for (int i = 0; i < background_obj_sky.Count; i++) {
			if (background_obj_sky [i].transform.localPosition.x <= -XLimit) {
//				Debug.Log (background_obj_sky [i].transform.localPosition.x);
				XNext = (10f + background_obj_sky [i].transform.localPosition.x) + 10f;
//				Debug.Log (XNext);
				background_obj_sky [i].transform.localPosition = new Vector3 (XNext, background_obj_sky [i].transform.localPosition.y, background_obj_sky [i].transform.localPosition.z);
			}
		}
		//return;
		//background_obj_sky [0].transform.position = new Vector3(background_obj_sky [0].transform.position.x, Camera.main.transform.position.y, 1.5f);
		//background_obj_sky [1].transform.position = new Vector3(background_obj_sky [1].transform.position.x, Camera.main.transform.position.y, 1.5f);
		//background_obj_sky [1].transform.Translate (Vector3.left * Time.deltaTime * bg_sky_speed * BG_Global_Speed, Space.Self);

//		Debug.Log ("Camera.main.transform.position.y= " + Camera.main.transform.position.y + " topPos.position.y=" + topPos.position.y);
		if (Camera.main.transform.position.y > topPos.position.y) {
//			Debug.LogWarning ("out move");
			float distanceY = Camera.main.transform.position.y - topPos.position.y;
			transform.position = new Vector3 (transform.position.x, transform.position.y + distanceY, transform.position.z);
		} else if (Camera.main.transform.position.y < bottomPos.position.y) {
			float distanceY = bottomPos.position.y - Camera.main.transform.position.y;
			transform.position = new Vector3 (transform.position.x, transform.position.y - distanceY, transform.position.z);
		} else {
			transform.position = Vector3.Lerp (transform.position, new Vector3 (transform.position.x, Camera.main.transform.position.y, transform.position.z), 1f);
		}


		//transform.position = new Vector3(Game12_CameraControl.instance.target.position.x, Game12_CameraControl.instance.target.position.y, 25f);
			//new Vector3 (transform.position.x, transform.position.y + distanceY, transform.position.z);
	}

	[ContextMenu ("Show")]
	public void Show ()
	{
		for (int j = 0; j < background_obj_sky.Count; j++) {
			Color bg = background_obj_sky [j].color;
			background_obj_sky [j].color = new Color (bg.r, bg.g, bg.b, 0);
		}
		gameObject.SetActive (true);
		StartCoroutine (watingBackgroundEffectShow ());
	}

	IEnumerator watingBackgroundEffectShow ()
	{
		yield return new WaitForEndOfFrame ();
		for (float i = 0; i <= 1; i += 0.1f) {
			for (int j = 0; j < background_obj_sky.Count; j++) {
				Color bg = background_obj_sky [j].color;
				background_obj_sky [j].color = new Color (bg.r, bg.g, bg.b, i);
				yield return new WaitForEndOfFrame ();
			}
		}
		for (int j = 0; j < background_obj_sky.Count; j++) {
			Color bg = background_obj_sky [j].color;
			background_obj_sky [j].color = new Color (bg.r, bg.g, bg.b, 1);
			yield return new WaitForEndOfFrame ();
		}
	}

	[ContextMenu ("Hide")]
	public void Hide ()
	{

		if (gameObject.activeSelf) {
			StartCoroutine (watingBackgroundEffectHide ());
		}

	}

	IEnumerator watingBackgroundEffectHide ()
	{
		yield return new WaitForEndOfFrame ();
		for (float i = 1; i >= 0; i -= 0.1f) {
			for (int j = 0; j < background_obj_sky.Count; j++) {
				Color bg = background_obj_sky [j].color;
				background_obj_sky [j].color = new Color (bg.r, bg.g, bg.b, i);
				yield return new WaitForEndOfFrame ();
			}
		}
		yield return new WaitForEndOfFrame ();
		gameObject.SetActive (false);
	}
}
