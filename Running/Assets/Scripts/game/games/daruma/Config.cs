using UnityEngine;
using System.Collections;
namespace Daruma
{
    public class Config
    {
        public const float TIME_SWITCH_STATE = 0.2f; // Switching state between normal and flick
        public const float BLOCK_HEIGHT = 2.35f;      // Block's size ( height)
        public const int MAX_BLOCK = 7;
        public const float POS_GEN_Y = 10.0f; // Position to auto generate objects.
        public const float MAX_GEN_X = 1.5f;

        // Blocks
        public const string BLOCK_OSOMATU = "Osomatu";
        public const string BLOCK_KARAMATU = "Karamatu";
        public const string BLOCK_CHOCOMATU = "Chocomatu";
        public const string BLOCK_ICHIMATU = "Ichimatu";
        public const string BLOCK_JYUSHIMATU = "Jyushimatu";
        public const string BLOCK_TODOMATU = "Todomatu";
        public const string BLOCK_IYAMI = "Iyami";
        public const string BLOCK_TOTOKO = "Totoko";
        public const int TIME_TO_COUNTDOWN = 5;
        public const float VOLUME_BG_SOUND = 0.9f;  // 90%
        public const string INFO_COMBO = "InfoCombo";
    }
}

