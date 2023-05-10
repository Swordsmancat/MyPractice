using GameFramework;
using GameFramework.Fsm;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Farm.Hotfix
{
    /// <summary>
    /// 敌人脱战状态
    /// </summary>
    public class EnemyOutOfTheFight : EnemyBaseActionState
    {
        private readonly static float minDistance = 2f;
        private Quaternion m_MyQuaternion;
        private EnemyLogic owner;

        protected override void OnInit(IFsm<EnemyLogic> procedureOwner)
        {
            base.OnInit(procedureOwner);
        }

        protected override void OnEnter(IFsm<EnemyLogic> procedureOwner)
        {
            base.OnEnter(procedureOwner);
            owner = procedureOwner.Owner;
            owner.enemyData.HP = owner.enemyData.MaxHP;
           // Debug.Log("进入脱战状态");
        }

        protected override void OnUpdate(IFsm<EnemyLogic> procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);
            Vector3 positionNoY = new Vector3(owner.transform.position.x, 0, owner.transform.position.z);
            float distacne = (positionNoY - new Vector3(owner.m_NextPatrol.x, 0, owner.m_NextPatrol.z)).magnitude;
            if (distacne < 5f)
            {
                owner.m_Animator.SetTrigger("isIdle");
                ChangeState(procedureOwner, owner.ChangeStateEnemy(EnemyStateType.Idle));
                return;
            }
            else
            {
                ChangeState(procedureOwner, owner.ChangeStateEnemy(EnemyStateType.Motion));
                return;
            }
        }

        protected override void OnLeave(IFsm<EnemyLogic> fsm, bool isShutdown)
        {
            base.OnLeave(fsm, isShutdown);
            owner.transform.rotation = owner.OriginRotate;
        }

        public static EnemyOutOfTheFight Create()
        {
            EnemyOutOfTheFight state = ReferencePool.Acquire<EnemyOutOfTheFight>();
            return state;
        }

    }
}

