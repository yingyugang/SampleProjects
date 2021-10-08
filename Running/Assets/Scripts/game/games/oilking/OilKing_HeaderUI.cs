using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class OilKing_HeaderUI : MonoBehaviour
{
	public Button btnPause;

	private static OilKing_HeaderUI m_Instance;
	public static OilKing_HeaderUI Instance
	{
		get
		{
			if (m_Instance == null)
			{
				m_Instance = GameObject.FindObjectOfType<OilKing_HeaderUI>();
			}
			return m_Instance;
		}
	}

	void Awake()
	{
		m_Instance = this;
	}

	public void PauseBtnInteractive(bool isEnable)
	{
		btnPause.interactable = isEnable;
	}

}
