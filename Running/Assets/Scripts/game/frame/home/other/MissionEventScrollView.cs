using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

public class MissionEventScrollView : MonoBehaviour
{
	public Transform container;
	public MissionEventScrollItem instantiation;
	private int length;
	private int totalScore;
	private bool isChallenage;
	private bool isFirstTime;
	public UnityAction<MissionEventScrollItem> unityAction;

	public IEnumerator Init (int totalScore, List<EventMission> list)
	{
		RogerContainerCleaner.Clean (container);
		this.totalScore = totalScore;
		length = list.Count;
		isFirstTime = true;
		for (int i = 0; i < length; i++)
		{
			yield return StartCoroutine (CreateMissionEventScrollItem (list[i], i));
		}
	}

	private void OnDisable ()
	{
		RogerContainerCleaner.Clean (container);
	}

	private IEnumerator CreateMissionEventScrollItem (EventMission eventMission, int id)
	{
		MissionEventScrollItem missionEventScrollItem = Instantiator.GetInstance ().Instantiate (instantiation, Vector2.zero, Vector3.one, container);
		missionEventScrollItem.unityAction = (MissionEventScrollItem currentMissionEventScrollItem) =>
		{
			if (unityAction != null)
			{
				unityAction (currentMissionEventScrollItem);
			}
		};
		if (isFirstTime && totalScore < eventMission.point)
		{
			isChallenage = true;
			isFirstTime = false;
		}
		else
		{
			isChallenage = false;
		}

		missionEventScrollItem.gameObject.SetActive (true);
		missionEventScrollItem.InitData (totalScore, eventMission, id == 0, isChallenage);
		yield return null;
	}
}
