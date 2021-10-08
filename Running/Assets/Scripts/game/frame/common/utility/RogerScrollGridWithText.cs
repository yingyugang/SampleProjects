using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using System.Linq;

public class RogerScrollGridWithText : RogerScrollGrid
{
	private int length;
	private Instantiator instantiator;
	public Text text;
	private List<GachaCardItem> gachaCardItemList;
	public GachaCardItem myInstantiation;
	public UnityAction<GachaItem> onClick;
	public UnityAction<GachaItem> onSelect;
	public GameObject number;

	override protected void Start ()
	{
		
	}

	public void Init (List<GachaItem> list)
	{
		rogerScrollRect.enabled = false;
		RemoveEventListeners ();
		itemCount = list.Count;
		StartCoroutine (CreateItems (list));
		eachPiece = itemCount * 2 - 2;
		CheckIsAuto ();
	}

	private IEnumerator CreateItems (List<GachaItem> list)
	{
		Instantiator instantiator = Instantiator.GetInstance ();
		gachaCardItemList = new List<GachaCardItem> ();
		for (int i = 0; i < itemCount; i++) {
			GachaCardItem gachaCardItem = instantiator.Instantiate<GachaCardItem> (myInstantiation, Vector2.zero, Vector3.one, container);
			gachaCardItem.unityAction = (GachaItem gachaItem) => {
				if (onClick != null) {
					onClick (gachaItem);
				}
			};
			CardCSVStructure cardCSVStructure = MasterCSV.cardCSV.FirstOrDefault (result => result.image_resource == list [i].cardItem.card_image_resource);
			yield return StartCoroutine (ComponentConstant.ASSETBUNDLE_RESOURCES_GETTER.GetResource (cardCSVStructure.assetbundle_name.ToString (), int.Parse (cardCSVStructure.image_resource).ToString (LanguageJP.FOUR_MASK), (Texture2D texture2D) => {
				list [i].cardItem.bigCardSprite = TextureToSpriteConverter.ConvertToSprite (texture2D);
			}, false));
			gachaCardItem.SetData (list [i], cardCSVStructure);
			gachaCardItemList.Add (gachaCardItem);
		}
		rogerScrollRect.enabled = true;
		AddEventListeners ();
		CheckArrows ();
		ChangeText ();

		if (initComplete != null) {
			initComplete ();
		}
	}

	override public void Reset ()
	{
		base.Reset ();
		text.text = string.Empty;
		RectTransform rectTransform = container.GetComponent<RectTransform> ();
		rectTransform.sizeDelta = rectTransform.anchoredPosition = Vector2.zero;
	}

	protected override void Moving (int direction)
	{
		base.Moving (direction);
		ChangeText ();
	}

	private void ChangeText ()
	{
		text.text = string.Format ("{0}{1}{2}", (currentRealIndex + 1).ToString (LanguageJP.TWO_MASK), LanguageJP.DEVIDE, itemCount.ToString (LanguageJP.TWO_MASK));
		if (onSelect != null) {
			onSelect (gachaCardItemList [currentRealIndex].gachaItem);
		}
	}
}