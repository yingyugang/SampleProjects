using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using System.Collections.Generic;
using UnityEngine.UI;

public class TutorialReadmeMediator : ActivityMediator
{
	public RogerScrollGrid rogerScrollGrid;
	public Transform instantiation;
	private int targetNumber;
	public TutorialPageMediator tutorialPageMediator;
	private TutorialReadme tutorialReadme;

	private const string BANNER = "banner_goldgacha";
	private const string MAN = "iyami";
	private const string ARROW = "arrow_tutorial";
	private const string POINT_GRAY = "point_gray";
	private const string POINT_PINK = "point_pink";

	protected override void InitData ()
	{
		tutorialReadme = viewWithDefaultAction as TutorialReadme;

		tutorialReadme.banner.sprite = tutorialPageMediator.dictionary [BANNER];
		tutorialReadme.man.sprite = tutorialPageMediator.dictionary [MAN];
		tutorialReadme.arrow.sprite = tutorialPageMediator.dictionary [ARROW];
		tutorialReadme.point_grey.sprite = tutorialPageMediator.dictionary [POINT_GRAY];
		tutorialReadme.point_pink.sprite = tutorialPageMediator.dictionary [POINT_PINK];

		ShowOrHideArrows (false);

		rogerScrollGrid.sendCurrentIndex = (int currentRealIndex, Image image) => {
			if (currentRealIndex == targetNumber) {
				Instantiator instantiator = Instantiator.GetInstance ();
				instantiator.SetParent<Transform> (instantiation, Vector2.zero, Vector3.one, image.transform).gameObject.SetActive (true);
			}
		};

		StartCoroutine (ComponentConstant.ASSETBUNDLE_RESOURCES_GETTER.GetAllResourcesList<Texture2D> (AssetBundleName.tutorial_image.ToString (), (List<Texture2D> list) => {
			List<Sprite> spriteList = TextureToSpriteConverter.ConvertToSpriteList (list);
			targetNumber = spriteList.Count - 1;
			rogerScrollGrid.Init (spriteList);
		}, false));
	}

	private void ShowOrHideArrows (bool isShow)
	{
		rogerScrollGrid.ArrowLeft.gameObject.SetActive (isShow);
		rogerScrollGrid.ArrowRight.gameObject.SetActive (isShow);
	}

	protected override void CreateActions ()
	{
		unityActionArray = new UnityAction[] {
			() => {
				ComponentConstant.SOUND_MANAGER.Play (SoundEnum.se01_ok);
				showWindow (1);
			}
		};
	}
}
