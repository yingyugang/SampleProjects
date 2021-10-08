using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ExchangeScrollItem : RogerInteractiveScrollItem
{
	[HideInInspector]
	public int m_exchange_id;
	public Text coinNumberField;
	public Text ticketNumberField;
	[HideInInspector]
	public int coinNumber;
	[HideInInspector]
	public int ticketNumber;

	public void Show (int id, int coinNumber, int ticketNumber)
	{
		m_exchange_id = id;
		this.coinNumber = coinNumber;
		this.ticketNumber = ticketNumber;
		coinNumberField.text = string.Format ("{0}{1}", coinNumber, LanguageJP.M);
		ticketNumberField.text = string.Format ("{0}{1}", ticketNumber, LanguageJP.M);
		gameObject.SetActive (true);
	}
}
