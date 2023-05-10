using GameFramework;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace Farm.Hotfix
{
    public class EquipWeapon : WeaponLogic
    {
        PlayerLogic m_PlayerLogic;

        private Transform m_SwordShield_R;
        private Transform m_SwordShieldPutDown_R;
        private Transform m_GiantSword_R;
        private Transform m_GiantSwordPutDown_R;
        private Transform m_Dagger_R;
        private Transform m_DaggerPutDown_R;
        private Transform m_RevengerDagger_R;
        private Transform m_RevengerDaggerPutDown_R;
        private Transform m_DoubleBlades_R;
        private Transform m_DoubleBladesPutDown_R;
        private Transform m_Bow_R;
        private Transform m_BowPutDown_R;
        private Transform m_SwordShield_L;
        private Transform m_SwordShieldPutDown_L;
        private Transform m_GiantSword_L;
        private Transform m_GiantSwordPutDown_L;
        private Transform m_Dagger_L;
        private Transform m_DaggerPutDown_L;
        private Transform m_RevengerDagger_L;
        private Transform m_RevengerDaggerPutDown_L;
        private Transform m_DoubleBlades_L;
        private Transform m_DoubleBladesPutDown_L;
        private Transform m_Bow_L;
        private Transform m_BowPutDown_L;



        private const string SwordShield_R = "SwordShield_R";
        private const string SwordShieldPutDown_R = "SwordShieldPutDown_R";

        private const string GiantSword_R = "GiantSword_R";
        private const string GiantSwordPutDown_R = "GiantSwordPutDown_R";

        private const string Dagger_R = "Dagger_R";
        private const string DaggerPutDown_R = "DaggerPutDown_R";

        private const string RevengerDagger_R = "RevengerDagger_R";
        private const string RevengerDaggerPutDown_R = "RevengerDaggerPutDown_R";

        private const string DoubleBlades_R = "DoubleBlades_R";
        private const string DoubleBladesPutDown_R = "DoubleBladesPutDown_R";

        private const string Bow_R = "Bow_R";
        private const string BowPutDown_R = "BowPutDown_R";

        private const string SwordShield_L = "SwordShield_L";
        private const string SwordShieldPutDown_L = "SwordShieldPutDown_L";

        private const string GiantSword_L = "GiantSword_L";
        private const string GiantSwordPutDown_L = "GiantSwordPutDown_L";

        private const string Dagger_L = "Dagger_L";
        private const string DaggerPutDown_L = "DaggerPutDown_L";

        private const string RevengerDagger_L = "RevengerDagger_L";
        private const string RevengerDaggerPutDown_L = "RevengerDaggerPutDown_L";

        private const string DoubleBlades_L = "DoubleBlades_L";
        private const string DoubleBladesPutDown_L = "DoubleBladesPutDown_L";

        private const string Bow_L = "Bow_L";
        private const string BowPutDown_L = "BowPutDown_L";



        Transform RightWeapon;
        Transform LeftWeapon;



        private void Start()
        {
            m_PlayerLogic = gameObject.GetComponent<PlayerLogic>();
            m_SwordShield_R = GameEntry.Entity.FindFunc(transform, SwordShield_R);
            m_SwordShieldPutDown_R = GameEntry.Entity.FindFunc(transform, SwordShieldPutDown_R);
            m_GiantSword_R = GameEntry.Entity.FindFunc(transform, GiantSword_R);
            m_GiantSwordPutDown_R = GameEntry.Entity.FindFunc(transform, GiantSwordPutDown_R);
            m_Dagger_R = GameEntry.Entity.FindFunc(transform, Dagger_R);
            m_DaggerPutDown_R = GameEntry.Entity.FindFunc(transform, DaggerPutDown_R);
            m_RevengerDagger_R = GameEntry.Entity.FindFunc(transform, RevengerDagger_R);
            m_RevengerDaggerPutDown_R = GameEntry.Entity.FindFunc(transform, RevengerDaggerPutDown_R);
            m_DoubleBlades_R = GameEntry.Entity.FindFunc(transform, DoubleBlades_R);
            m_DoubleBladesPutDown_R = GameEntry.Entity.FindFunc(transform, DoubleBladesPutDown_R);
            m_Bow_R = GameEntry.Entity.FindFunc(transform, Bow_R);
            m_BowPutDown_R = GameEntry.Entity.FindFunc(transform, BowPutDown_R);
            m_SwordShield_L = GameEntry.Entity.FindFunc(transform, SwordShield_L);
            m_SwordShieldPutDown_L = GameEntry.Entity.FindFunc(transform, SwordShieldPutDown_L);
            m_GiantSword_L = GameEntry.Entity.FindFunc(transform, GiantSword_L);
            m_GiantSwordPutDown_L = GameEntry.Entity.FindFunc(transform, GiantSwordPutDown_L);
            m_Dagger_L = GameEntry.Entity.FindFunc(transform, Dagger_L);
            m_DaggerPutDown_L = GameEntry.Entity.FindFunc(transform, DaggerPutDown_L);
            m_RevengerDagger_L = GameEntry.Entity.FindFunc(transform, RevengerDagger_L);
            m_RevengerDaggerPutDown_L = GameEntry.Entity.FindFunc(transform, RevengerDaggerPutDown_L);
            m_DoubleBlades_L = GameEntry.Entity.FindFunc(transform, DoubleBlades_L);
            m_DoubleBladesPutDown_L = GameEntry.Entity.FindFunc(transform, DoubleBladesPutDown_L);
            m_Bow_L = GameEntry.Entity.FindFunc(transform, Bow_L);
            m_BowPutDown_L = GameEntry.Entity.FindFunc(transform, BowPutDown_L);
        }



        public void PutDownWeaponAnim()
        {
            switch (m_PlayerLogic.EquiState)
            {
                case EquiState.SwordShield:
                    SetWeaponParent(m_SwordShieldPutDown_R, m_SwordShieldPutDown_L, WeaponEnum.SwordShield_PutDown);
                    break;
                case EquiState.GiantSword:
                    SetWeaponParent(m_GiantSwordPutDown_R, null, WeaponEnum.GiantSword_PutDown);
                    break;
                case EquiState.Katana:
                    SetWeaponParent(m_DaggerPutDown_R, null, WeaponEnum.Dagger_PutDown);
                    break;
                case EquiState.DoubleBlades:
                    SetWeaponParent(m_DoubleBladesPutDown_R, m_DoubleBladesPutDown_L, WeaponEnum.DoubleBlades_PutDown);
                    break;
                case EquiState.Bow:
                    SetWeaponParent(m_BowPutDown_R, m_BowPutDown_L, WeaponEnum.Pistol_PutDown);
                    break;
                case EquiState.RevengerDoubleBlades:
                    SetWeaponParent(m_RevengerDaggerPutDown_R, m_RevengerDaggerPutDown_L, WeaponEnum.DoubleBlades_PutDown);
                    break;
                default:
                    break;
            }
        }

        public void TakeOutWeaponAnim()
        {
            switch (m_PlayerLogic.EquiState)
            {
                case EquiState.SwordShield:
                    SetWeaponParent(m_SwordShield_R, m_SwordShield_L, WeaponEnum.SwordShield);
                    break;
                case EquiState.GiantSword:
                    SetWeaponParent(m_GiantSword_R, null, WeaponEnum.GiantSword);
                    break;
                case EquiState.Katana:
                    SetWeaponParent(m_Dagger_R, null, WeaponEnum.Dagger);
                    break;
                case EquiState.DoubleBlades:
                    Debug.Log("TakeOutWeaponFour");
                    SetWeaponParent(m_DoubleBlades_R, m_DoubleBlades_L, WeaponEnum.DoubleBlades);
                    break;
                case EquiState.Bow:
                    SetWeaponParent(m_Bow_R, m_Bow_L, WeaponEnum.Pistol);
                    break;
                case EquiState.RevengerDoubleBlades:
                    SetWeaponParent(m_RevengerDagger_R, m_RevengerDagger_L, WeaponEnum.RevengerDoubleBlades);
                    break;
                default:
                    break;
            }
        }


        private void SetWeaponParent(Transform putPos1, Transform putPos2, WeaponEnum m_weaponEnum)
        {
            FindWeaponTrans();
            RightWeapon.SetParent(putPos1);
            if (LeftWeapon != null)
            {
                LeftWeapon.SetParent(putPos2);
            }
            SetWeaponTrans(RightWeapon, LeftWeapon, m_weaponEnum);
        }
        private void SetWeaponTrans(Transform R, Transform L, WeaponEnum we)
        {
            if (R)
            {
                R.localPosition = Vector3.zero;
                R.localRotation = Quaternion.identity;
                R.localScale = Vector3.one;
            }
            if (L)
            {
                L.localPosition = Vector3.zero;
                L.localRotation = Quaternion.identity;
                L.localScale = Vector3.one;

            }
            RightWeapon = null;
            LeftWeapon = null;
        }
        private void FindWeaponTrans()
        {
            for (int i = 0; i < m_PlayerLogic.Weapons.Count; i++)
            {
                if (m_PlayerLogic.Weapons[i] is WeaponLogicRightHand)
                {
                    RightWeapon = m_PlayerLogic.Weapons[i].transform;
                }
                if (m_PlayerLogic.Weapons[i] is WeaponLogicLeftHand)
                {
                    LeftWeapon = m_PlayerLogic.Weapons[i].transform;
                }
            }
        }

    }

}
