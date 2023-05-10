//------------------------------------------------------------
// Creator: FatCat
// Creation date: 2022/10/27/周四 18:10:57
//------------------------------------------------------------
using UnityEngine;
using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;

namespace Farm.Hotfix
{
    public class ColliderControl:SerializedMonoBehaviour
    {
        [SerializeField,]
        public Dictionary<ColliderState ,Collider> m_ColliderDict;

       

    }



}
