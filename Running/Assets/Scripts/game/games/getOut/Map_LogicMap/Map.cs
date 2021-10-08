using UnityEngine;
using System.Collections;

public class Map
{
	public  const int WIDTH = 37;
	public const int HEIGHT = 58;
	public const float SizeBlock = 0.44f;//Block size

	public static Vector3[] DeltaDirection = new Vector3[]{
		new Vector3(SizeBlock,0f,0f),
		new Vector3(-SizeBlock,0f,0f),
		new Vector3(0f,SizeBlock,0f),
		new Vector3(0f,-SizeBlock,0f),
	};
	public const int GateLeftX = 7;
	public const int GateRightX = 49;
	public const int GateY1 = 2 ;
	public const int GateY2 = WIDTH-2;
	public const float Eps = 0.1f;
	public static int [,] s_Matrix;
	private static bool[] arrayValueOfWall = {true,false,false,false,false,false,false,false,true,true};	//if arrayValueOfPath[value of block] = true that block is wall
	private static bool[] arrayValueOfPath = {true,false,false,false,false,false,false,false,false};	//if arrayValueOfPath[valueOfBlock] = true that block is path
	public static bool IsNotWall(int x, int y)
	{
		if((x < Map.WIDTH && x >= 0) && (y < Map.HEIGHT && y >= 0) )
		{
			return arrayValueOfWall[s_Matrix[x,y]];
		}else
			return false;
		return true;
	}

	public static bool IsPath(int x, int y)
	{
		if((x < Map.WIDTH && x >= 0) && (y < Map.HEIGHT && y >= 0) )
		{
			return arrayValueOfPath[s_Matrix[x,y]];
		}else
			return false;
		return true;
	}
}
