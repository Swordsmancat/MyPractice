using UnityEngine;
using GameFramework.Fsm;
using ProcedureOwner = GameFramework.Fsm.IFsm<Farm.Hotfix.EnemyLogic>;
using GameFramework;

namespace Farm.Hotfix
{
    public class OrcDoubleAxeHurtState : EnemyHurtState
    {
        private static readonly int m_HurtState = Animator.StringToHash("HurtState");
        private static readonly int CanAvoid = Animator.StringToHash("CanAvoid");
        //private static readonly int infight = Animator.StringToHash("Infight");
        private static readonly int IsStun = Animator.StringToHash("IsStun");
        //private static readonly int FrenzyEnd = Animator.StringToHash("FrenzyEnd");
        //private static readonly int Frenzy = Animator.StringToHash("Frenzy");
        //private static readonly int m_Hurt = Animator.StringToHash("Hurt");
        private static readonly int m_HurtThump = Animator.StringToHash("HurtThump");
        private static readonly int KnockedDown = Animator.StringToHash("KnockedDown");
        private static readonly int FlyRevolt_Enemy = Animator.StringToHash("FlyRevolt_Enemy");
        private static readonly int DownRevolt_Enemy = Animator.StringToHash("DownRevolt_Enemy");
        private static readonly int HurtRandom = Animator.StringToHash("HurtRandom");
        private static readonly int SkillAttack = Animator.StringToHash("SkillAttack");
        private static readonly int SkillBackHit = Animator.StringToHash("SkillBackHit");
        private static readonly int KatanaSPAttack = Animator.StringToHash("KatanaSPAttack");
        private static readonly int GreatSwordUpAttack = Animator.StringToHash("GreatSwordUpAttack");
        private static readonly int ForwardAttack = Animator.StringToHash("ForwardAttack");
        private static readonly int ShieldAttack = Animator.StringToHash("ShieldAttack"); 
        private static readonly int TwoDaggersAttack = Animator.StringToHash("TwoDaggersAttack");
        private static readonly int KatanaAttack = Animator.StringToHash("KatanaAttack");
        private static readonly int StunAttack = Animator.StringToHash("StunAttack");
        private static readonly int LeftAttack = Animator.StringToHash("LeftAttack");
        private static readonly int RightAttack = Animator.StringToHash("RightAttack");
        private static readonly int UpAttack = Animator.StringToHash("UpAttack");
        private static readonly int LightningAttack = Animator.StringToHash("LightningAttack");

        private OrcDoubleAxeLogic me;


        public static OrcDoubleAxeHurtState Create()
        {
            OrcDoubleAxeHurtState state = ReferencePool.Acquire<OrcDoubleAxeHurtState>();
            return state;
        }
        protected override void OnEnter(ProcedureOwner fsm)
        {
            owner = fsm.Owner;
            me = owner as OrcDoubleAxeLogic;
            base.OnEnter(fsm);
            
        }
        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);

        }
        protected override void EnemyHurtStateStart(ProcedureOwner fsm)
        {
            if (!owner.Stoic) 
            {
                switch (me.m_BuffType)
                {
                    case BuffType.None:
                        break;
                    case BuffType.Tap:
                        HurtTap();
                        break;
                    case BuffType.Thump:
                        AHurtThump();
                        break;
                    case BuffType.Overwhelmed:
                        HurtOverwhelmed();
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
                    case BuffType.KatanaSPAttack:
                        owner.m_Animator.SetTrigger(KatanaSPAttack);
                        owner.m_IsInKnockedDown = true;
                        break;
                    case BuffType.GreatSwordUpAttack:
                        owner.m_Animator.SetTrigger(GreatSwordUpAttack);
                        owner.m_IsInKnockedDown = true;
                        break;
                    case BuffType.ForwardAttack:
                        owner.m_Animator.SetTrigger(ForwardAttack);
                        owner.m_IsInKnockedDown = true;
                        break;
                    case BuffType.ShieldAttack:
                        owner.m_Animator.SetTrigger(ShieldAttack);
                        owner.m_IsInKnockedDown = true;
                        break;
                    case BuffType.TwoDaggersAttack:
                        owner.m_Animator.SetTrigger(TwoDaggersAttack);
                        owner.m_IsInKnockedDown = true;
                        break; 
                    case BuffType.KatanaAttack:
                        owner.m_Animator.SetTrigger(KatanaAttack);
                        owner.m_IsInKnockedDown = true;
                        break;
                    case BuffType.StunAttack:
                        owner.m_Animator.SetTrigger(StunAttack);
                        break;
                    case BuffType.LeftAttack:
                        owner.m_Animator.SetTrigger(LeftAttack);
                        break;
                    case BuffType.RightAttack:
                        owner.m_Animator.SetTrigger(RightAttack);
                        break;
                    case BuffType.LightningAttack:
                        owner.m_Animator.SetTrigger(LightningAttack);
                        break;
                    case BuffType.UpAttack:
                        owner.m_Animator.SetTrigger(UpAttack);
                        break;
                    default:
                        owner.m_Animator.SetTrigger(Hurt);
                        break;
                
               }
               
        }
            
            owner.m_Animator.SetBool(CanAvoid, false);
        }
        protected override void OnLeave(ProcedureOwner fsm, bool isShutdown)
        {
            base.OnLeave(fsm, isShutdown);
            owner.m_Animator.SetInteger(m_HurtState, -1);
            Debug.Log("退出受伤状态");

        }
        private void HurtTap()
     
        {  
                     
         
                Vector3 target = owner.find_Player.transform.position - owner.transform.position;
                Vector3 obj = owner.transform.forward;
                //目标方位
                bool forward = (Vector3.Dot(target, obj) > 0);
                bool left = (Vector3.Cross(target, obj).y > 0);
                //owner.IsHurt = false;
                Debug.Log("轻攻击");
                if (forward)
                {
                    owner.m_Animator.SetInteger(HurtRandom, Utility.Random.GetRandom(0, 4));
                    owner.m_Animator.SetTrigger(Hurt);
                    owner.m_Animator.SetInteger(m_HurtState, 0);
                    //GameEntry.Sound.PlaySound(ShoutId);
                }
                else if (left)
                {
                    owner.m_Animator.SetTrigger(Hurt);
                    owner.m_Animator.SetInteger(m_HurtState, 1);
                    //GameEntry.Sound.PlaySound(ShoutId);
                }
            
           

        }
        private void AHurtThump()
        {
        
                Vector3 target = owner.find_Player.transform.position - owner.transform.position;
                Vector3 obj = owner.transform.forward;
                //目标方位
                bool forward = (Vector3.Dot(target, obj) > 0);
                bool left = (Vector3.Cross(target, obj).y > 0);
                Debug.Log("重攻击");
                int num = Utility.Random.GetRandom(0, 10);
                int ThumpNum = Utility.Random.GetRandom(0, 100);
                if (ThumpNum < 70)
                {

                    if (forward)
                    {
                        owner.m_Animator.SetTrigger(m_HurtThump);
                        owner.m_Animator.SetInteger(m_HurtState, 0);
                        //GameEntry.Sound.PlaySound(ShoutId);
                    }

                    if (left)
                    {
                        owner.m_Animator.SetTrigger(m_HurtThump);
                        owner.m_Animator.SetInteger(m_HurtState, 1);
                        //GameEntry.Sound.PlaySound(ShoutId);
                    }
                    else if (!left)
                    {
                        owner.m_Animator.SetTrigger(m_HurtThump);
                        owner.m_Animator.SetInteger(m_HurtState, 2);
                        //GameEntry.Sound.PlaySound(ShoutId);
                    }

                    //    if (num == 3 || num == 4)
                    //    {
                    //        owner.m_Animator.SetTrigger(HurtThump);
                    //        owner.m_Animator.SetInteger(m_HurtState, num);
                    //        //GameEntry.Sound.PlaySound(ShoutId);
                    //    }
                    else if (num == 0)
                        owner.m_Animator.SetTrigger(IsStun);
                }
                else
                {
                    owner.m_Animator.SetTrigger(FlyRevolt_Enemy);
                }
            
            
        }
        private void HurtOverwhelmed()
        {
            
            Vector3 target = owner.find_Player.transform.position - owner.transform.position;
            Vector3 obj = owner.transform.forward;
            //目标方位
            bool forward = (Vector3.Dot(target, obj) > 0);
            bool left = (Vector3.Cross(target, obj).y > 0);
            owner.m_Animator.SetTrigger(m_HurtThump);
            if (forward)
            {
                owner.m_Animator.SetInteger(m_HurtState, 0);
            }
            else if (left)
            {
                owner.m_Animator.SetInteger(m_HurtState, 1);
            }
            else if (!left)
            {
                owner.m_Animator.SetInteger(m_HurtState, 2);
            }
            else
            {
                owner.m_Animator.SetInteger(m_HurtState, 0);
            }
            
            
        }
        protected override void EnemyHurtStateEnd(IFsm<EnemyLogic> procedureOwner)
        {
            owner.m_Animator.SetBool(CanAvoid, true);
            //owner.m_Animator.SetInteger(m_HurtState, -1);
            if (!owner.Stoic && !owner.isKnockedDown)
            {
                base.EnemyHurtStateEnd(procedureOwner);
            }
            
        }
    }
}