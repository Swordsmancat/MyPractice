//------------------------------------------------------------
// Creator: FatCat
// Creation date: 2023/4/6/周四 15:59:41
//------------------------------------------------------------

using UnityEngine;
using GameFramework;
using ProcedureOwner = GameFramework.Fsm.IFsm<Farm.Hotfix.EnemyLogic>;
using UnityGameFramework.Runtime;

namespace Farm.Hotfix
{
    public class QuadrupedFightState :EnemyFightState
    {
        private float m_PlayerDistance = 0f;
        private float m_StateWaitTime = 0f;

        private readonly float NEXTSTATEWAITTIME = 1f;  //等待时间
        private readonly static int m_TurnLeft = Animator.StringToHash("TurnLeft");
        private readonly static int m_TurnRight = Animator.StringToHash("TurnRight");
        private readonly static int m_TurnBackLeft = Animator.StringToHash("TurnBackLeft");
        private readonly static int m_TurnBackRight = Animator.StringToHash("TurnBackRight");

        private bool m_IsTurn = false;

        public static new QuadrupedFightState Create()
        {
            QuadrupedFightState state = new QuadrupedFightState();
            return state;
        }

        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);
            owner.HideTrail();
            owner.EnemyAttackEnd();
            m_IsTurn = false;
            if (AIUtility.GetCross(owner, owner.LockingEntity) > 0)
            {
                if (AIUtility.GetAngleInSeek(owner, owner.LockingEntity) > 135f)
                {
                    owner.m_Animator.SetTrigger(m_TurnBackRight);
                    m_IsTurn = true;
                }
                else if(AIUtility.GetAngleInSeek(owner, owner.LockingEntity) > 45f)
                {
                    owner.m_Animator.SetTrigger(m_TurnRight);
                    m_IsTurn = true;
                }
            }
            else
            {
                if (AIUtility.GetAngleInSeek(owner, owner.LockingEntity) > 135f)
                {
                    owner.m_Animator.SetTrigger(m_TurnBackLeft);
                    m_IsTurn = true;
                }
                else if (AIUtility.GetAngleInSeek(owner, owner.LockingEntity) > 45f)
                {
                    owner.m_Animator.SetTrigger(m_TurnLeft);
                    m_IsTurn = true;
                }
            }
          //  AIUtility.RotateToTarget(owner.LockingEntity, owner, -5f, 5f, 1f, 100f);
        }



        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);
            if (!m_IsTurn)
            {
                AIUtility.RotateToTarget(owner.LockingEntity, owner, -5f, 5f, 5f, 200f);
            }
            if (owner.m_AttackEntity != null)
            {
                m_PlayerDistance = AIUtility.GetDistance(owner, owner.m_AttackEntity);
            }
            else
            {
                if (owner.IsLocking)
                {
                    m_PlayerDistance = AIUtility.GetDistance(owner, owner.LockingEntity);
                }
                else
                {
                    ChangeState(procedureOwner, owner.ChangeStateEnemy(EnemyStateType.Motion));
                    return;
                }
            }
        
            if (!owner.CheckInAttackRange(m_PlayerDistance))
            {
                m_StateWaitTime += elapseSeconds;
                if (m_StateWaitTime >= NEXTSTATEWAITTIME)
                {
                    m_StateWaitTime = 0;
                    ChangeState(procedureOwner, owner.ChangeStateEnemy(EnemyStateType.Motion));
                    return;
                }
            }
            else
            {
                if (owner.IsCanAttack)
                {
                    ChangeState(procedureOwner, owner.ChangeStateEnemy(EnemyStateType.Attack));
                    return;
                }
            }
           
        }

        protected override void OnLeave(ProcedureOwner fsm, bool isShutdown)
        {
            base.OnLeave(fsm, isShutdown);
        }
    }
}
