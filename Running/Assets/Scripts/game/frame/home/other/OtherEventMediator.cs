using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using System.Collections.Generic;
using System.Linq;
using System;
using home;

public class OtherEventMediator : ActivityMediator
{
	private OtherEvent otherEvent;
	private Coroutine currentEnumerator;
	public RankingEventScrollView rankingEventScrollView;
	private int type = 0;
	private string mask = "yyyy/MM/dd";
	private Sprite thumbnailSprite;
	private PopupContentMediator detailPopupContentMediator;
	private EventInfo currentEventInfo;
	private bool isBefore;
	public MissionEventScrollView missionEventScrollView;
	public MissionEventReceiveLogic missionEventReceiveLogic;
	public HeaderMediator headerMediator;
	public GameDetailTopMediator gameDetailTopMediator;
	private PopupContentMediator sendMailPopupContentMediator;

	protected override void CreateActions ()
	{
		otherEvent = viewWithDefaultAction as OtherEvent;
		unityActionArray = new UnityAction[] {
			() => {
				ComponentConstant.SOUND_MANAGER.Play (SoundEnum.se02_ng);
				showWindow (0);
			},
			() => {
				List<EventInfo> eventInfoList = UpdateInformation.GetInstance.event_info_list;
				isBefore = !isBefore;
				currentEventInfo = isBefore ? eventInfoList [1] : eventInfoList [0];
				otherEvent.SetRoundActive (!isBefore);
				otherEvent.SetRoundText (isBefore ? LanguageJP.EVENT_NOW : LanguageJP.EVENT_BEFORE);
				ComponentConstant.SOUND_MANAGER.Play (SoundEnum.se01_ok);
				GetData ();
			},
			() => {
				ComponentConstant.SOUND_MANAGER.Play (SoundEnum.se01_ok);
				EventRuleCSVStructure eventRuleCSVStructure = MasterCSV.eventRuleCSV.FirstOrDefault ();
				page.popupLoader.Popup (PopupEnum.EventBonusReadme, null, new List<object> {
					eventRuleCSVStructure.title,
					eventRuleCSVStructure.description
				});
			},
			() => {
				ComponentConstant.SOUND_MANAGER.Play (SoundEnum.se01_ok);
				detailPopupContentMediator = page.popupLoader.Popup (PopupEnum.EventBonusDetail, null, new List<object> {
					currentEventInfo.event_name,
					otherEvent.thumbnailField.sprite,
					GetEventBonusDetailDataList (),
					thumbnailSprite,
					GetTotalEventBonusDetailDataList (),
					currentEventInfo.hreward_desc,
					currentEventInfo.treward_desc
				});
			},
			() => {
				GotoSpecifiedGame ();
			},
			() => {
				sendMailPopupContentMediator = page.popupLoader.Popup (PopupEnum.EventSendMail, null, new List<object> {
					string.Format ("{0}{1}", LanguageJP.EVENT_MAIL_ID, PlayerPrefs.GetString (LanguageJP.P_CODE, string.Empty)),
					string.Format ("{0}{1}", LanguageJP.EVENT_MAIL_NAME, Player.GetInstance.name)
				});
			}
		};
		otherEvent.unityAction = (int type) => {
			this.type = type;
			InitScrollView ();
		};
	}

	private void GotoSpecifiedGame ()
	{
		ComponentConstant.SOUND_MANAGER.Play (SoundEnum.se01_ok);
		if (gotoSpecifiedGame != null) {
			gameDetailTopMediator.SetWindowByID (currentEventInfo.m_game_id);
			gotoSpecifiedGame ();
		}
	}

	private void InitScrollView ()
	{
		if (type == 0) {
			otherEvent.eventRankingScrollView.SetActive (false);
			otherEvent.eventMissionScrollView.SetActive (true);
			InitMissionScrollView ();
		} else if (type == 1 || type == 2) {
			otherEvent.eventRankingScrollView.SetActive (true);
			otherEvent.eventMissionScrollView.SetActive (false);
			InitEventRankingScrollView (currentEventInfo.id, type);
		}
	}

	private void InitMissionScrollView ()
	{
		int count = currentEventInfo.event_missions.Count;
		otherEvent.warning.SetActive (count == 0);
		otherEvent.warningText.text = LanguageJP.EVENT_WARNING;
		otherEvent.missionEventInfo.SetActive (count > 0);
		otherEvent.rankingEventInfo.SetActive (false);
		otherEvent.totalRankingEventInfo.SetActive (false);
		missionEventScrollView.unityAction = (MissionEventScrollItem missionEventScrollItem) => {
			SendAPI (missionEventScrollItem);
		};
		StartCoroutine (missionEventScrollView.Init (currentEventInfo.total_score, currentEventInfo.event_missions));
	}

	private void SendAPI (MissionEventScrollItem missionEventScrollItem)
	{
		ComponentConstant.SOUND_MANAGER.Play (SoundEnum.se01_ok);
		missionEventReceiveLogic.event_mission_id = missionEventScrollItem.eventMission.id;
		missionEventReceiveLogic.complete = () => {
			missionEventScrollItem.yellow.SetActive (false);
			missionEventScrollItem.button.interactable = false;
			UpdateData ();
		};
		missionEventReceiveLogic.SendAPI ();
	}

	private void UpdateData ()
	{
		headerMediator.UpdateCoinAndMoney ();
		popup (PopupEnum.MissionReceiveComplete);
	}

	private void GetResources (EventInfo eventInfo)
	{
		thumbnailSprite = null;
		otherEvent = viewWithDefaultAction as OtherEvent;
		string bannerImage = eventInfo.banner_image;
		string eventPresentThumbnail = LanguageJP.EVENT_PRESENT_THUMBNAIL + eventInfo.id;
		string eventTotalPresentThumbnail = LanguageJP.EVENT_TOTAL_PRESENT_THUMBNAIL + eventInfo.id;
		StartCoroutine (ComponentConstant.ASSETBUNDLE_RESOURCES_GETTER.GetResourcesList<Texture2D> (AssetBundleName.event_image.ToString (), new List<string> () {
			bannerImage,
			eventPresentThumbnail,
			eventTotalPresentThumbnail
		}, (List<Texture2D> texture2DList) => {
			Dictionary<string, Sprite> dictionary = TextureToSpriteConverter.ConvertToSpriteDictionary (texture2DList);
			otherEvent.eventBanner.sprite = dictionary [bannerImage];
			otherEvent.thumbnailField.sprite = dictionary [eventPresentThumbnail];
			otherEvent.totalThumbnailField.sprite = dictionary [eventTotalPresentThumbnail];
			otherEvent.eventBanner.gameObject.SetActive (true);
			otherEvent.thumbnailField.gameObject.SetActive (true);
			otherEvent.totalThumbnailField.gameObject.SetActive (true);
			if (detailPopupContentMediator != null) {
				EventBonusDetailMediator eventBonusDetailMediator =	detailPopupContentMediator as EventBonusDetailMediator;
				eventBonusDetailMediator.SetRewardImage (otherEvent.thumbnailField.sprite);
				eventBonusDetailMediator.SetTotalRewardImage (otherEvent.totalThumbnailField.sprite); 
			}
			if (eventInfo.treward_start != 0) {
				string sonota = eventInfo.treward_sonota;
				if (!string.IsNullOrEmpty (sonota)) {
					int result = 0;
					if (!int.TryParse (sonota, out result)) {
						thumbnailSprite = otherEvent.totalThumbnailField.sprite;
					}
				}
			}
		}, false));
	}

	private void GetRewardTotalImage (EventInfo eventInfo)
	{
		string eventTotalPresentThumbnail = LanguageJP.EVENT_TOTAL_PRESENT_THUMBNAIL + eventInfo.id;

		StartCoroutine (ComponentConstant.ASSETBUNDLE_RESOURCES_GETTER.GetResource<Texture2D> (AssetBundleName.event_image.ToString (), eventTotalPresentThumbnail, (Texture2D texture2D) => {
			thumbnailSprite = TextureToSpriteConverter.ConvertToSprite (texture2D);
			if (detailPopupContentMediator != null) {
				(detailPopupContentMediator as EventBonusDetailMediator).SetTotalRewardImage (thumbnailSprite);
			}
		}, false));
	}

	private List<EventBonusDetailData> GetTotalEventBonusDetailDataList ()
	{
		List<EventBonusDetailData> totalEventBonusDetailDataList = new List<EventBonusDetailData> ();

		if (currentEventInfo.treward_start != 0) {
			totalEventBonusDetailDataList.Add (new EventBonusDetailData () {
				rank = GetRankString (currentEventInfo.treward_start, currentEventInfo.treward_end),
				bonus = currentEventInfo.treward_title
			});
		}

		if (!string.IsNullOrEmpty (currentEventInfo.treward_sonota)) {
			string[] stringArr = currentEventInfo.treward_sonota.Split (',');
			int length = stringArr.Length;
			for (int i = 0; i < length; i++) {
				totalEventBonusDetailDataList.Add (new EventBonusDetailData () {
					rank = GetSpecifiedRankString (stringArr [i]),
					bonus = currentEventInfo.treward_title
				});
			}
		}
		return totalEventBonusDetailDataList;
	}

	private List<EventBonusDetailData> GetEventBonusDetailDataList ()
	{
		return new List<EventBonusDetailData> () {
			GetEventBonusDetailData (currentEventInfo.rank1_start, currentEventInfo.rank1_end, currentEventInfo.rank1_reward),
			GetEventBonusDetailData (currentEventInfo.rank2_start, currentEventInfo.rank2_end, currentEventInfo.rank2_reward),
			GetEventBonusDetailData (currentEventInfo.rank3_start, currentEventInfo.rank3_end, currentEventInfo.rank3_reward),
			GetEventBonusDetailData (currentEventInfo.rank4_start, currentEventInfo.rank4_end, currentEventInfo.rank4_reward),
			GetEventBonusDetailData (currentEventInfo.rank5_start, currentEventInfo.rank5_end, currentEventInfo.rank5_reward)
		};
	}

	private EventBonusDetailData GetEventBonusDetailData (int rankStart, int rankEnd, string reward)
	{
		if (rankStart == 0) {
			return null;
		} else {
			return new EventBonusDetailData () {
				rank = GetRankString (rankStart, rankEnd),
				bonus = reward
			};
		}
	}

	private string GetSpecifiedRankString (string rank)
	{
		return string.Format ("{0}{1}{2}", rank, LanguageJP.ORDER, LanguageJP.COLON);
	}

	private string GetRankString (int rankStart, int rankEnd)
	{
		if (rankEnd == 0) {
			return string.Format ("{0}{1}{2}", rankStart, LanguageJP.ORDER, LanguageJP.WAVE);
		} else {
			return string.Format ("{0}{1}{2}{3}{4}{5}", rankStart, LanguageJP.ORDER, LanguageJP.WAVE, LanguageJP.BREAK_LINE, rankEnd, LanguageJP.ORDER);
		}
	}

	private void OnEnable ()
	{
		otherEvent = viewWithDefaultAction as OtherEvent;
		isBefore = false;
		List<EventInfo> eventInfoList = UpdateInformation.GetInstance.event_info_list;
		if (GameConstant.eventTypeEnum == EventTypeEnum.ONE) {
			currentEventInfo = eventInfoList [0];
			otherEvent.SetRoundAvailable (false);
		} else if (GameConstant.eventTypeEnum == EventTypeEnum.BOTH) {
			otherEvent.SetRoundAvailable (true);
			otherEvent.SetRoundActive (true);
			otherEvent.SetRoundText (LanguageJP.EVENT_BEFORE);
			currentEventInfo = eventInfoList [0];
		}

		GetData ();
	}

	private void GetData ()
	{
		otherEvent.eventBanner.sprite = null;
		SetWindow ();
		GetResources (currentEventInfo);

		InitScrollView ();
	}

	private void InitEventRankingScrollView (int id, int type)
	{
		otherEvent = viewWithDefaultAction as OtherEvent;
		rankingEventScrollView.numOfList = (int count, int currentType) => {
			otherEvent.warning.SetActive (count == 0);
			if (currentType == 1) {
				otherEvent.warningText.text = LanguageJP.EVENT_WARNING;
				otherEvent.totalRankingEventInfo.SetActive (count > 0);
				otherEvent.rankingEventInfo.SetActive (false);
			} else if (currentType == 2) {
				otherEvent.warningText.text = LanguageJP.EVENT_WARNING_ADDITIONAL + LanguageJP.EVENT_WARNING;
				otherEvent.rankingEventInfo.SetActive (count > 0);
				otherEvent.totalRankingEventInfo.SetActive (false);
			}
			otherEvent.missionEventInfo.SetActive (false);
		};
		StartCoroutine (rankingEventScrollView.Init (id, type, 20));
	}

	private void SetWindow ()
	{
		otherEvent = viewWithDefaultAction as OtherEvent;
		otherEvent.Show (currentEventInfo, TimeUtil.TimestampToString (currentEventInfo.start_at, mask), TimeUtil.TimestampToString (currentEventInfo.end_at, mask), currentEventInfo.reward_on == 1);
	}
}
