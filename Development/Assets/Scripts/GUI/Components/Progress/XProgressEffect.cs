
using UnityEngine;
namespace XGUI
{
    public class XProgressEffect : XProgress
    {
        [SerializeField]
        public RectTransform m_content;
        [SerializeField]
        public RectTransform m_effect;//特效

        private Vector2 oriSize;

        protected void Awake()
        {
            oriSize = m_content.sizeDelta;
            this.SetValue(0);
        }

        public override void SetValue(float value)
        {
            base.SetValue(value);
            maxValue = maxValue == 0 ? 1 : maxValue;
            float rate = (this._value / maxValue);
            float dx = oriSize.x * rate;

            m_effect.anchoredPosition = new Vector2(dx, m_effect.anchoredPosition.y);

            if (rate == 0)
                m_effect.gameObject.SetActive(false);
            else
                m_effect.gameObject.SetActive(true);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            m_effect = null;
            m_content = null;
        }
    }
}