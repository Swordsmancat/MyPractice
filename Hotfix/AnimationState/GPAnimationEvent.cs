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
    public class GPAnimationEvent : StateMachineBehaviour
    {

        private TargetableObject owner;

        private PlayerLogic m_Player;

        private EnemyLogic m_Enemy;

        [SerializeField]
        private GPAttack m_GPAttack;

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateEnter(animator, stateInfo, layerIndex);
            owner = animator.gameObject.GetComponent<TargetableObject>();
            m_Player = owner as PlayerLogic;
            if (m_Player != null)
            {
                m_Player.GPStart(m_GPAttack);
            }
            else
            {
                m_Enemy = owner as EnemyLogic;
                if(m_Enemy != null)
                {
                    Log.Warning("怪物暂无GP");
                }
               
            }
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateExit(animator, stateInfo, layerIndex);
            if (m_Player != null)
            {
                m_Player.GPEnd();
            }
            else
            {
                m_Enemy = owner as EnemyLogic;
                if (m_Enemy != null)
                {

                    Log.Warning("怪物暂无GP");
                }
            }


        }
    }
}
