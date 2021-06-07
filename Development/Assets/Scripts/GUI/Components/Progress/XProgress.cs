using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.Events;

namespace XGUI
{
    public enum Direction
    {
        HORIZONTAL,
        VERTICAL,
    }

    public enum ProgressLabStyle
    {
        Normal,
        Value
    }

    public class XProgress : MonoBehaviour
    {
        public float maxValue = 0;
        public float minValue = 0;

        //仅供面板测试用
        [Range(0, 1)]
        [SerializeField]private float viewValue = 0;

        public float _value = 0;
        
        public Text label;
        public Image processImg;

        public Ease EaseType;
        private bool _isStartProgress = false;
        private float _arrivePercentage = 0;
        private float _startProgressStep = 0;
        public UnityAction onProgress;
        public UnityAction onProgressEnd;
        public ProgressLabStyle style = ProgressLabStyle.Normal;
        public Direction direction = Direction.HORIZONTAL;
        Tween m_Tween;

        RectTransform m_Rect;
        RectTransform Rect
        {
            get
            {
                if (m_Rect == null)
                {
                    m_Rect = GetComponent<RectTransform>();
                }
                return m_Rect;
            }
        }

        [SerializeField]
        private RectTransform m_MaskRect;
        RectTransform MaskRect
        {
            get
            {
                return m_MaskRect;
            }
            set
            {
                m_MaskRect = value;
            }
        }

        virtual protected void Start()
        {
              
        }

        public void StopAnim()
        {
            if(m_Tween != null)
            {
                m_Tween.Kill();
            }
        }

        virtual public void SetValue(float value)
        {
            this._value = Mathf.Min(maxValue, Mathf.Max(minValue, value));

            if (MaskRect != null)
            {
                if (direction == Direction.HORIZONTAL)
                {
                    MaskRect.sizeDelta = new Vector2(Rect.rect.width * GetPercentage(),Rect.rect.height);
                }
                else if (direction == Direction.VERTICAL)
                {
                    MaskRect.sizeDelta = new Vector2(Rect.rect.width,Rect.rect.height * GetPercentage());
                }
                
                MaskRect.gameObject.SetActive(_value != minValue);
            }

            if (processImg != null)
                processImg.fillAmount = GetPercentage();
            
            if (label)
            {
                if (style == ProgressLabStyle.Normal)
                {
                    float rate = 0;
                    if (maxValue != 0) rate = (this._value / maxValue * 100);

                    label.text = string.Format("{0:P}", rate);
                }
                else if (style == ProgressLabStyle.Value)
                {
                    label.text = string.Format("{0}/{1}", this._value, maxValue);
                }
            }
        }

        public float GetValue()
        {
            return _value;
        }

        public float GetPercentage()
        {
            float divisor = maxValue - minValue;
            if (divisor == 0) return 0;
            return (_value - minValue) / divisor;

        }

        public void StartProgress(float value, float step)
        {
            if (value <= minValue && value >= maxValue)
            {
                return;
            }

            _arrivePercentage = (value - minValue) / (maxValue - minValue);
            _startProgressStep = step;
            _isStartProgress = true;
        }

        //队列播放
        private List<float> waitList = new List<float>();
        private float tarValue = -1;
        public void RunProgressQueue(float value, float duration)
        {
            if (tarValue != -1 && tarValue != _value)
            {
                //等待
                waitList.Add(value);
                waitList.Add(duration);
                return;
            }

            RunProgress(value, duration);
        }


        public void RunProgress(float value, float duration)
        {

            if (value <= minValue && value >= maxValue)
            {
                return;
            }

            StopAnim();

            //if(value < _value) _value = 0;

            tarValue = value;

            m_Tween = DOTween.To(() =>
            {
                return this._value;
            }, (v) =>
            {
                _value = Mathf.Floor(v);
                this.SetValue(_value);

                if (onProgress != null)
                    onProgress.Invoke();
            }, value, duration);

            m_Tween.SetEase(EaseType);
            m_Tween.OnComplete(() =>
            {
                if (onProgressEnd != null)
                    onProgressEnd.Invoke();

                TweenFinish();
            });
           
        }
        private void TweenFinish()
        {
            if (waitList.Count > 1)
            {
                float value = waitList[0];
                float duration = waitList[1];

                waitList.RemoveAt(0);
                waitList.RemoveAt(0);

                RunProgress(value, duration);

                return;
            }

            waitList.Clear();
        }

        void Update()
        {
            if (_isStartProgress)
            {
                _value = _value + _startProgressStep;
                float perc = GetPercentage();

                if (MaskRect != null)
                {
                    MaskRect.sizeDelta = new Vector2(Rect.rect.width * perc,Rect.rect.height);
                }

                if (label)
                {
                    if (style == ProgressLabStyle.Normal)
                    {
                        label.text = (perc * 100).ToString("0.0") + "%";
                    }
                    else if (style == ProgressLabStyle.Value)
                    {
                        label.text = string.Format("{0}/{1}", this._value, maxValue);
                    }
                }

                if (perc < _arrivePercentage)
                {

                    if (onProgress != null)
                        onProgress.Invoke();
                }
                else
                {

                    if (onProgressEnd != null)
                        onProgressEnd.Invoke();
                    _isStartProgress = false;
                }
            }
        }

        virtual protected void OnDestroy()
        {
            onProgress = null;
            onProgressEnd = null;
            m_Tween = null;
        }
    }
}
