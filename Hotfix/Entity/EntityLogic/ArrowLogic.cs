using System.Collections;
using System.Collections.Generic;
using UnityGameFramework.Runtime;
using UnityEngine;
using GameFramework;

namespace Farm.Hotfix
{
    public class ArrowLogic : Entity
    {
        private ArrowData m_ArrowData;
        public ArrowData ArrowData
        {
            get
            {
                return m_ArrowData;
            }
        }

        private Rigidbody m_Rigidbody;
        private  BoxCollider m_BoxCollider;
        private TrailRenderer m_Trail;
        private bool m_DisableRotation;

        private float m_DestroyTime = 10f;
        private float m_PastTime;

        private float m_Power = 100;
        private float m_Gravity = -2;
        private Vector3 m_MoveSpeed;

        private Vector3 m_CurrentAngle;

        private Vector3 m_GravitySpeed;

        private float m_DistanceToTarget;

        private float m_HitAngle = 10f;

        private bool m_IsTrace;

        private float m_Speed = 10f;

        private Vector3 m_TargetPosition;

        private Coroutine m_ShootCoroutine;

        private bool m_IsFalling;

        private bool m_IsGetCollider;
        public  ImpactData GetImpactData()
        {
            return new ImpactData(m_ArrowData.OwnerCamp, 0, m_ArrowData.Attack, 0);
        }

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
        }

        protected override void OnShow(object userData)
        {
            base.OnShow(userData);
            m_ArrowData = userData as ArrowData;
            if (m_ArrowData == null)
            {
                Log.Error("Arrow data is invalid.");
                return;
            }
            m_IsTrace = true;
            m_Speed = m_ArrowData.ArrowSpeed;
            m_IsFalling = m_ArrowData.IsFalling;
            m_GravitySpeed = Vector3.zero;
            m_CurrentAngle = Vector3.zero;
            m_IsGetCollider = false;
            m_MoveSpeed = (m_ArrowData.ArrowImpulse) * m_Power;
            transform.eulerAngles = new Vector3(m_ArrowData.ArrowRotate.x,transform.eulerAngles.y, transform.eulerAngles.z);
            m_BoxCollider = GetComponent<BoxCollider>();
            m_BoxCollider.enabled = true;
            m_Trail = GetComponent<TrailRenderer>();
            if(m_Trail != null)
            {
                m_Trail.Clear();
            }
            if (m_Rigidbody != null)
            {
                m_Rigidbody.WakeUp();
            }

            // m_Rigidbody.AddForce(m_ArrowData.ArrowImpulse*3,ForceMode.Force);
            if (m_ArrowData.IsLock)
            {
                m_TargetPosition = m_ArrowData.Target;
                Shoot();
                // m_Rigidbody.AddForce(m_ArrowData.ArrowImpulse,ForceMode.Impulse);
            }
        }

        private void Shoot()
        {
            m_ShootCoroutine = StartCoroutine(LockArrowMove());
        }

        private IEnumerator LockArrowMove()
        {
            while (m_IsTrace)
            {
                transform.LookAt(m_TargetPosition);
                if (m_IsFalling)
                {
                    float angle = Mathf.Min(1, Vector3.Distance(transform.position, m_TargetPosition) / m_DistanceToTarget) * m_HitAngle;
                    transform.rotation = transform.rotation * Quaternion.Euler(Mathf.Clamp(-angle, -60, 60), 0, 0);
                }
                float currentdist = Vector3.Distance(transform.position, m_TargetPosition);
                if (currentdist < 0.5f)
                {
                    m_IsTrace = false;
                }
                transform.Translate(Vector3.forward * Mathf.Min(m_Speed * Time.deltaTime, currentdist));
                yield return null;
            }

            if (!m_IsTrace)
            {
                transform.position = m_TargetPosition;
                if (m_ShootCoroutine != null)
                {
                    StopCoroutine(m_ShootCoroutine);
                }
                m_PastTime = 0;
                GameEntry.Entity.HideEntity(this);
            }
            
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);


            if (!m_ArrowData.IsLock)
            {
                if (!m_IsGetCollider)
                {
                    m_GravitySpeed.y = m_Gravity * (m_PastTime += elapseSeconds);

                    transform.position += (m_MoveSpeed + m_GravitySpeed) * elapseSeconds;
                }
               // m_CurrentAngle.x = Mathf.Atan((m_MoveSpeed.y + m_GravitySpeed.y) / m_MoveSpeed.z) * Mathf.Rad2Deg;
               // transform.eulerAngles = m_CurrentAngle;
            }


            //if (!m_DisableRotation)
            //{
            //    transform.rotation = Quaternion.LookRotation(m_Rigidbody.velocity);
            //}
            if (m_PastTime < m_DestroyTime)
            {
                m_PastTime += elapseSeconds;
            }
            else
            {
                m_PastTime = 0;
              //  m_Rigidbody.Sleep();
                GameEntry.Entity.HideEntity(this);
            }

        }
        protected virtual void OnTriggerEnter(Collider other)
        {
            if(other.gameObject.tag == "ArrowCollider")
            {
                m_IsGetCollider = true;
                transform.SetParent(other.transform);
            }
            TargetableObject entity = other.gameObject.GetComponent<TargetableObject>();
            if (entity == null)
            {
                return;
            }
            if (m_ArrowData.OwnerId == entity.Id)
            {
                return;
            }
            Vector3 point = other.bounds.ClosestPoint(transform.position);
            m_BoxCollider.enabled = false;

            // GameEntry.Entity.AttachEntity(Entity, entity.Entity);

            AIUtility.PerformCollisionBow(entity, this, point, m_ArrowData.ArrowType,m_ArrowData.Owner);
            
            //GameEntry.Entity.HideEntity(this);
        }



    }
}

