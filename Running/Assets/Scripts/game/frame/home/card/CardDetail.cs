using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using UnityEngine.UI;

public class CardDetail : ViewWithDefaultAction
{
	public Image cardImage;
	public Image cardFrameImage;
	public Text numberField;
	public Text nameField;
	public Text titleField;
	public Text descriptionField;
	public Button skillButton;
	public Text additionalField;
	public Image gameIcon;
	public GameObject cardSkill;
	public RectTransform cardRectTransform;
	public GameObject printButton;

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
}
