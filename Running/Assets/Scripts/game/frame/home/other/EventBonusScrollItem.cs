using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EventBonusScrollItem : RogerScrollItem
{
	public Text rankField;
	public Text bonusField;
	public Image dot;

	public void Show (string rank, string bonus, bool showDot)
	{
		rankField.text = rank;
		bonusField.text = bonus;
		dot.gameObject.SetActive (false);
	}
}
