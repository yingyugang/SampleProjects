using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using System.Linq;

public class RankingEventScrollView : RogerScrollView
{
	[HideInInspector]
	public List<RankingScrollItem> rankingScrollItemList;
	public UnityAction<RankingScrollItem> unityAction;
	private List<RankingData> currentRankingDataList;
	public RankingEventLogic rankingEventLogic;
	public UnityAction<int, int> numOfList;
	private int id;
	private int maxView;
	private bool isFetching;
	private bool isFirst;
	public Transform container;
	private int type;

	public IEnumerator Init (int id, int type, int maxView = 10)
	{
		yield return new WaitForEndOfFrame ();
		RogerContainerCleaner.Clean (container);
		this.type = type;
		fetchTime = 0;
		this.id = id;
		this.maxView = maxView;
		rankingScrollItemList = new List<RankingScrollItem> ();
		currentRankingDataList = new List<RankingData> ();
		numOfBatchData = 100;
		isFirst = true;
		PrepareScrollView (maxView);
		FetchData ();
	}

	private void SendAPI ()
	{
		rankingEventLogic.complete = () =>
		{
			isFetching = false;
			fetchTime++;
			currentRankingDataList.AddRange (UpdateInformation.GetInstance.ranking_list);
			int need = fetchTime * numOfBatchData;
			int count = currentRankingDataList.Count;
			if (isFirst)
			{
				if (numOfList != null)
				{
					numOfList (count, type);
				}
				StartCoroutine (CreateScrollItem ());
				isFirst = false;
			}
			rogerDataGrid.UpdateMaxIndex (count > need ? need : count);
			EnableDataGrid ();
		};
		rankingEventLogic.error = (string status) =>
		{
			isFetching = false;
		};
		rankingEventLogic.event_id = id;
		rankingEventLogic.pageno = fetchTime;
		rankingEventLogic.mode = type;
		UpdateInformation.GetInstance.ranking_list = null;
		rankingEventLogic.SendAPI ();
	}

	private IEnumerator CreateScrollItem ()
	{
		yield return StartCoroutine (ComponentConstant.ASSETBUNDLE_RESOURCES_GETTER.GetAssetBundle (AssetBundleName.card_thumbnail.ToString (), null, false));

		int count = currentRankingDataList.Count ();
		int length = Mathf.Min (maxView, count);
		for (int i = 0; i < length; i++)
		{
			yield return StartCoroutine (CreateRogerScrollItem (Vector2.zero));
			rankingScrollItemList.Add (rogerScrollItem as RankingScrollItem);
			rogerScrollItem.unityAction = unityActionHandler;
			InitItem (rogerScrollItem as RankingScrollItem, i);
			ResetChildPosition ();
		}
	}

	private void unityActionHandler (RogerScrollItem rogerScrollItem)
	{
		if (unityAction != null)
		{
			unityAction (rogerScrollItem as RankingScrollItem);
		}
	}

	protected override void FetchData ()
	{
		if (isFirst || !isFetching && fetchTime < UpdateInformation.GetInstance.total_page)
		{
			isFetching = true;
			SendAPI ();
			DisableDataGrid ();
		}
	}

	private void DisableDataGrid ()
	{
		rogerDataGrid.StopScroll ();
	}

	private void EnableDataGrid ()
	{
		rogerDataGrid.WrapContent ();
		rogerDataGrid.StartScroll ();
	}

	private void InitItem (RankingScrollItem rankingScrollItem, int index)
	{
		rankingScrollItem.updateData = (int id, UnityAction<RankingData> unityAction) =>
		{
			if (unityAction != null)
			{
				int count = currentRankingDataList.Count ();
				if (count < id)
				{
					FetchData ();
				}
				else {
					unityAction (currentRankingDataList.ElementAtOrDefault (id));
				}
			}
		};
		rankingScrollItem.Init (index);
	}

	private void OnDisable ()
	{
		rogerDataGrid.Clear ();
	}
}
