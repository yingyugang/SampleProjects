using UnityEngine;
using System.Collections;

public class Game_Global_Camera : MonoBehaviour {
	void Awake(){
		Camera m_camera = GetComponent<Camera> ();
		float w = m_camera.pixelWidth;
		float h = m_camera.pixelHeight;
		float target = 1080.0f / 1920.0f;
		float ratio = w / h;

		if (Mathf.Abs (target - ratio) < 0.01) {
			Debug.Log ("No need to resize screen");
			return;
		}
		float adjust = target / ratio;

		Rect m_rect = m_camera.rect;
		m_rect.width = adjust;
		m_rect.height = 1.0f;
		m_rect.x = (1.0f - adjust) / 2.0f;
		m_rect.y = 0;
		m_camera.rect = m_rect;
	}
}
