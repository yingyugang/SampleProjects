using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SupportScrollItem : RogerInteractiveScrollItem
{
	public Text titleField;

	public void Show (string title)
	{
		titleField.text = title;
	}
}
