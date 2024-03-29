﻿using GameFramework.ObjectPool;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace Farm.Hotfix
{
   public class HPBarComponent :GameFrameworkComponent
    {
        [SerializeField]
        private HPBarItem m_HPBarItemTemplate = null;

        [SerializeField]
        private Transform m_HPBarInstanceRoot = null;

        [SerializeField]
        private int m_InstancePoolCapacity = 16;

        private IObjectPool<HPBarItemObject> m_HPBarItemObjectPool = null;
        private List<HPBarItem> m_ActiveHPBarItems = null;
        private Canvas m_CachedCanvas = null;

        private void Start()
        {
            if(m_HPBarInstanceRoot == null)
            {
                Log.Error("You must set HP bar instance root first.");
                return;
            }

            m_CachedCanvas = m_HPBarInstanceRoot.GetComponent<Canvas>();
            m_HPBarItemObjectPool = GameEntry.ObjectPool.CreateSingleSpawnObjectPool<HPBarItemObject>("HPBarItem", m_InstancePoolCapacity);
            m_ActiveHPBarItems = new List<HPBarItem>();
        }

        private void Update()
        {
            for (int i = m_ActiveHPBarItems.Count-1; i >=0; i--)
            {
                HPBarItem hPBarItem = m_ActiveHPBarItems[i];
                if (hPBarItem.Refresh())
                {
                    continue;
                }
                HideHPBar(hPBarItem);
            }
        }

        public void ShowHPBar(Entity entity ,float fromHPRatio,float toHPRatio,float fromTrunkRatio,float toTrunkRadtio)
        {
            if(entity == null)
            {
                Log.Warning("Entity is inValid.");
                return;
            }
            PlayerLogic player = entity as PlayerLogic;
            if(player != null)
            {
                return;
            }

            HPBarItem hpBarItem = GetActiveHPBarItem(entity);
            if(hpBarItem == null)
            {
                hpBarItem = CreateHPBarItem(entity);
                m_ActiveHPBarItems.Add(hpBarItem);
            }

            hpBarItem.Init(entity, m_CachedCanvas, fromHPRatio, toHPRatio,fromTrunkRatio,toTrunkRadtio);
        }

        public void ShowTrunkValue(Entity entity,float value)
        {
            if (entity == null)
            {
                Log.Warning("Entity is inValid.");
                return;
            }
            PlayerLogic player = entity as PlayerLogic;
            if (player != null)
            {
                return;
            }
            HPBarItem hpBarItem = GetActiveHPBarItem(entity);
            if (hpBarItem == null)
            {
                hpBarItem = CreateHPBarItem(entity);
                m_ActiveHPBarItems.Add(hpBarItem);
            }
            hpBarItem.InitTrunk(entity, m_CachedCanvas, value);
        }

        private void HideHPBar(HPBarItem hPBarItem)
        {
            hPBarItem.Reset();
            m_ActiveHPBarItems.Remove(hPBarItem);
            m_HPBarItemObjectPool.Unspawn(hPBarItem);
        }

        private HPBarItem GetActiveHPBarItem(Entity entity)
        {
            if(entity == null)
            {
                return null;
            }

            for (int i = 0; i < m_ActiveHPBarItems.Count; i++)
            {
                if(m_ActiveHPBarItems[i].Owner == entity)
                {
                    return m_ActiveHPBarItems[i];
                }
            }
            return null;
        }

        private HPBarItem CreateHPBarItem(Entity entity)
        {
            HPBarItem hpBarItem = null;
            HPBarItemObject hpBarItemObject = m_HPBarItemObjectPool.Spawn();
            if(hpBarItemObject != null)
            {
                hpBarItem = (HPBarItem)hpBarItemObject.Target;
            }
            else
            {
                hpBarItem = Instantiate(m_HPBarItemTemplate);
                Transform transform = hpBarItem.GetComponent<Transform>();
                transform.SetParent(m_HPBarInstanceRoot);
                transform.localScale = new Vector3(1f, 1f, 1f);
                m_HPBarItemObjectPool.Register(HPBarItemObject.Create(hpBarItem), true);
            }
            return hpBarItem;
        }

    }
}
