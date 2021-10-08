using UnityEngine;
using System.Collections;

namespace Swimming
{
	public class GUIManager : MonoBehaviour
	{
		public GameObject header;
		public GameObject gameOver;
		public GameObject ending;
		public GameObject opening;

		private static GUIManager m_Instance;
		public static GUIManager Instance
		{
			get
			{
				return m_Instance;
			}
		}

		void Awake()
		{
			m_Instance = this;
		}

		public void Init()
		{
			ShowHeader();
		}

		void HideAll()
		{
			header.SetActive(false);
			ending.SetActive(false);
			//gameOver.SetActive(false);
		}

		public void ShowGameOver()
		{
			HideAll();
			gameOver.SetActive(true);
		}

		public void ShowHeader()
		{
			HideAll();
			header.SetActive(true);
		}

		public void ShowEnding()
		{
			ending.SetActive(true);
		}

		public void CloseEnding()
		{
			ending.SetActive(false);
			GameManager.Instance.SendGameEndingAPI();
			ComponentConstant.SOUND_MANAGER.StopBGM();
		}

		public void ShowOpening()
		{
			Debug.Log("ShowOpening");
			opening.SetActive(true);
		}

		public void CloseOpening()
		{
			Debug.Log("CloseOpening");
			opening.SetActive(false);
			GameManager.Instance.CloseOpening();
		}

	}
}