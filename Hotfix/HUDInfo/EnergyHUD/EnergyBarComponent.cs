//------------------------------------------------------------
// Creator: FatCat
// Creation date: 2023/3/6/周一 11:43:55
//------------------------------------------------------------
using GameFramework.ObjectPool;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace Farm.Hotfix
{
    public class EnergyBarComponent:GameFrameworkComponent
    {
        [SerializeField]
        private EnergyBarItem m_EnergyBarItemTemplate = null;

        [SerializeField]
        private Transform m_EnergyBarInstanceRoot = null;

        [SerializeField]
        private int m_InstancePoolCapacit = 2;

        private EnergyBarItem m_ActiveEnergyItem = null;
        private Canvas m_CachedCanvas = null;

        private void Start()
        {
            if(m_EnergyBarInstanceRoot == null)
            {
                Log.Error("You must set EnergyHUD instance root first.");
                return;
            }

            m_CachedCanvas = m_EnergyBarInstanceRoot.GetComponent<Canvas>();
        }

        private void Update()
        {
            if (m_ActiveEnergyItem != null)
            {
                if (m_ActiveEnergyItem.Refresh())
                {
                    return;
                }
                HideEnergyBar(m_ActiveEnergyItem);
            }
        }

        public void ShowEnergyBar(Entity entity,float energyValue)
        {
            if(entity == null)
            {
                Log.Warning("Entity is inValid.");
                return;
            }
            EnergyBarItem energyBarItem = CreateEnergyBarItem();
            m_ActiveEnergyItem = energyBarItem;
            energyBarItem.Init(entity, m_CachedCanvas, energyValue);
        }

        private void HideEnergyBar(EnergyBarItem energyBarItem)
        {
            energyBarItem.Reset();
          //  m_ActiveEnergyItems =null;
        }

        private EnergyBarItem CreateEnergyBarItem()
        {
            EnergyBarItem energyBarItem = null;
            energyBarItem = m_ActiveEnergyItem;
            if(energyBarItem == null)
            {
                energyBarItem = Instantiate(m_EnergyBarItemTemplate);
                Transform transform = energyBarItem.GetComponent<Transform>();
                transform.SetParent(m_EnergyBarInstanceRoot);
                transform.localScale = Vector3.one;
            }
            return energyBarItem;
        }
    }
}
