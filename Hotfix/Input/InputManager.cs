using Farm.Hotfix;
using System;
using UnityEngine;

namespace Farm
{
   public static class InputManager
    {

        public static EquiState ChangeWeapon()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                return EquiState.SwordShield;
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                return EquiState.GiantSword;
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                return EquiState.Katana;
            }
            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                return EquiState.DoubleBlades;
            }
            if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                return EquiState.Bow;
            }
            if (Input.GetKeyDown(KeyCode.Alpha6))
            {
                return EquiState.Gunner;
            }
            if (Input.GetKeyDown(KeyCode.Alpha7))
            {
                return EquiState.Pike;
            }
            return EquiState.None;
        }
        public static void IsOnGame()
        {
            if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            if (Input.GetKeyDown("escape"))
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = true;
            }
        }

        public static bool IsAccelerate()
        {
#if UNITY_EDITOR
            if (Input.GetKey(KeyCode.LeftShift))
            {
                return true;
            }
            return false;
#endif
        }

        public static bool IsClickDownMouseLeft()
        {
#if UNITY_EDITOR
            if (Input.GetMouseButtonDown(0))
            {
                return true;
            }
#endif
            return false;
        }

        public static bool IsClickUpMouseLeft()
        {
#if UNITY_EDITOR
            if (Input.GetMouseButtonUp(0))
            {
                return true;
            }
#endif
            return false;
        }

        public static bool IsClickUpMouseRight()
        {
#if UNITY_EDITOR
            if (Input.GetMouseButtonUp(1))
            {
                return true;
            }
#endif
            return false;
        }

        public static bool IsClickHoldMouserLeft()
        {
#if UNITY_EDITOR
            if (Input.GetMouseButton(0))
            {
                return true;
            }
#endif
            return false;
        }

        public static bool IsClickDownMouseRight()
        {
#if UNITY_EDITOR
            if (Input.GetMouseButtonDown(1))
            {
                return true;
            }
#endif
            return false;
        }

        public static bool IsClickHoldMouseRight()
        {
#if UNITY_EDITOR
            if (Input.GetMouseButton(1))
            {
                return true;
            }
#endif
            return false;
        }


        public static bool IsLoosenAttack()
        {
#if UNITY_EDITOR
            if (Input.GetMouseButtonUp(0))
            {
                return true;
            }
#endif
            return false;
        }

        public static bool IsAttackThump()
        {
#if UNITY_EDITOR
            if (Input.GetKeyDown(KeyCode.C))
            {
                return true;
            }
#endif
            return false;
        }

        public static bool IsExplosion()
        {
#if UNITY_EDITOR
            if (Input.GetKeyDown(KeyCode.V))
            {
                return true;
            }
#endif

            return false;
        }

        public static bool IsBowAim()
        {
#if UNITY_EDITOR
            if (Input.GetKeyDown(KeyCode.T))
            {
                return true;
            }
#endif

            return false;
        }

        public static bool IsClickDefense()
        {
#if UNITY_EDITOR
            if (Input.GetKeyDown(KeyCode.Q)||Input.GetKey(KeyCode.Q))
            {
                return true;
            }
#endif 
            return false;
        }


        public static bool IsClickDodge()
        {
#if UNITY_EDITOR
            if (Input.GetKeyDown(KeyCode.Space))
            {
                return true;
            }
#endif
            return false;
        }

        public static bool IsClickLock()
        {
#if UNITY_EDITOR
            if (Input.GetMouseButtonDown(2))
            {
                return true;
            }
#endif
            return false;
        }

        public static bool IsClickStep()
        {
#if UNITY_EDITOR
            if (Input.GetKeyDown(KeyCode.I))
            {
                return true;
            }
#endif
            return false;
        }       
        public static bool IsWhirlwind()
        {
#if UNITY_EDITOR
            if (Input.GetKeyDown(KeyCode.O))
            {
                return true;
            }
#endif
            return false;
        }

        public static bool IsClickCourage()
        {
#if UNITY_EDITOR
            if (Input.GetKeyDown(KeyCode.F))
            {
                return true;
            }
#endif
            return false;
        }


    }
}
