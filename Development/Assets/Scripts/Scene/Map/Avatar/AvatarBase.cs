using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.MScene
{
    public class AvatarBase 
    {
       
        public string entityName = "";
        public string m_combineID = "";
        private int m_entityType = -1;  //实体类型 -1 代表不是实体 0 本机实体  1玩家
        public int entityType
        {
            get { return m_entityType; }
            set { m_entityType = value; }
        }
        public virtual void Update(float time)
        {

        }

        public virtual void SetFloat(string id ,float value)
        {

        }
        public virtual void SetTrigger(string id)
        {

        }

        public virtual void Clean()
        {

        }

        public virtual void Dispose()
        {

        }

    }
}

