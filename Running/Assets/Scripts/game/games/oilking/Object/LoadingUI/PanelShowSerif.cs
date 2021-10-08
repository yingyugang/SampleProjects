using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using CSV;

[System.Serializable]
public class ShowCharacterSerif
{
	public Image[] arrCharacters;
}

public class PanelShowSerif : MonoBehaviour {
	public ShowCharacterSerif[] arrShowCharSerif;
	public Image[] arrImgBoardChar;
	public Text textRate;
	public Slider slider;

	private int[,] arrSerifFromCSV;

	public Image imgSliderSerif,imgSliderSerifRed,imgSliderSerifBoder;
	private GameObject m_currentSerifTextCount = null;
	private bool m_Wink = false;
	private float m_WinkRate = 0.2f;
	void Start()
	{
		TrasferFormatArraySerif();
		SetSpritePresentCharacter();

		textRate.text = DetermineLengthNumber() + (ParameterServer.lstSerifDone != null? ParameterServer.lstSerifDone.Count : 0) + "/180";
		slider.value = (float)(ParameterServer.lstSerifDone != null ? ParameterServer.lstSerifDone.Count : 0) / 180;

		LoadFromAssetBundle ();
	}
	void LoadFromAssetBundle(){
		imgSliderSerif.sprite = OilKingAssetLoader.s_Instance.getSprite (TypeSprite.SliderSerif);
		imgSliderSerifRed.sprite = OilKingAssetLoader.s_Instance.getSprite (TypeSprite.SliderSerifForce);
		imgSliderSerifBoder.sprite = OilKingAssetLoader.s_Instance.getSprite (TypeSprite.SliderSerifBorder);
	}
	void OnDisable() {
		CancelInvoke();
	}
	string DetermineLengthNumber()
	{
		int tmpValue = 1;
		for (int i = 1; i < 9; i++)
		{
			tmpValue *= 10;
			if(ParameterServer.lstSerifDone != null)
			{
				if (ParameterServer.lstSerifDone.Count / tmpValue <= 0)
				{
					int numBegin = 3 - i;
					string stringBegin = "";
					for (int j = 0; j < numBegin; j++)
					{
						stringBegin += "0";
					}

					return stringBegin;
				}
			}
		}

		return "";
	}
	void CheckCurrentGroup(int i, int j, Text text, int value){
		text.text = value.ToString();
		if (i != LoadingUIOilKing.s_Instance.idHit || j != LoadingUIOilKing.s_Instance.idThrow || ParameterServer.DoubleCheck){
			text.color = Color.white;
			return;
		}
		else {
			m_currentSerifTextCount = text.gameObject;
			text.color = Color.red;
			InvokeRepeating("MakeWink", 0f, m_WinkRate);
		}
	}
	void SetSpritePresentCharacter()
	{
		for (int i = 0; i < arrImgBoardChar.Length; i++)
		{
			arrImgBoardChar[i].sprite = LoadingUIOilKing.s_Instance.GetSpriteBoardCharacter()[i];
		}

		for (int i = 0; i < 6; i++)
		{
			for (int j = 0; j < 5; j++)
			{
				arrShowCharSerif[i].arrCharacters[j].sprite = LoadingUIOilKing.s_Instance.GetSerifsItem()[0];
			}
		}

		for (int i = 0; i < 6; i++)
		{
			for (int j = 0; j < 6; j++)
			{
				if (arrSerifFromCSV[i, j] != 0)
				{
					if (j < i)
					{
						arrShowCharSerif[i].arrCharacters[j].sprite = LoadingUIOilKing.s_Instance.GetSerifsItem()[j + 1];
						Text text = arrShowCharSerif[i].arrCharacters[j].GetComponentInChildren<Text>();
						CheckCurrentGroup(i, j, text, arrSerifFromCSV[i, j]);
					}
					else if (j > i)
					{
						arrShowCharSerif[i].arrCharacters[j - 1].sprite = LoadingUIOilKing.s_Instance.GetSerifsItem()[j + 1];
						Text text = arrShowCharSerif[i].arrCharacters[j - 1].GetComponentInChildren<Text>();
						CheckCurrentGroup(i, j, text, arrSerifFromCSV[i, j]);
					}
				}
			}
		}

	}

	void MakeWink(){
		m_Wink=!m_Wink;
		m_currentSerifTextCount.SetActive(m_Wink);
	}
	void TrasferFormatArraySerif()
	{
		arrSerifFromCSV = new int[6, 6];

		//set value from csv to a array temp
		for (int i = 0; i < 6; i++)
		{
			for (int j = 0; j < 6; j++)
			{
				arrSerifFromCSV[i,j] = OilKingCSV.s_Instance.getArraysSerif()[i,j];
			}
		}
	}
}
