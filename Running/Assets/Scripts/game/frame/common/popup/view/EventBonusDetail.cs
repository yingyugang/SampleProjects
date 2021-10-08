using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using UnityEngine.UI;

public class EventBonusDetail : PopupContentWithDefaultAction
{
	public Text eventName;
	public Image rewardThumbnail;
	public Transform rewardContainer;
	public Image rewardTotalThumbnail;
	public Transform rewardTotalContainer;
	public GameObject rewardTotal;
	public Text rewardDescription;
	public Text rewardTotalDescription;

	private void OnDisable ()
	{
		RogerContainerCleaner.Clean (rewardContainer);
		RogerContainerCleaner.Clean (rewardTotalContainer);
	}
}
