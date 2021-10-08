using UnityEngine;
using System.Collections;

public class CameraResize : MonoBehaviour 
{
	public Renderer target;

	// Use this for initialization
	void Start () 
	{
		Resize();
	}

	void Resize()
	{
		Vector3 size = target.bounds.size;

		float width = size.x;

		float camW = width;
		float camH = camW / Camera.main.aspect;

		Camera.main.orthographicSize = camH / 2;
	}
}
