
using GameFramework;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace Farm.Hotfix
{
    /// <summary>
    /// AI 工具类。
    /// </summary>
    public static class AIUtility
    {
        private static Dictionary<CampPair, RelationType> s_CampPairToRelation = new Dictionary<CampPair, RelationType>();
        private static Dictionary<KeyValuePair<CampType, RelationType>, CampType[]> s_CampAndRelationToCamps = new Dictionary<KeyValuePair<CampType, RelationType>, CampType[]>();
        private static Dictionary<int, DRPlayerSkillGain> skillGainDict = new Dictionary<int, DRPlayerSkillGain>();
        private static int skillGain = 0;

        static AIUtility()
        {
            s_CampPairToRelation.Add(new CampPair(CampType.Player, CampType.Player), RelationType.Friendly);
            s_CampPairToRelation.Add(new CampPair(CampType.Player, CampType.Enemy), RelationType.Hostile);
            s_CampPairToRelation.Add(new CampPair(CampType.Player, CampType.Neutral), RelationType.Neutral);
            s_CampPairToRelation.Add(new CampPair(CampType.Player, CampType.Player2), RelationType.Hostile);
            s_CampPairToRelation.Add(new CampPair(CampType.Player, CampType.Enemy2), RelationType.Hostile);
            s_CampPairToRelation.Add(new CampPair(CampType.Player, CampType.Neutral2), RelationType.Neutral);

            s_CampPairToRelation.Add(new CampPair(CampType.Enemy, CampType.Enemy), RelationType.Friendly);
            s_CampPairToRelation.Add(new CampPair(CampType.Enemy, CampType.Neutral), RelationType.Neutral);
            s_CampPairToRelation.Add(new CampPair(CampType.Enemy, CampType.Player2), RelationType.Hostile);
            s_CampPairToRelation.Add(new CampPair(CampType.Enemy, CampType.Enemy2), RelationType.Hostile);
            s_CampPairToRelation.Add(new CampPair(CampType.Enemy, CampType.Neutral2), RelationType.Neutral);

            s_CampPairToRelation.Add(new CampPair(CampType.Neutral, CampType.Neutral), RelationType.Neutral);
            s_CampPairToRelation.Add(new CampPair(CampType.Neutral, CampType.Player2), RelationType.Neutral);
            s_CampPairToRelation.Add(new CampPair(CampType.Neutral, CampType.Enemy2), RelationType.Neutral);
            s_CampPairToRelation.Add(new CampPair(CampType.Neutral, CampType.Neutral2), RelationType.Hostile);

            s_CampPairToRelation.Add(new CampPair(CampType.Player2, CampType.Player2), RelationType.Friendly);
            s_CampPairToRelation.Add(new CampPair(CampType.Player2, CampType.Enemy2), RelationType.Hostile);
            s_CampPairToRelation.Add(new CampPair(CampType.Player2, CampType.Neutral2), RelationType.Neutral);

            s_CampPairToRelation.Add(new CampPair(CampType.Enemy2, CampType.Enemy2), RelationType.Friendly);
            s_CampPairToRelation.Add(new CampPair(CampType.Enemy2, CampType.Neutral2), RelationType.Neutral);

            s_CampPairToRelation.Add(new CampPair(CampType.Neutral2, CampType.Neutral2), RelationType.Neutral);
        }

        /// <summary>
        /// 获取两个阵营之间的关系。
        /// </summary>
        /// <param name="first">阵营一。</param>
        /// <param name="second">阵营二。</param>
        /// <returns>阵营间关系。</returns>
        public static RelationType GetRelation(CampType first, CampType second)
        {
            if (first > second)
            {
                CampType temp = first;
                first = second;
                second = temp;
            }

            RelationType relationType;
            if (s_CampPairToRelation.TryGetValue(new CampPair(first, second), out relationType))
            {
                return relationType;
            }

            Log.Warning("Unknown relation between '{0}' and '{1}'.", first.ToString(), second.ToString());
            return RelationType.Unknown;
        }

        /// <summary>
        /// 获取和指定具有特定关系的所有阵营。
        /// </summary>
        /// <param name="camp">指定阵营。</param>
        /// <param name="relation">关系。</param>
        /// <returns>满足条件的阵营数组。</returns>
        public static CampType[] GetCamps(CampType camp, RelationType relation)
        {
            KeyValuePair<CampType, RelationType> key = new KeyValuePair<CampType, RelationType>(camp, relation);
            CampType[] result = null;
            if (s_CampAndRelationToCamps.TryGetValue(key, out result))
            {
                return result;
            }

            // TODO: GC Alloc
            List<CampType> camps = new List<CampType>();
            Array campTypes = Enum.GetValues(typeof(CampType));
            for (int i = 0; i < campTypes.Length; i++)
            {
                CampType campType = (CampType)campTypes.GetValue(i);
                if (GetRelation(camp, campType) == relation)
                {
                    camps.Add(campType);
                }
            }

            // TODO: GC Alloc
            result = camps.ToArray();
            s_CampAndRelationToCamps[key] = result;

            return result;
        }

        /// <summary>
        /// 获取实体间的距离。
        /// </summary>
        /// <returns>实体间的距离。</returns>
        public static float GetDistance(Entity fromEntity, Entity toEntity)
        {
            Transform fromTransform = fromEntity.CachedTransform;
            Transform toTransform = toEntity.CachedTransform;
            return (toTransform.position - fromTransform.position).magnitude;
        }

        /// <summary>
        /// 获取实体间的距离。
        /// </summary>
        /// <returns>实体间的距离。</returns>
        public static float GetDistance(Entity fromEntity, UnityGameFramework.Runtime.Entity toEntity)
        {
            Transform fromTransform = fromEntity.transform;
            Transform toTransform = toEntity.transform;
            return (toTransform.position - fromTransform.position).magnitude;
        }

        /// <summary>
        /// 获取两个向量之间距离 (多余函数  修改完可删)
        /// </summary>
        /// <param name="a">向量a</param>
        /// <param name="b">向量b</param>
        /// <returns>返回距离</returns>
        public static float GetDistance(in Vector3 a, in Vector3 b)
        {
            return (a - b).magnitude;
        }

        /// <summary>
        /// 获取实体与坐标之间的距离。
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static float GetDistance(Entity fromEntity, in Vector3 to)
        {
            return (fromEntity.CachedTransform.position - to).magnitude;
        }

        /// <summary>
        /// 获得水平距离忽略Y轴
        /// </summary>
        /// <param name="from">原点</param>
        /// <param name="to">目标</param>
        /// <returns>水平距离忽略Y轴</returns>
        public static float GetDistanceNoYAxis(in Vector3 from, in Vector3 to)
        {
            Vector3 a = from;
            Vector3 b = to;

            a.y = 0;
            b.y = 0;

            return (a - b).magnitude;
        }

        public static Vector2 GetAttackerDir(Entity entity, Entity other)
        {
            Vector2 dir = Vector2.zero;
            dir.y = GetDot(entity, other);
            dir.x = GetCross(entity, other);
            return dir;
        }



        /// <summary>
        /// 获取实体平面的距离 (多余函数 修改完可删)
        /// </summary>
        /// <param name="posOne">位置1</param>
        /// <param name="posTwo">位置2</param>
        /// <returns>如果实体不在一平面上则返回0，如果在则正常返回距离</returns>
        public static float GetCheckPlaneDistance(Transform posOne, Transform posTwo)
        {
            if (Mathf.Abs(posOne.position.y - posTwo.position.y) > posOne.localScale.y + 1.0f)
            {
                //不在一个平面上
                return 0;
            }

            return Mathf.Abs((posOne.position - posTwo.position).magnitude);
        }

        /// <summary>
        /// 获取实体间的夹角
        /// </summary>
        /// <param name="fromEntity">当前实体</param>
        /// <param name="toEntity">目标实体</param>
        /// <returns>实体间的夹角</returns>
        public static float GetAngleInSeek(Entity fromEntity, Entity toEntity)
        {
            Vector3 direction = toEntity.transform.position - fromEntity.transform.position;
            float degree = Vector3.Angle(direction, fromEntity.transform.forward);
            return degree;
        }

        public static float GetDot(Entity fromEntity, Entity toEntity)
        {
            Vector3 direction = toEntity.transform.position - fromEntity.transform.position;
            return Vector3.Dot(fromEntity.transform.forward.normalized, direction.normalized);
        }

        public static float GetCross(Entity fromEntity, Entity toEntity)
        {
            Vector3 direction = toEntity.transform.position - fromEntity.transform.position;
            return Vector3.Cross(fromEntity.transform.forward.normalized, direction.normalized).y;
        }

        /// <summary>
        /// 获取实体间的夹角
        /// </summary>
        /// <param name="fromEntity">当前实体</param>
        /// <param name="toEntity">目标实体</param>
        /// <returns>实体间的夹角</returns>
        public static float GetAngleInSeek(Entity fromEntity, UnityGameFramework.Runtime.Entity toEntity)
        {
            Vector3 direction = toEntity.transform.position - fromEntity.transform.position;
            float degree = Vector3.Angle(direction, fromEntity.transform.forward);
            return degree;
        }

        /// <summary>
        /// 返回目标的水平夹角
        /// </summary>
        /// <param name="from">当前位置</param>
        /// <param name="to">目标位置</param>
        /// <returns>负为左，正为右</returns>
        public static float GetPlaneAngle(in Entity from, in Entity to)
        {
            Vector3 TargetDir = from.CachedTransform.position - to.CachedTransform.position;
            TargetDir.y = 0;
            return Vector3.SignedAngle(TargetDir, to.CachedTransform.forward, Vector3.up);
        }

        /// <summary>
        /// 返回目标的水平夹角
        /// </summary>
        /// <param name="from">当前位置</param>
        /// <param name="to">目标位置</param>
        /// <returns>负为左，正为右</returns>
        public static float GetPlaneAngle(in Entity from, in UnityGameFramework.Runtime.Entity to)
        {
            Vector3 TargetDir = from.transform.position - to.transform.position;
            TargetDir.y = 0;
            return Vector3.SignedAngle(TargetDir, to.transform.forward, Vector3.up);
        }

        /// <summary>
        /// 判断夹角是否在指定范围内
        /// </summary>
        /// <param name="mixAgnle">最小角度范围</param>
        /// <param name="maxAngle">最大角度范围</param>
        /// <param name="Angle">角度</param>
        /// <returns>如果在范围内返回真,在范围外返回假</returns>
        public static bool CheckInAngle(in float mixAgnle, in float maxAngle, in float Angle)
        {
            if (Angle > mixAgnle && Angle < maxAngle)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// 敌人绕Y轴旋转面向玩家不会作用在X轴上
        /// </summary>
        /// <param name="from">玩家实体</param>
        /// <param name="to">怪物实体</param>
        /// <param name="minAngle">最小开始旋转角度</param>
        /// <param name="maxAgnle">最大开始旋转角度</param>
        /// <param name="rotateSpeed">旋转速度</param>
        /// <param name="turnTime">旋转时间</param>
        public static void RotateToTarget(Entity from, Entity to,
             float minAngle, float maxAgnle,
             float rotateSpeed = 6f, float turnTime = 600f)
        {
            //获取怪物和玩家的平面夹角
            float angle = GetPlaneAngle(from, to);
            if (angle >= maxAgnle)
            {
                //向左旋转
                Quaternion targetRotate = Quaternion.Euler(to.transform.localEulerAngles.x, to.transform.localEulerAngles.y - rotateSpeed, to.transform.localEulerAngles.z);
                to.transform.rotation = Quaternion.RotateTowards(to.transform.rotation, targetRotate, turnTime * Time.deltaTime);
            }
            else if (angle <= minAngle)
            {
                //向右旋转
                Quaternion targetRotate = Quaternion.Euler(to.transform.localEulerAngles.x, to.transform.localEulerAngles.y + rotateSpeed, to.transform.localEulerAngles.z);
                to.transform.rotation = Quaternion.RotateTowards(to.transform.rotation, targetRotate, turnTime * Time.deltaTime);
            }

        }

        /// <summary>
        /// 平滑移动
        /// </summary>
        /// <param name="from">当前位置</param>
        /// <param name="to">目标位置</param>
        /// <param name="owner">敌人对象</param>
        /// <param name="speed">移动速度(时间区间)</param>
        public static void SmoothMove(Vector3 from, Vector3 to, EnemyLogic owner, float speed = 1)
        {
            owner.transform.position = Vector3.Lerp(from, to, Time.deltaTime * speed);
        }

        public static void SmoothMove(Vector3 from, Vector3 to, GameObject owner, float speed = 1)
        {
            owner.transform.position = Vector3.Lerp(from, to, Time.deltaTime * speed);
        }

        public static void PerformCollisionBow(TargetableObject entity, Entity other, Vector3 point, ArrowType arrowType, TargetableObject owner)
        {
            ArrowLogic arrow = other as ArrowLogic;
            if (arrow != null)
            {
                ImpactData entityImpactData = entity.GetImpactData();
                ImpactData bulletImpactData = arrow.GetImpactData();
                if (GetRelation(entityImpactData.Camp, bulletImpactData.Camp) == RelationType.Friendly)
                {
                    return;
                }
                int entityDamageHP = CalcDamageHP(bulletImpactData.Attack, entityImpactData.Defense);
                float critRateNum = Utility.Random.GetRandom(0, 10) / 10f;
                if (critRateNum < owner.TargetableObjectData.CritRate)
                {
                    entityDamageHP = entityDamageHP * 2;
                    entity.IsGetCrit = true;
                }
                else
                {
                    entity.IsGetCrit = false;
                }
                int entityDamageTrunk = CalcDamageTrunkArrow(arrowType);

                TargetBuff targetBuff = new TargetBuff();
                targetBuff.Target = entity;
                targetBuff.Buff = owner.Buff;
                targetBuff.Buff.BuffTypeEnum = BuffType.None;
                entity.GetBuffType =BuffType.None;
                GameEntry.Event.Fire(owner, ApplyBuffEventArgs.Create(targetBuff));
                entity.ApplyDamage(owner, other, entityDamageHP, entityDamageTrunk, point);

            }
        }

        public static void PerformCollisionAttack(TargetableObject entity, TargetableObject other, Vector3 point,Entity attackType =null)
        {
            if (entity == null || other == null)
            {
                return;
            }
            ImpactData ownerImpactData = entity.GetImpactData();
            ImpactData otherImpactData = other.GetImpactData();
            if (GetRelation(otherImpactData.Camp, ownerImpactData.Camp) == RelationType.Friendly)
            {
                return;
            }
            int entityDamageHP;
            int entityDamageTrunk;
            if (entity.tag.Equals("Player") && skillGain > 0.1f && skillGainDict.ContainsKey(skillGain))            // 玩家使用技能击中目标后，且伤害加成大于0时，将会使技能伤害按照指定倍数进行增减
            {
                if ((other as EnemyLogic).enemyData.TrunkValue < 0.01f)            // 躯干值为空时，应用配置表中的空躯干值伤害加成
                {
                    entityDamageHP = CalcDamageHP(ownerImpactData.Attack, otherImpactData.Defense, skillGainDict[skillGain].TrunkNullDamageGain);
                }
                else
                {
                    entityDamageHP = CalcDamageHP(ownerImpactData.Attack, otherImpactData.Defense, skillGainDict[skillGain].DamageGain);
                }
                entityDamageTrunk = skillGainDict[skillGain].TakeTrunkValue;            // 扣减躯干值
            }
            else
            {
                entityDamageHP = CalcDamageHP(ownerImpactData.Attack, otherImpactData.Defense);
                entityDamageTrunk = entity.TakeTrunkValue;
            }
            float critRateNum = Utility.Random.GetRandom(0, 10) / 10f;
            if (critRateNum < entity.TargetableObjectData.CritRate)
            {
                entityDamageHP = entityDamageHP * 2;
                other.IsGetCrit = true;
               
            }
            else 
            {
                other.IsGetCrit = false;
            }

            TargetBuff targetBuff = new TargetBuff();
            targetBuff.Target = other;
            targetBuff.Buff = entity.Buff;
            GameEntry.Event.Fire(entity, ApplyBuffEventArgs.Create(targetBuff));
            other.GetBuffType = entity.Buff.BuffTypeEnum;
            
            other.ApplyDamage(entity, attackType, entityDamageHP, entityDamageTrunk, point);
            return;
        }

        // 设置技能伤害加成
        public static void SetSkillGain(int gain)
        {
            if (!skillGainDict.ContainsKey(0))
            {
                skillGainDict.Add(0, new DRPlayerSkillGain());
            }
            skillGain = gain;
        }

        /// <summary>
        /// 检测是否包含技能伤害加成数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool ContainsSkillGainTable(int id)
        {
            return skillGainDict.ContainsKey(id);
        }

        public static void AddSkillGainTable(int id)
        {
            skillGainDict.Add(id, GameEntry.DataTable.GetDataTable<DRPlayerSkillGain>().GetDataRow(id));
        }

        public static DRPlayerSkillGain GetSkillGainTable(int id)
        {
            if (skillGainDict.ContainsKey(id))
                return skillGainDict[id];
            else
            {
                Log.Error(string.Format("技能伤害配置表中不包含id为“{0}”的数据", id));
                return skillGainDict[0];
            }
        }

        private static int CalcDamageTrunkArrow(ArrowType arrowType)
        {
            switch (arrowType)
            {
                case ArrowType.None:
                    return Utility.Random.GetRandom(8, 16);
                    break;
                case ArrowType.Fire:
                    return Utility.Random.GetRandom(15, 18);
                    break;
                case ArrowType.Pow:
                    return Utility.Random.GetRandom(15, 18);
                    break;
                default:
                    return Utility.Random.GetRandom(22, 26);
                    break;
            }
        }

        private static int CalcDamageHP(int attack, int defense)
        {
            if (attack <= 0)
            {
                return 0;
            }

            if (defense < 0)
            {
                defense = 0;
            }

            return attack * attack / (attack + defense);
        }

        private static int CalcDamageHP(int attack, int defense, float gain)
        {
            if (attack <= 0)
            {
                return 0;
            }

            if (defense < 0)
            {
                defense = 0;
            }

            return (int)(attack * attack / (attack + defense) * gain);
        }

        [StructLayout(LayoutKind.Auto)]
        private struct CampPair
        {
            private readonly CampType m_First;
            private readonly CampType m_Second;

            public CampPair(CampType first, CampType second)
            {
                m_First = first;
                m_Second = second;
            }

            public CampType First
            {
                get
                {
                    return m_First;
                }
            }

            public CampType Second
            {
                get
                {
                    return m_Second;
                }
            }
        }
    }
}
