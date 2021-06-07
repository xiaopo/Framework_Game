using System;
using System.Collections.Generic;

public class DictionaryExt<TKey, TValue> : Dictionary<TKey, TValue>
{
    public List<TKey> mList;

    public new TValue this[TKey tkey]
    {
        get
        {
            return base[tkey];
        }
        set
        {
            if (this.ContainsKey(tkey))
            {
                base[tkey] = value;
            }
            else
            {
                this.Add(tkey, value);
            }
        }
    }

    public DictionaryExt()
    {
        this.mList = new List<TKey>();
    }

    public DictionaryExt(int capacity)
        : base(capacity)
    {
        this.mList = new List<TKey>(capacity);
    }

    public DictionaryExt(IEqualityComparer<TKey> comparer)
        : base(comparer)
    {
    }

    public new void Add(TKey tkey, TValue tvalue)
    {
        this.mList.Add(tkey);
        base.Add(tkey, tvalue);
    }

    public new bool Remove(TKey tkey)
    {
        this.mList.Remove(tkey);
        return base.Remove(tkey);
    }

    public bool RemoveAt(int index)
    {
        if (index >= 0 && index < this.mList.Count)
        {
            TKey key = this.GetKey(index);
            if (key != null && this.mList.Remove(key))
            {
                return base.Remove(key);
            }
        }
        return false;
    }

    public void Sort()
    {
        this.mList.Sort();
    }

    public void Sort(Comparison<TKey> comparison)
    {
        this.mList.Sort(comparison);
    }

    public void Sort(IComparer<TKey> comparer)
    {
        this.mList.Sort(comparer);
    }

    public void Sort(int index, int count, IComparer<TKey> comparer)
    {
        this.mList.Sort(index, count, comparer);
    }

    public bool GetTryValue(int index, out TValue value)
    {
        if (index < 0 || index >= this.mList.Count)
        {
            value = default(TValue);
            return false;
        }
        TKey tKey = default(TKey);
        this.GetTryKey(index, out tKey);
        if (tKey == null)
        {
            value = default(TValue);
            return false;
        }
        value = this[tKey];
        return true;
    }

    public bool GetTryKey(int index, out TKey value)
    {
        if (index >= 0 && index < this.mList.Count)
        {
            value = this.mList[index];
            return true;
        }
        value = default(TKey);
        return false;
    }

    public TValue GetValue(int index)
    {
        return base[this.mList[index]];
    }

    public TValue GetValueSafe(int index)
    {
        if (this.mList != null && this.mList[index] != null && base.ContainsKey(this.mList[index]))
        {
            return base[this.mList[index]];
        }
        return default(TValue);
    }

    public TKey GetKey(int index)
    {
        return this.mList[index];
    }

    public new void Clear()
    {
        this.mList.Clear();
        base.Clear();
    }
}
