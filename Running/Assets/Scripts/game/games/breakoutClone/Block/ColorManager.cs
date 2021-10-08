using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ColorManager : MonoBehaviour {
	private static ColorManager _instance;

	public static ColorManager instance {
		get {
			if (_instance == null)
				_instance = GameObject.FindObjectOfType<ColorManager> ();
			return _instance;
		}
	}

	private List<Color_Block> MyColor;

	//-->Add Color into List MyColor
	public void AddColor(Color_Block newColor){
		if(MyColor == null) MyColor = new List<Color_Block>();
		MyColor.Add(newColor);
	}
	//--<

	//-->Return Color in Range
	public Color GetColorByID(int value, bool isBoss){
		foreach(Color_Block color in MyColor){
			if(color.CheckRangeColor(value, isBoss)){
				return color.GetColor();
			}
		}
		return Color.white;
	}
	//--<
}
