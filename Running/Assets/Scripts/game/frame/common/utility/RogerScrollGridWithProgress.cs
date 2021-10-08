using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class RogerScrollGridWithProgress : RogerScrollGrid
{
	public Transform greyDot;
	public Transform pinkDot;
	public Transform dotContainer;
	private int length;
	private List<Transform> dotList;
	private Instantiator instantiator;

	public override void Init (List<Sprite> list)
	{
		base.Init (list);
		instantiator = Instantiator.GetInstance ();
		instantiator.SetParent<Transform> (pinkDot, new Vector2 (0, 1920), Vector3.one, transform);
		RogerContainerCleaner.Clean (dotContainer);
		length = list.Count;
		CreateDotList ();
		ChangeDot ();
	}

	private void CreateDotList ()
	{
		dotList = new List<Transform> ();

		for (int i = 0; i < length; i++)
		{
			Transform dotTransform = instantiator.Instantiate<Transform> (greyDot, Vector2.zero, Vector3.one, dotContainer);
			if (length > 1) {
				dotTransform.gameObject.SetActive (true);
			}
			dotList.Add (dotTransform);
		}
	}

	protected override void Moving (int direction)
	{
		base.Moving (direction);
		ChangeDot ();
	}

	private void ChangeDot ()
	{
		if (length > 1) {
			instantiator.SetParent<Transform> (pinkDot, Vector2.one, Vector3.one, dotList [currentRealIndex]);
		}
	}
}

