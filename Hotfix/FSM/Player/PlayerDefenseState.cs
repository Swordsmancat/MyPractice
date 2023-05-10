using UnityEngine;
using GameFramework.Fsm;
using ProcedureOwner = GameFramework.Fsm.IFsm<Farm.Hotfix.PlayerLogic>;
using GameFramework;

namespace Farm.Hotfix
{
    public class PlayerDefenseState : PlayerBaseActionState
    {
        private PlayerLogic owner;
        private static readonly int TapParry = Animator.StringToHash("TapParry");
        private static readonly int ThumpParry = Animator.StringToHash("ThumpParry");
        private static readonly int OverParry = Animator.StringToHash("OverParry");
        private static readonly int SkillParry = Animator.StringToHash("SkillParry");
        private static readonly int ParryBreak = Animator.StringToHash("ParryBreak");
        private static readonly int Defense = Animator.StringToHash("Defense");
        private bool isOut;
        private bool OutHurt;
        private float currentMP;
        private float outTime;
        //private readonly string Layer = "Base Layer";


        protected override void OnInit(ProcedureOwner procedureOwner)
        {
            base.OnInit(procedureOwner);
        }

        protected override void OnEnter(ProcedureOwner procedureOwner)
        {

            base.OnEnter(procedureOwner);
            //Debug.Log("格挡状态");
            owner = procedureOwner.Owner;
            owner.m_Animator.SetBool(Defense, true);
            //Debug.Log("精力值:" + owner.PlayerData.MP);
            owner.Buff.BuffTypeEnum = BuffType.None;
            isOut = false;
            owner.underAttack = false;

        }

        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);

            if (owner.underAttack)
            {
                DefenseHurt();
            }

            if (isOut)
            {
              
                outTime += Time.deltaTime;
                //Debug.Log("破防退出");
                if (outTime > 0.2f)
                {
                    //Debug.Log("破防切换状态");
                    outTime = 0;
                    ChangeState<PlayerMotionState>(procedureOwner);
                    return;
                }  
            }
            if (OutHurt) //owner.m_Animator.SetTrigger(DefenseOut);
            {
                //owner.m_Animator.SetTrigger(DefenseOut);
                ChangeState<PlayerHurtState>(procedureOwner);
                return;
            }
            if (owner.m_Attack)
            {
                ChangeState<PlayerAttackState>(procedureOwner);
                return;
            }
            if (owner.isDodge)
            {
                ChangeState<PlayerDodgeState>(procedureOwner);
                return;
            }
            if (!owner.IsDefense)
            {
                ChangeState<PlayerMotionState>(procedureOwner);
                return;
            }
            //ChangeState<>(procedureOwner);
        }
        private void DefenseHurt()
        {
            Debug.Log("受击状态"+owner.m_BuffType);
            switch (owner.m_BuffType)
            {
                case BuffType.None:
                    HurtState();
                    break;
                case BuffType.Tap:
                    HurtState();
                    break;
                case BuffType.Thump:
                    KnockedDownState();
                    break;
                case BuffType.Overwhelmed:
                    KnockedFlyState();
                    break;
                default:
                    SkillState();
                    break;
            }
            //owner.Buff.BuffTypeEnum = BuffType.None;
            owner.underAttack = false;
            owner.HideTrail();//角色格挡 关闭拖尾和攻击检测 防止怪物碰到角色武器会一直受伤
            owner.AttackEnd();//同上
            
        }
        private void HurtState()
        {
            if (owner.PlayerData.TrunkValue > 0)
            {
                if (owner.PlayerData.TrunkValue >= 20)
                {
                    owner.PlayerData.TrunkValue -= 20;
                }
                else
                {
                    owner.PlayerData.TrunkValue = 0;
                }
                owner.m_ProcedureMain.SetPlayerValue(owner.PlayerData.HP, owner.PlayerData.TrunkValue);
                owner.m_Animator.SetTrigger(TapParry);
            }
            else
            {
                isOut = true;
                owner.m_Animator.SetTrigger(ParryBreak);
            }
           
           
        }
        private void KnockedDownState()
        {
            if (owner.PlayerData.TrunkValue > 0)
            {
                if (owner.PlayerData.TrunkValue >= 28)
                {
                    owner.PlayerData.TrunkValue -= 28;
                }
                else
                {
                    owner.PlayerData.TrunkValue = 0;
                }
                owner.m_ProcedureMain.SetPlayerValue(owner.PlayerData.HP, owner.PlayerData.TrunkValue);
                owner.m_Animator.SetTrigger(ThumpParry);
            }
            else
            {
                owner.m_Animator.SetTrigger(ParryBreak);
            }
            isOut = true;
            
        }
        private void KnockedFlyState()
        {
            if (owner.PlayerData.TrunkValue > 0)
            {
                if (owner.PlayerData.TrunkValue >= 35)
                {
                    owner.PlayerData.TrunkValue -= 35;
                }
                else
                {
                    owner.PlayerData.TrunkValue = 0;
                }
                owner.m_ProcedureMain.SetPlayerValue(owner.PlayerData.HP, owner.PlayerData.TrunkValue);
                owner.m_Animator.SetTrigger(OverParry);
            }
            else
            {
                owner.m_Animator.SetTrigger(ParryBreak);
            }
            isOut = true;

        }
        private void SkillState()
        {
            if (owner.PlayerData.TrunkValue > 0)
            {
                if (owner.PlayerData.TrunkValue >= 40)
                {
                    owner.PlayerData.TrunkValue -= 40;
                }
                else
                {
                    owner.PlayerData.TrunkValue = 0;
                }
                owner.m_ProcedureMain.SetPlayerValue(owner.PlayerData.HP, owner.PlayerData.TrunkValue);
                owner.m_Animator.SetTrigger(SkillParry);
            }
            else
            {
                owner.m_Animator.SetTrigger(ParryBreak);
            }
            isOut = true;

        }
        private void MPCalculate(int minNum, int maxNum)
        {
            int energyLoss = Utility.Random.GetRandom(minNum, maxNum);
            currentMP = owner.PlayerData.MP - energyLoss;
            owner.PlayerData.MP = currentMP > 0 ? currentMP : 0;
            Debug.Log("当前精力" + currentMP);
            Debug.Log("总精力" + owner.PlayerData.MP);
        }
        protected override void OnLeave(ProcedureOwner fsm, bool isShutdown)
        {
            base.OnLeave(fsm, isShutdown);
            owner.Buff.BuffTypeEnum = BuffType.None;
            isOut = false;
            OutHurt = false;
            owner.IsDefense = false;
            owner.m_Animator.ResetTrigger(TapParry);
            owner.m_Animator.ResetTrigger(ThumpParry);
            owner.m_Animator.ResetTrigger(OverParry);
            owner.m_Animator.ResetTrigger(SkillParry);
        }
        public static PlayerDefenseState Create()
        {
            PlayerDefenseState state = ReferencePool.Acquire<PlayerDefenseState>();
            return state;
        }

    }

}

