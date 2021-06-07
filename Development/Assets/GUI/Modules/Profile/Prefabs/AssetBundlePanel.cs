using AssetManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AssetBundlePanel : MonoBehaviour
{

    public Transform _content;

    public GameObject _teamplate;

    private List<AssetBundleItem> shows;
    private List<RawAsset> raws;
    private List<BundleInfo> bundls;
    public int panel_type = 1;
    private void Start()
    {
        _teamplate.SetActive(false);

        shows = new List<AssetBundleItem>();
        raws = new List<RawAsset>();
        bundls = new List<BundleInfo>();
    }

    private void Update()
    {
       if(Time.frameCount % 100 == 0)
        {
            if(panel_type == 1)
                DrawGUI_1();
            else
                DrawGUI_2();
        }
    }
    private AssetBundleItem GetItem()
    {
        GameObject item = GameObject.Instantiate(_teamplate);
        item.transform.SetParent(this._content);
        return item.GetComponent<AssetBundleItem>();
    }

    private void DrawGUI_1()
    {
        foreach (var item in shows)
        {
            item.Hide();
        }

        AssetbundleCache.GetAllBundles(bundls);

        int showCount = shows.Count;
        for (int i = 0; i < bundls.Count; i++)
        {
            AssetBundleItem item = null;
            if (i < showCount)
                item = shows[i];
            else
            {
                item = GetItem();
                shows.Add(item);
            }

            item.Show();

            item.text_abname.text = bundls[i].path;
            item.Text_ABRefrence.text = bundls[i].referenceCount.ToString();

        }
    }

    private void DrawGUI_2()
    {
        foreach(var item in shows)
        {
            item.Hide();
        }

        AssetRawobjCache.GetRawAssets(raws);
        int showCount = shows.Count;
        for(int i = 0;i< raws.Count;i++)
        {
            AssetBundleItem item = null;
            if (i < showCount)
                item = shows[i];
            else
            {
                item = GetItem();
                shows.Add(item);
            }

            item.Show();

            item.text_abname.text = raws[i].abPath;
            item.Text_rawname.text = raws[i].name;
            item.Text_ABRefrence.text = raws[i].assetBundleReferenceCount.ToString();
            item.Text_RawRefrence.text = raws[i].referenceCount.ToString();
            item.Text_Instantiate.text = raws[i].instanceCount.ToString();

        }

        raws.Clear();
    }
}
