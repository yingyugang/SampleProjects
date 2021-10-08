using UnityEngine;
using System.Collections;

public enum FollowAnchorPosDirection
{
	Top,
	Right,
	Left,
	Bottom
}

public class FollowAnchorPosAnimation : MonoBehaviour
{
	public RectTransform canvasRectTransform;
	public RectTransform targetRectTransform;
	public RectTransform rectTransform;
	public FollowAnchorPosDirection followAnchorPosDirection;

	private void Update ()
	{
		float x = targetRectTransform.sizeDelta.x;
		float y = targetRectTransform.sizeDelta.y;
		rectTransform.sizeDelta = canvasRectTransform.sizeDelta;
		switch (followAnchorPosDirection) {
		case FollowAnchorPosDirection.Top:
			rectTransform.anchoredPosition = new Vector2 (0, (targetRectTransform.sizeDelta.y - rectTransform.sizeDelta.y) / 2);
			break;
		case FollowAnchorPosDirection.Right:
			rectTransform.anchoredPosition = new Vector2 ((targetRectTransform.sizeDelta.x - rectTransform.sizeDelta.x) / 2, 0);
			break;
		case FollowAnchorPosDirection.Bottom:
			rectTransform.anchoredPosition = new Vector2 (0, (rectTransform.sizeDelta.y - targetRectTransform.sizeDelta.y) / 2);
			break;
		case FollowAnchorPosDirection.Left:
			rectTransform.anchoredPosition = new Vector2 ((rectTransform.sizeDelta.x - targetRectTransform.sizeDelta.x) / 2, 0);
			break;
		}

	}
}
