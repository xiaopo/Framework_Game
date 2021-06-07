using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class AssetsRecord
{
    public static AssetsRecord s_CurrentRecord;
    public List<string> p_Assets;
    public void Record(string assetName)
    {
        if (p_Assets == null) p_Assets = new List<string>();
        if (p_Assets.Contains(assetName)) return;
        p_Assets.Insert(0, assetName);
    }
}
