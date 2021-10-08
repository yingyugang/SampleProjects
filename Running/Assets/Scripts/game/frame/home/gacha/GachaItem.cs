using System;
using UnityEngine;

[Serializable]
public class GachaItem
{
	public int id;
	public bool isNew;
	[Range (1, 5)]
	public int ball_color_show = 1;
	[Range (1, 5)]
	public int ball_color = 1;
	//public bool isUp;
	//[Range (1, 5)]
	//public int itemType = 0;
	public CardItem cardItem;
}