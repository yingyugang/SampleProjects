using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class MapNode
{
	// Use this for initialization
	public Node[,] matrix = new Node[Map.WIDTH, Map.HEIGHT];
	public  List<Vector2> arrayAreaA;
	public  List<Vector2> arrayAreaC;
	public  List<Vector2> arrayAreaB;
	public  List<Vector2> arrayAreaD;
	public  List<Vector2> arrayEdgeOfMap;
	public MapNode(int[,] m_matrix)
	{
		arrayAreaA = new List<Vector2>();
		arrayAreaC = new List<Vector2>();
		arrayAreaD = new List<Vector2>();
		arrayAreaB = new List<Vector2>();
		arrayEdgeOfMap = new List<Vector2>();
		for (int i = 0; i < Map.WIDTH; i++)
			for (int j = 0; j < Map.HEIGHT; j++) {
				if (Map.IsNotWall(i, j) && Map.s_Matrix[i,j] != (int)TypeWall.Block && Map.s_Matrix[i,j] != (int)TypeWall.Door) {
					if(CheckPosiblePosition(i,j))
					{
						matrix[i,j] = new Node(new Vector2(i,j));
						if ((i - 1 >= 0) && matrix [i - 1, j] != null)
							matrix [i, j].MakeLink (ref matrix [i - 1, j]);
						if ((j - 1 >= 0) && matrix [i, j - 1] != null)
							matrix [i, j].MakeLink (ref matrix [i, j - 1]);
					}else
						matrix[i,j] = null;
				}
				/* 
				 -->| by anhgh
				else
					if(Map.s_Matrix[i,j] == (int)TypeWall.Block)
					{
						GetOut.GameManager.Instance().hideAreaShow = 1;
					}
				 --<| by anhgh 
				 */
			}
		LeftTopPosition ();
		LeftBotPosition ();
		RightTopPosition ();
		RightBotPosition ();
	}

	public bool CheckPosiblePosition(int i, int j)
	{
		if(i >= 2 && j+1 <= Map.HEIGHT-2)
		if (Map.IsPath(i - 1, j + 1) && Map.IsPath(i-1,j) && Map.IsPath(i,j+1)) {
				return true;
			}
		return false;
	}


	public void LeftTopPosition()
	{
		Vector2 position;
		for (int i = 1; i < (Map.WIDTH / 2); i++)
			for (int j = 1; j < (Map.HEIGHT / 2); j++) {
				if(matrix[i,j] != null && CheckDirection(i,j))
				{
					arrayAreaA.Add(matrix[i,j].Position);
					if(i == 2 || i == Map.WIDTH -2 || j == 1 || j == Map.HEIGHT -3)
					{
						arrayEdgeOfMap.Add(matrix[i,j].Position);
					}
				}
			}
		
	}

	public void LeftBotPosition()
	{
		Vector2 position;
		for (int i = (int)(Map.WIDTH *0.6f); i < Map.WIDTH; i++)
			for (int j = 1; j < (Map.HEIGHT / 2); j++) {
				if(matrix[i,j] != null && CheckDirection(i,j))
				{
					arrayAreaC.Add(matrix[i,j].Position);
					if(i == 2 || i == Map.WIDTH -2 || j == 1 || j == Map.HEIGHT -3)
						arrayEdgeOfMap.Add(matrix[i,j].Position);
				}
			}
	}

	public void RightTopPosition()
	{
		Vector2 position;
		for (int i = 1; i < (Map.WIDTH/2); i++)
			for (int j = (Map.HEIGHT/2); j < Map.HEIGHT; j++) {
				if(matrix[i,j] != null && CheckDirection(i,j))
				{
					arrayAreaB.Add(matrix[i,j].Position);
					if(i == 2 || i == Map.WIDTH -2 || j == 1 || j == Map.HEIGHT -3)
						arrayEdgeOfMap.Add(matrix[i,j].Position);
				}
			}
	}

	public void RightBotPosition()
	{
		Vector2 position;
		for (int i = (int)(Map.WIDTH*0.6f); i < Map.WIDTH; i++)
			for (int j = (Map.HEIGHT/2); j < Map.HEIGHT; j++) {
				if(matrix[i,j] != null && CheckDirection(i,j))
				{
					arrayAreaD.Add(matrix[i,j].Position);
					if(i == 2 || i == Map.WIDTH -2 || j == 1 || j == Map.HEIGHT -3)
						arrayEdgeOfMap.Add(matrix[i,j].Position);
				}
			}
	}

	//Check 4 direction around a point in logic matrix
	//don't set the character in the box
	public bool CheckDirection (int x, int y)
	{
		List<Direction> lstDirectionPosible = new List<Direction>();
		lstDirectionPosible.Clear ();

		Node node;
		for(int i = 0; i < matrix[x,y].Next.Count; i++)
		{
			node = matrix[x,y].Next[i];
			if(node.Position.x == x && node.Position.y > y)
				lstDirectionPosible.Add(Direction.Right);
			else if(node.Position.x == x && node.Position.y < y)
				lstDirectionPosible.Add(Direction.Left);
			else if(node.Position.y == y && node.Position.x < x)
				lstDirectionPosible.Add(Direction.Up);
			else if(node.Position.y == y && node.Position.x > x)
				lstDirectionPosible.Add(Direction.Down);
		}

		return (lstDirectionPosible.Count > 2);

	}
}
