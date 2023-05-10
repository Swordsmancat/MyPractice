using UnityEngine;
using GameFramework.Fsm;
using ProcedureOwner = GameFramework.Fsm.IFsm<Farm.Hotfix.EnemyLogic>;
using GameFramework;
using UnityGameFramework.Runtime;
namespace Farm.Hotfix
{
    public class OrcDoubleAxeAttackState : EnemyAttackState
    {

        private float m_FirstTakeAttackTime = 3.5f;
        private float m_SecondTakeAttackTime = 4f;
        private readonly float FRIST_PHASE = 0.5f; //第一阶段
        private readonly float SECOND_PHASE = 0.25f;//第二阶段
        AnimatorStateInfo info;
        private bool isFrenzyEnd =false;

        private static readonly int MoveBlend = Animator.StringToHash("MoveBlend");
        private static readonly int FightBlend = Animator.StringToHash("FightBlend");
        private static readonly int FrenzyEnd = Animator.StringToHash("FrenzyEnd");
        private static readonly int Frenzy = Animator.StringToHash("Frenzy");
        private readonly static int m_stop = Animator.StringToHash("Stop");
        private readonly static int m_InFight = Animator.StringToHash("InFight");
        private static readonly int m_HashStateTime = Animator.StringToHash("StateTime");
        private readonly static int RandomAttack = Animator.StringToHash("RandomAttack");
        private readonly static int RandomSkill = Animator.StringToHash("RandomSkill");

        public static new OrcDoubleAxeAttackState Create()
        {
            OrcDoubleAxeAttackState state = ReferencePool.Acquire<OrcDoubleAxeAttackState>();
            return state;
        }

        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);
            owner.AnimationStart();
            EnemyAttackStateStart(owner);
        }

        protected override void OnLeave(ProcedureOwner fsm, bool isShutdown)
        {
            base.OnLeave(fsm, isShutdown);
            owner.m_Animator.SetFloat(m_HashStateTime, 0);
            owner.m_Animator.SetInteger(AttackState, -1);
        }

        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);
            info = owner.m_Animator.GetCurrentAnimatorStateInfo(0);
            owner.m_Animator.SetFloat(m_HashStateTime, Mathf.Repeat(info.normalizedTime, 1f));
            owner.SetRichAiStop();
            if (owner.m_IsAttackRotate)
            {
                AIUtility.RotateToTarget(owner.find_Player, owner, -10, 10);
            }
            if (owner.IsAnimPlayed)
            {
                ChangeState(procedureOwner, owner.ChangeStateEnemy(EnemyStateType.Fight));
            }
        }


        protected override void EnemyAttackStateStart(EnemyLogic owner)
        {
            owner.SetRichAiStop();
            StopBlend(owner);
            owner.m_Animator.SetBool(m_InFight, true);
            owner.m_Animator.SetInteger(RandomAttack, Utility.Random.GetRandom(0, 3));
            owner.m_Animator.SetInteger(RandomSkill, Utility.Random.GetRandom(0, 3));

            if (owner.enemyData.HPRatio > FRIST_PHASE) //第一阶段
            {
                owner.ReduceAttackTime = m_FirstTakeAttackTime;
                if (owner.enemyData.MoraleValue > 80) //士气大于60进4号线
                {
                    if (!owner.m_IsFrenzy)
                    {

                        owner.m_Animator.SetTrigger(Frenzy);
                        StopBlend(owner);
                        owner.m_IsFrenzy = true;
                        owner.enemyData.MoraleValue -= 80;
                        int randomNum = Utility.Random.GetRandom(4, 8);
                        owner.m_Animator.SetInteger(AttackState, randomNum);
                    }
                    else
                    {
                        int randomNum = Utility.Random.GetRandom(4, 8);
                        owner.m_Animator.SetInteger(AttackState, randomNum);
                    }
                
                }
                else
                {
                    if (owner.m_IsFrenzy)
                    {
                        int randomNum = Utility.Random.GetRandom(4, 8);
                        owner.m_Animator.SetInteger(AttackState, randomNum);
                    }
                    else
                    {
                        int randomNum = Utility.Random.GetRandom(0, 4);
                        owner.m_Animator.SetInteger(AttackState, randomNum);
                    }
                   
                }

            }
            else if (SECOND_PHASE < owner.enemyData.HPRatio && owner.enemyData.HPRatio < FRIST_PHASE) //第二阶段
            {
                owner.ReduceAttackTime = m_SecondTakeAttackTime;
           
                if (owner.enemyData.MoraleValue > 80) //士气大于60进4号线
                {
                    if (!owner.m_IsFrenzy)
                    {

                        owner.m_Animator.SetTrigger(Frenzy);
                        StopBlend(owner);
                        owner.m_IsFrenzy = true;
                        owner.enemyData.MoraleValue -= 80;
                        int randomNum = Utility.Random.GetRandom(4, 8);
                        owner.m_Animator.SetInteger(AttackState, randomNum);
                    }
                    else
                    {
                        int randomNum = Utility.Random.GetRandom(4, 8);
                        owner.m_Animator.SetInteger(AttackState, randomNum);
                    }
                   
                }
                else
                {
                    if (owner.m_IsFrenzy)
                    {
                        int randomNum = Utility.Random.GetRandom(4, 8);
                        owner.m_Animator.SetInteger(AttackState, randomNum);
                    }
                    else
                    {
                        int randomNum = Utility.Random.GetRandom(8, 14);
                        owner.m_Animator.SetInteger(AttackState, randomNum);
                    }
                    
                }


            }
            else
            { 
                owner.ReduceAttackTime = m_SecondTakeAttackTime;
                owner.m_Animator.SetInteger(AttackState, 14);
            }
        }


        protected override void StopBlend(EnemyLogic owner)
        {
            if (owner.m_Animator.GetFloat(MoveBlend) > 0.8)
                owner.m_Animator.SetTrigger(m_stop);
            owner.m_Animator.SetFloat(MoveBlend, 0f);
            owner.m_Animator.SetFloat(FightBlend, 0f);
        }



    }
}
