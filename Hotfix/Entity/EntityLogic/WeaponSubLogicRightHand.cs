
using GameFramework;
using UMA;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace Farm.Hotfix
{
    /// <summary>
    /// 武器类。
    /// </summary>
    public class WeaponSubLogicRightHand : WeaponLogic
    {
        private const string AttachPoint_Take = "SubweaponTake_R";
        private const string AttachPoint_PutDown = "SubweaponPutDown_R";

        [SerializeField]
        private WeaponData m_WeaponData = null;

        public WeaponData weaponData => m_WeaponData;

        public bool IsShield;

        private UMAData m_UmaData;

        private Transform m_WeaponTake;

        private Transform m_WeaponPutDown;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
        }

        protected override void OnShow(object userData)
        {
            base.OnShow(userData);

            m_WeaponData = userData as WeaponData;
            if (m_WeaponData == null)
            {
                Log.Error("Weapon data is invalid.");
                return;
            }
            // GameEntry.Entity.AttachEntity(Entity, m_WeaponData.OwnerId, AttachRightPoint);
            m_UmaData = GameEntry.Entity.GetEntity(m_WeaponData.OwnerId).GetComponent<UMAData>();
            if (m_UmaData != null)
            {

            }
            else
            {
                m_WeaponTake = FindTools.FindFunc<Transform>(GameEntry.Entity.GetEntity(m_WeaponData.OwnerId).transform, AttachPoint_Take);
                m_WeaponPutDown = FindTools.FindFunc<Transform>(GameEntry.Entity.GetEntity(m_WeaponData.OwnerId).transform, AttachPoint_PutDown);
                GameEntry.Entity.AttachEntity(Entity, m_WeaponData.OwnerId, m_WeaponPutDown);
            }
            IsShield = m_WeaponData.IsShield;
        }

        protected override void OnDetachFrom(EntityLogic parentEntity, object userData)
        {
            base.OnDetachFrom(parentEntity, userData);
            CachedTransform.SetParent(m_WeaponPutDown);
            CachedTransform.localPosition = Vector3.zero;
            CachedTransform.localRotation = Quaternion.identity;
            CachedTransform.localScale = Vector3.one;
        }
        protected override void OnAttachTo(EntityLogic parentEntity, Transform parentTransform, object userData)
        {
            base.OnAttachTo(parentEntity, parentTransform, userData);
            Name = Utility.Text.Format("Shield of {0}", parentEntity.Name);
            CachedTransform.localPosition = Vector3.zero;
            CachedTransform.localRotation = Quaternion.identity;
            CachedTransform.localScale = Vector3.one;
        }


    }
}
