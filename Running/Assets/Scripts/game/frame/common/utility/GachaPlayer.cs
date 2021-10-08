using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.UI;
using System;

public class GachaPlayer : MonoBehaviour
{
	public GachaResourcesGetter gachaResourcesGetter;
	public Transform gachaContainer;
	public Button button;
	private GachaController gachaController;
	private int gachaAnimationType;
	public UnityAction<int,bool> unityAction;
	public GachaResultContainerMediator gachaResultContainerMediator;
	private List<GachaItem> gachaItemList;
	private bool isNoMore;
	public ShortLoadingMediator shortLoadingMediator;
	public GameObject gacha;
	private string currentBGMName;

	public void Play (int gachaAnimationType, bool isNoMore)
	{
		this.gachaAnimationType = gachaAnimationType;
		this.isNoMore = isNoMore;
		StartCoroutine (CreateGachaPanel ());
		currentBGMName = ComponentConstant.SOUND_MANAGER.GetBGMName ();
		ComponentConstant.SOUND_MANAGER.StopBGM ();
	}

	private GameObject gachaPrefab;

	private IEnumerator CreateGachaPanel ()
	{
		shortLoadingMediator.Show ();
		if (gachaPrefab != null) {
			GetResource (gachaPrefab);
			yield return null;
		} else {
			yield return StartCoroutine (ComponentConstant.ASSETBUNDLE_RESOURCES_GETTER.GetResource<GameObject> (AssetBundleName.GachaPanel.ToString (), AssetBundleName.GachaPanel.ToString (), GetResource, false));
		}
	}

	private void GetResource (GameObject go)
	{
		if (gachaPrefab == null)
			gachaPrefab = go;
		gachaController = Instantiator.GetInstance ().InstantiateGameObject (go, Vector2.zero, Vector3.one, gachaContainer).GetComponent<GachaController> ();
		RectTransform rectTransform = gachaController.GetComponent<RectTransform> ();
		rectTransform.anchoredPosition = Vector2.zero;
		rectTransform.sizeDelta = Vector2.zero;

		gachaResourcesGetter.unityAction = (List<GachaItem> list, GachaResultInfo gachaResultInfo) => {
			gachaItemList = list;
			gachaController.onGachaFinish = () => {
				ShowResult ();
			};
			shortLoadingMediator.Hide ();
			gacha.SetActive (true);
			gachaContainer.gameObject.SetActive (true);
			if (!isNoMore) {
				button.onClick.AddListener (SkipAnimation);
			}
			gachaController.Play (gachaAnimationType, gachaResultInfo.show_persons, list, gachaResultInfo.show_led);
		};
		gachaResourcesGetter.GetResources ();
	}

	private void SkipAnimation ()
	{
		ShowResult ();
	}

	private void ShowResult ()
	{
		if (!isNoMore) {
			button.onClick.RemoveListener (SkipAnimation);
		}
		Destroy (gachaController.gameObject);
		Resources.UnloadUnusedAssets ();
		gachaResultContainerMediator.unityAction = (int type) => {
			Clean ();
			APIInformation.GetInstance.gacha_result = null;
			gachaResultContainerMediator.gameObject.SetActive (false);
			gacha.SetActive (false);
			if (unityAction != null) {
				unityAction (type, Player.GetInstance.gacha_up_end > SystemInformation.GetInstance.current_time);
			}
		};
		if (currentBGMName == null) {
			currentBGMName = SoundEnum.bgm01_title.ToString ();
		}
		ComponentConstant.SOUND_MANAGER.Play ((SoundEnum)Enum.Parse (typeof(SoundEnum), currentBGMName));
		gachaResultContainerMediator.SetWindow (gachaItemList, isNoMore);
		gachaResultContainerMediator.gameObject.SetActive (true);
	}

	private void Clean ()
	{
		int length = gachaItemList.Count;
		for (int i = 0; i < length; i++) {
			gachaItemList [i].cardItem.Clean ();
			gachaItemList [i].cardItem = null;
		}
	}
}
