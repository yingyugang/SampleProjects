using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class LoadingUIOilKing : MonoBehaviour
{

	public static LoadingUIOilKing s_Instance;
	public ListCharacterOilKing lstCharacter;
	//public Image[] arrBGColor;
	public Sprite[] imageOfCharacter;
	public GameObject chooseDigging, chooseSpring, serifNormalPanel, serifTrickPanel;
	[HideInInspector]
	public int idHit;
	[HideInInspector]
	public int idThrow;
	[HideInInspector]
	public bool m_IsDone = false;
	[HideInInspector]
	public bool m_CheckSerif;

	public Image imgGotoGame;
	public GameObject imgBackgroundBlack;

	public PanelShowSerif panelShowSerif;

	private Sprite[] arrSerifsItem;
	private Sprite[] arrSpriteBoardChar;

	void Awake()
	{
		s_Instance = this;

		//Check Serif talk - Get value from Server to set true/false or normal/trick

		CheatData cheatData = CheatController.GetLastMatchCheat();
		if (cheatData != null)
		{
			//turn on
			m_CheckSerif = true;
		}
		else {
			m_CheckSerif = false;
		}

		imageOfCharacter = new Sprite[12];
	}

	// Use this for initialization
	IEnumerator Start()
	{
		yield return new WaitForEndOfFrame();
		GetAssetBundle();
		SetImageCharacter();
		CheckSerifTalk();
		//		imgBackgroundBlack.SetActive (false);
	}

	void GetAssetBundle()
	{
		for (int i = 0; i < 12; i++)
		{
			imageOfCharacter[i] = OilKingCSV.s_Instance.GetSpriteCharacterFromOilKingCSV(i);
		}

		//set color for button background
		//arrBGColor[0].sprite = OilKingAssetLoader.s_Instance.getSprite(TypeSprite.ButtonColorRed);
		//arrBGColor[1].sprite = OilKingAssetLoader.s_Instance.getSprite(TypeSprite.ButtonColorBlue);
		//arrBGColor[2].sprite = OilKingAssetLoader.s_Instance.getSprite(TypeSprite.ButtonColorGreen);
		//arrBGColor[3].sprite = OilKingAssetLoader.s_Instance.getSprite(TypeSprite.ButtonColorViolet);
		//arrBGColor[4].sprite = OilKingAssetLoader.s_Instance.getSprite(TypeSprite.ButtonColorYellow);
		//arrBGColor[5].sprite = OilKingAssetLoader.s_Instance.getSprite(TypeSprite.ButtonColorPink);
		//arrBGColor[6].sprite = OilKingAssetLoader.s_Instance.getSprite(TypeSprite.ButtonColorRed);
		//arrBGColor[7].sprite = OilKingAssetLoader.s_Instance.getSprite(TypeSprite.ButtonColorBlue);
		//arrBGColor[8].sprite = OilKingAssetLoader.s_Instance.getSprite(TypeSprite.ButtonColorGreen);
		//arrBGColor[9].sprite = OilKingAssetLoader.s_Instance.getSprite(TypeSprite.ButtonColorViolet);
		//arrBGColor[10].sprite = OilKingAssetLoader.s_Instance.getSprite(TypeSprite.ButtonColorYellow);
		//arrBGColor[11].sprite = OilKingAssetLoader.s_Instance.getSprite(TypeSprite.ButtonColorPink);

		//get serifs item
		arrSerifsItem = new Sprite[7];
		for (int i = 0; i < arrSerifsItem.Length; i++)
		{
			arrSerifsItem[i] = OilKingAssetLoader.s_Instance.getItemSerif(i);
		}

		//get sprite hit character present
		arrSpriteBoardChar = new Sprite[6];
		for (int i = 0; i < arrSpriteBoardChar.Length; i++)
		{
			arrSpriteBoardChar[i] = OilKingAssetLoader.s_Instance.getSpriteBoardCharacter(i + 1);
		}
	}

	public Sprite[] GetSerifsItem()
	{
		return arrSerifsItem;
	}

	public Sprite[] GetSpriteBoardCharacter()
	{
		return arrSpriteBoardChar;
	}

	public void SetImageCharacter()
	{
		for (int i = 0; i < imageOfCharacter.Length; i++)
		{
			lstCharacter.lstCharacter[i].image.sprite = imageOfCharacter[i];
		}
	}

	public void ChoosePlayer(int id)
	{
		if (lstCharacter != null)
		{
			foreach (var item in lstCharacter.lstCharacter)
			{
				if (item.id == id)
				{
					if (item.type == TypeOfCharacter.Digging)
					{
						//Id of Digging or Hit
						idHit = id;
						chooseDigging.SetActive(false);
						chooseSpring.SetActive(true);
						lstCharacter.lstCharacter[idHit + 6].GetComponent<Button>().interactable = false;
						return;
					}
					else if (item.type == TypeOfCharacter.Spring)
					{
						//Id of Spring or Throw
						if (id != idHit)
						{
							idThrow = id;
							chooseSpring.SetActive(false);
							serifNormalPanel.SetActive(true);

							return;
						}
						else {
							return;
						}
					}
				}
			}
		}
	}

	void CheckSerifTalk()
	{
		if (m_CheckSerif)
		{
			serifNormalPanel.SetActive(false);
			serifTrickPanel.SetActive(true);
		}
		else {
			serifTrickPanel.SetActive(false);
			serifNormalPanel.SetActive(true);
		}
	}

	public void ButtonGotoSerif()
	{
		PanelTalkOilKing.s_Instance.StartSerif();

		CheatData cheatData = CheatController.GetLastMatchCheat();
		if (cheatData != null)
		{
			//turn on
			OilKingManager.s_Instance.SendAPISerif ();
		}

	}
	public void ButtonGotoGame()
	{
		PanelTalkOilKing.s_Instance.StartGame();
	}
}
