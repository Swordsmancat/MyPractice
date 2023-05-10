//------------------------------------------------------------
// Creator: FatCat
// Creation date: 2023/2/17/周五 17:00:57
//------------------------------------------------------------
using GameFramework;
using GameFramework.ObjectPool;
using UnityEngine;

namespace Farm.Hotfix
{
    public class DamageHUDItemObject : ObjectBase
    {

        public static DamageHUDItemObject Create(object target)
        {
            DamageHUDItemObject damageHUDItemObject = ReferencePool.Acquire<DamageHUDItemObject>();
            damageHUDItemObject.Initialize(target);
            return damageHUDItemObject;
        }
        
        protected override void Release(bool isShutdown)
        {
            DamageHUDItem damageHUDItem = (DamageHUDItem)Target;
            if(damageHUDItem == null)
            {
                return;
            }

            Object.Destroy(damageHUDItem.gameObject);
        }
    }
}
