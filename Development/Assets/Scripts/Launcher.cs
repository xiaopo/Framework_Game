//龙跃
using AssetManagement;
using TPL;
using UnityEngine;
using UnityEngine.Events;

public partial class Launcher : MonoBehaviour
{

    //包模式--全部使用AB包资源
    public static bool assetBundleMode;
    /// <summary>
    /// 使用编辑器上的lua代码
    /// </summary>
    public static bool UseEditorLua;

    public static string signalName = "Launcher";


    void Start()
    {
        GameDebug.Log("Launcher.Start");
#if UNITY_EDITOR
        Resources.UnloadUnusedAssets();
#endif
        DontDestroyOnLoad(gameObject);

#if UNITY_EDITOR
        assetBundleMode = UnityEditor.EditorPrefs.GetBool("QuickMenuKey_LaunchGameAssetBundle", false);
        UseEditorLua = UnityEditor.EditorPrefs.GetBool("QuickMenuKey_LaunchLuaEditor", false);
#else
        assetBundleMode = true;
        UseEditorLua = false;
#endif
        //打开Load界面
        LauncherGUIPage UpdatePage  = LauncherGUIManager.Instance.UpdatePage;
        UpdatePage.Open();
        UpdatePage.RunProgress(100, 100,1);
        UpdatePage.Mesage = "开始检查版本..";
        SignalActionTool.Single(2.0f, Launcher.signalName);

        //初始化事件系统
        EventSystemManager.Instance.Init();

        //后台请求
        GameDebug.Log("1.玩家验证逻辑");
        TPLManager.Instance.Start(TPLCheckDone); 

    }

    //后台检测完毕
    private void TPLCheckDone()
    {

        //启动资源管理器，热更新，资源初始化等
        AssetProgram.Instance.OnInitialize(EnterGame);

    }


    void Update()
    {
        float time = Time.time;

        //信号
        SignalActionTool.Update(time);

        //SDK Http 后台逻辑
        TPLManager.Instance.Update();

        //游戏逻辑

        //下载逻辑落后执行,确保自动卸载逻辑,不会卸载未执行的下载任务所需的资源
        AssetProgram.Instance.OnUpdate();



    }

    //进入游戏
    void EnterGame()
    {
        GameDebug.Log("5.正在进入游戏...");
        LauncherGUIManager.Instance.UpdatePage.Mesage = "即将进入游戏";

        //start 其他启动


        //end 其他启动

        //--------------启动Lua-------------------


        LuaLauncher.Instance.EnterGame();


    }

    private void OnDestroy()
    {
#if UNITY_EDITOR
        Game.MScene.GameMapLoader.DisposeAction();
#endif
    }
}
