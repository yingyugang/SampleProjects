using UnityEngine;
using System.Collections;

namespace BikingKey
{
	public class GameConfig
	{
		public static int GameID = 6;
		public static float timerebirth = 3.2f;
		public static int multiJump = 2;
		public static int GameOverID = 13;
		public static int itemCount = 1;
		public static int MAX_ITEM_ID = 11;
		public static float BackgroundFadeDelay = 5.0f;
		public static float BackgroundFadeSpeed = .01f;
		public static float MaxSpeed = 9.5f;
		public static float JumPower = 20.0f;
		public static float JumRamp = 30.0f;
		public static float PlayerStartPosition = -20.0f;
		public static float JumDamp = 1.40f;
		public static float PlayerDistanceFromGround = 0.330f;
		public static float timeToMaxSpeed = 1.0f;
		public static float MaxDistance = float.MaxValue;
		public static int DistanceToChangePattern = 1000;
		public static int DistanceToReleaseItem = 100;
		public static int DistanceItemDemo = 600;
		public static string Dekapan = "Dekapan_Hit";
		public static string Score = "Score";
		public static string DiePosition = "DiePosition";
		public static int MaxMapID = 2000;

		public const string NameItemReborn = "ItemLifePlayer";
		public const float DistanceItemLife = 100f;
		public const float DistanceExistsLife = 300f;
	}

	public class ImageResources
	{
		public static string item1 = "scoreItem_1";
		public static string item2 = "scoreItem_2";
		public static string item3 = "scoreItem_3";
	}

	public class Tags
	{
		public static string obstacle = "Obstacle";
		public static string item = "Item";
		public static string Player = "Player";
	}

	public class Terrain
	{
		public static Vector3 MeshStartPosition = new Vector3 (.0f, -20.0f, .0f);
		public static int Resolution = 40;
		public static int Block = 2;
		public static int CSVMeshPoint = 100;
		public static int Width = 5000;
		public static float TerrainPixel_Standard = 0.05f;
		public static float HeightLifePointValue = 150.0f;
		public static float CordinateMulti = 50.0f;
		public static float Factor = 25;
		public static float CliffTriggerHeight = 0.55f;
		public static float AbyssDeep = 4;
		public static float CliffDeep = 1.50f;
		public static string Standard = "Terrain_Standard";
		public static string Bridge = "Terrain_Bridge";
		public static string Terrain_Collision = "Terrain_Collision";
		public static string Terrain_Abyss = "Terrain_Abyss";
		public static string EmptySlot = "Terrain_Bridge";
		public static string AbyssInvulnerable = "AbyssInvulnerable";
		public static string AbyssInvulnerableHeader = "AbyssInvulnerableHeader";
		public static string Cliff = "cliff";
		public static string HeightLifePoint = "HeightLifePoint";
		public static string Border_Ground = "blur_";
		public static string Border_Grass = "course1_grass_left";
		public static string Border = "Terrain_Border";
		public static string BorderLeft = "BorderLeft";
		public static string BorderRight = "BorderRight";
		public static string Route = "Route";
	}

	public class ShaderIncl
	{
		public static string Unlit_Transparent = "Unlit/Transparent";
		public static string Unlit_TransparentCutout = "Unlit/Transparent Cutout";
		public static string Unlit_Texture = "Unlit/Texture";
		public static string Mobile_Diff = "Mobile/Diffuse";
	}

	public class SSTring
	{
		public static string Must_Destroy = "MustDestroy";
		public static string CurrentEXP = "currentEXP";
		public static string PlayerEXP_Server = "PlayerEXP_Server";
		public static string EXPLvlUP = "EXPLvlUP";
		public static string PlayerCurrentLvl = "PlayerCurrentLvl";
		public static int IDGeneration = 0;
	}
}
