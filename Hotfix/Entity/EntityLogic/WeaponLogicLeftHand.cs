
using GameFramework;
using UMA;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace Farm.Hotfix
{
    /// <summary>
    /// 武器类。
    /// </summary>
    public class WeaponLogicLeftHand : WeaponLogic
    {
        private const string AttachPoint_Take = "MainweaponTake_L";
        private const string AttachPoint_PutDown = "MainweaponPutDown_L";

        //private const string AttachLeftPoint_Put = "ShieldPos";

        private const string AttachLeftPointPlayer = "LeftHand";
        private const string AttachLeftPointPlayer_PutDown = "LeftShoulderAdjust";

        private const string AttachPlayerSwordShield_L = "SwordShield_L";
        private const string AttachPlayerSwordShieldPutDown_L = "SwordShieldPutDown_L";

        private const string AttachPlayerGiantSword_L = "GiantSword_L";
        private const string AttachPlayerGiantSwordPutDown_L = "GiantSwordPutDown_L";

        private const string AttachPlayerDagger_L = "Dagger_L";
        private const string AttachPlayerDaggerPutDown_L = "DaggerPutDown_L";

        private const string AttachPlayerRevengerDagger_L = "RevengerDagger_L";
        private const string AttachPlayerRevengerDaggerPutDown_L = "RevengerDaggerPutDown_L";

        private const string AttachPlayerDoubleBlades_L = "DoubleBlades_L";
        private const string AttachPlayerDoubleBladesPutDown_L = "DoubleBladesPutDown_L";

        private const string AttachPlayerBow_L = "Bow_L";
        private const string AttachPlayerBowPutDown_L = "BowPutDown_L";

        private const string AttachPlayerGunner_L = "Gunner_L";
        private const string AttachPlayerGunnerPutDown_L = "GunnerPutDown_L";

        private const string AttachPlayerPike_L = "Pike_L";
        private const string AttachPlayerPikePutDown_L = "PikePutDown_L";




        //private const string AttachRightPoint_TwoSwords_L_Take = "Weapon_TwoSwords_L";
        //private const string AttachRightPoint_TwoSwords_L_Put = "WeaponPos_TwoSwords_L";

        [SerializeField]
        private WeaponData m_WeaponData = null;

        public WeaponData weaponData => m_WeaponData;

        public bool IsShield;

        private float m_NextAttackTime = 0f;

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
                switch (PlayerLogic.Instance.EquiState)
                {
                    case EquiState.SwordShield:
                        if (PlayerLogic.Instance.IsHands)
                        {
                            GameEntry.Entity.AttachEntityByFindChild(Entity, m_WeaponData.OwnerId, AttachPlayerSwordShield_L);
                        }
                        else
                        {
                            GameEntry.Entity.AttachEntityByFindChild(Entity, m_WeaponData.OwnerId, AttachPlayerSwordShieldPutDown_L);
                        }
                        break;
                    case EquiState.GiantSword:
                        if (PlayerLogic.Instance.IsHands)
                        {
                            GameEntry.Entity.AttachEntityByFindChild(Entity, m_WeaponData.OwnerId, AttachPlayerGiantSword_L);
                        }
                        else
                        {
                            GameEntry.Entity.AttachEntityByFindChild(Entity, m_WeaponData.OwnerId, AttachPlayerGiantSwordPutDown_L);
                        }
                        break;
                    case EquiState.Katana:
                        if (PlayerLogic.Instance.IsHands)
                        {
                            GameEntry.Entity.AttachEntityByFindChild(Entity, m_WeaponData.OwnerId, AttachPlayerDagger_L);
                        }
                        else
                        {
                            GameEntry.Entity.AttachEntityByFindChild(Entity, m_WeaponData.OwnerId, AttachPlayerDaggerPutDown_L);
                        }
                        break;
                    case EquiState.DoubleBlades:
                        if (PlayerLogic.Instance.IsHands)
                        {
                            GameEntry.Entity.AttachEntityByFindChild(Entity, m_WeaponData.OwnerId, AttachPlayerDoubleBlades_L);
                        }
                        else
                        {
                            GameEntry.Entity.AttachEntityByFindChild(Entity, m_WeaponData.OwnerId, AttachPlayerDoubleBladesPutDown_L);
                        }
                        break;
                    case EquiState.Bow:
                        if (PlayerLogic.Instance.IsHands)
                        {
                            GameEntry.Entity.AttachEntityByFindChild(Entity, m_WeaponData.OwnerId, AttachPlayerBow_L);
                        }
                        else
                        {
                            GameEntry.Entity.AttachEntityByFindChild(Entity, m_WeaponData.OwnerId, AttachPlayerBowPutDown_L);
                        }
                        break;
                    case EquiState.RevengerDoubleBlades:
                        if (PlayerLogic.Instance.IsHands)
                        {
                            GameEntry.Entity.AttachEntityByFindChild(Entity, m_WeaponData.OwnerId, AttachPlayerRevengerDagger_L);
                        }
                        else
                        {
                            GameEntry.Entity.AttachEntityByFindChild(Entity, m_WeaponData.OwnerId, AttachPlayerRevengerDaggerPutDown_L);
                        }
                        break;
                    case EquiState.Gunner:
                        if (PlayerLogic.Instance.IsHands)
                        {
                            GameEntry.Entity.AttachEntityByFindChild(Entity, m_WeaponData.OwnerId, AttachPlayerGunner_L);
                        }
                        else
                        {
                            GameEntry.Entity.AttachEntityByFindChild(Entity, m_WeaponData.OwnerId, AttachPlayerGunnerPutDown_L);
                        }
                        break;
                    case EquiState.Pike:
                        if (PlayerLogic.Instance.IsHands)
                        {
                            GameEntry.Entity.AttachEntityByFindChild(Entity, m_WeaponData.OwnerId, AttachPlayerPike_L);
                        }
                        else
                        {
                            GameEntry.Entity.AttachEntityByFindChild(Entity, m_WeaponData.OwnerId, AttachPlayerPikePutDown_L);
                        }
                        break;
                    default:
                        SetTransform(Vector3.zero, Quaternion.identity, Vector3.one);
                        break;
                }

            }
            else
            {
                m_WeaponTake = FindTools.FindFunc<Transform>(GameEntry.Entity.GetEntity(m_WeaponData.OwnerId).transform, AttachPoint_Take);
                m_WeaponPutDown = FindTools.FindFunc<Transform>(GameEntry.Entity.GetEntity(m_WeaponData.OwnerId).transform, AttachPoint_PutDown);
                GameEntry.Entity.AttachEntity(Entity, m_WeaponData.OwnerId, m_WeaponTake);
            }
            IsShield = m_WeaponData.IsShield;
        }



        protected override void OnAttachTo(EntityLogic parentEntity, Transform parentTransform, object userData)
        {
            base.OnAttachTo(parentEntity, parentTransform, userData);
            Name = Utility.Text.Format("Shield of {0}", parentEntity.Name);
            CachedTransform.localPosition = Vector3.zero;
            CachedTransform.localRotation = Quaternion.identity;
            CachedTransform.localScale = Vector3.one;
        }

        protected override void OnDetachFrom(EntityLogic parentEntity, object userData)
        {
            base.OnDetachFrom(parentEntity, userData);
            CachedTransform.SetParent(m_WeaponPutDown);
            CachedTransform.localPosition = Vector3.zero;
            CachedTransform.localRotation = Quaternion.identity;
            CachedTransform.localScale = Vector3.one;
        }

        private void SetTransform(Vector3 position, Quaternion rotation, Vector3 scale)
        {
            CachedTransform.localPosition = position;
            CachedTransform.localRotation = rotation;
            CachedTransform.localScale = scale;

        }

        public ImpactData GetImpactData()
        {
            return new ImpactData(m_WeaponData.OwnerCamp, 0, m_WeaponData.Attack, 0);
        }


        public void TryAttack()
        {
            if (Time.time < m_NextAttackTime)
            {
                return;
            }

            m_NextAttackTime = Time.time + m_WeaponData.AttackInterval;
            GameEntry.Sound.PlaySound(m_WeaponData.BulletSoundId);
        }
    }
}
