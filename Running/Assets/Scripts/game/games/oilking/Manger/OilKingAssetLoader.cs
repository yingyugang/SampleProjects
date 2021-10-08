using UnityEngine;
using System.Collections;

public class TypeSprite
{
	public const string Boom = "OilKing_Block_Bomb";
	public const string CutinKing = "CutinKing";
	public const string Fossil = "OilKing_Block_Fossil";
	public const string HitPerson = "HitPerson";
	public const string HitPersonShow = "HitPersonShow";
	public const string O_Item_1 = "O_Item_1";
	public const string O_Item_2 = "O_Item_2";
	public const string O_Item_3 = "O_Item_3";
	public const string O_Item_4 = "O_Item_4";
	public const string O_Item_5 = "O_Item_5";
	public const string Plaster = "OilKing_Block_Plaster";

	public const string Stone = "OilKing_Block_Stone";
	public const string ThrowPerson = "ThrowPerson";
	public const string ThrowPersonShow = "ThrowPersonShow";
	public const string Treasures1 = "OilKing_treasures_01";
	public const string Treasures2 = "OilKing_treasures_02";
	public const string Treasures3 = "OilKing_treasures_03";
	public const string Treasures4 = "OilKing_treasures_04";
	public const string Treasures5 = "OilKing_treasures_05";
	public const string Treasures6 = "OilKing_treasures_06";
	public const string UtilSkill = "UtilSkill";
	public const string UtilSkillShow = "UtilSkillShow";

	public const string springbomb = "springbomb";

	public const string ButtonFever = "ButtonFever";

	public const string SuffixHit = "hit";
	public const string SuffixThrow = "throw";
	public const string SuffixHitUI = "hitui";
	public const string SuffixThrowUI = "throwui";

	public const string SuffixDrill = "OilKing_Drill_0";

	public const string ButtonDrill = "OilKing_Button_Drill";

	//	public const string ButtonColorRed = "Button00_Red";
	//	public const string ButtonColorBlue = "Button01_Blue";
	//	public const string ButtonColorGreen = "Button02_Green";
	//	public const string ButtonColorViolet = "Button03_Violet";
	//	public const string ButtonColorYellow = "Button04_Yellow";
	//	public const string ButtonColorPink = "Button05_Pink";

	public const string OilKing_Background = "OilKing_Background";

	public const string CoinCollection = "coincollection";

	public const string SliderSerif = "serif-slider";
	public const string SliderSerifForce = "serif-slider-red";
	public const string SliderSerifBorder = "serif-slider-border";
	public const string BlockFever = "OilKing_Block_Fever";

	public const string EndingImage = "teikyo";

}

public class OilKingAssetLoader : MonoBehaviour
{

	private static OilKingAssetLoader _s_Instance;

	public static OilKingAssetLoader s_Instance {
		get {
			if (_s_Instance == null) {
				_s_Instance = GameObject.FindObjectOfType <OilKingAssetLoader> ();
			}
			return _s_Instance;
		}
	}

	void Awake ()
	{
		_s_Instance = this;
	}

	public Sprite getSprite (string typeSprite)
	{
//		Debug.Log ("AssetBundleResourcesLoader.oilking_Dictionary.Count= "+AssetBundleResourcesLoader.oilking_Dictionary.Count);
//		foreach (var item in AssetBundleResourcesLoader.oilking_Dictionary.Keys) {
//			Debug.Log ("itemname="+item.ToString ());
//		}

		Sprite mySprite = AssetBundleResourcesLoader.oilking_Dictionary [typeSprite];
		return mySprite;

	}

	/// <summary>
	/// id=1->6, key_suffix from class TypeSprite, index from 0-?
	/// </summary>
	/// <returns>The sprite character.</returns>
	/// <param name="id">Identifier.</param>
	/// <param name="key_suffix">Key suffix.</param>
	/// <param name="indexAnim">Index animation.</param>
	public Sprite getSpriteCharacter (int id, string key_suffix, int indexAnim)
	{
		string spriteKey = "OilKingMC_0" + id + "_" + key_suffix + "_0" + indexAnim;
//		Debug.Log ("spriteKey="+spriteKey);
		Sprite mySprite = AssetBundleResourcesLoader.oilking_Dictionary [spriteKey];
		return mySprite;
	}

	/// <summary>
	/// indexAnim from 1 to 3
	/// </summary>
	/// <returns>The sprite drill.</returns>
	/// <param name="indexAnim">Index animation.</param>
	public Sprite getSpriteDrill (int indexAnim)
	{
		string spriteKey = "OilKing_Drill_0" + indexAnim;
		Sprite mySprite = AssetBundleResourcesLoader.oilking_Dictionary [spriteKey];
		return mySprite;
	}

	/// <summary>
	/// Gets the sprite button.
	/// </summary>
	/// <returns>The sprite button.</returns>
	/// <param name="id_character">Identifier character from 1-6</param>
	public Sprite getSpriteButtonDigging (int id_character)
	{
		string spriteKey = "Digging" + id_character;
		Sprite mySprite = AssetBundleResourcesLoader.oilking_Dictionary [spriteKey];
		return mySprite;


	}

	/// <summary>
	/// Gets the sprite button.
	/// </summary>
	/// <returns>The sprite button.</returns>
	/// <param name="id_character">Identifier character from 1-6</param>
	public Sprite getSpriteButtonSpring (int id_character)
	{
		string spriteKey = "Spring" + id_character;
		Sprite mySprite = AssetBundleResourcesLoader.oilking_Dictionary [spriteKey];
		return mySprite;
	}

	/// <summary>
	/// Gets the sprite board character.
	/// </summary>
	/// <returns>The sprite board character.</returns>
	/// <param name="id_character">Identifier character from 0-6,0=null,1-6 id character</param>
	public Sprite getSpriteBoardCharacter (int id_character)
	{
		string spriteKey = "serif-board-character-" + id_character;
		Sprite mySprite = AssetBundleResourcesLoader.oilking_Dictionary [spriteKey];
		return mySprite;
	}

	public Sprite getItemSerif (int id_serif)
	{
		string spriteKey = "serif-item-" + id_serif;
		Sprite mySprite = AssetBundleResourcesLoader.oilking_Dictionary [spriteKey];
		return mySprite;
	}

	/// <summary>
	/// Gets the ending animation.
	/// </summary>
	/// <returns>The ending animation. index from 1-5</returns>
	public Sprite getEndingAnimation (int index)
	{
		string spriteKey = "teikyo" + index;
		Sprite mySprite = AssetBundleResourcesLoader.oilking_Dictionary [spriteKey];
		return mySprite;
	}
}