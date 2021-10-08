using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Linq;

public class MissionEventScrollItem : MonoBehaviour
{
	public GameObject yellow;
	public GameObject completed;
	public Image reward;
	public Text point;
	public Text title;
	public Text num;
	public GameObject challenage;
	public GameObject arrow;
	public GameObject card;
	public GameObject gold;
	public EventMission eventMission;
	public Image cardImage;
	public Image borderImage;
	public Image rateImage;
	public Button button;
	public UnityAction<MissionEventScrollItem> unityAction;

	public void InitData (int totalScore, EventMission eventMission, bool isFirst, bool isChallenage)
	{
		this.eventMission = eventMission;
		bool isCompleted = totalScore >= eventMission.point;
		
		completed.SetActive (isCompleted);
		challenage.SetActive (isChallenage);
		arrow.SetActive (!isFirst);
		point.text = isCompleted ? string.Format ("{0}{1}{2}", eventMission.point.ToString (), LanguageJP.PT, LanguageJP.BREAK) : string.Format ("{0}{1}", eventMission.point.ToString (), LanguageJP.PT);
		title.text = eventMission.title;
		if (isCompleted && eventMission.has_received == 0)
		{
			yellow.SetActive (true);
			button.enabled = true;
			button.onClick.AddListener (EventMissionButtonOnClickHandler);
		}
		else if (isCompleted && eventMission.has_received == 1)
		{
			button.interactable = false;
			yellow.SetActive (false);
		}
		else
		{
			button.enabled = false;
			yellow.SetActive (false);
		}
		
		LoadImage ();
	}

	private void OnDestroy ()
	{
		button.onClick.RemoveListener (EventMissionButtonOnClickHandler);
	}

	private void EventMissionButtonOnClickHandler ()
	{
		if (unityAction != null)
		{
			unityAction (this);
		}
	}

	private void LoadImage ()
	{
		if (eventMission.item_type == 1)
		{
			card.SetActive (true);
			gold.SetActive (false);
			int m_card_id = eventMission.item_id;
			CardCSVStructure currentCardCSVStructure = MasterCSV.cardCSV.FirstOrDefault (result => result.id == m_card_id);
			borderImage.sprite = AssetBundleResourcesLoader.cardFrameThumbnailDictionary[string.Format ("{0}{1}", currentCardCSVStructure.rarity, LanguageJP.CARD_THUMBNAIL_SUFFIX)];
			rateImage.sprite = AssetBundleResourcesLoader.cardFrameThumbnailDictionary[string.Format ("{0}{1}", currentCardCSVStructure.rarity, LanguageJP.CARD_RATE_SUFFIX)];
			//StartCoroutine (ComponentConstant.ASSETBUNDLE_RESOURCES_GETTER.GetResource<Texture2D> (AssetBundleName.card_thumbnail.ToString (), int.Parse (currentCardCSVStructure.image_resource).ToString (LanguageJP.FOUR_MASK) + LanguageJP.CARD_THUMBNAIL_SUFFIX, GetResource<Texture2D>, false));
			cardImage.sprite = AssetBundleResourcesLoader.cardFrameThumbnailDictionary[GameConstant.UNKNOWN_IMAGE_NAME];
		}
		else if (eventMission.item_type == 2)
		{
			card.SetActive (false);
			gold.SetActive (true);
			int m_item_id = eventMission.item_id;
			ItemCSVStructure currentItemCSVStructure = MasterCSV.itemCSV.FirstOrDefault (result => result.id == m_item_id);
			reward.sprite = AssetBundleResourcesLoader.itemIconDictionary[currentItemCSVStructure.image_resource];
			num.text = string.Format ("{0}{1}", LanguageJP.X, eventMission.num.ToString ());
		}
	}

	private void GetResource<T> (T t)
	{
		cardImage.sprite = TextureToSpriteConverter.ConvertToSprite (t as Texture2D);	
	}
}
