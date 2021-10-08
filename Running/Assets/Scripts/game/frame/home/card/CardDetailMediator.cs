using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using System.Collections.Generic;
using System.Linq;
using home;

public class CardDetailMediator : ActivityMediator
{
	private CardDetail cardDetail;
	private CardCSVStructure cardCSVStructure;
	public IssueCodeLogic issueCodeLogic;
	private int modeID;
	private string modeStr;
	public HeaderMediator header;

	protected override void CreateActions ()
	{
		unityActionArray = new UnityAction[] {
			() => {
				ComponentConstant.SOUND_MANAGER.Play (SoundEnum.se02_ng);
				showWindow (0);
			},
			() => {
				Zoom ();
			},
			() => {
				Zoom ();
			},
			() => {
				List<int> cardIDList = CheatController.GetInstance ().cheatCardIDList.Where (result => result == cardCSVStructure.id).ToList ();
				ComponentConstant.POPUP_LOADER.Popup (PopupEnum.CardSkillDetail, null, new List<object> {
					cardIDList
				});
			},
			() => {
				Card card = UpdateInformation.GetInstance.card_list.FirstOrDefault (result => cardCSVStructure.id == result.m_card_id);
				PopupContentMediator popupContentMediator = page.popupLoader.Popup (PopupEnum.PrintShow, null, new List<object>{ card });
				(popupContentMediator as PrintShowMediator).unityAction = (int modeID) => {
					if (modeID != 3) {
						this.modeID = modeID;
						if ((string.IsNullOrEmpty(card.codel) && modeID == 1) || (string.IsNullOrEmpty(card.code2l) && modeID == 2)) {
							SendAPI ();
						}else{
							int own = Player.GetInstance.ticket_num;
							int cost = ((modeID == 1) ? UpdateInformation.GetInstance.issue_1l_value : UpdateInformation.GetInstance.issue_2l_value);
							if (own >= cost) {
								popupContentMediator = page.popupLoader.Popup (PopupEnum.PrintAgain, null, new List<object> {
									own,
									cost,
									modeID
								});
								(popupContentMediator as PrintAgainMediator).unityAction = () => {
									SendAPI ();
								};
							} else {
								popupContentMediator = page.popupLoader.Popup (PopupEnum.PrintAgainNoEnough, null, new List<object> {
									own,
									cost,
									modeID
								});
								(popupContentMediator as PrintAgainNoEnoughMediator).unityAction = () => {
									gotoShop ();
								};
							}
						}
					} else {
						popupContentMediator = page.popupLoader.Popup (PopupEnum.PrintReadme);
					}
				};
			}
		};
	}

	private void SendAPI ()
	{
		issueCodeLogic.m_card_id = cardCSVStructure.id;
		issueCodeLogic.mode = modeID;
		issueCodeLogic.complete = () => {
			header.UpdateCoinAndMoney ();
			Card card = UpdateInformation.GetInstance.card_list.FirstOrDefault (result => cardCSVStructure.id == result.m_card_id);
			string code = ((modeID == 1) ? card.codel : card.code2l);
			page.popupLoader.Popup (PopupEnum.PrintResult, null, new List<object>{ code, modeStr, modeID });
		};
		issueCodeLogic.SendAPI ();
	}

	private void Zoom ()
	{
		ComponentConstant.SOUND_MANAGER.Play (SoundEnum.se01_ok);
		ComponentConstant.POPUP_LOADER.Popup (PopupEnum.CardShow, null, new List<object> () {
			cardDetail.cardImage.sprite,
			cardDetail.cardFrameImage.sprite
		});
	}

	private void OnEnable ()
	{
		cardDetail = viewWithDefaultAction as CardDetail;
		cardDetail.buttonArray [1].enabled = false;
		cardDetail.cardRectTransform.anchoredPosition = Vector2.zero;
	}

	public void SetWindow (CardCSVStructure cardCSVStructure)
	{
		this.cardCSVStructure = cardCSVStructure;
		cardDetail = viewWithDefaultAction as CardDetail;
		StartCoroutine (ComponentConstant.ASSETBUNDLE_RESOURCES_GETTER.GetResource<Texture2D> (cardCSVStructure.assetbundle_name.ToString (), int.Parse (cardCSVStructure.image_resource).ToString (LanguageJP.FOUR_MASK), GetResource<Texture2D>, false));
		cardDetail.cardFrameImage.sprite = AssetBundleResourcesLoader.cardFrameDetailList [cardCSVStructure.rarity - 1];
		cardDetail.numberField.text = cardCSVStructure.number.ToString ();
		cardDetail.nameField.text = cardCSVStructure.name;
		cardDetail.titleField.text = cardCSVStructure.title;
		cardDetail.descriptionField.text = cardCSVStructure.description;
		cardDetail.skillButton.gameObject.SetActive (CheatController.GetInstance ().cheatCardIDList.Contains (cardCSVStructure.id));
		if (cardCSVStructure.up_type == 0) {
			cardDetail.cardSkill.SetActive (false);
		} else if (cardCSVStructure.up_type == 1) {
			cardDetail.additionalField.text = string.Format (LanguageJP.CARD_DETAIL_TIME_DESCRIPTION, cardCSVStructure.up_value);
			cardDetail.gameIcon.sprite = AssetBundleResourcesLoader.gameIconDictionary [LanguageJP.TIME_ICON];
			cardDetail.gameIcon.gameObject.SetActive (true);
			cardDetail.gameObject.SetActive (true);
			cardDetail.cardSkill.SetActive (true);
		} else if (cardCSVStructure.up_type == 2) {
			GameCSVStructure gameCSVStructure = MasterCSV.gameCSV.FirstOrDefault (result => result.id == cardCSVStructure.up_game_id);
			cardDetail.additionalField.text = string.Format (LanguageJP.CARD_DETAIL_CARD_DESCRIPTION, gameCSVStructure.name, cardCSVStructure.up_value);
			cardDetail.gameIcon.sprite = AssetBundleResourcesLoader.gameIconDictionary [string.Format ("{0}{1}", LanguageJP.GAME_ICON_PREFIX, cardCSVStructure.up_game_id)];
			cardDetail.gameIcon.gameObject.SetActive (true);
			cardDetail.gameObject.SetActive (true);
			cardDetail.cardSkill.SetActive (true);
		}
		cardDetail.printButton.SetActive (cardCSVStructure.can_print == 1);
	}

	private void GetResource<T> (T t)
	{
		cardDetail.buttonArray [1].enabled = true;
		cardDetail.cardImage.sprite = TextureToSpriteConverter.ConvertToSprite (t as Texture2D);
	}

	private void OnDisable ()
	{
		cardDetail.cardImage.sprite = null;
		cardDetail.cardFrameImage.sprite = null;
	}
}
