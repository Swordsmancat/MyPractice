//------------------------------------------------------------
// Creator: FatCat
// Creation date: 2023/2/17/周五 17:07:46
//------------------------------------------------------------
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;
using DG.Tweening;
using GameFramework;

namespace Farm.Hotfix
{
    public class EnergyBarItem : MonoBehaviour
    {
        private const float AnimationSeconds = 0.3f;
        private const float KeepSeconds = 2f;
        private const float FadeOutSeconds = 0.3f;

        [SerializeField]
        private Slider m_EnergyBar = null;

        private Canvas m_ParentCanvas = null;
        private RectTransform m_CachedTransform = null;
        private CanvasGroup m_CachedCanvasGroup = null;
        private Entity m_Owner = null;
        private int m_OwnerId = 0;

        private Vector2 m_DefaultVector =new Vector2(100f,180f);

        public Entity Owner
        {
            get
            {
                return m_Owner;
            }
        }

        public void Init(Entity owner,Canvas parentCanvas,float energyRatio)
        {
            if(owner == null)
            {
                Log.Error("Owner is invalid");
                return;
            }

            m_ParentCanvas = parentCanvas;
            gameObject.SetActive(true);
            StopAllCoroutines();

            m_CachedCanvasGroup.alpha = 1;
            if(m_Owner !=owner || m_OwnerId != owner.Id)
            {
                m_Owner = owner;
                m_OwnerId = owner.Id;
            }
            m_EnergyBar.value = energyRatio;
            //m_DefaultVector = new Vector2((Screen.width/1440f)* m_DefaultVector.x,(Screen.height/720f)*m_DefaultVector.y);
            Refresh();
            StartCoroutine(EnergyHUDItemCo(KeepSeconds, FadeOutSeconds));
        }

        public bool Refresh()
        {
            if (m_CachedCanvasGroup.alpha <= 0f)
            {
                return false;
            }

            if (m_Owner != null && Owner.Available && Owner.Id == m_OwnerId)
            {
                ////获得坐标和旋转
                //Vector3 worldPosition = m_Owner.CachedTransform.position + new Vector3(0f, 3f, 0f);
                //Quaternion worldRotation = GameEntry.Scene.MainCamera.transform.rotation;
                ////更新坐标和旋转 
                //m_CachedTransform.localPosition = worldPosition;
                //m_CachedTransform.localRotation = worldRotation;


                Vector3 worldPosition = m_Owner.CachedTransform.position;
                Vector3 screenPosition = GameEntry.Scene.MainCamera.WorldToScreenPoint(worldPosition);

                Vector2 position;
                if (RectTransformUtility.ScreenPointToLocalPointInRectangle((RectTransform)m_ParentCanvas.transform, screenPosition,
                    m_ParentCanvas.worldCamera, out position))
                {
                    m_CachedTransform.localPosition = position+ m_DefaultVector;
                }
            }

            return true;
        }

        public void Reset()
        {
            StopAllCoroutines();
            m_CachedCanvasGroup.alpha = 1f;
            m_Owner = null;
            gameObject.SetActive(false);
        }

        private void Awake()
        {
            m_CachedTransform = GetComponent<RectTransform>();
            if(m_CachedTransform == null)
            {
                Log.Error("RectTransform is invalid.");
                return;
            }

            m_CachedCanvasGroup = GetComponent<CanvasGroup>();
            if(m_CachedCanvasGroup == null)
            {
                Log.Error("CanvasGroup is invalid.");
                return;
            }
        }

        private IEnumerator EnergyHUDItemCo(float keepDuration, float fadeOutDuration)
        {
            
            yield return new WaitForSeconds(keepDuration);
            yield return m_CachedCanvasGroup.FadeToAlpha(0f, fadeOutDuration);
        }
    }
}
