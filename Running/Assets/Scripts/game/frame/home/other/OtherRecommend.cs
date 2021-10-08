using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class OtherRecommend : ViewWithDefaultAction
{
	public Transform container;
	public RecommendScrollItem instantiation;

	protected override void AddActions ()
	{
		unityActionArray = new UnityAction[] {
			() => {
				Send (0);
			}
		};
	}
}
