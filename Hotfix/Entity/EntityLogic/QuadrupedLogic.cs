//------------------------------------------------------------
// Creator: FatCat
// Creation date: 2023/4/11/周二 16:44:56
//------------------------------------------------------------


using System;
using UnityEngine;

namespace Farm.Hotfix
{
    public class QuadrupedLogic : EnemyLogic
    {

        protected override void AddFsmState()
        {
            stateList.Add(QuadrupedIdleState.Create());
            stateList.Add(QuadrupedMotionState.Create());
            stateList.Add(QuadrupedHurtState.Create());
            stateList.Add(QuadrupedAttackState.Create());
            stateList.Add(QuadrupedDeadState.Create());
            stateList.Add(EnemyShoutState.Create());
            stateList.Add(QuadrupedFightState.Create());
            stateList.Add(QuadrupedOfTFState.Create());
            stateList.Add(EnemyVertigoState.Create());
        }

        public override void ApplyDamage(Entity attacker,Entity attackType, int damageHP, int damageTrunk, Vector3 weapon)
        {
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

        }

        public override Type ChangeStateEnemy(EnemyStateType stateType)
        {
            switch (stateType)
            {
                case EnemyStateType.Idle:
                    return typeof(QuadrupedIdleState);
                case EnemyStateType.Motion:
                    return typeof(QuadrupedMotionState);
                case EnemyStateType.Hurt:
                    return typeof(QuadrupedHurtState);
                case EnemyStateType.Attack:
                    return typeof(QuadrupedAttackState);
                case EnemyStateType.Dead:
                    return typeof(QuadrupedDeadState);
                case EnemyStateType.Shout:
                    return typeof(EnemyShoutState);
                case EnemyStateType.Fight:
                    return typeof(QuadrupedFightState);
                case EnemyStateType.OutOfFight:
                    return typeof(QuadrupedOfTFState);
                case EnemyStateType.Vertigo:
                    return typeof(EnemyVertigoState);
                default:
                    return typeof(QuadrupedIdleState);
            }
        }

        protected override void StartFsm()
        {
            fsm.Start<QuadrupedIdleState>();
        }

    }
}
