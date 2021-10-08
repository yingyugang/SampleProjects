using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class GameItem : MonoBehaviour
{
	private int id;
	public Image title;
	public Image balloon;
	public Rank rank;
	public GameObject newGo;
	public GameObject container;
	public Button button;
	public SequenceFrameAnimation sequenceFrameAnimation;
	private Dictionary<string,Sprite> gameIntroductionResourcesDirectory;
	private List<Sprite> gameAnimationResourceList;
	public Image instantiation;

	public void SetData (int id, int rankValue, bool isNew, bool isLock)
	{
		this.id = id;
		rank.SetRank (rankValue);
		SetNew (isNew);
		SetLock (isLock);
		gameIntroductionResourcesDirectory = GamePageMediator.gameIntroductionResourcesDirectory [id];
		gameAnimationResourceList = GamePageMediator.gameAnimationResourcesDirectory [id];
		button.image.sprite = gameIntroductionResourcesDirectory [LanguageJP.GAME_LOCK];
	}

	public void SetNew (bool isNew)
	{
		newGo.SetActive (isNew);
	}

	public void CreateAnimation ()
	{
		title.sprite = gameIntroductionResourcesDirectory [LanguageJP.GAME_TITLE];
		balloon.sprite = gameIntroductionResourcesDirectory [LanguageJP.GAME_BALLOON];
		int length = gameAnimationResourceList.Count;
		sequenceFrameAnimation.imageArray = new Image[length];
		Instantiator instantiator = Instantiator.GetInstance ();
		for (int i = 0; i < length; i++) {
			Image image = instantiator.Instantiate<Image> (instantiation, Vector2.zero, Vector3.one, sequenceFrameAnimation.transform);
			sequenceFrameAnimation.imageArray [i] = image;
			image.sprite = gameAnimationResourceList [i];
		}
		sequenceFrameAnimation.enabled = true;
	}

	public void SetLock (bool isLock)
	{
		container.SetActive (!isLock);
	}
}
