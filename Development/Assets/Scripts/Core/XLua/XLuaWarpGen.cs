using System;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Events;
using System.Collections.Generic;
using UnityEngine.AI;
using XLua;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Object = UnityEngine.Object;
using UnityEngine.Rendering.Universal;

public static class XLuaWarpGen
{
    [BlackList]
    public static List<List<string>> BlackList = new List<List<string>>()
    {
        //如果某属性、方法不需要生成，加这个标签
        new List<string>(){ "XGUI.EmojiText.XInlineText", "OnRebuildRequested"},
    };


    [GCOptimize]
    public static List<Type> GCOptimizeList = new List<Type>()
    {
        //如果想对struct生成免GC代码，加这个标签
        typeof(XGUI.XScrollRect.MovementType),
    };

    #region System

    [ReflectionUse]
    public static List<Type> ReflectionUseList = new List<Type>()
    {
        //如果想在反射下使用，加这个标签
        //typeof(UnityEngine.Object),
        //typeof(Camera),
        //typeof(GameObject),
        //typeof(Transform),
        //typeof(Time),
        //typeof(Texture2D),
        //typeof(RenderTexture),
        //typeof(RenderTextureDescriptor),
        //typeof(Animation),
        //typeof(Animator),
        //typeof(Collider),
        //typeof(CapsuleCollider),
        //typeof(BoxCollider),
        //typeof(SphereCollider),
        //typeof(CharacterController),
        //typeof(NavMeshAgent),
        //typeof(NavMesh),
        //typeof(Light),
      

        //typeof(RectTransform),
        //typeof(Canvas),
        //typeof(CanvasGroup),
        //typeof(Image),

        //typeof(Screen),
        //typeof(QualitySettings),
        //typeof(SleepTimeout),
    };

    #endregion

    #region Game
    [LuaCallCSharp]
    [ReflectionUse]
    public static List<Type> LuaCallCSharp = new List<Type>()
    {
        typeof(ResolutionDefine),
        typeof(EventSystemManager),
        typeof(WaitForSeconds),
        typeof(LuaCoroutine),
        typeof(TimerManager),

        typeof(UGUIExtension),
        typeof(ImageConversion),
        typeof(UnityExtension),
        typeof(SelectedObjectHelper),
        
        typeof(SUtility),
        typeof(XDefine),
        typeof(XLogger),
        typeof(GameCameraUtiliy),
        typeof(UCamera),

        
        typeof(LocalCacheUtility),
        typeof(SFileUtility),
        typeof(XMobileUtility),
   
        typeof(AssetManagement.AssetUtility),
        typeof(AssetManagement.AssetLoaderParcel),
  
        typeof(FDeltaFPS),

      
        typeof(UnityEvent),
        typeof(Joint2D),
        typeof(HingeJoint2D),
        typeof(AnchoredJoint2D),
        typeof(Rigidbody2D),
        typeof(RenderTexture),
        typeof(Application),


    };

    #endregion

    #region CShapCallLua 函数
    [CSharpCallLua]
    [ReflectionUse]
    public static List<Type> CSharpCallLua = new List<Type>()
    {
        typeof(UnityEvent),
        typeof(UnityEventBase),
        typeof(UnityEvent<string,GameObject>),
        typeof(UnityEvent<int,int>),
        //typeof(UnityEvent<XGUI.IrregularItemRenderer, int>),
        //typeof(UnityEvent<object, int>),
        typeof(UnityEvent<string,GameObject>),
        typeof(UnityEvent<string,string>),
        
        typeof(Action),
        typeof(Action<int>),
        typeof(Action<int,int>),
        typeof(Action<int,bool>),
        typeof(Action<int,bool,bool>),
        typeof(Action<float,float>),
        typeof(Action<string>),
        typeof(Action<float>),
        typeof(Action<double>),
        typeof(Action<bool>),
        typeof(Action<object>),
        typeof(Action<string,GameObject>),
        typeof(Action<AssetManagement.AssetLoaderParcel>),
        typeof(Func<double, double, double>),
        typeof(UnityAction),
        typeof(UnityAction<int>),
        typeof(UnityAction<int,int>),
        typeof(UnityAction<bool>),
        typeof(UnityAction<float>),
        typeof(UnityAction<string>),
        typeof(UnityAction<string,bool>),
        typeof(UnityAction<object>),
        typeof(UnityAction<BaseEventData>),
        typeof(UnityAction<string,GameObject>),
        typeof(UnityAction<PointerEventData>),
        typeof(Action<string,string>),
        typeof(Action<string,GameObject>),
        typeof(Action<int,List<Vector3>>),
        typeof(ActionX<bool>),
        typeof(ActionX<int>),
        typeof(ActionX<int,List<Vector3>>),
        typeof(ActionX<int,int,List<Vector3>>),
        typeof(ActionX<int,string>),
        typeof(Action<Vector3>),
        typeof(UnityAction<Vector2>),

        typeof(UnityAction<UnityEngine.EventSystems.PointerEventData>),
        typeof(UnityEvent<int,int>),
        

        typeof(TimerManager.Tick),
        typeof(CSharpLuaInterface.Language),
        typeof(System.Collections.IEnumerator),
        typeof(System.Collections.IEnumerable),

        typeof(DG.Tweening.TweenCallback<float>),
        typeof(UnityEngine.UI.InputField.OnValidateInput),
    };

    #endregion


    #region URP
    [LuaCallCSharp]
    [ReflectionUse]
    public static List<Type> LuaCallCSharpURP = new List<Type>()
    {
       typeof(UniversalRenderPipeline),
       typeof(UniversalAdditionalCameraData)


    };
    #endregion

    #region XExternalInterface
    //外部接口
    [CSharpCallLua]
    [ReflectionUse]
    public static List<Type> XExternal_CSharpCallLua = new List<Type>()
    {



    };

    #endregion

    #region GUI
    /// <summary>
    /// xgui
    /// </summary>
    [CSharpCallLua]
    [ReflectionUse]
    public static List<Type> XGUI_CSharpCallLua = new List<Type>()
    {
        typeof(Action<XGUI.XListView.ListItemRenderer>),
        typeof(Action<XGUI.XLoader.LoaderItemRenderer>),
        typeof(Action<XGUI.XViewport.ViewportChangeEvent>),
    };

    [LuaCallCSharp]
    [ReflectionUse]
    public static List<Type> XGUI_LuaCallCSharp = new List<Type>()
    {
        typeof(LauncherGUIManager),
        typeof(LauncherGUIAlrt),
        typeof(LauncherGUIPage),
        typeof(ProfileGUIManager),
        
        typeof(XGUI.XListView),
        typeof(XGUI.XListView.ListItemRenderer),

        typeof(XGUI.XListTreeView),
        typeof(XGUI.TreeItem),
        typeof(XGUI.TreeItemInfo),

        typeof(XGUI.XView),
        typeof(XGUI.XImage),
        typeof(XGUI.XImageSequence),

        typeof(XGUI.XToggle),
        typeof(XGUI.XButton),
        typeof(XGUI.XButtonGroup),
        typeof(XGUI.XScrollRect),
        typeof(XGUI.XViewport),
        typeof(XGUI.XViewport.ViewportChangeEvent),
        typeof(XGUI.XTiledImage),
        typeof(XGUI.XTouchClick),
        typeof(XGUI.XTouchDrag),
        typeof(XGUI.XNoDrawingView),
        typeof(XGUI.XLoader),
        typeof(XGUI.XSortingOrder),
        typeof(XGUI.XTouchRotTarget),
        typeof(XGUI.XProgressImage),
        typeof(XGUI.XProgress),
        typeof(XGUI.XProgressEffect),
        typeof(XGUI.XInputField),

        typeof(XGUI.XClickComponent),//点击控件
        typeof(XGUI.XScrollRect.MovementType),//ListView拖动方式
        typeof(XGUI.XSpriteMaskOrder),//特效裁剪层级
        typeof(XGUI.XImageNode),//换图片
        typeof(XGUI.XImageGroup),//换图片

    };
    #endregion

    #region DoTween
    /// <summary>
    /// DoTween
    /// </summary>
    [LuaCallCSharp]
    [ReflectionUse]
    public static List<Type> DoTween_LuaCallCSharp = new List<Type>()
    {
        typeof(DG.Tweening.AutoPlay),
        typeof(DG.Tweening.AxisConstraint),
        typeof(DG.Tweening.Ease),
        typeof(DG.Tweening.LogBehaviour),
        typeof(DG.Tweening.LoopType),
        typeof(DG.Tweening.PathMode),
        typeof(DG.Tweening.PathType),
        typeof(DG.Tweening.RotateMode),
        typeof(DG.Tweening.ScrambleMode),
        typeof(DG.Tweening.TweenType),
        typeof(DG.Tweening.UpdateType),

        typeof(DG.Tweening.DOTween),
        typeof(DG.Tweening.DOVirtual),
        typeof(DG.Tweening.EaseFactory),
        typeof(DG.Tweening.Tweener),
        typeof(DG.Tweening.Tween),
        typeof(DG.Tweening.Sequence),
        typeof(DG.Tweening.TweenParams),
        typeof(DG.Tweening.Core.ABSSequentiable),

        typeof(DG.Tweening.Core.TweenerCore<Vector3, Vector3, DG.Tweening.Plugins.Options.VectorOptions>),

        typeof(DG.Tweening.TweenCallback),

        typeof(DG.Tweening.TweenExtensions),
        typeof(DG.Tweening.TweenSettingsExtensions),
        typeof(DG.Tweening.ShortcutExtensions),

    };
    #endregion

    #region Network
    /// <summary>
    /// Network
    /// </summary>
    [LuaCallCSharp]
    [ReflectionUse]
    public static List<Type> Network_LuaCallCSharp = new List<Type>()
    {
        

    };


    [CSharpCallLua]
    [ReflectionUse]
    public static List<Type> Network_CSharpCallLua = new List<Type>()
    {
     
    };
    #endregion

    #region XAudio
    [LuaCallCSharp]
    [ReflectionUse]
    public static List<Type> Audio_LuaCallCSharp = new List<Type>()
    {
        
    };
    #endregion

    #region 场景实体战斗
    [LuaCallCSharp]
    [ReflectionUse]
    public static List<Type> MAP_LuaCallCSharp = new List<Type>()
    {
        typeof(Game.MScene.LoadMapMode),
        typeof(Game.MScene.GameMapLoader),
        typeof(Game.MScene.SingleSceneLoader),
        typeof(Game.MScene.MapfightProgram)

    };
    #endregion

}
