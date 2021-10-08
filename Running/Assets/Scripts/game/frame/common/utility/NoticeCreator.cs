using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NoticeCreator : MonoBehaviour
{
	public void Create (NoticeScrollItem instantiation, Transform container, bool isPopup = false)
	{
		List<Information> informationList = UpdateInformation.GetInstance.information_list;
		int length = informationList.Count;

		for (int i = 0; i < length; i++) {
			NoticeScrollItem noticeScrollItem = Instantiator.GetInstance ().Instantiate (instantiation, Vector2.zero, Vector3.one, container);
			Information information = informationList [i];
			noticeScrollItem.Show (information.id, information.title, information.description, information.start_at, information.image_url, information.width, information.high, information.link_url, isPopup);
		}
	}
}
