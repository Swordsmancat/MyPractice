using System;
using UnityEngine;

namespace Farm.Hotfix
{
  public  class SkillEffectData : EntityData
    {
        [SerializeField]
        private Vector3 m_Position;

        private TargetableObject owner;

        private Vector3 m_Target;

        private bool m_IsLock;

        private Vector3 m_ArrowImpulse;

        private SkillEffectTime m_SkillEffectTime;


        public SkillEffectData(int entityId, int typeId=0)
           : base(entityId, typeId)
        {

        }

        public SkillEffectTime SkillEffectTime
        {
            get
            {
                return m_SkillEffectTime;
            }
            set
            {
                m_SkillEffectTime = value;
            }
        }

        public Vector3 ArrowImpulse
        {
            get
            {
                return m_ArrowImpulse;
            }
            set
            {
                m_ArrowImpulse = value;
            }
        }

        public bool IsLock
        {
            get
            {
                return m_IsLock;
            }
            set
            {
                m_IsLock = value;
            }
        }

        public Vector3 Target
        {
            get
            {
                return m_Target;
            }
            set
            {
                m_Target = value;
            }
        }

        public TargetableObject Owner
        {
            get
            {
                return owner;
            }
            set
            {
                owner = value;
            }
        }


    }
}
