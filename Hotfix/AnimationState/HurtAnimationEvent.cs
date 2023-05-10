//------------------------------------------------------------
// Creator: FatCat
// Creation date: 2022/10/19/周三 18:03:42
//------------------------------------------------------------


using GameFramework.Event;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace Farm.Hotfix
{
    public class HurtAnimationEvent : StateMachineBehaviour
    {

        private TargetableObject owner;

        private PlayerLogic m_Player;

        private EnemyLogic m_Enemy;

        private bool m_PersistentHurt = false;


        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateEnter(animator, stateInfo, layerIndex);
            owner = animator.gameObject.GetComponent<TargetableObject>();
            m_Player = owner as PlayerLogic;

            if (!GameEntry.Event.Check(ApplyDamageEventArgs.EventId, ApplyDamageEvent))
            {
                GameEntry.Event.Subscribe(ApplyDamageEventArgs.EventId, ApplyDamageEvent);
            }
            if (m_Player != null)
            {
                m_Player.PlayerHurtStart();
            }
        }
        private void ApplyDamageEvent(object sender, GameEventArgs e)
        {
            ApplyDamageEventArgs ne = (ApplyDamageEventArgs)e;
            if (ne.UserData != owner)
            {
                return;
            }
            m_PersistentHurt = true;
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateExit(animator, stateInfo, layerIndex);
            if (GameEntry.Event.Check(ApplyDamageEventArgs.EventId, ApplyDamageEvent))
            {
                GameEntry.Event.Unsubscribe(ApplyDamageEventArgs.EventId, ApplyDamageEvent);
            }
            if (!m_PersistentHurt)
            {
                if (m_Player != null)
                {
                    m_Player.PlayerHurtEnd();
                }
                else
                {
                    m_Enemy = owner as EnemyLogic;
                    if (m_Enemy != null)
                    {
                        m_Enemy.EnemyHurtEnd();

                    }
                }
            }
            m_PersistentHurt = false;




        }
    }
}
