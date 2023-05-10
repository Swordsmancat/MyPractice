using GameFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace Farm.Hotfix
{
    /// <summary>
    /// 特效类。
    /// </summary>
    public class SkillEffectLogic : Entity
    {
        public SkillEffectData m_SkillEffectData = null;

        private float m_ElapseSeconds = 0f;

        private float m_HitInterval;

        private bool m_IsCollider;

        private float m_CurrentTime;

        private Collider m_Collider;

        private bool m_IsAttackStart;

        private bool m_IsAttackEnd;

        private Coroutine m_ShootCoroutine;
        private Coroutine m_WhirlCoroutine;

        private Vector3 m_TargetPosition;

        private bool m_IsTrace;

        private Vector3 m_MoveSpeed;

        private Vector3 m_GravitySpeed;

        private PlayerLogic m_PlayerLogic;

        private EnemyLogic m_EnemyLogic;

        private bool m_IsLookAt;

        private bool m_IsWhirlBack;

        private float m_WhirlBackTime;

        private Transform m_RevolveTransform;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
        }

        protected override void OnShow(object userData)
        {
            base.OnShow(userData);
            m_SkillEffectData = userData as SkillEffectData;
            if (m_SkillEffectData == null)
            {
                Log.Error("SkillEffect data is invalid.");
                return;
            }
            m_ElapseSeconds = 0f;
            m_CurrentTime = 0;
            m_WhirlBackTime = 0f;
            m_IsTrace = true;
            m_IsLookAt = true;
            m_IsCollider = false;
            m_IsAttackStart = false;
            m_IsAttackEnd = false;
            m_IsWhirlBack = false;
            m_Collider = transform.GetComponent<Collider>();
            if (m_Collider == null)
            {
                m_Collider = GetComponentInChildren<Collider>();
            }
            m_HitInterval = m_SkillEffectData.SkillEffectTime.m_HitIntervalTime;
            m_MoveSpeed = m_SkillEffectData.ArrowImpulse * m_SkillEffectData.SkillEffectTime.m_FlySpeed;
            m_TargetPosition = m_SkillEffectData.Target;
            m_PlayerLogic = m_SkillEffectData.Owner as PlayerLogic;
            if (m_ShootCoroutine != null)
            {
                StopCoroutine(m_ShootCoroutine);
            }
            if (m_WhirlCoroutine != null)
            {
                StopCoroutine(m_WhirlCoroutine);
            }
            if (m_PlayerLogic == null)
            {
                m_EnemyLogic = m_SkillEffectData.Owner as EnemyLogic;
            }
            if (m_Collider != null)
            {
                m_Collider.enabled = false;
            }
            else
            {
                Log.Warning("特效无碰撞伤害");
            }
            if (m_ShootCoroutine != null)
            {
                StopCoroutine(m_ShootCoroutine);
            }
            if (m_SkillEffectData.SkillEffectTime.m_IsFollow)
            {
                GameEntry.Entity.AttachEntityByFindChild(Entity, m_SkillEffectData.Owner.Id, m_SkillEffectData.SkillEffectTime.m_ParentName);
            }

            switch (m_SkillEffectData.SkillEffectTime.m_SkillType)
            {
                case SkillEffectType.None:
                    break;
                case SkillEffectType.Shoot:
                    if (m_SkillEffectData.IsLock)
                    {
                        Shoot();
                    }
                    break;
                case SkillEffectType.AppearTarget:
                    break;
                case SkillEffectType.StraightWhirl:
                    if (m_SkillEffectData.SkillEffectTime.m_IsRevolve)
                    {
                        m_RevolveTransform = transform.GetChild(0).transform;
                    }
                    if (m_SkillEffectData.IsLock)
                    {
                        ShootStraigthWhirl();
                    }
                    break;
                default:
                    break;
            }

        }

        protected override void OnAttachTo(EntityLogic parentEntity, Transform parentTransform, object userData)
        {
            base.OnAttachTo(parentEntity, parentTransform, userData);
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
            transform.localScale = Vector3.one;
        }

        private void ShootStraigthWhirl()
        {
            m_WhirlCoroutine = StartCoroutine(LockStraigthWhirl());
        }

        private IEnumerator LockStraigthWhirl()
        {
            transform.LookAt(m_TargetPosition);
            while (m_IsTrace)
            {
                float currentdist = Vector3.Distance(transform.position, m_SkillEffectData.Owner.transform.position);
                if (currentdist >= m_SkillEffectData.SkillEffectTime.m_StraightWhirlLength)
                {
                    m_IsWhirlBack = true;
                    transform.LookAt(m_SkillEffectData.Owner.transform.position + m_SkillEffectData.Owner.transform.up);
                    // m_IsTrace = false;
                }

                if (m_IsWhirlBack)
                {
                    if (m_WhirlBackTime >= m_SkillEffectData.SkillEffectTime.m_StraightWhirlTime)
                    {
                        transform.Translate(Vector3.forward * m_SkillEffectData.SkillEffectTime.m_StraightWhirlFlySpeed * Time.deltaTime);

                        float currentdistNoY = Vector3.Distance(new Vector3(transform.position.x, 0, transform.position.z), new Vector3(m_SkillEffectData.Owner.transform.position.x, 0, m_SkillEffectData.Owner.transform.position.z));
                        if (currentdistNoY <= 1f)
                        {
                            GameEntry.Entity.HideEntity(this);
                        }
                    }
                }
                else
                {
                    transform.Translate(Vector3.forward * m_SkillEffectData.SkillEffectTime.m_StraightWhirlFlySpeed * Time.deltaTime);
                }
                yield return null;
            }
        }

        private void Shoot()
        {
            m_ShootCoroutine = StartCoroutine(LockArrowMove());
        }

        protected override void OnHide(bool isShutdown, object userData)
        {
            base.OnHide(isShutdown, userData);
            if (m_SkillEffectData.SkillEffectTime.m_IsRevolve)
            {
                m_RevolveTransform.eulerAngles = Vector3.zero;
            }
            transform.eulerAngles = Vector3.zero;
        }

        private IEnumerator LockArrowMove()
        {
            transform.LookAt(m_TargetPosition);
            while (m_IsTrace)
            {
                //float currentdist = Vector3.Distance(transform.position, m_TargetPosition);
                //if (m_IsLookAt)
                //{

                //}
                //if (currentdist < 0.5f)
                //{
                //    m_IsLookAt = false;
                //}
                if (m_SkillEffectData.SkillEffectTime.m_IsFalling)
                {
                    float angle = Mathf.Min(1, Vector3.Distance(transform.position, m_TargetPosition)) * 10f;
                    transform.rotation = transform.rotation * Quaternion.Euler(Mathf.Clamp(-angle, -10, 10), 0, 0);
                }
                // float currentdist = Vector3.Distance(transform.position, m_TargetPosition);
                //if (currentdist < 0.5f)
                //{
                //    m_IsTrace = false;
                //}
                // transform.Translate(Vector3.forward * Mathf.Min(m_SkillEffectData.SkillEffectTime.m_FlySpeed * Time.deltaTime, currentdist));
                transform.Translate(Vector3.forward * m_SkillEffectData.SkillEffectTime.m_FlySpeed * Time.deltaTime);
                yield return null;
            }

            //if (!m_IsTrace)
            //{
            //  //  transform.position = m_TargetPosition;
            //    if (m_ShootCoroutine != null)
            //    {
            //        StopCoroutine(m_ShootCoroutine);
            //    }
            //    //GameEntry.Entity.HideEntity(this);
            //}

        }
        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
            if (m_IsWhirlBack)
            {
                m_WhirlBackTime += elapseSeconds;
            }

            switch (m_SkillEffectData.SkillEffectTime.m_SkillType)
            {
                case SkillEffectType.None:
                    break;
                case SkillEffectType.Shoot:
                    if (!m_SkillEffectData.IsLock)
                    {
                        if (m_SkillEffectData.SkillEffectTime.m_IsFalling)
                        {
                            m_GravitySpeed.y = -2f * (m_ElapseSeconds += elapseSeconds);
                        }
                        else
                        {
                            m_GravitySpeed = Vector3.zero;
                        }
                        transform.position += (m_MoveSpeed + m_GravitySpeed) * elapseSeconds;
                    }
                    break;
                case SkillEffectType.AppearTarget:
                    break;
                case SkillEffectType.StraightWhirl:
                    if (m_SkillEffectData.SkillEffectTime.m_IsRevolve)
                    {
                        switch (m_SkillEffectData.SkillEffectTime.m_RevolveType)
                        {
                            case RevolveType.X:
                                m_RevolveTransform.Rotate(Vector3.right, 30f, Space.Self);
                                break;
                            case RevolveType.Y:
                                m_RevolveTransform.Rotate(Vector3.up, 30f, Space.Self);
                                break;
                            case RevolveType.Z:
                                m_RevolveTransform.Rotate(Vector3.forward, 30f, Space.Self);
                                break;
                            default:
                                break;
                        }
                    }
                    if (!m_SkillEffectData.IsLock)
                    {
                        float currentdist = Vector3.Distance(transform.position, m_SkillEffectData.Owner.transform.position);
                        if (currentdist >= m_SkillEffectData.SkillEffectTime.m_StraightWhirlLength)
                        {
                            m_IsWhirlBack = true;
                            transform.LookAt(m_SkillEffectData.Owner.transform.position);
                        }
                        if (m_IsWhirlBack)
                        {
                            if (m_WhirlBackTime >= m_SkillEffectData.SkillEffectTime.m_StraightWhirlTime)
                            {
                                transform.Translate(Vector3.forward * m_SkillEffectData.SkillEffectTime.m_StraightWhirlFlySpeed * Time.deltaTime);
                            }
                        }
                        else
                        {
                            transform.Translate(Vector3.forward * m_SkillEffectData.SkillEffectTime.m_StraightWhirlFlySpeed * Time.deltaTime);
                        }
                    }
                    break;
                default:
                    break;
            }
            m_CurrentTime += elapseSeconds;
            if (!m_IsAttackStart)
            {
                if (m_CurrentTime >= m_SkillEffectData.SkillEffectTime.m_HitStartTime)
                {
                    m_IsAttackStart = true;
                    m_Collider.enabled = true;
                    if (m_PlayerLogic != null)
                    {
                        m_PlayerLogic.DeBuffAnimStart(m_SkillEffectData.SkillEffectTime.m_BuffTypeEnum);
                    }
                    else
                    {
                        m_EnemyLogic.DeBuffAnimStart(m_SkillEffectData.SkillEffectTime.m_BuffTypeEnum);
                    }
                }
            }

            if (!m_IsAttackEnd)
            {
                if (m_CurrentTime >= m_SkillEffectData.SkillEffectTime.m_HitEndTime)
                {
                    m_IsAttackEnd = true;
                    m_Collider.enabled = false;
                    if (m_PlayerLogic != null)
                    {
                        m_PlayerLogic.DeBuffAnimEnd();
                    }
                    else
                    {
                        m_EnemyLogic.DeBuffAnimEnd();
                    }
                }
            }

            if (m_SkillEffectData.SkillEffectTime.m_KeepTime > 0)
            {
                m_ElapseSeconds += elapseSeconds;
                if (m_ElapseSeconds >= m_SkillEffectData.SkillEffectTime.m_KeepTime)
                {
                    GameEntry.Entity.HideEntity(this);
                }
            }
            if (m_IsCollider)
            {
                m_HitInterval += elapseSeconds;
            }

        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
            {
                Vector3 point = other.bounds.ClosestPoint(transform.position);
                GameEntry.Entity.ShowEffect(new EffectData(GameEntry.Entity.GenerateSerialId(), m_SkillEffectData.SkillEffectTime.m_HitColliderEffectID)
                {
                    Position = point,
                    KeepTime = 3f
                });
                GameEntry.Entity.HideEntity(this);
            }
            TargetableObject entity = other.gameObject.GetComponent<TargetableObject>();
            if (entity == null)
            {
                return;
            }
            if (m_SkillEffectData.Owner.Id == entity.Id)
            {
                return;
            }
            m_IsCollider = true;

        }

        private void OnTriggerStay(Collider other)
        {
            TargetableObject entity = other.gameObject.GetComponent<TargetableObject>();
            if (entity == null)
            {
                return;
            }
            if (m_SkillEffectData.Owner.Id == entity.Id)
            {
                return;
            }

            if (m_HitInterval < m_SkillEffectData.SkillEffectTime.m_HitIntervalTime)
            {
                return;
            }
            m_HitInterval = 0;
            Vector3 point = other.bounds.ClosestPoint(transform.position);
            GameEntry.Entity.ShowEffect(new EffectData(GameEntry.Entity.GenerateSerialId(), m_SkillEffectData.SkillEffectTime.m_HitColliderEffectID)
            {
                Position = point,
                KeepTime = 3f
            });
            AIUtility.PerformCollisionAttack(m_SkillEffectData.Owner, entity, point,this);
            if (m_SkillEffectData.SkillEffectTime.m_HitIntervalTime <= 0)
            {
                if (m_SkillEffectData.SkillEffectTime.m_SkillType != SkillEffectType.StraightWhirl)
                {
                    GameEntry.Entity.HideEntity(this);
                }
            }


        }



    }
}
