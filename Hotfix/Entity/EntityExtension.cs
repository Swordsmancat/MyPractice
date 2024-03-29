﻿//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using GameFramework.DataTable;
using System;
using UnityGameFramework.Runtime;

namespace Farm.Hotfix
{
    public static class EntityExtension
    {
        // 关于 EntityId 的约定：
        // 0 为无效
        // 正值用于和服务器通信的实体（如玩家角色、NPC、怪等，服务器只产生正值）
        // 负值用于本地生成的临时实体（如特效、FakeObject等）
        private static int s_SerialId = 0;

        public static Entity GetGameEntity(this EntityComponent entityComponent, int entityId)
        {
            UnityGameFramework.Runtime.Entity entity = entityComponent.GetEntity(entityId);
            if (entity == null)
            {
                return null;
            }

            return (Entity)entity.Logic;
        }

        public static void HideEntity(this EntityComponent entityComponent, Entity entity)
        {
            entityComponent.HideEntity(entity.Entity);
        }

        public static void AttachEntity(this EntityComponent entityComponent, Entity entity, int ownerId, string parentTransformPath = null, object userData = null)
        {
            entityComponent.AttachEntity(entity.Entity, ownerId, parentTransformPath, userData);
        }

        public static void ShowMyPlayer(this EntityComponent entityComponent, MyPlayerData data)
        {
            entityComponent.ShowEntity(typeof(MyPlayerLogic), "Character", Constant.AssetPriority.MyCharacterAsset, data);
        }

        //public static void ShowAircraft(this EntityComponent entityComponent, AircraftData data)
        //{
        //    entityComponent.ShowEntity(typeof(Aircraft), "Aircraft", Constant.AssetPriority.AircraftAsset, data);
        //}

        //public static void ShowThruster(this EntityComponent entityComponent, ThrusterData data)
        //{
        //    entityComponent.ShowEntity(typeof(Thruster), "Thruster", Constant.AssetPriority.ThrusterAsset, data);
        //}

        public  enum  WeaponHand
        {
            Left,
            Right,
            SubLeft,
            SubRight,
        }

        public static void ShowWeapon(this EntityComponent entityComponent, WeaponData data, WeaponHand hand,object userData =null)
        {
            switch (hand)
            {
                case WeaponHand.Left:
                    entityComponent.ShowEntity(typeof(WeaponLogicLeftHand), "Weapon", Constant.AssetPriority.WeaponAsset, data);
                    break;
                case WeaponHand.Right:
                    entityComponent.ShowEntity(typeof(WeaponLogicRightHand), "Weapon", Constant.AssetPriority.WeaponAsset, data);
                    break;
                case WeaponHand.SubLeft:
                    entityComponent.ShowEntity(typeof(WeaponSubLogicLeftHand), "Weapon", Constant.AssetPriority.WeaponAsset, data);
                    break;
                case WeaponHand.SubRight:
                    entityComponent.ShowEntity(typeof(WeaponSubLogicRightHand), "Weapon", Constant.AssetPriority.WeaponAsset, data);
                    break;
                default:
                    break;
            }
        }

        public static void ShowArmor(this EntityComponent entityComponent, ArmorData data)
        {
            entityComponent.ShowEntity(typeof(Armor), "Armor", Constant.AssetPriority.ArmorAsset, data);
        }

        public static void ShowEnemy(this EntityComponent entityCompoennt, EnemyData data)
        {
            entityCompoennt.ShowEntity(typeof(EnemyLogic), "Enemy", Constant.AssetPriority.EnemyAsset, data);
        }

        public static void ShowNPC(this EntityComponent entityComponent, NPCData data)
        {
            entityComponent.ShowEntity(typeof(NPCLogic), "NPC", Constant.AssetPriority.NPCAsset, data);
        }

        public static void ShowEnemy(this EntityComponent entityCompoennt, EnemyData data,Type type)
        {
            entityCompoennt.ShowEntity(type, "Enemy", Constant.AssetPriority.EnemyAsset, data);
        }

        public static void ShowSkillEffect(this EntityComponent entityComponent, SkillEffectData data)
        {
            entityComponent.ShowEntity(typeof(SkillEffectLogic), "Effect", Constant.AssetPriority.EnemyAsset, data);
        }

        public static void ShowArrow(this EntityComponent entityCompoennt, ArrowData data)
        {
            entityCompoennt.ShowEntity(typeof(ArrowLogic), "Arrow", Constant.AssetPriority.BulletAsset, data);
        }

        public static void ShowArrow(this EntityComponent entityCompoennt, ArrowData data,Type type)
        {
            entityCompoennt.ShowEntity(type, "Arrow", Constant.AssetPriority.BulletAsset, data);
        }


        //默认为效果基类逻辑
        public static void ShowEffect(this EntityComponent entityComponent, EffectData data)
        {
            entityComponent.ShowEntity(typeof(EffectLogic), "Effect", Constant.AssetPriority.EffectAsset, data);
        }
        
        
        public static void ShowEffect(this EntityComponent entityComponent, EffectData data, Type type)
        {
            entityComponent.ShowEntity(type, "Effect", Constant.AssetPriority.EffectAsset, data);
        }

        private static void ShowEntity(this EntityComponent entityComponent, Type logicType, string entityGroup, int priority, EntityData data)
        {
            if (data == null)
            {
                Log.Warning("Data is invalid.");
                return;
            }

            IDataTable<DREntity> dtEntity = GameEntry.DataTable.GetDataTable<DREntity>();
            DREntity drEntity = dtEntity.GetDataRow(data.TypeId);
            if (drEntity == null)
            {
                Log.Warning("Can not load entity id '{0}' from data table.", data.TypeId.ToString());
                return;
            }
            entityComponent.ShowEntity(data.Id, logicType, AssetUtility.GetEntityAsset(drEntity.AssetName), entityGroup, priority, data);
        }


        public static int GenerateSerialId(this EntityComponent entityComponent)
        {
            return --s_SerialId;
        }
    }
}
