
using UnityEngine;


namespace Game.MScene
{
    
    public abstract class MPObject
    {
        protected Transform mtransfrom;
        protected GameObject gameObject;

        public float GetPosition(out float y, out float z)
        {
            Vector3 pos = mtransfrom.position;

            y = pos.y;
            z = pos.z;
            return pos.x;
        }

        private Vector3 point1 = Vector3.zero;
        public void SetPosition(float x, float y, float z, bool isLocal = false)
        {
            point1.x = x; point1.y = y; point1.z = z;
            if (isLocal)
                mtransfrom.localPosition = point1;
            else
                mtransfrom.position = point1;
        }

        public float GetRotation(out float y, out float z, out float w)
        {
            Quaternion pos = mtransfrom.rotation;
            y = pos.y;
            z = pos.z;
            w = pos.w;

            return pos.x;
        }

        public float EulerAnglesY()
        {
            Quaternion pos = mtransfrom.rotation;

            return pos.eulerAngles.y;
        }

        public float GetForward(out float y, out float z)
        {
            y = mtransfrom.forward.y;
            z = mtransfrom.forward.z;

            return mtransfrom.forward.x;
        }

        public void SetRotationV(float x, float y, float z,bool isLocal = false)
        {
            Quaternion quaA = Quaternion.Euler(x, y, z);
            if (isLocal)
                mtransfrom.localRotation = quaA;
            else
                mtransfrom.rotation = quaA;
        }

        private Quaternion quaA = Quaternion.identity;
        public void SetRotation(float x, float y, float z, float w, bool isLocal = false)
        {
            quaA.x = x; quaA.y = y; quaA.z = z; quaA.w = w;
            if (isLocal)
                mtransfrom.localRotation = quaA;
            else
                mtransfrom.rotation = quaA;
        }

        public void SlerpRotation(float x, float y, float z, float w, float speed)
        {
            quaA.x = x; quaA.y = y; quaA.z = z; quaA.w = w;
            mtransfrom.rotation = Quaternion.Slerp(mtransfrom.rotation, quaA, MapftTime.deltaTime * speed);
        }


        public void LookAt(float x, float y, float z, bool isLocal = true)
        {
            if (isLocal) y = mtransfrom.position.y;//自己的y
            point1.x = x; point1.y = y; point1.z = z;
            mtransfrom.LookAt(point1);

        }

        public void LookAtDir(float x, float y, float z)
        {
            point1.x = x; point1.y = y; point1.z = z;
            mtransfrom.LookAt(mtransfrom.position + point1);
        }

        public float TransformPoint(float x,float y,float z,out float wy,out float wz)
        {
            Vector3 v3 =  mtransfrom.TransformPoint(new Vector3(x, y, z));

            wy = v3.y;
            wz = v3.z;

            return v3.x;
        }
    
        public void LookAtForward()
        {
            mtransfrom.LookAt(mtransfrom.forward);
        }

        public virtual void SetScale(float x,float y,float z)
        {
            Vector3 p = Vector3.zero;
            p.x = x; p.y = y; p.z = z;
            mtransfrom.localScale = p;
        }
       
        public void OffSetPos(float x,float y,float z)
        {
            Vector3 p = Vector3.zero;
            p.x = x; p.y = y; p.z = z;
            mtransfrom.position = mtransfrom.position + p;

        }

        protected abstract void Update(float deltaTime, float time);

        protected int updateNum = 0;
        protected void RemoveFrame()
        {
            if(updateNum != 0)
            {
                MapftFrameUtil.instance.Remove(updateNum);
                updateNum = 0;
            }
           
        }
        //进入Lua 对象池前调用
        virtual public void Dead()
        {
            RemoveFrame();
        }

        virtual public void Live(bool lanuchUpdate = true)
        {
            if (updateNum == 0 && lanuchUpdate) updateNum = MapftFrameUtil.instance.AddEvent(Update, false);
        }


        virtual public void Dispose()
        {
            RemoveFrame();

        }

    }
}
