using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class NoticeScrollItem : RogerInteractiveScrollItem
{
	[HideInInspector]
	public int id;
	public Text titleField;
	public Text descriptionField;
	public Text timeField;
	public LayoutElement imageLayoutElement;
	private string linkUrl;
	public Image imageField;
	public NetTextureLoader netTextureLoader;
	private bool isPopup;
	private const int MAX_WIDTH = 800;
	private const int MAX_WIDTH_POPUP = 660;

	public void Show (int id, string title, string description, int time, string imageUrl, float imageWidth, float ImageHeight, string linkUrl, bool isPopup)
	{
		this.id = id;
		this.isPopup = isPopup;
		titleField.text = title;
		descriptionField.text = description;
		this.linkUrl = linkUrl;
		button.gameObject.SetActive (!string.IsNullOrEmpty (linkUrl));
		timeField.text = GetTime (time);
		gameObject.SetActive (true);
		if (!string.IsNullOrEmpty (imageUrl)) {
			GetImage (imageUrl, imageWidth);
		} else {
			imageField.gameObject.SetActive (false);
		}
	}

	private void GetImage (string image_url, float imageWidth)
	{
		StartCoroutine (netTextureLoader.Load (image_url, (Texture2D texture2D) => {
			imageField.sprite = TextureToSpriteConverter.ConvertToSprite (texture2D);
			imageField.SetNativeSize ();
			Vector2 size = imageField.rectTransform.sizeDelta;
			float ratio = size.x / size.y;
			imageField.gameObject.SetActive (true);

			if (isPopup) {
				if (imageWidth >= MAX_WIDTH_POPUP) {
					imageWidth = MAX_WIDTH_POPUP;
				}
			} else {
				if (imageWidth >= MAX_WIDTH) {
					imageWidth = MAX_WIDTH;
				}
			}
			imageLayoutElement.minWidth = imageLayoutElement.preferredWidth = imageWidth;
			imageLayoutElement.minHeight = imageLayoutElement.preferredHeight = imageWidth / ratio;
		}));
	}

	override protected void ButtonOnClickHandler ()
	{
		ComponentConstant.SOUND_MANAGER.Play (SoundEnum.se01_ok);
		Application.OpenURL (linkUrl);
	}

	private string GetTime (int time)
	{
		DateTime dateTime = TimeUtil.TimestampToDateTime (time);
		return string.Format ("{0}{1}{2}{3}{4}", TimeUtil.TimeStampToDateString (dateTime), " ", LanguageJP.LEFT_BRACKET, TimeUtil.DateToDay (dateTime), LanguageJP.RIGHT_BRACKET);
	}
}
