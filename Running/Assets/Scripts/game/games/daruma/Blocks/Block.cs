using UnityEngine;
using UnityEngine.UI;
namespace Daruma
{
    public enum BlockType
    {
        Osomatu,
        Karamatu,
        Chocomatu,
        Ichimatu,
        Jyushimatu,
        Todomatu,
        Iyami,
        Totoko
    }

    public enum Direction
    {
        None = 0,
        Left = 1,
        Right = 2
    }

    public class Block : MonoBehaviour
    {
        protected Vector2 m_PointStart; // Start point when touching down.
        protected Vector2 m_PointEnd;   // End point when touching up.
        protected Vector2 m_DirectionFlick = Vector2.zero;
        protected bool m_IsFlicked = false;

        protected float m_DropSpeed = 10.0f;

        public BlockType type;

        public Rigidbody2D myRigidbody2D;
        public Sprite flickSprite;
        protected Sprite normalSprite;

        public Sprite targetSprite;

        protected SpriteRenderer m_MainSprite;
        protected BoxCollider2D m_BoxCollider2D;

        protected void Start()
        {
            Init();
        }

        protected void Init()
        {
            m_MainSprite = GetComponent<SpriteRenderer>();
            //normalSprite = m_MainSprite.sprite;
            m_BoxCollider2D = GetComponentInChildren<BoxCollider2D>();
            if (myRigidbody2D != null)
            {
                myRigidbody2D.velocity = new Vector2(0, -GameManager.s_Instance.gameParameter.down_speed);
            }
        }

        protected void OnEnable()
        {
            myRigidbody2D.gravityScale = 10;
        }

        public void ChangeStateFlick()
        {
			if (m_MainSprite == null) {
				m_MainSprite = GetComponent<SpriteRenderer>();
				normalSprite = m_MainSprite.sprite;
			}
			if (!CheatController.IsCheated(1)) {
				m_MainSprite.sprite = flickSprite;
			} else {
				m_MainSprite.sprite = normalSprite;
			}
        }

        public void ChangeStateNormal()
        {
			if (m_MainSprite == null) {
				m_MainSprite = GetComponent<SpriteRenderer>();
				normalSprite = m_MainSprite.sprite;
			}
				
			if (!CheatController.IsCheated(1)) {
				m_MainSprite.sprite = normalSprite;
			} else {
				m_MainSprite.sprite = flickSprite;
			}
        }

        public void ApplyMotion(Vector2 direction, int factor = 0)
        {
            myRigidbody2D.velocity = new Vector2(0, 0);
            myRigidbody2D.gravityScale = 0;
            m_BoxCollider2D.isTrigger = true;
            GameManager.s_Instance.score++;
            GameManager.s_Instance.blockNum++;
            ChangeStateFlick();
            myRigidbody2D.AddForce(direction * GameManager.s_Instance.gameParameter.flick_speed * 40);
            BlockManager.s_Instance.blocksRef.Remove(this);
            BlockManager.s_Instance.AddNewBlock(factor);
            BlockManager.s_Instance.RestoreVelocityBlock();

            // Update displaying score/combo
            Header.Instance.SetScore(GameManager.s_Instance.score.ToString() + "コ");
        }

        public void OnMouseDrag()
        {
            if (Header.Instance.GetLifeTime() <= 0)
            {
                return;
            }

            if (!GameCountDownMediator.didEndCountDown)
            {
                return;
            }

            if (PauseManager.s_Instance.childPanel.activeSelf)
            {
                return;
            }

            m_PointEnd = Input.mousePosition;
            float distance = Vector2.Distance(m_PointStart, m_PointEnd);
            int count = BlockManager.s_Instance.blocksRef.Count;
            Vector2 direction = m_PointEnd - m_PointStart;
            m_DirectionFlick = new Vector2(direction.x >= 0 ? 1 : -1, 0);
            // Skip top block.
            if (Equals(BlockManager.s_Instance.blocksRef[count - 1]))
            {
                return;
            }

            if (distance >= GameManager.s_Instance.gameParameter.flick_sensitivity && !m_IsFlicked)
            {
                m_IsFlicked = true;
                ActionFlick();
            }
        }

        protected void ActionFlickTrue()
        {
            if (ComponentConstant.SOUND_MANAGER != null)
            {
                ComponentConstant.SOUND_MANAGER.Play(SoundEnum.SE17_mutsugo_smash);
            }
            Direction direction = (m_DirectionFlick.x > 0) ? Direction.Right : Direction.Left;
            if (BlockManager.s_Instance.currentArrow == direction)
            {
                if (ComponentConstant.SOUND_MANAGER != null)
                {
                    ComponentConstant.SOUND_MANAGER.Play(SoundEnum.SE18_mutsugo_right);
                }
                GameManager.s_Instance.score++;
            }
            ApplyMotion(m_DirectionFlick);
            BlockManager.s_Instance.checkingCombo = true;
            BlockManager.s_Instance.CheckShowCombo(m_DirectionFlick, transform.position);
            BlockManager.s_Instance.GenNextTarget();
        }

        protected void ActionFlickFalse()
        {
            // Reset Combo
            GameManager.s_Instance.noMiss = 0;
            BlockManager.s_Instance.ResetCombo();
            // Change state for all blocks.
				BlockManager.s_Instance.ToggleAllBlockStateFlick ();
            if (ComponentConstant.SOUND_MANAGER != null)
            {
                ComponentConstant.SOUND_MANAGER.Play(SoundEnum.se13_miss);
            }
        }

        protected virtual void ActionFlick()
        {
            // Flick true
            if (BlockManager.s_Instance.currentBlockTarget.type == type)
            {
                ActionFlickTrue();
            }
            else // Flick false
            {
                ActionFlickFalse();
            }
        }

        public void OnMouseDown()
        {
            m_IsFlicked = false;
            m_PointStart = Input.mousePosition;
        }

        public virtual void Destroy()
        {
            m_BoxCollider2D.isTrigger = false;
            ChangeStateNormal();
            gameObject.SetActive(false);
        }
    }
}

