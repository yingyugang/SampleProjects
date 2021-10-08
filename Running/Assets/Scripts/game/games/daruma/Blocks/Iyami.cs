using UnityEngine;
using System.Collections;
using System;
namespace Daruma
{
    public class Iyami : Block
    {
        protected override void ActionFlick()
        {
            if (ComponentConstant.SOUND_MANAGER != null)
            {
                ComponentConstant.SOUND_MANAGER.Play(SoundEnum.SE19_mutsugo_smashall);
            }
            BlockManager.s_Instance.currentCombo += 5;
            BlockManager.s_Instance.checkingCombo = true;
            BlockManager.s_Instance.CheckShowCombo(m_DirectionFlick, transform.position);
            BlockManager.s_Instance.ApplyMotionAllBlock(m_DirectionFlick);
        }
    }
}

