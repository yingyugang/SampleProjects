using UnityEngine;
using System.Collections;
using System.Collections.Generic;
[System.Serializable]
public class Pattern_Info{
	public int[] id = new int[BikingKey.Terrain.CSVMeshPoint];
	public int[] map_id = new int[BikingKey.Terrain.CSVMeshPoint];
	public int[] index = new int[BikingKey.Terrain.CSVMeshPoint];
	public List<Vector2> points = new List<Vector2>();
	public List<KeyValuePair<int, Vector2>> itemPattern = new List<KeyValuePair<int, Vector2>>();
	public int[] itemPercent = new int[BikingKey.Terrain.CSVMeshPoint];
	public int percentage;
}
