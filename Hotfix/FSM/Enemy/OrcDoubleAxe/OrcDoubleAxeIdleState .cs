using UnityEngine;
using System;
using GameFramework.Fsm;
using ProcedureOwner = GameFramework.Fsm.IFsm<Farm.Hotfix.EnemyLogic>;
using GameFramework;
using UnityGameFramework.Runtime;

namespace Farm.Hotfix
{
    
    public class OrcDoubleAxeIdleState : EnemyIdleState
    {
        //protected EnemyLogic owner;
        private float idletime;
        //private readonly static int m_FightBlend = Animator.StringToHash("FightBlend");
        private readonly static int m_MoveBlend = Animator.StringToHash("MoveBlend");
        private readonly static int isIdle = Animator.StringToHash("isIdle");
        protected override void OnInit(ProcedureOwner procedureOwner)
        {
            base.OnInit(procedureOwner);
        }

        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);
            owner.m_Animator.SetFloat("MoveBlend", 0f);
            owner.m_Animator.SetTrigger(isIdle);
        }

        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);
            owner.RestoreEnergy();
            idletime += Time.deltaTime;
            if(idletime > 20f)
            OrcDoubleAxeIdleStateStart(owner);
        }

        protected override void OnLeave(ProcedureOwner fsm, bool isShutdown)
        {
            base.OnLeave(fsm, isShutdown);
        }
        public static new OrcDoubleAxeIdleState Create()
        {
            OrcDoubleAxeIdleState state = ReferencePool.Acquire<OrcDoubleAxeIdleState>();
            return state;
        }
        protected override void LockPlayerDo(ProcedureOwner procedureOwner)
        {
            owner.LockEntity(owner.find_Player);
            owner.m_Animator.SetTrigger("Shout");
            ChangeState(procedureOwner, owner.ChangeStateEnemy(EnemyStateType.Shout));
            //owner.m_Animator.SetFloat(m_MoveBlend, 0f);
            //owner.m_Animator.SetFloat(m_FightBlend, 0.5f);


        }
        protected  void OrcDoubleAxeIdleStateStart(EnemyLogic owner)
        {
            int num = Utility.Random.GetRandom(15, 30);
            
            if (idletime > num)
            {
                owner.m_Animator.SetTrigger("Idle1");
                idletime = 0;

            }
            else
            {
                owner.m_Animator.SetTrigger("Idle2");
                idletime = 0;
            }
                    
                

            if (owner.find_Player.IsDead)
            {
                owner.m_Animator.SetInteger("Idle", 3);
            }
        }
        
    }
}
