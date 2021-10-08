using UnityEngine;
using System.Collections;

public enum ItemType
{
	Item1 = 0,
	Item2,
	Item3,
	Item4,
	Item5,
	Item6
}

public class OilKing_Item : OilKing_Block 
{
	public ItemType itemName;

	public int score;

	private float m_RangeItem1;
	private float m_RangeItem2;
	private float m_RangeItem3;
	private float m_RangeItem4;
	private float m_RangeItem5;
	private float m_RangeItem6;
	private float m_MaxIRange;

	private float[] arrIIndex = new float[6];

	private float[] arrTmpIIndex = new float[6];

	private int itemAppear;


	private float tmpIValue;

	IEnumerator Start()
	{
		yield return new WaitForEndOfFrame ();
		GetIValuePercent ();
		Init ();

	}

	void OnEnable()
	{
		Init ();
	}


	void Init()
	{
		//if color of character digging coincident color of treasure => appear fever item (at order 4)
		itemAppear = OilKingGamePlay.checkColorCoincident ? 4 : ChooseNextItem ();


		if(OilKingCSV.s_Instance != null)
		{
			this.nameBlock = OilKingCSV.s_Instance.lstItemAttribute[itemAppear].typeItem;
			this.imgBlock = OilKingCSV.s_Instance.lstItemAttribute [itemAppear].imgSprite;
			this.hitNumber = OilKingCSV.s_Instance.lstItemAttribute[itemAppear].timeBreak - 1;
			this.canThrow = OilKingCSV.s_Instance.lstItemAttribute[itemAppear].throwable;
			this.score = OilKingCSV.s_Instance.lstItemAttribute [itemAppear].score;
			this.GetComponent <SpriteRenderer>().sprite = this.imgBlock;
		}
	}

	void GetIValuePercent()
	{
		if(OilKingCSV.s_Instance != null) {
			m_RangeItem1 = ParameterServer.lstProbabilityChestItem [0];
			m_RangeItem2 = ParameterServer.lstProbabilityChestItem [1];
			m_RangeItem3 = ParameterServer.lstProbabilityChestItem [2];
			m_RangeItem4 = ParameterServer.lstProbabilityChestItem [3];
			m_RangeItem5 = ParameterServer.lstProbabilityChestItem [4];
			m_RangeItem6 = ParameterServer.lstProbabilityChestItem [5];

			arrIIndex [0] = m_RangeItem1;
			arrIIndex [1] = m_RangeItem2;
			arrIIndex [2] = m_RangeItem3;
			arrIIndex [3] = m_RangeItem4;
			arrIIndex [4] = m_RangeItem5;
			arrIIndex [5] = m_RangeItem6;

			arrTmpIIndex [0] = arrIIndex [0];

			for (int i = 1; i < arrIIndex.Length; i++) {
				for (int j = 0; j <= i; j++) {
					arrTmpIIndex [i] += arrIIndex [j];
				}
			}

			m_MaxIRange = arrTmpIIndex [arrTmpIIndex.Length - 1];

		}
	}

	int ChooseNextItem()
	{
		int tmpIndex = 0;
		tmpIValue = Random.Range (0, m_MaxIRange);

		for (int i = 0; i < arrTmpIIndex.Length; i++) {
			if (tmpIValue < arrTmpIIndex [i]) {
				tmpIndex = i;
				break;
			}
		}

		return tmpIndex ;
	}


}
