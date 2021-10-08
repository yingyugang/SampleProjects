using UnityEngine;
using System.Collections;
using System.Collections.Generic;
[System.Serializable]
public class Terrain_RenderMap {
	public int ID;
	public string Name;
	public int Percentage;
	public int Appear_Distance;
	public float Max_Y;
	public float Min_Y;
	public int Appear_Distance_End;

	public Terrain_RenderMap(int ID, string name, int percent, int appear, float max_Y, float Min_y, int appear_end)
	{
		this.ID = ID;
		this.Name = name;
		this.Percentage = percent;
		this.Appear_Distance = appear;
		this.Max_Y = max_Y;
		this.Min_Y = Min_Y;
		this.Appear_Distance_End = appear_end;
	}
}
