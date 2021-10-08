using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;

public class EventBonusDetailMediator : PopupContentActivityMediator
{
	private EventBonusDetail eventBonusDetail;
	public EventBonusScrollItem instantiation;

	private void OnEnable ()
	{
		eventBonusDetail = popupContent as EventBonusDetail;
		eventBonusDetail.eventName.text = objectList[0].ToString ();
		
		SetDescription (objectList[5],eventBonusDetail.rewardDescription);
		SetDescription (objectList[6],eventBonusDetail.rewardTotalDescription);

		CreateReward ();
		CreateTotalReward ();
	}

	private void SetDescription (object obj, Text text)
	{
		if (obj != null)
		{
			string descriptionTotal = obj.ToString ();
            if (!string.IsNullOrEmpty (descriptionTotal))
            {
                text.text = descriptionTotal;
                text.gameObject.SetActive (true);
            }
            else
            {
                text.gameObject.SetActive (false);
            }
        }
		else
		{
			text.gameObject.SetActive (false);
		}
	}

	private void CreateReward ()
	{
		SetRewardImage (objectList[1] as Sprite);
		List<EventBonusDetailData> list = objectList[2] as List<EventBonusDetailData>;
		list = list.Where (result => result != null).ToList ();
		int length = list.Count;
		Instantiator instantiator = Instantiator.GetInstance ();
		for (int i = 0; i < length; i++)
		{
			EventBonusScrollItem eventBonusScrollItem = instantiator.Instantiate (instantiation, Vector2.zero, Vector3.one, eventBonusDetail.rewardContainer);
			eventBonusScrollItem.Show (list[i].rank, list[i].bonus, i != length - 1);
			eventBonusScrollItem.gameObject.SetActive (true);
		}
	}

	private void CreateTotalReward ()
	{
		Sprite thumbnail = objectList[3] as Sprite;
		if (thumbnail == null)
		{
			eventBonusDetail.rewardTotal.SetActive (false);
			return;
		}
		SetTotalRewardImage (thumbnail);
		List<EventBonusDetailData> list = objectList[4] as List<EventBonusDetailData>;
		int length = list.Count;
		Instantiator instantiator = Instantiator.GetInstance ();
		for (int i = 0; i < length; i++)
		{
			EventBonusScrollItem eventBonusScrollItem = instantiator.Instantiate (instantiation, Vector2.zero, Vector3.one, eventBonusDetail.rewardTotalContainer);
			eventBonusScrollItem.Show (list[i].rank, list[i].bonus, i != length - 1);
			eventBonusScrollItem.gameObject.SetActive (true);
		}
		eventBonusDetail.rewardTotal.SetActive (true);
	}

	public void SetRewardImage (Sprite rewardThumbnail)
	{
		eventBonusDetail.rewardThumbnail.sprite = rewardThumbnail;
    }

	public void SetTotalRewardImage (Sprite rewardTotalThumbnail)
	{
		eventBonusDetail.rewardTotalThumbnail.sprite = rewardTotalThumbnail;
	}
}
