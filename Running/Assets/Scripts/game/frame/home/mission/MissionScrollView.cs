using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using System.Linq;

public class MissionScrollView : RogerScrollView
{
	[HideInInspector]
	public List<MissionScrollItem> missionScrollItemList;
	public UnityAction<MissionScrollItem> unityAction;
	private IEnumerable<MissionCSVStructure> missionCSVStructureEnumerable;
	private IEnumerable<MissionCSVStructure> currentMissionCSVStructureEnumerable;

	public IEnumerator Init (IEnumerable<MissionCSVStructure> missionCSVStructureEnumerable, int maxView = 10)
	{
		yield return new WaitForEndOfFrame ();
		missionScrollItemList = new List<MissionScrollItem> ();
		this.missionCSVStructureEnumerable = missionCSVStructureEnumerable;
		int count = missionCSVStructureEnumerable.Count ();
		numOfBatchData = count;
		FetchData ();
		PrepareScrollView (maxView);
		int length = Mathf.Min (maxView, count);
		for (int i = 0; i < length; i++) {
			yield return StartCoroutine (CreateRogerScrollItem (Vector2.zero));
			missionScrollItemList.Add (rogerScrollItem as MissionScrollItem);
			rogerScrollItem.unityAction = unityActionHandler;
			InitItem (rogerScrollItem as MissionScrollItem, i);
			ResetChildPosition ();
		}
	}

	private void unityActionHandler (RogerScrollItem rogerScrollItem)
	{
		if (unityAction != null) {
			unityAction (rogerScrollItem as MissionScrollItem);
		}
	}

	protected override void FetchData ()
	{
		currentMissionCSVStructureEnumerable = missionCSVStructureEnumerable.Take (fetchTime * numOfBatchData);
		rogerDataGrid.UpdateMaxIndex (currentMissionCSVStructureEnumerable.Count ());
		fetchTime++;
	}

	private void InitItem (MissionScrollItem missionScrollItem, int index)
	{
		missionScrollItem.updateData = (int id, UnityAction<MissionCSVStructure> unityAction) => {
			if (unityAction != null) {
				int count = currentMissionCSVStructureEnumerable.Count ();
				if (count <= id) {
					FetchData ();
				}
				unityAction (currentMissionCSVStructureEnumerable.ElementAtOrDefault (id));

			}
		};
		missionScrollItem.Init (index);
	}
}
