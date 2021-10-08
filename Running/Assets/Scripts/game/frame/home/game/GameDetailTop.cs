using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using UnityEngine.UI;
using home;
using System.Collections.Generic;

public class GameDetailTop : ViewWithDefaultAction
{
	public Text title;
	public Image banner;
	public Text point;
	public Rank rank;
	public Text clearGacha;
	public HeaderMediator header;
	public List<GameEventItem> gameEventItemList;
	public GameObject leaf;
	public Image gameIcon;
	public Text gameIconText;
	public Text warningText;

	public void Show (string title, int pointValue, int rankValue, int ratio, int clearGacha, Sprite sprite, Sprite icon, float rate)
	{
		this.title.text = title;
		this.point.text = string.Format ("{0}{1}", pointValue, LanguageJP.POINT);
		rank.SetRank (rankValue);
		this.clearGacha.text = string.Format ("{0}{1}{2}{3}{4}", LanguageJP.GAME_CLEAR_GACHA, LanguageJP.PURPLISH_COLOR_PREFIX, clearGacha, LanguageJP.COLOR_SUFFIX, LanguageJP.ROUND);
		banner.sprite = sprite;
		gameIcon.sprite = icon;
		gameIconText.text = string.Format ("{0}{1}{2}", LanguageJP.PLUS, rate, LanguageJP.PERSENTAGE);
		warningText.text = string.Format (LanguageJP.GAME_PT_WARNING, ratio);
	}

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
			},
			() => {
				Send (5);
			}
		};
	}
}