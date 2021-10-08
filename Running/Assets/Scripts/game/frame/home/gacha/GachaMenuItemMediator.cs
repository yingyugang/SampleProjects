using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class GachaMenuItemMediator : ActionMediator
{
	private GachaMenuItem gachaMenuItem;
	public UnityAction<int,Sprite> sendOrder;
	private int order;

	protected override void CreateActions ()
	{
		unityActionArray = new UnityAction[] {
			() => {
				sendOrder (order, gachaMenuItem.image.sprite);
			}
		};
	}

	public void InitData (int i, Gacha gacha)
	{
		order = i;
		gachaMenuItem = viewWithDefaultAction as GachaMenuItem;
		gachaMenuItem.descriptionList [gacha.gacha_type - 1].SetActive (true);
		gachaMenuItem.image.sprite = AssetBundleResourcesLoader.gachaBannerDictionary [gacha.image_resource];
	}
}
