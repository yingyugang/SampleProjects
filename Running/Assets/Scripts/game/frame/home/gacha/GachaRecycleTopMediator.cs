
using System.Collections;
using UnityEngine.Events;
using System.Collections.Generic;

public class GachaRecycleTopMediator : ActivityMediator
{
	public GachaRecycleDetailTopMediator gachaRecycleDetailTopMediator;
	private Gacha gacha;
	private string[] cardDescriptionArray;
	private GachaRecycleTop gachaRecycleTop;
	private List<string> ownList;
	private List<string> costList;
	private int[] indexList;
	private int[] countList;

        private void OnEnable ()
        {
                SetOwnAndCostPoint ();
        }

	protected override void Start ()
	{
		base.Start ();
		AddEventListeners ();
	}

	private void AddEventListeners ()
	{
		gachaRecycleTop.unityAction = (int index) => {
			ComponentConstant.SOUND_MANAGER.Play (SoundEnum.se01_ok);
			gachaRecycleDetailTopMediator.SetWindow (gacha, CreateGachaRecycleDetailData (index));
			showWindow (5);
		};
	}

	protected override void OnDestroy ()
	{
		base.OnDestroy ();
		gachaRecycleTop.unityAction = null;
	}

	protected override void CreateActions ()
	{
		unityActionArray = new UnityAction[] {
			() => {
				ComponentConstant.SOUND_MANAGER.Play (SoundEnum.se02_ng);
				showWindow (0);
			}
		};
	}

	private GachaRecycleDetailData CreateGachaRecycleDetailData (int index)
	{
		GachaRecycleDetailData gachaRecycleDetailData = new GachaRecycleDetailData ();
		gachaRecycleDetailData.mode = index + GameConstant.GACHA_RECYCLE_START_ID;
		gachaRecycleDetailData.sprite = gachaRecycleTop.buttonDetectorList [index].button.image.sprite;
		gachaRecycleDetailData.card_desc = cardDescriptionArray [index];
		gachaRecycleDetailData.ownList = GetRange (ownList, index);
		gachaRecycleDetailData.costList = GetRange (costList, index);
		return gachaRecycleDetailData;
	}

	private List<string> GetRange (List<string> list, int index)
	{
		return list.GetRange (indexList [index], countList [index]);
	}

	public void SetWindow (Gacha gacha)
	{
		this.gacha = gacha;
	}

        private void SetOwnAndCostPoint ()
        {
                cardDescriptionArray = gacha.card_desc.Split (',');
                gachaRecycleTop = viewWithDefaultAction as GachaRecycleTop;
                indexList = new int[] { 0, 1, 2, 5, 8 };
                countList = new int[] { 1, 1, 3, 3, 3 };

                costList = new List<string> ();
                string[] costArray = gacha.single_cost.Split (',');
                int length = costArray.Length;
                for (int i = 0; i < length; i++)
                {
                costList.Add (costArray[i]);
                }

                ownList = new List<string> ();
                PrepareRecyclePoint (0, GameConstant.GACHA_RECYCLE_NUMBER_OF_MODE);
                PrepareRecyclePoint (2, GameConstant.GACHA_RECYCLE_NUMBER_OF_MODE);
                PrepareRecyclePoint (2, GameConstant.GACHA_RECYCLE_NUMBER_OF_MODE);
        
                CreateGachaSprite (gacha.rarity_banner_image.Split (','));
        
                gachaRecycleTop.Show (ownList, costList, cardDescriptionArray);
        }

	private void CreateGachaSprite (string[] imageResourceArray)
	{
		int length = imageResourceArray.Length;
		for (int i = 0; i < length; i++) {
			gachaRecycleTop.buttonDetectorList [i].button.image.sprite = AssetBundleResourcesLoader.gachaBannerDictionary [imageResourceArray [i]];
		}
	}

	private void PrepareRecyclePoint (int start, int total)
	{
		for (int i = start; i < total; i++) {
			ownList.Add (UpdateInformation.GetInstance.recycle_pt [i].ToString ());
		}
	}
}
