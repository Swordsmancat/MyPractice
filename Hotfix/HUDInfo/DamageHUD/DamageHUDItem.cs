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
    public class DamageHUDItem:MonoBehaviour
    {
        private const float AnimationSeconds = 0.3f;
        private const float KeepSeconds = 0.5f;
        private const float FadeOutSeconds = 0.3f;

        [SerializeField]
        private Text m_Text = null;

        private Canvas m_ParentCanvas = null;
        private RectTransform m_CachedTransform = null;
        private CanvasGroup m_CachedCanvasGroup = null;
        private Entity m_Owner = null;
        private int m_OwnerId = 0;

        private Vector3 m_Point;
        private bool m_IsGetCrit;

        public Entity Owner
        {
            get
            {
                return m_Owner;
            }
        }

        public void Init(Entity owner,Canvas parentCanvas,float damageValue,Color color,Vector3 point,bool isGetCrit)
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
                m_Text.text = damageValue.ToString();
                m_Text.color = color;
                m_Owner = owner;
                m_OwnerId = owner.Id;
                m_Point = point;
                m_IsGetCrit = isGetCrit;
            }
            Refresh();

            SetTextHUD();
            StartCoroutine(DamageHUDItemCo(KeepSeconds, FadeOutSeconds));
        }

        private void SetTextHUD()
        {
            int m_RandomX = Utility.Random.GetRandom(0, 01);
            int m_RandomY = Utility.Random.GetRandom(0, 00);
            m_Text.transform.localPosition =new Vector3(m_RandomX, m_RandomY, 0);
            if(m_IsGetCrit)
            {
                DOTween.To(() => m_Text.fontSize, value => m_Text.fontSize = value, 40, 1f);
            }
            else
            {
                DOTween.To(() => m_Text.fontSize, value => m_Text.fontSize = value, 20, 1f);
            }
           
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


               // Vector3 worldPosition = m_Owner.CachedTransform.position + new Vector3(0f, 3f, 0f);
                Vector3 worldPosition = m_Point;
                Vector3 screenPosition = GameEntry.Scene.MainCamera.WorldToScreenPoint(worldPosition);

                Vector2 position;
                if (RectTransformUtility.ScreenPointToLocalPointInRectangle((RectTransform)m_ParentCanvas.transform, screenPosition,
                    m_ParentCanvas.worldCamera, out position))
                {
                    m_CachedTransform.localPosition = position;
                }
            }

            return true;
        }

        public void Reset()
        {
            StopAllCoroutines();
            m_CachedCanvasGroup.alpha = 1f;
            m_Text.text = "";
            m_Text.fontSize = 15;
            m_Text.transform.localPosition = Vector3.zero;
            m_Owner = null;
            m_Point = Vector3.zero;
            m_IsGetCrit = false;
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

        private IEnumerator DamageHUDItemCo(float keepDuration, float fadeOutDuration)
        {
            
            yield return new WaitForSeconds(keepDuration);
            yield return m_CachedCanvasGroup.FadeToAlpha(0f, fadeOutDuration);
        }
    }
}
