using UnityEngine;
using GameFramework.Fsm;
using ProcedureOwner = GameFramework.Fsm.IFsm<Farm.Hotfix.EnemyLogic>;
using GameFramework;
using UnityGameFramework.Runtime;

namespace Farm.Hotfix
{
    public class OrcDoubleAxeOutOfTheFight : EnemyOutOfTheFight
    {
        private readonly static float minDistance = 2f;
        private Quaternion m_MyQuaternion;
        private EnemyLogic owner; 
        private readonly static int PutDownWeapon = Animator.StringToHash("PutDownWeapon");

        protected override void OnInit(IFsm<EnemyLogic> procedureOwner)
        {
            base.OnInit(procedureOwner);
        }

        protected override void OnEnter(IFsm<EnemyLogic> procedureOwner)
        {
            base.OnEnter(procedureOwner);
            owner = procedureOwner.Owner;
            owner.enemyData.HP = owner.enemyData.MaxHP;
            Debug.Log("½øÈëÍÑÕ½×´Ì¬");
            if (owner.m_IsTakeOutWeapon)
            {
                owner.m_Animator.SetTrigger(PutDownWeapon);
                owner.m_IsTakeOutWeapon = false;
            }

        }

        protected override void OnUpdate(IFsm<EnemyLogic> procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);
            owner.RestoreEnergy();
            Vector3 positionNoY = new Vector3(owner.transform.position.x, 0, owner.transform.position.z);
            float distacne = (positionNoY - new Vector3(owner.m_NextPatrol.x, 0, owner.m_NextPatrol.z)).magnitude;
            if (distacne < 5f)
            {
                owner.m_Animator.SetTrigger("isIdle");
                ChangeState(procedureOwner, owner.ChangeStateEnemy(EnemyStateType.Idle));
            }
            else
            {
                ChangeState(procedureOwner, owner.ChangeStateEnemy(EnemyStateType.Motion));
            }
        }
        protected override void OnLeave(IFsm<EnemyLogic> fsm, bool isShutdown)
        {
            base.OnLeave(fsm, isShutdown);
            owner.transform.rotation = owner.OriginRotate;
        }
        public static OrcDoubleAxeOutOfTheFight Create()
        {
            OrcDoubleAxeOutOfTheFight state = ReferencePool.Acquire<OrcDoubleAxeOutOfTheFight>();
            return state;
        }
    }

}
