//------------------------------------------------------------
// Creator: FatCat
// Creation date: 2023/2/14/周二 13:40:36
//------------------------------------------------------------

using GameFramework.Event;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace Farm.Hotfix
{
    public class ChangeWeaponAnimationEvent : StateMachineBehaviour
    {
        private TargetableObject owner;

        private PlayerLogic m_Player;

        private EnemyLogic m_Enemy;
        
        [SerializeField,LabelText("是否切换为副武器武器")]
        private bool m_IsChangeWeaponStart;

        [SerializeField, LabelText("切换副武器开始时间"),ShowIf("m_IsChangeWeaponStart")]
        private float m_ChangeWeaponStartTime;

        [SerializeField, LabelText("切换副武器左手"), ShowIf("m_IsChangeWeaponStart")]
        private bool m_IsChangeWeaponSubLeft;

        [SerializeField, LabelText("切换副武器右手"), ShowIf("m_IsChangeWeaponStart")]
        private bool m_IsChangeWeaponSubRight;

        [SerializeField,HideLabel,LabelText("是否切换为主武器武器")]
        private bool m_IsChangeWeaponEnd;

        [SerializeField, LabelText("切换主武器时间"),ShowIf("m_IsChangeWeaponEnd")]
        private float m_ChangeWeaponEndTime;

        [SerializeField, LabelText("切换主武器左手"), ShowIf("m_IsChangeWeaponEnd")]
        private bool m_ChangeWeaponMainLeft;

        [SerializeField, LabelText("切换主武器右手"), ShowIf("m_IsChangeWeaponEnd")]
        private bool m_ChangeWeaponMainRight;

        private int? m_ChangeWeaponStartTimeID = null;
        private int? m_ChangeWeaponEndTimeID = null;

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
                Log.Warning("脚本错误运用");
            }
            else
            {
                m_Enemy = owner as EnemyLogic;
                if (m_IsChangeWeaponStart)
                {
                    m_ChangeWeaponStartTimeID = GameEntry.Timer.AddOnceTimer((long)(m_ChangeWeaponStartTime * 1000), () => m_Enemy.ChangeSubWeapon(m_IsChangeWeaponSubLeft, m_IsChangeWeaponSubRight));
                }
                if (m_IsChangeWeaponEnd)
                {
                    m_ChangeWeaponEndTimeID = GameEntry.Timer.AddOnceTimer((long)(m_ChangeWeaponEndTime * 1000), () => m_Enemy.ChangeMainWeapon(m_ChangeWeaponMainLeft, m_ChangeWeaponMainRight));
                }

            }

        }

        private void ApplyDamageEvent(object sender, GameEventArgs e)
        {
            ApplyDamageEventArgs ne = (ApplyDamageEventArgs)e;
            if (ne.UserData != owner)
            {
                return;
            }
            if (m_ChangeWeaponStartTimeID != null)
            {
                if (GameEntry.Timer.IsExistTimer((int)m_ChangeWeaponStartTimeID))
                {
                    GameEntry.Timer.CancelTimer((int)m_ChangeWeaponStartTimeID);
                }

            }
            if (m_ChangeWeaponEndTimeID != null)
            {
                if (GameEntry.Timer.IsExistTimer((int)m_ChangeWeaponEndTimeID))
                {
                    GameEntry.Timer.CancelTimer((int)m_ChangeWeaponEndTimeID);
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
        }

    }
}
