using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScaleableFullScreenImage : MonoBehaviour
{
	public RectTransform canvasTransform;
	public RectTransform rectTransform;

	private void Start ()
	{
		float screenRatio = rectTransform.sizeDelta.x / (float)rectTransform.sizeDelta.y;
		rectTransform.sizeDelta = new Vector2 (canvasTransform.sizeDelta.x, canvasTransform.sizeDelta.x / screenRatio);
	}
}
