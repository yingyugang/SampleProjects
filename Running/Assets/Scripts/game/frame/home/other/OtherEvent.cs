using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class OtherEvent : ViewWithDefaultAction
{
	public Image eventBanner;
	public Image thumbnailField;
    public Image totalThumbnailField;
	public Text nameField;
	public Text timeField;
	public Transform container;
	public RankingScrollItem instantiation;
	public List<Toggle> toggleList;
	public UnityAction<int> unityAction;
	public SwitchButton switchButton;
	public Text buttonText;
	public GameObject warning;
	public Text warningText;
	private Toggle currentToggle;
	public Text banner;
	public Text tip;
	public Text missionScoreField;
	public Text totalScoreField;
	public Text highScoreField;
	public GameObject missionEventInfo;
	public GameObject rankingEventInfo;
	public GameObject totalRankingEventInfo;
	public GameObject eventRankingScrollView;
	public GameObject eventMissionScrollView;
	public GreyButton gotoGame;
	public Image rewardText;
	public Button rewardButton;

	protected override void AddActions ()
	{
		unityActionArray = new UnityAction[] {
			() => {
				Send (0);
			},
			() => {
				Send (1);
			},
			() => {
				Send (2);
			},
			() => {
				Send (3);
			},
			() => {
				Send (4);
			},
			() => {
				Send (5);
			}
		};
		AddEvents ();
	}

	public void SetRoundAvailable (bool isAvailable)
	{
		buttonArray[1].gameObject.SetActive (isAvailable);
	}

	public void SetRoundActive (bool isActive)
	{
		switchButton.SetActive (isActive);
	}

	public void SetRoundText (string str)
	{
		buttonText.text = str;
	}

	private void AddEvents ()
	{
		int length = toggleList.Count;
		for (int i = 0; i < length; i++)
		{
			toggleList[i].name = i.ToString ();
			toggleList[i].onValueChanged.AddListener (ToggleArrayOnValueChangedHandler);
		}
		currentToggle = toggleList[0];
	}

	public void RemoveEvents ()
	{
		int length = toggleList.Count;
		for (int i = 0; i < length; i++)
		{
			toggleList[i].onValueChanged.RemoveListener (ToggleArrayOnValueChangedHandler);
		}
	}

	protected override void OnDestroy ()
	{
		base.OnDestroy ();
		RemoveEvents ();
	}

	private void ToggleArrayOnValueChangedHandler (bool isSelected)
	{
		if (isSelected)
		{
			Toggle toggle = toggleList.FirstOrDefault (result => result.isOn == true);
			if (currentToggle != toggle)
			{
				ComponentConstant.SOUND_MANAGER.Play (SoundEnum.se01_ok);
				currentToggle = toggle;
				unityAction (int.Parse (toggle.name));
			}
		}
	}

	public void Show (EventInfo eventInfo, string fromTime, string toTime, bool isReward)
	{
		GameCSVStructure gameCSVStructure = MasterCSV.gameCSV.FirstOrDefault (result => result.id == eventInfo.m_game_id);
		if (gameCSVStructure != null)
		{
			timeField.text = string.Format ("{0}{1}", LanguageJP.OBJECT, gameCSVStructure.name);

			List<GameDetail> gameDetailList = UpdateInformation.GetInstance.game_list;
		}
		GameDetail gameDetail = UpdateInformation.GetInstance.game_list.FirstOrDefault (result => result.id == eventInfo.m_game_id);
		if (gameDetail != null)
		{
			gotoGame.SetActive (gameDetail.islock != 1);
		}
		else
		{
			gotoGame.SetActive (false);
		}

		rewardText.gameObject.SetActive (isReward);
		rewardButton.gameObject.SetActive (isReward);

		missionScoreField.text = totalScoreField.text = eventInfo.total_score.ToString ();
		highScoreField.text = eventInfo.high_score.ToString ();
		nameField.text = string.Format ("{0}{1}{2}{3}", LanguageJP.DURING, fromTime, LanguageJP.WAVE, toTime);

		if (eventInfo.eventStatusEnum == EventStatusEnum.OPENED)
		{
			banner.text = LanguageJP.EVENT_OPEN_TITLE;
			tip.text = string.Empty;
			tip.gameObject.SetActive (false);
		}
		else if (eventInfo.eventStatusEnum == EventStatusEnum.CLOSED)
		{
			banner.text = LanguageJP.EVENT_CLOSE_TITLE;
			tip.text = LanguageJP.EVENT_CLOSE_TEXT;
			tip.gameObject.SetActive (true);
		}
		else if (eventInfo.eventStatusEnum == EventStatusEnum.PUBLIC)
		{
			banner.text = LanguageJP.EVENT_PUBLIC_TITLE;
			tip.text = LanguageJP.EVENT_PUBLIC_TEXT;
			tip.gameObject.SetActive (true);
		}
	}
}
