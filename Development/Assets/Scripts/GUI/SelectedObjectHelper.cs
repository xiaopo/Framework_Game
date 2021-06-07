using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class SelectedObjectHelper : MonoBehaviour
{
    List<RaycastResult> m_RaycastResult = new List<RaycastResult>();
    void Update()
    {
        if (Input.GetMouseButtonDown(1) && Input.GetKey(KeyCode.LeftControl))
        {
            
            PointerEventData data = new PointerEventData(EventSystem.current);
            data.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            EventSystem.current.RaycastAll(data, m_RaycastResult);
            if (m_RaycastResult.Count > 0)
            {
#if UNITY_EDITOR
                UnityEditor.EditorGUIUtility.PingObject(m_RaycastResult[0].gameObject);
#endif
            }
        }
        
    }
}
