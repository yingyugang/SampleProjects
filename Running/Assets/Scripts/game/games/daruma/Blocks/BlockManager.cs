using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
namespace Daruma
{
    public class BlockManager : MonoBehaviour
    {
		public BlockType[] listBlockType = new BlockType[]
            {   BlockType.Chocomatu,
                BlockType.Ichimatu,
                BlockType.Jyushimatu,
                BlockType.Karamatu,
                BlockType.Osomatu,
                BlockType.Todomatu
            };

        [HideInInspector]
        public float currentComboSecond;
        [HideInInspector]
        public int currentCombo;
        [HideInInspector]
        public bool checkingCombo;
        public bool checkCountComboNum = false;

        private int currentTime; // using this for play sound countdown;

        //[HideInInspector]
        public List<int> countTimeCombo; //Using for calc bonus combo

        public static BlockManager s_Instance;

        //[HideInInspector]
        public List<Block> blocksRef;   // Use this for saving total existing current target list on screen.

        [HideInInspector]
        public Block currentBlockTarget; // Use this for saving current value of target.
        [HideInInspector]
        public Direction currentArrow = Direction.None;
        [HideInInspector]
        public Direction nextArrow = Direction.None;

        [HideInInspector]
        public Block nextBlockTarget;    // Use this for saving current value of target.

        private void Awake()
        {
            s_Instance = this;
        }

        private void Start()
        {
            StartCoroutine(StartBlockManager());
        }

        IEnumerator StartBlockManager()
        {
            yield return new WaitForEndOfFrame();
            countTimeCombo = new List<int>();
            blocksRef = new List<Block>();
            ResetCombo();
            Init();
        }

        public void ResetCombo()
        {
            if (currentCombo > 1)
            {
                countTimeCombo.Add((currentCombo - 1));
            }
            currentComboSecond = 0;
            currentCombo = 0;
            checkingCombo = false;
            checkCountComboNum = true;
        }

        private void Update()
        {
            if (GameManager.s_Instance.isPauseGame || !GameCountDownMediator.didEndCountDown)
            {
                return;
            }

            if (Header.Instance.GetLifeTime() <= 0 && !GameManager.s_Instance.didGameOver)
            {
                GameManager.s_Instance.didGameOver = true;
                PoolManager.s_Instance.DesSpawn(Config.INFO_COMBO);
                UIManager.s_Instance.panelSlideShow.SetActive(true);

                if (ComponentConstant.SOUND_MANAGER != null)
                {
                    GameManager.s_Instance.ResetVolume();
                    ComponentConstant.SOUND_MANAGER.Play(SoundEnum.bgm15_title_back);
                    ComponentConstant.SOUND_MANAGER.Play(SoundEnum.se07_timeup);
                }
                return;
            }

            int time = (int)Header.Instance.GetLifeTime();
            if (time < currentTime)
            {
                currentTime = time;
                if (ComponentConstant.SOUND_MANAGER != null)
                {
                    ComponentConstant.SOUND_MANAGER.Play(SoundEnum.se06_timeup_countdown);
                }
            }

            if (time > Config.TIME_TO_COUNTDOWN)
            {
                currentTime = Config.TIME_TO_COUNTDOWN + 1;
            }

            if (checkingCombo)
            {
                currentComboSecond += Time.deltaTime;
                if (currentComboSecond > GameManager.s_Instance.gameParameter.comb_second)
                {
                    ResetCombo();
                }
            }
        }

        public void CheckShowCombo(Vector2 direction, Vector3 posTarget)
        {
            // Right target.
			//Debug.Log(GameManager.s_Instance.comboNum);
            if (checkingCombo && currentComboSecond < GameManager.s_Instance.gameParameter.comb_second)
            {
                currentCombo++;
                currentComboSecond = 0;
                UIManager.s_Instance.SetTextComboMax(GameManager.s_Instance.comboMax.ToString());
                // Vector3 pos = new Vector3(direction.x > 0 ? -2.5f : 2.5f, 0, 0);
                Vector3 pos = new Vector3(-2.8f, 0, 0);
                PoolManager.s_Instance.DesSpawn(Config.INFO_COMBO);
                if (currentCombo >= 2)
                {
                    if (checkCountComboNum)
                    {
                        GameManager.s_Instance.comboNum++;
                        //checkCountComboNum = false;
                    }

                    GameObject infocombo = PoolManager.s_Instance.GetFreeObject(Config.INFO_COMBO, pos + posTarget);
                    infocombo.GetComponent<InfoCombo>().SetTextCombo(currentCombo.ToString());
                }
            }
            UpdateDisplayCombo();
        }

        public void UpdateDisplayCombo()
        {
            if (currentCombo > GameManager.s_Instance.comboMax)
            {
                GameManager.s_Instance.comboMax = currentCombo;
            }
            if (GameManager.s_Instance.comboMax > 1)
            {
                UIManager.s_Instance.SetComboMax(GameManager.s_Instance.comboMax.ToString());
            }
        }

        private float TotalRatio(float[] list)
        {
            float total = 0;
            foreach (int elem in list)
            {
                total += elem;
            }
            return total;
        }

        public float RandomWithRatio(float[] list, bool isInitRatio = false)
        {
            float total = TotalRatio(list);
            float randomPoint = Random.value * total;
            for (int i = 0; i < list.Length; i++)
            {
                if (randomPoint < list[i])
                {
                    return i;
                }
                else
                {
                    randomPoint -= list[i];
                }
            }
            return list.Length - 1;
        }

        private void InitTarget()
        {
            List<Block> list = ListTarget();

            // Init for current target
            int indexcurrent = Random.Range(0, list.Count - 1);
            currentBlockTarget = list[indexcurrent];
            list.RemoveAt(indexcurrent);

            // Init for next target
            int index = Random.Range(0, list.Count);
            nextBlockTarget = list[index];
            UIManager.s_Instance.SetImageForBlock();
        }

        public void ApplyMotionAllBlock(Vector2 direction)
        {
            for (int i = 0; i < blocksRef.Count - 1; i++)
            {
                Block block = blocksRef[0];
                block.ApplyMotion(direction, i);
                if (block.type == BlockType.Totoko)
                {
                    Totoko child = (Totoko)block;
                    child.UpdateTimeToPlay();
                    child.ShowEffect();
                }
            }
            InitTarget();
        }

		public void ToggleAllBlockStateFlick(){
			ChangeAllBlockStateFlick ();
			Invoke("ChangeAllBlockStateNormal", Config.TIME_SWITCH_STATE);
		}

		private void ChangeAllBlockStateFlick()
        {
            for (int i = 0; i < blocksRef.Count; i++)
            {
                blocksRef[i].ChangeStateFlick();
            }
        }

		public void ToggleAllBlockStateNormal(){
			ChangeAllBlockStateNormal ();
			Invoke("ChangeAllBlockStateFlick", Config.TIME_SWITCH_STATE);
		}

		private void ChangeAllBlockStateNormal()
        {
            for (int i = 0; i < blocksRef.Count; i++)
            {
                blocksRef[i].ChangeStateNormal();
            }
        }

        public void Init()
        {
            currentTime = Config.TIME_TO_COUNTDOWN + 1;
            InitBlocks();
            InitTarget();
        }

        private void InitBlocks(int count = Config.MAX_BLOCK)
        {
            for (int i = 0; i < count; i++)
            {
                AddNewBlock(i);
            }
        }

        private void ResetRatioIyamiItem()
        {
            float ratioIyami = GameManager.s_Instance.blocksRatio[(int)BlockType.Iyami];
            GameManager.s_Instance.countIyamiItem++;
            //for (int i = 0; i < GameManager.s_Instance.countIyamiItem; i++)
            //{
                ratioIyami *= (1.0f - GameManager.s_Instance.blocksRatioReduction[(int)BlockType.Iyami] / 100.0f);
            //}
            GameManager.s_Instance.blocksRatio[(int)BlockType.Iyami] = ratioIyami;
        }

        private void ResetRatioTotokkoItem()
        {
            float ratioTotoko = GameManager.s_Instance.blocksRatio[(int)BlockType.Totoko];
            GameManager.s_Instance.countTotokoItem++;
            //for (int i = 0; i < GameManager.s_Instance.countTotokoItem; i++)
            //{
                ratioTotoko *= (1.0f - GameManager.s_Instance.blocksRatioReduction[(int)BlockType.Totoko] / 100.0f);
            //}
            GameManager.s_Instance.blocksRatio[(int)BlockType.Totoko] = ratioTotoko;
        }
		//-->| by anhgh
		private float m_lastpercent_Iyami = 0f;
		private float m_lastpercent_Totoko = 0f;
		public void GetSpecificBlockPercent(){
			m_lastpercent_Totoko = GameManager.s_Instance.blocksRatio[(int)BlockType.Totoko];
			m_lastpercent_Iyami = GameManager.s_Instance.blocksRatio [(int)BlockType.Iyami];
		}
		//--<| by anhgh
        public void AddNewBlock(int factor = 0)
        {
            if (factor == 0 && ComponentConstant.SOUND_MANAGER != null)
            {
                ComponentConstant.SOUND_MANAGER.Play(SoundEnum.SE16_mutsugo_fall);
            }

            float posx = Random.Range(-Config.MAX_GEN_X, Config.MAX_GEN_X);
            Vector2 pos = new Vector2(posx, Config.POS_GEN_Y + Config.BLOCK_HEIGHT * factor);

            List<Block> list = ListTarget();

			GameManager.s_Instance.blocksRatio[(int)BlockType.Iyami] = m_lastpercent_Iyami;
			GameManager.s_Instance.blocksRatio[(int)BlockType.Totoko] = m_lastpercent_Totoko;
			//-->| by anhgh
			for (int i = 0; i < blocksRef.Count; i++) { //why blocksRef.Count max is 6 ??
				if (blocksRef [i].type == BlockType.Iyami) {
					GameManager.s_Instance.blocksRatio [(int)BlockType.Iyami] = 0f;
				} 
				if (blocksRef [i].type == BlockType.Totoko) {
					GameManager.s_Instance.blocksRatio [(int)BlockType.Totoko] = 0f;
				} 
			}
			//--<| by anhgh
            BlockType type = (BlockType)RandomWithRatio(GameManager.s_Instance.blocksRatio);

            if (list.Count <= 3 && list.Count > 0)
            {
                type = listBlockType[Random.Range(0, listBlockType.Length)];
            }

            // reset ratio for totoko and Iyami
            if (type == BlockType.Totoko)
            {
                ResetRatioTotokkoItem();
				m_lastpercent_Totoko = GameManager.s_Instance.blocksRatio[(int)BlockType.Totoko];

            }
            if (type == BlockType.Iyami)
            {
                ResetRatioIyamiItem();
				m_lastpercent_Iyami = GameManager.s_Instance.blocksRatio [(int)BlockType.Iyami];
            }

            GameObject block = PoolManager.s_Instance.GetFreeObject(type.ToString(), pos);
            blocksRef.Add(block.GetComponent<Block>());
			block.GetComponent<Block> ().ChangeStateNormal ();
            // Update sorting layer
            for (int i = 0; i < blocksRef.Count; i++)
            {
                Vector3 newPos = blocksRef[i].transform.position;
                newPos.z = -newPos.y + 50;
                blocksRef[i].transform.position = newPos;
            }
        }

        private List<Block> ListTarget()
        {
            List<Block> list = new List<Block>();
            for (int i = 0; i < blocksRef.Count; i++)
            {
                Block block = blocksRef[i];
                if ((block.type != BlockType.Iyami) && (block.type != BlockType.Totoko))
                {
                    list.Add(block);
                }
            }
            return list;
        }

        public void RestoreVelocityBlock()
        {
            for (int i = 0; i < blocksRef.Count; i++)
            {
                Block block = blocksRef[i];
                block.myRigidbody2D.velocity = new Vector2(0, -GameManager.s_Instance.gameParameter.down_speed);
            }
        }

        private void RemoveBlock(ref List<Block> list, BlockType type)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].type == type)
                {
                    list.RemoveAt(i);
                    break;
                }
            }
        }

        public void GenNextTarget()
        {
            List<Block> list = ListTarget();
            // Remove current and next target

            RemoveBlock(ref list, currentBlockTarget.type);
            RemoveBlock(ref list, nextBlockTarget.type);

            //list.Remove(currentBlockTarget);
            //list.Remove(nextBlockTarget);

            currentBlockTarget = nextBlockTarget;

            int index = Random.Range(0, list.Count);
            nextBlockTarget = list[index];
            UIManager.s_Instance.SetImageForBlock();

            // Arrow direction
            currentArrow = nextArrow;

            // Random
            nextArrow = (Direction)RandomWithRatio(GameManager.s_Instance.arrowRatio);
            UIManager.s_Instance.SetCurrentArrowDirection(currentArrow);
            UIManager.s_Instance.SetNextArrowDirection(nextArrow);
        }
    }
}

