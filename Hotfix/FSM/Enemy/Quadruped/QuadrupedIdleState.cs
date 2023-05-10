//------------------------------------------------------------
// Creator: FatCat
// Creation date: 2023/4/4/周二 13:57:27
//------------------------------------------------------------

using UnityEngine;
using ProcedureOwner =GameFramework.Fsm.IFsm<Farm.Hotfix.EnemyLogic>;
using GameFramework;
using GameFramework.Fsm;
using UnityGameFramework.Runtime;

namespace Farm.Hotfix
{
    public class QuadrupedIdleState :EnemyIdleState
    {
        public static new  QuadrupedIdleState Create()
        {
            QuadrupedIdleState state = ReferencePool.Acquire<QuadrupedIdleState>();
             return state;
        }

        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);
        }

        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);
        }

        protected override void OnLeave(ProcedureOwner fsm, bool isShutdown)
        {
            base.OnLeave(fsm, isShutdown);
        }
    }
}
