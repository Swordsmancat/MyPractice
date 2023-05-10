using GameFramework.Fsm;
using System.Collections;
using System.Collections.Generic;
using GameFramework;
using UnityEngine;
namespace Farm.Hotfix
{
    public class OrcDoubleAxeParryState : EnemyParryState
    {
        private readonly static int m_IsParry = Animator.StringToHash("IsParry");
        private readonly static int m_ParryOut = Animator.StringToHash("ParryOut");
        private readonly static int m_Hurt = Animator.StringToHash("Hurt");
        private readonly static int m_ParryHurtL = Animator.StringToHash("ParryHurtL");
        private readonly static int m_ParryHurtR = Animator.StringToHash("ParryHurtR");
        private readonly static int m_Counter = Animator.StringToHash("Counter");
        private static readonly int KnockedDown = Animator.StringToHash("KnockedDown");
        private static readonly int SkillAttack = Animator.StringToHash("SkillAttack");
        private float parryTime;
        private bool parryout;
        private EnemyLogic owner;
        private float currentEnergy;
        private int hurtNum = 0;
        private int hurtLoss;
        private OrcDoubleAxeLogic me;
        private bool isDown;
        protected override void OnInit(IFsm<EnemyLogic> procedureOwner)
        {
            base.OnInit(procedureOwner);
        }

        protected override void OnEnter(IFsm<EnemyLogic> procedureOwner)
        {
            base.OnEnter(procedureOwner);
            owner = procedureOwner.Owner;
            me = owner as OrcDoubleAxeLogic;
            //IsParry = owner.IsParry;
            owner.IsDefense = true;
            //Debug.Log("�����״̬");
            owner.HideTrail();//�ر���β�͹������ ��ֹ����������ɫ������һֱ����
            owner.EnemyAttackEnd();//ͬ��
            //Energy = EnemyLogic.Energy;
            //hurtNum = EnemyLogic.hurtNum;
            hurtLoss = Utility.Random.GetRandom(0, 6);
            EnemyParryStateStart(owner);
            owner.SetRichAiStop();

        }
        public static OrcDoubleAxeParryState Create()
        {
            OrcDoubleAxeParryState state = ReferencePool.Acquire<OrcDoubleAxeParryState>();
            return state;
        }
        protected override void OnUpdate(IFsm<EnemyLogic> procedureOwner, float elapseSeconds, float realElapseSeconds)
        {

            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);
            parryTime += Time.deltaTime;
            if (owner.underAttack)
            {
                ParryHurt();
            }


            OutParry(procedureOwner);




        }
        /// <summary>
        /// ����ֵ����
        /// </summary>
        /// <param name="maxNum"></param>
        /// <param name="minNum"></param>
        private void EnergyCalculate(int minNum, int maxNum)
        {
            int energyLoss = Utility.Random.GetRandom(minNum, maxNum);
            currentEnergy = owner.Energy - energyLoss;
            owner.Energy = currentEnergy > 0 ? currentEnergy : 0;
        }

        private void HurtTap()
        {
            if (owner.enemyData.TrunkValue > 0)
            {
                if (owner.enemyData.TrunkValue >= 20)
                {
                    owner.enemyData.TrunkValue -= 20;
                }
                else
                {
                    owner.enemyData.TrunkValue = 0;
                }
                GameHotfixEntry.HPBar.ShowTrunkValue(owner, owner.enemyData.TrunkRatio);
                owner.m_Animator.SetTrigger(m_Hurt);
            }
            else
            {
                parryout = true;
              
            }
           


        }
        private void HurtThump()
        {
            if (owner.enemyData.TrunkValue > 0)
            {
                if (owner.enemyData.TrunkValue >= 28)
                {
                    owner.enemyData.TrunkValue -= 28;
                }
                else
                {
                    owner.enemyData.TrunkValue = 0;
                }
                GameHotfixEntry.HPBar.ShowTrunkValue(owner, owner.enemyData.TrunkRatio);
                owner.m_Animator.SetTrigger(m_Hurt);
            }
            else
            {
                parryout = true;

            }


        }
        private void HurtOverwhelmed()
        {
            if (owner.enemyData.TrunkValue > 0)
            {
                if (owner.enemyData.TrunkValue >= 35)
                {
                    owner.enemyData.TrunkValue -= 35;
                }
                else
                {
                    owner.enemyData.TrunkValue = 0;
                }
                GameHotfixEntry.HPBar.ShowTrunkValue(owner, owner.enemyData.TrunkRatio);
                owner.m_Animator.SetTrigger(m_Hurt);
            }
            else
            {
                parryout = true;

            }

        }

        private void HurtSkill()
        {
            if (owner.enemyData.TrunkValue > 0)
            {
                if (owner.enemyData.TrunkValue >= 35)
                {
                    owner.enemyData.TrunkValue -= 35;
                }
                else
                {
                    owner.enemyData.TrunkValue = 0;
                }
                GameHotfixEntry.HPBar.ShowTrunkValue(owner, owner.enemyData.TrunkRatio);
                owner.m_Animator.SetTrigger(SkillAttack);
            }
            else
            {
                parryout = true;

            }

        }
        /// <summary>
        /// ���ܻ�
        /// </summary>
        private void ParryHurt()
        {
            //hurtNum += 1; 
            switch (me.m_BuffType)
            {
                case BuffType.None:
                    HurtTap();
                    break;
                case BuffType.Tap:
                    HurtTap();
                    break;
                case BuffType.Thump:
                    HurtThump();
                    break;
                case BuffType.Overwhelmed:
                    HurtOverwhelmed();
                    break;
                default:
                    HurtSkill();                  
                    break;
            }
            owner.underAttack = false;
            //toParry = false;

        }
        /// <summary>
        /// ���˳���ʽ
        /// </summary>
        /// <param name="procedureOwner"></param>
        private void OutParry(IFsm<EnemyLogic> procedureOwner)
        {
            //����������״̬
            if (parryTime >= 3f)
            {
                //IsParry = false;
                EnemyParryStateEnd(procedureOwner);
            }
            ////����������״̬
            else if (hurtNum > hurtLoss)
            {
                EnemyParryStateCounter(procedureOwner);
            }
            //�Ʒ�
            else if (parryout)
            {
                //owner.m_Animator.SetTrigger(m_ParryOut);
                EnemyParryOutStateEnd(procedureOwner);
            }
            else
            {
                if (parryTime > 3f)
                {
                    //SetAnimaSpeed();
                    //EnemyParryStateCounter(procedureOwner);
                    EnemyParryStateEnd(procedureOwner);
                }

            }

        }
        protected override void OnLeave(IFsm<EnemyLogic> fsm, bool isShutdown)
        {
            base.OnLeave(fsm, isShutdown);
            owner.IsParry = false;
            owner.IsDefense = false;
            owner.m_Animator.ResetTrigger(m_ParryHurtL);
            owner.m_Animator.ResetTrigger(m_ParryHurtR);
            owner.m_Animator.ResetTrigger(m_Hurt);
            owner.m_Animator.ResetTrigger(m_ParryOut);
            //hurtNum = 0;
            //owner.toParry = true;
            //owner.IsDefense = false;
            parryTime = 0;
            isDown = false;
            owner.underAttack = false;
            owner.m_Animator.SetBool(m_IsParry, false);
        }
        /// <summary>
        /// �м�״̬��ʼ
        /// </summary>
        protected override void EnemyParryStateStart(EnemyLogic owner)
        {

            owner.m_Animator.SetBool(m_IsParry, true);
            base.EnemyParryStateStart(owner);
            //owner.SetRichAiStop();
        }

        /// <summary>
        /// �м�״̬����
        /// </summary>
        /// <param name="procedureOwner"></param>
        protected override void EnemyParryStateEnd(IFsm<EnemyLogic> procedureOwner)
        {
            //Debug.Log("�����뿪�м�״̬");


            if (owner.Energy < 50)
            {
                ChangeState(procedureOwner, owner.ChangeStateEnemy(EnemyStateType.Motion));
                //owner.m_Animator.SetTrigger(m_Counter);
                //Debug.Log("�л��ƶ�״̬");
            }

            else
            {
                ChangeState(procedureOwner, owner.ChangeStateEnemy(EnemyStateType.Attack));
                //owner.m_Animator.SetTrigger(m_Counter);
                //Debug.Log("�л�����״̬");
            }
        }
        /// <summary>
        /// �мܷ���
        /// </summary>
        /// <param name="procedureOwner"></param>
        protected override void EnemyParryStateCounter(IFsm<EnemyLogic> procedureOwner)
        {
            owner.m_Animator.SetTrigger(m_Counter);
            owner.IsDefense = false;
            hurtNum = 0;
            ChangeState(procedureOwner, owner.ChangeStateEnemy(EnemyStateType.Attack));
            //base.EnemyParryStateCounter(procedureOwner);
        }
        /// <summary>
        /// �м��Ʒ��˳�
        /// </summary>
        /// <param name="procedureOwner"></param>
        protected override void EnemyParryOutStateEnd(IFsm<EnemyLogic> procedureOwner)
        {
            //info = owner.m_Animator.GetCurrentAnimatorStateInfo(0);
            Debug.Log("�Ʒ��뿪��״̬");
            owner.IsDefense = false;
                owner.m_Animator.SetTrigger(m_ParryOut);
                parryout = false;
                //owner.Energy += 1;
            if (isDown)
            {
                Debug.Log("����");
                //ChangeState<EnemyKnockedDownState>(procedureOwner);
                owner.isKnockedDown = true;
                owner.Buff.BuffTypeEnum = BuffType.None;
                owner.m_Animator.SetTrigger(KnockedDown);
                ChangeState(procedureOwner, owner.ChangeStateEnemy(EnemyStateType.Hurt));
                //ChangeState(procedureOwner, owner.ChangeStateEnemy(EnemyStateType.Block));
            }
            else
                ChangeState(procedureOwner, owner.ChangeStateEnemy(EnemyStateType.Motion));
            //if (info.IsName("���Ʒ�") && info.normalizedTime > 0.55f)
            //{

            //    //base.EnemyParryOutStateEnd(procedureOwner);
            //}

            //
        }
    }
}


