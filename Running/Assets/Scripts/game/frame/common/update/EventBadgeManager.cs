using UnityEngine;
using System.Collections;

public class EventBadgeManager : MonoBehaviour
{
	public GameObject eventBadge;

	public void ShowEventBadge ()
	{
		if (eventBadge != null) {
			eventBadge.SetActive (true);
		}
	}

	public void HideEventBadge ()
	{
		if (eventBadge != null) {
			eventBadge.SetActive (false);
		}
	}
}
