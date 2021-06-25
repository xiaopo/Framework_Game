

using UnityEngine;

namespace Game.MScene
{
    public class EntityBase: MPObject
    {
        protected string m_combineID;
        protected int entityType = -1;
        protected GameObjectPool m_pool_empty;
        protected Transform head_temp;

        //模型
        protected AvatarShell m_avatarAgent;
        public EntityBase(string name)
        {
            m_pool_empty = GopManager.Instance.TryGet(GopManager.Empty);
            //创建或获得一个空的gameObject
            gameObject = m_pool_empty.Get("EntityEmpty");
            gameObject.SetActive(true);
            gameObject.name = name;
            mtransfrom = gameObject.GetComponent<Transform>();
            mtransfrom.SetParentOEx(MapfightProgram.Instance.EntityParent);

            head_temp = m_pool_empty.Get("Empty").transform;
            head_temp.gameObject.SetActive(true);
            head_temp.name = "Head_temp";
            head_temp.transform.SetParentOEx(mtransfrom);
            head_temp.transform.LocalPositionEx(0, 3.5f, 0);
        }

        public virtual void InitByLua(string sname, int eType, string combineid)
        {
            gameObject.name = sname;
            entityType = eType;
            m_combineID = combineid;


        }

        override protected void Update(float deltaTime, float time)
        {

        }

        //进入Lua 对象池前调用
        override public void Dead()
        {
            gameObject.SetActive(false);
            mtransfrom.SetParentOEx(MapfightProgram.Instance.EntityDeathParent);

            base.Dead();

        }

        override public void Live(bool lanuchUpdate = true)
        {
            gameObject.SetActive(true);
            mtransfrom.SetParentOEx(MapfightProgram.Instance.EntityParent);

            base.Live(lanuchUpdate);
        }


        //lua  调用reset
        virtual public void LuaReset()
        {

        }

        //进入C# 对象池前调用
        virtual public void CSClean()
        {

            this.Dispose();


            m_pool_empty.Release(gameObject, "EntityEmpty");
            m_pool_empty.Release(head_temp.gameObject, GopManager.Empty);
     
            gameObject = null;
            mtransfrom = null;


        }

        override public void Dispose()
        {
            base.Dispose();

  
        }
    }
}
