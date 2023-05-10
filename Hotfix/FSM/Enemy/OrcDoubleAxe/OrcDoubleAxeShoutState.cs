using UnityEngine;
using GameFramework.Fsm;
using ProcedureOwner = GameFramework.Fsm.IFsm<Farm.Hotfix.EnemyLogic>;
using GameFramework;
using UnityGameFramework.Runtime;

namespace Farm.Hotfix
{
    public class OrcDoubleAxeShoutState : EnemyShoutState
    {
        private readonly static int m_Shout = Animator.StringToHash("Shout");
        private readonly static int m_ShoutEnd = Animator.StringToHash("ShoutEnd");
        private static readonly int FrenzyEnd = Animator.StringToHash("FrenzyEnd");
        private static readonly int Frenzy = Animator.StringToHash("Frenzy");
        private bool isFrenzyEnd;
        AnimatorStateInfo info;
        private EnemyLogic owner;
        protected override void EnterShoutState(ProcedureOwner procedureOwner)
        {
            owner = procedureOwner.Owner;


            owner.m_Animator.SetTrigger(m_Shout);
            owner.m_Animator.SetBool(m_ShoutEnd, false);


        }

        protected override void UpdateShoutState(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {

            info = owner.m_Animator.GetCurrentAnimatorStateInfo(0);
            if (info.normalizedTime >= 0.9f)
            {
                //owner.AnimationEnd();
                //Debug.Log("吼叫中" + owner.IsAnimPlayed);
                //owner.AnimationEnd();
                if (info.IsName("buff_01"))
                {
                    //
                    //Debug.Log("吼叫进行中" + owner.IsAnimPlayed);
                    owner.AnimationEnd();
                    owner.IsAnimPlayed = true;
                    //owner.m_Animator.SetBool(m_ShoutEnd, true);
                }

                ChangeState(procedureOwner, owner.ChangeStateEnemy(EnemyStateType.Motion));
            }

        }
        public static OrcDoubleAxeShoutState Create()
        {
            OrcDoubleAxeShoutState state = ReferencePool.Acquire<OrcDoubleAxeShoutState>();
            return state;
        }


        protected override void LeaveShoutState()
        {
            owner.m_Animator.SetBool(m_ShoutEnd, true);
            owner.IsAnimPlayed = false;
        }
    }
}
