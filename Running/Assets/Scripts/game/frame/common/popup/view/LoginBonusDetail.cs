using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;

public class LoginBonusDetail : PopupContentWithDefaultAction
{
	public List<LoginBonusItem> loginBonusItemList;
	public Text targetText;
	public Image itemImage;
	private int dayIndex;
	private LoginBonusDetailCSVStructure currentLoginBonusDetailCSVStructure;
	private List<LoginBonusDetailCSVStructure> loginBonusDetailCSVStructureList;
	public List<Image> imageList;
	private List<string> imageResourceNameList;

	private void GetResourcesList<T> (List<Texture2D> list)
	{
		Dictionary<string,Sprite> spriteDictionary = TextureToSpriteConverter.ConvertToSpriteDictionary (list);
		int length = imageResourceNameList.Count;
		for (int i = 0; i < length; i++) {
			imageList [i].sprite = spriteDictionary [imageResourceNameList [i]];
		}
		Init ();
	}

	private void Init ()
	{
		int presentNumber = currentLoginBonusDetailCSVStructure.num;
		int m_item_id = currentLoginBonusDetailCSVStructure.m_item_id;
		ItemCSVStructure currentItemCSVStructure = MasterCSV.itemCSV.FirstOrDefault (result => result.id == m_item_id);

		for (int i = 1; i < dayIndex; i++) {
			loginBonusItemList [i - 1].ShowOrHideFace (true);
		}

		int length = loginBonusItemList.Count;
		for (int j = dayIndex; j < length; j++) {
			LoginBonusDetailCSVStructure loginBonusDetailCSVStructure = loginBonusDetailCSVStructureList [j];
			ItemCSVStructure itemCSVStructure = MasterCSV.itemCSV.FirstOrDefault (result => result.id == loginBonusDetailCSVStructure.m_item_id);
			loginBonusItemList [j].SetData (loginBonusDetailCSVStructure.num, AssetBundleResourcesLoader.itemIconDictionary [itemCSVStructure.image_resource]);
			loginBonusItemList [j].ShowOrHideFace (false);
		}

		LoginBonusItem loginBonusItem = loginBonusItemList [dayIndex - 1];
		loginBonusItem.unityAction = () => {
			targetText.text = string.Format ("{0}{1}", LanguageJP.X, presentNumber);
		};
			
		itemImage.sprite = AssetBundleResourcesLoader.itemIconDictionary [currentItemCSVStructure.image_resource];

		loginBonusItem.ShowAnimation ();
	}

	public void Show (IEnumerable<LoginBonusDetailCSVStructure> loginBonusDetailCSVStructureEnumerable, int dayIndex)
	{
		loginBonusDetailCSVStructureList = loginBonusDetailCSVStructureEnumerable.ToList ();
		currentLoginBonusDetailCSVStructure = loginBonusDetailCSVStructureEnumerable.FirstOrDefault (result => dayIndex == result.index);
		this.dayIndex = dayIndex;

		imageResourceNameList = new List<string> () {
			dayIndex.ToString (),
			"frame1",
			"frame2",
			"loginbonusday1",
			"loginbonusday2",
			"loginbonusday3",
			"loginbonusday4",
			"loginbonusday5",
			"loginbonusday6",
			"Stamp01",
			"Stamp02",
			"Stamp03",
			"Stamp04",
			"Stamp05",
			"Stamp06"
		};
		StartCoroutine (ComponentConstant.ASSETBUNDLE_RESOURCES_GETTER.GetResourcesList<Texture2D> (AssetBundleName.login_bonus.ToString (), imageResourceNameList, GetResourcesList<Texture2D>, false));
	}
}
