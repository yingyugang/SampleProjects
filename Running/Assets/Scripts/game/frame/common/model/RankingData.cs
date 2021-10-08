using System;

[Serializable]
public class RankingData
{
	public int id;
	public int rank;
	public int player_id;
	public string head_image;
	public string name;
	public int score;

	public static RankingData GetInstance{ get; set; }

	public RankingData ()
	{
		GetInstance = this;
	}
}
