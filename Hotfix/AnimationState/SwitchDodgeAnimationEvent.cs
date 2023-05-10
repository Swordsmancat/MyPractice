//------------------------------------------------------------
// Creator: FatCat
// Creation date: 2023/2/14/周二 13:40:36
//------------------------------------------------------------

using GameFramework.Event;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Farm.Hotfix
{
    public class SwitchDodgeAnimationEvent : StateMachineBehaviour
    {
        private TargetableObject owner;

        private PlayerLogic m_Player;

        private EnemyLogic m_Enemy;
        
        [SerializeField,LabelText("滑步开始时间")]
        private float m_StartDodgeStepTime;

        [SerializeField,HideLabel,LabelText("是否在动画结束时退出滑步")]
        private bool m_IsExitStep;

        private int? m_DodgeStepTimer = null;
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
                m_DodgeStepTimer  = GameEntry.Timer.AddOnceTimer((long)(m_StartDodgeStepTime* 1000), () =>  m_Player.SetDodgeStepOn());
            }

        }

        private void ApplyDamageEvent(object sender, GameEventArgs e)
        {
            ApplyDamageEventArgs ne = (ApplyDamageEventArgs)e;
            if (ne.UserData != owner)
            {
                return;
            }
            if (m_DodgeStepTimer != null)
            {
                if (GameEntry.Timer.IsExistTimer((int)m_DodgeStepTimer))
                {
                    GameEntry.Timer.CancelTimer((int)m_DodgeStepTimer);
                }

            }
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateExit(animator, stateInfo, layerIndex);
            if (GameEntry.Event.Check(ApplyDamageEventArgs.EventId, ApplyDamageEvent))
            {
                GameEntry.Event.Unsubscribe(ApplyDamageEventArgs.EventId, ApplyDamageEvent);
            }
            if (m_Player != null)
            {
                if (m_IsExitStep)
                {
                    m_Player.SetDodgeStepOff();
                }
            }
        }

    }
}
