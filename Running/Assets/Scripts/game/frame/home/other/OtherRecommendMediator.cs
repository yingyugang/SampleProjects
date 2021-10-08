using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using System.Collections.Generic;

public class OtherRecommendMediator : ActivityMediator
{
	private OtherRecommend otherRecommend;

	protected override void CreateActions ()
	{
		unityActionArray = new UnityAction[] {
			() => {
				ComponentConstant.SOUND_MANAGER.Play (SoundEnum.se02_ng);
				showWindow (0);
			}
		};
	}

	protected override void InitData ()
	{
		otherRecommend = viewWithDefaultAction as OtherRecommend;
		List<Recommend> recommendList = UpdateInformation.GetInstance.recommend_list;
		int length = recommendList.Count;

		for (int i = 0; i < length; i++) {
			RecommendScrollItem recommendScrollItem = Instantiator.GetInstance ().Instantiate (otherRecommend.instantiation, Vector2.zero, Vector3.one, otherRecommend.container);
			Recommend recommend = recommendList [i];
			recommendScrollItem.unityAction = (RogerScrollItem rogerScrollItem) => {
				Application.OpenURL ((rogerScrollItem as RecommendScrollItem).url);		
			};
			recommendScrollItem.Show (recommend.id, recommend.title, recommend.description, recommend.image_resource_name, recommend.url);
		}
	}
}
