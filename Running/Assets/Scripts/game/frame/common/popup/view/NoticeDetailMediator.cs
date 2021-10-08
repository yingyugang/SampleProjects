using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NoticeDetailMediator : PopupContentMediator
{
	private NoticeDetail noticeDetail;
	public NoticeCreator noticeCreator;

	private void OnEnable ()
	{
		noticeDetail = popupContent as NoticeDetail;
		noticeCreator.Create (noticeDetail.instantiation, noticeDetail.container, true);
	}
}
