using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using DG.Tweening;
namespace Daruma
{
    public class UIManager : MonoBehaviour
    {
        public Image imgCurrentBlock;
        public Image imgNextBlock;
        public Text  txtComboMax;
		public Image imgBack;
		public Sprite normalBack;
		public Sprite cheatBack;

        public GameObject panelSlideShow;

        public GameObject currentArrowLeft;
        public GameObject currentArrowRight;

        public GameObject nextArrowLeft;
        public GameObject nextArrowRight;

        public static UIManager s_Instance;

        private Animator m_AnimatorCurrentBlock;
        private Animator m_AnimatorNextBlock;
        
        private void Awake()
        {
            s_Instance = this;
        }

        private void Start()
        {
            panelSlideShow.SetActive(false);
            nextArrowLeft.SetActive(false);
            nextArrowRight.SetActive(false);
            currentArrowLeft.SetActive(false);
            currentArrowRight.SetActive(false);
            m_AnimatorCurrentBlock = imgCurrentBlock.gameObject.GetComponent<Animator>();
            m_AnimatorNextBlock = imgNextBlock.gameObject.GetComponent<Animator>();
			if(imgBack!=null){
				if (!CheatController.IsCheated(0))
					imgBack.sprite = normalBack;
				else
					imgBack.sprite = cheatBack;
			}
        }

        private void SetSpeedAnimationTarget(float speed)
        {
            if (m_AnimatorCurrentBlock != null)
            {
                m_AnimatorCurrentBlock.speed = speed;

            }
            if (m_AnimatorNextBlock != null)
            {
                m_AnimatorNextBlock.speed = speed;
            }
        }

        public void SetTextComboMax(string str)
        {
            txtComboMax.text = str;
        }

        // Display target's image on screen
        public void SetImageForBlock()
        {
            SetSpeedAnimationTarget(GameManager.s_Instance.gameParameter.target_animation_speed);
            if (m_AnimatorCurrentBlock != null)
            {
                m_AnimatorCurrentBlock.Play("ZoomIn", -1, 0);

            }
            if (m_AnimatorNextBlock != null)
            {
                m_AnimatorNextBlock.Play("ZoomOut", -1, 0);
            }
			if (!CheatController.IsCheated(0)) {
				UIManager.s_Instance.imgCurrentBlock.sprite = BlockManager.s_Instance.currentBlockTarget.targetSprite;
				UIManager.s_Instance.imgNextBlock.sprite = BlockManager.s_Instance.nextBlockTarget.targetSprite;
			} else {
				UIManager.s_Instance.imgCurrentBlock.sprite = BlockManager.s_Instance.currentBlockTarget.flickSprite;
				UIManager.s_Instance.imgNextBlock.sprite = BlockManager.s_Instance.nextBlockTarget.flickSprite;
			}
        }
        
        public void SetComboMax(string str)
        {
            txtComboMax.transform.parent.gameObject.SetActive(true);
            txtComboMax.text = str;
        }

        public void SetCurrentArrowDirection(Direction direction)
        {
            // Reset all
            currentArrowLeft.SetActive(false);
            currentArrowRight.SetActive(false);

            switch (direction)
            {
                case Direction.None:
                    break;
                case Direction.Left:
                    currentArrowLeft.SetActive(true);
                    break;
                case Direction.Right:
                    currentArrowRight.SetActive(true);
                    break;
                default:
                    break;
            }
        }

        public void SetNextArrowDirection(Direction direction)
        {
            // Reset all
            nextArrowLeft.SetActive(false);
            nextArrowRight.SetActive(false);

            switch (direction)
            {
                case Direction.None:
                    break;
                case Direction.Left:
                    nextArrowLeft.SetActive(true);
                    break;
                case Direction.Right:
                    nextArrowRight.SetActive(true);
                    break;
                default:
                    break;
            }
        }
    }
}

