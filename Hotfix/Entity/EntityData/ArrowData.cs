//------------------------------------------------------------
// Creator: FatCat
// Creation date: 2022/9/20/周二 14:36:37
//------------------------------------------------------------
using UnityEngine;

namespace Farm.Hotfix
{
    public class ArrowData : EntityData
    {
        private int m_OwnerId = 0;

        private CampType m_OwnerCamp = CampType.Unknown;

        private int m_Attack = 0;

        private Vector3 m_ArrowImpulse;

        private Transform m_Parent;

        private bool m_IsLock;

        private Vector3 m_Target;

        private ArrowType m_ArrowType;

        private float m_ArrowSpeed;

        private bool m_IsFalling;

        private Vector3 m_ArrowRotate;

        private TargetableObject m_Owner;

        private bool m_IgnoreParry;

        private bool m_IgnoreRebound;
        public ArrowData(int entityId, int typeId,int ownerId,CampType ownerCamp,int attack) : base(entityId, typeId)
        {
            m_OwnerId = ownerId;
            m_OwnerCamp = ownerCamp;
            m_Attack = attack;
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

        public TargetableObject Owner
        {
            get
            {
                return m_Owner;
            }
            set
            {
                m_Owner = value;
            }
        }

        public Vector3 ArrowRotate
        {
            get
            {
               return m_ArrowRotate;
            }
            set
            {
                m_ArrowRotate = value;
            }
        }

        public bool IsFalling
        {
            get
            {
                return m_IsFalling;
            }
            set
            {
                m_IsFalling = value;
            }
        }

        public float ArrowSpeed
        {
            get
            {
                return m_ArrowSpeed;
            }
            set
            {
                m_ArrowSpeed = value;
            }
        }

        public ArrowType ArrowType
        {
            get
            {
                return m_ArrowType;
            }
            set
            {
                m_ArrowType = value;
            }
        }

        public int OwnerId
        {
            get
            {
                return m_OwnerId;
            }
        }

        public CampType OwnerCamp
        {
            get
            {
                return m_OwnerCamp;
            }
        }

        public int Attack
        {
            get
            {
                return m_Attack;
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

        public Transform Parent
        {
            get
            {
                return m_Parent;
            }
            set
            {
                m_Parent = value;
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

    }
}
