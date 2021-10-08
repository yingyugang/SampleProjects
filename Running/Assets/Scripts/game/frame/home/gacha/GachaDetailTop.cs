using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using UnityEngine.UI;
using System;
using home;

public class GachaDetailTop : ViewWithDefaultAction
{
	public Text titleField;
	public Image imageField;
	public Text probabilityField;
	public GameObject gacha1;
	public GameObject gacha10;
	public Text gacha1NameField;
	public Text gacha10NameField;
	public Text gacha1CostField;
	public Text gacha10CostField;
	public HeaderMediator header;
	public GameObject coin1;
	public GameObject coin10;
	public GameObject gold1;
	public GameObject gold10;
	public GameObject ad;
	public GameObject normalPoint;
	public GameObject originalPoint;
	public Text originalNameField;
	public Text originalCostField;
	public Text originalOwnField;
	public GameObject originalTicket10Times;
	public Text originalTicketText10Times;
	public GameObject originalTicket;
	public Text originalTicketText;
	public GameObject originalText10Times;
	public GameObject originalText;

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
			}
		};
	}

	public void ShowNormal (Gacha gacha, Sprite sprite)
	{
		SetCommonField (gacha, sprite);

		int gacha1Cost = int.Parse (gacha.single_cost);
		int gacha10Cost = gacha.multi_cost;
		bool isCoin = gacha.gacha_type == 2;
		gacha1NameField.text = gacha10NameField.text = isCoin ? LanguageJP.GAME_COIN_NAME : LanguageJP.GAME_TICKET_NAME;
		gacha1.SetActive (gacha1Cost != -1);
		gacha1CostField.text = string.Format ("X{0}{1}{2}{3}", LanguageJP.PINK_COLOR_PREFIX, gacha1Cost.ToString (), LanguageJP.COLOR_SUFFIX, LanguageJP.Gacha1Time);
		gacha10.SetActive (gacha10Cost != -1);
		gacha10CostField.text = string.Format ("X{0}{1}{2}{3}", LanguageJP.PINK_COLOR_PREFIX, gacha10Cost.ToString (), LanguageJP.COLOR_SUFFIX, LanguageJP.Gacha10Times);
		coin1.SetActive (isCoin);
		coin10.SetActive (isCoin);
		gold1.SetActive (isCoin);
		gold10.SetActive (isCoin);
		originalTicketText10Times.text = string.Format ("{0}{1}", gacha.original_ticket, LanguageJP.M);
		bool isShow = isCoin && gacha.original_ticket > 0;
		originalTicket10Times.SetActive (isShow);
		originalText10Times.SetActive (isShow);
	}

	public void ShowOriginal (Gacha gacha, Sprite sprite)
	{
		SetCommonField (gacha, sprite);
		int cost = int.Parse (gacha.single_cost);
		originalNameField.text = LanguageJP.GAME_ORIGINAL_COIN_NAME;
		originalCostField.text = string.Format ("X{0}{1}{2}{3}", LanguageJP.PINK_COLOR_PREFIX, cost.ToString (), LanguageJP.COLOR_SUFFIX, LanguageJP.Gacha1Time);
		originalOwnField.text = PlayerItems.GetInstance.original_point.ToString ();
		originalTicketText.text = string.Format ("{0}{1}", gacha.original_ticket, LanguageJP.M);
		bool isShow = gacha.original_ticket > 0;
		originalTicket.SetActive (isShow);
		originalText.SetActive (isShow);
	}

	private void OnEnable ()
	{
		originalOwnField.text = PlayerItems.GetInstance.original_point.ToString ();
	}

	private void SetCommonField (Gacha gacha, Sprite sprite)
	{
		titleField.text = gacha.title;
		imageField.sprite = sprite;
		probabilityField.text = gacha.card_desc;
	}
}
