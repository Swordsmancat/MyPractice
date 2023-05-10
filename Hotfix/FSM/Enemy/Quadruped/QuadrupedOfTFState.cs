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
    public class QuadrupedOfTFState : EnemyOutOfTheFight
    {
        public static new QuadrupedOfTFState Create()
        {
            QuadrupedOfTFState state = ReferencePool.Acquire<QuadrupedOfTFState>();
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
