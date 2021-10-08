using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class CardItem
{
	public Sprite cardSprite;
	public Sprite borderSprite;
	public Sprite rateSprite;
	public string card_image_resource;
	public int rarity;
	public Sprite bigCardSprite;
	public Sprite bigBorderSprite;

	public void Clean ()
	{
		cardSprite = null;
		borderSprite = null;
		rateSprite = null;
		bigCardSprite = null;
		bigBorderSprite = null;
	}
}
