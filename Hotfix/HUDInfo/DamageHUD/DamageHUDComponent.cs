//------------------------------------------------------------
// Creator: FatCat
// Creation date: 2023/2/20/周一 16:44:40
//------------------------------------------------------------
using GameFramework.ObjectPool;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace Farm.Hotfix
{
    public class DamageHUDComponent:GameFrameworkComponent
    {
        [SerializeField]
        private DamageHUDItem m_DamageHUDItemTemplate = null;

        [SerializeField]
        private Transform m_DamageHUDInstanceRoot = null;

        [SerializeField]
        private int m_InstancePoolCapacit = 32;

        private IObjectPool<DamageHUDItemObject> m_DamageItemObjectPool = null;
        private List<DamageHUDItem> m_ActiveDamageItems = null;
        private Canvas m_CachedCanvas = null;

        private void Start()
        {
            if(m_DamageHUDInstanceRoot == null)
            {
                Log.Error("You must set DamageHUD instance root first.");
                return;
            }

            m_CachedCanvas = m_DamageHUDInstanceRoot.GetComponent<Canvas>();
            m_DamageItemObjectPool = GameEntry.ObjectPool.CreateSingleSpawnObjectPool<DamageHUDItemObject>("DamageHUDItem", m_InstancePoolCapacit);
            m_ActiveDamageItems = new List<DamageHUDItem>();
        }

        private void Update()
        {
            for (int i = m_ActiveDamageItems.Count -1; i>=0; i--)
            {
                DamageHUDItem damageHUDItem = m_ActiveDamageItems[i];
                if (damageHUDItem.Refresh())
                {
                    continue;
                }
                HideDamageHUD(damageHUDItem);
            }
        }

        public void ShowDamageHUD(Entity entity,float takeDamage,Color color,Vector3 point,bool isGetCrit)
        {
            if(entity == null)
            {
                Log.Warning("Entity is inValid.");
                return;
            }
            DamageHUDItem damageHUDItem = CreateDamageHUDItem(entity);
            m_ActiveDamageItems.Add(damageHUDItem);
            damageHUDItem.Init(entity, m_CachedCanvas, takeDamage, color, point, isGetCrit);
        }


        private void HideDamageHUD(DamageHUDItem damageHUDItem)
        {
            damageHUDItem.Reset();
            m_ActiveDamageItems.Remove(damageHUDItem);
            m_DamageItemObjectPool.Unspawn(damageHUDItem);
        }

        private DamageHUDItem CreateDamageHUDItem(Entity entity)
        {
            DamageHUDItem damageHUDItem = null;
            DamageHUDItemObject damageHUDItemObject = m_DamageItemObjectPool.Spawn();
            if(damageHUDItemObject != null)
            {
                damageHUDItem = (DamageHUDItem)damageHUDItemObject.Target;
            }
            else
            {
                damageHUDItem = Instantiate(m_DamageHUDItemTemplate);
                Transform transform = damageHUDItem.GetComponent<Transform>();
                transform.SetParent(m_DamageHUDInstanceRoot);
                transform.localScale = new Vector3(1f, 1f, 1f);
                m_DamageItemObjectPool.Register(DamageHUDItemObject.Create(damageHUDItem), true);
            }
            return damageHUDItem;
        }

    }
}
