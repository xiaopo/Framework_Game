using System;
using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// 游戏对象池管理
/// </summary>
public class GopManager : MonoBehaviour
{
    //对象池key
    public static string Empty = "Empty";
    public static string FightText = "FightText";
    public static string Effect = "EffectGmObj";
    public static string RoleEffect = "RoleEffectGmObj";
    public static string HPBar = "EntityHPPool";
    public static string Avatar = "AvatarPartPool";
    public static string AvatarEffect = "AvatarEffect";
    public static string GUIAvatarScene = "GUIAvatarScene";
    public static string GUIAvatarRoot = "GUIAvatarRoot";
    public static string SkinPreview = "SkinPreview";
    public static string MirrorScence = "MirrorScence";
    public static string UIPrefabLoader = "PrefabLoader";
    private static bool s_IsOnDestroy;
    private Dictionary<string, GameObjectPool> m_AllPool = new Dictionary<string, GameObjectPool>();
    public Dictionary<string, GameObjectPool> allPool { get { return m_AllPool; } }
    public GameObjectPool Create(string poolName)
    {
        if (string.IsNullOrEmpty(poolName))
        {
            XLogger.ERROR("GopManager::Create()  poolName=null");
            return null;
        }

        if (m_AllPool.ContainsKey(poolName))
        {
            XLogger.ERROR(string.Format("GopManager::Create()  poolName = {0} already exist", poolName));
            return null;
        }

        GameObject go = new GameObject(poolName);
        go.transform.SetParent(transform);
        GameObjectPool pool = go.AddComponent<GameObjectPool>();
        m_AllPool.Add(poolName, pool);
        return pool;
    }



    public GameObjectPool Get(string poolName)
    {
        return (!string.IsNullOrEmpty(poolName) && m_AllPool.ContainsKey(poolName)) ? m_AllPool[poolName] : null;
    }

    public GameObjectPool TryGet(string poolName)
    {
        if (s_IsOnDestroy)
            return null;

        GameObjectPool pool = Get(poolName);
        if (pool == null)
            pool = Create(poolName);

        return pool;
    }

    public void ClearAll(string[] ignores)
    {
        foreach (var pool in m_AllPool)
        {
            if (ignores == null || Array.IndexOf<string>(ignores, pool.Key) == -1)
                pool.Value.UnloadAll();
        }
    }


    void OnDestroy()
    {
        s_IsOnDestroy = true;
    }


    private static GopManager m_Instance;
    public static GopManager Instance
    {
        get
        {
            if (m_Instance == null && !s_IsOnDestroy)
            {
                GameObject go = new GameObject("GopManager");
                m_Instance = go.AddComponent<GopManager>();
                DontDestroyOnLoad(go);
            }
            return m_Instance;
        }
    }
}
