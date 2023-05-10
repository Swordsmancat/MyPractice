//------------------------------------------------------------
// Creator: FatCat
// Creation date: 2023/2/9/周四 10:53:00
//------------------------------------------------------------


using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace Farm.Hotfix
{
    public enum SkillEffectType
    {
        None,
        Shoot,
        AppearTarget,
        StraightWhirl,
    }

    public enum RevolveType
    {
        X,
        Y,
        Z,
    }

    [Serializable, SerializeField]
    public class SkillEffectTime
    {
        [LabelText("特效开始时间"), HideLabel]
        public float m_StartEffectTime;
        [LabelText("特效实体ID"), HideLabel]
        public int m_ID;
        [LabelText("特效持续时间"), HideLabel]
        public float m_KeepTime;
        [LabelText("伤害开始时间"), HideLabel]
        public float m_HitStartTime;
        [LabelText("伤害结束时间"), HideLabel]
        public float m_HitEndTime;
        [LabelText("伤害间隔"), HideLabel]
        public float m_HitIntervalTime;
        [LabelText("Buff"), HideLabel]
        public BuffType m_BuffTypeEnum;
        [LabelText("父节点名称"), HideLabel]
        public string m_ParentName;
        [LabelText("是否跟随"), HideLabel]
        public bool m_IsFollow;
        [LabelText("是否无视格挡"), SerializeField]
        public bool m_IgnoreParry;
        // [LabelText("是否无视弹反"), SerializeField]
        private bool m_IgnoreRebound;
        [LabelText("碰撞特效实体编号"), HideLabel]
        public int m_HitColliderEffectID;
        [LabelText("技能类型"), HideLabel]
        public SkillEffectType m_SkillType;
        [LabelText("飞行速度"), HideLabel, ShowIf("m_SkillType", SkillEffectType.Shoot)]
        public float m_FlySpeed;
        [LabelText("是否产生下坠"), HideLabel, ShowIf("m_SkillType", SkillEffectType.Shoot)]
        public bool m_IsFalling;
        [LabelText("出现在目标方位"), HideLabel, ShowIf("m_SkillType", SkillEffectType.AppearTarget)]
        public Vector3 m_AppearTargetPosition;
        [LabelText("回旋长度"), HideLabel, ShowIf("m_SkillType", SkillEffectType.StraightWhirl)]
        public float m_StraightWhirlLength;
        [LabelText("回旋停留时间"), HideLabel, ShowIf("m_SkillType", SkillEffectType.StraightWhirl)]
        public float m_StraightWhirlTime;
        [LabelText("飞行速度"), HideLabel, ShowIf("m_SkillType", SkillEffectType.StraightWhirl)]
        public float m_StraightWhirlFlySpeed;
        [LabelText("是否旋转"), HideLabel, ShowIf("m_SkillType", SkillEffectType.StraightWhirl)]
        public bool m_IsRevolve;
        [LabelText("旋转类型"), HideLabel, ShowIf("m_IsRevolve")]
        public RevolveType m_RevolveType;


    }
}
