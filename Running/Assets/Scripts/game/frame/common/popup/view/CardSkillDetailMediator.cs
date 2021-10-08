using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class CardSkillDetailMediator : PopupContentMediator
{
	public RogerScrollGrid rogerScrollGrid;
	private List<string> stringList;

	private void OnEnable ()
	{
		rogerScrollGrid.Reset ();
		stringList = new List<string> ();
		List<int> cardIDList = (List<int>)objectList [0];
		int length = cardIDList.Count;
		for (int i = 0; i < length; i++) {
			stringList.Add (cardIDList [i].ToString () + (length > 1 ? LanguageJP.UNDER_LINE + (i + 1).ToString () : ""));
		}
		StartCoroutine (ComponentConstant.ASSETBUNDLE_RESOURCES_GETTER.GetResourcesList<Texture2D> (AssetBundleName.game_skill.ToString (), stringList, (List<Texture2D> list) => {
			List<Sprite> spriteList = TextureToSpriteConverter.ConvertToSpriteList (list);
			rogerScrollGrid.Init (spriteList);
		}, false));
	}
}
