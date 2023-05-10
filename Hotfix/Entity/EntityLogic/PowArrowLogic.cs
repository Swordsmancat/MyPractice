using System.Collections;
using System.Collections.Generic;
using UnityGameFramework.Runtime;
using UnityEngine;
using GameFramework;

namespace Farm.Hotfix
{
    public class PowArrowLogic : ArrowLogic
    {
        private TrailRenderer m_Trail;

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
                m_Trail = FindTools.FindFunc<Transform>(transform, "ArrowTrail").GetComponent<TrailRenderer>();
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
            GameEntry.Entity.ShowEffect(new EffectData(GameEntry.Entity.GenerateSerialId(), 30057)
            {
             Position = point,
            });
        }


    }
}

