//------------------------------------------------------------
// Creator: FatCat
// Creation date: 2023/4/4/周二 13:40:17
//------------------------------------------------------------


using GameFramework.Fsm;
using UnityEngine;
using ProcedureOwner = GameFramework.Fsm.IFsm<Farm.Hotfix.EnemyLogic>;
using GameFramework;
using UnityGameFramework.Runtime;

namespace Farm.Hotfix
{
    public class QuadrupedAttackState :EnemyAttackState
    {
        private readonly float FRIST_PHASE = 0.5f; //第一阶段
        private readonly float SECOND_PHASE = 0.25f;//第二阶段
        private readonly static int m_InFight = Animator.StringToHash("InFight");
        private static readonly int Frenzy = Animator.StringToHash("Frenzy");
        public static new  QuadrupedAttackState Create()
        {
            QuadrupedAttackState state = ReferencePool.Acquire<QuadrupedAttackState>();
            return state;
        }
        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);
            owner.AnimationStart();
            EnemyAttackStateStart(owner);
        }

        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);
            if (owner.IsAnimPlayed)
            {
                ChangeState(procedureOwner, owner.ChangeStateEnemy(EnemyStateType.Fight));
            }
            if (owner.m_IsAttackRotate)
            {
                AIUtility.RotateToTarget(owner.find_Player, owner, -30, 30,1,300);
            }
        }

        protected override void OnLeave(ProcedureOwner fsm, bool isShutdown)
        {
            base.OnLeave(fsm, isShutdown);
        }

        protected override void EnemyAttackStateStart(EnemyLogic owner)
        {
            base.EnemyAttackStateStart(owner);
            owner.SetRichAiStop();
            StopBlend(owner);
            owner.m_Animator.SetBool(m_InFight, true);

            if (owner.enemyData.HPRatio > FRIST_PHASE)
            {
                if (owner.enemyData.MoraleValue > 80)
                {
                    if (!owner.m_IsFrenzy)
                    {

                        owner.m_Animator.SetTrigger(Frenzy);
                        owner.m_IsFrenzy = true;
                        owner.enemyData.MoraleValue -= 80;
                    }
                    int randomNum = Utility.Random.GetRandom(4, 8);
                    owner.m_Animator.SetInteger(AttackState, randomNum);
                }
                else
                {
                    if (owner.m_IsFrenzy)
                    {
                        int randomNum = Utility.Random.GetRandom(4, 8);
                        owner.m_Animator.SetInteger(AttackState, randomNum);
                    }
                    else
                    {
                        int randomNum = Utility.Random.GetRandom(0, 4);
                        owner.m_Animator.SetInteger(AttackState, randomNum);
                    }
                }
            }
            else if (SECOND_PHASE < owner.enemyData.HPRatio && owner.enemyData.HPRatio < FRIST_PHASE)
            {

                if (owner.enemyData.MoraleValue > 80) 
                {
                    if (!owner.m_IsFrenzy)
                    {

                        owner.m_Animator.SetTrigger(Frenzy);
                        owner.m_IsFrenzy = true;
                        owner.enemyData.MoraleValue -= 80;
                    }
                        int randomNum = Utility.Random.GetRandom(4, 8);
                        owner.m_Animator.SetInteger(AttackState, randomNum);

                }
                else
                {
                    if (owner.m_IsFrenzy)
                    {
                        int randomNum = Utility.Random.GetRandom(4, 8);
                        owner.m_Animator.SetInteger(AttackState, randomNum);
                    }
                    else
                    {
                        int randomNum = Utility.Random.GetRandom(8, 14);
                        owner.m_Animator.SetInteger(AttackState, randomNum);
                    }

                }
            }
            else
            {
                owner.m_Animator.SetInteger(AttackState, 14);
            }
        }
    }
}
