using UnityEngine;
using GameFramework.Fsm;
using ProcedureOwner = GameFramework.Fsm.IFsm<Farm.Hotfix.PlayerLogic>;
using GameFramework;
using UnityGameFramework.Runtime;

namespace Farm.Hotfix
{
    public class PlayerMotionState : PlayerBaseActionState
    {
        private PlayerLogic owner;

        private static readonly int MoveBlend = Animator.StringToHash("MoveBlend");
        private static readonly int Defense = Animator.StringToHash("Defense");
        private static readonly int LockMoveBlendX = Animator.StringToHash("LockMoveBlendX");
        private static readonly int LockMoveBlendY = Animator.StringToHash("LockMoveBlendY");
        private static readonly int IsLockEnemy = Animator.StringToHash("IsLockEnemy");
        private static readonly int TakeOutWeaponTrigger = Animator.StringToHash("TakeOutWeaponTrigger");

        private float dampTimeX = 0.1f;



        //private int? m_WalkSound;
        //private int? m_RunSound;
        //private bool isPlaySound;
        //private float m_TakeWeaponTime;
        protected override void OnInit(ProcedureOwner procedureOwner)
        {
            base.OnInit(procedureOwner);
        }

        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);
            owner = procedureOwner.Owner;
            LeftFoot = FindTools.FindFunc<Transform>(owner.transform, "LeftFoot");
            RightFoot = FindTools.FindFunc<Transform>(owner.transform, "RightFoot");
            //isPlaySound = false;
        }
        private int WalkLayerMask = 1 << 6;
        private Transform LeftFoot;
        private Transform RightFoot;
        Ray Ray;
        float soundTime;

        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);

           // owner.RestoreEnergy();

            if (owner.m_LockEnemy)
            {
                owner.m_Animator.SetBool(IsLockEnemy, true);
                owner.m_Animator.SetFloat(MoveBlend, 0);
                owner.m_Animator.SetFloat(LockMoveBlendX, owner.MoveX);
                owner.m_Animator.SetFloat(LockMoveBlendY, owner.MoveY);

            }
            else
            {
                owner.m_Animator.SetBool(IsLockEnemy, false);
                owner.m_Animator.SetFloat(LockMoveBlendX, 0);
                owner.m_Animator.SetFloat(LockMoveBlendY, 0);
                if (owner.isRun)
                {
                    owner.m_Animator.SetFloat(MoveBlend, Mathf.Max(Mathf.Abs(owner.MoveX), Mathf.Abs(owner.MoveY)) * 2, dampTimeX, Time.deltaTime);
                    owner.PutDownWeapon();
                }
                else
                {
                    owner.m_Animator.SetFloat(MoveBlend, Mathf.Max(Mathf.Abs(owner.MoveX), Mathf.Abs(owner.MoveY)), dampTimeX, Time.deltaTime);
                }
            }
            if (owner.MoveX == default && owner.MoveY == default)
            {
                owner.m_Animator.SetFloat(MoveBlend, 0, 0, Time.deltaTime);
                ChangeState<PlayerIdleState>(procedureOwner);
                return;
            }

            if (owner.IsDefense && owner.IsHands)
            {
                owner.m_Animator.SetBool(Defense, true);
                ChangeState<PlayerDefenseState>(procedureOwner);
                return;
            }
            else
            {
                owner.m_Animator.SetBool(Defense, false);
            }
            if (owner.m_Attack)
            {
                if (!owner.IsHands)
                {
                    if (owner.m_PutDownWeaponFinish)
                    {
                        ChangeState<PlayerEquipWeaponState>(procedureOwner);
                        return;
                    }
                    
                }
                else
                {
                    ChangeState<PlayerAttackState>(procedureOwner);
                    return;
                }
                    

                
            }
            //if (owner.isFourthAtk && owner.IsHands && !owner.isSecondSkill)
            //{
            //    if (owner.m_Animator.GetInteger("ThumpNum") > 10)
            //    {
            //        owner.isFourthAtk = false;
            //        return;
            //    }
            //    ChangeState<PlayerSkillState>(procedureOwner);
            //}

            if (owner.isDodge)
            {
                ChangeState<PlayerDodgeState>(procedureOwner);
                return;
            }
            if (owner.isWeaponState)
            {
                ChangeState<PlayerEquipState>(procedureOwner);
                return;
            }
         
            if (owner.isStep)
            {
                ChangeState<PlayerStepState>(procedureOwner);
                return;
            }


        }

        protected override void OnLeave(ProcedureOwner fsm, bool isShutdown)
        {
            base.OnLeave(fsm, isShutdown);
            owner.m_Animator.SetBool(IsLockEnemy, false);
           // owner.m_Animator.SetFloat(LockMoveBlendX, 0);
           // owner.m_Animator.SetFloat(LockMoveBlendY, 0);
        }


        public static PlayerMotionState Create()
        {
            PlayerMotionState state = ReferencePool.Acquire<PlayerMotionState>();
            return state;
        }

        public new void Clear()
        {
            owner = null;
        }
    }
}
