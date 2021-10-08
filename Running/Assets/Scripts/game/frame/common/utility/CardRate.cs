using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

public class CardRate
{
	public static float[] cardTotalArray;
	public static float[] cardAdditionalArray;
	private static int gameCount;

	static CardRate ()
	{
		gameCount = MasterCSV.gameCSV.Count () + 1;
		cardTotalArray = new float[gameCount];
		cardAdditionalArray = new float[gameCount];
	}

	public static void GetTotalForAll ()
	{
		for (int i = 0; i < gameCount; i++) {
			cardTotalArray [i] = GetTotal (i == 0 ? 1 : 2, i);
		}
	}

	private static void ResetAdditionalRate ()
	{
		cardAdditionalArray = new float[gameCount];
	}

	public static void SetAdditionalRate (int id, float value)
	{
		if(id < 0){
			id = 0;
		}
		cardAdditionalArray [id] += value;
	}

	public static void AddToOriginalRate ()
	{
		for (int i = 0; i < gameCount; i++) {
			cardTotalArray [i] += cardAdditionalArray [i];
		}
		ResetAdditionalRate ();
	}

	private static List<CardCSVStructure> GetOwnCardList (int up_type, int id = 0)
	{
		List<CardCSVStructure> ownCardCSVStructureList = new List<CardCSVStructure> ();
		List<CardCSVStructure> cardCSVStructureList = null;
		if (up_type == 1) {
			cardCSVStructureList = MasterCSV.cardCSV.Where (result => result.up_type == up_type).ToList ();
		} else if (up_type == 2) {
			cardCSVStructureList = MasterCSV.cardCSV.Where (result => result.up_game_id == id && result.up_type == up_type).ToList ();
		}
		
		foreach (var card in UpdateInformation.GetInstance.card_list) {
			foreach (var cardCSVStructure in cardCSVStructureList) {
				if (card.m_card_id == cardCSVStructure.id) {
					ownCardCSVStructureList.Add (cardCSVStructure);
				}
			}
		}
		return ownCardCSVStructureList;
	}


	public static float GetTotal (int up_type, int id = 0)
	{
		List<CardCSVStructure> ownCardCSVStructureList = GetOwnCardList (up_type, id);
		float total = ownCardCSVStructureList.Sum (result => result.up_value);

		ownCardCSVStructureList = null;
		return total;
	}

	public static string GetAdditionalCardID (int up_type, int id = 0)
	{
		List<CardCSVStructure> ownCardCSVStructureList = GetOwnCardList (up_type, id);
		string idStr = string.Empty;
		int length = ownCardCSVStructureList.Count;
		for (int i = 0; i < length; i++) {
			idStr += ownCardCSVStructureList [i].id;
			if (i != length - 1) {
				idStr += LanguageJP.COMMA;
			}
		}
		ownCardCSVStructureList = null;
		return idStr;
	}
}