using AssetManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
///  调试UI系统
/// </summary>
public class ProfileGUIManager : SingleBehaviourTemplate<ProfileGUIManager>
{
 
    private RectTransform _rectTransform;

    protected override void OnInitialize()
    {

        Canvas canvas = this.gameObject.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;

        CanvasScaler scaler = this.gameObject.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(ResolutionDefine.resolution_width, ResolutionDefine.resolution_hight);
        scaler.matchWidthOrHeight = 0;

        this.gameObject.AddComponent<GraphicRaycaster>();

        _rectTransform = this.gameObject.GetComponent<RectTransform>();
        _rectTransform.anchoredPosition = new Vector2(0, 0);

    }

    private GameObject _assetBundleView;
    //显示AssetBundle调试面板
    public void ShowAssetBundleView()
    {
        if(_assetBundleView != null)
        {
            _assetBundleView.SetActive(true);
            return;
        }

        StartCoroutine(LoadBundleViewPrefab("AssetBundleView.prefab"));
    }


    private IEnumerator LoadBundleViewPrefab(string prefabName)
    {
        AssetLoaderParcel loader = AssetsGetManger.Instance.LoadAsset(prefabName, typeof(GameObject));

        yield return loader;

        _assetBundleView = loader.Instantiate<GameObject>(_rectTransform);
        RectTransform rect = _assetBundleView.GetComponent<RectTransform>();
        rect.anchoredPosition = Vector2.zero;
        rect.localScale = Vector3.one;

    }

    private void Update()
    {
        
    }
}
