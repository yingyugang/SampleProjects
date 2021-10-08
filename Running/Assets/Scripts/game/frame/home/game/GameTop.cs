using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using UnityEngine.UI;
using System.Collections.Generic;
using home;

public class GameTop : ViewWithDefaultAction
{
	public GameObject information;
	public Text informationText;
	public List<GameItem> gameItemList;
	public Text gameNumber;
	public HeaderMediator header;
	public Button eventButton;
	public UnityAction eventUnityAction;
	public Image role;
	public GameObject arrow;
	public Button presentButton;
	public UnityAction presentUnityAction;

	public void SetInformation (string content, string imageResource)
	{
		informationText.text = content;
		information.SetActive (!string.IsNullOrEmpty (content));
		if (!string.IsNullOrEmpty (imageResource)) {
			StartCoroutine (ComponentConstant.ASSETBUNDLE_RESOURCES_GETTER.GetResource<Texture2D> (AssetBundleName.role_image.ToString (), imageResource, (Texture2D texture2D) => {
				role.sprite = TextureToSpriteConverter.ConvertToSprite (texture2D);
				role.gameObject.SetActive (true);
				arrow.SetActive (true);
			}));
		} else {
			role.gameObject.SetActive (false);
			arrow.SetActive (false);	
		}
	}

	public override void AddEventListeners ()
	{
		int length = gameItemList.Count;
		for (int i = 0; i < length; i++) {
			gameItemList [i].button.onClick.AddListener (unityActionArray [i]);
		}
	}

	public override void RemoveEventListeners ()
	{
		for (int i = 0; i < buttonArray.Length; i++) {
			gameItemList [i].button.onClick.RemoveListener (unityActionArray [i]);
			gameItemList [i].button.onClick = null;
		}

		unityActionArray = null;
	}

	protected override void AddActions ()
	{
		unityActionArray = new UnityAction[] {
			() => {
				Send (0);
			},
			() => {
				Send (1);
			},
			() => {
				Send (2);
			},
			() => {
				Send (3);
			},
			() => {
				Send (4);
			},
			() => {
				Send (5);
			},
			() => {
				Send (6);
			},
			() => {
				Send (7);
			},
			() => {
				Send (8);
			}
		};

		eventButton.onClick.AddListener (() => {
			eventUnityAction ();
		});

		presentButton.onClick.AddListener (() => {
			presentUnityAction ();
		});
	}
}
