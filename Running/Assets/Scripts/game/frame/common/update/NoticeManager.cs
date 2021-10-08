using UnityEngine;
using home;
using System.Collections.Generic;
using UnityEngine.UI;

public class NoticeManager : MonoBehaviour
{
	public const string GAME = "GAME";
	public const string GACHA = "GACHA";
	public const string CARD = "CARD";
	public const string SHOP = "SHOP";
	public const string MISSION = "MISSION";
	public const string OTHER = "OTHER";
	public const string PRESENT = "PRESENT";
	public const string INFO = "INFO";

	private Dictionary<string,int> dictionary;

	public GameObject[] noticeArray;

	public void UpdateNoticeManager ()
	{
		Mark mark = SystemInformation.GetInstance.mark;
		SetValue (GAME, mark.game);
		SetValue (GACHA, mark.gacha);
		SetValue (CARD, mark.card);
		SetValue (SHOP, mark.shop);
		SetValue (MISSION, mark.mission);
		SetValue (PRESENT, mark.other_present);
		SetValue (INFO, mark.other_infor);
	}

	public static void ClearAllNotice ()
	{
		PlayerPrefs.DeleteKey (GAME);
		PlayerPrefs.DeleteKey (GACHA);
		PlayerPrefs.DeleteKey (CARD);
		PlayerPrefs.DeleteKey (SHOP);
		PlayerPrefs.DeleteKey (MISSION);
		PlayerPrefs.DeleteKey (OTHER);
		PlayerPrefs.DeleteKey (PRESENT);
		PlayerPrefs.DeleteKey (INFO);
	}

	private void SetValue (string key, int value)
	{
		GetNoticeDictionary ();
		int before = PlayerPrefs.GetInt (key, 0);
		if (before < value) {
			if (key == PRESENT || key == INFO) {
				noticeArray [dictionary [OTHER]].SetActive (true);
			}
			noticeArray [dictionary [key]].SetActive (true);
		}
	}

	public bool GetStatus (string noticeName)
	{
		return noticeArray [dictionary [noticeName]].activeSelf;
	}

	public void ClearNoticeManager (string key)
	{
		GetNoticeDictionary ();
		noticeArray [dictionary [key]].SetActive (false);
		PlayerPrefs.SetInt (key, TimeUtil.GetCurrentServerTimeStamp ());
		if (key == PRESENT || key == INFO) {
			if (noticeArray [dictionary [PRESENT]].activeSelf == false && noticeArray [dictionary [INFO]].activeSelf == false) {
				noticeArray [dictionary [OTHER]].SetActive (false);
			}
		}
	}

	private void GetNoticeDictionary ()
	{
		dictionary = new Dictionary<string, int> ();
		dictionary.Add (GAME, 0);
		dictionary.Add (GACHA, 1);
		dictionary.Add (CARD, 2);
		dictionary.Add (SHOP, 3);
		dictionary.Add (MISSION, 4);
		dictionary.Add (OTHER, 5);
		dictionary.Add (PRESENT, 6);
		dictionary.Add (INFO, 7);
	}
}
