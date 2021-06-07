using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AssetBundleItem : MonoBehaviour
{
    // Start is called before the first frame update
    public Text text_abname;
    public Text Text_rawname;
    public Text Text_ABRefrence;
    public Text Text_RawRefrence;
    public Text Text_Instantiate;

    private RectTransform _rtarnsform;

    private void Start()
    {
        _rtarnsform = this.GetComponent<RectTransform>();
    }

    public void Hide()
    {
        this.gameObject.SetActive(false);
        this._rtarnsform.anchoredPosition = Vector2.zero;
    }
   
    public void Show()
    {
        this.gameObject.SetActive(true);
    }
}
