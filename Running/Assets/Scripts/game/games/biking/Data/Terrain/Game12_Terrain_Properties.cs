using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Game12_Terrain_Properties {
	//1000=20xblock(50);
	// This is a terrain struct(pattern), include 10 child pattern.
	// I do that because we can switch pattern'child each other if recall, so diversity for hills.
	public class Pattern{
		public static List<Pattern_Info> terrainList = new List<Pattern_Info>();
		public static List<float[]> terrainAbyssList = new List<float[]>();
		public static List<int[]> terrainItemList = new List<int[]>();
	}
	public class TerrainDat{
		
		// The first pattern for standard parttern where used for anime intro
		public static float[] _standard_pattern = new float[12]{1000,250,0,0,0,0,0,0,0,0,0,250};
		// When the background was change, the terrain texture should change follow, so that need a abyss terrain to spilit different texture,
		// that's good for terrain, smoothly.

		public static float[] _pattern_bridge = new float[12]{1000,250,0,0,0,0,0,0,0,0,0,250};
	}
	public class TerrainAbyssDat{
		public static float[] _standard_pattern = new float[5]{0,0,0,0,0};
		public static float[] _pattern_bridge = new float[5]{000100,0,0,0,0};
	}
	public class TerrainItem{
		public static int[] _standard_pattern = new int[10]{0,0,0,0,0,0,0,0,0,0};
		public static int[] _pattern_bridge = new int[10]{0,0,0,0,0,0,0,0,0,0};
	}
}
