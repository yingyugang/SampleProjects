using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class OilKingFooter : MonoBehaviour {
	public GameObject iconStar;
	public Slider slider;
	public GameObject left;
	public GameObject right;

	public Text txtMatsu;
	public Text txtOden;
	public Text txtOnigiri;

	private static OilKingFooter m_Instance;

	public static OilKingFooter Instance
	{
		get {
			if (m_Instance == null) {
				m_Instance = GameObject.FindObjectOfType<OilKingFooter> ();
			}
			return m_Instance;
		}
	}

	void Awake()
	{
		m_Instance = this;
	}

	public void SetSliderValue(float value)
	{
		slider.value = value;
	}

	public void ActiveFeverBoard()
	{
		iconStar.SetActive (true);
		slider.gameObject.SetActive (true);
		left.SetActive (false);
		right.SetActive (false);
	}

	public void DisactiveFeverBoard()
	{
		iconStar.SetActive (false);
		slider.gameObject.SetActive (false);
		left.SetActive (true);
	}

	public void SetLeftValue(Block item)
	{
		switch (item) {
		case Block.Item2:
			OilKingUtils.numPine ++;
			txtMatsu.text = OilKingUtils.numPine.ToString ();
			break;
		case Block.Item3:
			OilKingUtils.numOden ++;
			txtOden.text = OilKingUtils.numOden.ToString ();
			break;
		case Block.Item4:
			OilKingUtils.numOnigiri++;
			txtOnigiri.text = OilKingUtils.numOnigiri.ToString ();
			break;
		}
	}
}
