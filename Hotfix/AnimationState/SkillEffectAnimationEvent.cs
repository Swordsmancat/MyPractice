//------------------------------------------------------------
// Creator: FatCat
// Creation date: 2022/10/19/周三 18:03:42
//------------------------------------------------------------


using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;
using System;
using GameFramework.Event;

namespace Farm.Hotfix
{
    public class SkillEffectAnimationEvent : StateMachineBehaviour
    {

        private TargetableObject owner;

        private PlayerLogic m_Player;

        private EnemyLogic m_Enemy;

        #region 展示字段

        [TableList, LabelText("攻击伤害效果"), SerializeField]
        private List<AttackHurtEffect> attackHurtEffects;

        [LabelText("造成躯干值"), HideLabel, SerializeField]
        private int m_TakeTrunkValue;

        [TableList, LabelText("伤害特效列表"), SerializeField]
        private List<SkillEffectTime> EffectTimelist;

        [InfoBox("需要添加名为ArrowModel的箭矢节点")]
        [SerializeField, LabelText("是否隐藏箭矢模型")]
        private bool m_IsHideArrow;

        [SerializeField, LabelText("隐藏箭矢模型开始时间"),ShowIf("m_IsHideArrow")]
        private float m_HideArrowTime;

        [SerializeField, LabelText("是否显示箭矢模型")]
        private bool m_IsShowArrow;

        [SerializeField, LabelText("显示箭矢模型开始时间"), ShowIf("m_IsShowArrow")]
        private float m_ShowArrowTime;

        #endregion

        private int m_LoopEffectID;
        private bool m_IsLoopEffect = false;
        private int? m_HideHandArrowID = null;
        private int? m_ShowHandArrowID = null;

        /// <summary>
        /// 保存特效时间计时器 id
        /// </summary>
        private List<int?> m_EffectTimeList = null;

        /// <summary>
        /// 保存攻击伤害效果计时器 id
        /// </summary>
        private List<int?> m_AttackHurtEffectTimeList = null;

        #region 私有方法

        /// <summary>
        /// 攻击伤害效果
        /// </summary>
        private void AttackHurtEffectFunction()
        {
            m_Player = owner as PlayerLogic;

            if (m_Player != null)
            {
                for (int i = 0; i < attackHurtEffects.Count; i++)
                {
                    AttackHurtEffect attackHurtEffect = attackHurtEffects[i];

                    // 得到攻击伤害效果开始的计时器 id
                    int? m_attackHurtStartTimer = GameEntry.Timer.AddOnceTimer((long)EffectTimelist[i].m_HitStartTime * 1000, () =>
                    {
                        m_Player.AttackStart(attackHurtEffect.m_StutterFrame, attackHurtEffect.MoraleValue, attackHurtEffect.m_IgnoreParry, attackHurtEffect.m_IgnoreRebound, attackHurtEffect.m_IsProduceSF);
                    });

                    // 得到攻击伤害效果结束的计时器 id
                    int? m_attackHurtEndTimer = GameEntry.Timer.AddOnceTimer((long)EffectTimelist[i].m_HitEndTime * 1000, () =>
                    {
                        m_Player.AttackEnd();
                    });

                    // 添加计时器 id 到 列表 统一管理
                    m_AttackHurtEffectTimeList.Add(m_attackHurtStartTimer);
                    m_AttackHurtEffectTimeList.Add(m_attackHurtEndTimer);
                }

                // 造成躯干值
                m_Player.SetTakeTrunkValue(m_TakeTrunkValue);
            }
            else
            {
                m_Enemy = owner as EnemyLogic;

                if (m_Enemy == null)
                {
                    Log.Warning("not found owner");
                    return;
                }

                for (int i = 0; i < attackHurtEffects.Count; i++)
                {
                    AttackHurtEffect attackHurtEffect = attackHurtEffects[i];

                    // 得到攻击伤害效果开始的计时器 id
                    int? m_attackHurtStartTimer = GameEntry.Timer.AddOnceTimer((long)EffectTimelist[i].m_HitStartTime * 1000, () =>
                    {
                        m_Enemy.EnemyAttackStart(attackHurtEffect.m_IgnoreParry, attackHurtEffect.m_IgnoreRebound, attackHurtEffect.MoraleValue);
                    });

                    // 得到攻击伤害效果结束的计时器 id
                    int? m_attackHurtEndTimer = GameEntry.Timer.AddOnceTimer((long)EffectTimelist[i].m_HitEndTime * 1000, () =>
                    {
                        m_Enemy.EnemyAttackEnd();
                    });

                    // 添加计时器 id 到 列表 同意管理
                    m_AttackHurtEffectTimeList.Add(m_attackHurtStartTimer);
                    m_AttackHurtEffectTimeList.Add(m_attackHurtEndTimer);
                }

                // 造成躯干值
                m_Enemy.SetTakeTrunkValue(m_TakeTrunkValue);
            }

            return;
        }

        private void ArrowShowOrHide()
        {
            if (m_Player != null)
            {
                m_Player = owner as PlayerLogic;
                if (m_IsHideArrow)
                {
                    m_HideHandArrowID = GameEntry.Timer.AddOnceTimer((long)(m_HideArrowTime * 1000), () => m_Player.HideHandArrow());
                }
                if (m_IsShowArrow)
                {
                    m_ShowHandArrowID = GameEntry.Timer.AddOnceTimer((long)(m_ShowArrowTime * 1000), () => m_Player.ShowHandArrow());
                }
            }
            else
            {
                m_Enemy = owner as EnemyLogic;
                if (m_IsHideArrow)
                {
                    m_HideHandArrowID = GameEntry.Timer.AddOnceTimer((long)(m_HideArrowTime * 1000), () => m_Enemy.HideHandArrow());
                }
                if (m_IsShowArrow)
                {
                    m_ShowHandArrowID = GameEntry.Timer.AddOnceTimer((long)(m_ShowArrowTime * 1000), () => m_Enemy.ShowHandArrow());
                }
            }
         
        }

        private void EffectEvent()
        {
            m_Player = owner as PlayerLogic;
            if (m_Player != null)
            {
                for (int i = 0; i < EffectTimelist.Count; i++)
                {
                    SkillEffectTime effectTime = EffectTimelist[i];
                    int? m_EffectStartTimer = null;
                    if (effectTime.m_KeepTime < 0)
                    {
                        m_IsLoopEffect = true;
                    }
                    else
                    {
                        m_IsLoopEffect = false;
                    }
                    m_EffectStartTimer = GameEntry.Timer.AddOnceTimer((long)(effectTime.m_StartEffectTime * 1000), () => m_Player.PlayerSKillEffect(effectTime, out m_LoopEffectID));
                    m_EffectTimeList.Add(m_EffectStartTimer);
                }

            }
            else
            {
                m_Enemy = owner as EnemyLogic;
                if (m_Enemy == null)
                {
                    Log.Warning("not found owner");
                    return;
                }
                for (int i = 0; i < EffectTimelist.Count; i++)
                {
                    SkillEffectTime effectTime = EffectTimelist[i];
                    int? m_EffectStartTimer = null;
                    if (effectTime.m_KeepTime < 0)
                    {
                        m_IsLoopEffect = true;
                    }
                    else
                    {
                        m_IsLoopEffect = false;
                    }

                    m_EffectStartTimer = GameEntry.Timer.AddOnceTimer((long)(effectTime.m_StartEffectTime * 1000), () => m_Enemy.PlayerSKillEffect(effectTime, out m_LoopEffectID));
                    m_EffectTimeList.Add(m_EffectStartTimer);
                }
            }

        }

        #endregion

        #region 回调函数

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateEnter(animator, stateInfo, layerIndex);
            owner = animator.gameObject.GetComponent<TargetableObject>();
            m_Player = owner as PlayerLogic;
            m_LoopEffectID = 0;
            m_EffectTimeList = new List<int?>();
            m_AttackHurtEffectTimeList = new List<int?>();
            EffectEvent();
            ArrowShowOrHide();

            AttackHurtEffectFunction();
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateExit(animator, stateInfo, layerIndex);
            if (m_IsLoopEffect)
            {
                if (m_LoopEffectID != 0)
                {
                    GameEntry.Entity.HideEntity(m_LoopEffectID);
                }
            }

            // 清除攻击伤害效果计时器
            for (int i = 0; i < m_AttackHurtEffectTimeList.Count; i++)
            {
                if (m_AttackHurtEffectTimeList[i] != null)
                {
                    if (GameEntry.Timer.IsExistTimer((int)m_AttackHurtEffectTimeList[i]))
                    {
                        GameEntry.Timer.CancelTimer((int)m_AttackHurtEffectTimeList[i]);
                    }
                }
            }
            m_AttackHurtEffectTimeList.Clear();

            for (int i = 0; i < m_EffectTimeList.Count; i++)
            {
                if (m_EffectTimeList[i] != null)
                {
                    if (GameEntry.Timer.IsExistTimer((int)m_EffectTimeList[i]))
                    {
                        GameEntry.Timer.CancelTimer((int)m_EffectTimeList[i]);
                    }
                }
            }
            m_EffectTimeList.Clear();

            if (m_IsHideArrow)
            {
                if (m_HideHandArrowID != null)
                {
                    if (GameEntry.Timer.IsExistTimer((int)m_HideHandArrowID))
                    {
                        GameEntry.Timer.CancelTimer((int)m_HideHandArrowID);
                    }
                }
            }

            if (m_IsShowArrow)
            {
                if (m_ShowHandArrowID != null)
                {
                    if (GameEntry.Timer.IsExistTimer((int)m_ShowHandArrowID))
                    {
                        GameEntry.Timer.CancelTimer((int)m_ShowHandArrowID);
                    }
                }
            }

        }

        #endregion

        #region 类

        /// <summary>
        /// 攻击伤害效果
        /// </summary>
        [Serializable, SerializeField]
        private struct AttackHurtEffect
        {
            /// <summary>
            /// 顿帧时间
            /// </summary>
            [LabelText("顿帧时间"), HideLabel]
            public float m_StutterFrame;

            /// <summary>
            /// 士气值
            /// </summary>
            [LabelText("士气值"), HideLabel]
            public int MoraleValue;

            /// <summary>
            /// 是否无视格挡
            /// </summary>
            [LabelText("是否无视格挡")]
            public bool m_IgnoreParry;

            /// <summary>
            /// 是否无视弹反
            /// </summary>
            [LabelText("是否无视弹反")]
            public bool m_IgnoreRebound;

            /// <summary>
            /// 敌人是否产生顿帧
            /// </summary>
            [LabelText("敌人是否产生顿帧")]
            public bool m_IsProduceSF;
        }

        #endregion

    }
}
