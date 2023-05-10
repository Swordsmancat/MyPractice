using UnityEngine;
using GameFramework.Fsm;
using ProcedureOwner = GameFramework.Fsm.IFsm<Farm.Hotfix.PlayerLogic>;
using GameFramework;
using UnityGameFramework.Runtime;

namespace Farm.Hotfix
{
    public class PlayerHurtState : PlayerBaseActionState
    {
        private PlayerLogic owner;
        private readonly string Layer = "Base Layer";
        private static readonly int Hurt = Animator.StringToHash("Hurt");
        //private static readonly int DownHurt = Animator.StringToHash("DownHurt");
        //private static readonly int AttackTrigger = Animator.StringToHash("AttackTrigger");
        private static readonly int HoldClick = Animator.StringToHash("HoldClick");
        private static readonly int AttackTrigger = Animator.StringToHash("AttackTrigger");
        private static readonly int HurtDirX = Animator.StringToHash("HurtDirX");
        private static readonly int HurtDirY = Animator.StringToHash("HurtDirY");
        private static readonly int KnockedDown = Animator.StringToHash("KnockedDown");
        private static readonly int DownRevolt = Animator.StringToHash("DownRevolt");
        private static readonly int KnockedFly = Animator.StringToHash("KnockedFly");
        private static readonly int RandomFly = Animator.StringToHash("RandomFly");
        private static readonly int FlyRevolt = Animator.StringToHash("FlyRevolt");
        private static readonly int HurtRandom = Animator.StringToHash("HurtRandom");
        private static readonly int MoveBlend = Animator.StringToHash("MoveBlend");
        private static readonly int LockMoveBlendX = Animator.StringToHash("LockMoveBlendX");
        private static readonly int LockMoveBlendY = Animator.StringToHash("LockMoveBlendY");
        private static readonly int ShoulderStrike = Animator.StringToHash("ShoulderStrike");
        private static readonly int LefthandStrike = Animator.StringToHash("LefthandStrike");
        private static readonly int RighthandStrike = Animator.StringToHash("Righthandtrike");
        private static readonly int LeftfootStrike = Animator.StringToHash("LeftfootStrike");
        private static readonly int RightfootStrike = Animator.StringToHash("RightfootStrike");
        private static readonly int ShieldStrike = Animator.StringToHash("ShieldStrike");
        private static readonly int SkillAttack = Animator.StringToHash("SkillAttack");
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
        private static readonly int JumpAttack = Animator.StringToHash("JumpAttack");
        private static readonly int Punchhard = Animator.StringToHash("Punchhard");
        private static readonly int Punch = Animator.StringToHash("Punch");
        private static readonly int LightningAttack = Animator.StringToHash("LightningAttack");
        private static readonly int FireAttack = Animator.StringToHash("FireAttack");
        private static readonly int SoundAttack = Animator.StringToHash("SoundAttack");
        private static readonly int m_GetRebound = Animator.StringToHash("GetRebound");
        private static readonly int ReboundState = Animator.StringToHash("ReboundState");
        private float downTime;
        private int DownRevoltNum = 2;
        private bool IsDown;
        private bool Isfly;



        protected override void OnInit(ProcedureOwner procedureOwner)
        {
            base.OnInit(procedureOwner);
        }

        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);
            owner = procedureOwner.Owner;

            owner.m_Animator.SetFloat(MoveBlend, 0);
            owner.m_Animator.SetFloat(LockMoveBlendX, 0);
            owner.m_Animator.SetFloat(LockMoveBlendY, 0);
            owner.SetDodgeStepOff();
            if (!owner.isStoic)
            {
                owner.HurtEnd = false;
                if (owner.GetRebound)
                {
                    owner.m_Animator.SetTrigger(m_GetRebound);
                    owner.m_Animator.SetInteger(ReboundState, (int)owner.GetReboundState);
                    owner.GetRebound = false;
                    owner.GetReboundEqui = EquiState.None;
                    owner.GetCollider = ColliderState.None;
                    owner.GetReboundState = Hotfix.ReboundState.Ordinary;
                }
                else
                {
                    switch (owner.m_BuffType)
                    {
                        case BuffType.None:
                            switch (owner.GetCollider)
                            {
                                case ColliderState.None:
                                    HurtState();
                                    break;
                                case ColliderState.Shoulder:
                                    owner.m_Animator.SetTrigger(ShoulderStrike);
                                    break;
                                case ColliderState.Lefthand:
                                    owner.m_Animator.SetTrigger(LefthandStrike); 
                                    break;
                                case ColliderState.Righthand:
                                    owner.m_Animator.SetTrigger(RighthandStrike); 
                                    break;
                                case ColliderState.Leftfoot:
                                    owner.m_Animator.SetTrigger(LeftfootStrike);
                                    break;
                                case ColliderState.Rightfoot:
                                    owner.m_Animator.SetTrigger(RightfootStrike); 
                                    break;
                                case ColliderState.Shield:
                                    owner.m_Animator.SetTrigger(ShieldStrike); 
                                    break;
                                case ColliderState.Head:
                                    break;
                                default:
                                    break;
                            }
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
                        case BuffType.SkillAttack:
                            owner.m_Animator.SetTrigger(SkillAttack);
                            break;
                        case BuffType.KatanaSPAttack:
                            owner.m_Animator.SetTrigger(KatanaSPAttack);
                            break;
                        case BuffType.GreatSwordUpAttack:
                            owner.m_Animator.SetTrigger(GreatSwordUpAttack);
                            break;
                        case BuffType.ForwardAttack:
                            owner.m_Animator.SetTrigger(ForwardAttack);
                            break;
                        case BuffType.ShieldAttack:
                            owner.m_Animator.SetTrigger(ShieldAttack);
                            break;
                        case BuffType.TwoDaggersAttack:
                            owner.m_Animator.SetTrigger(TwoDaggersAttack);
                            break;
                        case BuffType.KatanaAttack:
                            owner.m_Animator.SetTrigger(KatanaAttack);
                            break;
                        case BuffType.StunAttack:
                            owner.m_Animator.SetTrigger(StunAttack);
                            break;
                        case BuffType.UpAttack:
                            owner.m_Animator.SetTrigger(UpAttack); 
                            break;
                        case BuffType.JumpAttack:
                            owner.m_Animator.SetTrigger(JumpAttack);
                            break;
                        case BuffType.LeftAttack:
                            owner.m_Animator.SetTrigger(LeftAttack);
                            break;
                        case BuffType.RightAttack:
                            owner.m_Animator.SetTrigger(RightAttack); 
                            break;
                        case BuffType.Punchhard:
                            owner.m_Animator.SetTrigger(Punchhard);
                            break;
                        case BuffType.Punch:
                            owner.m_Animator.SetTrigger(Punch);
                            break;
                        case BuffType.LightningAttack:
                            owner.m_Animator.SetTrigger(LightningAttack);
                            break;
                        case BuffType.FireAttack:
                            owner.m_Animator.SetTrigger(FireAttack);
                            break;
                        case BuffType.SoundAttack:
                            owner.m_Animator.SetTrigger(SoundAttack);
                            break;
                        default:
                            HurtState();
                            break;
                    }

                    GameEntry.Entity.ShowEffect(new EffectData(GameEntry.Entity.GenerateSerialId(),
                     owner.Armors[0].ArmorData.HurtEffectId)
                    {
                        Owner = owner
                    },
                     typeof(PlayerHurtEffectLogic));
                }



                owner.HideTrail();//角色倒地 关闭拖尾和攻击检测 防止怪物碰到角色武器会一直受伤
                owner.AttackEnd();//同上
                owner.m_moveBehaviour.IsKnockLock(true);//防止角色倒地后 按方向键会转动
                                                        //受伤声音测试
                                                        //     GameEntry.Sound.PlaySound(owner.Armors[0].ArmorData.HurtSoundId);
                                                        //      GameEntry.Sound.PlaySound(owner.Armors[0].ArmorData.HurtSoundId2);


            }
            else
            {
                GameEntry.Sound.PlaySound(owner.Armors[0].ArmorData.StoicHurtSoundId);
                GameEntry.Entity.ShowEffect(new EffectData(GameEntry.Entity.GenerateSerialId(),
                    owner.Armors[0].ArmorData.StoicHurtEffectId)
                {
                    Owner = owner
                },
                    typeof(PlayerHurtEffectLogic));
                owner.HurtEnd = true;
            }
            owner.isHurt = true;
            //owner.Buff.BuffTypeEnum = BuffType.None;


        }
        private void KnockedDownState()
        {
            if (!owner.isKnockedDown)
            {
                //owner.isKnockedDown = true;
                int num = Random.Range(0, 10);
                IsDown = true;
                owner.isKnockedDown = true;
                owner.Buff.BuffTypeEnum = BuffType.None;
                owner.m_AnimationEventGetUp = false;
                owner.m_moveBehaviour.IsKnockLock(true);
                if (num > DownRevoltNum)
                {
                    owner.m_Animator.SetInteger(HurtRandom, Utility.Random.GetRandom(0, 4));
                    owner.m_Animator.SetFloat(HurtDirX, owner.attackDir.x);
                    owner.m_Animator.SetFloat(HurtDirY, owner.attackDir.y);
                    owner.m_AnimationEventGetUp = false;
                    owner.m_Animator.SetTrigger(KnockedDown);
                    owner.isKnockedDown = true;
                }
                else
                {
                    owner.m_Animator.SetTrigger(DownRevolt);
                    Log.Info("抵抗击倒");
                }
            }
            else
            {
                owner.m_Animator.SetTrigger(KnockedDown);
                owner.isKnockedDown = true;
            }

        }
        private void KnockedFlyState()
        {
            if (!owner.isKnockedDown)
            {
                owner.isKnockedDown = true;
                int num = Random.Range(0, 10);

                Isfly = true;
                if (num > 2)
                {
                    owner.m_Animator.SetInteger(RandomFly, Utility.Random.GetRandom(0, 3));
                    owner.m_Animator.SetFloat(HurtDirX, owner.attackDir.x);
                    owner.m_Animator.SetFloat(HurtDirY, owner.attackDir.y);
                    owner.m_AnimationEventGetUp = false;
                    owner.m_Animator.SetTrigger(KnockedFly);
                    owner.isKnockedDown = true;
                }
                else
                {
                    owner.m_Animator.SetTrigger(FlyRevolt);
                    Log.Info("抵抗击飞");
                }

            }
            else
            {
                owner.m_Animator.SetTrigger(KnockedDown);
                owner.isKnockedDown = true;
            }

        }
        private void HurtState()
        {
            if (!owner.isKnockedDown)
            {
                //Debug.Log("普通受击");
                owner.m_Animator.SetInteger(HurtRandom, Utility.Random.GetRandom(0, 4));
                owner.m_Animator.SetFloat(HurtDirX, owner.attackDir.x);
                owner.m_Animator.SetFloat(HurtDirY, owner.attackDir.y);
                owner.m_Animator.SetTrigger(Hurt);
                owner.m_Animator.ResetTrigger(AttackTrigger);

                //  owner.m_Animator.SetFloat(HoldClick, -1);

            }
            else
            {
                owner.m_Animator.SetTrigger(KnockedDown);
                owner.isKnockedDown = true;
            }
        }

        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);

            if (owner.HurtEnd)
            {
                if (owner.isKnockedDown)
                {
                    owner.isKnockedDown = false;
                    IsDown = false;
                    Isfly = false;
                    if (owner.m_moveBehaviour.m_Horizontal != 0 || owner.m_moveBehaviour.m_Vertical != 0)
                    {
                        ChangeState<PlayerDodgeState>(procedureOwner);
                    }
                    else
                    {
                        ChangeState<PlayerMotionState>(procedureOwner);
                    }
                }
                else
                {
                    ChangeState<PlayerMotionState>(procedureOwner);
                }
            }



        }

        protected override void OnLeave(ProcedureOwner fsm, bool isShutdown)
        {
            base.OnLeave(fsm, isShutdown);
            //owner.m_Animator.SetFloat(HurtDirX, 0);
            //owner.m_Animator.SetFloat(HurtDirY, 0);
            owner.isHurt = false;
            //  owner.isKnockedDown = false;
            owner.m_moveBehaviour.IsKnockLock(false);
            owner.Buff.BuffTypeEnum = BuffType.None;
        }

        public static PlayerHurtState Create()
        {
            PlayerHurtState state = ReferencePool.Acquire<PlayerHurtState>();
            return state;
        }

    }
}
