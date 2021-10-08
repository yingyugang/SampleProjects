using UnityEngine;
using System.Collections;

public class GameConfig   {

	public const float SCREEN_TARGET_WIDTH = 1080f;
	public const float SCREEN_TARGET_HEIGHT = 1920f;

	public const string TAG_ENEMY = "Enemy";
	public const string TAG_PLAYER = "Player";
	public const string VALUE_ANIMATOR = "value";
	public const string VALUE_ANIMATORSTARFOOTER = "isStarEffect";
	public const string VALUE_ANIMATORCLOCKFOOTER = "isClockEffect";
	public const int ANIM_LEFT = 1;
	public const int ANIM_RIGHT = 2;
	public const int ANIM_UNDER = 3;
	public const int ANIM_STOP_LEFT = 5;
	public const int ANIM_STOP_RIGHT = 6;
	public const int ANIM_STOP_UNDER = 7;
	public const int ANIM_STOP_UP = 8;
	public const int ANIM_UP = 4;
	public const int ANIM_IDLE_ENEMY = 0;

	public const int ID_CORNER_UP = 1;
	public const int ID_CORNER_DOWN = 2;
	public const int ID_CORNER_RIGHT = 3;
	public const int ID_CORNER_LEFT = 4;
	public const int ID_THE_FORK_UP = 5;
	public const int ID_THE_FORK_DOWN = 6;
	public const int ID_THE_FORK_RIGHT = 7;
	public const int ID_THE_FORK_LEFT = 8;
	public const int ID_LINE_HORIZONTAL = 9;
	public const int ID_LINE_VERTICLE = 10;
	public const int ID_LINE_END_UP = 11;
	public const int ID_LINE_END_DOWN = 12;
	public const int ID_LINE_END_LEFT = 13;
	public const int ID_LINE_END_RIGHT = 14;

	public const int ID_SPRITE_LINE_END = 0;
	public const int ID_SPRITE_LINE = 1;
	public const int ID_SPRITE_CORNER = 2;
	public const int ID_SPRITE_THE_FORK = 3;


	public const int INDEX_FLICKER = 6;						//number flicker
	public const float TIME_FLICKER = 0.5f;					//Time for once flicker when character or enemy init
	public const float TIME_FLICKER_CLOCK = 0.2f;
	public static float TIME_STAR = 30f;						//Time star
	public static float TIME_CLOCK = 30f;
	public static float SPEED_CLOCK = 0.5f;
	public static float TIME_INIT_ITEM = 30f;
	public static float TIME_DOOR_CLOSE = 3.5f;
	public static float PLAYER_SPEED_RATE;
	public static bool ITEM_ON = true;
	public static float FLICK_SENSITIVITY = 50f;
}
