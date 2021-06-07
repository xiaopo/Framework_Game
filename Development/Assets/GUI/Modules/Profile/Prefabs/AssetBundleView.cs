using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AssetBundleView : MonoBehaviour
{
    public AssetBundlePanel panel_1;
    public AssetBundlePanel panel_2;

    public Button button_1;
    public Button button_2;
    public Button button_close;

    void Start()
    {
        panel_1.panel_type = 1;
        panel_2.panel_type = 2;
        button_1.onClick.AddListener(Button1Click);
        button_2.onClick.AddListener(Button2Click);
        button_close.onClick.AddListener(OnClose);
    }

    void OnClose()
    {
        this.gameObject.SetActive(false);
    }

    void Button1Click()
    {
        panel_1.gameObject.SetActive(true);
        panel_2.gameObject.SetActive(false);
    }

    void Button2Click()
    {
        panel_1.gameObject.SetActive(false);
        panel_2.gameObject.SetActive(true);
    }


    
}
