using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class ShakeCamera : MonoBehaviour
 {
    public enum ShakeOrient
    {
        horizontal = 1,     //水平
        vertical,       //垂直
        forward,        //正朝向
    }

    //周期
    public float mPeriod = 2;

    //偏移周期
    public float mOffPeriod = 0;

    public ShakeOrient mShakeOrient = ShakeOrient.horizontal;
   
    //振动时间
    public float mShakeTime = 10.0f;

    //最大波峰
    public float mMaxWave = 5;               

    //最小波峰
    public float mMinWave  = 1;

    //总共经过时间
    private float mCurTime = 0;

    //是否shake状态
    public bool mIsShake = false;

    //初始位置
    public Vector3 mDefaultPos;
    
    //振动方向
    public Vector3 mShakeDir;    
    public Transform mCamerTrans;

    private UnityAction OnFinish;
    void Start()
    {
        //mCamerTrans = gameObject.transform;
    }


    //获取Transform
    public Transform GetTransform()
    {
        if (mCamerTrans == null)
        {
            mCamerTrans = gameObject.GetComponent<Transform>();
        }
        return mCamerTrans;
    }    

    //振屏
    public void ShakeScreen(int stype, float period, float shakeTime, float maxWave, float minWave, float offPeriod = 0, UnityAction finish = null)
    {
        ShakeOrient shakeOrient = (ShakeOrient)stype;
        //不在振动状态        
        if (!mIsShake)
        {

            //确保Transform有效
            if (GetTransform() == null)
                return;

            this.OnFinish = finish;
            mShakeOrient = shakeOrient;                        
            mPeriod = period;
            mShakeTime = shakeTime;
            mMaxWave = maxWave;
            mMinWave = minWave;
            mOffPeriod = offPeriod;

            //保存默认位置
            mDefaultPos = transform.localPosition;

            //垂直方向 
            if (shakeOrient == ShakeOrient.vertical)
            {
                mShakeDir = new Vector3(0, 1, 0);
            }
            else if (shakeOrient == ShakeOrient.forward)
            {
                mShakeDir = mCamerTrans.forward;
            }
            else if (shakeOrient == ShakeOrient.horizontal)
            {
                Vector3 v1 = new Vector3(0, 1, 0);
                Vector3 v2 = mCamerTrans.forward;

                mShakeDir = Vector3.Cross(v1, v2);
                mShakeDir.Normalize();
            }
            
            mIsShake = true;
        }
    }

    private void OnDestroy()
    {
        OnFinish = null;
    }

    public void OnDisable()
    {
        OnFinish = null;
        mIsShake = false;
        mCurTime = 0;
    }

    
    public void LateUpdate()
    {      
        if (mIsShake)
        {
          
            float factor = mCurTime / mShakeTime;
            //总周期
            float totalPeriod = mPeriod * Mathf.PI;

            //当前时刻值
            float maxValue = mMaxWave - (mMaxWave - mMinWave) * factor;

            //当前弧度值
            float radValue = mOffPeriod * Mathf.PI + factor * totalPeriod;
            float value = maxValue * Mathf.Sin(radValue);

            //垂直振动，只固定y方向
            if (mShakeOrient == ShakeOrient.vertical)
                mCamerTrans.localPosition = new Vector3(mCamerTrans.localPosition.x, mDefaultPos.y, mCamerTrans.localPosition.z) + mShakeDir * value;
            else
                mCamerTrans.localPosition = mDefaultPos + mShakeDir * value;
            

            mCurTime += Time.deltaTime;
            //结束振屏状态上
            if (mCurTime > mShakeTime)
            {
              
                mIsShake = false;
                mCurTime = 0;

                if(OnFinish != null)
                {
                    OnFinish.Invoke();
                    OnFinish = null;
                }
            }
        }
    }
}