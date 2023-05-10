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
    public class DodgeAnimationEvent : StateMachineBehaviour
    {

        private TargetableObject owner;

        private PlayerLogic m_Player;

        private EnemyLogic m_Enemy;

        [SerializeField, LabelText("翻滚无敌开始时间")]
        private float m_DodgeStartTime;

        [SerializeField, LabelText("翻滚无敌结束时间")]
        private float m_DodgeEndTime;


        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateEnter(animator, stateInfo, layerIndex);
            owner = animator.gameObject.GetComponent<TargetableObject>();
            m_Player = owner as PlayerLogic;
            if (m_Player != null)
            {
                GameEntry.Timer.AddOnceTimer((long)(m_DodgeStartTime * 1000), () => m_Player.DodgeInvincibleStart());
                GameEntry.Timer.AddOnceTimer((long)(m_DodgeEndTime * 1000), () => m_Player.DodgeInvincibleEnd());
            }
            else
            {
                m_Enemy = owner as EnemyLogic;
                if(m_Enemy != null)
                {
                    GameEntry.Timer.AddOnceTimer((long)(m_DodgeStartTime * 1000), () => m_Enemy.DodgeInvincibleStart());
                    GameEntry.Timer.AddOnceTimer((long)(m_DodgeStartTime * 1000), () => m_Enemy.DodgeInvincibleEnd());

                }
               
            }
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateExit(animator, stateInfo, layerIndex);
        }
    }
}
