//------------------------------------------------------------
// Creator: FatCat
// Creation date: 2023/3/15/周三 14:55:40
//------------------------------------------------------------


using Sirenix.OdinInspector;
using UnityEngine;

namespace Farm.Hotfix
{
    public class ArrowSpecialAnimationEvent :StateMachineBehaviour
    {
        private TargetableObject owner;

        private PlayerLogic m_Player;
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateEnter(animator, stateInfo, layerIndex);
            owner = animator.gameObject.GetComponent<TargetableObject>();
            m_Player = owner as PlayerLogic;
            if (m_Player != null)
            {
               m_Player.SetPlayerInArrowSpecialOn();
            }

        }
        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateExit(animator, stateInfo, layerIndex);
            if (m_Player != null)
            {
              
                    m_Player.SetPlayerInArrowSpecialOff();
                
            }
        }
    }
}
