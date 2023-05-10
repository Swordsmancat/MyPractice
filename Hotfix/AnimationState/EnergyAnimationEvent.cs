//------------------------------------------------------------
// Creator: FatCat
// Creation date: 2022/10/19/周三 18:03:42
//------------------------------------------------------------


using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace Farm.Hotfix
{
    public class EnergyAnimationEvent : StateMachineBehaviour
    {

        private TargetableObject owner;

        private PlayerLogic m_Player;

        private EnemyLogic m_Enemy;


        [SerializeField, LabelText("扣除精力值")]
        private float m_TakeEnergyValue;

        [SerializeField, LabelText("是否使用动画状态机预设扣减精力值")]
        private bool useAnimSetting = false;

        [LabelText("玩家技能属性编号"), InfoBox("只获取数据表中的精力值"), HideLabel, SerializeField]
        public int m_SkillGainNum;

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateEnter(animator, stateInfo, layerIndex);
            owner = animator.gameObject.GetComponent<TargetableObject>();
            m_Player = owner as PlayerLogic;
            if (m_Player != null)
            {
                if (!useAnimSetting && !m_SkillGainNum.Equals(0))         // 根据表中字段扣减相应的精力值
                {
                    switch (AIUtility.ContainsSkillGainTable(m_SkillGainNum))
                    {
                        case false:
                            {
                                if (GameEntry.DataTable.GetDataTable<DRPlayerSkillGain>().HasDataRow(m_SkillGainNum))
                                {
                                    AIUtility.AddSkillGainTable(m_SkillGainNum);
                                    m_Player.TakeEnergyValue(AIUtility.GetSkillGainTable(m_SkillGainNum).TakeEnergyValue);
                                }
                                else
                                {
                                    AnimationClip[] temp = animator.runtimeAnimatorController.animationClips;           // 处理技能加成倍数编号不存在的情况
                                    for (int i = 0; i < temp.Length; i++)
                                    {
                                        if (stateInfo.IsName(temp[i].name))
                                        {
                                            Log.Error(string.Format("不存在指定的玩家技能属性编号：{0}，角色动画名称：{1}，使用的技能名称：{2}", m_SkillGainNum, animator.name, temp[i].name));
                                            break;
                                        }
                                    }
                                    AIUtility.SetSkillGain(0);
                                }
                            }
                            break;
                        case true:
                            {
                                m_Player.TakeEnergyValue(AIUtility.GetSkillGainTable(m_SkillGainNum).TakeEnergyValue);
                            }
                            break;
                    }
                }
                else
                {
                    m_Player.TakeEnergyValue(m_TakeEnergyValue);
                }
            }
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateExit(animator, stateInfo, layerIndex);
         
        }
    }
}
