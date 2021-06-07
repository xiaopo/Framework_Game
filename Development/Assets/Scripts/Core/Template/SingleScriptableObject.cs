
using UnityEngine;

/// <summary>
/// 必须放在 Resources/Settings下，且资产名和类名相同
/// </summary>
/// <typeparam name="T"></typeparam>
public class SingleScriptableObject<T> : ScriptableObject where T : ScriptableObject
{
  
    public static ScriptableObject Load()
    {
        return Resources.Load<ScriptableObject>("Settings/"+typeof(T).Name);
    }

    private static T _instance;

    public SingleScriptableObject() : base()
    {
        _instance = this as T;
    }

    public static T Instance
    {
        get
        {
            if (_instance == null) _instance = Load() as T;

            return _instance;


        }
    }
}
