using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class CardTopMediator : ActivityMediator
{
	private bool isSelectMode;
	private CardTop cardTop;
	private int current = 0;
	private int total = 0;
	private Sprite selectSprite;
	private Sprite frameSprite;

	public CardScrollView cardScrollView;
	private Coroutine currentEnumerator;
	public UpdatePlayerHeadImageLogic updatePlayerHeadImageLogic;
	public CardDetailMediator cardDetailMediator;
	private const string IMAGE_NAME = "selector";
	private RectTransform selectorRectTransform;
	public GameObject selector;
	public bool hasInit;

	private void OnEnable ()
	{
		isSelectMode = false;
		CreateCardSelector ();
		CardRate.GetTotalForAll ();
		float[] array = CardRate.cardTotalArray;
		int length = array.Length;
		for (int i = 0; i < length; i++)
		{
			if (i != 0)
			{
				cardTop.textList[i].text = string.Format ("{0}{1}{2}", LanguageJP.PLUS, array[i], LanguageJP.PERSENTAGE);
			}
			else
			{
				cardTop.textList[i].text = string.Format ("{0}{1}{2}", LanguageJP.MINUS, array[i], LanguageJP.MINUTE);
			}
		}
		SendMessageUpwards (GameConstant.ClearNoticeManager, NoticeManager.CARD, SendMessageOptions.DontRequireReceiver);
		SetMode ();
		if (!hasInit)
		{
			currentEnumerator = StartCoroutine (cardScrollView.Init (MasterCSV.cardCSV, 30));
		}
	}

	private void CreateCardSelector ()
	{
		cardTop = viewWithDefaultAction as CardTop;
		cardTop.selector = Instantiator.GetInstance ().InstantiateGameObject (selector, Vector2.zero, Vector3.one, transform).GetComponent<Image> ();
		cardTop.selector.gameObject.SetActive (false);
		selectorRectTransform = cardTop.selector.GetComponent<RectTransform> ();
		selectorRectTransform.anchoredPosition = new Vector2 (1080, 1920);
		cardTop.selector.sprite = AssetBundleResourcesLoader.cardFrameThumbnailDictionary[IMAGE_NAME];
	}

	protected override void InitData ()
	{
		cardScrollView.unityAction = (CardScrollItem cardScrollItem) =>
		{
			DealWithData (cardScrollItem);
		};
	}

	private void OnDisable ()
	{
		cardTop = viewWithDefaultAction as CardTop;
		if (cardTop.selector != null)
		{
			Destroy (cardTop.selector.gameObject);
		}
	}

	protected override void CreateActions ()
	{
		cardTop = viewWithDefaultAction as CardTop;

		unityActionArray = new UnityAction[] {
			() => {
				ComponentConstant.SOUND_MANAGER.Play (SoundEnum.se01_ok);
				PopupContentMediator popupContentMediator = page.popupLoader.Popup (PopupEnum.CardSelectFilter);
				(popupContentMediator as SelectFilterMediator).unityAction = (List<int> list1, List<int> list2,List<int> list3) => {
					IEnumerable<CardCSVStructure> cardCSVStructureEnumerable = null;
					cardCSVStructureEnumerable = MasterCSV.cardCSV.Where (result => list2.Intersect (result.GetCharacterType).Count () > 0);
					cardCSVStructureEnumerable = cardCSVStructureEnumerable.Where (result => list1.Contains (result.rarity));
					if (list3.Count != MasterCSV.gameCSV.Count() + 1) {
						if (list3.Count > 0 && list3[0] == 1){
							list3[0] = 0;
						}
						cardCSVStructureEnumerable = cardCSVStructureEnumerable.Where (result => list3.Contains (result.up_game_id + 1));	
					}
					StopCoroutine (currentEnumerator);
					currentEnumerator = StartCoroutine (cardScrollView.Init (cardCSVStructureEnumerable, 30));
				};
			},
			() => {
				ComponentConstant.SOUND_MANAGER.Play (SoundEnum.se01_ok);
				ChangeMode ();
			}
		};
	}

	private void DealWithData (CardScrollItem cardScrollItem)
	{
		Button button = cardScrollItem.button;
		if (isSelectMode)
		{
			selectSprite = cardScrollItem.image.sprite;
			frameSprite = cardScrollItem.frame.sprite;
			SetSelector (button.transform);
			SendAPI (cardScrollItem.cardCSVStructure.id);
		}
		else {
			SetSelector (transform);
			hasInit = true;
			showWindow (1);
			cardDetailMediator.SetWindow (cardScrollItem.cardCSVStructure);
		}
	}

	private void SendAPI (int m_card_id)
	{
		updatePlayerHeadImageLogic.m_card_id = m_card_id;
		updatePlayerHeadImageLogic.complete = () => {
            GameConstant.currentHeadImageID = m_card_id;
			CardCSVStructure cardCSVStructure = MasterCSV.cardCSV.FirstOrDefault (result => result.id == m_card_id);
			Sprite rate = AssetBundleResourcesLoader.cardFrameThumbnailDictionary[string.Format ("{0}{1}", cardCSVStructure.rarity, LanguageJP.CARD_RATE_SUFFIX)];
			cardTop.header.SetHeaderImage (selectSprite, frameSprite, rate);
		};
		updatePlayerHeadImageLogic.SendAPI ();
	}

	private void SetSelector (Transform t)
	{
		cardTop = viewWithDefaultAction as CardTop;
		cardTop.selector.transform.SetParent (t);
		selectorRectTransform = cardTop.selector.GetComponent<RectTransform> ();
		if (isSelectMode)
		{
			selectorRectTransform.anchoredPosition = Vector2.zero;
		}
		else {
			selectorRectTransform.anchoredPosition = new Vector2 (1080, 1920);
		}
	}

	private void ChangeMode ()
	{
		isSelectMode = !isSelectMode;
		SetMode ();
	}

	private void SetMode ()
	{
		cardTop = viewWithDefaultAction as CardTop;
		current = UpdateInformation.GetInstance.card_list.Count;
		total = MasterCSV.cardCSV.Count ();
		cardTop.select.SetActive (isSelectMode);
		if (isSelectMode)
		{
			cardTop.content.text = LanguageJP.CardContent;
			cardTop.content.rectTransform.anchoredPosition = new Vector2 (442, 16);
			cardTop.selector.gameObject.SetActive (true);
		}
		else {
			cardTop.content.text = string.Format ("{0}{1}{2}{3}{4}{5}{6}", LanguageJP.PINK_COLOR_PREFIX, LanguageJP.SIZE_64_PREFIX, current, LanguageJP.SIZE_SUFFIX, LanguageJP.COLOR_SUFFIX, LanguageJP.DEVIDE, total);
			cardTop.content.rectTransform.anchoredPosition = new Vector2 (442, 30);
			cardTop.selector.gameObject.SetActive (false);
		}
	}
}
