using UnityEngine;
using System.Collections;

public enum Block : int
{
	Stone = 0,
	Fossil,
	Plaster,
	Boom,
	Treasure1,
	Treasure2,
	Treasure3,
	Treasure4,
	Treasure5,
	Treasure6,
	Item1,
	Item2,
	Item3,
	Item4,
	Item5,
	Item6,
	Fever,
	Limit
}
	

public class OilKing_Block : MonoBehaviour
{
	public Block nameBlock;

	public Sprite imgBlock;

	public int hitNumber;

	public int coinMin;
	public int coinmax;

	public bool canThrow;
	public bool canMove;
	public int brokenable; //1: it broken, 2: it explosive;



	//0 <= value <= rStone : Stone appear
	private float m_RangeStone;
	private float m_RangeBoom;
	private float m_RangeFossil;
	private float m_RangePlaster;
	private float m_RangeTreasure1;
	private float m_RangeTreasure2;
	private float m_RangeTreasure3;
	private float m_RangeTreasure4;
	private float m_RangeTreasure5;
	private float m_RangeTreasure6;

	private float m_MaxRange;

	private float[] arrIndex = new float[10];
	private float[] arrTmpIndex = new float[10];

	private int blockAppear;

	private float tmpValue;

	IEnumerator Start()
	{
		yield return new WaitForEndOfFrame ();
		GetValuePercent ();
		Init ();
	}

	void OnEnable()
	{
		Init ();
	}


	void Init()
	{
		blockAppear = OilKingGamePlay.Instance.isFever ? (int)Block.Fever - 6 : ChooseNextBlock ();
		if(OilKingCSV.s_Instance != null)
		{
			this.nameBlock = OilKingCSV.s_Instance.lstBlockAttribute[blockAppear].typeBlock;
			this.imgBlock = OilKingCSV.s_Instance.lstBlockAttribute [blockAppear].imgBlock;
			if(OilKingGamePlay.Instance.isFever)
			{
				Texture2D tex = this.imgBlock.texture;
				this.imgBlock = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.551899f, 0.3855862f), 100.0f);
			}
			this.hitNumber = OilKingCSV.s_Instance.lstBlockAttribute[blockAppear].timeBreak - 1;
			this.canThrow = OilKingCSV.s_Instance.lstBlockAttribute[blockAppear].throwable;
			this.canMove = OilKingCSV.s_Instance.lstBlockAttribute[blockAppear].moveable;
			this.coinmax = OilKingCSV.s_Instance.lstBlockAttribute[blockAppear].coinMax;
			this.coinMin = OilKingCSV.s_Instance.lstBlockAttribute[blockAppear].coinMin;
			this.brokenable = OilKingCSV.s_Instance.lstBlockAttribute[blockAppear].brokenable;
			this.GetComponent <SpriteRenderer>().sprite = this.imgBlock;
		}
	}

	void GetValuePercent()
	{
		if(OilKingCSV.s_Instance != null) {


			m_RangeStone = ParameterServer.lstProbabilityBlock [0];
			m_RangeBoom = ParameterServer.lstProbabilityBlock [3];
			m_RangeFossil = ParameterServer.lstProbabilityBlock [1];
			m_RangePlaster = ParameterServer.lstProbabilityBlock [2];
			m_RangeTreasure1 = ParameterServer.lstProbabilityBlock [4]/6;
			m_RangeTreasure2 = ParameterServer.lstProbabilityBlock [4]/6;
			m_RangeTreasure3 = ParameterServer.lstProbabilityBlock [4]/6;
			m_RangeTreasure4 = ParameterServer.lstProbabilityBlock [4]/6;
			m_RangeTreasure5 = ParameterServer.lstProbabilityBlock [4]/6;
			m_RangeTreasure6 = ParameterServer.lstProbabilityBlock [4]/6;

			arrIndex [0] = m_RangeStone;
			arrIndex [1] = m_RangeFossil;
			arrIndex [2] = m_RangePlaster;
			arrIndex [3] = m_RangeBoom;
			arrIndex [4] = m_RangeTreasure1;
			arrIndex [5] = m_RangeTreasure2;
			arrIndex [6] = m_RangeTreasure3;
			arrIndex [7] = m_RangeTreasure4;
			arrIndex [8] = m_RangeTreasure5;
			arrIndex [9] = m_RangeTreasure6;

			arrTmpIndex [0] = arrIndex [0];

			for (int i = 1; i < arrIndex.Length; i++) {
				for (int j = 0; j <= i; j++) {
					arrTmpIndex [i] += arrIndex [j];
				}
			}

			m_MaxRange = arrTmpIndex [arrTmpIndex.Length - 1];
				
		}
	}

	int ChooseNextBlock()
	{
		int tmpIndex = 0;
		tmpValue = Random.Range (0, m_MaxRange);



		for (int i = 0; i < arrTmpIndex.Length; i++) {
			if (tmpValue < arrTmpIndex [i]) {
				tmpIndex = i;
				break;
			}
		}
		return tmpIndex;
	}

}
