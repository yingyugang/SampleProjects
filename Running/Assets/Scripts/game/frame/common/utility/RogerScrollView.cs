using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

public class RogerScrollView : MonoBehaviour
{
	protected RogerScrollItem rogerScrollItem;
	public RogerDataGrid rogerDataGrid;
	public RogerScrollItem instantiation;
	protected int numOfBatchData = 5;
	protected int fetchTime = 1;

	virtual protected void PrepareScrollView (int maxView)
	{
		RectTransform rectTransform = rogerDataGrid.GetComponent<RectTransform> ();
		rectTransform.anchoredPosition = new Vector2 (rectTransform.anchoredPosition.x, 0);
		AddListeners ();
		rogerDataGrid.InitList (maxView);
	}

	private void UpdateRogerScrollItem (Transform item, int realIndex)
	{
		SetIndex (item, realIndex);
	}

	private void SetIndex (Transform item, int realIndex)
	{
		item.GetComponent<RogerScrollItem> ().CurrentIndex = realIndex;
	}

	virtual protected IEnumerator CreateRogerScrollItem (Vector2 vector2)
	{
		rogerScrollItem = Instantiator.GetInstance ().Instantiate (instantiation, vector2, Vector3.one, rogerDataGrid.transform);
		rogerDataGrid.AddChild (rogerScrollItem.transform);
		yield return null;
	}

	virtual protected void ResetChildPosition ()
	{
		rogerDataGrid.ResetChildPosition ();
	}

	protected void RemoveListeners ()
	{
		rogerDataGrid.onInitializeItem = null;
		rogerDataGrid.addData = null;
	}

	protected void AddListeners ()
	{
		rogerDataGrid.onInitializeItem = UpdateRogerScrollItem;
		rogerDataGrid.addData = FetchData;
	}

	virtual protected void FetchData ()
	{
		
	}
}
