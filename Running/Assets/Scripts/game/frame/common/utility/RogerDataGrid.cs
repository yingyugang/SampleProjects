using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class RogerDataGrid : MonoBehaviour
{
	public UnityAction<Transform,int> onInitializeItem;
	public UnityAction addData;
	public int paddingTop;
	public int paddingBottom;
	private bool isOverFlow;

	public enum ArrangeType
	{
		Horizontal = 0,
		Vertical = 1
	}

	public int cell_x = 100;
	public int cell_y = 100;
	public bool cullContent = true;
	[HideInInspector]
	public int minIndex = 0;
	[HideInInspector]
	public int maxIndex = 0;
	public ArrangeType arrangeType = ArrangeType.Horizontal;
	public int ConstraintCount = 0;
	private Transform mTrans;
	private RectTransform mRTrans;
	private RogerScrollRect mScroll;
	private bool mHorizontal;
	private List<Transform> mChild = new List<Transform> ();
	private float extents = 0;
	private Vector2 SR_size = Vector2.zero;
	private Vector3[] conners = new Vector3[4];
	private Vector2 startPos;
	private int maxView;
	private float maxSize = 0;
	private float minSize;
	private float currentScrollBarValue;
	private float lastScrollBarValue;
	private float currentTime;

	private int SortByName (Transform a, Transform b)
	{
		return string.Compare (a.name, b.name);
	}

	public void InitList (int maxView)
	{
		Clear ();
		InitValue ();
		this.maxView = maxView;
	}

	public void UpdateMinSize(){
		minSize = mRTrans.sizeDelta.y;
	}

	public float UpdateMaxIndex (int numberOfItem)
	{
		maxIndex = numberOfItem;
		maxSize = paddingTop + paddingBottom + ((numberOfItem / ConstraintCount) + (numberOfItem % ConstraintCount == 0?0:1)) * ((arrangeType == ArrangeType.Vertical) ? cell_x : cell_y);
		return maxSize;
	}

	public void Clear ()
	{
		int length = mChild.Count;
		for (int i = 0; i < length; i++) {
			Destroy (mChild [i].gameObject);
		}
		mChild.Clear ();
	}

	public void AddChild (Transform child)
	{
		mChild.Add (child);
	}

	private void InitValue ()
	{
		if (ConstraintCount <= 0)
			ConstraintCount = 1;
		if (minIndex > maxIndex)
			minIndex = maxIndex;
		mTrans = transform;
		mRTrans = transform.GetComponent<RectTransform> ();
		mScroll = transform.parent.parent.GetComponent<RogerScrollRect> ();
		mHorizontal = mScroll.horizontal;
		
		SR_size = transform.parent.GetComponent<RectTransform> ().rect.size;

		conners [0] = new Vector3 (-SR_size.x / 2f, SR_size.y / 2f, 0);
		conners [1] = new Vector3 (SR_size.x / 2f, SR_size.y / 2f, 0);
		conners [2] = new Vector3 (-SR_size.x / 2f, -SR_size.y / 2f, 0);
		conners [3] = new Vector3 (SR_size.x / 2f, -SR_size.y / 2f, 0);
		for (int i = 0; i < 4; i++) { 
			Vector3 temp = transform.parent.TransformPoint (conners [i]);
			conners [i].x = temp.x;
			conners [i].y = temp.y;
		}
		
		mRTrans.pivot = new Vector2 (0, 1);
		
		mScroll.valueChange = WrapContentHandler;

		mScroll.onBeginDrag = OnBeginDragHandler;

		startPos = mTrans.localPosition;
	}

	private void WrapContentHandler ()
	{
		WrapContent ();
	}

	public void ResetChildPosition ()
	{
		int rows = 1, cols = 1;
		
		Vector2 startAxis = new Vector2 (cell_x / 2f, -paddingTop);
		int i;
		int imax = mChild.Count;

		if (arrangeType == ArrangeType.Vertical) { 
			rows = ConstraintCount;
			cols = (int)Mathf.Ceil ((float)imax / (float)rows);
			extents = (float)(cols * cell_x) * 0.5f; 
		} else if (arrangeType == ArrangeType.Horizontal) { 
			cols = ConstraintCount; 
			rows = (int)Mathf.Ceil ((float)imax / (float)cols); 
			extents = (float)(rows * cell_y) * 0.5f; 
		}
		
		for (i = 0; i < imax; i++) {
			Transform temp = mChild [i];
			
			int x = 0, y = 0;
			if (arrangeType == ArrangeType.Horizontal) {
				x = i / cols;
				y = i % cols;
			} else if (arrangeType == ArrangeType.Vertical) {
				x = i % rows;
				y = i / rows;
			}
			
			temp.localPosition = new Vector2 (temp.localPosition.x, startAxis.y - x * cell_y);
			if (minIndex == maxIndex || (i >= minIndex && i <= maxIndex)) {
				UpdateRectsize (temp.localPosition);
				UpdateItem (temp, i, i);
			} else {
				cullContent = false;
				temp.gameObject.SetActive (false);
			}
		}
	}

	public void ResetPosition ()
	{
		mScroll.StopMovement ();
		mTrans.localPosition = startPos;
	}

	public void StopScroll ()
	{
		mScroll.enabled = false;
	}

	public void StartScroll ()
	{
		mScroll.enabled = true;
	}

	public void UpdateRectsize (Vector2 pos)
	{
		if (RogerVerticalScrollBar.HasScrollBar) {
			mRTrans.sizeDelta = new Vector2 (mRTrans.sizeDelta.x, maxSize);
		} else {
			if (arrangeType == ArrangeType.Vertical) {
				mRTrans.sizeDelta = new Vector2 (pos.x + cell_x, ConstraintCount * cell_y);
			} else {
				mRTrans.sizeDelta = new Vector2 (ConstraintCount * cell_x, -pos.y + cell_y + paddingBottom);
			}
		}
	}

	private int getRealIndex (Vector2 pos)
	{
		int x = (int)Mathf.Ceil (-pos.y / cell_y) - 1;
		int y = (int)Mathf.Round (pos.x / cell_x);
		
		int realIndex;
		if (arrangeType == ArrangeType.Horizontal) {
			realIndex = x * ConstraintCount + y;
		} else {
			realIndex = x + ConstraintCount * y;
		}
		
		return realIndex;
	}

	public void WrapContent ()
	{
		if (RogerVerticalScrollBar.IsDragVerticalBar) {
			mTrans.GetComponent<RectTransform> ().anchoredPosition = RogerVerticalScrollBar.CurrentPosition;
		}

		Vector3[] conner_local = new Vector3[4];
		for (int i = 0; i < 4; i++) {
			conner_local [i] = mTrans.InverseTransformPoint (conners [i]);
		}

		Vector2 center = (conner_local [3] + conner_local [0]) / 2f;
		
		if (mHorizontal) {
			float min = conner_local [0].x - cell_x;
			float max = conner_local [3].x + cell_x;
			for (int i = 0, imax = mChild.Count; i < imax; i++) {
				Transform temp = mChild [i];
				float distance = temp.localPosition.x - center.x;
				
				if (distance < -extents * 2) {
					Vector2 pos = temp.localPosition;
					pos.x += extents * 2f;
					int realIndex = getRealIndex (pos);
					if (minIndex == maxIndex || (realIndex >= minIndex && realIndex < maxIndex)) {    
						temp.localPosition = pos;

						UpdateItem (temp, i, realIndex);       
					}
				}
				
				if (distance > extents * 2) {
					Vector2 pos = temp.localPosition;
					pos.x -= extents * 2f;
					
					int realIndex = getRealIndex (pos);
					
					if (minIndex == maxIndex || (realIndex >= minIndex && realIndex < maxIndex)) {      
						temp.localPosition = pos;

						UpdateItem (temp, i, realIndex);                
					} 
				}
				
				if (cullContent) {
					Vector2 pos = temp.localPosition;
					temp.gameObject.SetActive ((pos.x > min && pos.x < max) ? true : false);
				}
				
			}
		} else {
			float min = conner_local [3].y - cell_y;
			float max = conner_local [0].y + cell_y;
			isOverFlow = false;
			for (int i = 0, imax = mChild.Count; i < imax; i++) {
				Transform temp = mChild [i];
				float distance = temp.localPosition.y - center.y;
				
				if (distance < -extents) {
					Vector2 pos = temp.localPosition;
					pos.y += extents * 2f;
					
					int realIndex = getRealIndex (pos);
					if (minIndex == maxIndex || (realIndex >= minIndex && realIndex < maxIndex)) {
						temp.localPosition = pos;
						UpdateItem (temp, i, realIndex);
					}
				}
				if (distance > extents) {
					Vector2 pos = temp.localPosition;
					pos.y -= extents * 2f;
					
					int x = (int)Mathf.Ceil (-pos.y / cell_y) - 1;
					int y = (int)Mathf.Round (pos.x / cell_x);
					
					int realIndex;
					if (arrangeType == ArrangeType.Horizontal) {
						realIndex = x * ConstraintCount + y;
					} else {
						realIndex = x + ConstraintCount * y;
					}
					
					if (minIndex == maxIndex || (realIndex >= minIndex && realIndex < maxIndex)) {
						UpdateRectsize (pos);
						temp.localPosition = pos;
						UpdateItem (temp, i, realIndex);
					}

					if (realIndex >= maxIndex) {
						isOverFlow = true;
					}
				}
				if (cullContent) {
					Vector2 pos = temp.localPosition;
					temp.gameObject.SetActive ((pos.y > min && pos.y < max) ? true : false);
				}
			}
		}
	}

	private void LateUpdate()
	{
		if(RogerVerticalScrollBar.HasScrollBar){
			currentTime += Time.deltaTime;
			if (currentTime > 0.1f) {
				currentTime = 0;
				WrapContent ();
			}
		}
	}

	private void OnBeginDragHandler (PointerEventData eventData)
	{
		if (isOverFlow && eventData.delta.y > 0) {
			if (addData != null) {
				addData ();
			}
			isOverFlow = false;
		}
	}

	private void UpdateItem (Transform item, int index, int realIndex)
	{
		if (onInitializeItem != null) {
			onInitializeItem (item, realIndex);
		}
	}

	private void OnDestroy()
	{
		mScroll.valueChange = null;
		mScroll.onBeginDrag = null;
	}
}
