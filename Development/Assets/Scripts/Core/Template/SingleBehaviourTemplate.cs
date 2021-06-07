//ม๚ิพ
using UnityEngine;

public class SingleBehaviourTemplate<T> : MonoBehaviour where T :MonoBehaviour
{
   
    private static T _instance;
    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject m_obj = new GameObject(typeof(T).Name);
                _instance = m_obj.AddComponent<T>();
                (_instance as SingleBehaviourTemplate<T>).OnInitialize();
                GameObject.DontDestroyOnLoad(m_obj);

            }

            return _instance;
        }
    }


    public static void Destroy()
    {
        if(_instance != null)
        {
            GameObject.DestroyImmediate(_instance.gameObject);
            _instance = null;
        }
    }

    protected virtual void OnInitialize()
    {

    }

}
