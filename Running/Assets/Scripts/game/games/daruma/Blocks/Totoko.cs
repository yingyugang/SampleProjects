using UnityEngine;
using System.Collections;
using System;
using DG.Tweening;

namespace Daruma
{
    public class Totoko : Block
    {
        public void UpdateTimeToPlay()
        {
            float time = Header.Instance.GetLifeTime();
            time += GameManager.s_Instance.gameParameter.totoko_add_second;
            GameManager.s_Instance.playTime += (int)GameManager.s_Instance.gameParameter.totoko_add_second;
            Header.Instance.SetLifeTime(time);
        }

        public void ShowEffect()
        {
            Vector3 pos = new Vector3(m_DirectionFlick.x > 0 ? -3.5f : 3.5f, 0, 0);
            GameObject heartTimeRecovery = PoolManager.s_Instance.GetFreeObject("HeartTimeRecovery", pos);
            heartTimeRecovery.GetComponent<Animator>().speed = GameManager.s_Instance.gameParameter.totoko_animation_speed;
            // Move efffect
            Vector3 posWorld = Header.Instance.txtLife.GetComponent<RectTransform>().transform.position;
            heartTimeRecovery.transform.DOMove(posWorld, GameManager.s_Instance.gameParameter.totoko_animation_speed);
        }

        protected override void ActionFlick()
        {
            if (ComponentConstant.SOUND_MANAGER != null)
            {
                ComponentConstant.SOUND_MANAGER.Play(SoundEnum.SE20_mutsugo_recovery);
            }
            ApplyMotion(m_DirectionFlick);
            BlockManager.s_Instance.checkingCombo = true;
            BlockManager.s_Instance.currentComboSecond = 0;
            BlockManager.s_Instance.CheckShowCombo(m_DirectionFlick, transform.position);
            UpdateTimeToPlay();
            ShowEffect();
        }
    }
}

