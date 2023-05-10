
using GameFramework;
using UnityEngine;
using UnityGameFramework.Runtime;
using UMA;

namespace Farm.Hotfix
{
    /// <summary>
    /// 武器类。
    /// </summary>
    public class WeaponLogicRightHand : WeaponLogic
    {
        private const string AttachPoint_Take = "MainweaponTake_R";
        private const string AttachPoint_PutDown = "MainweaponPutDown_R";
        //private const string AttachRightPoint_Put = "WeaponPos"; 

        private const string AttachRightPointPlayer = "RightHand";
        private const string AttachRightPointPlayer_PutDown = "RightShoulderAdjust";

        private const string AttachPlayerSwordShield_R = "SwordShield_R";
        private const string AttachPlayerSwordShieldPutDown_R = "SwordShieldPutDown_R";

        private const string AttachPlayerGiantSword_R = "GiantSword_R";
        private const string AttachPlayerGiantSwordPutDown_R = "GiantSwordPutDown_R";

        private const string AttachPlayerDagger_R = "Dagger_R";
        private const string AttachPlayerDaggerPutDown_R = "DaggerPutDown_R";

        private const string AttachPlayerRevengerDagger_R = "RevengerDagger_R";
        private const string AttachPlayerRevengerDaggerPutDown_R = "RevengerDaggerPutDown_R";

        private const string AttachPlayerDoubleBlades_R = "DoubleBlades_R";
        private const string AttachPlayerDoubleBladesPutDown_R = "DoubleBladesPutDown_R";

        private const string AttachPlayerBow_R = "Bow_R";
        private const string AttachPlayerBowPutDown_R = "BowPutDown_R";

        private const string AttachPlayerGunner_R = "Gunner_R";
        private const string AttachPlayerGunnerPutDown_R = "GunnerPutDown_R";

        private const string AttachPlayerPike_R = "Pike_R";
        private const string AttachPlayerPikePutDown_R = "PikePutDown_R";



        //private const string AttachRightPoint_ShortHand = "Weapon_ShortHand";
        //private const string AttachRightPoint_TwoHands = "Weapon_TwoHands";
        //private const string AttachRightPoint_ShortHand_Take = "Weapon_ShortHand";
        //private const string AttachRightPoint_ShortHand_Put = "WeaponPos_ShortHand";

        //private const string AttachRightPoint_TwoHands_Take = "Weapon_TwoHands";
        //private const string AttachRightPoint_TwoHands_Put = "WeaponPos_TwoHands";

        //private const string AttachRightPoint_TwoSwords_R_Take = "Weapon_TwoSwords_R";
        //private const string AttachRightPoint_TwoSwords_R_Put = "WeaponPos_TwoSwords_R";

        [SerializeField]
        private WeaponData m_WeaponData = null;

        public WeaponData weaponData => m_WeaponData;

        private UMAData m_UmaData;

        private float m_NextAttackTime = 0f;

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
            m_UmaData = GameEntry.Entity.GetEntity(m_WeaponData.OwnerId).GetComponent<UMAData>();
            if (m_UmaData != null)
            {
                switch (PlayerLogic.Instance.EquiState)
                {
                    case EquiState.SwordShield:
                        if (PlayerLogic.Instance.IsHands)
                        {
                            GameEntry.Entity.AttachEntityByFindChild(Entity, m_WeaponData.OwnerId, AttachPlayerSwordShield_R);
                        }
                        else
                        {
                            GameEntry.Entity.AttachEntityByFindChild(Entity, m_WeaponData.OwnerId, AttachPlayerSwordShieldPutDown_R);
                        }
                        break;
                    case EquiState.GiantSword:
                        if (PlayerLogic.Instance.IsHands)
                        {
                            GameEntry.Entity.AttachEntityByFindChild(Entity, m_WeaponData.OwnerId, AttachPlayerGiantSword_R);
                        }
                        else
                        {
                            GameEntry.Entity.AttachEntityByFindChild(Entity, m_WeaponData.OwnerId, AttachPlayerGiantSwordPutDown_R);
                        }
                        break;
                    case EquiState.Katana:
                        if (PlayerLogic.Instance.IsHands)
                        {
                            GameEntry.Entity.AttachEntityByFindChild(Entity, m_WeaponData.OwnerId, AttachPlayerDagger_R);
                        }
                        else
                        {
                            GameEntry.Entity.AttachEntityByFindChild(Entity, m_WeaponData.OwnerId, AttachPlayerDaggerPutDown_R);
                        }
                        break;
                    case EquiState.DoubleBlades:
                        if (PlayerLogic.Instance.IsHands)
                        {
                            GameEntry.Entity.AttachEntityByFindChild(Entity, m_WeaponData.OwnerId, AttachPlayerDoubleBlades_R);
                        }
                        else
                        {
                            GameEntry.Entity.AttachEntityByFindChild(Entity, m_WeaponData.OwnerId, AttachPlayerDoubleBladesPutDown_R);
                        }
                        break;
                    case EquiState.Bow:
                        if (PlayerLogic.Instance.IsHands)
                        {
                            GameEntry.Entity.AttachEntityByFindChild(Entity, m_WeaponData.OwnerId, AttachPlayerBow_R);
                        }
                        else
                        {
                            GameEntry.Entity.AttachEntityByFindChild(Entity, m_WeaponData.OwnerId, AttachPlayerBowPutDown_R);
                        }
                        break;
                    case EquiState.RevengerDoubleBlades:
                        if (PlayerLogic.Instance.IsHands)
                        {
                            GameEntry.Entity.AttachEntityByFindChild(Entity, m_WeaponData.OwnerId, AttachPlayerRevengerDagger_R);
                        }
                        else
                        {
                            GameEntry.Entity.AttachEntityByFindChild(Entity, m_WeaponData.OwnerId, AttachPlayerRevengerDaggerPutDown_R);
                        }
                        break;
                    case EquiState.Gunner:
                        if (PlayerLogic.Instance.IsHands)
                        {
                            GameEntry.Entity.AttachEntityByFindChild(Entity, m_WeaponData.OwnerId, AttachPlayerGunner_R);
                        }
                        else
                        {
                            GameEntry.Entity.AttachEntityByFindChild(Entity, m_WeaponData.OwnerId, AttachPlayerGunnerPutDown_R);
                        }
                        break;
                    case EquiState.Pike:
                        if (PlayerLogic.Instance.IsHands)
                        {
                            GameEntry.Entity.AttachEntityByFindChild(Entity, m_WeaponData.OwnerId, AttachPlayerPike_R);
                        }
                        else
                        {
                            GameEntry.Entity.AttachEntityByFindChild(Entity, m_WeaponData.OwnerId, AttachPlayerPikePutDown_R);
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
        }

        protected override void OnAttachTo(EntityLogic parentEntity, Transform parentTransform, object userData)
        {
            base.OnAttachTo(parentEntity, parentTransform, userData);
            CachedTransform.localPosition = Vector3.zero;
            CachedTransform.localRotation = Quaternion.identity;
            CachedTransform.localScale = Vector3.one;
            Name = Utility.Text.Format("Weapon of {0}", parentEntity.Name);
        }

        protected override void OnDetachFrom(EntityLogic parentEntity, object userData)
        {
            base.OnDetachFrom(parentEntity, userData);
            CachedTransform.SetParent(m_WeaponPutDown);
            CachedTransform.localPosition = Vector3.zero;
            CachedTransform.localRotation = Quaternion.identity;
            CachedTransform.localScale = Vector3.one;
        }

        private void SetTransform(Vector3 position, Quaternion rotation,Vector3 scale)
        {
            CachedTransform.localPosition = position;
            CachedTransform.localRotation = rotation;
            CachedTransform.localScale = scale;

        }

        public  ImpactData GetImpactData()
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
