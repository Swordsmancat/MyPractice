using System.Collections;
using System.Collections.Generic;
using UnityGameFramework.Runtime;
using GameFramework.Fsm;
using UnityEngine;
using System;
using GameFramework;

namespace Farm.Hotfix
{
    public class OrcDoubleAxeLogic : EnemyLogic
    {
        private readonly static string s_startPoint = "StartPoint";//右手武器特效点位名
        private Vector3 EffectsPoint_R; //特效点位置右
        private GameObject Weapon_StartPoint;
        public BuffType m_BuffType;
        private bool m_IgnoreParry;

        private static readonly int m_Rebound = Animator.StringToHash("Rebound");


        protected override void OnDead(Entity attacker, Vector3 point)
        {
            base.OnDead(attacker, point);
            EnemyAttackEnd();
            GameEntry.Entity.ShowEffect(new EffectData(GameEntry.Entity.GenerateSerialId(), enemyData.DeadEffectId)
            {
                Position = point,
            });
        }

        protected override void AddFsmState()
        {
            stateList.Add(OrcDoubleAxeIdleState.Create());
            stateList.Add(OrcDoubleAxeMotionState.Create());
            stateList.Add(OrcDoubleAxeAttackState.Create());
            stateList.Add(OrcDoubleAxeDeadState.Create());
            //stateList.Add(EnemyDeadState.Create());
            //stateList.Add(OrcHurtState.Create());
            stateList.Add(OrcDoubleAxeHurtState.Create());
            stateList.Add(EnemyRotateState.Create());
            //stateList.Add(EnemyShoutState.Create());
            stateList.Add(OrcDoubleAxeShoutState.Create());
            stateList.Add(OrcDoubleAxeFightState.Create());
            //stateList.Add(EnemyFightState.Create());
            stateList.Add(OrcDoubleAxeOutOfTheFight.Create());
            stateList.Add(EnemyParryState.Create());
            stateList.Add(EnemyKnockedDownState.Create());
            stateList.Add(EnemyVertigoState.Create());
            //stateList.Add(EnemyOutOfTheFight.Create());

        }

        public override Type ChangeStateEnemy(EnemyStateType stateType)
        {
            switch (stateType)
            {
                case EnemyStateType.Idle:
                    //Debug.Log("站立");
                    return typeof(OrcDoubleAxeIdleState);
                //return typeof(EnemyIdleState);
                case EnemyStateType.Motion:
                    //Debug.Log("移动");
                    return typeof(OrcDoubleAxeMotionState);
                //return typeof(EnemyMotionState);
                case EnemyStateType.Attack:
                    //Debug.Log("攻击");
                    return typeof(OrcDoubleAxeAttackState);
                case EnemyStateType.Dead:
                    return typeof(OrcDoubleAxeDeadState);
                //return typeof(EnemyDeadState);
                case EnemyStateType.Hurt:
                    //return typeof(OrcHurtState);
                    return typeof(OrcDoubleAxeHurtState);
                case EnemyStateType.Rotate:
                    return typeof(EnemyRotateState);
                case EnemyStateType.Fight:
                    return typeof(OrcDoubleAxeFightState);
                //return typeof(EnemyFightState);
                case EnemyStateType.OutOfFight:
                    return typeof(OrcDoubleAxeOutOfTheFight);
                //return typeof(EnemyOutOfTheFight);
                case EnemyStateType.Shout:
                    //return typeof(EnemyShoutState);
                    return typeof(OrcDoubleAxeShoutState);
                case EnemyStateType.Parry:
                    return typeof(EnemyParryState);
                case EnemyStateType.KnockedDown:
                    return typeof(EnemyKnockedDownState);
                case EnemyStateType.Vertigo:
                    return typeof(EnemyVertigoState);
                default:
                    //return typeof(EnemyIdleState);
                    return typeof(OrcDoubleAxeIdleState);
            }
        }

        protected override void StartFsm()
        {
            //fsm.Start<EnemyIdleState>();
            fsm.Start<OrcDoubleAxeIdleState>();
        }
        /// <summary>
        /// 攻击人物声音
        /// </summary>

        public override void ApplyDamage(Entity attacker, Entity attackType, int damageHP, int damageTrunk, Vector3 weapon)
        {


            TargetableObject targetable = attacker as TargetableObject;
            isBehindAtked = (AIUtility.GetDot(this, attacker)) > 0;
            if (targetable != null)
            {
                m_Animator.ResetTrigger(m_Rebound);
                if (!targetable.IgnoreRebound)
                {
                    if (IsRebound)
                    {
                        if (m_IsCanRebound)
                        {

                            targetable.GetRebound = true;
                            targetable.GetReboundState = ReboundState;
                            m_Animator.SetTrigger(m_Rebound);
                            if (m_EnemyData.MoraleValue + TakeMoralevalue > 0)
                            {
                                if (m_EnemyData.MoraleValue + TakeMoralevalue >= 100)
                                {
                                    m_EnemyData.MoraleValue = 100;
                                }
                                else
                                {
                                    m_EnemyData.MoraleValue = m_EnemyData.MoraleValue + TakeMoralevalue;
                                }
                            }
                            else
                            {
                                m_EnemyData.MoraleValue = 0;
                            }
                            GameEntry.Event.Fire(this, ApplyDamageEventArgs.Create(targetable));
                            m_IsCanRebound = false;
                            return;
                        }
                    }
                }


            }

            if (attackType as ArrowLogic)
            {
                ArrowLogic arrowLogic = attackType as ArrowLogic;
                m_IgnoreParry = arrowLogic.ArrowData.IgnoreParry;
            }
            else if (attackType as SkillEffectLogic)
            {
                SkillEffectLogic skillLogic = attackType as SkillEffectLogic;
                m_IgnoreParry = skillLogic.m_SkillEffectData.SkillEffectTime.m_IgnoreParry;
            }
            else
            {
                m_IgnoreParry = false;
            }
            if (Utility.Random.GetRandom(0, 11) > 5 && isBehindAtked && GetCollider == ColliderState.None && enemyData.TrunkValue > 0 && enemyData.VertigoValue > 0 && !targetable.IgnoreParry && !m_IsInKnockedDown && !m_IsNotToDefense && !IsBreak && !m_IgnoreParry)
            {
                IsDefense = true;
            }
            if (IsDefense)
            {
                GameEntry.Event.Fire(attacker, ApplyDefenseEventArgs.Create(this));
                return;
            }





            base.ApplyDamage(attacker, attackType, damageHP, damageTrunk, weapon);
            if (AIUtility.GetDot(this, attacker) < 0)
            {

                GameEntry.Entity.ShowEffect(new EffectData(GameEntry.Entity.GenerateSerialId(), enemyData.BehindHurtEffectID)
                {
                    Position = weapon
                });
                if (IsGetCrit)
                {
                    GameEntry.Entity.ShowEffect(new EffectData(GameEntry.Entity.GenerateSerialId(), enemyData.GetCritEffectID)
                    {
                        Position = weapon
                    });
                }
            }
            else
            {
                if (IsDefense)
                {
                    if (GetCollider != ColliderState.None)
                    {
                        GameEntry.Entity.ShowEffect(new EffectData(GameEntry.Entity.GenerateSerialId(), enemyData.MotionSoundId0)
                        {
                            Position = weapon
                        });

                    }
                }
                else
                {

                    if (GetCollider != ColliderState.None)
                    {
                        GameEntry.Entity.ShowEffect(new EffectData(GameEntry.Entity.GenerateSerialId(), enemyData.MotionSoundId1)
                        {
                            Position = weapon
                        });

                    }
                    else
                    {
                        if (!Stoic)
                        {
                            GameEntry.Sound.PlaySound(enemyData.ByAttackSoundId);
                            if (enemyData.TrunkValue > 0)
                            {
                                GameEntry.Sound.PlaySound(enemyData.StoicHurtSoundId);
                                GameEntry.Entity.ShowEffect(new EffectData(GameEntry.Entity.GenerateSerialId(), enemyData.MotionSoundId2)
                                {
                                    Position = weapon
                                });

                            }
                            else
                            {
                                GameEntry.Entity.ShowEffect(new EffectData(GameEntry.Entity.GenerateSerialId(), enemyData.BloodEffectId)
                                {
                                    Position = weapon
                                });

                            }
                            if (IsGetCrit)
                            {
                                GameEntry.Entity.ShowEffect(new EffectData(GameEntry.Entity.GenerateSerialId(), enemyData.GetCritEffectID)
                                {
                                    Position = weapon
                                });
                            }


                        }
                        else
                        {
                            GameEntry.Sound.PlaySound(enemyData.StoicHurtSoundId);
                            GameEntry.Entity.ShowEffect(new EffectData(GameEntry.Entity.GenerateSerialId(), enemyData.StoicHurtEffectId)
                            {
                                Position = weapon
                            });
                        }
                    }

                }
            }



        }

        public virtual void Delayed()
        {
            m_Animator.speed = 0.5f;
        }
        public virtual void DelayedT()
        {
            m_Animator.speed = 0.25f;
        }
        public virtual void DelayedP()
        {
            m_Animator.speed = 0.1f;
        }


        public virtual void DelayedEnd()
        {
            m_Animator.speed = 1f;
        }
        public override void ApplyBuffEvent(BuffType buffType)
        {
            base.ApplyBuffEvent(buffType);
            m_BuffType = buffType;

        }
    }
}
