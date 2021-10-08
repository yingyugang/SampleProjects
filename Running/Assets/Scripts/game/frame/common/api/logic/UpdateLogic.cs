using UnityEngine;
using System.Collections.Generic;

public class UpdateLogic : APILogic
{
	public override void UpdateFromData ()
	{
        GameConstant.lastNumOfCard = GameConstant.numOfCard;
        GameConstant.numOfCard = UpdateInformation.GetInstance.card_list.Count;
        SendMessageUpwards (GameConstant.UpdateNoticeManager, 0, SendMessageOptions.DontRequireReceiver);
		SendMessageUpwards (GameConstant.UpdateBadgeManager, SendMessageOptions.DontRequireReceiver);
		SendMessageUpwards (GameConstant.UpdateApRecoveryTime, SendMessageOptions.DontRequireReceiver);
	}
}
