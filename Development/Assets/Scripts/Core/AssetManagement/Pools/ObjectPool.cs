using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
public class ObjectPoolStatic
{
    public static List<PoolInterface> s_AllObjectPool = new List<PoolInterface>();
}

public interface PoolInterface
{
    int countActive { get; }
    int countInactive { get; }
    System.Type objectType { get; }
    ICollection collection { get; }

}

public class ObjectPool<T> : PoolInterface where T : new()
{
    private readonly Stack<T> m_Stack = new Stack<T>();
    private readonly Action<T> m_ActionOnGet;
    private readonly Action<T> m_ActionOnRelease;

    public int countAll { get; private set; }
    public int totalCount { get { return countAll; } }
    public int countActive { get { return countAll - countInactive; } }
    public int countInactive { get { return m_Stack.Count; } }
    public System.Type objectType { get { return typeof(T); } }
    public ICollection collection { get { return m_Stack; } }

    public ObjectPool()
        : this(null, null)
    { }
    public ObjectPool(Action<T> actionOnGet, Action<T> actionOnRelease)
    {
        if (actionOnGet != null) m_ActionOnGet = actionOnGet;
        if (actionOnRelease != null) m_ActionOnRelease = actionOnRelease;

        ObjectPoolStatic.s_AllObjectPool.Add(this);
    }

    public T Get()
    {
        T element;
        if (m_Stack.Count == 0)
        {
            element = new T();
            countAll++;
        }
        else
        {
            element = m_Stack.Pop();
        }
        if (m_ActionOnGet != null)
            m_ActionOnGet(element);
        return element;
    }

    public void Release(T element)
    {
        if (m_Stack.Count > 0 && ReferenceEquals(m_Stack.Peek(), element))
            Debug.LogError("Internal error. Trying to destroy object that is already released to pool.");
        if (m_ActionOnRelease != null)
            m_ActionOnRelease(element);
        m_Stack.Push(element);
    }

    public void ClearAll()
    {
        m_Stack.Clear();
    }
}

public static class ListPool<T>
{
    private static readonly ObjectPool<List<T>> s_ListPool = new ObjectPool<List<T>>(null, Clear);
    static void Clear(List<T> l) { l.Clear(); }
    public static List<T> Get()
    {
        return s_ListPool.Get();
    }

    public static void Release(List<T> toRelease)
    {
        s_ListPool.Release(toRelease);
    }
}


public static class DictionaryPool<K, V>
{
    private static readonly ObjectPool<Dictionary<K, V>> s_DicPool = new ObjectPool<Dictionary<K, V>>(null, Clear);
    static void Clear(Dictionary<K, V> l) { l.Clear(); }

    public static Dictionary<K, V> Get()
    {
        return s_DicPool.Get();
    }

    public static void Release(Dictionary<K, V> toRelease)
    {
        s_DicPool.Release(toRelease);
    }
}

public static class Pool<T> where T : new()
{
    private static readonly ObjectPool<T> s_ObjectPool = new ObjectPool<T>(null, null);
    public static T Get()
    {
        return s_ObjectPool.Get();
    }

    public static void Release(T element)
    {
        s_ObjectPool.Release(element);
    }

}