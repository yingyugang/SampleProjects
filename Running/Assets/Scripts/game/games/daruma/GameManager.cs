using UnityEngine;
using System.Collections.Generic;
using System.Collections;
namespace Daruma
{
    public class GameManager : MonoBehaviour
    {
        public const int BLOCK_OSOMATU = 1;
        public const int BLOCK_KARAMATU = 2;
        public const int BLOCK_CHOCOMATU = 3;
        public const int BLOCK_ICHIMATU = 4;
        public const int BLOCK_JYUSHIMATU = 5;
        public const int BLOCK_TODOMATU = 6;
        public const int BLOCK_IYAMI = 7;
        public const int BLOCK_TOTOKO = 8;
        public const int ID_GAME = 1;
        [HideInInspector]
        public GameParameter gameParameter;

        public static GameManager s_Instance;
        //[HideInInspector]
        public float[] blocksRatio;
        //[HideInInspector]
        public float[] blocksRatioReduction;

        [HideInInspector]
        public float[] arrowRatio;
        private const int ARROW_NONE = 0;
        private const int ARROW_LEFT = 1;
        private const int ARROW_RIGHT = 2;
        // Background ingame sound
        private AudioSource m_BgSoundInGame;

        public int countTrueFlick { get; set; }
        [HideInInspector]
        public float currentGameTime;

		//add by sya
		private int _combMax;
        [HideInInspector]

		public int comboMax{
			get {
				return _combMax >> 1;
			}
			set {
				_combMax = value << 1;
			}
		}

		//add by sya
		private int _score;
        [HideInInspector]
		public int score {
			get {
				return _score >> 1;
			}
			set {
				_score = value << 1;
			}
		}

        public bool isPause { get; set; }
        [HideInInspector]
        public Direction arrowDirection;
        [HideInInspector]
        public int countTotokoItem;
        [HideInInspector]
        public int countIyamiItem;
        [HideInInspector]
        public bool didGameOver;

        #region EndLogic will be send to the server
        public GameEndLogic paramsEndLogic;
        // Params for sending to server
        [HideInInspector]
        public int blockNum;
        [HideInInspector]
        public int playTime;
        [HideInInspector]
        public int comboNum;
        [HideInInspector]
        public int noMiss = 1;

        [HideInInspector]
        public bool isPauseGame = false;
        [HideInInspector]
        public float m_OriginBgVolume;

		float cardBonus;

        IEnumerator InitPreloadedSound()
        {
            ComponentConstant.SOUND_MANAGER.StoreSounds(new List<SoundEnum>()
            {
                SoundEnum.bgm03_daruma,
                SoundEnum.bgm15_title_back
        }, InitCommon);
            yield return null;
        }

        private void InitCommon()
        {
            // Init header
            Header.Instance.SetLife(LifeType.Time, (int)gameParameter.game_time);
            Header.Instance.SetScore(GameManager.s_Instance.score.ToString() + "コ");
            Header.Instance.onPause += OnPauseGame;
            Header.Instance.popupCountDown.showOrHideBg += InitPausePanel;
            PauseManager.s_Instance.SetOnBackToGameCallback(OnResumeGame);

            // Init result panel
            GameResaultManager.Instance.SetImageHeaderPanelResault(ID_GAME);
        }

        // end
        public void SendGameEndingAPI()
        {
			BlockManager.s_Instance.ResetCombo ();
			int comboCount = 0;
			int count = BlockManager.s_Instance.countTimeCombo.Count;
			for (int i = 0; i < count; i++)
			{
				comboCount += BlockManager.s_Instance.countTimeCombo[i];
			}
			paramsEndLogic.score =  Mathf.RoundToInt(CalcSumScore() * (1 + this.cardBonus / 100));
			paramsEndLogic.card_bonus = cardBonus;
			paramsEndLogic.card_ids = CardRate.GetAdditionalCardID (2,ID_GAME);
            paramsEndLogic.m_game_id = ID_GAME;
            paramsEndLogic.blocks_num = blockNum;
            paramsEndLogic.play_time = playTime;
			paramsEndLogic.combo_num = comboMax; //last time is comboCount
            paramsEndLogic.nomiss = noMiss;
            paramsEndLogic.complete = SendAPICompleteHandler;
            paramsEndLogic.error = SendAPIErrorHandler;
            paramsEndLogic.SendAPI();

            Debug.Log("paramsEndLogic.score: " + (int)CalcSumScore());
            Debug.Log("paramsEndLogic.m_game_id: " + ID_GAME);
            Debug.Log("paramsEndLogic.blocks_num: " + blockNum);
            Debug.Log("paramsEndLogic.play_time: " + playTime);
			Debug.Log("paramsEndLogic.combo_num: " + comboCount);
			//Debug.Log("paramsEndLogic.combo_num: " + comboCount);
            Debug.Log("paramsEndLogic.nomiss: " + noMiss);
        }

        public void ResetVolume()
        {
//            if (m_BgSoundInGame != null)
//            {
//                m_BgSoundInGame.volume = m_OriginBgVolume;
//            }
        }

        void SendAPICompleteHandler()
        {
            Debug.Log("SendAPICompleteHandler!" + UpdateInformation.GetInstance.game_list[ID_GAME].score);
            Debug.Log("SendAPICompleteHandler!" + APIInformation.GetInstance.rank);
            Debug.Log("SendAPICompleteHandler!" + Player.GetInstance.lv);
            Debug.Log("SendAPICompleteHandler!" + Player.GetInstance.exp);
            int rank = APIInformation.GetInstance.rank;
            SetGameOverInfo(rank);
        }

        private float CalcSumScore()
        {
            float score = GameManager.s_Instance.score * gameParameter.comb_var1;
            float scorecomboBonus = 0;
            int count = BlockManager.s_Instance.countTimeCombo.Count;
            for (int i = 0; i < count; i++)
            {
                scorecomboBonus += BlockManager.s_Instance.countTimeCombo[i];
            }
            scorecomboBonus *= gameParameter.comb_var2;
			float scoreComboMaxBonus = ((float)GameManager.s_Instance.comboMax * gameParameter.comb_var3);

            return score + scorecomboBonus + scoreComboMaxBonus;
        }

        private void SetGameOverInfo(int rank = 0)
        {
            float score = GameManager.s_Instance.score * gameParameter.comb_var1;
            float scorecomboBonus = 0;
            int count = BlockManager.s_Instance.countTimeCombo.Count;
            for (int i = 0; i < count; i++)
            {
                scorecomboBonus += BlockManager.s_Instance.countTimeCombo[i];
            }
            scorecomboBonus *= gameParameter.comb_var2;
			float scoreComboMaxBonus = ((float)GameManager.s_Instance.comboMax * gameParameter.comb_var3);
            int highscore = GameConstant.gameDetail.score; // It's high score.

            if (ComponentConstant.SOUND_MANAGER != null)
            {
                ComponentConstant.SOUND_MANAGER.StopBGM();
            }
            Debug.Log("Rank: " + rank);
			GameResaultManager.Instance.SetGameResultInformation((int)score, (int)scorecomboBonus, (int)scoreComboMaxBonus, rank,cardBonus, true, false, (rank > 0));
        }

        void SendAPIErrorHandler(string str)
        {
            SetGameOverInfo();
            Debug.Log("SendAPIErrorHandler! " + str);
        }

        private void OnPauseGame()
        {
            if (!GameCountDownMediator.didEndCountDown)
            {
                return;
            }
            isPauseGame = true;
            PoolManager.s_Instance.DesSpawn(Config.INFO_COMBO);
            PauseManager.s_Instance.SetVisible(true);
            Header.Instance.isPause = isPauseGame; // stop cout time
            //Time.timeScale = 0;
        }
        #endregion

        private void OnResumeGame()
        {
            isPauseGame = false;
            Header.Instance.isPause = isPauseGame;
            //Time.timeScale = 1;
        }

        private void OnDisable()
        {
            Header.Instance.onPause -= OnPauseGame;
            PauseManager.s_Instance.RemoveOnBackToGameCallback(OnResumeGame);
        }

        public void GoHome()
        {
            ComponentConstant.SCENE_COMBINE_MANAGER.LoadScene(SceneEnum.Home);
        }

        public void InitBlockRatio()
        {
            // Block;
            blocksRatio = new float[(int)BlockType.Totoko + 1];
            blocksRatioReduction = new float[(int)BlockType.Totoko + 1];
            DataManager.ReadListRatioBlock();
        }

        public void InitArrowRatio()
        {
            // Arrow
            arrowRatio = new float[3];
            arrowRatio[ARROW_NONE] = 100 - GameManager.s_Instance.gameParameter.arrow_percentage;
            arrowRatio[ARROW_LEFT] = GameManager.s_Instance.gameParameter.arrow_percentage / 2;
            arrowRatio[ARROW_RIGHT] = arrowRatio[ARROW_LEFT];
        }

        public void InitRanking()
        {
            //Global_GameOver.instance.GameOver_Element.rankPoint = DataManager.ReadRanking();
        }

        void InitPausePanel(bool value = false)
        {
            //Int pause panel
            if (ComponentConstant.SOUND_MANAGER != null)
            {
                ComponentConstant.SOUND_MANAGER.Play(SoundEnum.bgm03_daruma, (audio) =>
                {
                    m_BgSoundInGame = audio;
//                    m_OriginBgVolume = m_BgSoundInGame.volume;
//                    m_BgSoundInGame.volume = Config.VOLUME_BG_SOUND;
                    if (m_BgSoundInGame != null)
                    {
                        PauseManager.s_Instance.Init(ID_GAME, m_BgSoundInGame);
                    }
                });
            }
        }

        public void Init()
        {
            score = 0;
            DataManager.ReadListParams();
            InitRanking();
            InitArrowRatio();
            InitBlockRatio();
            didGameOver = false;
            countTotokoItem = 0;
            countIyamiItem = 0;

            // Init header
            Header.Instance.SetLife(LifeType.Time, (int)gameParameter.game_time);
            Header.Instance.SetScore(GameManager.s_Instance.score.ToString() + "コ");
            Header.Instance.onPause += OnPauseGame;
            Header.Instance.popupCountDown.showOrHideBg += InitPausePanel;
            PauseManager.s_Instance.SetOnBackToGameCallback(OnResumeGame);

            // Init result panel
            GameResaultManager.Instance.SetImageHeaderPanelResault(ID_GAME);
			GameResaultManager.Instance.SetLastLevel(UpdateInformation.GetInstance.player.lv,UpdateInformation.GetInstance.player.exp);

            // Init parms will be send to the server when ending game.
            blockNum = 0;
            comboNum = 0;
            playTime = (int)gameParameter.game_time;
            noMiss = 1;

            StartCoroutine(InitPreloadedSound());
        }

        void Start()
        {
            Init();
        }
        private void Awake()
        {
            s_Instance = this;
			cardBonus = GetCardBonus ();
        }

		float GetCardBonus(){
			//CardRate.GetTotal (2,GAME_ID);
			return CardRate.GetTotal (2,ID_GAME);
		}
    }
}
