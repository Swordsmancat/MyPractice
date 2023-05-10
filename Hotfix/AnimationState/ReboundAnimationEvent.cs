//------------------------------------------------------------
// Creator: FatCat
// Creation date: 2022/10/19/周三 18:03:42
//------------------------------------------------------------


using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace Farm.Hotfix
{
    public class ReboundAnimationEvent : StateMachineBehaviour
    {

        private TargetableObject owner;

        private PlayerLogic m_Player;

        private EnemyLogic m_Enemy;

        [SerializeField, LabelText("是否为重击")]
        private bool m_IsReboundHeavy;

        [SerializeField, LabelText("弹反类型")]
        private ReboundState m_ReboundState; 

        [SerializeField,LabelText("士气值")]
        private int m_MoraleValue;

        [SerializeField, LabelText("弹反开始")]
        private float m_ReboundStartTime;

        [SerializeField, LabelText("弹反结束")]
        private float m_ReboundEndTime;


        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateEnter(animator, stateInfo, layerIndex);
            owner = animator.gameObject.GetComponent<TargetableObject>();
            m_Player = owner as PlayerLogic;
            if (m_Player != null)
            {
                GameEntry.Timer.AddOnceTimer((long)(m_ReboundStartTime * 1000), () => m_Player.ReboundStart(m_ReboundState));
                GameEntry.Timer.AddOnceTimer((long)(m_ReboundEndTime * 1000), () => m_Player.ReboundEnd());
                m_Player.SetMoraleValue(m_MoraleValue);
            }
            else
            {
                m_Enemy = owner as EnemyLogic;
                if(m_Enemy != null)
                {
                    if (m_IsReboundHeavy)
                    {
                        m_Enemy.IsReboundHeavy = true;
                    }
                    GameEntry.Timer.AddOnceTimer((long)(m_ReboundStartTime * 1000), () => m_Enemy.ReboundStart(m_ReboundState));
                    GameEntry.Timer.AddOnceTimer((long)(m_ReboundEndTime * 1000), () => m_Enemy.ReboundEnd());
                    m_Enemy.SetMoraleValue(m_MoraleValue);

                }
               
            }
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateExit(animator, stateInfo, layerIndex);
            //if (m_Player != null)
            //{
            //    m_Player.ReboundEnd();
            //}
            //else
            //{
            //    m_Enemy = owner as EnemyLogic;
            //    if (m_Enemy != null)
            //    {
            //        if (m_IsReboundHeavy)
            //        {
            //            m_Enemy.IsReboundHeavy = false;
            //        }
            //        m_Enemy.ReboundEnd();

            //    }
            //}


        }
    }
}
