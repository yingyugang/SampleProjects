using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Marquee : MonoBehaviour
{
	public Text text;
	private float width;
	private const int startPos = 970;

	private void Start ()
	{
		width = text.preferredWidth;
		text.rectTransform.anchoredPosition = new Vector2 (startPos, text.rectTransform.anchoredPosition.y);
	}

	private void Update ()
	{
		Vector2 anchoredPosition = text.rectTransform.anchoredPosition;
		text.rectTransform.anchoredPosition = new Vector2 (anchoredPosition.x - 4, anchoredPosition.y);
		if (text.rectTransform.anchoredPosition.x <= -width) {
			text.rectTransform.anchoredPosition = new Vector2 (startPos, anchoredPosition.y);
		}
	}
}
