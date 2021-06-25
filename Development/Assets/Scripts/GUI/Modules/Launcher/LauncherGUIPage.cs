
using AssetManagement;
using UnityEngine;
using UnityEngine.UI;
using XGUI;

public class LauncherGUIPage : LauncherUIBase
{
    private Text m_text_msg;
    private Text m_text_title;
    private XProgressEffect m_progress;

    public LauncherGUIPage(LauncherGUIManager manager) : base(manager) 
    {
        resourceName = "Update_Page/res_update_page";
    }

    protected override void Init()
    {

        m_text_msg = this.GetBindComponet<Text>("Text_msg");
        m_text_msg.text = "Å¬Á¦Æô¶¯ÖÐ...";

        m_text_title = this.GetBindComponet<Text>("Text_Title");

        m_progress = this.GetBindComponet<XProgressEffect>("ProgressBar");


       
    }
    string formatstr = "<color=#{0}>{1}</color>";
    string formatstr2 = "<color=#{0}>{1}</color><color=#{2}>->{3}</color>";
    string color_green = "45e57a";//ÂÌ
    string color_white = "FFFFFF";
    public void DrawVersion()
    {
        
        XVersionFile fileL = UpdateManager.Instance.LXVersionFile;
        XVersionFile fileW = UpdateManager.Instance.WXVersionFile;

        string luastr;
        string devstr;
        string artstr;

      
        if (fileL == null)
        {
            luastr = string.Format(formatstr, color_green, fileW.p_LuaVersion.svnVer);
            devstr = string.Format(formatstr, color_green, fileW.p_DevVersion.svnVer);
            artstr = string.Format(formatstr, color_green, fileW.p_ArtVersion.svnVer);
        }
        else
        {

            luastr = GetVersionCompareStr(fileL.p_LuaVersion.svnVer, fileW.p_LuaVersion.svnVer);
            devstr = GetVersionCompareStr(fileL.p_DevVersion.svnVer, fileW.p_DevVersion.svnVer);
            artstr = GetVersionCompareStr(fileL.p_ArtVersion.svnVer, fileW.p_ArtVersion.svnVer);
        }

        this.GetBindComponet<Text>("Text_lua").text = "Lua: " + luastr; 
        this.GetBindComponet<Text>("Text_dev").text = "Dev£º" + devstr;
        this.GetBindComponet<Text>("Text_art").text = "Art£º" + artstr;
    }

    protected string GetVersionCompareStr(string ver1,string ver2)
    {
        string outStr;
        if (ver1 != ver2)
            outStr = string.Format(formatstr2, color_white, ver1, color_green, ver2);
        else
            outStr = string.Format(formatstr, color_white, ver1);

        return outStr;
    }

    public string Mesage { set { m_text_msg.text = value; } get { return m_text_msg.text; } }

    public void Open()
    {
        this.gameObject.SetActive(true);
    }


    public void RunProgress(float value, float maxValue,float duration= 0.2f)
    {
        m_progress.maxValue = maxValue;
        m_progress.RunProgress(value, duration);
    }

}
