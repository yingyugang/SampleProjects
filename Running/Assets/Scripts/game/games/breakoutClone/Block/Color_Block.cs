using UnityEngine;
using System.Collections;

public class Color_Block{

	private int m_IdColor;
	private int m_MinStrong;
	private int m_MaxStrong;
	private string m_ColorName;
	private bool m_IsBoss;

	//-->Contruction Color 
	public Color_Block(int id, int min, int max, string color, bool isboss){
		m_IdColor = id;
		m_MinStrong = min;
		m_MaxStrong = max;
		m_ColorName = color;
		m_IsBoss = isboss;
	}
	//--<

	//-->Is value in range
	public bool CheckRangeColor(int value, bool isboss){
		if(m_IsBoss != isboss) return false;
		return (value >= m_MinStrong && value <= m_MaxStrong);
	}
	//--<

	public Color GetColor(){
		return ConvertToColor(m_ColorName);
	}

	//-->Convert string to Color
	Color ConvertToColor(string colorName){
		Color result = Color.white;
		switch (colorName) {
		case BreackoutConfig.COLOR_RED:
			result = Color.red;
			break;
		case BreackoutConfig.COLOR_YELLOW:
			result = Color.yellow;
			break;
		case BreackoutConfig.COLOR_GREEN:
			result = Color.green;
			break;
		case BreackoutConfig.COLOR_WHITE:
			result = Color.white;
			break;
		default:
			result = Color.white;
			break;
		}
		return result;
	}
	//--<
}
