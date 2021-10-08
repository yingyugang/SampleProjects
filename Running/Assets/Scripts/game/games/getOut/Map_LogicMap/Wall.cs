using UnityEngine;
using System.Collections;

public class Wall {
	public Vector2 m_Position;
	public int totalWallAround;
	public int idPrefabs;
	public bool haveWallUp;
	public bool haveWallDown;
	public bool haveWallLeft;
	public bool haveWallRight;
	public int idColor;
	public int idSprite;
	public Vector2 Position{
		get{ return m_Position;}
		set{m_Position = value;}
	}

	public int TotalWallAround{
		get{
			return totalWallAround;
		}
		set{ 
			totalWallAround = value;
		}
	}

	public Wall(Vector2 position)
	{
		totalWallAround = 0;
		idPrefabs = 0;
		m_Position = position;
	}

}
