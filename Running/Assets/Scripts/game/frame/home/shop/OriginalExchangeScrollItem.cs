using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Linq;
using System;

public class OriginalExchangeScrollItem : RogerInteractiveScrollItem
{
	[HideInInspector]
	public int m_exchange_id;
	public Text ticketNumberField;
	public Text cardNameField;
	public Image icon;
	private TicketExchange ticketExchange;
	public GameObject mask;

	[HideInInspector]
	public int ticketNumber;
	[HideInInspector]
	public string cardName;

	public void Show (TicketExchange ticketExchange)
	{
		this.ticketExchange = ticketExchange;
		m_exchange_id = ticketExchange.id;
		this.ticketNumber = ticketExchange.need_ticket;
		ticketNumberField.text = string.Format ("{0}{1}", ticketNumber, LanguageJP.M);
		cardName = cardNameField.text = ticketExchange.description;
		CardCSVStructure currentCardCSVStructure = MasterCSV.cardCSV.FirstOrDefault (result => result.id == ticketExchange.m_card_id);
		gameObject.SetActive (true);
		StartCoroutine (ComponentConstant.ASSETBUNDLE_RESOURCES_GETTER.GetResource<Texture2D> (AssetBundleName.card_thumbnail.ToString (), int.Parse (currentCardCSVStructure.image_resource).ToString (LanguageJP.FOUR_MASK) + LanguageJP.CARD_THUMBNAIL_SUFFIX, GetResource<Texture2D>, false));
		bool isMask = UpdateInformation.GetInstance.card_list.FirstOrDefault (result => result.m_card_id == currentCardCSVStructure.id) != null;
		mask.SetActive (isMask);
		button.enabled = !isMask;
	}

	private void GetResource<T> (T t)
	{
		icon.sprite = TextureToSpriteConverter.ConvertToSprite (t as Texture2D);
	}
}
