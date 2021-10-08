using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class RogerScrollGrid : MonoBehaviour
{
	public Button ArrowLeft;
	public Button ArrowRight;
	public RogerScrollRect rogerScrollRect;
	private Coroutine coroutine;
	protected float itemCount;
	private const int INTERVAL = 2;
	private bool isMoving;
	protected float targetValue;
	public float moveSpeed;
	protected float currentIndex;
	protected float eachPiece;
	public bool isAuto;
	public Transform container;
	public Image instantiation;
	public UnityAction<int,Image> sendCurrentIndex;
	public UnityAction initComplete;
	private List<Image> imageList;
	protected int currentRealIndex;

	virtual protected void Start ()
	{
		AddEventListeners ();
	}

	virtual public void Init (List<Sprite> list)
	{
		RogerContainerCleaner.Clean (container);
		rogerScrollRect.enabled = false;
		ShowOrHideArrow (false);
		itemCount = list.Count;
		StartCoroutine (CreateImages (list));
		eachPiece = itemCount * 2 - 2;
		CheckIsAuto ();
	}

	protected void CheckIsAuto ()
	{
		if (isAuto) {
			coroutine = StartCoroutine (AutoScroll ());
		}
	}

	private IEnumerator CreateImages (List<Sprite> list)
	{
		Instantiator instantiator = Instantiator.GetInstance ();
		imageList = new List<Image> ();
		for (int i = 0; i < itemCount; i++) {
			Image image = instantiator.Instantiate<Image> (instantiation, Vector2.zero, Vector3.one, container);
			image.sprite = list [i];
			image.gameObject.SetActive (true);
			imageList.Add (image);
			yield return null;
		}
		CheckArrows ();
		rogerScrollRect.enabled = true;
		if (initComplete != null) {
			initComplete ();
		}
	}

	protected void AddEventListeners ()
	{
		rogerScrollRect.onBeginDrag = RogerScrollRectOnBeginDragHandler;
		rogerScrollRect.onEndDrag = RogerScrollRectOnEndDragHandler;
		ArrowLeft.onClick.AddListener (ArrowLeftOnClickHandler);
		ArrowRight.onClick.AddListener (ArrowRightOnClickHandler);
	}

	protected void RemoveEventListeners ()
	{
		rogerScrollRect.onBeginDrag = null;
		rogerScrollRect.onEndDrag = null;
		ArrowLeft.onClick.RemoveListener (ArrowLeftOnClickHandler);
		ArrowRight.onClick.RemoveListener (ArrowRightOnClickHandler);
	}

	private void RogerScrollRectOnBeginDragHandler (PointerEventData eventData)
	{
		isMoving = false;
		if (coroutine != null) {
			StopCoroutine (coroutine);
		}
	}

	private void RogerScrollRectOnEndDragHandler (PointerEventData eventData)
	{
		float max = 1 / eachPiece * (currentIndex + 1);
		float min = 1 / eachPiece * (currentIndex - 1);
		float offset = (1 / eachPiece) / 1.1f;
		if (rogerScrollRect.horizontalNormalizedPosition > max - offset && max < 1) {
			Moving (1);
		} else if (rogerScrollRect.horizontalNormalizedPosition < min + offset && min > 0) {
			Moving (-1);
		} else {
			Moving (0);
		}
	}

	virtual protected void Moving (int direction)
	{
		if (direction == 1) {
			currentIndex += 1;
			targetValue = 1 / eachPiece * (currentIndex + 1);
			currentIndex += 1;
		} else if (direction == -1) {
			currentIndex -= 1;
			targetValue = 1 / eachPiece * (currentIndex - 1);
			currentIndex -= 1;
		} else {
			targetValue = 1 / eachPiece * currentIndex;
		}
		currentRealIndex = (int)currentIndex / 2;
		CheckArrows ();
		isMoving = true;
		if (sendCurrentIndex != null) {
			sendCurrentIndex (currentRealIndex, imageList [currentRealIndex]);
		}
	}

	protected void CheckArrows ()
	{
		if (itemCount > 1) {
			ShowOrHideArrow (true);
			if (currentIndex <= 0) {
				ArrowLeft.gameObject.SetActive (false);
			} else if (currentIndex >= itemCount * 2 - 2) {
				ArrowRight.gameObject.SetActive (false);
			}
		} else {
			ShowOrHideArrow (false);
		}
	}

	protected void ShowOrHideArrow (bool isShow)
	{
		ArrowLeft.gameObject.SetActive (isShow);
		ArrowRight.gameObject.SetActive (isShow);
	}

	private void Update ()
	{
		if (isMoving) {
			rogerScrollRect.horizontalNormalizedPosition = Mathf.Lerp (rogerScrollRect.horizontalNormalizedPosition, targetValue, moveSpeed / 10);
		}
	}

	private void ArrowLeftOnClickHandler ()
	{
		ComponentConstant.SOUND_MANAGER.Play (SoundEnum.se01_ok);
		if (coroutine != null) {
			StopCoroutine (coroutine);
		}
		Moving (-1);
	}

	private void ArrowRightOnClickHandler ()
	{
		ComponentConstant.SOUND_MANAGER.Play (SoundEnum.se01_ok);
		if (coroutine != null) {
			StopCoroutine (coroutine);
		}
		Moving (1);
	}

	private void OnDestroy ()
	{
		RemoveEventListeners ();
		sendCurrentIndex = null;
		initComplete = null;
		if (coroutine != null) {
			StopCoroutine (coroutine);
		}
	}

	public IEnumerator AutoScroll ()
	{
		for (int i = 0; i < itemCount - 1; i++) {
			yield return new WaitForSeconds (INTERVAL);
			Moving (1);
		}
	}

	virtual public void Reset ()
	{
		ShowOrHideArrow (false);
		targetValue = currentIndex = currentRealIndex = 0;
		RogerContainerCleaner.Clean (container);
		RectTransform rectTransform = container.GetComponent<RectTransform> ();
		rectTransform.anchoredPosition = Vector2.zero;
	}
}

