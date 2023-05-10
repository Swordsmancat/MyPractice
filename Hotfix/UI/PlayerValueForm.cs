using System;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityGameFramework.Runtime;
using UnityEngine.EventSystems;
using UnityEngine.Events;

namespace Farm.Hotfix
{
    public class PlayerValueForm : UGuiForm
    {

        [SerializeField]
        private Slider m_HPSlider;
        [SerializeField]
        private Slider m_TrunkSlider;
        [SerializeField]
        private Slider m_MoraleSlider;
        //[SerializeField]
        //private Slider m_MPSlider;

        [SerializeField]
        private GameObject m_Courage;



        private ProcedureMain m_procedureMain;


        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
        }

        

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            m_procedureMain = (ProcedureMain)userData;
            if (m_procedureMain == null)
            {
                Log.Warning("m_procedureMain is invalid when open LockFormPanel");
                return;
            }
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);

        }

        public void SetPlayerValue(float hp,float trunk)
        {
            m_HPSlider.value = hp;
            m_TrunkSlider.value = trunk;
        }

        public void SetMoraleValue(float morale)
        {
            m_MoraleSlider.value = morale;
        }

        //public void SetMPValue(float mp)
        //{
        //    m_MPSlider.value = mp;
        //}

        public void SetCourageValue(int value)
        {
            for (int i = 0; i < m_Courage.transform.childCount; i++)
            {
                m_Courage.transform.GetChild(i).gameObject.SetActive(false);
            }
            for (int i = 0; i < value; i++)
            {
                m_Courage.transform.GetChild(i).gameObject.SetActive(true);
            }
          
        }



    }
}
