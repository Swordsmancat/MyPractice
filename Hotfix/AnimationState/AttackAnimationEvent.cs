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
    public class AttackAnimationEvent :StateMachineBehaviour
    {

        private TargetableObject owner;

        private PlayerLogic m_Player;

        private EnemyLogic m_Enemy;

        [SerializeField,LabelText("是否为攻击结束动画")]
        private bool m_IsEndAttackAnimation =true;

        [SerializeField, LabelText("是否存在攻击旋转")]
        private bool m_IsAttackRotate;


        [LabelText("攻击旋转开始"),SerializeField,ShowIf("m_IsAttackRotate")]
        private float m_AttackRotateStart;

        [LabelText("攻击旋转结束"), SerializeField, ShowIf("m_IsAttackRotate")]
        private float m_AttackRotateEnd;


        [LabelText("不格挡(只作用于敌人)"), HideLabel, SerializeField]
        private bool m_DontDefense;


        private int? m_AttackRotateStartID = null;
        private int? m_AttackRotateEndID = null;

        private bool m_IsContinuousRotate = false;

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateEnter(animator, stateInfo, layerIndex);
            owner = animator.gameObject.GetComponent<TargetableObject>();
            m_Player = owner as PlayerLogic;
            if (m_Player != null)
            {
                Log.Info("暂无玩家攻击信息");
            }
            else
            {
                m_Enemy = owner as EnemyLogic;
                if (m_Enemy == null)
                {
                    Log.Warning("not found owner");
                    return;
                }

                if (m_DontDefense)
                {
                    m_Enemy.SetDontDefenseStart();
                }
                else
                {
                    m_Enemy.SetDontDefenseEnd();
                }
                if (m_IsAttackRotate)
                {
                    if(m_AttackRotateStart > m_AttackRotateEnd)
                    {
                        GameEntry.Timer.AddOnceTimer((long)(m_AttackRotateStart * 1000), () => m_Enemy.AttackRotateStart());
                    }
                    else
                    {
                        GameEntry.Timer.AddOnceTimer((long)(m_AttackRotateStart * 1000), () => m_Enemy.AttackRotateStart());
                        GameEntry.Timer.AddOnceTimer((long)(m_AttackRotateEnd * 1000), () => m_Enemy.AttackRotateEnd());
                    }
                }
               
            }
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateExit(animator, stateInfo, layerIndex);
            if (m_IsEndAttackAnimation)
            {
                if (m_Player != null)
                {
                    Log.Info("暂无玩家攻击信息");
                }
                else
                {
                    m_Enemy = owner as EnemyLogic;
                    if (m_Enemy == null)
                    {
                        Log.Warning("not found owner");
                        return;
                    }
                    GameEntry.Timer.AddOnceTimer(0, () => m_Enemy.AnimationEnd());
                }
            }

            if (m_AttackRotateStartID != null)
            {
                if (GameEntry.Timer.IsExistTimer((int)m_AttackRotateStartID))
                {
                    GameEntry.Timer.CancelTimer((int)m_AttackRotateStartID);
                }
            }

            if (m_AttackRotateEndID != null)
            {
                if (GameEntry.Timer.IsExistTimer((int)m_AttackRotateEndID))
                {
                    GameEntry.Timer.CancelTimer((int)m_AttackRotateEndID);
                }
            }
            if (m_IsEndAttackAnimation)
            {
                 m_Enemy.AttackRotateEnd();
                m_Enemy.SetDontDefenseEnd();
            }
        }
    }
}
