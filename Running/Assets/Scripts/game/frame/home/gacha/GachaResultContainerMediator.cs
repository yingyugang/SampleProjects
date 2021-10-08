using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Events;

public class GachaResultContainerMediator : ActivityMediator
{
	public GachaResultMediator gachaResultMediator;
	public GachaAdditionalMediator gachaAdditionalMediator;
	public GachaOriginalTicketMediator gachaOriginalTicketMediator;
	public UnityAction<int> unityAction;
	private GachaResultContainer gachaResultContainer;
	private List<GachaItem> gachaItemList;

	private void SetGacha ()
	{
		gachaResultContainer = viewWithDefaultAction as GachaResultContainer;
		gachaResultContainer.contentRectTransform.anchoredPosition = Vector2.zero;
		if (CheckAdditional () || CheckOriginalTicket ()) {
			gachaResultContainer.normalButtonGroup.SetActive (false);
			gachaResultContainer.nextButtonGroup.SetActive (true);
		} else {
			gachaResultContainer.normalButtonGroup.SetActive (true);
			gachaResultContainer.nextButtonGroup.SetActive (false);
		}
		gachaResultMediator.SetWindow (gachaItemList);
		gachaResultMediator.gameObject.SetActive (true);
		gachaAdditionalMediator.gameObject.SetActive (false);
		gachaOriginalTicketMediator.gameObject.SetActive (false);
	}

	private bool CheckAdditional (bool checkValue = false)
	{
		bool isShow = false;
		int length = gachaItemList.Count;
		for (int i = 0; i < length; i++) {
			GachaItem gachaItem = gachaItemList [i];
			if (gachaItem.isNew) {
				CardCSVStructure cardCSVStructure = MasterCSV.cardCSV.FirstOrDefault (result => result.id == gachaItem.id);
				float up_value = cardCSVStructure.up_value;
				if (up_value != 0) {
					isShow = true;
					if (checkValue) {
						CardRate.SetAdditionalRate (cardCSVStructure.up_game_id, up_value);
					}
				}
			}
		}
		return isShow;
	}

	private bool CheckOriginalTicket ()
	{
		return GachaResultInfo.GetInstance.original_ticket > 0;
	}

	public void SetWindow (List<GachaItem> list, bool isNoMore)
	{
		gachaItemList = list;
		gachaResultContainer = viewWithDefaultAction as GachaResultContainer;
		gachaResultContainer.gachaButton1.SetActive (!isNoMore);
		gachaResultContainer.gachaButton2.SetActive (!isNoMore);
		gachaResultContainer.gachaButton3.SetActive (!isNoMore);
		SetGacha ();
	}

	protected override void CreateActions ()
	{
		unityActionArray = new UnityAction[] {
			() => {
				unityAction (0);
				gachaResultMediator.Clean ();
			},
			() => {
				unityAction (1);
				gachaResultMediator.Clean ();
			},
			() => {
				if (CheckAdditional (true)) {
					gachaResultMediator.gameObject.SetActive (false);
					gachaAdditionalMediator.gameObject.SetActive (true);
					gachaOriginalTicketMediator.gameObject.SetActive (false);
					gachaResultMediator.Clean ();

					if (CheckOriginalTicket ()) {
						gachaResultContainer.additionalButtonGroup.SetActive (false);
						gachaResultContainer.additionNextButtonGroup.SetActive (true);
					} else {
						gachaResultContainer.additionalButtonGroup.SetActive (true);
						gachaResultContainer.additionNextButtonGroup.SetActive (false);
					}
				} else if (CheckOriginalTicket ()) {
					ShowOriginal ();
				}
			},
			() => {
				unityAction (0);
				gachaResultMediator.Clean ();
			},
			() => {
				unityAction (1);
				gachaResultMediator.Clean ();
			},
			() => {
				ShowOriginal ();
			},
			() => {
				unityAction (0);
				gachaResultMediator.Clean ();
			},
			() => {
				unityAction (1);
				gachaResultMediator.Clean ();
			},
			() => {
				
			}
		};
	}

	private void ShowOriginal ()
	{
		gachaResultMediator.gameObject.SetActive (false);
		gachaAdditionalMediator.gameObject.SetActive (false);
		if (GameConstant.CurrentGachaType == 2) {
			gachaOriginalTicketMediator.SetTitle (LanguageJP.ORIGINAL_TICKET_TITLE_TEXT1);
		} else {
			gachaOriginalTicketMediator.SetTitle (LanguageJP.ORIGINAL_TICKET_TITLE_TEXT2);
		}

		gachaOriginalTicketMediator.gameObject.SetActive (true);
		gachaResultMediator.Clean ();

		gachaResultContainer.originalButtonGroup.SetActive (true);
		gachaResultContainer.originalNextButtonGroup.SetActive (false);
	}
}
