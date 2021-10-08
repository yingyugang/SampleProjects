using UnityEngine;
using System.Collections;
using System.Collections.Generic;
namespace Daruma
{
    public class DataManager
    {
        public static List<int> ReadRanking()
        {
            List<int> list = new List<int>();
            List<Dictionary<string, object>> data = CSVReader.Read("csv/m_dharma_dropping_rank");
            for (int i = 0; i < data.Count; i++)
            {
                int max = (int)data[i]["max"];
                list.Add(max);
            }
            return list;
        }

        public static void ReadListParams()
        {
            if (APIInformation.GetInstance == null) // run game from scene (not home scene) (for test only)
            {
                List<Dictionary<string, object>> data = CSVReader.Read("csv/m_dharma_dropping_parameter");
                if (data != null)
                {
                    Dictionary<string, float> dict = new Dictionary<string, float>();
                    for (int i = 0; i < data.Count; i++)
                    {
                        string para_name = (string)data[i]["para_name"].ToString();
                        float value = float.Parse(data[i]["value"].ToString());
                        dict.Add(para_name, value);
                    }

                    // Set value
                    if (GameManager.s_Instance.gameParameter == null)
                    {
                        GameManager.s_Instance.gameParameter = new GameParameter();
                    }

                    GameManager.s_Instance.gameParameter.game_time = (int)dict["game_time"];
                    GameManager.s_Instance.gameParameter.totoko_add_second = (int)dict["totoko_add_second"];
                    GameManager.s_Instance.gameParameter.flick_speed = (int)dict["flick_speed"];
                    GameManager.s_Instance.gameParameter.down_speed = (int)dict["down_speed"];
                    GameManager.s_Instance.gameParameter.flick_sensitivity = (int)dict["flick_sensitivity"];
                    GameManager.s_Instance.gameParameter.comb_second = dict["comb_second"];
                    GameManager.s_Instance.gameParameter.arrow_percentage = (int)dict["arrow_percentage"];
                    GameManager.s_Instance.gameParameter.totoko_animation_speed = dict["totoko_animation_speed"];
                    GameManager.s_Instance.gameParameter.target_animation_speed = (int)dict["target_animation_speed"];
                    GameManager.s_Instance.gameParameter.total_comb_var = dict["total_comb_var"];
                    GameManager.s_Instance.gameParameter.comb_var1 = dict["comb_var1"];
                    GameManager.s_Instance.gameParameter.comb_var2 = dict["comb_var2"];
                    GameManager.s_Instance.gameParameter.comb_var3 = dict["comb_var3"];
                }
            }
            else // run game from home scene
            {
                GameManager.s_Instance.gameParameter = APIInformation.GetInstance.gameparameter;
            }
        }

        private static void SetValueRatioBlock(int id, float percentageappear, float reduction)
        {
            switch (id)
            {
                case GameManager.BLOCK_OSOMATU:
                    GameManager.s_Instance.blocksRatio[(int)BlockType.Osomatu] = percentageappear;
                    GameManager.s_Instance.blocksRatioReduction[(int)BlockType.Osomatu] = reduction;
                    break;
                case GameManager.BLOCK_KARAMATU:
                    GameManager.s_Instance.blocksRatio[(int)BlockType.Karamatu] = percentageappear;
                    GameManager.s_Instance.blocksRatioReduction[(int)BlockType.Karamatu] = reduction;
                    break;
                case GameManager.BLOCK_CHOCOMATU:
                    GameManager.s_Instance.blocksRatio[(int)BlockType.Chocomatu] = percentageappear;
                    GameManager.s_Instance.blocksRatioReduction[(int)BlockType.Chocomatu] = reduction;
                    break;
                case GameManager.BLOCK_ICHIMATU:
                    GameManager.s_Instance.blocksRatio[(int)BlockType.Ichimatu] = percentageappear;
                    GameManager.s_Instance.blocksRatioReduction[(int)BlockType.Ichimatu] = reduction;
                    break;
                case GameManager.BLOCK_JYUSHIMATU:
                    GameManager.s_Instance.blocksRatio[(int)BlockType.Jyushimatu] = percentageappear;
                    GameManager.s_Instance.blocksRatioReduction[(int)BlockType.Jyushimatu] = reduction;
                    break;
                case GameManager.BLOCK_TODOMATU:
                    GameManager.s_Instance.blocksRatio[(int)BlockType.Todomatu] = percentageappear;
                    GameManager.s_Instance.blocksRatioReduction[(int)BlockType.Todomatu] = reduction;
                    break;
                case GameManager.BLOCK_IYAMI:
                    GameManager.s_Instance.blocksRatio[(int)BlockType.Iyami] = percentageappear;
                    GameManager.s_Instance.blocksRatioReduction[(int)BlockType.Iyami] = reduction;
                    break;
                case GameManager.BLOCK_TOTOKO:
                    GameManager.s_Instance.blocksRatio[(int)BlockType.Totoko] = percentageappear;
                    GameManager.s_Instance.blocksRatioReduction[(int)BlockType.Totoko] = reduction;
                    break;
            }
        }

        public static void ReadListRatioBlock()
        {
            List<Dictionary<string, object>> data = CSVReader.Read("csv/m_dharma_dropping_character");
            for (int i = 0; i < data.Count; i++)
            {
                int id = int.Parse(data[i]["id"].ToString());
                float percentageappear_probability = float.Parse(data[i]["percentageappear_probability"].ToString());
                float reduction_percentage = float.Parse(data[i]["reduction_percentage"].ToString());
                SetValueRatioBlock(id, percentageappear_probability, reduction_percentage);
            }
			BlockManager.s_Instance.GetSpecificBlockPercent ();
        }
    }
}
