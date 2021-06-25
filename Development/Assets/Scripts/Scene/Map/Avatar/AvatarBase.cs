using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.MScene
{
    public class AvatarBase 
    {
       
        public string entityName = "";
        public string m_combineID = "";
        private int m_entityType = -1;  //ʵ������ -1 ������ʵ�� 0 ����ʵ��  1���
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

