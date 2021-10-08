using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.Events;
using System.Linq;

public class MissionScrollItem : RogerInteractiveScrollItem
{
	public UnityAction<int,UnityAction<MissionCSVStructure>> updateData;
	public UnityAction<MissionCSVStructure> callBack;
	public Text missionName;
	public Text num;
	public Text clearNum;
	public Image thumbnail;
	public List<GameObject> stars;
	public GameObject yellow;
	public GameObject completed;
	public GameObject locked;
	public MissionCSVStructure missionCSVStructure;
	private int clear_num;
	private bool isLock;
	public Image reward;

	private void InitData (MissionCSVStructure missionCSVStructure)
	{
		this.missionCSVStructure = missionCSVStructure;
		missionName.text = missionCSVStructure.name;
		num.text = "X" + missionCSVStructure.num;

		int length = missionCSVStructure.difficulty;
		for (int j = 0; j < 6; j++) {
			stars [j].SetActive (false);
		}
		for (int i = 0; i < length; i++) {
			stars [i].SetActive (true);
		}

		if (missionCSVStructure.mission.status == 1) {
			ShowNoGetCompletion ();
		} else if (missionCSVStructure.mission.status == 2) {
			ShowNormalCompletion ();
		} else {
			isLock = CheckIfLock ();
			if (isLock) {
				ShowLocked ();
			} else {
				ShowNormal ();
			}
		}
		ShowNumOfCompleted ();

		if (missionCSVStructure.reward_type == 1) {
			int m_card_id = missionCSVStructure.reward_id;
			CardCSVStructure currentCardCSVStructure = MasterCSV.cardCSV.FirstOrDefault (result => result.id == m_card_id);
			StartCoroutine (ComponentConstant.ASSETBUNDLE_RESOURCES_GETTER.GetResource<Texture2D> (AssetBundleName.card_thumbnail.ToString (), int.Parse (currentCardCSVStructure.image_resource).ToString (LanguageJP.FOUR_MASK) + LanguageJP.CARD_THUMBNAIL_SUFFIX, GetResource<Texture2D>, false));
		} else if (missionCSVStructure.reward_type == 2) {
			int m_item_id = missionCSVStructure.reward_id;
			ItemCSVStructure currentItemCSVStructure = MasterCSV.itemCSV.FirstOrDefault (result => result.id == m_item_id);
			reward.sprite = AssetBundleResourcesLoader.itemIconDictionary [currentItemCSVStructure.image_resource];
		}

		thumbnail.sprite = AssetBundleResourcesLoader.gameIconDictionary [missionCSVStructure.image_resource];
		thumbnail.gameObject.SetActive (true);
	}

	private void ShowNumOfCompleted ()
	{
		MissionClearInfo missionClearInfo = UpdateInformation.GetInstance.mission_clear_info_list.FirstOrDefault (result => result.m_mission_id == missionCSVStructure.id);
		if (missionClearInfo != null) {
			clear_num = missionClearInfo.clear_num;
			if (!isLock) {
				clearNum.text = string.Format ("{0}{1}{2}{3}{4}", LanguageJP.PINK_COLOR_PREFIX, clear_num, LanguageJP.PERSON, LanguageJP.COLOR_SUFFIX, LanguageJP.COMPLETE);
			}
			clearNum.gameObject.SetActive (!isLock);
		} else {
			clearNum.gameObject.SetActive (false);
		}
	}

	private bool CheckIfLock ()
	{
		if (missionCSVStructure.mission_type > 0) {
			GameDetail gameDetail = UpdateInformation.GetInstance.game_list.FirstOrDefault (result => missionCSVStructure.mission_type == result.id);
			if (gameDetail != null) {
				return gameDetail.islock == 1;
			}
		}
		return false;
	}

	private void ShowNoGetCompletion ()
	{
		button.enabled = true;
		yellow.SetActive (true);
		completed.SetActive (true);
		locked.SetActive (false);
	}

	public void ShowNormalCompletion ()
	{
		button.enabled = false;
		missionCSVStructure.mission.status = missionCSVStructure.mission.status = 2;
		yellow.SetActive (false);
		completed.SetActive (true);
		locked.SetActive (false);
	}

	private void ShowLocked ()
	{
		button.enabled = false;
		yellow.SetActive (false);
		completed.SetActive (false);
		locked.SetActive (true);
	}

	private void ShowNormal ()
	{
		button.enabled = false;
		yellow.SetActive (false);
		completed.SetActive (false);
		locked.SetActive (false);
	}

	protected override void UpdateData (int index)
	{
		if (updateData != null) {
			updateData (index, InitData);
		}
	}

	protected override void OnDestroy ()
	{
		base.OnDestroy ();
		updateData = null;
		callBack = null;
	}

	private void GetResource<T> (T t)
	{
		reward.sprite = TextureToSpriteConverter.ConvertToSprite (t as Texture2D);
		reward.gameObject.SetActive (true);
	}
}
