using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace XGUI
{
    public class XClickComponent : MonoBehaviour
    {

        public bool IsEnable = false;

        public class CloseEventLua : UnityEvent { }
        private CloseEventLua m_closeFunc = new CloseEventLua();
        public CloseEventLua closeFunc { get { return m_closeFunc; } }

        List<GameObject> m_IngoreList;
        List<GameObject> ingoreList
        {
            get
            {
                if (m_IngoreList == null)
                {
                    m_IngoreList = new List<GameObject>();
                }
                return m_IngoreList;
            }
        }

        public void AddIngoreList(GameObject go)
        {
            ingoreList.Add(go);
        }

        // Update is called once per frame
        void Update()
        {
            if (!IsEnable)
                return;

            if (UnityEngine.Application.platform == RuntimePlatform.Android || UnityEngine.Application.platform == RuntimePlatform.IPhonePlayer)
            {
                if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
                {
                    GameObject go = EventSystem.current.currentSelectedGameObject;
                    if (go && go.transform.IsChildOf(transform))
                    {
                        ClickFunc();
                    }
                }
            }
            else
            {
                if (Input.GetMouseButtonDown(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended))
                {
                    ClickFunc();
                }
            }
        }

        void ClickFunc()
        {
            GameObject go = EventSystem.current.currentSelectedGameObject;
            if (go)
            {
                if( go.transform.IsChildOf(this.transform))
                {
                    return;
                }
                for (int i = 0; i < ingoreList.Count; i++)
                {
                    if (go.transform.IsChildOf(ingoreList[i].transform))
                    {
                        return;
                    }
                }
            }
            m_closeFunc.Invoke();
        }

        private void OnDestroy()
        {
            m_IngoreList.Clear();
            m_IngoreList = null;
        }
    }

}



