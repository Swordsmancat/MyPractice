using GameFramework.Fsm;
using System;
using System.Collections;
using UnityGameFramework.Runtime;
using UnityEngine;
using GameFramework;
using System.Collections.Generic;
using DG.Tweening;
using FIMSpace.FLook;
using GameFramework.Event;

namespace Farm.Hotfix
{
    public class PlayerLogic : TargetableObject
    {
        public static PlayerLogic Instance;
        [SerializeField]
        private PlayerData m_PlayerData = null;

        public PlayerData PlayerData => m_PlayerData;


        public WeaponInfo m_WeaponInfo;
        public SkillInfo m_SkillInfo;

        private List<WeaponData> weaponDatas;

        [SerializeField]
        private List<Armor> m_Armors = new List<Armor>();

        public IFsm<PlayerLogic> fsm;                     //状态机
        public List<FsmState<PlayerLogic>> stateList;     //状态机状态列表
        public List<Armor> Armors => m_Armors;


        public WeaponLogicLeftHand LeftHand;
        public WeaponLogicRightHand RightHand;


        public GameObject m_Attacker;               //攻击者

        public float MoveX = 0;

        public float MoveY = 0;

        public bool isDodge = false;                //翻滚
        public bool isInvincible = false;
        //public bool isDefense = false;              //防御
        public bool isRun = false;                  //跑
        public bool isMotion = false;               //运动状态
        public bool isWeaponState = false;          //角色进入武器切换状态
        public bool isKnockedDown = false;          //被击倒
        public bool isFourthAtk = false;            //角色技能（当时是把普通攻击第四招扯出来撤出来设为特殊攻击 后面改为技能）
        public bool isSecondSkill = false;          //角色第二段技能
        public bool isHurt = false;                 //角色受伤
        public bool isChangeWeapon = false;         //更换武器
        public bool isStep = false;                 //角色滑步（垫步）
        public bool IsParry = false;                //是否招架(拼刀)
        public bool isCanStepAtk = false;           //角色垫步后是否可以执行垫步攻击
        public bool isAim = false;                  //瞄准怪物
        public bool isThump = false;                //重击(现在的技能 没改名字)
        public bool isShoulder = false;             //肩撞无敌
        public bool isPushAttack = false;
        public bool isGunAim = false;               //角色是否举枪瞄准
        public bool isBehindAtked = false;          //角色是否从背后受到攻击
        public bool isFocusEngy = false;            //角色蓄力 按下攻击键为true 抬起后为false
        public int attackCount = 0;                 //第几招普攻
        public bool isSpecialAtk = false;           //特殊大招？(用于顿帧)
        private bool isHands = true;                //武器是否在手上
        public bool IsHands { get => isHands; set => isHands = value; }
        private bool isShift = false;               //角色是否冲刺
        public float shiftSpeed;                   //冲刺速度
        public Vector3 attackDir;
        public bool m_AnimationEventGetUp;
        public BuffType m_BuffType; //受击状态
        public bool isStoic;  //受否为霸体
        public bool underAttack;  //防御受击
        public bool IsProduceSF;


        public int TakeMoralevalue = 0;



        public MoveBehaviour m_moveBehaviour;

        private ColliderControl m_ColliderControl;

        public BoxCollider m_LeftWeaponCollider;

        public BoxCollider m_RightWeaponCollider;

        public AnimatorStateInfo currentStateInfo;

        private FLookAnimator m_lookAnimator;       //头看向怪物插件


        //===================== Attack =====================//
        private bool m_WeaponAttack;
        private WaitForSeconds m_AttackInputWait;
        private WaitForSeconds m_AttackHoldWait;
        private Coroutine m_AttackWaitCoroutine;
        private Coroutine m_AttackHoldWaitCoroutine;

        private float m_AttackInputDuration = 0.3f;
        private float m_AttackHoldDuration = 1f;
        public bool m_Attack;
        public bool m_IsAttackTap;
        public bool m_IsAttackThump;
        public bool m_Explosion;
        public bool m_DoubleClick;
        public bool m_IsAttackJump;
        public bool Whirlwind;
        private int m_FocusCount = 0;

        public float m_ClickAttackBtnDuration;      // 按下攻击键的持续时间
        public bool m_IsStartCount;                 // 是否开始计时
        private float comboTime;                    //记录连续攻击时间
        private bool isCombo = false;               //是否可以连击 用于大剑肩撞之后一段时间内可衔接普攻
        private float R_HoldTime;
        private float L_HoldTime;
        private float NeedHoldTime = 0.4f;

        private bool m_BowInit;
        private bool m_IsOnBowAim;


        private Transform m_ArrorTransform;
        private Vector3 m_ArrowImpulse;
        private Vector3 m_DefaultImpulse = new Vector3(2, 5, 80);

        private Vector3 m_CameraDir;

        private Transform m_BowShootPoint;
        private Vector3 m_ArrowPosition;
        private Vector3 m_AppearPosition;
        private Vector3 m_ArrowRotate;
        private int m_ArrowEntityID = 0;

        [SerializeField]
        private WeaponAttackPoint m_WeaponAttackPoint_L = null;
        [SerializeField]
        private WeaponAttackPoint m_WeaponAttackPoint_R = null;
        public WeaponAttackPoint WeaponAttackPoint_R => m_WeaponAttackPoint_R;

        #region LockField
        private int currentLockEnemyIndex = 0;
        private int chnageLockCount = 1;

        private List<GameObject> m_FrontEnemyObj;

        private int frontEnemyNum = 0;
        public EnemyLogic currentLockEnemy;

        public bool m_LockEnemy;

        #endregion 

        public ProcedureMain m_ProcedureMain;

        private float m_ReboundInterval;
        private float m_ReboundIntervaldefault = 0.5f;
        private bool m_IsCanRebound = true;

        private float m_GPInterval;
        private float m_GPIntervaldefault = 0.5f;
        private bool m_IsCanGP = true;
        private int m_NeedCourageValue = 85;


        private EquiState m_EquiState = EquiState.SwordShield;

        public EquiState EquiState
        {
            get
            {
                return m_EquiState;
            }
        }


        private bool firstShow = true;

        float m_DodgeX, m_DodgeY;
        private static readonly int m_Rebound = Animator.StringToHash("Rebound");
        private static readonly int ParryBreak = Animator.StringToHash("ParryBreak");
        private static readonly int m_ReboundMoraleValue = Animator.StringToHash("ReboundMorale");
        private static readonly int m_ReboundHeavy = Animator.StringToHash("ReboundHeavy");
        private static readonly int m_DodgeDirX = Animator.StringToHash("DodgeDirX");
        private static readonly int m_DodgeDirY = Animator.StringToHash("DodgeDirY");
        private static readonly int m_Courage = Animator.StringToHash("CourageValue");
        private static readonly int m_GetCourage = Animator.StringToHash("GetCourage");
        private static readonly int m_DodgeStep = Animator.StringToHash("DodgeStep");
        private static readonly int IsLockEnemy = Animator.StringToHash("IsLockEnemy");


        private float m_MPRestoreMaxDefaultTime = 0.007f;
        private float m_MPDefaultWaitTime = 1f;
        private float m_MPWaitTime;
        public bool m_MPStartRestore = false;
        public bool m_TakeWeaponFinish = false;
        public bool m_PutDownWeaponFinish = false;

        private bool m_IsInArrowSpecial = false;

        private float stopFrameTime;          // 盾反顿帧时间
        private float stopFrameSize;        // 盾反顿帧强度


        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            Instance = this;
            m_moveBehaviour = GetComponent<MoveBehaviour>();
            stateList = new List<FsmState<PlayerLogic>>();
            m_AttackInputWait = new WaitForSeconds(m_AttackInputDuration);
            m_AttackHoldWait = new WaitForSeconds(m_AttackHoldDuration);
        }

        protected override void OnDead(Entity attacker, Vector3 point)
        {
            base.OnDead(attacker, point);
        }

        protected override void OnShow(object userData)
        {
            base.OnShow(userData);
            m_PlayerData = userData as PlayerData;
            if (m_PlayerData == null)
            {
                Log.Error("Player data is invalid.");
                return;
            }
            Name = Utility.Text.Format("Player {0}", Id);
            weaponDatas = m_PlayerData.GetAllWeaponDatas();
            //for (int i = 0; i < weaponDatas.Count; i++)
            //{
            //   // GameEntry.Entity.ShowWeapon(weaponDatas[i]);
            //    GameEntry.Entity.ShowWeapon(weaponDatas[i],EntityExtension.WeaponHand.Right);

            //}

            // GameEntry.Entity.ShowWeapon(weaponDatas[0], EntityExtension.WeaponHand.Right);
            // GameEntry.Entity.ShowWeapon(weaponDatas[1], EntityExtension.WeaponHand.Left);
            m_ProcedureMain = (ProcedureMain)GameEntry.Procedure.CurrentProcedure;
            List<ArmorData> armorDatas = m_PlayerData.GetAllArmorDatas();
            m_FrontEnemyObj = new List<GameObject>();
            for (int i = 0; i < armorDatas.Count; i++)
            {
                GameEntry.Entity.ShowArmor(armorDatas[i]);
            }
            SetWeaponInfo();
            CreateFsm();
            m_lookAnimator = GetComponent<FLookAnimator>();
            m_ColliderControl = GetComponent<ColliderControl>();
            m_moveBehaviour.SetCameraGruop();
        }

        private void SetWeaponInfo()
        {
            m_WeaponInfo = gameObject.AddComponent<WeaponInfo>();
            m_WeaponInfo.Init();

            m_SkillInfo = gameObject.AddComponent<SkillInfo>();
            m_SkillInfo.Init();
        }

        protected override void OnAttached(EntityLogic childEntity, Transform parentTransform, object userData)
        {
            base.OnAttached(childEntity, parentTransform, userData);
            isChangeWeapon = true;

            if (childEntity is WeaponLogicLeftHand)
            {
                Weapons.Add((WeaponLogicLeftHand)childEntity);
                m_WeaponAttackPoint_L = childEntity.GetComponent<WeaponAttackPoint>();
                m_LeftWeaponCollider = childEntity.GetComponent<BoxCollider>();
                LeftHand = (WeaponLogicLeftHand)childEntity;
                return;
            }
            if (childEntity is WeaponLogicRightHand)
            {
                Weapons.Add((WeaponLogicRightHand)childEntity);
                m_WeaponAttackPoint_R = childEntity.GetComponent<WeaponAttackPoint>();
                m_RightWeaponCollider = childEntity.GetComponent<BoxCollider>();
                RightHand = (WeaponLogicRightHand)childEntity;
                return;
            }
            if (childEntity is Armor)
            {
                m_Armors.Add((Armor)childEntity);
                return;
            }
        }

        protected override void OnDetached(EntityLogic childEntity, object userData)
        {
            base.OnDetached(childEntity, userData);
            if (childEntity is WeaponLogic)
            {
                Weapons.Remove((WeaponLogic)childEntity);
                return;
            }

            if (childEntity is Armor)
            {
                m_Armors.Remove((Armor)childEntity);
                return;
            }
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
            if (m_LockEnemy)
            {
                m_Animator.SetBool(IsLockEnemy, true);
            }

            else
            {

                m_Animator.SetBool(IsLockEnemy, false);
            }
            if (firstShow)
            {
                GameEntry.Entity.ShowWeapon(weaponDatas[0], EntityExtension.WeaponHand.Right);
                GameEntry.Entity.ShowWeapon(weaponDatas[1], EntityExtension.WeaponHand.Left);
                firstShow = false;
            }

            PlayerInteractive();
            PlayerShiftInUpdate(isShift, shiftSpeed);

            if (!m_IsCanRebound)
            {
                m_ReboundInterval += Time.deltaTime;
                if (m_ReboundInterval > m_ReboundIntervaldefault)
                {
                    m_ReboundInterval = 0f;
                    m_IsCanRebound = true;

                }
            }

            if (!m_IsCanGP)
            {
                m_GPInterval += Time.deltaTime;
                if (m_GPInterval > m_GPIntervaldefault)
                {
                    m_GPInterval = 0f;
                    m_IsCanGP = true;
                }
            }
            if (Input.GetKeyDown(KeyCode.Alpha9))
            {
                PlayerHurtEnd();
            }
            if (m_MPStartRestore)
            {
                m_MPWaitTime += elapseSeconds;
                if(m_MPWaitTime > m_MPDefaultWaitTime)
                {
                   m_PlayerData.MP = Mathf.Lerp(m_PlayerData.MP, m_PlayerData.MaxMP, m_MPRestoreMaxDefaultTime);
                    if (m_PlayerData.MPRatio < 0.95f)
                    {
                        GameHotfixEntry.EnergyBar.ShowEnergyBar(this, m_PlayerData.MPRatio);
                    }
                }
            }
            else
            {
                m_MPWaitTime = 0;
            }

        }

        protected override void OnHide(bool isShutdown, object userData)
        {
            base.OnHide(isShutdown, userData);
            firstShow = true;
            DestroyFsm();
        }


        private void PlayerInteractive()
        {

            // 采用GetAxisRaw 因为锁定后 需要灵活控制方向移动 GetAxis使用起来方向不灵活
            m_DodgeX = Input.GetAxisRaw("Horizontal");
            m_DodgeY = Input.GetAxisRaw("Vertical");
            MoveX = Input.GetAxis("Horizontal");
            MoveY = Input.GetAxis("Vertical");

            m_Animator.SetFloat("DodgeX", m_DodgeX);
            m_Animator.SetFloat("DodgeY", m_DodgeY);
            m_Animator.SetInteger(m_DodgeDirX, (int)m_DodgeX);
            m_Animator.SetInteger(m_DodgeDirY, (int)m_DodgeY);

            m_Animator.SetFloat("MoveX", MoveX);
            m_Animator.SetFloat("MoveY", MoveY);

            if (m_moveBehaviour.m_Horizontal != 0 || m_moveBehaviour.m_Vertical != 0)
            {
                if (InputManager.IsAccelerate() && !IsDefense)
                {
                    if (!isRun)
                    {
                        isRun = true;
                        isFourthAtk = false;
                    }

                }
                else
                {
                    if (isRun)
                    {
                        isRun = false;
                    }
                }
            }

            //TODO:整合攻击方式。
                switch (EquiState)
                {
                    case EquiState.None:
                        break;
                    case EquiState.SwordShield:
                        SwordShieldAttack();
                        SwordShieldDefense();
                        break;
                    case EquiState.GiantSword:
                        GiantSwordAttack();
                        SwordShieldDefense();
                        break;
                    case EquiState.Katana:
                        DaggerAttack();
                        SwordShieldDefense();
                        break;
                    case EquiState.DoubleBlades:
                        DoubleBladesAttack();
                        SwordShieldDefense();
                        break;
                    case EquiState.RevengerDoubleBlades:
                        DoubleBladesAttack();
                        SwordShieldDefense();
                        break;
                    case EquiState.Bow:
                        BowAttack();
                        break;
                case EquiState.Gunner:
                    BowAttack();
                    break;
                case EquiState.Pike:
                    DaggerAttack();
                    SwordShieldDefense();
                    break;
                    default:
                        break;
                }
                //if (!IsDefense && !isHurt)
                if (!isHurt)
                {
                    if (InputManager.IsClickDodge())
                    {
                        isDodge = true;
                    }
                }
            
           
            //================= Dod&Step =================//
          
            if (InputManager.IsClickStep())
            {
                isStep = true;
            }

            if (InputManager.IsClickCourage())
            {
                if (m_PlayerData.MoraleValue >= m_NeedCourageValue)
                {
                    m_PlayerData.CourageValue++;
                    m_PlayerData.MoraleValue -= m_NeedCourageValue;
                    m_Animator.SetInteger(m_Courage, m_PlayerData.CourageValue);
                    m_Animator.SetTrigger(m_GetCourage);
                    m_ProcedureMain.SetMoraleValue(m_PlayerData.MoraleValue * 0.01f);
                    m_ProcedureMain.SetCourageValue(m_PlayerData.CourageValue);
                }
            }

            //================= Lock =================//
            if (InputManager.IsClickLock())
            {
                if(EquiState == EquiState.Bow ||EquiState ==EquiState.Gunner)
                {
                    if (!m_IsOnBowAim)
                    {
                        m_moveBehaviour.BowAimOn();
                        m_Animator.SetBool("IsAim", true);
                        m_IsOnBowAim = true;

                    }
                    else
                    {
                        m_moveBehaviour.BowAimOff();
                        m_Animator.SetBool("IsAim", false);
                        m_IsOnBowAim = false;
                    }
                }
                else
                {
                    OnSetLock();
                }               
            }

            LockOnEnemyDead();


            ChangeWeapon();

            // GameObject.FindGameObjectsWithTag(NPCTag);
#if UNITY_EDITOR
            InputManager.IsOnGame();
#endif 
            isMotion = IsDefaultAnimPlay();
            RecordCombon();
        }

        private void ChangeWeapon()
        {
            //TODO:切换武器读表
            EquiState m_Equi = InputManager.ChangeWeapon();
            if(m_EquiState != m_Equi)
            {
                switch (m_Equi)
                {
                    case EquiState.None:
                        break;
                    case EquiState.SwordShield:
                        isWeaponState = true;
                        m_EquiState = EquiState.SwordShield;
                        ChangeWeaponRight(30000);
                        ChangeWeaponLeft(30002);
                        ExitBowAim();
                        break;
                    case EquiState.GiantSword:
                        Whirlwind = false;
                        isWeaponState = true;
                        m_EquiState = EquiState.GiantSword;
                        ChangeWeaponRight(30012);
                        DestroyWeapon(30002);
                        ExitBowAim();
                        break;
                    case EquiState.Katana:
                        isWeaponState = true;
                        m_EquiState = EquiState.Katana;
                        ChangeWeaponRight(30006);
                        DestroyWeapon(30002);
                        ExitBowAim();
                        break;
                    case EquiState.DoubleBlades:
                        m_Explosion = false;
                        isWeaponState = true;
                        m_EquiState = EquiState.DoubleBlades;
                        ChangeWeaponRight(30011);
                        ChangeWeaponLeft(30010);
                        ExitBowAim();
                        break;
                    case EquiState.Bow:
                        isWeaponState = true;
                        m_EquiState = EquiState.Bow;
                        CleanHandleWeapon();
                        ChangeWeaponLeft(30009);
                        ChangeWeaponRight(30016);
                        UnLockEnemy();
                        break;
                    case EquiState.Gunner:
                        isWeaponState = true;
                        m_EquiState = EquiState.Gunner;
                        CleanHandleWeapon();
                        ChangeWeaponRight(30077);
                        UnLockEnemy();
                        break;
                    case EquiState.Pike:
                        isWeaponState = true;
                        m_EquiState = EquiState.Pike;
                        CleanHandleWeapon();
                        ChangeWeaponRight(30078);
                        ExitBowAim();
                        break;
                    case EquiState.RevengerDoubleBlades:
                        break;
                    default:
                        break;
                }
            }
        }

        private void ExitBowAim()
        {
            m_moveBehaviour.BowAimOff();
            m_Animator.SetBool("IsAim", false);
            m_IsOnBowAim = false;
        }

        private void GiantSwordAttack()
        {
            if (!Whirlwind)
            {
                if (InputManager.IsWhirlwind())
                {
                    Whirlwind = true;
                    m_Animator.SetTrigger("Whirlwind");
                    Whirlwind = false;
                }
            }
            if (m_PlayerData.MP > 0)
            {
                if (InputManager.IsClickDownMouseLeft() || InputManager.IsClickDownMouseRight() || InputManager.IsClickHoldMouseRight() || InputManager.IsClickHoldMouserLeft() || InputManager.IsClickUpMouseLeft() || InputManager.IsClickUpMouseRight() && !isHurt)
                {
                    if (InputManager.IsClickHoldMouseRight())
                    {
                        R_HoldTime += Time.deltaTime;
                    }
                    else
                    {
                        R_HoldTime = 0;
                    }

                    if (InputManager.IsClickHoldMouserLeft())
                    {
                        L_HoldTime += Time.deltaTime;
                    }
                    else
                    {
                        L_HoldTime = 0;
                    }

                    if (R_HoldTime > NeedHoldTime && InputManager.IsClickHoldMouseRight())
                    {
                        m_Animator.SetInteger("R_HoldClick", 1);// -1为停止蓄力 0为左键蓄力，1为右键蓄力
                    }
                    else
                    {
                        m_Animator.SetInteger("R_HoldClick", 0);
                    }

                    if (L_HoldTime > NeedHoldTime && InputManager.IsClickHoldMouserLeft())
                    {
                        m_Animator.SetInteger("L_HoldClick", 1);
                    }
                    else
                    {
                        m_Animator.SetInteger("L_HoldClick", 0);
                    }

                    m_Animator.SetInteger("FocusCount", m_FocusCount);



                    if (InputManager.IsClickDownMouseLeft() || InputManager.IsClickDownMouseRight())
                    {
                        if (m_AttackWaitCoroutine != null)
                        {
                            StopCoroutine(m_AttackWaitCoroutine);
                        }
                        m_AttackWaitCoroutine = StartCoroutine(AttackWait());
                    }
                    else
                    {
                        if (m_AttackHoldWaitCoroutine != null)
                        {
                            StopCoroutine(m_AttackHoldWaitCoroutine);
                        }
                        m_AttackHoldWaitCoroutine = StartCoroutine(AttackHoldWait());
                    }
                }
            }

            if (m_WeaponAttack)
            {
                m_WeaponAttackPoint_R.SetAttackPoint();
                // m_WeaponAttackPoint_L.SetAttackPoint();
            }
        }


        private void BowInit()
        {
            m_BowShootPoint = FindTools.FindFunc<Transform>(transform, "LeftHand");
            m_BowInit = true;
        }
        private void BowAttack()
        {
            if (m_PlayerData.MP > 0)
            {
                if (InputManager.IsClickDownMouseLeft() || InputManager.IsClickDownMouseRight() || InputManager.IsClickHoldMouseRight() || InputManager.IsClickHoldMouserLeft() || InputManager.IsClickUpMouseLeft() || InputManager.IsClickUpMouseRight() && !isHurt)
                {
                    if (!m_BowInit)
                    {
                        BowInit();
                    }
                    if (InputManager.IsClickHoldMouserLeft())
                    {
                        L_HoldTime += Time.deltaTime;
                    }
                    else
                    {
                        L_HoldTime = 0;
                    }

                    if (L_HoldTime > NeedHoldTime && InputManager.IsClickHoldMouserLeft())
                    {
                        m_Animator.SetInteger("L_HoldClick", 1);
                    }
                    else
                    {
                        m_Animator.SetInteger("L_HoldClick", 0);
                    }

                    if (InputManager.IsClickDownMouseLeft() || InputManager.IsClickDownMouseRight())
                    {
                        if (m_AttackWaitCoroutine != null)
                        {
                            StopCoroutine(m_AttackWaitCoroutine);
                        }
                        m_AttackWaitCoroutine = StartCoroutine(AttackWait());
                    }
                    else
                    {
                        if (m_AttackHoldWaitCoroutine != null)
                        {
                            StopCoroutine(m_AttackHoldWaitCoroutine);
                        }
                        m_AttackHoldWaitCoroutine = StartCoroutine(AttackHoldWait());
                    }

                }
            }


        }
        private void DaggerAttack()
        {
            if (m_PlayerData.MP > 0)
            {
                if (InputManager.IsClickDownMouseLeft() || InputManager.IsClickDownMouseRight() || InputManager.IsClickHoldMouseRight() || InputManager.IsClickHoldMouserLeft() || InputManager.IsClickUpMouseLeft() || InputManager.IsClickUpMouseRight() && !isHurt)
                {
                    if (InputManager.IsClickHoldMouseRight())
                    {
                        R_HoldTime += Time.deltaTime;
                    }
                    else
                    {
                        R_HoldTime = 0;
                    }

                    if (InputManager.IsClickHoldMouserLeft())
                    {
                        L_HoldTime += Time.deltaTime;
                    }
                    else
                    {
                        L_HoldTime = 0;
                    }

                    if (R_HoldTime > NeedHoldTime && InputManager.IsClickHoldMouseRight())
                    {
                        m_Animator.SetInteger("R_HoldClick", 1);// -1为停止蓄力 0为左键蓄力，1为右键蓄力
                    }
                    else
                    {
                        m_Animator.SetInteger("R_HoldClick", 0);
                    }

                    if (L_HoldTime > NeedHoldTime && InputManager.IsClickHoldMouserLeft())
                    {
                        m_Animator.SetInteger("L_HoldClick", 1);
                    }
                    else
                    {
                        m_Animator.SetInteger("L_HoldClick", 0);
                    }

                    if (InputManager.IsClickDownMouseLeft() || InputManager.IsClickDownMouseRight())
                    {
                        if (m_AttackWaitCoroutine != null)
                        {
                            StopCoroutine(m_AttackWaitCoroutine);
                        }
                        m_AttackWaitCoroutine = StartCoroutine(AttackWait());
                    }
                    else
                    {
                        if (m_AttackHoldWaitCoroutine != null)
                        {
                            StopCoroutine(m_AttackHoldWaitCoroutine);
                        }
                        m_AttackHoldWaitCoroutine = StartCoroutine(AttackHoldWait());
                    }

                }
            }

            if (m_Attack)
            {
                m_moveBehaviour.isAttack = true;
            }
            else
            {
                m_moveBehaviour.isAttack = false;
            }
            if (m_WeaponAttack)
            {
                m_WeaponAttackPoint_R.SetAttackPoint();
                // m_WeaponAttackPoint_L.SetAttackPoint();
            }
        }
        public void RestoreEnergy()
        {
            float currentEnergy;
            //if (Energy < 100)
            currentEnergy = PlayerData.MP + (20 * Time.deltaTime);
            PlayerData.MP = currentEnergy > PlayerData.MaxMP ? PlayerData.MP : currentEnergy;
            GameHotfixEntry.EnergyBar.ShowEnergyBar(this, m_PlayerData.MPRatio);

        }
        private void SwordShieldDefense()
        {

            if (InputManager.IsClickDefense())
            {

                if (m_PlayerData.MP > 0)
                {
                    if (!isKnockedDown)
                    {
                        IsDefense = true;
                    }
                }

            }
            else
            {
                IsDefense = false;
                m_Animator.SetBool("Defense", false);
            }
        }

        private void SwordShieldAttack()
        {
            if (m_PlayerData.MP > 0)
            {
                if (InputManager.IsClickDownMouseLeft() || InputManager.IsClickDownMouseRight() || InputManager.IsClickHoldMouseRight() || InputManager.IsClickHoldMouserLeft() || InputManager.IsClickUpMouseLeft() || InputManager.IsClickUpMouseRight() && !isHurt)
                {
                    if (InputManager.IsClickHoldMouseRight())
                    {
                        R_HoldTime += Time.deltaTime;

                    }
                    else
                    {
                        R_HoldTime = 0;
                    }

                    if (InputManager.IsClickHoldMouserLeft())
                    {
                        L_HoldTime += Time.deltaTime;
                    }
                    else
                    {
                        L_HoldTime = 0;
                    }


                    if (R_HoldTime > NeedHoldTime && InputManager.IsClickHoldMouseRight())
                    {
                        m_Animator.SetInteger("R_HoldClick", 1);// 0为停止蓄力 1为蓄力
                    }
                    else
                    {
                        m_Animator.SetInteger("R_HoldClick", 0);
                    }

                    if (L_HoldTime > NeedHoldTime && InputManager.IsClickHoldMouserLeft())
                    {
                        m_Animator.SetInteger("L_HoldClick", 1);
                    }
                    else
                    {
                        m_Animator.SetInteger("L_HoldClick", 0);
                    }


                    if (InputManager.IsClickDownMouseLeft() || InputManager.IsClickDownMouseRight())
                    {
                        if (m_AttackWaitCoroutine != null)
                        {
                            StopCoroutine(m_AttackWaitCoroutine);
                        }
                        m_AttackWaitCoroutine = StartCoroutine(AttackWait());

                    }
                    else
                    {
                        if (m_AttackHoldWaitCoroutine != null)
                        {
                            StopCoroutine(m_AttackHoldWaitCoroutine);
                        }
                        m_AttackHoldWaitCoroutine = StartCoroutine(AttackHoldWait());
                    }

                }
            }


            if (m_Attack)
            {
                m_moveBehaviour.isAttack = true;
            }
            else
            {
                m_moveBehaviour.isAttack = false;
            }
            if (m_WeaponAttack)
            {
                m_WeaponAttackPoint_R.SetAttackPoint();
                // m_WeaponAttackPoint_L.SetAttackPoint();
            }
        }
        private IEnumerator ExplosionChange()
        {
            //延迟0.5秒切换武器
            yield return new WaitForSecondsRealtime(0.5f);
            isWeaponState = true;
            m_EquiState = EquiState.RevengerDoubleBlades;
            ChangeWeaponRight(30011);
            ChangeWeaponLeft(30010);
        }

        private void DoubleBladesAttack()
        {
            if (!m_Explosion)
            {
                if (InputManager.IsExplosion())
                {
                    m_Explosion = true;
                    m_Animator.SetTrigger("TwoSwordsBuff");
                    StartCoroutine(ExplosionChange());
                }
            }

            if (MoveY < 0)
            {
                m_Animator.SetBool("ClickBack", true);
            }
            else
            {
                m_Animator.SetBool("ClickBack", false);
            }
            if (m_PlayerData.MP > 0)
            {
                if (InputManager.IsClickDownMouseLeft() || InputManager.IsClickDownMouseRight() && !isHurt)
                {
                    if (m_AttackWaitCoroutine != null)
                    {
                        StopCoroutine(m_AttackWaitCoroutine);
                    }
                    m_AttackWaitCoroutine = StartCoroutine(AttackWait());
                }
            }

            if (m_Attack)
            {
                m_moveBehaviour.isAttack = true;
            }
            else
            {
                m_moveBehaviour.isAttack = false;
            }
            if (m_WeaponAttack)
            {
                m_WeaponAttackPoint_R.SetAttackPoint();
                m_WeaponAttackPoint_L.SetAttackPoint();
            }
        }

        private bool IsDefaultAnimPlay()
        {
            if (m_Animator.GetCurrentAnimatorClipInfoCount(3) >= 1)
            {
                return true;
            }
            return false;
        }

        #region LockLogic
        public void OnSetLock()
        {
            if (m_LockEnemy)
            {
                if (chnageLockCount < m_FrontEnemyObj.Count)
                {
                    int i = GetNextLockIndex();
                    currentLockEnemy = m_FrontEnemyObj[i].GetComponent<EnemyLogic>();
                    m_moveBehaviour.SetLockLookAt(currentLockEnemy.LockTransform);
                    if (m_ProcedureMain != null)
                    {
                        m_ProcedureMain.HideLockIcon();
                    }
                    if (m_ProcedureMain != null)
                    {
                        m_ProcedureMain.ShowLockIcon(currentLockEnemy.LockTransform);
                    }
                    m_LockEnemy = true;
                    m_lookAnimator.SetLookTarget(currentLockEnemy.transform);
                }
                else
                {
                    m_moveBehaviour.UnLockLookAt();
                    if (m_ProcedureMain != null)
                    {
                        m_ProcedureMain.HideLockIcon();
                    }
                    currentLockEnemy = null;
                    m_lookAnimator.SetLookTarget(null);
                    m_LockEnemy = false;
                }
            }
            else
            {
                GetFrontEnemy();
                frontEnemyNum = m_FrontEnemyObj.Count;
                if (frontEnemyNum > 0)
                {
                    currentLockEnemy = m_FrontEnemyObj[GetNearestEnemyIndex()].GetComponent<EnemyLogic>();
                    m_moveBehaviour.SetLockLookAt(currentLockEnemy.LockTransform);
                    if (m_ProcedureMain != null)
                    {
                        m_ProcedureMain.ShowLockIcon(currentLockEnemy.LockTransform);
                    }
                    //  TakeOutWeapon();//空手状态锁定敌人自动拿出武器
                    m_LockEnemy = true;
                    m_lookAnimator.SetLookTarget(currentLockEnemy.transform);
                }
            }
        }

        private void UnLockEnemy()
        {
            m_moveBehaviour.UnLockLookAt();
            if (m_ProcedureMain != null)
            {
                m_ProcedureMain.HideLockIcon();
            }
            currentLockEnemy = null;
            m_lookAnimator.SetLookTarget(null);
            m_LockEnemy = false;
        }

        private void LockOnEnemyDead()
        {
            if (currentLockEnemy != null && currentLockEnemy.IsDead)
            {
                OnSetLock();
            }
        }
        private int GetNextLockIndex()
        {
            chnageLockCount++;
            currentLockEnemyIndex++;
            return currentLockEnemyIndex % m_FrontEnemyObj.Count;
        }

        private void GetFrontEnemy()
        {
            chnageLockCount = 1;
            m_FrontEnemyObj.Clear();
            GameObject[] enemyObjects = GameObject.FindGameObjectsWithTag("Enemy");
            for (int i = 0; i < enemyObjects.Length; i++)
            {
                float distance = AIUtility.GetDistance(enemyObjects[i].GetComponent<Entity>(), this);
                if (distance < Constant.PlayerParameter.LOCK_DISTANCE)
                {
                    Vector3 relativePosition = enemyObjects[i].transform.position - transform.position;
                    // float result = Vector3.Dot(m_moveBehaviour.playerCamera.transform.forward, relativePosition);
                    float result = Vector3.Dot(m_moveBehaviour.playerCamera.transform.forward, relativePosition);
                    if (result > 0)
                    {
                        m_FrontEnemyObj.Add(enemyObjects[i]);
                    }
                }
            }
        }

        private int GetNearestEnemyIndex()
        {
            int m_index = 0;
            float indexDistance = 0;
            for (int i = 0; i < m_FrontEnemyObj.Count; i++)
            {
                float distance = AIUtility.GetDistance(m_FrontEnemyObj[i].GetComponent<Entity>(), this);
                if (i == 0)
                {
                    indexDistance = distance;
                }
                else
                {
                    if (indexDistance > distance)
                    {
                        m_index = i;
                        indexDistance = distance;
                    }
                }
            }
            return currentLockEnemyIndex = m_index;
        }

        #endregion

        #region About put and take weapon

        public void PutDownWeapon()
        {

            if (!isHands)
            {
                return;
            }
            m_PutDownWeaponFinish = false;
            IsHands = !IsHands;
            m_Animator.SetBool("IsHand", IsHands);
            m_Animator.SetTrigger("PutOrTakeTrigger");
        }

        public void TakeOutWeapon()
        {

            if (IsHands)
            {
                return;
            }
            IsHands = !IsHands;
            m_Animator.SetBool("IsHand", IsHands);
            m_Animator.SetTrigger("PutOrTakeTrigger");
        }

        public void TakeOutWeaoinFinish()
        {
            m_TakeWeaponFinish = true;
        }

        public void PutDownWeaoinFinish()
        {
            m_PutDownWeaponFinish = true;
        }

        public void TakeOutWeaponWhenAtk()
        {
            if (IsHands)
            {
                return;
            }
            IsHands = !IsHands;
            m_Animator.SetBool("IsHand", IsHands);
        }

        #endregion

        #region About ChangeWeaponLogic
        private void ChangeWeaponRight(int weaponId)
        {
            var oldEntity = GameEntry.Entity.GetEntity(weaponDatas[0].Id);
            if (oldEntity != null)
            {
                GameEntry.Entity.DetachEntity(weaponDatas[0].Id);
                GameEntry.Entity.HideEntity(oldEntity);
                //oldEntity.OnHide(false,null);
            }
            weaponDatas[0] = new WeaponData(m_ArrowEntityID = GameEntry.Entity.GenerateSerialId(), weaponId, Id, CampType.Player);
            GameEntry.Entity.ShowWeapon(weaponDatas[0], EntityExtension.WeaponHand.Right);
        }

        public void HideHandArrow()
        {
            if (GameEntry.Entity.HasEntity(m_ArrowEntityID))
            {
                GameEntry.Entity.HideEntity(m_ArrowEntityID);
            }
        }

        public void ShowHandArrow()
        {
            if (!GameEntry.Entity.HasEntity(m_ArrowEntityID))
            {
                GameEntry.Entity.ShowWeapon(weaponDatas[0], EntityExtension.WeaponHand.Right);
            }
        }
        private void ChangeWeaponLeft(int weaponId)
        {
            var oldEntity = GameEntry.Entity.GetEntity(weaponDatas[1].Id);
            if (oldEntity != null)
            {
                GameEntry.Entity.DetachEntity(weaponDatas[1].Id);
                GameEntry.Entity.HideEntity(oldEntity);
                //oldEntity.OnHide(false, null);
            }
            weaponDatas[1] = new WeaponData(GameEntry.Entity.GenerateSerialId(), weaponId, Id, CampType.Player);
            GameEntry.Entity.ShowWeapon(weaponDatas[1], EntityExtension.WeaponHand.Left);
        }

        private void CleanHandleWeapon()
        {
            for (int i = 0; i < weaponDatas.Count; i++)
            {
                var thisEntity = GameEntry.Entity.GetEntity(weaponDatas[i].Id);
                if (thisEntity == null)
                {
                    return;
                }
                GameEntry.Entity.DetachEntity(thisEntity);
                GameEntry.Entity.HideEntity(thisEntity);
            }
        }
        private void DestroyWeapon(int weaponId)
        {
            var thisEntity = GameEntry.Entity.GetEntity(weaponDatas[1].Id);
            if (thisEntity == null)
            {
                return;
            }
            GameEntry.Entity.DetachEntity(weaponDatas[1].Id);
            GameEntry.Entity.HideEntity(thisEntity);
        }
        #endregion

        #region Coroutine

        private IEnumerator AttackHoldWait()
        {
            m_Attack = true;
            yield return m_AttackHoldWait;
            m_FocusCount = 0;
            m_Animator.SetInteger("FocusCount", m_FocusCount);
            m_Attack = false;
        }

        private IEnumerator AttackWait()
        {
                m_Attack = true;


            if (InputManager.IsClickDownMouseLeft() && InputManager.IsClickDownMouseRight())
            {
                m_DoubleClick = true;
                m_IsAttackTap = false;
                m_IsAttackThump = false;
                m_IsAttackJump = false;

            }
            else if (InputManager.IsClickDownMouseRight())
            {
                m_IsAttackThump = true;
                m_IsAttackTap = false;
                m_DoubleClick = false;
                m_IsAttackJump = false;

            }
            else if (InputManager.IsClickDownMouseLeft() || InputManager.IsClickUpMouseLeft())
            {
                m_IsAttackTap = true;
                m_IsAttackThump = false;
                m_DoubleClick = false;
                m_IsAttackJump = false;
                Whirlwind = false;
            }
            else if (InputManager.IsClickDownMouseLeft() && Input.GetKeyDown(KeyCode.W))
            {
                m_IsAttackTap = false;
                m_IsAttackThump = false;
                m_DoubleClick = false;
                m_IsAttackJump = true;

            }

            yield return m_AttackInputWait;
            m_DoubleClick = false;
            m_IsAttackTap = false;
            m_IsAttackThump = false;
            m_IsAttackJump = false;
            //Whirlwind = false;
            m_Attack = false;
        }

        //用于垫步完一定时间内可释放特殊攻击-垫步攻击 （目前好像没有垫步了）
        public void StartCoro(float m_time)
        {
            StartCoroutine(StepToAtkWait(m_time));
        }

        // 角色垫步后一秒内可执行特殊攻击
        private IEnumerator StepToAtkWait(float duringTime)
        {
            isCanStepAtk = true;

            yield return new WaitForSeconds(duringTime);
            isCanStepAtk = false;
        }

        // 角色蓄力冲刺
        public IEnumerator ShiftWait(float time)
        {
            isShift = true;
            yield return new WaitForSeconds(time);
            isShift = false;
        }


        #endregion

        #region About Atked

        //判断敌人攻击的方向 用于处理向背后倒还是向前倒
        private bool EnemyAtkedDir(Entity attacker)
        {
            Entity player = gameObject.GetComponent<TargetableObject>();
            float angle = AIUtility.GetAngleInSeek(player, attacker);
            if (angle > 110)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void RecordCombon()
        {
            if (!isCombo) return;
            comboTime += Time.deltaTime;
            if (comboTime >= 3.5f)
            {
                isCombo = false;
                attackCount = 0;
            }
        }
        #endregion

        #region InAnimation Event


        public void SetPlayerInArrowSpecialOn()
        {
            m_IsInArrowSpecial = true;
        }

        public void SetPlayerInArrowSpecialOff()
        {
            m_IsInArrowSpecial = false;
        }

        public bool IsPlayerInArrowSpecial()
        {
            return m_IsInArrowSpecial;
        }
        public void SetDodgeStepOn()
        {
            m_Animator.SetBool(m_DodgeStep, true);
        }

        public void SetDodgeStepOff()
        {
            m_Animator.SetBool(m_DodgeStep, false);
        }

        public void TakeEnergyValue(float value)
        {
            if (m_PlayerData.MP >= value)
            {
                m_PlayerData.MP -= value;
            }
            else
            {
                m_PlayerData.MP = 0;
            }
            GameHotfixEntry.EnergyBar.ShowEnergyBar(this, m_PlayerData.MPRatio);
        }
        public void DodgeInvincibleStart()
        {
            isInvincible = true;
        }

        public void DodgeInvincibleEnd()
        {
            isInvincible = false;
        }
        public void SetTakeTrunkValue(int value)
        {
            TakeTrunkValue = value;
        }

        public void EndTakeTrunkValue()
        {
            TakeTrunkValue = 0;
        }

        public void SetMoraleValue(int value)
        {
            TakeMoralevalue = value;
        }

        public void SetCourseValue(int value)
        {
            if (m_PlayerData.CourageValue > 0)
            {
                m_PlayerData.CourageValue += value;
                m_Animator.SetInteger(m_Courage, m_PlayerData.CourageValue);
                m_ProcedureMain.SetCourageValue(m_PlayerData.CourageValue);
            }
        }

        public void ReboundStart(ReboundState rebound)
        {
            IsRebound = true;
            ReboundState = rebound;
        }

        public void PlayerHurtStart()
        {
            HurtEnd = false;
        }

        public void PlayerHurtEnd()
        {
            HurtEnd = true;
        }

        public void ReboundEnd()
        {
            IsRebound = false;
            ReboundState = ReboundState.Ordinary;
        }

        public void GPStart(GPAttack gP)
        {
            GPAttack = gP;
        }

        public void GPEnd()
        {
            GPAttack = GPAttack.None;
        }

        public void BodyStrikeStart(ColliderState state,float stutterTime)
        {
            if (m_ColliderControl.m_ColliderDict.ContainsKey(state))
            {
                ColliderState = state;
                m_ColliderControl.m_ColliderDict[state].gameObject.SetActive(true);
            }
            StutterFrameTime = stutterTime;
        }

        public void BodyStrikeEnd(ColliderState state)
        {
            if (m_ColliderControl.m_ColliderDict.ContainsKey(state))
            {
                ColliderState = ColliderState.None;
                m_ColliderControl.m_ColliderDict[state].gameObject.SetActive(false);
            }
            StutterFrameTime = 0;
        }

        public void ShootArrow(ArrowType arrow,float arrowSpeed,bool isFalling,bool ignoreParry,bool ignoreRebound)
        {
            if (m_LockEnemy)
            {
                m_ArrorTransform = transform;
                Vector3 dir = currentLockEnemy.transform.position - m_BowShootPoint.position;
                m_ArrowImpulse = new Vector3(dir.x, dir.y + 7, dir.z);
                //  m_ArrowPosition = currentLockEnemy.transform.position ;
                m_ArrowPosition = currentLockEnemy.LockTransform.position;
            }
            else
            {
                if (m_IsOnBowAim)
                {
                    m_ArrorTransform = m_moveBehaviour.playerCamera.transform;
                    m_CameraDir = m_moveBehaviour.playerCamera.forward;
                }
                else
                {
                    m_ArrorTransform = transform;
                    m_CameraDir = transform.forward;
                }
                m_ArrowImpulse = m_CameraDir;
                m_ArrowPosition = Vector3.zero;
            }
            m_ArrowRotate.x = m_moveBehaviour.playerCamera.transform.eulerAngles.x;
            switch (arrow)
            {
                case ArrowType.None:
                    GameEntry.Entity.ShowArrow(new ArrowData(GameEntry.Entity.GenerateSerialId(), 30015, weaponDatas[1].OwnerId, weaponDatas[1].OwnerCamp, weaponDatas[1].Attack)
                    {
                        ArrowImpulse = m_ArrowImpulse,
                        Position = m_BowShootPoint.position,
                        Rotation = transform.rotation,
                        Parent = m_ArrorTransform,
                        IsLock = m_LockEnemy,
                        Target = m_ArrowPosition,
                        ArrowType = arrow,
                        ArrowSpeed = arrowSpeed,
                        IsFalling = isFalling,
                        ArrowRotate = m_ArrowRotate,
                        Owner = this,
                        IgnoreParry = ignoreParry,
                        IgnoreRebound =ignoreRebound,
                    }) ;
                    break;
                case ArrowType.Fire:
                    GameEntry.Entity.ShowArrow(new ArrowData(GameEntry.Entity.GenerateSerialId(), 30033, weaponDatas[1].OwnerId, weaponDatas[1].OwnerCamp, weaponDatas[1].Attack)
                    {
                        ArrowImpulse = m_ArrowImpulse,
                        Position = m_BowShootPoint.position,
                        Rotation = transform.rotation,
                        Parent = m_ArrorTransform,
                        IsLock = m_LockEnemy,
                        Target = m_ArrowPosition,
                        ArrowType = arrow,
                        ArrowSpeed = arrowSpeed,
                        IsFalling = isFalling,
                        ArrowRotate = m_ArrowRotate,
                        Owner = this,
                        IgnoreParry = ignoreParry,
                        IgnoreRebound = ignoreRebound,
                    }, typeof(FireArrowLogic));
                    break;
                case ArrowType.Pow:
                    GameEntry.Entity.ShowArrow(new ArrowData(GameEntry.Entity.GenerateSerialId(), 30058, weaponDatas[1].OwnerId, weaponDatas[1].OwnerCamp, weaponDatas[1].Attack)
                    {
                        ArrowImpulse = m_ArrowImpulse,
                        Position = m_BowShootPoint.position,
                        Rotation = transform.rotation,
                        Parent = m_ArrorTransform,
                        IsLock = m_LockEnemy,
                        Target = m_ArrowPosition,
                        ArrowType = arrow,
                        ArrowSpeed = arrowSpeed,
                        IsFalling = isFalling,
                        ArrowRotate = m_ArrowRotate,
                        Owner = this,
                        IgnoreParry = ignoreParry,
                        IgnoreRebound = ignoreRebound,
                    }, typeof(PowArrowLogic));
                    break;
                default:
                    break;
            }



        }

        public void PlayerSound(int soundID, out int? ID)
        {
            ID = GameEntry.Sound.PlaySound(soundID);
        }
        public void GetUpAnimationEvent()
        {
            m_AnimationEventGetUp = true;
        }

        public void DeBuffAnimStart(BuffType buffType)
        {
            Buff.BuffTypeEnum = buffType;

        }

        public void DeBuffAnimEnd()
        {
            Buff.BuffTypeEnum = BuffType.None;

        }

        /// <summary>
        /// 霸体
        /// </summary>
        public void SuperArmorStart()
        {
            IsCanBreak = false;
        }

        public void SuperArmorEnd()
        {
            IsCanBreak = true;
        }

        public void FocusEneryCount(int count)
        {
            m_FocusCount = count;
        }
        public void AttackStart(float stutterTime, int moraleValue, bool ignoreParry,bool ignoreRebound,bool isProduceSF)
        {
            m_WeaponAttack = true;
            isAim = false;
            IgnoreParry = ignoreParry;
            IgnoreRebound = ignoreRebound;
            StutterFrameTime = stutterTime;
            TakeMoralevalue = moraleValue;
            IsProduceSF = isProduceSF;
            if (!isFocusEngy && m_EquiState == EquiState.GiantSword)
            {
                m_Animator.SetFloat("ClickAttackBtnDuration", m_ClickAttackBtnDuration);
            }
            if (m_RightWeaponCollider != null)
                m_RightWeaponCollider.enabled = true;
            ShowTrail();
        }



        public void AttackEnd()
        {
            StutterFrameTime = 0;
            m_WeaponAttack = false;
            IgnoreParry = false;
            IgnoreRebound = false;
            isThump = false;
            IsProduceSF = false;
            if (m_RightWeaponCollider != null)
                m_RightWeaponCollider.enabled = false;
            HideTrail();
        }


        public void PlayerEffectInAnimation(int ID)
        {
            Vector3 point = FindTools.FindFunc<Transform>(transform, "GreatSword_Attack_1").position;
            Log.Info(point);
            GameEntry.Entity.ShowEffect(new EffectData(GameEntry.Entity.GenerateSerialId(), ID)
            {
                Position = transform.position + transform.forward * 2,
                Rotation = Quaternion.identity,
                Owner = this,
            });
        }
        public void FocusRecovery()//蓄力完毕修改武器
        {
            ChangeWeaponRight(30012);
            DestroyWeapon(30002);
        }
        public void PlayEffect(string parentName, int entityID, Vector3 rotate, float keepTime, Type type, out int ID)
        {
            Vector3 point = FindTools.FindFunc<Transform>(transform, parentName).position;
            Log.Warning(point);
            GameEntry.Entity.ShowEffect(new EffectData(ID = GameEntry.Entity.GenerateSerialId(), entityID)
            {
                Position = FindTools.FindFunc<Transform>(transform, parentName).position,
                Rotation = Quaternion.Euler(rotate),
                KeepTime = keepTime,
                Owner = this

            }, type);

        }

        public void PlayEffect(string parentName, int entityID, Vector3 rotate, float keepTime, bool isFollow, out int ID)
        {
            Vector3 point = FindTools.FindFunc<Transform>(transform, parentName).position;
            GameEntry.Entity.ShowEffect(new EffectData(ID = GameEntry.Entity.GenerateSerialId(), entityID)
            {
                Position = FindTools.FindFunc<Transform>(transform, parentName).position,
                // Rotation = Quaternion.Euler(rotate),
                Rotation = transform.rotation,
                KeepTime = keepTime,
                Owner = this,
                IsFollow = isFollow,
                ParentName = parentName
            });
        }

        public void PlayerSKillEffect(SkillEffectTime m_SkillEffectTime, out int ID)
        {

            if (m_LockEnemy)
            {
                m_ArrorTransform = transform;
                Vector3 dir = currentLockEnemy.transform.position - transform.position;
                m_ArrowPosition = currentLockEnemy.LockTransform.position;
                m_ArrowImpulse = new Vector3(dir.x, dir.y + 7, dir.z);
                m_AppearPosition = currentLockEnemy.LockTransform.position +m_SkillEffectTime.m_AppearTargetPosition;
            }
            else
            {
                m_ArrowPosition = Vector3.zero;
                m_AppearPosition = m_SkillEffectTime.m_AppearTargetPosition;
                m_ArrowImpulse = transform.forward;
                Log.Info(m_ArrowImpulse);
            }
            if(m_SkillEffectTime.m_SkillType == SkillEffectType.AppearTarget)
            {
                GameEntry.Entity.ShowSkillEffect(new SkillEffectData(ID = GameEntry.Entity.GenerateSerialId(), m_SkillEffectTime.m_ID)
                {
                    Position =  m_AppearPosition,
                    Rotation = transform.rotation,
                    Owner = this,
                    Target = m_ArrowPosition,
                    IsLock = m_LockEnemy,
                    ArrowImpulse = m_ArrowImpulse,
                    SkillEffectTime =m_SkillEffectTime,
                });
            }
            else
            {
                GameEntry.Entity.ShowSkillEffect(new SkillEffectData(ID = GameEntry.Entity.GenerateSerialId(), m_SkillEffectTime.m_ID)
                {
                    Position = FindTools.FindFunc<Transform>(transform, m_SkillEffectTime.m_ParentName).position,
                    Rotation = transform.rotation,
                    Owner = this,
                    Target = m_ArrowPosition,
                    IsLock = m_LockEnemy,
                    ArrowImpulse = m_ArrowImpulse,
                    SkillEffectTime = m_SkillEffectTime,
                });
            }
          
        }

        public void SetDefense(bool isDefense)
        {
            IsDefense = isDefense;
        }


        // 角色向前位移动画事件
        public void PlayerShiftStartAnim(float time)
        {
            shiftSpeed = 3f;
            StartCoroutine(ShiftWait(time));
        }

        #endregion

        #region AboutFsm

        private void CreateFsm()
        {
            AddFsmState();
            fsm = GameEntry.Fsm.CreateFsm(gameObject.name, this, stateList);
            StartFsm();
        }

        private void StartFsm()
        {
            fsm.Start<PlayerMotionState>();
        }

        private void DestroyFsm()
        {
            GameEntry.Fsm.DestroyFsm(fsm);
            for (int i = 0; i < stateList.Count; i++)
            {
                ReferencePool.Release((IReference)stateList[i]);
            }
            stateList.Clear();
            fsm = null;
        }

        private void AddFsmState()
        {
            stateList.Add(PlayerIdleState.Create());
            stateList.Add(PlayerMotionState.Create());
            stateList.Add(PlayerAttackState.Create());
            stateList.Add(PlayerDefenseState.Create());
            stateList.Add(PlayerDodgeState.Create());
            stateList.Add(PlayerHurtState.Create());
            stateList.Add(PlayerEquipState.Create());
            stateList.Add(PlayerStepState.Create());
            stateList.Add(PlayerEquipWeaponState.Create());
        }
        #endregion

        #region About ShiftLogic
        /// <summary>
        /// 垫步的位移
        /// </summary>
        public void StepMove(Transform player, float LR, float FB, float moveBlend, float speed)
        {
            if (moveBlend != 0)
            {
                PlayerFrontShift(player, speed);
            }
            else
            {
                if (LR > 0)
                {
                    player.Translate(Vector3.right * Time.deltaTime * speed, Space.Self);
                }
                else if (LR < 0)
                {
                    player.Translate(Vector3.left * Time.deltaTime * speed, Space.Self);
                }
                if (FB > 0)
                {
                    player.Translate(Vector3.forward * Time.deltaTime * speed, Space.Self);
                }
                else if (FB < 0)
                {
                    player.Translate(Vector3.back * Time.deltaTime * speed, Space.Self);
                }

            }
        }

        //角色向前位移
        public void PlayerFrontShift(Transform player, float speed)
        {
            player.Translate(Vector3.forward * Time.deltaTime * speed, Space.Self);
        }
        public void PlayerShiftInUpdate(bool isShift, float speed)
        {
            if (isShift) PlayerFrontShift(transform, speed);
        }
        #endregion


        #region About DamageLogic
        public override ImpactData GetImpactData()
        {
            return new ImpactData(m_PlayerData.Camp, m_PlayerData.HP, m_PlayerData.Attack, m_PlayerData.Defense);
        }
        public override void ApplyDamage(Entity attacker, Entity attackType, int damageHP, int damageTrunk, Vector3 weapon)
        {
            //int HurtDefense = Animator.StringToHash("HurtDefense");
            if (isInvincible)
            {
                return;
            }

            if (attacker != null)
            {
                m_Attacker = attacker.gameObject;
            }
            else
            {
                return;
            }

            TargetableObject target = attacker as TargetableObject;
            if (target != null)
            {
                GetCollider = target.ColliderState;
            }
            else
            {
                GetCollider = ColliderState.None;
            }
            if (target != null)
            {
                m_Animator.ResetTrigger(m_Rebound);
                m_Animator.ResetTrigger(m_ReboundHeavy);
                if (!target.IgnoreRebound)
                {
                    if (IsRebound)
                    {
                        TimeStop();     // 盾反顿帧加入主位置
                        if (PlayerData.TrunkValue > 0)
                        {
                            if (PlayerData.TrunkValue >= 55)
                            {
                                PlayerData.TrunkValue -= 55;
                            }
                            else
                            {
                                PlayerData.TrunkValue = 0;
                            }
                            
                            m_ProcedureMain.SetPlayerValue(PlayerData.HP, PlayerData.TrunkValue);
                            if (m_IsCanRebound)
                            {
                                if(target as OrcDoubleAxeLogic)
                                {
                                    target.GetRebound = true;
                                    target.GetReboundEqui = EquiState;
                                    target.GetReboundState = ReboundState;
                                    int trunkValue = Utility.Random.GetRandom(15, 21);
                                    target.TargetableObjectData.TrunkValue -= trunkValue;
                                    GameHotfixEntry.HPBar.ShowTrunkValue(target, target.TargetableObjectData.TrunkRatio);
                                    if (target.IsWeak)
                                    {
                                        target.TargetableObjectData.VertigoValue -= trunkValue * 3;
                                    }
                                    else
                                    {
                                        target.TargetableObjectData.VertigoValue -= trunkValue;
                                    }
                                    if (m_PlayerData.MoraleValue + TakeMoralevalue > 0)
                                    {
                                        if (m_PlayerData.MoraleValue + TakeMoralevalue >= 100)
                                        {
                                            m_PlayerData.MoraleValue = 100;
                                            m_ProcedureMain.SetMoraleValue(m_PlayerData.MoraleValue * 0.01f);
                                        }
                                        else
                                        {
                                            m_PlayerData.MoraleValue = m_PlayerData.MoraleValue + TakeMoralevalue;

                                            m_ProcedureMain.SetMoraleValue(m_PlayerData.MoraleValue * 0.01f);
                                        }
                                    }
                                    else
                                    {
                                        m_PlayerData.MoraleValue = 0;
                                        m_ProcedureMain.SetMoraleValue(m_PlayerData.MoraleValue);
                                    }

                                    GameEntry.Event.Fire(this, ApplyDamageEventArgs.Create(target));
                                }
                            }
                            if (m_PlayerData.MoraleValue > 50)
                            {
                                m_Animator.SetBool(m_ReboundMoraleValue, true);
                                
                            }
                            else
                            {
                                m_Animator.SetBool(m_ReboundMoraleValue, false);
                            }
                            if (target.IsReboundHeavy)
                            {
                                m_Animator.SetTrigger(m_ReboundHeavy);
                            }
                            else
                            {
                                // 盾反顿帧备选加入位置
                                m_Animator.SetTrigger(m_Rebound);
                            }
                            m_IsCanRebound = false;
                            return;
                        }
                        else
                        {
                                m_Animator.SetTrigger(ParryBreak);
                        }
                    }
                       
                       
                }
                

            }
            if (target != null)
            {
                GetCollider = target.ColliderState;
                EnemyLogic enemy = attacker as EnemyLogic;
                if (enemy != null)
                {
                    if (enemy.enemyData.MoraleValue + enemy.TakeMoralevalue > 0)
                    {
                        if (enemy.enemyData.MoraleValue + enemy.TakeMoralevalue >= 100)
                        {
                            enemy.enemyData.MoraleValue = 100;
                           
                        }
                        else
                        {
                            enemy.enemyData.MoraleValue = enemy.enemyData.MoraleValue + enemy.TakeMoralevalue;

                        
                        }
                    }
                    else
                    {
                        enemy.enemyData.MoraleValue = 0;
            
                    }

                    enemy.TakeMoralevalue = 0;
                }
            }
            else
            {
                GetCollider = ColliderState.None;
            }
            if (GPAttack != GPAttack.None)
            {
                if (m_IsCanGP)
                {

                    if (target != null)
                    {
                        target.GetGP = GPAttack;
                        int trunkValue = Utility.Random.GetRandom(35, 41);
                        target.TargetableObjectData.TrunkValue -= trunkValue;
                        if (target.IsWeak)
                        {
                            target.TargetableObjectData.VertigoValue -= trunkValue * 3;
                        }
                        else
                        {
                            target.TargetableObjectData.VertigoValue -= trunkValue;
                        }
                        GameHotfixEntry.HPBar.ShowTrunkValue(target, target.TargetableObjectData.TrunkRatio);
                        GameEntry.Event.Fire(this, ApplyDamageEventArgs.Create(target));
                    }
                    else
                    {
                        Log.Info("GP攻击失败，攻击者为空");
                    }
                }
            }

            isBehindAtked = EnemyAtkedDir(attacker);//判断是否是从背后受到的攻击
            if (IsDefense && !isBehindAtked && m_PlayerData.MP > 0)
            {
                if(target != null)
                {
                    if (!target.IgnoreParry)
                    {
                        underAttack = true;
                        GameEntry.Event.Fire(attacker, ApplyDefenseEventArgs.Create(this));
                        return;
                    }
                }
                else
                {
                    underAttack = true;
                    GameEntry.Event.Fire(attacker, ApplyDefenseEventArgs.Create(this));
                    return;
                }
               
               
            }
            //if (Invulnerable)
            //{
            //    return;
            //}
            m_ProcedureMain.SetPlayerValue(m_PlayerData.HPRatio, m_PlayerData.TrunkRatio);
            base.ApplyDamage(attacker, attackType, damageHP, damageTrunk, weapon);
            GameEntry.Event.Fire(attacker, ApplyDamageEventArgs.Create(this));
            // Invulnerable = true;
        }

        #endregion

        #region WeaponTrail
        public void ShowTrail()
        {
            if (m_WeaponAttackPoint_R != null)
            {
                WeaponTrail weaponTrail = m_WeaponAttackPoint_R.gameObject.GetComponent<WeaponTrail>();
                if (weaponTrail != null)
                {
                    weaponTrail.ShowTrail();
                }
            }
            if (m_WeaponAttackPoint_L != null)
            {
                WeaponTrail weaponTrail = m_WeaponAttackPoint_L.gameObject.GetComponent<WeaponTrail>();
                if (weaponTrail != null)
                {
                    weaponTrail.ShowTrail();
                }
            }
        }

        public void HideTrail()
        {
            if (m_WeaponAttackPoint_R != null)
            {
                WeaponTrail weaponTrail = m_WeaponAttackPoint_R.gameObject.GetComponent<WeaponTrail>();
                if (weaponTrail != null)
                {
                    weaponTrail.HideTrail();
                }
            }
            if (m_WeaponAttackPoint_L != null)
            {
                WeaponTrail weaponTrail = m_WeaponAttackPoint_L.gameObject.GetComponent<WeaponTrail>();
                if (weaponTrail != null)
                {
                    weaponTrail.HideTrail();
                }
            }
        }
        #endregion

        /// <summary>
        /// 该方法为修改游戏整体运行时间，顿帧时间建议小于1s
        /// </summary>
        /// <param name="stopFrameTime"></param>
        /// <param name="size"></param>
        #region 顿帧
        public void SetStopFrameTimeAndSize(float stopFrameTime, float size, bool start = false)
        {
            if (this.stopFrameTime.Equals(0))
            {
                this.stopFrameTime = stopFrameTime;
                stopFrameSize = size;
            }
            if (start)
            {
                TimeStop();
            }
        }

        private void TimeStop()
        {
            StartCoroutine(CommonStop(stopFrameTime));
        }

        IEnumerator CommonStop(float m_comnStopTime)
        {
            Time.timeScale = stopFrameSize;
            for (int i = 0; i < 5; i++)
            {
                yield return new WaitForSeconds(m_comnStopTime/5);
            }
            Time.timeScale = 1f;
        }
        #endregion
    }
}
