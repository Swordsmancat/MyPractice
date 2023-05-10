//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using GameFramework;
using GameFramework.DataTable;
using System.Collections.Generic;
using UnityGameFramework.Runtime;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;
using UnityEngine;
using GameFramework.Event;
using System;
using GameFramework.Procedure;

namespace Farm.Hotfix
{
    public class ProcedureMain : ProcedureBase
    {
        private const float GameOverDelayedSeconds = 2f;

        private readonly Dictionary<GameMode, GameBase> m_Games = new Dictionary<GameMode, GameBase>();
        private GameBase m_CurrentGame = null;
        private bool m_GotoMenu = false;
        private float m_GotoMenuDelaySeconds = 0f;


        private int? m_LockFormUIID;
        private int? m_GunAimUIID;
        private int? m_PlayerValueFormID;

        private LockForm m_LockForm;

        public GunAimForm m_GunAimForm;

        public PlayerValueForm m_PlayerValueForm;

        public override bool UseNativeDialog
        {
            get
            {
                return false;
            }
        }

        public void GotoMenu()
        {
            m_GotoMenu = true;
        }

        protected override void OnInit(ProcedureOwner procedureOwner)
        {
            base.OnInit(procedureOwner);

          //  m_Games.Add(GameMode.Survival, new SurvivalGame());
        }

        protected override void OnDestroy(ProcedureOwner procedureOwner)
        {
            base.OnDestroy(procedureOwner);

            m_Games.Clear();
        }

        private void LoadSkillData()
        {
            GameHotfixEntry.Skill.GetAllSkillData();
        }

        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);

            GameEntry.Event.Subscribe(OpenUIFormSuccessEventArgs.EventId, OnOpenUIFormSuccess);
            GameEntry.Event.Subscribe(OpenUIFormFailureEventArgs.EventId, OnOpenUIFormFailure);
            LoadSkillData();
            GameEntry.Entity.ShowMyPlayer(new MyPlayerData(GameEntry.Entity.GenerateSerialId(), 10000)
            {
                Name = "my Player",
                Position = new UnityEngine.Vector3(330, 28, 280),
                Scale = new UnityEngine.Vector3(1f, 1f, 1f)
            });
            IDataTable<DREnemy> dtEnemy = GameEntry.DataTable.GetDataTable<DREnemy>();
            //GameEntry.Entity.ShowEnemy(new EnemyData(GameEntry.Entity.GenerateSerialId(), 60005)
            //{
            //    Position = new UnityEngine.Vector3(55, 8, 45),
            //    Scale = new UnityEngine.Vector3(1.5f, 1.5f, 1.5f)
            //}, typeof(OrcLogic));
            //GameEntry.Entity.ShowEnemy(new EnemyData(GameEntry.Entity.GenerateSerialId(), 60061)
            //{
            //    Position = new UnityEngine.Vector3(55, 8, 55),
            //    Scale = new UnityEngine.Vector3(0.8f, 0.8f, 0.8f)
            //}, typeof(DragonideLogic));

            //GameEntry.Entity.ShowEnemy(new EnemyData(GameEntry.Entity.GenerateSerialId(), 60063)
            //{
            //    Position = new UnityEngine.Vector3(55, 8, 45),
            //    Scale = new UnityEngine.Vector3(1.3f, 1.3f, 1.3f)
            //}, typeof(WeregoatLogic));
            //int i = 0;
            //while (i < 5)
            //{
            //    int n = i + 3;
            //    GameEntry.Entity.ShowEnemy(new EnemyData(GameEntry.Entity.GenerateSerialId(), 60065)
            //    {
            //        Position = new UnityEngine.Vector3(350 + n, 28, 250 + n),
            //        Scale = new UnityEngine.Vector3(1f, 1f, 1f)
            //    }, typeof(GoatLogic));
            //    i++;
            //}

            ////GameEntry.Entity.ShowEnemy(new EnemyData(GameEntry.Entity.GenerateSerialId(), 60114)

            ////{
            ////    Position = new UnityEngine.Vector3(350, 50, 310),
            ////    Scale = new UnityEngine.Vector3(1.7f, 1.7f, 1.7f)
            ////}, typeof(OrcDoubleAxeLogic));

            //GameEntry.Entity.ShowEnemy(new EnemyData(GameEntry.Entity.GenerateSerialId(), 60115)

            //{
            //    Position = new UnityEngine.Vector3(340, 30, 350),
            //    Scale = new UnityEngine.Vector3(1.7f, 1.7f, 1.7f)
            //}, typeof(OrcDoubleAxeLogic));

            //GameEntry.Entity.ShowEnemy(new EnemyData(GameEntry.Entity.GenerateSerialId(), 60116)

            //{
            //    Position = new UnityEngine.Vector3(30, 30, 29),
            //    Scale = new UnityEngine.Vector3(1.6f, 1.6f, 1.6f)
            //}, typeof(CyclopLogic));

            //GameEntry.Entity.ShowEnemy(new EnemyData(GameEntry.Entity.GenerateSerialId(), 60117)

            //{
            //    Position = new UnityEngine.Vector3(330, 50, 330),
            //    Scale = new UnityEngine.Vector3(1.6f, 1.6f, 1.6f)
            //}, typeof(OrcDoubleAxeLogic));

            //GameEntry.Entity.ShowEnemy(new EnemyData(GameEntry.Entity.GenerateSerialId(), 60118)

            //{
            //    Position = new UnityEngine.Vector3(330, 50, 330),
            //    Scale = new UnityEngine.Vector3(2f, 2f, 2f)
            //}, typeof(OrcDoubleAxeLogic));

            GameEntry.Entity.ShowEnemy(new EnemyData(GameEntry.Entity.GenerateSerialId(), 60119)

            {
                Position = new UnityEngine.Vector3(350, 50, 310),
                Scale = new UnityEngine.Vector3(1.4f, 1.4f, 1.4f)
            }, typeof(OrcDoubleAxeLogic));

            //GameEntry.Entity.ShowEnemy(new EnemyData(GameEntry.Entity.GenerateSerialId(), 60120)

            //{
            //    Position = new UnityEngine.Vector3(350, 50, 310),
            //    Scale = new UnityEngine.Vector3(1.3f, 1.3f, 1.3f)
            //}, typeof(OrcDoubleAxeLogic));


            //GameEntry.Entity.ShowEnemy(new EnemyData(GameEntry.Entity.GenerateSerialId(), 60121)

            //{
            //    Position = new UnityEngine.Vector3(340, 30, 350),
            //    Scale = new UnityEngine.Vector3(1.8f, 1.8f, 1.8f)
            //}, typeof(OrcDoubleAxeLogic));

            //GameEntry.Entity.ShowEnemy(new EnemyData(GameEntry.Entity.GenerateSerialId(), 60122)

            //{
            //    Position = new UnityEngine.Vector3(330, 50, 330),
            //    Scale = new UnityEngine.Vector3(1.6f, 1.6f, 1.6f)
            //}, typeof(OrcDoubleAxeLogic));

            //GameEntry.Entity.ShowEnemy(new EnemyData(GameEntry.Entity.GenerateSerialId(), 60123)

            //{
            //    Position = new UnityEngine.Vector3(330, 50, 330),
            //    Scale = new UnityEngine.Vector3(1.8f, 1.8f, 1.8f)
            //}, typeof(OrcDoubleAxeLogic));

            //GameEntry.Entity.ShowEnemy(new EnemyData(GameEntry.Entity.GenerateSerialId(), 60124)

            //{
            //    Position = new UnityEngine.Vector3(330, 50, 330),
            //    Scale = new UnityEngine.Vector3(2f, 2f, 2f)
            //}, typeof(OrcDoubleAxeLogic));

            //GameEntry.Entity.ShowEnemy(new EnemyData(GameEntry.Entity.GenerateSerialId(), 60125)

            //{
            //    Position = new UnityEngine.Vector3(310, 50, 310),
            //    Scale = new UnityEngine.Vector3(1.3f, 1.3f, 1.3f)
            //}, typeof(OrcDoubleAxeLogic));

            //GameEntry.Entity.ShowEnemy(new EnemyData(GameEntry.Entity.GenerateSerialId(), 60126)

            //{
            //    Position = new UnityEngine.Vector3(330, 50, 330),
            //    Scale = new UnityEngine.Vector3(1.2f, 1.2f, 1.2f)
            //}, typeof(OrcDoubleAxeLogic));

            //GameEntry.Entity.ShowEnemy(new EnemyData(GameEntry.Entity.GenerateSerialId(), 60133)

            //{
            //    Position = new UnityEngine.Vector3(330, 50, 330),
            //    Scale = new UnityEngine.Vector3(1.8f, 1.8f, 1.8f)
            //}, typeof(OrcDoubleAxeLogic));

            //GameEntry.Entity.ShowEnemy(new EnemyData(GameEntry.Entity.GenerateSerialId(), 60128)

            //{
            //    Position = new UnityEngine.Vector3(330, 50, 330),
            //    Scale = new UnityEngine.Vector3(1.8f, 1.8f, 1.8f)
            //}, typeof(OrcDoubleAxeLogic));

            //GameEntry.Entity.ShowEnemy(new EnemyData(GameEntry.Entity.GenerateSerialId(), 60129)

            //{
            //    Position = new UnityEngine.Vector3(330, 50, 330),
            //    Scale = new UnityEngine.Vector3(1.8f, 1.8f, 1.8f)
            //}, typeof(OrcDoubleAxeLogic));

            //GameEntry.Entity.ShowEnemy(new EnemyData(GameEntry.Entity.GenerateSerialId(), 60130)

            //{
            //    Position = new UnityEngine.Vector3(330, 50, 330),
            //    Scale = new UnityEngine.Vector3(1.8f, 1.8f, 1.8f)
            //}, typeof(OrcDoubleAxeLogic));

            //GameEntry.Entity.ShowEnemy(new EnemyData(GameEntry.Entity.GenerateSerialId(), 60133)

            //{
            //    Position = new UnityEngine.Vector3(330, 50, 330),
            //    Scale = new UnityEngine.Vector3(1.8f, 1.8f, 1.8f)
            //}, typeof(OrcDoubleAxeLogic));

            //GameEntry.Entity.ShowEnemy(new EnemyData(GameEntry.Entity.GenerateSerialId(), 60137)

            //{
            //    Position = new UnityEngine.Vector3(330, 50, 330),
            //    Scale = new UnityEngine.Vector3(1.5f, 1.5f, 1.5f)
            //}, typeof(OrcDoubleAxeLogic));

            //GameEntry.Entity.ShowEnemy(new EnemyData(GameEntry.Entity.GenerateSerialId(), 60135)

            //{
            //    Position = new UnityEngine.Vector3(330, 50, 330),
            //    Scale = new UnityEngine.Vector3(2.5f, 2.5f, 2.5f)
            //}, typeof(QuadrupedLogic));

            //GameEntry.Entity.ShowEnemy(new EnemyData(GameEntry.Entity.GenerateSerialId(), 60136)

            //{
            //    Position = new UnityEngine.Vector3(330, 50, 330),
            //    Scale = new UnityEngine.Vector3(1.2f, 1.2f, 1.2f)
            //}, typeof(QuadrupedLogic));

            GameEntry.Entity.ShowEnemy(new EnemyData(GameEntry.Entity.GenerateSerialId(), 60137)

            {
                Position = new UnityEngine.Vector3(330, 50, 330),
                Scale = new UnityEngine.Vector3(1.7f, 1.7f, 1.7f)
            }, typeof(QuadrupedLogic));



            m_LockFormUIID = GameEntry.UI.OpenUIForm(UIFormId.UILock, this);
            m_GunAimUIID = GameEntry.UI.OpenUIForm(UIFormId.UIGunAim, this);
            m_PlayerValueFormID = GameEntry.UI.OpenUIForm(UIFormId.UIPlayerValue, this);
            //GameEntry.UI.OpenUIForm(UIFormId.ArenaForm, this);
            //   GameEntry.Entity.ShowEntity<PlayerLogic>()
            //   m_GotoMenu = false;
            //  GameMode gameMode = (GameMode)procedureOwner.GetData<VarByte>("GameMode").Value;
            // m_CurrentGame = m_Games[gameMode];
            //  m_CurrentGame.Initialize();
        }

        private void OnOpenUIFormFailure(object sender, GameEventArgs e)
        {

        }

        private void OnOpenUIFormSuccess(object sender, GameEventArgs e)
        {
            OpenUIFormSuccessEventArgs ne = (OpenUIFormSuccessEventArgs)e;
            if(ne.UserData != this)
            {
                return;
            }
            if(ne.UIForm.SerialId == m_PlayerValueFormID)
            {
                m_PlayerValueForm = (PlayerValueForm)ne.UIForm.Logic;
            }
            
            if (ne.UIForm.SerialId == m_GunAimUIID)
            {
                m_GunAimForm = (GunAimForm)ne.UIForm.Logic;
            }
            if (ne.UIForm.SerialId == m_LockFormUIID)
            {
                m_LockForm = (LockForm)ne.UIForm.Logic;
            }
        }

        public void SetMoraleValue(float morale)
        {
            if (m_PlayerValueForm != null)
            {
                m_PlayerValueForm.SetMoraleValue(morale);
            }
        }

        public void SetCourageValue(int value)
        {
            if (m_PlayerValueForm != null)
            {
                m_PlayerValueForm.SetCourageValue(value);
            }
        }

        public void SetPlayerValue(float hp,float trunk)
        {
            if(m_PlayerValueForm != null)
            {
                m_PlayerValueForm.SetPlayerValue(hp, trunk);
            }
        }
       public void HideLockIcon()
        {
            if (m_LockForm != null)
            {
                m_LockForm.HideLock();
            }
        }
        public void ShowLockIcon(Transform transform)
        {
            if (m_LockForm != null)
            {
                m_LockForm.ShowLock(transform);
            }
        }
        
        public void HideGunAimIcon()
        {
            if (m_GunAimForm != null)
            {
                m_GunAimForm.HideGunAim();
            }
        }

        public void ShowGunAimIcon()
        {
            if (m_GunAimForm != null)
            {
                m_GunAimForm.ShowGunAim();
            }
        }

        protected override void OnLeave(ProcedureOwner procedureOwner, bool isShutdown)
        {
            GameEntry.Event.Unsubscribe(OpenUIFormSuccessEventArgs.EventId, OnOpenUIFormSuccess);
            GameEntry.Event.Unsubscribe(OpenUIFormFailureEventArgs.EventId, OnOpenUIFormFailure);
            if (m_CurrentGame != null)
            {
                m_CurrentGame.Shutdown();
                m_CurrentGame = null;
            }

            base.OnLeave(procedureOwner, isShutdown);
        }

        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);

            //if (m_CurrentGame != null && !m_CurrentGame.GameOver)
            //{
            //    m_CurrentGame.Update(elapseSeconds, realElapseSeconds);
            //    return;
            //}

            //if (!m_GotoMenu)
            //{
            //    m_GotoMenu = true;
            //    m_GotoMenuDelaySeconds = 0;
            //}

            //m_GotoMenuDelaySeconds += elapseSeconds;
            //if (m_GotoMenuDelaySeconds >= GameOverDelayedSeconds)
            //{
            //    procedureOwner.SetData<VarInt32>("NextSceneId", GameEntry.Config.GetInt("Scene.Menu"));
            //    ChangeState<ProcedureChangeScene>(procedureOwner);
            //}
        }
    }
}
