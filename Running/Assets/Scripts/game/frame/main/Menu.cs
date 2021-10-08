using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Events;

public class Menu : ViewWithDefaultAction
{
	public Page page;
	public Image bg;
	public Image start;
	public Image title;
	public Image credit;
	public Text id;
	public Text ver;

	private const string BG = "bg";
	private const string START = "start";
	private const string TITLE = "title";
	private const string CREDIT = "credit";

	public MainMediator mainMediator;

	private void OnEnable ()
	{
		bg.sprite = mainMediator.menuDictionary [BG];
		start.sprite = mainMediator.menuDictionary [START];
		title.sprite = mainMediator.menuDictionary [TITLE];
		credit.sprite = mainMediator.menuDictionary [CREDIT];
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
			}
		};
	}
}
