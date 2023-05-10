using UnityEngine;
using GameFramework.Fsm;
using ProcedureOwner = GameFramework.Fsm.IFsm<Farm.Hotfix.EnemyLogic>;
using GameFramework;
using UnityGameFramework.Runtime;


namespace Farm.Hotfix
{
    public class EnemyHurtState : EnemyBaseActionState
    {
        private readonly string Layer = "Base Layer";
        protected static readonly int Hurt = Animator.StringToHash("Hurt");
        protected static readonly int HurtThump = Animator.StringToHash("HurtThump");
        protected static readonly int HurtSky = Animator.StringToHash("HurtInSky");
        private static readonly int HurtDirX = Animator.StringToHash("HurtDirX");
        private static readonly int HurtDirY = Animator.StringToHash("HurtDirY");
        private static readonly int ShoulderStrike = Animator.StringToHash("ShoulderStrike");
        private static readonly int ShieldStrike = Animator.StringToHash("ShieldStrike"); 
        private static readonly int RightfootStrike = Animator.StringToHash("RightfootStrike");
        private static readonly int LeftfootStrike = Animator.StringToHash("LeftfootStrike");
        private static readonly int m_GetRebound = Animator.StringToHash("GetRebound");
        private static readonly int ReboundEquiState = Animator.StringToHash("ReboundEquiState");
        private static readonly int ReboundState = Animator.StringToHash("ReboundState");
        private static readonly int ReboundRadomState = Animator.StringToHash("ReboundRadomState");
        private static readonly int GPAttackState = Animator.StringToHash("GPAttackState");
        private static readonly int GetGPAttack = Animator.StringToHash("GetGPAttack");

        protected EnemyLogic owner;

        protected override void OnEnter(ProcedureOwner fsm)
        {
            base.OnEnter(fsm);
            owner = fsm.Owner;
            owner.Buff.BuffTypeEnum = BuffType.None;

            if (owner.GetRebound)
            {
                
                owner.m_Animator.SetTrigger(m_GetRebound);
                owner.m_Animator.SetInteger(ReboundEquiState, (int)owner.GetReboundEqui);
                owner.m_Animator.SetInteger(ReboundState, (int)owner.GetReboundState);
                owner.GetRebound = false;
                owner.GetReboundEqui = EquiState.None;
                owner.GetCollider = ColliderState.None;
                owner.GetReboundState = Hotfix.ReboundState.Ordinary;
            }
            else
            {
                switch (owner.GetCollider)
                {
                    case ColliderState.None:
                        EnemyHurtStateStart(fsm);
                        owner.m_Animator.SetTrigger(Hurt);
                        break;
                    case ColliderState.Shoulder:
                        EnemyHurtStateStart(fsm);
                        owner.m_Animator.SetTrigger(ShoulderStrike);
                        break;
                    case ColliderState.Shield:
                        EnemyHurtStateStart(fsm);
                        owner.m_Animator.SetTrigger(ShieldStrike);
                        break;
                    case ColliderState.Rightfoot:
                        EnemyHurtStateStart(fsm);
                        owner.m_Animator.SetTrigger(RightfootStrike);
                        break;
                    case ColliderState.Leftfoot:
                        EnemyHurtStateStart(fsm);
                        owner.m_Animator.SetTrigger(LeftfootStrike);
                        break;
                    default:
                        EnemyHurtStateStart(fsm);
                        owner.m_Animator.SetTrigger(Hurt);
                        break;
                }
                owner.GetCollider = ColliderState.None;
            }

            if (owner.GetGP != GPAttack.None)
            {
                owner.m_Animator.SetInteger(GPAttackState, (int)owner.GetGP);
                owner.m_Animator.SetTrigger(GetGPAttack);
                owner.GetGP = GPAttack.None;
            }
            if (owner.Stoic)
            {
                owner.HurtEnd = true;
            }
            else
            {
                owner.HurtEnd = false;
            }
            
            owner.m_Animator.SetFloat(HurtDirX, owner.attackDir.x);
            owner.m_Animator.SetFloat(HurtDirY, owner.attackDir.y);
            if (owner.enemyData.HP <= 0)
            {
                ChangeState(fsm, owner.ChangeStateEnemy(EnemyStateType.Dead));
    			return;            
            }

            //Debug.Log("进入敌人受伤状态");            
           
        }

        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);

            //Debug.Log(owner.IsAnimPlayed);

            if (owner.enemyData.HP <= 0)
            {
                ChangeState(procedureOwner, owner.ChangeStateEnemy(EnemyStateType.Dead));
    			return;            }

            if (owner.HurtEnd)
            {
                EnemyHurtStateEnd(procedureOwner);
               // ChangeState<PlayerMotionState>(procedureOwner);
            }
            //动画播放完毕
            //if (owner.IsAnimPlayed)
            //{
            //}

        }

        protected override void OnLeave(ProcedureOwner fsm, bool isShutdown)
        {
            base.OnLeave(fsm, isShutdown);
            owner.IsAnimPlayed = false;
            owner.m_IsInKnockedDown = false;
            owner.EnemyAttackEnd();
            //Debug.Log("离开敌人受伤状态");
        }

        public static EnemyHurtState Create()
        {
            EnemyHurtState state = ReferencePool.Acquire<EnemyHurtState>();
            return state;
        }


        /// <summary>
        /// 受伤状态开始
        /// </summary>
        protected virtual void EnemyHurtStateStart(ProcedureOwner fsm)
        {
            owner.SetRichAiStop();
            owner.EnemyAttackEnd();
        }

        /// <summary>
        /// 受伤状态结束
        /// </summary>
        /// <param name="procedureOwner"></param>
        protected virtual void EnemyHurtStateEnd(ProcedureOwner procedureOwner)
        {
            ChangeState(procedureOwner, owner.ChangeStateEnemy(EnemyStateType.Fight));
        }
    }
}