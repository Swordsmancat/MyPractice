using UnityEngine;
using GameFramework.Fsm;
using ProcedureOwner = GameFramework.Fsm.IFsm<Farm.Hotfix.EnemyLogic>;
using GameFramework;
using UnityGameFramework.Runtime;

namespace Farm.Hotfix
{
    /// <summary>
    /// 敌人搜寻状态
    /// </summary>
    public class EnemySeekState : EnemyBaseActionState
    {
        private EnemyLogic owner;
        private float m_WaitTime;
        private float m_DefaultWaitTime = 5f;
        private bool m_IsMove = false;
        private int nextMoveIndex = 0;
        protected override void OnInit(ProcedureOwner procedureOwner)
        {
            base.OnInit(procedureOwner);

        }

        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);
            owner = procedureOwner.Owner;
        }

        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);

            if (owner.IsLocking)
            {
                if (owner.LockingEntity == null || owner.IsDead)
                {
                    ChangeState(procedureOwner, owner.ChangeStateEnemy(EnemyStateType.Idle));
                    return;
                }
                float distance = AIUtility.GetDistance(owner, owner.LockingEntity);

                //if (owner.CheckInAttackRange(distance))
                //{
                //    if (owner.IsCanAttack)
                //    {
                //        //Log.Info("在攻击范围内");
                //        ChangeState(procedureOwner, owner.ChangeStateEnemy(EnemyStateType.Attack));
                //        return;
                //    }
                //}

                if (owner.CheckInSeekRange(distance))
                {
                    if (procedureOwner.CurrentState.GetType() != owner.ChangeStateEnemy(EnemyStateType.Motion))
                    {
                        ChangeState(procedureOwner, owner.ChangeStateEnemy(EnemyStateType.Motion));
                        return;

                    }
                }
                else
                {
                    //超过视野范围进入脱战状态;
                    //Log.Info("解除锁定");
                    // owner.UnLockEntity();
                    owner.IsLocking = false;
                    //ChangeState(procedureOwner, owner.ChangeStateEnemy(EnemyStateType.Idle));
                    ChangeState(procedureOwner, owner.ChangeStateEnemy(EnemyStateType.OutOfFight));
                    return;
                }
                return;
            }



            CampType camp = owner.GetImpactData().Camp;
            if (owner.find_Player != null && !owner.find_Player.IsDead && AIUtility.GetRelation(owner.find_Player.GetImpactData().Camp, camp) == RelationType.Hostile)
            {
                float distance = AIUtility.GetDistance(owner, owner.find_Player);
                float angle = AIUtility.GetAngleInSeek(owner, owner.find_Player);
                if (owner.CheckInSeekAngle(angle))
                {
                    if (owner.CheckInAttackRange(distance))
                    {
                        owner.LockEntity(owner.find_Player);
                        ChangeState(procedureOwner, owner.ChangeStateEnemy(EnemyStateType.Attack));
                        return;
                    }
                    else if (owner.CheckInSeekRange(distance))
                    {
                        owner.LockEntity(owner.find_Player);
                        ChangeState(procedureOwner, owner.ChangeStateEnemy(EnemyStateType.Motion));
                        return;
                    }
                }


            }
            if (owner.enemyData.PatrolList != null)
            {
                if (!m_IsMove)
                {
                    m_IsMove = true;
                    owner.m_NextPatrol = owner.enemyData.PatrolList[nextMoveIndex];
                    ChangeState(procedureOwner, owner.ChangeStateEnemy(EnemyStateType.Motion));
                    return;
                }
                else
                {
                    
                    float distance = (new Vector3(owner.gameObject.transform.position.x,0,owner.gameObject.transform.position.z) - new Vector3(owner.enemyData.PatrolList[nextMoveIndex].x,0,owner.enemyData.PatrolList[nextMoveIndex].z)).magnitude;
                    if (distance < 2)
                    {
                        m_WaitTime += elapseSeconds;
                        if (m_WaitTime > m_DefaultWaitTime)
                        {
                            if (nextMoveIndex < owner.enemyData.PatrolList.Count-1)
                            {
                                nextMoveIndex++;
                            }
                            else
                            {
                                nextMoveIndex = 0;
                            }
                            m_WaitTime = 0;
                            m_IsMove = false;
                        }
                        else
                        {
                            ChangeState(procedureOwner, owner.ChangeStateEnemy(EnemyStateType.Idle));
                            return;
                        }

                    }
                }
            }
           
            //if (owner.enemyData.PatrolList != null)
            //{
            //    for (int i = 0; i < owner.enemyData.PatrolList.Count;)
            //    {
            //        owner.SetRichAIMove();
            //        owner.SetSearchTargetPosition(owner.enemyData.PatrolList[i]);
            //        //  ChangeState(procedureOwner, owner.ChangeStateEnemy(EnemyStateType.Motion));
            //        float distance = AIUtility.GetDistance(owner, owner.find_Player);
            //        if (distance < 5)
            //        {
            //            m_WaitTime += elapseSeconds;
            //            if (m_WaitTime > m_DefaultWaitTime)
            //            {
            //                i++;
            //                if (i >= owner.enemyData.PatrolList.Count)
            //                {
            //                    i = 0;
            //                }
            //            }

            //        }
            //    }
            //}




            ////GameObject[] gameObjects = GameObject.FindGameObjectsWithTag(PlayerTag);
            ////for (int i = 0; i < gameObjects.Length; i++)
            ////{
            ////    PlayerLogic player = gameObjects[i].GetComponent<PlayerLogic>();
            ////    if (!player.IsDead && AIUtility.GetRelation(player.GetImpactData().Camp, camp) == RelationType.Hostile)
            ////    {
            ////        float distance = AIUtility.GetDistance(owner, player);
            ////        float angle = AIUtility.GetAngleInSeek(owner, player);
            ////        if (owner.CheckInSeekAngle(angle))
            ////        {
            ////            if (owner.CheckInAttackRange(distance))
            ////            {
            ////                Log.Info("有敌人进入攻击范围");
            ////                // ChangeState<EnemyAttackState>(procedureOwner);
            ////                ChangeState(procedureOwner, owner.ChangeStateEnemy(EnemyStateType.Attack));
            ////                break;
            ////            }
            ////            else if (owner.CheckInSeekRange(distance))
            ////            {
            ////                Log.Info("有敌人进入视野范围");
            ////                owner.LockEntity(player);
            ////               // ChangeState<EnemyMotionState>(procedureOwner);
            ////                ChangeState(procedureOwner, owner.ChangeStateEnemy(EnemyStateType.Motion));
            ////                break;
            ////            }

            ////        }
            ////    }
            ////}



        }

        protected override void OnLeave(ProcedureOwner fsm, bool isShutdown)
        {
            base.OnLeave(fsm, isShutdown);
        }

        /// <summary>
        /// 锁定敌人时(idle和motion都继承至EnemySeekState若需要重写,则两个子状态都要重写此函数)
        /// </summary>
        /// <param name="procedureOwner"></param>
        protected virtual void LockPlayerDo(ProcedureOwner procedureOwner)
        {
            owner.LockEntity(owner.find_Player);
            ChangeState(procedureOwner, owner.ChangeStateEnemy(EnemyStateType.Motion));
        }
    }
}
