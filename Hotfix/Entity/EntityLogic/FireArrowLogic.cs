using System.Collections;
using System.Collections.Generic;
using UnityGameFramework.Runtime;
using UnityEngine;
using GameFramework;

namespace Farm.Hotfix
{
    public class FireArrowLogic : ArrowLogic
    {
        private ArrowData m_ArrowData;
        private Rigidbody m_Rigidbody;
        private  BoxCollider m_BoxCollider;
        private TrailRenderer m_Trail;
        private bool m_DisableRotation;

        private float m_DestroyTime = 10f;
        private float m_PastTime;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
        }

        protected override void OnShow(object userData)
        {
            base.OnShow(userData);
            m_Trail = GetComponent<TrailRenderer>();
            if(m_Trail != null)
            {
                m_Trail.Clear();
            }
            else
            {
                m_Trail = FindTools.FindFunc<Transform>(transform, "Trail").GetComponent<TrailRenderer>();
                if(m_Trail != null)
                {
                    m_Trail.Clear();
                }
            }


        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);

        }

        protected override void OnTriggerEnter(Collider other)
        {
            base.OnTriggerEnter(other);
            Vector3 point = other.bounds.ClosestPoint(transform.position);
            GameEntry.Entity.ShowEffect(new EffectData(GameEntry.Entity.GenerateSerialId(), 30034)
            {
             Position = point,
            });
        }


    }
}

