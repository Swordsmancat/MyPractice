//------------------------------------------------------------
// Creator: FatCat
// Creation date: 2022/10/19/周三 18:03:42
//------------------------------------------------------------


using GameFramework.Event;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace Farm.Hotfix
{
    public class BodyStrikeAnimationEvent : StateMachineBehaviour
    {

        private TargetableObject owner;

        private PlayerLogic m_Player;

        private EnemyLogic m_Enemy;

        [TableList, LabelText("肢体伤害判断列表"), InfoBox("基于动画时间,填入以秒为单位"), SerializeField]
        private List<BodyStrikeTime> BodyStrikeTimeList;

        [TableList, LabelText("特效列表"), SerializeField]
        private List<EffectTime> EffectTimelist;

        [Serializable, SerializeField]
        private struct BodyStrikeTime
        {
            [LabelText("攻击开始时间"), HideLabel]
            public float m_StartAttackTime;
            [LabelText("攻击结束时间"), HideLabel]
            public float m_EndAttackTime;
            [LabelText("肢体部位"), HideLabel]
            public ColliderState m_ColliderState;
            [LabelText("顿帧时间"), HideLabel]
            public float m_StutterFrame;
        }

        [Serializable, SerializeField]
        private class EffectTime
        {
            [LabelText("特效开始时间"), HideLabel]
            public float m_StartEffectTime;

            [LabelText("特效实体ID"), HideLabel]
            public int m_ID;
            [LabelText("特效持续时间"), HideLabel]
            public float m_KeepTime;
            [LabelText("父节点名称"), HideLabel]
            public string m_ParentName;
            [LabelText("旋转角度"), HideLabel]
            public Vector3 m_Rotate;
            [ToggleLeft, LabelText("是否使用自定义脚本"), HideLabel]
            public bool m_IsCoustomLogic;
            [LabelText("自定义特效脚本名称"), HideLabel, EnableIf("m_IsCoustomLogic")]
            public string m_EffectLogic;
            [LabelText("是否跟随"), HideLabel, DisableIf("m_IsCoustomLogic")]
            public bool m_IsFollow;

        }

        private int? m_BodyStrikeStartTimer = null;
        private int? m_BodyStrikeEndTimer = null;
        private int m_LoopEffectID;
        private bool m_IsLoopEffect = false;
        private List<int?> m_EffectStartTimerList = null;
        private List<int?> m_EffectCustomStartTimerList = null;

        [SerializeField]
        private int m_MoraleValue;


        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateEnter(animator, stateInfo, layerIndex);
            owner = animator.gameObject.GetComponent<TargetableObject>();
            m_Player = owner as PlayerLogic;
            m_LoopEffectID = 0;
            m_EffectStartTimerList = new List<int?>();
            m_EffectCustomStartTimerList = new List<int?>();
            if (!GameEntry.Event.Check(StutterEventArgs.EventId, StutterEvent))
            {
                GameEntry.Event.Subscribe(StutterEventArgs.EventId, StutterEvent);
            }
            if (!GameEntry.Event.Check(ApplyDamageEventArgs.EventId, ApplyDamageEvent))
            {
                GameEntry.Event.Subscribe(ApplyDamageEventArgs.EventId, ApplyDamageEvent);
            }
            if (m_Player != null)
            {
                for (int i = 0; i < BodyStrikeTimeList.Count; i++)
                {
                    BodyStrikeTime bodyStrikeTime = BodyStrikeTimeList[i];
                    m_BodyStrikeStartTimer =GameEntry.Timer.AddOnceTimer((long)(bodyStrikeTime.m_StartAttackTime*1000),() => m_Player.BodyStrikeStart(bodyStrikeTime.m_ColliderState,bodyStrikeTime.m_StutterFrame));
                    m_BodyStrikeEndTimer = GameEntry.Timer.AddOnceTimer((long)(bodyStrikeTime.m_EndAttackTime * 1000), () => m_Player.BodyStrikeEnd(bodyStrikeTime.m_ColliderState));
                }
                m_Player.SetMoraleValue(m_MoraleValue);
            }
            else
            {
                m_Enemy = owner as EnemyLogic;
                if(m_Enemy != null)
                {
                    for (int i = 0; i < BodyStrikeTimeList.Count; i++)
                    {
                        BodyStrikeTime bodyStrikeTime = BodyStrikeTimeList[i];
                        m_BodyStrikeStartTimer = GameEntry.Timer.AddOnceTimer((long)(bodyStrikeTime.m_StartAttackTime * 1000), () => m_Enemy.BodyStrikeStart(bodyStrikeTime.m_ColliderState));
                        m_BodyStrikeEndTimer = GameEntry.Timer.AddOnceTimer((long)(bodyStrikeTime.m_EndAttackTime * 1000), () => m_Enemy.BodyStrikeEnd(bodyStrikeTime.m_ColliderState));
                    }
                    m_Enemy.SetMoraleValue(m_MoraleValue);
                }
                else
                {
                    Log.Info("实体不存在");
                }
                    
            }
            EffectEvent();
        }

        private void StutterEvent(object sender, GameEventArgs e)
        {
            StutterEventArgs ne = (StutterEventArgs)e;
            if (ne.UserData != m_Player)
            {
                return;
            }
            for (int i = 0; i < m_EffectStartTimerList.Count; i++)
            {
                int? effectTime = m_EffectStartTimerList[i];
                if (effectTime != null)
                {
                    if (GameEntry.Timer.IsExistTimer((int)effectTime))
                    {
                        GameEntry.Timer.PauseTimer((int)effectTime);
                        GameEntry.Timer.AddOnceTimer((long)(m_Player.StutterFrameTime * 2 * 1000), () => GameEntry.Timer.ResumeTimer((int)effectTime));
                    }
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
            if (m_BodyStrikeStartTimer != null)
            {
                if (GameEntry.Timer.IsExistTimer((int)m_BodyStrikeStartTimer))
                {
                    GameEntry.Timer.CancelTimer((int)m_BodyStrikeStartTimer);
                }
                if (m_BodyStrikeEndTimer != null)
                {
                    if (GameEntry.Timer.IsExistTimer((int)m_BodyStrikeEndTimer))
                    {
                        GameEntry.Timer.ChangeTime((int)m_BodyStrikeEndTimer, 0);
                    }

                }
            }

        }

        private void EffectEvent()
        {
            if (m_Player != null)
            {
                for (int i = 0; i < EffectTimelist.Count; i++)
                {
                    EffectTime effectTime = EffectTimelist[i];
                    //int m_LoopEffectID = 0;
                    int? m_EffectCustomStartTimer = null;
                    int? m_EffectStartTimer = null;
                    Type type = Type.GetType("Farm.Hotfix." + effectTime.m_EffectLogic);
                    if (effectTime.m_KeepTime < 0)
                    {
                        m_IsLoopEffect = true;
                    }
                    else
                    {
                        m_IsLoopEffect = false;
                    }
                    if (effectTime.m_IsCoustomLogic)
                    {
                        m_EffectCustomStartTimer = GameEntry.Timer.AddOnceTimer((long)(effectTime.m_StartEffectTime * 1000), () => m_Player.PlayEffect(effectTime.m_ParentName, effectTime.m_ID, effectTime.m_Rotate, effectTime.m_KeepTime, type, out m_LoopEffectID));
                        m_EffectCustomStartTimerList.Add(m_EffectCustomStartTimer);
                    }
                    else
                    {
                        m_EffectStartTimer = GameEntry.Timer.AddOnceTimer((long)(effectTime.m_StartEffectTime * 1000), () => m_Player.PlayEffect(effectTime.m_ParentName, effectTime.m_ID, effectTime.m_Rotate, effectTime.m_KeepTime, effectTime.m_IsFollow, out m_LoopEffectID));
                        m_EffectStartTimerList.Add(m_EffectStartTimer);
                    }
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
                    EffectTime effectTime = EffectTimelist[i];
                    // int m_LoopEffectID = 0;
                    int? m_EffectCustomStartTimer = null;
                    int? m_EffectStartTimer = null;
                    Type type = Type.GetType("Farm.Hotfix." + effectTime.m_EffectLogic);
                    if (effectTime.m_KeepTime < 0)
                    {
                        m_IsLoopEffect = true;
                    }
                    else
                    {
                        m_IsLoopEffect = false;
                    }
                    if (effectTime.m_IsCoustomLogic)
                    {
                        m_EffectCustomStartTimer = GameEntry.Timer.AddOnceTimer((long)(effectTime.m_StartEffectTime * 1000), () => m_Enemy.PlayEffect(effectTime.m_ParentName, effectTime.m_ID, effectTime.m_Rotate, effectTime.m_KeepTime, type, out m_LoopEffectID));
                        m_EffectCustomStartTimerList.Add(m_EffectCustomStartTimer);
                    }
                    else
                    {
                        m_EffectStartTimer = GameEntry.Timer.AddOnceTimer((long)(effectTime.m_StartEffectTime * 1000), () => m_Enemy.PlayEffect(effectTime.m_ParentName, effectTime.m_ID, effectTime.m_Rotate, effectTime.m_KeepTime, effectTime.m_IsFollow, out m_LoopEffectID));
                        m_EffectStartTimerList.Add(m_EffectStartTimer);
                    }
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
            if (GameEntry.Event.Check(StutterEventArgs.EventId, StutterEvent))
            {
                GameEntry.Event.Unsubscribe(StutterEventArgs.EventId, StutterEvent);
            }

            if (m_IsLoopEffect)
            {
                if (m_LoopEffectID != 0)
                {
                    GameEntry.Entity.HideEntity(m_LoopEffectID);
                }
            }
            for (int i = 0; i < m_EffectCustomStartTimerList.Count; i++)
            {
                if (m_EffectCustomStartTimerList[i] != null)
                {
                    if (GameEntry.Timer.IsExistTimer((int)m_EffectCustomStartTimerList[i]))
                    {
                        GameEntry.Timer.CancelTimer((int)m_EffectCustomStartTimerList[i]);
                    }
                }
            }
            for (int i = 0; i < m_EffectStartTimerList.Count; i++)
            {
                if (m_EffectStartTimerList[i] != null)
                {
                    if (GameEntry.Timer.IsExistTimer((int)m_EffectStartTimerList[i]))
                    {
                        GameEntry.Timer.CancelTimer((int)m_EffectStartTimerList[i]);
                    }
                }
            }

            m_EffectCustomStartTimerList.Clear();
            m_EffectStartTimerList.Clear();
        }


    }
}
