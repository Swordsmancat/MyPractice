//------------------------------------------------------------
// Creator: FatCat
// Creation date: 2023/4/4/周二 18:09:44
//------------------------------------------------------------

using UnityEngine;
using GameFramework;
using ProcedureOwner = GameFramework.Fsm.IFsm<Farm.Hotfix.EnemyLogic>;
using UnityGameFramework.Runtime;

namespace Farm.Hotfix
{
    public class QuadrupedMotionState:EnemyMotionState
    {
        private int m_FightStateRange = 20;
        private bool m_IsInFight = false;
        public static new QuadrupedMotionState Create()
        {
            QuadrupedMotionState state = ReferencePool.Acquire<QuadrupedMotionState>();
            return state;
        }

        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);
        }

        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);
            float distance = 0f;
            if (owner.m_AttackEntity != null)
            {
                distance = AIUtility.GetDistance(owner, owner.m_AttackEntity);
            }
            else
            {
                if (owner.IsLocking)
                {
                    distance = AIUtility.GetDistance(owner, owner.LockingEntity);
                }

            }
            if (owner.IsLocking)
            {
                if (distance <= owner.enemyData.AttackRange + m_FightStateRange)
                {
                    m_IsInFight = true;
                }
                else
                {
                    m_IsInFight = false;
                }
                if (!owner.CheckInAttackRange(distance))
                {
                    SetMovement(distance);
                    owner.SetSearchTarget(owner.LockingEntity.CachedTransform);
                    owner.SetRichAIMove(owner.m_Animator.GetFloat(m_MoveBlend));
                }
                else
                {
                    owner.m_Animator.SetFloat(m_MoveBlend, 0f,0.5f,Time.deltaTime);
                    owner.m_Animator.SetFloat(m_FightBlend, 0.5f,0.5f,2*Time.deltaTime);
                    owner.SetRichAiStop();
                    ChangeState(procedureOwner, owner.ChangeStateEnemy(EnemyStateType.Fight));
                    return;
                }
            }
            else
            {
                owner.m_Animator.SetFloat(m_MoveBlend, 0.5f, 0.25f, Time.deltaTime);
                owner.SetRichAIMove();
                owner.SetSearchTargetPosition(owner.m_NextPatrol);
            }
        }

        protected override void OnLeave(ProcedureOwner fsm, bool isShutdown)
        {
            base.OnLeave(fsm, isShutdown);
        }

        private void IsBack(float disdance)
        {
            float MoveX = owner.find_Player.MoveX;
            bool left = MoveX > 0;
            if (disdance < 1.5)
            {
                owner.m_Animator.SetFloat(m_FightBlend, 0, 0.5f, Time.deltaTime);
                owner.m_Animator.SetFloat(m_MoveBlend, 0);
                owner.SetRichAiStop();
                owner.m_Animator.SetBool(m_IsBack, true);
            }

            else if (disdance > 2.5 && disdance < 5 && left)
            {
                owner.m_Animator.SetFloat(m_FightBlend, 1f, 0.5f, 2 * Time.deltaTime);
                owner.m_Animator.SetFloat(m_MoveBlend, 0.8f, 0.5f, Time.deltaTime);
                owner.m_Animator.SetBool(m_IsBack, false);

            }
            else if (disdance > 2.5 && disdance < 5 && !left)
            {
                owner.m_Animator.SetFloat(m_FightBlend, -1f, 0.5f, 2 * Time.deltaTime);
                owner.m_Animator.SetFloat(m_MoveBlend, 0.8f, 0.5f, Time.deltaTime);
                owner.m_Animator.SetBool(m_IsBack, false);
            }
            else if (disdance >= 5)
            {
                owner.m_Animator.SetFloat(m_FightBlend, 0, 0.5f, 2 * Time.deltaTime);
                owner.m_Animator.SetFloat(m_MoveBlend, 1f, 0.5f, Time.deltaTime);
                owner.m_Animator.SetBool(m_IsBack, false);
            }



        }

        private void SetMovement(float disdance)
        {
            if (m_IsInFight)
            {
                owner.m_Animator.SetBool(m_InFight, true);
                IsBack(disdance);
            }
            else
            {
                owner.m_Animator.SetBool(m_InFight, false);
                if (disdance >= m_FightStateRange)
                {
                    owner.m_Animator.SetFloat(m_MoveBlend, 1.5f, 0.5f, Time.deltaTime);
                }
                else
                {
                    owner.m_Animator.SetFloat(m_MoveBlend, 0.5f, 0.25f, Time.deltaTime);
                }
            }
        }
    }
}
