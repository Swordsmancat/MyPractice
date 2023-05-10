//------------------------------------------------------------
// Creator: FatCat
// Creation date: 2022/9/29/周四 9:47:41
//------------------------------------------------------------
using UnityEngine;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;

namespace Farm.Hotfix
{
    public class BowAnimationEvent : StateMachineBehaviour
    {
        private TargetableObject owner;

        private PlayerLogic m_Player;

        private EnemyLogic m_Enemy;

        [LabelText("攻击方式"), SerializeField]
        private ArrowType m_ArrowType;

        [LabelText("箭矢速度"), SerializeField]
        private float m_ArrowSpeed;

        [LabelText("是否产生下坠"), SerializeField]
        private bool m_IsFalling;

        [LabelText("攻击伤害判断列表"), InfoBox("基于动画时间,填入以秒为单位"), SerializeField]
        private List<float> ShootTimelist;

        [LabelText("是否无视格挡"), SerializeField]
        private bool m_IgnoreParry;
       // [LabelText("是否无视弹反"), SerializeField]
        private bool m_IgnoreRebound;

        private List<int> m_ShootIDList;

        [SerializeField,LabelText("是否隐藏弓箭模型")]
        private bool m_ArrowIsHide;

        [SerializeField,LabelText("弓箭模型消失时间"),ShowIf("m_ArrowIsHide")]
        private float m_ArrowHideTime;

        [SerializeField, LabelText("是否显示弓箭模型")]
        private bool m_ArrowIsShow;
        [SerializeField,LabelText("弓箭模型显示时间"),ShowIf("m_ArrowIsShow")]
        private float m_ArrowShowTime;


        [SerializeField, LabelText("怪物投射实体编号")]
        private int m_EntityID;


        private int? m_HideHandArrowID = null;
        private int? m_ShowHandArrowID = null;


        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateEnter(animator, stateInfo, layerIndex);
            owner = animator.gameObject.GetComponent<TargetableObject>();
            m_ShootIDList = new List<int>();
            m_Player = owner as PlayerLogic;
            if(m_Player != null)
            {
                if (m_Player.IsPlayerInArrowSpecial())
                {
                    return;
                }
                for (int i = 0; i < ShootTimelist.Count; i++)
                {
                    m_ShootIDList.Add(GameEntry.Timer.AddOnceTimer((long)(ShootTimelist[i] * 1000), () => m_Player.ShootArrow(m_ArrowType,m_ArrowSpeed,m_IsFalling, m_IgnoreParry, m_IgnoreRebound)));
                }
                if (m_ArrowIsHide)
                {
                    m_HideHandArrowID = GameEntry.Timer.AddOnceTimer((long)(m_ArrowHideTime * 1000), () => m_Player.HideHandArrow());
                }
                if (m_ArrowIsShow)
                {
                    m_ShowHandArrowID = GameEntry.Timer.AddOnceTimer((long)(m_ArrowShowTime * 1000), () => m_Player.ShowHandArrow());
                }
            }
            else
            {
                m_Enemy = owner as EnemyLogic;
                for (int i = 0; i < ShootTimelist.Count; i++)
                {
                    m_ShootIDList.Add(GameEntry.Timer.AddOnceTimer((long)(ShootTimelist[i] * 1000), () => m_Enemy.ShootArrow(m_EntityID, m_ArrowSpeed, m_IsFalling, m_IgnoreParry, m_IgnoreRebound)));
                }
            }
          
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateExit(animator, stateInfo, layerIndex);
            for (int i = 0; i < m_ShootIDList.Count; i++)
            {
                if (GameEntry.Timer.IsExistTimer(m_ShootIDList[i]))
                {
                    GameEntry.Timer.CancelTimer(m_ShootIDList[i]);
                }
            }
            m_ShootIDList.Clear();

            if (m_ArrowIsHide)
            {
                if (m_HideHandArrowID != null)
                {
                    if (GameEntry.Timer.IsExistTimer((int)m_HideHandArrowID))
                    {
                        GameEntry.Timer.CancelTimer((int)m_HideHandArrowID);
                    }
                }
            }

            if (m_ArrowIsShow)
            {
                if (m_ShowHandArrowID != null)
                {
                    if (GameEntry.Timer.IsExistTimer((int)m_ShowHandArrowID))
                    {
                        GameEntry.Timer.CancelTimer((int)m_ShowHandArrowID);
                    }
                }
            }
        }

    }
}
