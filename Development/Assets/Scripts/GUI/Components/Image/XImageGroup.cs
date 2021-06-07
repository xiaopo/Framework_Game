using AssetManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XGUI
{
    public class XImageGroup : MonoBehaviour
    {
        [SerializeField]
        private string m_nameSuffix = string.Empty;
        public string nameSuffix
        {
            set
            {
                if (m_nameSuffix != value)
                {
                    m_nameSuffix = value;
                    ChangeImageSuffix();
                }
            }
            get
            {
                return m_nameSuffix;
            }
        }

        [SerializeField]
        private XImageGroupStructure m_Objects;

        void Awake()
        {
        }

        void ChangeImageSuffix()
        {
            if (string.IsNullOrEmpty(m_nameSuffix))
            {
                return;
            }

            for (int i = 0; i < m_Objects.unityObjects.Count; i++)
            {
                if (m_Objects.unityObjects[i].node != null && m_Objects.unityObjects[i].node.image != null)
                {
                    string spriteName = string.Format("{0}_{1}.png", m_Objects.unityObjects[i].node.imageName, m_nameSuffix);
                    if (AssetUtility.Contains(spriteName))
                    {
                        m_Objects.unityObjects[i].node.image.spriteAssetName = spriteName;
                    }
                    else
                    {
                        m_Objects.unityObjects[i].node.image.spriteAssetName = string.Format("{0}.png", m_Objects.unityObjects[i].node.imageName);
                    }
                }
            }
        }

        bool ContainNode(XImageNode node)
        {
            foreach(XImageGroupStructure.UnityObject obj in m_Objects.unityObjects)
            {
                if (obj.node != null && obj.node.GetInstanceID() == node.GetInstanceID())
                    return true;
            }
            return false;
        }

        void OnDestroy()
        {
            if (m_Objects != null && m_Objects.unityObjects != null)
            {
                m_Objects.unityObjects.Clear();
            }
        }

        public void RegisterGroup(XImageNode node)
        {
            if (!ContainNode(node))
            {
                XImageGroupStructure.UnityObject StructureObj = new XImageGroupStructure.UnityObject();
                StructureObj.target = node.gameObject;
                StructureObj.node = node;
                m_Objects.unityObjects.Add(StructureObj);
            }
        }
    }
}

