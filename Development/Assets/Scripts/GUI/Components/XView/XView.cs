using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

namespace XGUI
{
    public class XView : MonoBehaviour, XIEvent
    {
        [SerializeField]
        private UnityObjectStructure m_Objects;
        public UnityObjectStructure objects
        {
            get { return m_Objects; }
        }
        [SerializeField]
        public class ViewEvent : UnityEvent { }
        protected ViewEvent m_OnDestroy = new ViewEvent();
        public XView.ViewEvent onDestroy { get { return m_OnDestroy; } }

        private XLua.LuaTable m_InjectLuaTable;
        public virtual void Start()
        { }

        public Object Get(string name)
        {
            foreach (var item in m_Objects.unityObjects)
                if (item.name == name)
                    return item.component;
            return null;
        }

        public void InitInject(XLua.LuaTable tab)
        {
            m_InjectLuaTable = tab;
            UnityEngine.Profiling.Profiler.BeginSample("XView.InitInject");
            foreach (var item in m_Objects.unityObjects)
                tab.Set<string, Object>(item.name, item.component);
            UnityEngine.Profiling.Profiler.EndSample();
        }

        public T GetBindComponet<T>(string oname) where T:Object
        {
            foreach (var item in m_Objects.unityObjects)
            {
                if (item.name == oname) return item.component as T;
            }

            return default(T);
        }

        public virtual void ClearEvent()
        {
            if (this.m_OnDestroy != null)
            {
                this.m_OnDestroy.Invoke();
                this.m_OnDestroy.RemoveAllListeners();
                this.m_OnDestroy = null;
            }
        }

        public virtual void OnDestroy()
        {
            if (m_InjectLuaTable != null)
            {
                if(!m_InjectLuaTable.disposed)
                    m_InjectLuaTable.Dispose();

                m_InjectLuaTable = null;
            }

            ClearEvent();
            //#if UNITY_EDITOR
            if (m_Objects != null && m_Objects.unityObjects != null)
            {
                foreach (var item in m_Objects.unityObjects)
                {
                    if (item.component is XIEvent)
                        ((XIEvent)item.component).ClearEvent();
                }
                m_Objects.unityObjects.Clear();
            }
            //#endif
        }
    }
}
