using System;

[Serializable]
public class RankingTop
{
	public int total_pt;
	public int total_rank;
	public int card_num;
	public int card_rank;
	public int game1_pt;
	public int game1_rank;
	public int game2_pt;
	public int game2_rank;
	public int game3_pt;
	public int game3_rank;
	public int game4_pt;
	public int game4_rank;
	public int game5_pt;
	public int game5_rank;
	public int game6_pt;
	public int game6_rank;
	public int game7_pt;
	public int game7_rank;
	public int game8_pt;
	public int game8_rank;
	public int game9_pt;
	public int game9_rank;

	public int[] this[int index]
	{
		get
		{
			switch (index)
			{
				case 0:
					return new int[] { game1_pt, game1_rank };
				case 1:
					return new int[] { game2_pt, game2_rank };
				case 2:
					return new int[] { game3_pt, game3_rank };
				case 3:
					return new int[] { game4_pt, game4_rank };
				case 4:
					return new int[] { game5_pt, game5_rank };
				case 5:
					return new int[] { game6_pt, game6_rank };
				case 6:
					return new int[] { game7_pt, game7_rank };
				case 7:
					return new int[] { game8_pt, game8_rank };
				case 8:
					return new int[] { game9_pt, game9_rank };
				default:
					return null;
			}
		}
	}

	public static RankingTop GetInstance { get; set; }

	public RankingTop ()
	{
		GetInstance = this;
	}
}