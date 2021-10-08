using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class GachaDetailMediator : ActivityMediator
{
	private Gacha gacha;
	private GachaDetail gachaDetail;

	protected override void CreateActions ()
	{
		unityActionArray = new UnityAction[] {
			() => {
				ComponentConstant.SOUND_MANAGER.Play (SoundEnum.se02_ng);
				showWindow ((gacha.gacha_type != 3) ? 1 : 5);
			}
		};
	}

	public void SetWindow (Gacha gacha, Sprite sprite)
	{
		this.gacha = gacha;
		gachaDetail = viewWithDefaultAction as GachaDetail;
		gachaDetail.image.sprite = sprite;
		gachaDetail.Show (gacha.detail_title, gacha.detail_desc);
	}
}
