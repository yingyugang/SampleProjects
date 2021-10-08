using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using System.Linq;

public class CardScrollView : RogerScrollView
{
	public UnityAction<CardScrollItem> unityAction;
	private IEnumerable<CardCSVStructure> cardCSVStructureEnumerable;
	private IEnumerable<CardCSVStructure> currentCardCSVStructureEnumerable;
	private const int perData = 3;
	public RogerVerticalScrollBar rogerVerticalScrollBar;

	public IEnumerator Init (IEnumerable<CardCSVStructure> cardCSVStructureEnumerable, int maxView = 10)
	{
		ComponentConstant.API_MANAGER.shortLoadingMediator.Show ();
		yield return new WaitForEndOfFrame ();
		this.cardCSVStructureEnumerable = cardCSVStructureEnumerable;
		int count = cardCSVStructureEnumerable.Count ();
		numOfBatchData = count;
		FetchData ();
		PrepareScrollView (maxView);

		List<Card> list = UpdateInformation.GetInstance.card_list;
		int listLength = list.Count;
		int length = Mathf.Min (maxView, count);
		for (int i = 0; i < length; i++) {
			yield return StartCoroutine (CreateRogerScrollItem (new Vector2 ((i % 3) * 307, 0)));
			rogerScrollItem.unityAction = unityActionHandler;
			InitItem (rogerScrollItem as CardScrollItem, i);
			ResetChildPosition ();
		}
		ComponentConstant.API_MANAGER.shortLoadingMediator.Hide ();
		rogerDataGrid.UpdateMinSize ();
	}

	private void unityActionHandler (RogerScrollItem rogerScrollItem)
	{
		if (unityAction != null) {
			unityAction (rogerScrollItem as CardScrollItem);
		}
	}

	protected override void FetchData ()
	{
		currentCardCSVStructureEnumerable = cardCSVStructureEnumerable.Take (fetchTime * numOfBatchData);
		rogerVerticalScrollBar.Init (rogerDataGrid.UpdateMaxIndex (currentCardCSVStructureEnumerable.Count ()));
		fetchTime++;
	}

	private void InitItem (CardScrollItem cardScrollItem, int index)
	{
		cardScrollItem.updateData = (int id, UnityAction<CardCSVStructure> unityAction) => {
			if (unityAction != null) {
				int count = currentCardCSVStructureEnumerable.Count ();
				if (count <= id) {
					FetchData ();
				}
				unityAction (currentCardCSVStructureEnumerable.ElementAtOrDefault (id));
			}
		};
		cardScrollItem.Init (index);
	}
}
