using UnityEngine;
using GameFramework.Fsm;
using ProcedureOwner = GameFramework.Fsm.IFsm<Farm.Hotfix.EnemyLogic>;
using GameFramework;
using UnityGameFramework.Runtime;

namespace Farm.Hotfix
{
    /// <summary>
    /// 敌人眩晕状态
    /// </summary>
   public class EnemyVertigoState : EnemyBaseActionState
    {
        protected EnemyLogic owner;
        private readonly string Layer = "Base Layer";
        private static readonly int m_IsStun = Animator.StringToHash("isStun");
        private static readonly int BigStun = Animator.StringToHash("BigStun");
        private float m_VertigoTime;
        private float m_MaxVertigoTime = 3f;
        protected override void OnInit(ProcedureOwner procedureOwner)
        {
            base.OnInit(procedureOwner);
        }

        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);
            owner = procedureOwner.Owner;
            EnemyVertigoStateStart();
            owner.m_VertigoSum += 1;
            if(owner.m_VertigoSum > 2)
            {
                owner.m_Animator.SetTrigger(BigStun);
                owner.m_VertigoSum = 0;
            }
            else
            {
                owner.m_Animator.SetTrigger(m_IsStun);
            }
            Log.Info("进入眩晕");
        }

        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);
            
            if (owner.m_Animator.GetCurrentAnimatorStateInfo(owner.m_Animator.GetLayerIndex(Layer)).normalizedTime >= 0.8f)
            {
                owner.TargetableObjectData.VertigoValue = owner.TargetableObjectData.MaxVertigo;
                EnemyVertigoStateEnd(procedureOwner);
                return;
            }
            m_VertigoTime += elapseSeconds;
            if(m_VertigoTime >= m_MaxVertigoTime)
            {
                EnemyVertigoStateEnd(procedureOwner);
            }
        }

        protected override void OnLeave(ProcedureOwner procedureOwner, bool isShutdown)
        {
            base.OnLeave(procedureOwner, isShutdown);
            m_VertigoTime = 0;

        }

        public static EnemyVertigoState Create()
        {
            EnemyVertigoState state = ReferencePool.Acquire<EnemyVertigoState>();
            return state;
        }


        /// <summary>
        /// 眩晕状态开始
        /// </summary>
        private void EnemyVertigoStateStart()
        {
            owner.SetRichAiStop();
            owner.EnemyAttackEnd();
        }

        /// <summary>
        /// 眩晕状态结束
        /// </summary>
        /// <param name="procedureOwner"></param>
        private void EnemyVertigoStateEnd(ProcedureOwner procedureOwner)
        {
            ChangeState(procedureOwner, owner.ChangeStateEnemy(EnemyStateType.Fight));
        }
    }
}
