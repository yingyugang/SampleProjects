using UnityEngine;
using System.Collections;
[System.Serializable]
public class Game12_Player_Properties {
	public int m_ID;
	public string m_Name;
	public string m_ImageSource;
	public float m_velocity;
	public float m_jumpPower;
	public float m_itemRate;
	public string m_Desciption;
	public int m_MultiJump;
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
	public float GetVelocity(){
		return m_velocity;
	}
	public void SetVelocity(float value){
		m_velocity = value;
	}
	public float GetJumpPower(){
		return m_jumpPower;
	}
	public void SetJumpPower(float value){
		m_jumpPower = value;
	}
	public float GetItemRate(){
		return m_itemRate;
	}
	public void SetItemRate(float value){
		m_itemRate = value;
	}
	public string GetDescription(){
		return m_Desciption;
	}
	public void SetDescription(string value){
		m_Desciption = value;
	}
	public int GetMultiJump(){
		return m_MultiJump;
	}
	public void SetMultiJump(int value){
		m_MultiJump = value;
	}
}
