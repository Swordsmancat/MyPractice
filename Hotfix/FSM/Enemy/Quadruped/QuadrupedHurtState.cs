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
    public class QuadrupedHurtState :EnemyHurtState
    {
        private static readonly int m_HurtThump = Animator.StringToHash("HurtThump");
        private static readonly int KnockedDown = Animator.StringToHash("KnockedDown");
        private static readonly int FlyRevolt_Enemy = Animator.StringToHash("FlyRevolt_Enemy");
        private static readonly int DownRevolt_Enemy = Animator.StringToHash("DownRevolt_Enemy");
        private static readonly int HurtRandom = Animator.StringToHash("HurtRandom");
        private static readonly int SkillAttack = Animator.StringToHash("SkillAttack");
        private static readonly int SkillBackHit = Animator.StringToHash("SkillBackHit");
        //private static readonly int KatanaSPAttack = Animator.StringToHash("KatanaSPAttack");
        private static readonly int GreatSwordUpAttack = Animator.StringToHash("GreatSwordUpAttack");
        //private static readonly int ForwardAttack = Animator.StringToHash("ForwardAttack");
        //private static readonly int ShieldAttack = Animator.StringToHash("ShieldAttack");
        //private static readonly int TwoDaggersAttack = Animator.StringToHash("TwoDaggersAttack");
        //private static readonly int KatanaAttack = Animator.StringToHash("KatanaAttack");
        private static readonly int StunAttack = Animator.StringToHash("StunAttack");
        //private static readonly int LeftAttack = Animator.StringToHash("LeftAttack");
        //private static readonly int RightAttack = Animator.StringToHash("RightAttack");
        //private static readonly int UpAttack = Animator.StringToHash("UpAttack");
        //private static readonly int LightningAttack = Animator.StringToHash("LightningAttack");

        public static new QuadrupedHurtState Create()
        {
            QuadrupedHurtState state = ReferencePool.Acquire<QuadrupedHurtState>();
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

        protected override void EnemyHurtStateStart(ProcedureOwner fsm)
        {
            //base.EnemyHurtStateStart(fsm);
            switch (owner.BuffType)
            {
                case BuffType.None:
                    owner.HurtEnd = true;
                    break;
                case BuffType.Tap:
                    owner.HurtEnd = true;
                    break;
                case BuffType.Thump:
                    owner.HurtEnd = true;
                    break;
                case BuffType.Overwhelmed:
                    owner.HurtEnd = true;
                    break;
                case BuffType.SkillAttack:
                    Vector3 target = owner.find_Player.transform.position - owner.transform.position;
                    Vector3 obj = owner.transform.forward;
                    bool forward = (Vector3.Dot(target, obj) > 0);
                    if (forward)
                    {
                        owner.m_Animator.SetTrigger(SkillAttack);
                    }
                    else
                    {
                        owner.m_Animator.SetTrigger(SkillBackHit);
                    }
                    owner.m_IsInKnockedDown = true;
                    break;
                //case BuffType.KatanaSPAttack:
                //    owner.m_Animator.SetTrigger(KatanaSPAttack);
                //    break;
                case BuffType.GreatSwordUpAttack:
                    owner.m_Animator.SetTrigger(GreatSwordUpAttack);
                    break;
                //case BuffType.ForwardAttack:
                //    owner.m_Animator.SetTrigger(ForwardAttack);
                //    break;
                //case BuffType.ShieldAttack:
                //    owner.m_Animator.SetTrigger(ShieldAttack);
                //    break;
                //case BuffType.TwoDaggersAttack:
                //    owner.m_Animator.SetTrigger(TwoDaggersAttack);
                //    break;
                //case BuffType.KatanaAttack:
                //    owner.m_Animator.SetTrigger(KatanaAttack);
                //    break;
                case BuffType.StunAttack:
                    owner.m_Animator.SetTrigger(StunAttack);
                    break;
                //case BuffType.LeftAttack:
                //    owner.m_Animator.SetTrigger(LeftAttack);
                //    break;
                //case BuffType.RightAttack:
                //    owner.m_Animator.SetTrigger(RightAttack);
                //    break;
                //case BuffType.LightningAttack:
                //    owner.m_Animator.SetTrigger(LightningAttack);
                //    break;
                //case BuffType.UpAttack:
                //    owner.m_Animator.SetTrigger(UpAttack);
                //    break;
                default:
                    owner.HurtEnd = true;
                    break;

            }

        }
    }
}
