using UnityEngine;
using System.Collections;
[System.Serializable]
public class Game12_Item_Properties{
	public int m_ID;
	public string m_Name;
	public string m_ImageSource;
	public int m_Type;
	public float m_Effect_Value1;
	public float m_Effect_Value2;
	public int m_Percentage;
	public int m_PercentageReduction;
	public string m_Desciption;
	public int GetID(){
		return m_ID;
	}
	public void SetID(int value){
		m_ID = value;
	}
	public string GetName(){
		return m_Name;
	}
	public void SetName(string value){
		m_Name = value;
	}
	public string GetImageSource(){
		return m_ImageSource;
	}
	public void SetImageSource(string value){
		m_ImageSource = value;
	}
	public int GetItemType(){
		return m_Type;
	}
	public void SetItemType(int value){
		m_Type = value;
	}
	public float GetEffectValue1(){
		return m_Effect_Value1;
	}
	public void SetEffectValue1(float value){
		m_Effect_Value1 = value;
	}
	public float GetEffectValue2(){
		return m_Effect_Value2;
	}
	public void SetEffectValue2(float value){
		m_Effect_Value2 = value;
	}
	public int GetPercentage(){
		return m_Percentage;
	}
	public void SetPercentage(int value){
		m_Percentage = value;
	}
	public int GetPercentageReduction(){
		return m_PercentageReduction;
	}
	public void SetPercentageReduction(int value){
		m_PercentageReduction = value;
	}
	public string GetDescription(){
		return m_Desciption;
	}
	public void SetDescription(string value){
		m_Desciption = value;
	}

}
