using UnityEngine;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System;
using UnityGameFramework.Runtime;
using GameFramework.Event;

namespace Farm.Hotfix
{
    public class AnimationEvent : StateMachineBehaviour
    {
        private TargetableObject owner;

        [TableList, LabelText("攻击伤害判断列表"), InfoBox("基于动画时间,填入以秒为单位"), SerializeField]
        private List<AttackTime> AttackTimelist;

        [TableList, LabelText("特效列表"), SerializeField]
        private List<EffectTime> EffectTimelist;

        [TableList, LabelText("声音列表"), SerializeField]
        private List<SoundTime> SoundTimelist;

        [TableList, LabelText("霸体列表"), SerializeField]
        private List<StoicTime> StoicTimelist;

        [TableList, LabelText("攻击时间与速度"), SerializeField]
        private List<AttackVelocityAndTime> AttackVelocityAndTimes;

        [LabelText("顿帧时间与强度"), InfoBox("顿帧效果原理为修改游戏整体运行速度，建议顿帧时间小于1s"), SerializeField]
        private StopFrameTime StopFrameTimes;

        [LabelText("是否消耗勇气值"), ToggleLeft, HideLabel, SerializeField]
        private bool m_IsSpendCourse;

        [LabelText("消耗勇气值"), HideLabel, SerializeField, EnableIf("m_IsSpendCourse")]
        private int m_CourseValue;

        [LabelText("造成躯干值"), HideLabel, SerializeField,]
        private int m_TakeTrunkValue;


        [LabelText("玩家技能属性编号"), HideLabel, SerializeField]
        public int m_SkillGainNum;

        [Serializable, SerializeField]
        private struct AttackVelocityAndTime
        {
            [LabelText("攻击开始时间"), HideLabel]
            public float attackStartTime;

            [LabelText("攻击结束时间"), HideLabel]
            public float attackEndTime;

            [LabelText("攻击速度"), HideLabel]
            public float attackVelocity;
        }


        [Serializable, SerializeField]
        private struct AttackTime
        {
            [LabelText("攻击开始时间"), HideLabel]
            public float m_StartAttackTime;
            [LabelText("攻击结束时间"), HideLabel]
            public float m_EndAttackTime;
            [LabelText("Buff"), HideLabel]
            public BuffType BuffTypeEnum;
            [LabelText("顿帧时间"), HideLabel]
            public float m_StutterFrame;
            [LabelText("士气值"), HideLabel]
            public int MoraleValue;
            [LabelText("是否无视格挡")]
            public bool m_IgnoreParry;
            [LabelText("是否无视弹反")]
            public bool m_IgnoreRebound;
            [LabelText("敌人是否产生顿帧")]
            public bool m_IsProduceSF;
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
            [LabelText("是否跟随"), HideLabel,DisableIf("m_IsCoustomLogic")]
            public bool m_IsFollow;

        }

        [Serializable, SerializeField]
        private struct SoundTime
        {
            [LabelText("音效开始时间"), HideLabel]
            public float m_StartSoundTime;
            [LabelText("音效实体ID"), HideLabel]
            public int m_ID;
            [LabelText("是否在该动画循环"), HideLabel]
            public bool m_IsLoop;
        }


        [Serializable, SerializeField]
        private struct StoicTime
        {
            [LabelText("霸体开始时间"), HideLabel]
            public float m_StartStoicTime;
            [LabelText("霸体结束时间"), HideLabel]
            public float m_EndStoicTime;
            [LabelText("是否全霸体"), HideLabel]
            public bool allStoic;
        }

        [Serializable, SerializeField]
        private struct StopFrameTime
        {
            [LabelText("盾反顿帧时间"), HideLabel]
            public float m_StopFrameTime;

            [LabelText("盾反顿帧强度"), InfoBox("强度越小，效果越强"), HideLabel]
            [Range(0.2f,1f)]
            public float m_StopFrameSize;

            [LabelText("顿帧开始时间"), InfoBox("时间为0则将在进入动画时开启，开始时间不应超过动画执行时间"), HideLabel]
            public float m_StopFrameSizeStartTime;

            [LabelText("主动开启顿帧"), SerializeField]
            public bool startStopFrame;
        }


        private PlayerLogic m_player;

        private EnemyLogic m_Enemy;

        private int m_LoopEffectID;
        private bool m_IsLoopEffect = false;

        private int? m_LoopSoundID;
        private bool m_IsLoopSound = false;

        private bool m_AllStoic = false;
        private int? m_AttackStartTimer = null;
        private int? m_AttackEndTimer = null;
        private int? m_BuffStartTimer = null;
        private int? m_BuffEndTimer = null;
        private int? m_StoicEndTimer = null;
        private int? m_StoicStartTimer = null;

        private List<int?> m_EffectStartTimerList = null;
        private List<int?> m_EffectCustomStartTimerList = null;

        private List<int?> m_SoundStartTimeList = null;


        public bool TestDebug;

        /// <summary>
        /// 动画原速度
        /// </summary>
        private float originVelocity;

        /// <summary>
        /// 速度变化量
        /// </summary>
        private float velocityChangeValue;

        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateEnter(animator, stateInfo, layerIndex);
            owner = animator.gameObject.GetComponent<TargetableObject>();
            m_LoopEffectID = 0;
            m_EffectStartTimerList = new List<int?>();
            m_EffectCustomStartTimerList = new List<int?>();
            m_SoundStartTimeList = new List<int?>();
            if (!GameEntry.Event.Check(ApplyDamageEventArgs.EventId, ApplyDamageEvent))
            {
                GameEntry.Event.Subscribe(ApplyDamageEventArgs.EventId, ApplyDamageEvent);
            }
            if (!GameEntry.Event.Check(StutterEventArgs.EventId, StutterEvent))
            {
                GameEntry.Event.Subscribe(StutterEventArgs.EventId, StutterEvent);
            }

            if (!m_SkillGainNum.Equals(0))          // 添加技能伤害加成
            {
                if (AIUtility.ContainsSkillGainTable(m_SkillGainNum))
                {
                    AIUtility.SetSkillGain(m_SkillGainNum);
                }
                else if (GameEntry.DataTable.HasDataTable(typeof(DRPlayerSkillGain)))
                {
                    if (GameEntry.DataTable.GetDataTable<DRPlayerSkillGain>().HasDataRow(m_SkillGainNum))
                    {
                        AIUtility.AddSkillGainTable(m_SkillGainNum);
                        AIUtility.SetSkillGain(m_SkillGainNum);
                    }
                    else
                    {
                        AnimationClip[] temp = animator.runtimeAnimatorController.animationClips;           // 处理技能加成倍数编号不存在的情况
                        for (int i = 0; i < temp.Length; i++)
                        {
                            if (stateInfo.IsName(temp[i].name))
                            {
                                Log.Error(string.Format("不存在指定的玩家技能属性编号：{0}，角色动画名称：{1}，使用的技能名称：{2}", m_SkillGainNum, animator.name, temp[i].name));
                                break;
                            }
                        }
                        AIUtility.SetSkillGain(0);
                    }
                }
                else
                {
                    Log.Error("玩家技能属性配置表!!!");
                }
            }
            else
            {
                AIUtility.SetSkillGain(0);
            }

            AttackEvent();
            EffectEvent();
            SoundEvent();
            StoicEvent();


            // 保存动画初始速度
            originVelocity = animator.speed;

            // 得出速度变化量
            velocityChangeValue = attackVelocityAndTime.attackVelocity - originVelocity;

            AttackTimeAndVelocityStart(animator, stateInfo);
            if (StopFrameTimes.startStopFrame)
            {
                m_player = owner as PlayerLogic;
                m_EffectCustomStartTimerList.Add(GameEntry.Timer.AddOnceTimer((long)(StopFrameTimes.m_StopFrameSizeStartTime * 1000), () => { m_player.SetStopFrameTimeAndSize(StopFrameTimes.m_StopFrameTime, StopFrameTimes.m_StopFrameSize, true); }));
            }
        }

        private void AttackEvent()
        {
            m_player = owner as PlayerLogic;
            if (m_player != null)
            {
                m_player.SetStopFrameTimeAndSize(StopFrameTimes.m_StopFrameTime, StopFrameTimes.m_StopFrameSize);
                for (int i = 0; i < AttackTimelist.Count; i++)
                {
                    AttackTime attackTime = AttackTimelist[i];

                    if (!m_SkillGainNum.Equals(0))              // 应用配置表中设定的士气值
                    {
                        m_AttackStartTimer = GameEntry.Timer.AddOnceTimer((long)(attackTime.m_StartAttackTime * 1000), () => m_player.AttackStart(attackTime.m_StutterFrame, AIUtility.GetSkillGainTable(m_SkillGainNum).MoraleValue, attackTime.m_IgnoreParry, attackTime.m_IgnoreRebound, attackTime.m_IsProduceSF));
                    }
                    else
                    {
                        m_AttackStartTimer = GameEntry.Timer.AddOnceTimer((long)(attackTime.m_StartAttackTime * 1000), () => m_player.AttackStart(attackTime.m_StutterFrame, attackTime.MoraleValue, attackTime.m_IgnoreParry, attackTime.m_IgnoreRebound, attackTime.m_IsProduceSF));
                    }
                    
                    m_AttackEndTimer = GameEntry.Timer.AddOnceTimer((long)(attackTime.m_EndAttackTime * 1000), () => m_player.AttackEnd());
                    if (attackTime.BuffTypeEnum != BuffType.None)
                    {
                        m_BuffStartTimer = GameEntry.Timer.AddOnceTimer((long)(attackTime.m_StartAttackTime * 1000), () => m_player.DeBuffAnimStart(attackTime.BuffTypeEnum));
                        m_BuffEndTimer = GameEntry.Timer.AddOnceTimer((long)(attackTime.m_EndAttackTime * 1000), () => m_player.DeBuffAnimEnd());
                    }
                }
                m_player.SetTakeTrunkValue(m_TakeTrunkValue);
            }
            else
            {
                m_Enemy = owner as EnemyLogic;
                if (m_Enemy == null)
                {
                    Log.Warning("not found owner");
                    return;
                }
                for (int i = 0; i < AttackTimelist.Count; i++)
                {
                    AttackTime attackTime = AttackTimelist[i];
                    m_AttackStartTimer = GameEntry.Timer.AddOnceTimer((long)(attackTime.m_StartAttackTime * 1000), () => m_Enemy.EnemyAttackStart(attackTime.m_IgnoreParry, attackTime.m_IgnoreRebound,attackTime.MoraleValue));
                    m_AttackEndTimer = GameEntry.Timer.AddOnceTimer((long)(attackTime.m_EndAttackTime * 1000), () => m_Enemy.EnemyAttackEnd());
                    if (attackTime.BuffTypeEnum != BuffType.None)
                    {
                        m_BuffStartTimer = GameEntry.Timer.AddOnceTimer((long)(attackTime.m_StartAttackTime * 1000), () => m_Enemy.DeBuffAnimStart(attackTime.BuffTypeEnum));
                        m_BuffEndTimer = GameEntry.Timer.AddOnceTimer((long)(attackTime.m_EndAttackTime * 1000), () => m_Enemy.DeBuffAnimEnd());
                    }
                }

                m_Enemy.SetTakeTrunkValue(m_TakeTrunkValue);
            }
        }
        private void StoicEvent()
        {
            m_player = owner as PlayerLogic;
            if (m_player != null)
            {
                for (int i = 0; i < StoicTimelist.Count; i++)
                {
                    StoicTime stoicTime = StoicTimelist[i];
                    if (stoicTime.allStoic)
                    {
                        m_player.isStoic = stoicTime.allStoic;
                        m_AllStoic = true;
                    }
                    else
                    {
                        m_StoicStartTimer = GameEntry.Timer.AddOnceTimer((long)(stoicTime.m_StartStoicTime * 1000), () => m_player.isStoic = true);
                        m_StoicEndTimer = GameEntry.Timer.AddOnceTimer((long)(stoicTime.m_EndStoicTime * 1000), () => m_player.isStoic = false);
                    }
                }
                //m_player.isStoic = Stoic;
            }
            else
            {
                m_Enemy = owner as EnemyLogic;
                if (m_Enemy == null)
                {
                    Log.Warning("not found owner");
                    return;
                }
                for (int i = 0; i < StoicTimelist.Count; i++)
                {
                    StoicTime stoicTime = StoicTimelist[i];
                    if (stoicTime.allStoic)
                    {
                        m_Enemy.Stoic = stoicTime.allStoic;
                        m_AllStoic = true;
                    }
                    else
                    {
                        m_StoicStartTimer = GameEntry.Timer.AddOnceTimer((long)(stoicTime.m_StartStoicTime * 1000), () => m_Enemy.Stoic = true);
                        m_StoicEndTimer = GameEntry.Timer.AddOnceTimer((long)(stoicTime.m_EndStoicTime * 1000), () => m_Enemy.Stoic = false);
                    }
                }
                //m_Enemy.Stoic = Stoic;
            }
        }
        private void StoicEventEnd()
        {
            m_player = owner as PlayerLogic;
            if (m_player != null)
            {
                m_player.isStoic = false;
            }
            else
            {
                m_Enemy = owner as EnemyLogic;
                if (m_Enemy == null)
                {
                    Log.Warning("not found owner");
                    return;
                }
                m_Enemy.Stoic = false;
            }
        }

        private void DefenseEvent()
        {

        }

        private void StutterEvent(object sender,GameEventArgs e)
        {
            StutterEventArgs ne = (StutterEventArgs)e;
            if(ne.UserData != m_player)
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
                        GameEntry.Timer.AddOnceTimer((long)(m_player.StutterFrameTime * 2 * 1000), () => GameEntry.Timer.ResumeTimer((int)effectTime));
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
            if (m_AttackStartTimer != null)
            {
                if (GameEntry.Timer.IsExistTimer((int)m_AttackStartTimer))
                {
                    GameEntry.Timer.CancelTimer((int)m_AttackStartTimer);
                }
                if (m_AttackEndTimer != null)
                {
                    if (GameEntry.Timer.IsExistTimer((int)m_AttackEndTimer))
                    {
                        GameEntry.Timer.ChangeTime((int)m_AttackEndTimer, 0);
                    }

                }
            }

            if (m_BuffStartTimer != null)
            {
                if (GameEntry.Timer.IsExistTimer((int)m_AttackStartTimer))
                {
                    GameEntry.Timer.CancelTimer((int)m_BuffStartTimer);
                }
                if (m_BuffEndTimer != null)
                {
                    if (GameEntry.Timer.IsExistTimer((int)m_BuffEndTimer))
                    {
                        GameEntry.Timer.ChangeTime((int)m_BuffEndTimer, 0);
                    }
                }
            }
            if (m_StoicStartTimer != null)
            {
                if (GameEntry.Timer.IsExistTimer((int)m_StoicStartTimer))
                {
                    GameEntry.Timer.CancelTimer((int)m_StoicStartTimer);
                }
                if (m_StoicEndTimer != null)
                {
                    if (GameEntry.Timer.IsExistTimer((int)m_StoicEndTimer))
                    {
                        GameEntry.Timer.ChangeTime((int)m_StoicEndTimer, 0);
                    }

                }
            }
        }

        private void EffectEvent()
        {
            m_player = owner as PlayerLogic;
            if (m_player != null)
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
                        m_EffectCustomStartTimer = GameEntry.Timer.AddOnceTimer((long)(effectTime.m_StartEffectTime * 1000), () => m_player.PlayEffect(effectTime.m_ParentName, effectTime.m_ID, effectTime.m_Rotate, effectTime.m_KeepTime, type, out m_LoopEffectID));
                        m_EffectCustomStartTimerList.Add(m_EffectCustomStartTimer);
                    }
                    else
                    {
                        m_EffectStartTimer = GameEntry.Timer.AddOnceTimer((long)(effectTime.m_StartEffectTime * 1000), () => m_player.PlayEffect(effectTime.m_ParentName, effectTime.m_ID, effectTime.m_Rotate, effectTime.m_KeepTime,effectTime.m_IsFollow, out m_LoopEffectID));
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
                        m_EffectStartTimer = GameEntry.Timer.AddOnceTimer((long)(effectTime.m_StartEffectTime * 1000), () => m_Enemy.PlayEffect(effectTime.m_ParentName, effectTime.m_ID, effectTime.m_Rotate, effectTime.m_KeepTime,effectTime.m_IsFollow, out m_LoopEffectID));
                        m_EffectStartTimerList.Add(m_EffectStartTimer);
                    }
                }

            }
        }

        private void SoundEvent()
        {
            m_player = owner as PlayerLogic;
            if (m_player != null)
            {
                for (int i = 0; i < SoundTimelist.Count; i++)
                {
                    int ID = SoundTimelist[i].m_ID;

                    int? m_SoundTimeID = null;
                    if (SoundTimelist[i].m_IsLoop)
                    {
                        m_IsLoopSound = true;
                    }
                    else
                    {
                        m_IsLoopSound = false;
                    }
                    m_SoundTimeID = GameEntry.Timer.AddOnceTimer((long)(SoundTimelist[i].m_StartSoundTime * 1000), () => m_player.PlayerSound(ID, out m_LoopSoundID));
                    m_SoundStartTimeList.Add(m_SoundTimeID);
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
                for (int i = 0; i < SoundTimelist.Count; i++)
                {
                    int ID = SoundTimelist[i].m_ID;
                    int? m_SoundTimeID = null;
                    if (SoundTimelist[i].m_IsLoop)
                    {
                        m_IsLoopSound = true;
                    }
                    else
                    {
                        m_IsLoopSound = false;
                    }
                    m_SoundTimeID = GameEntry.Timer.AddOnceTimer((long)(SoundTimelist[i].m_StartSoundTime * 1000), () => m_Enemy.PlayerSound(ID, out m_LoopSoundID));
                    m_SoundStartTimeList.Add(m_SoundTimeID);
                }

            }
        }

        #region 攻击时间与速度

        /// <summary>
        /// 攻击时间与速度的结构体
        /// </summary>
        private AttackVelocityAndTime attackVelocityAndTime;

        /// <summary>
        /// 是否更改速度
        /// </summary>
        private bool isChangeVelocity = false;

        /// <summary>
        /// 攻击时间与速度启动方法
        /// </summary>
        /// <param name="animator"></param>
        /// <param name="stateInfo"></param>
        private void AttackTimeAndVelocityStart(Animator animator, AnimatorStateInfo stateInfo)
        {

            for (int i = 0; i < AttackVelocityAndTimes.Count; i++)
            {
                attackVelocityAndTime = AttackVelocityAndTimes[i];

                // 如果动画更改时间大于动画时长
                if (attackVelocityAndTime.attackStartTime > stateInfo.length)
                    return;

                isChangeVelocity = true;
            }
        }

        /// <summary>
        /// 攻击时间与速度计时器
        /// </summary>
        private float timer_AttackTimeAndVelocity = 0;

        /// <summary>
        /// 攻击时间与速度更新方法
        /// </summary>
        private void AttackTimeAndVelocityUpdate(Animator animator, AnimatorStateInfo stateInfo)
        {
            if (!isChangeVelocity)
            {
                return;
            }

            // 计时器记时
            timer_AttackTimeAndVelocity += Time.deltaTime;

            // 目标速度
            float targetVelocity = attackVelocityAndTime.attackVelocity * originVelocity;

            // 当前进度
            float progress = timer_AttackTimeAndVelocity / (attackVelocityAndTime.attackEndTime - attackVelocityAndTime.attackStartTime);

            // 进度完成
            if (progress > 0.99f)
                isChangeVelocity = false;

            // 当前进度小于 0.5
            if (progress < 0.5f)
            {
                // 当前速度
                float presentVelocity = originVelocity + velocityChangeValue * Mathf.Lerp(0f, 1f, progress * 2);

                animator.speed = presentVelocity;
            }
            else
            {
                // 当前速度
                float presentVelocity = attackVelocityAndTime.attackVelocity - velocityChangeValue * Mathf.Lerp(0f, 1f, (progress - 0.5f) * 2);
                
                animator.speed = presentVelocity;
            }
        }

        #endregion

        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateUpdate(animator, stateInfo, layerIndex);
            StoicEvent();

            AttackTimeAndVelocityUpdate(animator, stateInfo);
        }

        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
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
            if (m_AllStoic)
            {
                StoicEventEnd();
                m_AllStoic = false;
            }

            if (m_IsLoopEffect)
            {
                if (m_LoopEffectID != 0)
                {
                    GameEntry.Entity.HideEntity(m_LoopEffectID);
                }
            }

            if (m_IsLoopSound)
            {
                if(m_LoopSoundID != 0 && m_LoopSoundID!=null)
                {
                    GameEntry.Sound.StopSound((int)m_LoopSoundID);
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

            for (int i = 0; i < m_SoundStartTimeList.Count; i++)
            {
                if(m_SoundStartTimeList[i] != null)
                {
                    if (GameEntry.Timer.IsExistTimer((int)m_SoundStartTimeList[i]))
                    {
                        GameEntry.Timer.CancelTimer((int)m_SoundStartTimeList[i]);
                    }
                }
            }
            
            m_EffectCustomStartTimerList.Clear();
            m_EffectStartTimerList.Clear();
            if (m_IsSpendCourse)
            {
                m_player = owner as PlayerLogic;
                if (m_player != null)
                {
                    m_player.SetCourseValue(m_CourseValue);
                }
            }

            #region 攻击时间与速度

            // 计时器归零
            timer_AttackTimeAndVelocity = 0;

            isChangeVelocity = false;

            /*if (originVelocity != 0)
            {
                // 动画速度还原
                animator.speed = originVelocity;
                Debug.Log($"*************originVelocity=={originVelocity}***************");
            }*/

            animator.speed = 1;
            
            #endregion
        }
    }
}