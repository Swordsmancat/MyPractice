using GameFramework.DataTable;
using GameFramework.Resource;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;
using System.Linq;
using System.Collections;
using GameFramework.Fsm;
using GameFramework.Event;
using System;
using DG.Tweening;

namespace Farm.Hotfix
{
   public abstract class TargetableObject:Entity
    {
        [SerializeField]
        private TargetableObjectData m_targetableObjectData = null;
        public Animator m_Animator;
        private bool m_IsCanBreak =true;

        [SerializeField]
        private List<WeaponLogic> m_Weapons = new List<WeaponLogic>();  //武器逻辑

        public List<WeaponLogic> Weapons { get => m_Weapons; set => m_Weapons = value; }   //武器逻辑

        private bool m_Invulnerable;

        public float invulnerabiltyTime=0.1f;

        public float m_timeSinceLastHit = 0.0f;

        public bool IsDefense = false;

        private Dictionary<int, float> m_SkillCD;

        private Buff m_Buff;

        private bool m_IsInvincible = false;

        private bool m_InitEvent = false;

        private bool m_IsShoulderStrike = false;

        private ColliderState m_ColliderState = ColliderState.None;

        private ColliderState m_GetCollider = ColliderState.None;

        private bool m_IsRebound = false;

        private bool m_GetRebound = false;

        private EquiState m_GetReboundEqui;

        private bool m_IsReboundHeavy = false;

        private GPAttack m_GPAttack = GPAttack.None;

        private GPAttack m_GetGP = GPAttack.None;

        private float m_TrunkRecoveryTime =10f;

        private float m_TrunkWaitTime;

        private bool m_IsTrunkRecovery = false;

        private float m_TrunkRecoveryPastTime;
        private float m_DefauftRecoveryTime = 0.2f;//躯干为空恢复速度

        private float m_HitPastTime;
        private float m_DefaultHitTime = 2f;//躯干不为空受击恢复间隔
        private bool m_GetHit = false;
        private float m_DefaultTrunkRecoveryTime = 0.005f;//躯干不为空恢复速度

        private bool m_IsWeak;
        private bool m_IsBreak;
        private bool m_IsCanWeakBreak =true;
        private float m_BreakWaitTime;
        private float m_BreakWaitDefaultTime = 2f;
        private static readonly int WeakBreak = Animator.StringToHash("WeakBreak");

        private ProcedureMain m_ProcedureMain;

        private BuffType m_GetBuffType;

        private bool m_HurtEnd;

        private float m_StutterFrameTime;

        private bool m_IgnoreParry;

        private bool m_IgnoreRebound;

        private int m_TakeTrunkValue;

        private ReboundState m_ReboundState;

        private ReboundState m_GetReboundState;

        private bool m_IsGetCrit;

        public bool IsGetCrit
        {
            get
            {
                return m_IsGetCrit;
            }
            set
            {
                m_IsGetCrit = value;
            }
        }

        public bool IsBreak
        {
            get
            {
                return m_IsBreak;
            }
        }




        public ReboundState ReboundState
        {
            get
            {
                return m_ReboundState;
            }
            set
            {
                m_ReboundState = value;
            }
        }


        public ReboundState GetReboundState
        {
            get
            {
                return m_GetReboundState;
            }
            set
            {
                m_GetReboundState = value;
            }
        }

        public int TakeTrunkValue
        {
            get
            {
                return m_TakeTrunkValue;
            }
            set
            {
                m_TakeTrunkValue = value;
            }
        }

        public bool IgnoreRebound
        {
            get
            {
                return m_IgnoreRebound;
            }
            set
            {
                m_IgnoreRebound = value;
            }
        }

        public bool IgnoreParry
        {
            get
            {
                return m_IgnoreParry;
            }
            set
            {
                m_IgnoreParry = value;
            }
        }

        public float StutterFrameTime
        {
            get
            {
                return m_StutterFrameTime;
            }
            set
            {
                m_StutterFrameTime = value;
            }
        }

        public bool HurtEnd
        {
            get
            {
                return m_HurtEnd;
            }
            set
            {
                m_HurtEnd = value;
            }
        }

        public BuffType GetBuffType
        {
            get
            {
                return m_GetBuffType;
            }
            set
            {
                m_GetBuffType = value;
            }
        }


        public bool IsWeak
        {
            get
            {
                return m_IsWeak;
            }
        }

        public GPAttack GPAttack
        {
            get
            {
                return m_GPAttack;
            }
            set
            {
                m_GPAttack = value;
            }
        }

        public GPAttack GetGP
        {
            get
            {
                return m_GetGP;
            }
            set
            {
                m_GetGP = value;
            }
        }

        public bool IsReboundHeavy
        {
            get
            {
                return m_IsReboundHeavy;
            }
            set
            {
                m_IsReboundHeavy = value;
            }
        }
        public EquiState GetReboundEqui
        {
            get
            {
                return m_GetReboundEqui;
            }
            set
            {
                m_GetReboundEqui = value;
            }
        }

        public bool GetRebound
        {
            get
            {
                return m_GetRebound;
            }
            set
            {
                m_GetRebound = value;
            }
        }

        

        public bool IsRebound
        {
            get
            {
                return m_IsRebound;
            }
            set
            {
                m_IsRebound = value;
            }
        }
        public ColliderState GetCollider
        {
            get
            {
                return m_GetCollider;
            }
            set
            {
                m_GetCollider = value;
            }
        }
        public ColliderState ColliderState
        {
            get
            {
                return m_ColliderState;
            }
            set
            {
                m_ColliderState = value;
            }
        }
        public bool IsShoulderStrike
        {
            get
            {
                return m_IsShoulderStrike;
            }
            set
            {
                m_IsShoulderStrike = value;
            }
        }
        public bool InitEvent
        {
            get
            {
                return m_InitEvent;
            }
            set
            {
                m_InitEvent = value;
            }
        }

        public bool Invulnerable
        {
            get
            {
                return m_Invulnerable;
            }
            set
            {
                m_Invulnerable = value;
            }
        }

        public Buff Buff
        {
            get
            {
               return m_Buff;
            }
            set
            {
                m_Buff = value;
            }
        }

        /// <summary>
        /// int 技能ID ，float技能CD时间;
        /// </summary>
        public Dictionary<int, float> SkillCD
        {
            get
            {
                return m_SkillCD;
            }
            set
            {
                m_SkillCD = value;
            }
        }

        public bool IsCanBreak
        {
            get
            {
                return m_IsCanBreak;
            }
            set
            {
                m_IsCanBreak = value;
            }
        }

        public TargetableObjectData TargetableObjectData
        {
            get
            {
                return m_targetableObjectData;
            }
        }

        public bool IsDead
        {
            get
            {
                return m_targetableObjectData.HP <= 0;
            }
        }

        public abstract ImpactData GetImpactData();

        public virtual void ApplyDamage(Entity attacker, Entity attackType, int damageHP,int damageTrunk,Vector3 weapon)
        {
            //if (Invulnerable)
            //{
            //    return;
            //}
            float fromHPRatio = m_targetableObjectData.HPRatio;
            m_targetableObjectData.HP -= damageHP;
            float toHPRatio = m_targetableObjectData.HPRatio;

            float fromTrunkRatio = m_targetableObjectData.TrunkRatio;
            m_targetableObjectData.TrunkValue -= damageTrunk;
            m_GetHit = true;
            m_HitPastTime = 0;
            if (IsWeak)
            {
                m_targetableObjectData.VertigoValue -= damageTrunk*3;
            }
            else
            {
                m_targetableObjectData.VertigoValue -= damageTrunk;
            }
            float toTrunkRatio = m_targetableObjectData.TrunkRatio;
            if (fromHPRatio > toHPRatio)
            {
                GameHotfixEntry.HPBar.ShowHPBar(this, fromHPRatio, toHPRatio,fromTrunkRatio,toTrunkRatio);
            }
            // isInvulnerable = true;
            if (m_targetableObjectData.HP <= 0)
            {
                if(attacker != null)
                    OnDead(attacker,weapon);
            }
        }

        public virtual void ApplySkillDamage(Entity attacker ,int damageHP)
        {
            float fromHPRatio = m_targetableObjectData.HPRatio;
            m_targetableObjectData.HP -= damageHP;
            float toHPRatio = m_targetableObjectData.HPRatio;
            if (fromHPRatio > toHPRatio)
            {
               // GameHotfixEntry.HPBar.ShowHPBar(this, fromHPRatio, toHPRatio);
            }
            if (m_targetableObjectData.HP <= 0)
            {
               // OnDead(attacker);
            }
        }

        public virtual void ApplyEnergy(Entity attacker ,int damageMP)
        {
            float fromMPRatio = m_targetableObjectData.MPRatio;
            m_targetableObjectData.MP -= damageMP;
            float toMPRatio = m_targetableObjectData.MPRatio;
            if(fromMPRatio > toMPRatio)
            {
                //蓝条
            }

        }


        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
            //if (Invulnerable)
            //{
            //    m_timeSinceLastHit += Time.deltaTime;
            //    if (m_timeSinceLastHit > invulnerabiltyTime)
            //    {
            //        m_timeSinceLastHit = 0f;
            //        Invulnerable = false;

            //    }
            //}
            foreach (int item in m_SkillCD.Keys.ToList())
            {
                if (m_SkillCD[item] > 0)
                {
                    m_SkillCD[item] -= elapseSeconds;
                }
                else
                {
                    m_SkillCD[item] = 0;
                }
            }
            if (m_GetHit)
            {
                m_HitPastTime += Time.deltaTime;
               
            }
           
            if (m_IsTrunkRecovery)
            {
                m_targetableObjectData.TrunkValue = m_targetableObjectData.MaxTrunk;
                if (this is PlayerLogic)
                {
                    m_ProcedureMain.SetPlayerValue(m_targetableObjectData.HPRatio, m_targetableObjectData.TrunkRatio);
                }
                else
                {
                    GameHotfixEntry.HPBar.ShowTrunkValue(this, m_targetableObjectData.TrunkRatio);
                }
                m_IsTrunkRecovery = false;
                //m_TrunkRecoveryPastTime += Time.deltaTime;
                //m_targetableObjectData.TrunkValue = Mathf.Lerp(0, m_targetableObjectData.MaxTrunk, m_TrunkRecoveryPastTime * m_DefauftRecoveryTime * m_targetableObjectData.HPRatio);
                //if(this is PlayerLogic)
                //{
                //    m_ProcedureMain.SetPlayerValue(m_targetableObjectData.HPRatio, m_targetableObjectData.TrunkRatio);
                //}
                //else
                //{
                //     GameHotfixEntry.HPBar.ShowTrunkValue(this, m_targetableObjectData.TrunkRatio);
                //}
                //if (m_targetableObjectData.TrunkValue >= m_targetableObjectData.MaxTrunk)
                //{
                //    m_TrunkRecoveryPastTime = 0;
                //    m_IsTrunkRecovery = false;
                //}
            }
            else
            {
                if (m_targetableObjectData.TrunkValue > 0)
                {
                    if (m_HitPastTime >= m_DefaultHitTime)
                    {
                        m_GetHit = false;
                        m_targetableObjectData.TrunkValue = Mathf.Lerp(m_targetableObjectData.TrunkValue, m_targetableObjectData.MaxTrunk, m_DefaultTrunkRecoveryTime);
                        if (this is PlayerLogic)
                        {
                            m_ProcedureMain.SetPlayerValue(m_targetableObjectData.HPRatio, m_targetableObjectData.TrunkRatio);
                        }
                        else
                        {
                            GameHotfixEntry.HPBar.ShowTrunkValue(this, m_targetableObjectData.TrunkRatio);
                        }
                    }
                }
                
            }

            if (m_targetableObjectData.TrunkValue <= 0)
            {
                m_IsWeak = true;
                m_IsBreak = true;
                m_TrunkWaitTime += elapseSeconds;
                if(m_TrunkWaitTime >= m_TrunkRecoveryTime)
                {
                    m_IsWeak = false;
                    m_IsTrunkRecovery = true;
                    m_TrunkWaitTime = 0;
                }
                if (m_IsCanWeakBreak)
                {
                    m_IsCanWeakBreak = false;
                    m_Animator.SetTrigger(WeakBreak);
                }
            }
        
            if (m_IsBreak)
            {
                if (m_targetableObjectData.TrunkValue >= m_targetableObjectData.MaxTrunk)
                {
                    m_IsCanWeakBreak = true;
                    m_IsBreak = false;
                }
            }
        
           

        }

        


        protected override void OnInit(object userData)
        {  
            base.OnInit(userData);
            m_Animator = GetComponent<Animator>();
            if(m_Animator == null)
            {
                m_Animator = GetComponentInChildren<Animator>();
            }
            m_SkillCD = new Dictionary<int, float>();
            m_Buff = new Buff();
            //gameObject.SetLayerRecursively(Constant.Layer.TargetableObjectLayerId);
        }


        protected override void OnShow(object userData)
        {
            base.OnShow(userData);
            m_targetableObjectData = userData as TargetableObjectData;
            if(m_targetableObjectData == null)
            {
                Log.Error("Targetable object data is invalid.");
                return;
            }
            m_ProcedureMain = (ProcedureMain)GameEntry.Procedure.CurrentProcedure;
        }

        protected virtual void OnDead(Entity attacker,Vector3 point)
        {
            GameEntry.Entity.HideEntity(this);
        }

        protected override void OnHide(bool isShutdown, object userData)
        {
            base.OnHide(isShutdown, userData);
        }


        protected virtual void OnTriggerEnter(Collider other)
        {
            TargetableObject entity;
            if (other.gameObject.tag == "BodyCollider")
            {
                entity = other.gameObject.GetComponent<ColliderOwner>().m_Owner.gameObject.GetComponent<TargetableObject>();

                if (m_targetableObjectData.Id == entity.Id)
                {
                    return;
                }
                else
                {
                    if (entity != null)
                    {
                        other.gameObject.SetActive(false);
                    }
                }
            }
            else
            {
                entity = other.gameObject.GetComponent<TargetableObject>();
            }
            if(entity == null)
            {
                return;
            }
            

            //if (entity.Id >= Id)
            //{
            //    // 碰撞事件由 Id 小的一方处理，避免重复处理
            //    return;
            //}
            Vector3 point = other.bounds.ClosestPoint(transform.position);

            AIUtility.PerformCollisionAttack(entity, this, point);
            
            // AIUtility.PerformCollision(this, entity);
        }

    }
}
