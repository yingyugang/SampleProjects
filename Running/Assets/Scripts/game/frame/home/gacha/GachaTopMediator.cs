using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class GachaTopMediator : ActivityMediator
{
	public GachaDetailTopMediator gachaDetailTopMediator;
	public GachaRecycleTopMediator gachaRecycleTopMediator;
	private GachaTop gachaTop;
	public GachaListLogic gachaListLogic;
	private string changeContent;

	private void OnEnable ()
	{
		SendMessageUpwards (GameConstant.ClearNoticeManager, NoticeManager.GACHA, SendMessageOptions.DontRequireReceiver);
		gachaTop = viewWithDefaultAction as GachaTop;
		RogerContainerCleaner.Clean (gachaTop.container);
		SendAPI ();
	}

	private void SendAPI ()
	{
		gachaListLogic.complete = () => {
			ShowGachaList ();
		};
		gachaListLogic.SendAPI ();
	}

	private void ShowGachaList ()
	{
		gachaTop = viewWithDefaultAction as GachaTop;
		gachaTop.OpenChangeTimeText ();
		CreateGachaMenuItems ();
	}

	public void CloseChangeTime ()
	{
		gachaTop = viewWithDefaultAction as GachaTop;
		gachaTop.CloseChangeTimeText ();
	}

	private void CreateGachaMenuItems ()
	{
		int length = UpdateInformation.GetInstance.gacha_list.Count;
		Instantiator instantiator = Instantiator.GetInstance ();
		for (int i = 0; i < length; i++) {
			Gacha gacha = UpdateInformation.GetInstance.gacha_list [i];
			GachaMenuItemMediator gachaMenuItemMediator = instantiator.Instantiate (gachaTop.gachaMenuItemMediator, Vector2.zero, Vector3.one, gachaTop.container);
			gachaMenuItemMediator.InitData (i, gacha);
			gachaMenuItemMediator.sendOrder = (int id, Sprite sprite) => {
				ComponentConstant.SOUND_MANAGER.Play (SoundEnum.se01_ok);
				if (gacha.gacha_type != 3) {
					SetGachaDetailTopWindow (gacha, sprite);
					showWindow (1);
				} else {
					SetGachaRecycleTopWindow (gacha);
					showWindow (4);
				}
			};
			gachaMenuItemMediator.gameObject.SetActive (true);
		}
	}

	private void SetGachaDetailTopWindow (Gacha gacha, Sprite sprite)
	{
		gachaDetailTopMediator.SetWindow (gacha, sprite);
	}

	private void SetGachaRecycleTopWindow (Gacha gacha)
	{
		gachaRecycleTopMediator.SetWindow (gacha);
	}
}
