using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using UnityEngine.UI;
using System.Linq;
using System.Collections.Generic;

public class GachaCardItem : MonoBehaviour
{
	public UnityAction<GachaItem> unityAction;

	public Image cardImage;
	public Image borderImage;
	public Image newImage;
	public Button sound;
	public Button button;
	public GachaItem gachaItem;
	public Button skill;
	private CardCSVStructure cardCSVStructure;

	public void SetData (GachaItem gachaItem, CardCSVStructure cardCSVStructure)
	{
		this.cardCSVStructure = cardCSVStructure;
		this.gachaItem = gachaItem;
		AddEventListener ();
		if (CheatController.GetInstance() != null) {
			skill.gameObject.SetActive (CheatController.GetInstance ().cheatCardIDList.Contains (cardCSVStructure.id));
		}
		cardImage.sprite = gachaItem.cardItem.bigCardSprite;
		cardImage.gameObject.SetActive (true);
		if (AssetBundleResourcesLoader.cardFrameDetailList == null) {
			gameObject.SetActive (true);
			StartCoroutine (ComponentConstant.ASSETBUNDLE_RESOURCES_GETTER.GetAllResourcesList<Texture2D> (AssetBundleName.card_detail_frame.ToString (), (List<Texture2D> list) => {
				AssetBundleResourcesLoader.cardFrameDetailList = TextureToSpriteConverter.ConvertToSpriteList (list);
				borderImage.sprite = gachaItem.cardItem.bigBorderSprite = AssetBundleResourcesLoader.cardFrameDetailList [gachaItem.cardItem.rarity - 1];
			}, false));
		} else {
			borderImage.sprite = gachaItem.cardItem.bigBorderSprite = AssetBundleResourcesLoader.cardFrameDetailList [gachaItem.cardItem.rarity - 1];
		}
		newImage.gameObject.SetActive (gachaItem.isNew);
	}

	private void OnDestroy ()
	{
		RemoveEventListener ();
	}

	private void RemoveEventListener ()
	{
		button.onClick.RemoveListener (ButtonOnClickHandler);
		button.onClick = null;
	}

	private void AddEventListener ()
	{
		button.onClick.AddListener (ButtonOnClickHandler);
		skill.onClick.AddListener (SkillOnClickHandler);
	}

	private void SkillOnClickHandler ()
	{
		List<int> cardIDList = CheatController.GetInstance ().cheatCardIDList.Where (result => result == cardCSVStructure.id).ToList ();
		ComponentConstant.POPUP_LOADER.Popup (PopupEnum.CardSkillDetail, null, new List<object> {
			cardIDList
		});
	}

	private void ButtonOnClickHandler ()
	{
		if (unityAction != null) {
			unityAction (gachaItem);
		}
	}
}
