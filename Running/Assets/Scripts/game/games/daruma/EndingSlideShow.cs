using UnityEngine;
using System.Collections;
namespace Daruma
{
    public class EndingSlideShow : MonoBehaviour
    {
        private bool m_ClickOK = false;

        public void PointerDown()
        {
            if (m_ClickOK)
            {
                ActionEndOfFrame();
            }
            // skip slide show
        }

        public void AcceptClick()
        {
            m_ClickOK = true;
        }

        public void ActionEndOfFrame()
        {
            // Params will be send to server when ending game.
            GameManager.s_Instance.SendGameEndingAPI();
            PoolManager.s_Instance.DesSpawn(Config.INFO_COMBO);
            gameObject.SetActive(false);
        }
    }
}

