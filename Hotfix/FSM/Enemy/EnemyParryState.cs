using GameFramework.Fsm;
using System.Collections;
using System.Collections.Generic;
using GameFramework;
using UnityEngine;

namespace Farm.Hotfix
{
    public class EnemyParryState : EnemyBaseActionState
    {
        private readonly static int m_Inparry = Animator.StringToHash("InParry");
        private readonly static int m_TapParry = Animator.StringToHash("TapParry");
        private readonly static int m_ThumpParry = Animator.StringToHash("ThumpParry");
        private readonly static int m_OverParry = Animator.StringToHash("OverParry");
        private readonly static int m_SkillParry = Animator.StringToHash("SkillParry");
        private readonly static int m_Counter = Animator.StringToHash("Counter");
        private readonly static int m_Hurt = Animator.StringToHash("Hurt");
        private readonly static int m_ParryOut = Animator.StringToHash("ParryOut");
        //private readonly static float ExitTime = 0.3f;
        private EnemyLogic owner;
        //private float exitTimer = 0;
        private float parryTime;

        private int hurtLoss;
        private int hurtNum;

        protected override void OnInit(IFsm<EnemyLogic> procedureOwner)
        {
            base.OnInit(procedureOwner);
        }

        protected override void OnEnter(IFsm<EnemyLogic> procedureOwner)
        {
            base.OnEnter(procedureOwner);
            owner = procedureOwner.Owner;
            EnemyParryStateStart(owner);
            owner.m_Animator.SetBool(m_ParryOut, false);
            hurtLoss = Utility.Random.GetRandom(3, 6);
            hurtNum = 0;
            owner.underAttack = false;
            int random = Utility.Random.GetRandom(5, 8);
            owner.TargetableObjectData.TrunkValue -= random;
            GameHotfixEntry.HPBar.ShowTrunkValue(owner, owner.TargetableObjectData.TrunkRatio);
            Debug.Log("½øÈë¸ñµ²");
        }
        public static EnemyParryState Create()
        {
            EnemyParryState state = ReferencePool.Acquire<EnemyParryState>();
            return state;
        }

        protected override void OnUpdate(IFsm<EnemyLogic> procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);

            parryTime += elapseSeconds;
            if (owner.underAttack)
            {
                ParryHurt();
            }
            else
            {
                if (parryTime >= 2f)
                {
                    owner.m_Animator.SetBool(m_ParryOut, true);
                    owner.IsDefense = false;
                    ChangeState(procedureOwner, owner.ChangeStateEnemy(EnemyStateType.Motion));
                    return;

                }
                else
                {
                    if (hurtNum > hurtLoss)
                    {
                        owner.m_Animator.SetTrigger(m_Counter);
                        owner.m_Animator.SetBool(m_ParryOut, true);
                        owner.IsDefense = false;
                        ChangeState(procedureOwner, owner.ChangeStateEnemy(EnemyStateType.Fight));
                        return;
                    }
                }
            }

    

        }

        protected override void OnLeave(IFsm<EnemyLogic> fsm, bool isShutdown)
        {
            base.OnLeave(fsm, isShutdown);
            owner.IsParry = false;
            parryTime = 0;
            hurtNum = 0;
            owner.underAttack = false;
            owner.IsDefense = false;
            owner.m_Animator.ResetTrigger(m_TapParry);
            owner.m_Animator.ResetTrigger(m_ThumpParry);
            owner.m_Animator.ResetTrigger(m_OverParry);
            owner.m_Animator.ResetTrigger(m_SkillParry);
            owner.m_Animator.ResetTrigger(m_Inparry);
            Debug.Log("ÍË³ö¸ñµ²");
            //exitTimer = 0;
        }

        /// <summary>
        /// ÕÐ¼Ü×´Ì¬¿ªÊ¼
        /// </summary>
        protected virtual void EnemyParryStateStart(EnemyLogic owner)
        {
            switch (owner.GetBuffType)
            {
                case BuffType.None:
                    owner.m_Animator.SetTrigger(m_TapParry);
                    break;
                case BuffType.Tap:
                    owner.m_Animator.SetTrigger(m_TapParry);
                    break;
                case BuffType.Thump:
                    owner.m_Animator.SetTrigger(m_ThumpParry);
                    break;
                case BuffType.Overwhelmed:
                    owner.m_Animator.SetTrigger(m_OverParry);
                    break;
                case BuffType.SkillAttack:
                    owner.m_Animator.SetTrigger(m_SkillParry);
                    break;
                case BuffType.KatanaSPAttack:
                    owner.m_Animator.SetTrigger(m_SkillParry);
                    break;
                case BuffType.GreatSwordUpAttack:
                    owner.m_Animator.SetTrigger(m_SkillParry);
                    break;
                case BuffType.ForwardAttack:
                    owner.m_Animator.SetTrigger(m_SkillParry);
                    break;
                case BuffType.ShieldAttack:
                    owner.m_Animator.SetTrigger(m_SkillParry);
                    break;
                case BuffType.TwoDaggersAttack:
                    owner.m_Animator.SetTrigger(m_SkillParry);
                    break;
                case BuffType.KatanaAttack:
                    owner.m_Animator.SetTrigger(m_SkillParry);
                    break;
                case BuffType.StunAttack:
                    owner.m_Animator.SetTrigger(m_OverParry);
                    break;
                case BuffType.LeftAttack:
                    owner.m_Animator.SetTrigger(m_SkillParry);
                    break;
                case BuffType.RightAttack:
                    owner.m_Animator.SetTrigger(m_SkillParry);
                    break;
                case BuffType.Punchhard:
                    owner.m_Animator.SetTrigger(m_SkillParry);
                    break;
                case BuffType.Punch:
                    owner.m_Animator.SetTrigger(m_TapParry);
                    break;
                default:
                    owner.m_Animator.SetTrigger(m_SkillParry);
                    break;
            }
            owner.m_Animator.SetTrigger(m_Inparry);
            owner.EnemyAttackEnd();
        }

        /// <summary>
        /// ÕÐ¼Ü×´Ì¬½áÊø
        /// </summary>
        /// <param name="procedureOwner"></param>
        protected virtual void EnemyParryStateEnd(IFsm<EnemyLogic> procedureOwner)
        {

        }
        /// <summary>
        /// ÕÐ¼Ü·´»÷ÍË³ö
        /// </summary>
        /// <param name="procedureOwner"></param>
        protected virtual void EnemyParryStateCounter(IFsm<EnemyLogic> procedureOwner)
        {

        }

        /// <summary>
        /// ÕÐ¼ÜÆÆ·ÀÍË³ö
        /// </summary>
        /// <param name="procedureOwner"></param>
        protected virtual void EnemyParryOutStateEnd(IFsm<EnemyLogic> procedureOwner)
        {
            ChangeState(procedureOwner, owner.ChangeStateEnemy(EnemyStateType.Motion));
        }

        private void ParryHurt()
        {
            //hurtNum += 1; 
            switch (owner.GetBuffType)
            {
                case BuffType.None:
                    owner.m_Animator.SetTrigger(m_TapParry);
                    break;
                case BuffType.Tap:
                    owner.m_Animator.SetTrigger(m_TapParry);
                    break;
                case BuffType.Thump:
                    owner.m_Animator.SetTrigger(m_ThumpParry);
                    break;
                case BuffType.Overwhelmed:
                    owner.m_Animator.SetTrigger(m_OverParry);
                    break;
                case BuffType.SkillAttack:
                    owner.m_Animator.SetTrigger(m_SkillParry);
                    break;
                default:
                    owner.m_Animator.SetTrigger(m_TapParry);
                    break;
            }
            hurtNum += 1;
            owner.m_Animator.SetTrigger(m_Hurt);
            parryTime = 0;
            owner.underAttack = false;
            int random = Utility.Random.GetRandom(5, 8);
            owner.TargetableObjectData.TrunkValue -= random;
            GameHotfixEntry.HPBar.ShowTrunkValue(owner, owner.TargetableObjectData.TrunkRatio);
            //toParry = false;

        }



    }
}

