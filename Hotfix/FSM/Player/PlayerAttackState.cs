using UnityEngine;
using GameFramework.Fsm;
using ProcedureOwner = GameFramework.Fsm.IFsm<Farm.Hotfix.PlayerLogic>;
using GameFramework;
using UnityGameFramework.Runtime;

namespace Farm.Hotfix
{
    public class PlayerAttackState : PlayerBaseActionState
    {
        private PlayerLogic owner;
        private float dampTimeX = 0.1f;

        private static readonly int AttackTrigger = Animator.StringToHash("AttackTrigger");
        private static readonly int Dodge = Animator.StringToHash("Dodge");
        private static readonly int AttackTap = Animator.StringToHash("AttackTap");
        private static readonly int AttackJump = Animator.StringToHash("AttackJump");
        private static readonly int AttackThump = Animator.StringToHash("AttackThump");
        private static readonly int MoveBlend = Animator.StringToHash("MoveBlend");
        private static readonly int m_HashStateTime = Animator.StringToHash("StateTime");
        private static readonly int m_ClickAttackBtnDuration = Animator.StringToHash("ClickAttackBtnDuration");
        private static readonly int StepAtk = Animator.StringToHash("StepAtk");             //是否滑步 若为ture就执行特殊的滑步攻击 目前双剑有滑步
        //private static readonly int IsFourthAtk = Animator.StringToHash("IsFourthAtk");     
        private static readonly int ThumpNum = Animator.StringToHash("ThumpNum");
        private static readonly int DoubleClick = Animator.StringToHash("DoubleClick");
        private static readonly int Whirlwind = Animator.StringToHash("Whirlwind");



        protected override void OnInit(ProcedureOwner procedureOwner)
        {
            base.OnInit(procedureOwner);
        }

        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);
            owner = procedureOwner.Owner;
            owner.m_Animator.SetFloat(m_ClickAttackBtnDuration, owner.m_ClickAttackBtnDuration);
            owner.m_Animator.SetFloat(m_HashStateTime, 0);
            if (owner.m_Animator.GetFloat(MoveBlend) > 1.8f)
            {
               //奔跑中抽刀并攻击时增加位移
                owner.PlayerShiftStartAnim(0.6f);
            }

              owner.m_moveBehaviour.isAttack = true;
            owner.m_MPStartRestore = false;
        }

        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);
            owner.m_Animator.SetFloat(MoveBlend, Mathf.Max(Mathf.Abs(owner.MoveX), Mathf.Abs(owner.MoveY)), dampTimeX, Time.deltaTime);
           

            if (owner.currentLockEnemy)
            {
                owner.m_moveBehaviour.AttackRotating();
            }

            //角色滑步后实现特殊的攻击 目前只有双剑有滑步
            if (owner.isCanStepAtk)
            {
                owner.m_Animator.SetBool(StepAtk, true);
            }
            else
            {
                owner.m_Animator.SetBool(StepAtk, false);
                
            }
            owner.m_Animator.SetFloat(m_HashStateTime, Mathf.Repeat(owner.m_Animator.GetCurrentAnimatorStateInfo(owner.m_Animator.GetLayerIndex("Base Layer")).normalizedTime, 1f));
            owner.m_Animator.ResetTrigger(AttackTrigger);
            owner.m_Animator.ResetTrigger(AttackTap);
            owner.m_Animator.ResetTrigger(AttackThump);
            owner.m_Animator.ResetTrigger(AttackJump);
            owner.m_Animator.ResetTrigger(DoubleClick);
            if (owner.m_Attack)
            {
                owner.m_Animator.SetTrigger(AttackTrigger);
                //攻击转向
               
                owner.m_moveBehaviour.AttackRotating();
                // Player is moving on ground, Y component of camera facing is not relevant.
                if (owner.m_DoubleClick)
                {
                    owner.m_Animator.SetTrigger(DoubleClick);

                }
                else if (owner.m_IsAttackTap)
                {
                   
                        owner.m_Animator.SetTrigger(AttackTap);
                }
                else if (owner.m_IsAttackThump)
                {
                    owner.m_Animator.SetTrigger(AttackThump);
                }
                else if (owner.m_IsAttackJump)
                {
                   
                    
                        owner.m_Animator.SetTrigger(AttackTap);
                    
                    
                }
                
                //   owner.gameObject.transform.rotation = Quaternion.Euler(0,
                //      owner.CachedTransform.localRotation.eulerAngles.y + owner.MoveX, 0);
            }
            else
            {
                if (owner.m_Animator.GetCurrentAnimatorStateInfo(owner.m_Animator.GetLayerIndex("Base Layer")).normalizedTime > 0 && owner.m_Animator.GetAnimatorTransitionInfo(owner.m_Animator.GetLayerIndex("Base Layer")).nameHash !=0)
                {
                    ChangeState<PlayerMotionState>(procedureOwner);
                }
                if(owner.EquiState == EquiState.Bow)
                {
                    if (owner.m_Animator.GetCurrentAnimatorStateInfo(owner.m_Animator.GetLayerIndex("Player")).normalizedTime > 0 && owner.m_Animator.GetAnimatorTransitionInfo(owner.m_Animator.GetLayerIndex("Player")).nameHash != 0)
                    {
                        ChangeState<PlayerMotionState>(procedureOwner);
                    }
                }
            }
            if (InputManager.IsClickDodge())
            {
                if (owner.m_Animator.GetCurrentAnimatorStateInfo(owner.m_Animator.GetLayerIndex("Base Layer")).normalizedTime > 0.4f)
                    ChangeState<PlayerDodgeState>(procedureOwner);
                return;
            }

        }

        protected override void OnLeave(ProcedureOwner fsm, bool isShutdown)
        {
            base.OnLeave(fsm, isShutdown);
            owner.m_Attack = false;
            owner.m_IsAttackTap = false;
            owner.m_IsAttackThump = false;
            owner.m_IsAttackJump = false;
            owner.Whirlwind = false;
            owner.m_ClickAttackBtnDuration = 0;
            owner.m_moveBehaviour.isAttack = false;
            owner.m_Animator.ResetTrigger(AttackTrigger);
            owner.m_Animator.ResetTrigger(AttackTap);
            owner.m_Animator.ResetTrigger(AttackThump);
            owner.m_Animator.ResetTrigger(AttackJump);
            owner.m_Animator.ResetTrigger(DoubleClick);
            owner.m_MPStartRestore = true;
            //owner.m_Animator.ResetTrigger(Dodge);
            //owner.isFourthAtk = false;
            //owner.m_Animator.SetBool(IsFourthAtk, owner.isFourthAtk);
            //owner.m_Animator.SetBool(IsSecondSkill, false);
        }

        public static PlayerAttackState Create()
        {
            PlayerAttackState state = ReferencePool.Acquire<PlayerAttackState>();
            return state;
        }

    }
}
